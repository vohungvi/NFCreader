using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CardsRegistration
{
    class HttpRequest
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> SendPOSTrequestAsync(string APIaddress, Dictionary<string, string> param)
        {
            var content = new FormUrlEncodedContent(param);

            var response = await client.PostAsync(APIaddress, content);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
