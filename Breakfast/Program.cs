using Breakfast;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;

class Program
{
  static async Task Main(string[] args)
  {
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    var config = new ConfigurationBuilder()
          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
          .AddJsonFile("appsettings.json")
          .AddUserSecrets<Program>()
          .Build();

    // Google
#pragma warning disable SKEXP0070
    IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
    kernelBuilder.AddGoogleAIGeminiChatCompletion(
        modelId: "gemini-2.0-flash",
        apiKey: config.GetValue<string>("GoogleApiKey")
    );
#pragma warning restore SKEXP0010

    // Add logging
    kernelBuilder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

    // Build the kernel
    Kernel kernel = kernelBuilder.Build();

    // Set up the prompt execution settings
    GeminiPromptExecutionSettings promptExecutionSettings = new()
    {
      ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions,
    };

    var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

    // Add a plugin
    kernel.Plugins.AddFromType<CookingSteps>("CookingBreakfast");

    // Create a history store the conversation
    var systemMessage = "You are a breakfast chef. You can prepare and cook breakfast.";
    var history = new ChatHistory(systemMessage);

    // Initiate a back-and-forth chat
    string? userInput;
    do
    {
      // Collect user input
      Console.Write("User > ");
      userInput = Console.ReadLine();

      // Add user input
      history.AddUserMessage(userInput);

      // Get the response from the AI
      var result = await chatCompletionService.GetChatMessageContentAsync(
          history,
          executionSettings: promptExecutionSettings,
          kernel: kernel);

      // Print the results
      Console.WriteLine("AI > " + result);

      // Add the message from the agent to the chat history
      history.AddMessage(result.Role, result.Content ?? string.Empty);
    } while (userInput is not null);
  }
}
