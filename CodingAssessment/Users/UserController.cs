using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CodingAssessment.Users;

public record ApiKeyResponse([Required] string ApiKey);

[ApiController, Route("users")]
public class UserController : ControllerBase
{

    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [SwaggerOperation("Get Api key", "Get by email")]
    public async Task<ActionResult<ApiKeyResponse>> GetApiKey(string email)
    {
        var apiKey = await _userService.GetApiKey(email);
        return Ok(new ApiKeyResponse(apiKey));
    }

    [HttpPost]
    [SwaggerOperation("Generate api key", "Generate an key to access the package you have added")]
    public async Task<ActionResult> CreateUser([Required] string email)
    {
        var key = await _userService.CreateUser(email);
        return Ok(new ApiKeyResponse(key));
    }
}