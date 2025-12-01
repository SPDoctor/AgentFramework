using System;
using System.ComponentModel;
using Azure;
using System.ComponentModel;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.AI;
using Plugins;

var endpoint = Environment.GetEnvironmentVariable("endpoint", EnvironmentVariableTarget.User) ?? throw new InvalidOperationException("Azure endpoint is not set.");
var deploymentName = Environment.GetEnvironmentVariable("deploymentname", EnvironmentVariableTarget.User) ?? "gpt4";
var apiKey = Environment.GetEnvironmentVariable("apiKey", EnvironmentVariableTarget.User) ?? throw new InvalidOperationException("Azure key is not set.");
var credential = new AzureKeyCredential(apiKey);

[Description("Get the weather for a given location.")]
static string GetWeather([Description("The location to get the weather for.")] string location)
    => $"The weather in {location} is cloudy with a high of 15°C.";

// Create the chat client and agent, and provide the function tool to the agent.
//var client = new AzureOpenAIClient(new Uri(endpoint), credential).GetChatClient(deploymentName);
// var agent = client.CreateAIAgent(
//         instructions: "You are a helpful assistant",
//         tools: [AIFunctionFactory.Create(new BingPlugin().GetWeatherAsync)]
//         );

// Create the chat client with function tools.
var client = new AzureOpenAIClient(
    new Uri(endpoint),
    credential)
    .AsChatClient(deploymentName)
    .AsBuilder()
    .UseFunctionInvocation()
    .Build();


// Non-streaming interaction with function tools.
var response = await client.CompleteAsync(
    "What is the weather like in Amsterdam?",
    new ChatOptions
    {
        Tools = [AIFunctionFactory.Create(GetWeather)]
    });
Console.WriteLine(response.Message.Text);

// Non-streaming agent interaction with function tools.
//Console.WriteLine(await agent.RunAsync("What is the weather like in Amsterdam?"));

// Streaming agent interaction with function tools.
// await foreach (var update in agent.RunStreamingAsync("What is the weather like in Amsterdam?"))
// {
//     Console.WriteLine(update);
// }
