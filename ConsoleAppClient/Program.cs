using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleAppClient
{
    public static class Program
    {
        private static async Task Main()
        {
            var client = new HttpClient();
            // get the discovery document for our identityserver 
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Exception.Message);
                return;
            }

            // get the identity token 
            // clientID = client the client id is registred in Config.cs in identityServer scope is the alloed scop 
            // for this 
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "protectedApi"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            var apiClient = new HttpClient();

            //set the access token
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            // try to access to protected API
            var response = apiClient.GetAsync("http://localhost:5001/api/values");
            if (!response.Result.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Result.StatusCode);
            }
            else
            {
                var content = await response.Result.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadLine();
        }
    }
}
