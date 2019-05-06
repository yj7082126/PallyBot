// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.BotBuilderSamples;
using System;

namespace PallyBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        //private BotState _conversationState;
        //private BotState _userState;
        public QnAMaker EchoBotQnA { get; private set; }

        //public EchoBot(ConversationState conversationState, UserState userState,  QnAMakerEndpoint endpoint) {
        //    _conversationState = conversationState;
        //    _userState = userState;
        //    EchoBotQnA = new QnAMaker(endpoint);
        //}

        public EchoBot(QnAMakerEndpoint endpoint)
        {
            EchoBotQnA = new QnAMaker(endpoint);
        }

        //public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken)) {
        //    await base.OnTurnAsync(turnContext, cancellationToken);

        //    // Save any state changes that might have occured during the turn.
        //    await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        //    await _userState.SaveChangesAsync(turnContext, false, cancellationToken);

        //}

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // Get state properties from turn context
            //var conversationStateAccessors = _conversationState.CreateProperty<ConversationData>(nameof(ConversationData));
            //var conversationData = await conversationStateAccessors.GetAsync(turnContext, () => new ConversationData());

            //var userStateAccessors = _userState.CreateProperty<UserProfile>(nameof(UserProfile));
            //var userProfile = await userStateAccessors.GetAsync(turnContext, () => new UserProfile());

            //// If no name stored on turn context:
            //if (string.IsNullOrEmpty(userProfile.Name))
            //{
            //    // If chat never prompted user for name, prompt it
            //    if (conversationData.PromptedUserForName)
            //    {
            //        // Set name to what the user has provided
            //        userProfile.Name = turnContext.Activity.Text?.Trim();
            //        // Acknowledge that we got their name.
            //        await turnContext.SendActivityAsync($"Thanks {userProfile.Name}. To see conversation data, type anything.");
            //        // Reset flag to allow bot to go through the cycle again
            //        conversationData.PromptedUserForName = false;
            //    }
            //    else
            //    {
            //        // Prompt user for their name
            //        await turnContext.SendActivityAsync($"What is your name?");
            //        // Set flag to true
            //        conversationData.PromptedUserForName = true;
            //    }
            //}
            //else {
            //    var messageTimeOffset = (DateTimeOffset)turnContext.Activity.Timestamp;
            //    var localMessageTime = messageTimeOffset.ToLocalTime();
            //    conversationData.Timestamp = localMessageTime.ToString();
            //    conversationData.ChannelId = turnContext.Activity.ChannelId.ToString();

            //    await turnContext.SendActivityAsync($"{userProfile.Name} sent: {turnContext.Activity.Text}");
            //    await turnContext.SendActivityAsync($"Message received at: {conversationData.Timestamp}");
            //    await turnContext.SendActivityAsync($"Message received from: {conversationData.ChannelId}");
            //}
            // First send the user input to your QnA Maker knowledgebase
            await AccessQnAMaker(turnContext, cancellationToken);
            //await turnContext.SendActivityAsync(MessageFactory.Text($"Echo: {turnContext.Activity.Text}"), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and Welcome!"), cancellationToken);
                }
            }
        }

        private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);
            if (results.Any())
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("QnA Maker Returned: " + results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Sorry, could not find an answer in the Q and A system."), cancellationToken);
            }
        }

    }
}
