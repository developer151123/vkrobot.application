using Carter;
using Carter.OpenApi;
using vkborot.application.dto;
using vkrobot.application.exceptions;
using vkrobot.application.interfaces;

namespace vkrobot.application.webapi.Modules
{
    public class GroupModule : ICarterModule
    {
        private const string APICorsPolicy = "APICorsPolicy";
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("groups", async (IGroupService service) =>
            {
                try
                {
                    var result = await service.GetAsync();
                    return Results.Ok(result);
                }
                catch (DataException)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
             .Produces<IEnumerable<GroupDto>>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Groups")
             .IncludeInOpenApi()
             .RequireCors(APICorsPolicy);

            app.MapGet("groups/{id}", async (Guid id, IGroupService service) =>
            {
                try
                {
                    var result = await service.GetAsync(id);
                    return Results.Ok(result);
                }
                catch (NotFoundException)
                {
                    return Results.StatusCode(StatusCodes.Status404NotFound);
                }
                catch (DataException)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }

            })
            .Produces<GroupDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Groups")
            .IncludeInOpenApi()
            .RequireCors(APICorsPolicy);

            app.MapPut("groups/{id}", async (Guid id,GroupDto dto, IGroupService service) =>
            {
                try
                {
                    await service.UpdateAsync(id, dto);
                    return Results.Ok();
                }
                catch (NotFoundException)
                {
                    return Results.StatusCode(StatusCodes.Status404NotFound);
                }
                catch (DataException)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
                .Accepts<GroupDto>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithTags("Groups")
                .IncludeInOpenApi()
                .RequireCors(APICorsPolicy);
            
            app.MapPost("groups", async (GroupDto dto, IGroupService service) =>
                {
                    try
                    {
                        await service.CreateAsync(dto);
                        return Results.Ok();
                    }
                    catch (NotFoundException)
                    {
                        return Results.StatusCode(StatusCodes.Status404NotFound);
                    }
                    catch (DataException)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }
                })
                .Accepts<GroupDto>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithTags("Groups")
                .IncludeInOpenApi()
                .RequireCors(APICorsPolicy);
            
            app.MapDelete("groups/{id}", async (Guid id, IGroupService service) =>
                {
                    try
                    {
                        await service.DeleteAsync(id);
                        return Results.Ok();
                    }
                    catch (NotFoundException)
                    {
                        return Results.StatusCode(StatusCodes.Status404NotFound);
                    }
                    catch (DataException)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }

                })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithTags("Groups")
                .IncludeInOpenApi()
                .RequireCors(APICorsPolicy);
        }
    }
}