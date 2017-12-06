using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestTask
{
    public static class InternetHelper
    {
        private class ODataResponse<T>
        {
            [JsonProperty("@odata.context")]
            public string Metadata { get; set; }
            public List<T> Value { get; set; }
        }

        async public static Task<List<T>> LoadControlsDataAsync<T>(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var odataString = await client.GetStringAsync(url).ConfigureAwait(false);
                    var odata = JsonConvert.DeserializeObject<ODataResponse<T>>(odataString);
                    return odata.Value;
                }
            }
            catch (Exception exc)
            {
                throw;               
            }
        }
    }
}
