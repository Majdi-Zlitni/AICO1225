using System.ClientModel.Primitives;
using System.Text.Json;
using AiTestAgent.src;
using Azure.Identity;
using OpenAI;
using OpenAI.Chat;

#pragma warning disable OPENAI001

namespace AiTestAgent;

public class AzureAiService
{
    private readonly AzureOpenAISettings _settings;

    public AzureAiService(AzureOpenAISettings settings)
    {
        _settings = settings;
    }

    public async Task<TestPlan?> GenerateTestPlanAsync(string userStoryContent)
    {
        if (
            string.IsNullOrWhiteSpace(_settings.Endpoint)
            || string.IsNullOrWhiteSpace(_settings.DeploymentName)
        )
        {
            throw new InvalidOperationException(
                "Azure OpenAI Endpoint or Deployment Name is not configured."
            );
        }

        var tokenPolicy = new BearerTokenPolicy(
            new DefaultAzureCredential(),
            "https://cognitiveservices.azure.com/.default"
        );

        var client = new ChatClient(
            model: _settings.DeploymentName,
            authenticationPolicy: tokenPolicy,
            options: new OpenAIClientOptions() { Endpoint = new Uri(_settings.Endpoint) }
        );

        var prompt = BuildPrompt(userStoryContent);

        Console.WriteLine("\n---> Sending request to Azure OpenAI to generate test plan...");

        try
        {
            ChatCompletion completion = await client.CompleteChatAsync(
                new ChatMessage[]
                {
                    new SystemChatMessage("You are an expert Automation Engineer."),
                    new UserChatMessage(prompt),
                }
            );

            string generatedJson = completion.Content[0].Text;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("---> AI Generated Raw JSON Plan:");
            Console.WriteLine(generatedJson);
            Console.ResetColor();

            var cleanJson = generatedJson.Trim().Trim('`', 'j', 's', 'o', 'n', '\n');

            var testPlan = JsonSerializer.Deserialize<TestPlan>(
                cleanJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return testPlan;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error communicating with Azure AI service: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"---> Inner Exception: {ex.InnerException.Message}");
            }
            return null;
        }
    }

    private string BuildPrompt(string userStory)
    {
        return @"
                Analyze the following user story and generate a precise test plan in a specific JSON format.
                The JSON output should be a single, clean, valid JSON object ready for parsing. Do not include any text, notes, or markdown formatting outside of the main JSON object.

                The JSON object must conform to this structure:
                {
                  ""test_plan"": [
                    {
                      ""step"": <integer>,
                      ""description"": ""<string>"",
                      ""element"": ""<string: ID, selector, or name>"",
                      ""action"": ""<string: navigate, type, click, verify_contains_text, verify_url_path>"",
                      ""value"": ""<string: URL, text to type, text to verify, etc.>""
                    }
                  ]
                }

                Here is the user story:
                ---
                "
            + userStory
            + @"
                ---

                Generate the JSON test plan now.
                ";
    }
}
