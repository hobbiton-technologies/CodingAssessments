using System.Text.Json.Serialization;

namespace CodingAssessment;

public record PackageRequest(string Name, string Description, double Premium, string SupportingDocumentUrl, ICollection<BenefitRequest> Benefits);
public record PackageUpdateRequest(string? Name, string? Description, double? Premium, string? SupportingDocumentUrl);


public class Package
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required double Premium { get; set; }
    public required string SupportingDocumentUrl { get; set; }
    public ICollection<Benefit> Benefits { get; set; } = new HashSet<Benefit>();
}

public record BenefitRequest(string Name, string Description);
public record BenefitUpdateRequest(string? Name, string? Description);
public record ProductBenefitRequest(int ProductId,string Name, string Description);

public class Benefit
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}


public class FileUploadResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; } = null!;
}
