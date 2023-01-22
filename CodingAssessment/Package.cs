using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CodingAssessment;

public class PackageRequest
{
    [Required] public required string Name { get; init; }
    [Required] public required string Description { get; init; }
    [Required] public double Premium { get; init; }
    [Required] public required string SupportingDocumentUrl { get; init; }
    [MinLength(1)] public ICollection<BenefitRequest> Benefits { get; init; }
}

public class PackageUpdateRequest
{
    [MinLength(1)] public string? Name { get; init; }
    [MinLength(1)] public string? Description { get; init; }
    [MinLength(1)] public double? Premium { get; init; }
    [MinLength(1)] public string? SupportingDocumentUrl { get; init; }

}

public class Package
{
    [Required] public int Id { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required string Description { get; set; }
    [Required] public required double Premium { get; set; }
    [Required] public required string SupportingDocumentUrl { get; set; }
    [MinLength(1)] public ICollection<Benefit> Benefits { get; set; } = new HashSet<Benefit>();
}

public class FileUploadResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; } = null!;
}