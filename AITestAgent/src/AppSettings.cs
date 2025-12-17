namespace AiTestAgent;

public class AppSettings
{
    public GitHubSettings GitHub { get; set; } = new();

    public AzureOpenAISettings AzureOpenAI { get; set; } = new();
}

public class GitHubSettings
{
    public string UserStoryUrl { get; set; } = string.Empty;
}

public class AzureOpenAISettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
}
