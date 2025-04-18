// Import packages
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Connectors.Google;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Populate values from your OpenAI deployment
//var modelId = "phi-4-mini-instruct";
//var endpoint = "http://127.0.0.1:1234/v1";
var modelId = "llama3.2";
var endpoint = "http://localhost:11434/v1";
var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(1) };
// Create a kernel with Azure OpenAI chat completion

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var builder = Kernel.CreateBuilder().AddOpenAIChatCompletion(
  modelId: modelId,
  apiKey: null,
  endpoint: new Uri(endpoint),
  httpClient: httpClient);
#pragma warning restore SKEXP0010

//Add enterprise components
builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

//Build the kernel
Kernel kernel = builder.Build();

OpenAIPromptExecutionSettings promptExecutionSettings = new()
{
  FunctionChoiceBehavior = FunctionChoiceBehavior.Required(),
};

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Add a plugin (the LightsPlugin class is defined below)
kernel.Plugins.AddFromType<LightsPlugin>("Lights");

//var systemMessage = "You control lights. You can turn them on, turn them off, and report their current status. Respond concisely and only acknowledge valid commands.";

// Create a history store the conversation
var history = new ChatHistory("You are an expert at managing lights. You can toggle them ON and OFF and know their state.");

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
  Console.WriteLine("Assistant > " + result);

  // Add the message from the agent to the chat history
  history.AddMessage(result.Role, result.Content ?? string.Empty);
} while (userInput is not null);