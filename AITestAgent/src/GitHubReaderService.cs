namespace AiTestAgent.src;

public class GitHubReaderService
{
    private readonly HttpClient _httpClient;

    public GitHubReaderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Fetches the content of a user story from a given URL.
    /// </summary>
    /// <param name="url">The raw content URL of the user story on GitHub.</param>
    /// <returns>The string content of the user story file.</returns>
    public async Task<string> GetUserStoryAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException(
                "GitHub user story URL cannot be null or empty.",
                nameof(url)
            );
        }

        try
        {
            Console.WriteLine($"---> Fetching user story from: {url}");
            var content = await _httpClient.GetStringAsync(url);
            return content;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching user story: {ex.Message}");
            throw;
        }
    }
}
