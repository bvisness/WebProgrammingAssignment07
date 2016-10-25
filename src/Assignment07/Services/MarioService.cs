using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Practices.TransientFaultHandling;
using System.IO;
using Newtonsoft.Json;

namespace Assignment07.Services
{
    public class MarioService : IMarioService
    {
        private static string baseUrl = "http://webprogrammingassignment7.azurewebsites.net/mario/";
        //private static string baseUrl = "http://localhost:63312/mario/";

        private RetryPolicy retryPolicy = new RetryPolicy(
            new MarioDetectionStrategy(),
            10,
            new TimeSpan(0, 0, 0, 0, 50),
            new TimeSpan(0, 0, 0, 1, 0),
            new TimeSpan(0, 0, 0, 0, 100)
        );

        public async Task<MarioResult> GetMarioActionResult(string action)
        {
            var request = WebRequest.Create(baseUrl + action);

            HttpWebResponse response;
            try {
                response = await retryPolicy.ExecuteAsync(async () =>
                {
                    return (HttpWebResponse)(await request.GetResponseAsync());
                });
            } catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var reader = new StreamReader(response.GetResponseStream());
                    var body = await reader.ReadToEndAsync();
                    dynamic bodyParsed = JsonConvert.DeserializeObject(body);
                    var result = new MarioResult {
                        ActionResult = MarioActionResult.OK,
                        NextStep = bodyParsed.NextStep
                    };
                    return result;
                case HttpStatusCode.InternalServerError:
                    return new MarioResult { ActionResult = MarioActionResult.Died };
                default:
                    return new MarioResult { ActionResult = MarioActionResult.Error };
            }
        }
    }
}
