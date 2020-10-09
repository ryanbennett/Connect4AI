using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Connect4.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Connect4.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<Dictionary<string, double>> PlayAI(List<string[]> board, string aiPlayer)
        {
            board.Reverse();

            try
            {


                if (aiPlayer != "Random Move")
                {

                    var handler = new HttpClientHandler()
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback =
                            (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
                    };


                    using (var client = new HttpClient(handler))
                    {
                        
                        HttpResponseMessage response;
                        response = aiPlayer == "Random Forest" ? await CallRandomForest(board, client) : await CallNeuralNetwork(board, client);
                    
                        if (response.IsSuccessStatusCode)
                            {
                            string result = await response.Content.ReadAsStringAsync();

                            var resultJson = JsonConvert.DeserializeObject<dynamic>(result);

                            var scores = new List<double>();
                            var scoreArray = resultJson["result"][0];
                            scores.Add(scoreArray[0].Value);
                            scores.Add(scoreArray[1].Value);
                            scores.Add(scoreArray[2].Value);
                            scores.Add(scoreArray[3].Value);
                            scores.Add(scoreArray[4].Value);
                            scores.Add(scoreArray[5].Value);
                            scores.Add(scoreArray[6].Value);

                            var scoreDict = new Dictionary<string, double>();
                            scoreDict.Add("ProbabilityColumn1", scores[0]);
                            scoreDict.Add("ProbabilityColumn2", scores[1]);
                            scoreDict.Add("ProbabilityColumn3", scores[2]);
                            scoreDict.Add("ProbabilityColumn4", scores[3]);
                            scoreDict.Add("ProbabilityColumn5", scores[4]);
                            scoreDict.Add("ProbabilityColumn6", scores[5]);
                            scoreDict.Add("ProbabilityColumn7", scores[6]);
                            scoreDict.Add("PredictedPlay", scores.IndexOf(scores.Max()));

                            return scoreDict;
                        }
                        else
                        {
                            Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                            // Print the headers - they include the requert ID and the timestamp,
                            // which are useful for debugging the failure
                            Console.WriteLine(response.Headers.ToString());

                            string responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine(responseContent);
                        }
                    }
                }
                else
                {
                    Random rand = new Random();
                    var move = rand.Next(0, 6);
                    var scoreDict = new Dictionary<string, double>();

                    for (var c = 0; c < 6; c++)
                    {
                        scoreDict.Add($"ProbabilitColumn{c + 1}", c == move ? 1 : 0);
                    }

                    scoreDict.Add("PredictedPlay", move);

                    return scoreDict;
                }


            }
            catch (Exception e)
            {
                var debug = e;
            }

            return null;
        }

        private async Task<HttpResponseMessage> CallNeuralNetwork(List<string[]> board, HttpClient client)
        {

            var dataRow = new int[42];
            var count = 0;
            for (var r = 0; r < 6; r++)
            {
                for (var c = 0; c < 7; c++)
                {
                    int value = board[r][c] == "black" ? 1 : board[r][r] == "red" ? -1 : 0;
                    dataRow[count] = value;
                    count++;
                }
            }

            var scoreRequest = new
            {
                Inputs = new Dictionary<string, int[][]>()
                            {
                                {
                                    "BoardState",new int[1][]{ dataRow}
                                }
                            },
                        
                GlobalParameters = new Dictionary<string, string>()
                {
                }
            };

            var values = GetAIValues("Neural Network");

            string apiKey = values.Item1; // Replace this with the API key for the web service
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri(values.Item2);

            // WARNING: The 'await' statement below can result in a deadlock
            // if you are calling this code from the UI thread of an ASP.Net application.
            // One way to address this would be to call ConfigureAwait(false)
            // so that the execution does not attempt to resume on the original context.
            // For instance, replace code such as:
            //      result = await DoSomeTask()
            // with the following:
            //      result = await DoSomeTask().ConfigureAwait(false)

            var requestString = JsonConvert.SerializeObject(scoreRequest);
            var content = new StringContent(requestString);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync("", content);
            return response;
        }

        private async Task<HttpResponseMessage> CallRandomForest(List<string[]> board, HttpClient client)
        {

            Dictionary<string, string> boardForAI = new Dictionary<string, string>();
            var i = 0;
            for (var r = 0; r < 6; r++)
            {
                for (var c = 0; c < 7; c++)
                {
                    var val = board[r][c];
                    var newVal = val == "red" ? -1 : val == "black" ? 1 : 0;
                    var index = "i" + i;
                    boardForAI.Add(index, newVal.ToString());
                    i++;
                }
            }

            boardForAI.Add("indexPlayed", 0.ToString());
            var scoreRequest = new
            {
                Inputs = new Dictionary<string, List<Dictionary<string, string>>>()
                            {
                                {
                                "BoardState",
                                new List<Dictionary<string, string>>(){
                                boardForAI
                                }
                            },
                        },
                GlobalParameters = new Dictionary<string, string>()
                {
                }
            };

            var values = GetAIValues("Random Forest");

            string apiKey = values.Item1; // Replace this with the API key for the web service
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri(values.Item2);

            // WARNING: The 'await' statement below can result in a deadlock
            // if you are calling this code from the UI thread of an ASP.Net application.
            // One way to address this would be to call ConfigureAwait(false)
            // so that the execution does not attempt to resume on the original context.
            // For instance, replace code such as:
            //      result = await DoSomeTask()
            // with the following:
            //      result = await DoSomeTask().ConfigureAwait(false)

            var requestString = JsonConvert.SerializeObject(scoreRequest);
            var content = new StringContent(requestString);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync("", content);
            return response;
        }

        private Tuple<string, string> GetAIValues(string aiPlayer)
        {
            Tuple<string, string> aiValues;

            switch (aiPlayer)
            {
                case "Random Forest":
                    aiValues = new Tuple<string, string>("ZBV7hcZXShTdoBufnO8cqP9HceuUSG5U", "http://52.162.209.17:80/api/v1/service/connect4rf/score");
                    break;
                case "Neural Network":
                    aiValues = new Tuple<string, string>("QhHvAettSDJXKE90i4WlkcDAY2hHEQ7c", "http://52.162.209.17:80/api/v1/service/connect4nn/score");
                    break;
                default:
                    aiValues = new Tuple<string, string>(string.Empty, string.Empty);
                    break;
            }

            return aiValues;
        }
    }
}



