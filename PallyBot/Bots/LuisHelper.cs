

using Microsoft.Bot.Builder;
using Microsoft.BotBuilderSamples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Bot.Builder.AI.Luis;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

public static class LuisHelper {
    public static async Task<BookingDetails> ExecuteLuisQuery(IConfiguration configuration, ILogger logger, ITurnContext turnContext, CancellationToken cancellationToken) {
        var bookingDetails = new BookingDetails();
        try
        {
            var luisApplication = new LuisApplication(configuration["LuisAppId"],
                configuration["LuisAPIKey"],
                "https://" + configuration["LuisAPIHostName"]);

            var recognizer = new LuisRecognizer(luisApplication);

            var recognizerResult = await recognizer.RecognizeAsync(turnContext, cancellationToken);

            var (intent, score) = recognizerResult.GetTopScoringIntent();

            if (intent == "Book_flight") {
                bookingDetails.Destination = recognizerResult.Entities["To"]?.FirstOrDefault()?["Airport"]?.FirstOrDefault()?.FirstOrDefault()?.ToString();
                bookingDetails.Origin = recognizerResult.Entities["From"]?.FirstOrDefault()?["Airport"]?.FirstOrDefault()?.FirstOrDefault()?.ToString();
                bookingDetails.TravelDate = recognizerResult.Entities["datetime"]?.FirstOrDefault()?["timex"]?.FirstOrDefault()?.ToString().Split('T')[0];
            }
        }
        catch (Exception e)
        {
            logger.LogWarning($"LUIS Exception: {e.Message} Check your LUIS configuration.");
        }
        return bookingDetails;
    }
}