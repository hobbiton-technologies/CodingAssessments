using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CodingAssessment;

[ApiController, Route("benefits")]
public class BenefitController : ControllerBase
{
    private readonly PackageService _packageService;

    public BenefitController(PackageService packageService)
    {
        _packageService = packageService;
    }


    [HttpPost("")]
    [SwaggerOperation("AddBenefit", "Add a benefit")]
    public async Task<ActionResult<Benefit>> AddBenefit(ProductBenefitRequest benefit)
    {
        return Ok(await _packageService.AddBenefitAsync(benefit));
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation("UpdateBenefit", "Update a benefit")]
    public async Task<ActionResult<Benefit>> UpdateBenefit(int id, BenefitUpdateRequest benefit)
    {
        return Ok(await _packageService.UpdateBenefitAsync(id, benefit));
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation("DeleteBenefit", "Delete a benefit")]
    public async Task<ActionResult> DeleteBenefit(int id)
    {
        await _packageService.DeleteBenefitAsync(id);
        return Ok();
    }


}