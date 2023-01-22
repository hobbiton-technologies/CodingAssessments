using System.Text.Json;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CodingAssessment;

public class PackageService
{

    private readonly PostgresDbContext _postgresDbContext;

    public PackageService(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }


    public async Task<List<Package>> GetPackagesAsync()
    {
        return await _postgresDbContext.Packages.Include(x => x.Benefits).ToListAsync();
    }

    public async Task<Package> GetPackageAsync(int packageId)
    {
        return await _postgresDbContext.Packages
            .Include(p => p.Benefits)
            .FirstOrDefaultAsync(p => p.Id == packageId) ?? throw new Exception($"Package with id {packageId} not found");
    }


    public async Task<Package> AddPackageAsync(PackageRequest package)
    {
        var newPackage = package.Adapt<Package>();
        await _postgresDbContext.Packages.AddAsync(newPackage);
        await _postgresDbContext.SaveChangesAsync();
        return newPackage;
    }


    public async Task<Package> UpdatePackageAsync(int packageId, PackageUpdateRequest package)
    {
        var existingPackage = await GetPackageAsync(packageId);

        package.Adapt(existingPackage);

        _postgresDbContext.Packages.Update(existingPackage);
        await _postgresDbContext.SaveChangesAsync();
        return existingPackage;
    }


    public async Task<Benefit> AddBenefitAsync(ProductBenefitRequest benefit)
    {
        var existingPackage = await GetPackageAsync(benefit.ProductId);
        var newBenefit = benefit.Adapt<Benefit>();
        existingPackage.Benefits.Add(newBenefit);
        await _postgresDbContext.Benefits.AddAsync(newBenefit);
        await _postgresDbContext.SaveChangesAsync();
        return newBenefit;
    }

    public async Task<Benefit> UpdateBenefitAsync(int benefitId, BenefitUpdateRequest benefit)
    {
        var existingBenefit = await _postgresDbContext.Benefits.FirstOrDefaultAsync(x => x.Id == benefitId) ?? throw new Exception($"Benefit with id {benefitId} not found");
        benefit.Adapt(existingBenefit);
        _postgresDbContext.Benefits.Update(existingBenefit);
        await _postgresDbContext.SaveChangesAsync();
        return existingBenefit;
    }

    public async Task DeleteBenefitAsync(int benefitId)
    {
        var existingBenefit = await _postgresDbContext.Benefits.FirstOrDefaultAsync(x => x.Id == benefitId) ?? throw new Exception($"Benefit with id {benefitId} not found");
        _postgresDbContext.Benefits.Remove(existingBenefit);
        await _postgresDbContext.SaveChangesAsync();
    }


    public async Task DeletePackageAsync(int packageId)
    {
        var existingPackage = await GetPackageAsync(packageId);

        foreach (var existingPackageBenefit in existingPackage.Benefits)
        {
            _postgresDbContext.Benefits.Remove(existingPackageBenefit);
        }

        _postgresDbContext.Packages.Remove(existingPackage);
        await _postgresDbContext.SaveChangesAsync();
    }


    public static async Task<string> UploadDocumentAsync(IFormFile document)
    {
        using var httpClient = new HttpClient();
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(document.OpenReadStream()), "file", document.FileName);
        var response = await httpClient.PostAsync("https://storage-api.hobbiton.tech/Uploads", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to upload document");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var uploadResponse = JsonSerializer.Deserialize<FileUploadResponse>(responseContent);
        return uploadResponse?.Url ?? throw new Exception("Failed to upload document");
    }


}