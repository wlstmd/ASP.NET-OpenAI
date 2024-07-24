using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace AzureOpenAI.Services;

public class ChatService
{
    private readonly Kernel _kernel;
    private readonly ChatHistory _chatHistory;

    public ChatService(Kernel kernel)
    {
        _kernel = kernel;
        _chatHistory = new ChatHistory();
        _chatHistory.AddSystemMessage("���� �丮 �����Ǹ� �ȳ��ϴ� ê���̾�. �亯�� �����ϰ� ����.");
    }

    public async Task<string> GetResponseAsync(string userInput)
    {
        if (string.IsNullOrEmpty(userInput))
        {
            throw new ArgumentException("User input cannot be null or empty.", nameof(userInput));
        }

        _chatHistory.AddUserMessage(userInput);
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

        try
        {
            return await GetNonStreamingResponseAsync(chatCompletionService);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }

    private async Task<string> GetNonStreamingResponseAsync(IChatCompletionService chatCompletionService)
    {
        var result = await chatCompletionService.GetChatMessageContentAsync(_chatHistory);
        if (!string.IsNullOrEmpty(result.Content))
        {
            _chatHistory.AddAssistantMessage(result.Content);
        }
        return result.Content ?? string.Empty;
    }
}