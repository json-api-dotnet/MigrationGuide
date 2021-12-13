namespace Example.Tests;

public static class HttpResponseMessageExtensions
{
    public static void EnsureSuccessStatus(this HttpResponseMessage httpResponseMessage)
    {
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            string body = httpResponseMessage.Content.ReadAsStringAsync().Result;
            string message = $"Failed with status '{(int)httpResponseMessage.StatusCode}' and body:\n<<{body}>>";

            throw new Exception(message);
        }
    }
}
