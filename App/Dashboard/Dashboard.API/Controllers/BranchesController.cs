using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BranchesController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchesController(IBranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBranches([FromQuery] GetBranchesInput input)
    {
        return Ok(await _branchService.GetBranchesAsync(input));
    }
}
