using Dashboard.Common.Exceptions;
using Dashboard.DataAccess.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestErrorsController : ControllerBase
{
    private readonly WebbanhangDbContext _dbContext;

    public TestErrorsController(WebbanhangDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetNotFoundRequest()
    {
        throw new NotFoundException("Not found test");
    }

    [HttpGet]
    public IActionResult GetBadRequest()
    {
        throw new BadRequestException("Bad request test");
    }

    [HttpGet]
    public IActionResult GetConflictRequest()
    {
        throw new ConflictException("Conflict request test");
    }

    [HttpGet]
    public IActionResult GetValidationRequest()
    {
        var validationErrors = new List<ValidationError>
        {
            new ValidationError("propertyName1", "Error message 1."),
            new ValidationError("propertyName2", "Error message 2.")
        };
        throw new ValidationException(validationErrors);
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAuthErrorRequest()
    {
        return Ok("Authorized");
    }

    [HttpGet]
    public IActionResult GetInternalServerErrorRequest()
    {
        var thing = _dbContext.Products.Find(-1);
        var thingToReturn = thing!.ToString();
        return Ok(thingToReturn);
    }
}
