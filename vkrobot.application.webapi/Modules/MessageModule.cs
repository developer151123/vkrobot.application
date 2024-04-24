

using System.Text.Json;
using Carter;
using Carter.OpenApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using vkborot.application.dto;
using vkrobot.application.exceptions;
using vkrobot.application.interfaces;

namespace vkrobot.application.webapi.Modules
{
    public class MessageModule : ICarterModule
    {
        private const string APICorsPolicy = "APICorsPolicy";
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("messages", async ([FromQuery]string filter, int? skip, int? take, IMessageService service) =>
            {
                try
                {
                    var filterDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(filter);
                    if (filterDictionary == null)
                        return Results.BadRequest();
                    var result = await service.GetListAsync(filterDictionary, skip, take);
                    return Results.Ok(result);
                }   
                catch (DataException)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
             .Produces<IEnumerable<MessageDto>>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags("Messages")
             .IncludeInOpenApi()
             .RequireCors(APICorsPolicy);

            app.MapGet("messages{id}", async (Guid id, IMessageService service) =>
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
            .Produces<MessageDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Messages")
            .IncludeInOpenApi()
            .RequireCors(APICorsPolicy);
            
            app.MapPost("messages", async (MessageDto dto, IMessageService service) =>
                {
                    try
                    {
                        await service.AddAsync(dto);
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
                .Accepts<MessageDto>("application/json")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithTags("Messages")
                .IncludeInOpenApi()
                .RequireCors(APICorsPolicy);
            
        }
    }
}