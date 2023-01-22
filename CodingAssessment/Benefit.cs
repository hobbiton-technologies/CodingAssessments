using System.ComponentModel.DataAnnotations;

namespace CodingAssessment;

public class Benefit
{
    [Required] public int Id { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required string Description { get; set; }
}

public class ProductBenefitRequest
{
    [Required] public required int ProductId { get; init; }
    [Required] public required string Name { get; init; }
    [Required] public required string Description { get; init; }
}

public class BenefitRequest
{
    [Required] public required string Name { get; init; }
    [Required] public required string Description { get; init; }
}

public class BenefitUpdateRequest
{
    [MinLength(1)] public string? Name { get; init; }
    [MinLength(1)] public string? Description { get; init; }
}