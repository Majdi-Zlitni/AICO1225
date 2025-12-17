using AiTestAgent;
using AiTestAgent.src;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static async Task Main(string[] args)
    {
        // 1. Set up configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var appSettings = configuration.Get<AppSettings>();

        if (appSettings is null || string.IsNullOrWhiteSpace(appSettings.GitHub.UserStoryUrl))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: GitHub UserStoryUrl is not configured in appsettings.json.");
            Console.ResetColor();
            return;
        }

        // 2. Fetch the user story from GitHub
        var httpClient = new HttpClient();
        var githubReader = new GitHubReaderService(httpClient);
        string userStoryContent = await githubReader.GetUserStoryAsync(
            appSettings.GitHub.UserStoryUrl
        );

        // 3. Generate the test plan using Azure AI
        var aiService = new AzureAiService(appSettings.AzureOpenAI);
        var testPlan = await aiService.GenerateTestPlanAsync(userStoryContent);

        if (testPlan == null || testPlan.Steps.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n---> Failed to generate a valid test plan. Aborting.");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(
            $"\n---> Successfully generated test plan with {testPlan.Steps.Count} steps."
        );
        Console.ResetColor();

        // 4. Execute the generated test plan with Playwright
        var testExecutor = new PlaywrightTestExecutor();
        await testExecutor.ExecuteTestPlanAsync(testPlan);
    }
}
