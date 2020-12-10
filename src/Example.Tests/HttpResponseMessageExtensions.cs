using System;
using System.Net.Http;

namespace Example.Tests
{
    public static class HttpResponseMessageExtensions
    {
        public static void EnsureSuccessStatus(this HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var body = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var message = $"Failed with status '{(int) httpResponseMessage.StatusCode}' and body:\n<<{body}>>";

                throw new Exception(message);
            }
        }
    }
}
