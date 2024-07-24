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
        _chatHistory.AddSystemMessage("나는 요리 레시피를 안내하는 챗봇이야. 답변은 따뜻하게 해줘.");
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