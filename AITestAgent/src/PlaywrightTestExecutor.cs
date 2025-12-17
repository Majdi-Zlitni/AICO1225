using AiTestAgent.src;
using Microsoft.Playwright;

namespace AiTestAgent;

public class PlaywrightTestExecutor
{
    public async Task ExecuteTestPlanAsync(TestPlan testPlan)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions { Headless = false }
        );

        var page = await browser.NewPageAsync();

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n---> Starting Playwright test execution...");
        Console.ResetColor();

        foreach (var step in testPlan.Steps.OrderBy(s => s.Step))
        {
            Console.WriteLine($"  [Step {step.Step}] {step.Description}...");

            var smartLocator = $"#{step.Element}, [data-test='{step.Element}']";

            try
            {
                switch (step.Action.ToLower())
                {
                    case "navigate":
                        await page.GotoAsync(step.Value);
                        break;
                    case "type":
                        await page.Locator(smartLocator).FillAsync(step.Value);
                        break;
                    case "click":
                        await page.Locator(smartLocator).ClickAsync();
                        break;
                    case "verify_url_path":
                        await Assertions
                            .Expect(page)
                            .ToHaveURLAsync(
                                new System.Text.RegularExpressions.Regex($".*{step.Value}")
                            );
                        break;
                    case "verify_contains_text":
                        await Assertions
                            .Expect(page.Locator($"*:has-text('{step.Value}')").First)
                            .ToBeVisibleAsync();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(
                            $"    -> Warning: Action '{step.Action}' is not implemented."
                        );
                        Console.ResetColor();
                        break;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"    -> PASSED");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"    -> FAILED on action '{step.Action}' for element '{step.Element}'."
                );
                Console.WriteLine($"    -> Error: {ex.Message.Split('\n')[0]}");
                Console.ResetColor();
                await browser.CloseAsync();
                return;
            }
        }

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n---> Test execution completed successfully!");
        Console.ResetColor();

        await Task.Delay(5000);
    }
}
