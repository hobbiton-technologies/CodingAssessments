using System.Text.Json;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CodingAssessment;

public class PackageService
{

    private readonly DatabaseContext _databaseContext;

    public PackageService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    public async Task<List<Package>> GetPackagesAsync()
    {
        return await _databaseContext.Packages.Include(x=>x.Benefits).ToListAsync();
    }

    public async Task<Package> GetPackageAsync(int packageId)
    {
        return await _databaseContext.Packages
            .Include(p => p.Benefits)
            .FirstOrDefaultAsync(p => p.Id == packageId) ?? throw new Exception("Package not found");
    }


    public async Task<Package> AddPackageAsync(PackageRequest package)
    {
        var newPackage = package.Adapt<Package>();
        await _databaseContext.Packages.AddAsync(newPackage);
        await _databaseContext.SaveChangesAsync();
        return newPackage;
    }


    public async Task<Package> UpdatePackageAsync(int packageId, PackageUpdateRequest package)
    {
        var existingPackage = await GetPackageAsync(packageId);

        package.Adapt(existingPackage);

        _databaseContext.Packages.Update(existingPackage);
        await _databaseContext.SaveChangesAsync();
        return existingPackage;
    }


    public async Task DeletePackageAsync(int packageId)
    {
        var existingPackage = await GetPackageAsync(packageId);
        _databaseContext.Packages.Remove(existingPackage);
        await _databaseContext.SaveChangesAsync();
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