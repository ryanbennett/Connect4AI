using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Connect4AI.Game;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GameProducer
{
    public static class RunGames
    {
        [FunctionName("RunGames")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
             await context.CallActivityAsync<string>("RunGames_Start", 25000);

            return;
        }

        [FunctionName("RunGames_Start")]
        public static void Start([ActivityTrigger] int gameCount, ILogger log)
        {
            log.LogInformation("In Start Method");

            log.LogInformation($"Games: {gameCount}");

            for (var i = 0; i < gameCount; i++)
            {

                var game = new Game(new GameLogAzureStorage());
                while (game.PlayAutomated(false)) { };
                game.log.Save();
                log.LogInformation($"Saved game. Iterarion: {i}");

            }

        }

        [FunctionName("RunGames_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("RunGames", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}