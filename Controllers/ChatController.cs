using Microsoft.AspNetCore.Mvc;
using AzureOpenAI.Services;

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

    [HttpGet]
    public async Task<ActionResult<string>> GetChat([FromQuery] string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return BadRequest("Message content cannot be null or empty.");
        }

        try
        {
            var response = await _chatService.GetResponseAsync(message);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}