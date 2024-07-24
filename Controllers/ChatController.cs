using Microsoft.AspNetCore.Mvc;
using AzureOpenAI.Services;
using AzureOpenAI.Models;

namespace AzureOpenAI.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> PostChat([FromBody] ChatMessage message)
    {
        if (message == null || string.IsNullOrEmpty(message.Content))
        {
            return BadRequest("Message content cannot be null or empty.");
        }

        try
        {
            var response = await _chatService.GetResponseAsync(message.Content);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}