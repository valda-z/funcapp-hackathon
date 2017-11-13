using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    
    string result="n/a";

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    
    using (var client = new HttpClient())
{
    var scoreRequest = new
    {
        Inputs = new Dictionary<string, List<Dictionary<string, string>>> () {
            {
                "input1",
                new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
                                {
                                    "Survived", "-1"
                                },
                                {
                                    "PassengerClass", data.PassengerClass.ToString()
                                },
                                {
                                    "Gender", data.Gender.ToString()
                                },
                                {
                                    "Age", data.Age.ToString()
                                },
                                {
                                    "SiblingSpouse", "1"
                                },
                                {
                                    "ParentChild", "0"
                                },
                                {
                                    "FarePrice", "7.25"
                                },
                                {
                                    "PortEmbarkation", "S"
                                },
                    }
                }
            },
        },
        GlobalParameters = new Dictionary<string, string>() {
        }
    };

    const string apiKey = "### YOUR_KEY ###"; // Replace this with the API key for the web service
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", apiKey);
    client.BaseAddress = new Uri("### YOUR_URI ###");


    HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

    if (response.IsSuccessStatusCode)
    {
        result = await response.Content.ReadAsStringAsync();
        log.Info("SUCCESS! Result: " + result);
    }
    else
    {
        log.Info(string.Format("The request failed with status code: {0}", response.StatusCode));

        // Print the headers - they include the requert ID and the timestamp,
        // which are useful for debugging the failure
        log.Info(response.Headers.ToString());

        string responseContent = await response.Content.ReadAsStringAsync();
        log.Info(responseContent);
    }
}


   return req.CreateResponse(HttpStatusCode.OK, result);

}
