using System.Text.Json.Serialization;

namespace AiTestAgent;

public class AzureAiRequest
{
    [JsonPropertyName("input_data")]
    public RequestInputData InputData { get; set; }

    public AzureAiRequest(string prompt)
    {
        InputData = new RequestInputData(prompt);
    }
}

public class RequestInputData
{
    [JsonPropertyName("input_string")]
    public List<string> InputString { get; set; }

    [JsonPropertyName("parameters")]
    public Dictionary<string, object> Parameters { get; set; } =
        new()
        {
            { "temperature", 0.1 },
            { "top_p", 0.9 },
            { "do_sample", true },
            { "max_new_tokens", 2048 }
        };

    public RequestInputData(string prompt)
    {
        InputString = new List<string> { prompt };
    }
}

public class AzureAiResponse
{
    [JsonPropertyName("output")]
    public List<string> Output { get; set; } = new();
}
