﻿using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo5luismiddleware.Bots
{
    public class SimpleBot : IBot
    {
        public async Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var result = turnContext.Services.Get<RecognizerResult>
                    (LuisRecognizerMiddleware.LuisRecognizerResultKey);
                var topIntent = result?.GetTopScoringIntent();
                switch ((topIntent != null) ? topIntent.Value.intent : null)
                {
                    case null:
                        await turnContext.SendActivity("Failed to get results from LUIS.");
                        break;
                    case "none":
                        await turnContext.SendActivity("Sorry, I don't understand.");
                        break;
                    case "adjustlights":
                        await turnContext.SendActivity("Adjusting the lights");
                        break;
                    case "adjusttemperature":
                        // Cancel the process.
                        await turnContext.SendActivity("Adjusting the temperature.");
                        break;
                    default:
                        // Received an intent we didn't expect, so send its name and score.
                        await turnContext.SendActivity($"Intent: {topIntent.Value.intent} ({topIntent.Value.score}).");
                        break;
                }
            }
        }
    }
}
