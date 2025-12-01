using Azure;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Plugins;
using System;
using System.ComponentModel;

var endpoint = Environment.GetEnvironmentVariable("endpoint", EnvironmentVariableTarget.User) ?? throw new InvalidOperationException("Azure endpoint is not set.");
var deploymentName = Environment.GetEnvironmentVariable("deploymentname", EnvironmentVariableTarget.User) ?? "gpt4";
var apiKey = Environment.GetEnvironmentVariable("apiKey", EnvironmentVariableTarget.User) ?? throw new InvalidOperationException("Azure key is not set.");
var credential = new AzureKeyCredential(apiKey);

[Description("Get the weather for a given location.")]
static string GetWeather([Description("The location to get the weather for.")] string location)
    => $"The weather in {location} is cloudy with a high of 15°C.";

// Create the chat client with function tools.
var client = new AzureOpenAIClient(new Uri(endpoint), credential);
var weatherAgent = client.AsChatClient(deploymentName)
  .AsBuilder()
  .UseFunctionInvocation()
  .Build();

var chatOptions = new ChatOptions
{
    Tools = [AIFunctionFactory.Create(GetWeather)]
};


// Non-streaming interaction with function tools.
var response = await weatherAgent.CompleteAsync(
  "What is the weather like in Amsterdam?", chatOptions);
Console.WriteLine(response.Message.Text);

// Streaming agent interaction with function tools.
// await foreach (var update in weatherAgent.CompleteStreamingAsync("What is the weather like in Amsterdam?"), chatOptions)
// {
//     Console.WriteLine(update);
// }
