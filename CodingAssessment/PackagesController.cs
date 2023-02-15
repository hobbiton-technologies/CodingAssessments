using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CodingAssessment;

[ApiController, Route("packages"), Produces("application/json"), Consumes("application/json"),]
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
    public async Task<ActionResult<Package>> UpdatePackage(int id, PackageUpdateRequest package)
    {
        return Ok(await _packageService.UpdatePackageAsync(id, package));
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation("DeletePackage", "Delete a package")]
    public async Task<ActionResult> DeletePackage(int id)
    {
        await _packageService.DeletePackageAsync(id);
        return Ok();
    }


}


[ApiController,Route("upload")]
public class UploadController : ControllerBase
{
    private readonly PackageService _packageService;

    public UploadController(PackageService packageService)
    {
        _packageService = packageService;
    }

    [HttpPost]
    [SwaggerOperation("Upload supporting document", "Upload document")]
    public async Task<ActionResult> UploadDocument([FromForm] FileUploadRequest request)
    {
        var result = await _packageService.UploadDocumentAsync(new FileUploadPayload
        {
            File = request.File,
            BucketName = "assessment",
        });
        return Ok(new
        {
            Url = result,
        });
    }
}