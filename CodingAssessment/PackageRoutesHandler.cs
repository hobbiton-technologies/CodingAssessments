// using Microsoft.AspNetCore.Mvc;
//
// namespace CodingAssessment;
//
// public static class PackageRoutesHandler
// {
//     
//     
//
//     public static void RegisterPackageRoutes(this IEndpointRouteBuilder routeBuilder)
//     {
//         
//         //for some reason this is not working work around is to use a controller
//         // routeBuilder.MapGet("packages", GetPackages)
//         //     .WithOpenApi(operation =>
//         //     {
//         //         operation.Description = "Get all packages";
//         //         operation.Summary = "Get all packages";
//         //         return operation;
//         //     });
//
//         routeBuilder.MapGet("{id:int}", GetPackage)
//             .WithOpenApi(operation =>
//             {
//                 operation.Summary = "GetPackage";
//                 operation.Description = "Get a package by id";
//                 return operation;
//             });
//
//         routeBuilder.MapPost("", CreatePackage).WithOpenApi(operation =>
//         {
//             operation.Summary = "CreatePackage";
//             operation.Description = "Create a package";
//             return operation;
//         });
//
//         routeBuilder.MapPut("{id:int}", UpdatePackage).WithOpenApi(operation =>
//         {
//             operation.Summary = "UpdatePackage";
//             operation.Description = "Update a package";
//             return operation;
//         });
//
//         routeBuilder.MapDelete("{id:int}", DeletePackage).WithOpenApi(operation =>
//         {
//             operation.Summary = "DeletePackage";
//             operation.Description = "Delete a package";
//             return operation;
//         });
//     }
//
//     // private static async Task GetPackages(HttpContext context)
//     // {
//     //     var packages = await context.RequestServices.GetRequiredService<PackageService>().GetPackages();
//     //     await context.Response.WriteAsJsonAsync(packages);
//     // }
//
//     private static async Task GetPackage(HttpContext context, int id)
//     {
//         var package = await context.RequestServices.GetRequiredService<PackageService>().GetPackageAsync(id);
//         await context.Response.WriteAsJsonAsync(package);
//     }
//
//     private static async Task CreatePackage(HttpContext context, PackageRequest request)
//     {
//         var package  =await context.RequestServices.GetRequiredService<PackageService>().AddPackageAsync(request);
//         context.Response.StatusCode = 201;
//         await context.Response.WriteAsJsonAsync(package);
//     }
//
//     private static async Task UpdatePackage(HttpContext context, int id, PackageUpdateRequest request)
//     {
//         var package = await context.RequestServices.GetRequiredService<PackageService>().UpdatePackageAsync(id, request);
//         context.Response.StatusCode = 204;
//         await context.Response.WriteAsJsonAsync(package);
//     }
//
//
//     private static async Task DeletePackage(HttpContext context, int id)
//     {
//         await context.RequestServices.GetRequiredService<PackageService>().DeletePackageAsync(id);
//         context.Response.StatusCode = 204;
//     }
//
//
// }