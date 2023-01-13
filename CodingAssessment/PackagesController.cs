using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CodingAssessment;

[ApiController, Route("packages")]
public class PackagesController : ControllerBase
{
    private readonly PackageService _packageService;

    public PackagesController(PackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpGet]
    [SwaggerOperation("GetPackages", "Get all packages")]
    public async Task<ActionResult<ICollection<Package>>> GetPackages()
    {
        return Ok(await _packageService.GetPackagesAsync());
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation("GetPackage", "Get a package by id")]
    public async Task<ActionResult<Package>> GetPackage(int id)
    {
        return Ok(await _packageService.GetPackageAsync(id));
    }

    [HttpPost]
    [SwaggerOperation("AddPackage", "Add a package")]
    public async Task<ActionResult<Package>> AddPackage(PackageRequest package)
    {
        return Ok(await _packageService.AddPackageAsync(package));
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation("UpdatePackage", "Update a package")]
    public async Task<ActionResult<Package>> UpdatePackage(int id,PackageUpdateRequest package)
    {
        return Ok(await _packageService.UpdatePackageAsync(id,package));
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation("DeletePackage", "Delete a package")]
    public async Task<ActionResult> DeletePackage(int id)
    {
        await _packageService.DeletePackageAsync(id);
        return Ok();
    }
}