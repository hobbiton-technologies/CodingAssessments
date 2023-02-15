using System.Text.Json;
using CodingAssessment.Users;
using Flurl.Http;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CodingAssessment;

public class PackageService
{

    private readonly PostgresDbContext _postgresDbContext;
    private readonly UserService _userService;
    private readonly string? _apiKey;

    public PackageService(PostgresDbContext postgresDbContext, IHttpContextAccessor httpContextAccessor,
        UserService userService)
    {
        _postgresDbContext = postgresDbContext;
        _userService = userService;
        _apiKey = httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
    }


    public async Task<List<Package>> GetPackagesAsync()
    {
        if (_apiKey == null)
        {
            return await _postgresDbContext.Packages
                .Include(x => x.Benefits)
                .Where(x => x.CreatedBy == null)
                .ToListAsync();
        }

        var user = await _userService.GetUser(_apiKey);
        return await _postgresDbContext.Packages
            .Include(x => x.Benefits)
            .Where(x => x.CreatedById == user.Id)
            .ToListAsync();
    }

    public async Task<Package> GetPackageAsync(int packageId)
    {
        if (_apiKey == null)
        {
            return await _postgresDbContext.Packages
                       .Include(p => p.Benefits)
                       .Where(x => x.CreatedBy == null)
                       .FirstOrDefaultAsync(p => p.Id == packageId) ??
                   throw new Exception($"Package with id {packageId} not found");
        }

        var user = await _userService.GetUser(_apiKey);

        return await _postgresDbContext.Packages
                   .Include(p => p.Benefits)
                   .Where(x => x.CreatedById == user.Id)
                   .FirstOrDefaultAsync(p => p.Id == packageId) ??
               throw new Exception($"Package with id {packageId} not found");
    }


    public async Task<Package> AddPackageAsync(PackageRequest package)
    {
        User? user = null;

        if (_apiKey != null)
        {
            user = await _userService.GetUser(_apiKey);
        }

        var newPackage = package.Adapt<Package>();
        newPackage.CreatedBy = user;
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
        var existingBenefit = await _postgresDbContext.Benefits.FirstOrDefaultAsync(x => x.Id == benefitId) ??
                              throw new Exception($"Benefit with id {benefitId} not found");
        benefit.Adapt(existingBenefit);
        _postgresDbContext.Benefits.Update(existingBenefit);
        await _postgresDbContext.SaveChangesAsync();
        return existingBenefit;
    }

    public async Task DeleteBenefitAsync(int benefitId)
    {
        var existingBenefit = await _postgresDbContext.Benefits.FirstOrDefaultAsync(x => x.Id == benefitId) ??
                              throw new Exception($"Benefit with id {benefitId} not found");
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


    public async Task<string> UploadDocumentAsync(FileUploadPayload document)
    {
        var result = await "https://storage-api.hobbiton.tech/uploads"
            .PostMultipartAsync(content =>
            {
                content.AddFile("file", document.File.OpenReadStream(), document.File.FileName);
                content.AddString("bucketName", document.BucketName);
                
            })
            .ReceiveJson<FileUploadResponse>();

        return result?.Url ?? throw new Exception("Failed to upload document");
    }


}