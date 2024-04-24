using Carter;
using Carter.OpenApi;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using vkrobot.application.exceptions;
using vkrobot.application.interfaces.CRUD;

namespace vkrobot.application.webapi.Helpers
{
    public abstract class CRUDModule<K, T, S> : ICarterModule where S : ICRUDService<K, T> 
    {
        protected abstract string CORSPolicy { get; }
        protected abstract string Path { get; }
        protected abstract string[] Tags { get; }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(Path, async (S service, DataSourceLoadOptions loadOptions) =>
            {
                try
                {
                    var result = await service.GetWithLoadOptions(loadOptions);
                    return Results.Ok(result);
                }
                catch (DataException)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
             .Accepts<DataSourceLoadOptionsBase>("application/json")
             .Produces<LoadResult>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status500InternalServerError)
             .WithTags(Tags)
             .IncludeInOpenApi()
             .RequireCors(CORSPolicy);

            app.MapGet(Path + "/{id}", async (K id, S service) =>
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
            .Produces<T>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags(Tags)
            .IncludeInOpenApi()
            .RequireCors(CORSPolicy);

            app.MapPost(Path, async (T dto, S service) =>
            {
                try
                {
                    var created = await service.CreateAsync(dto);
                    return Results.Ok(created);
                }
                catch (DataException)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            })
                .Produces<T>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithTags(Tags)
                .IncludeInOpenApi()
                .RequireCors(CORSPolicy);

            app.MapPut(Path + "/{id}",
            async (K id, T dto, S service) =>
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
                catch (ConcurrencyException e)
                {
                    return Results.StatusCode(e.RowDeleted ? StatusCodes.Status410Gone : StatusCodes.Status409Conflict);
                }
            })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status409Conflict)
                .Produces(StatusCodes.Status410Gone)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithTags(Tags)
                .IncludeInOpenApi()
                .RequireCors(CORSPolicy);

            app.MapDelete(Path + "/{id}", async (K id, S service) =>
            {
                try
                {
                    await service.DeleteAsync(id);
                    return Results.Ok();
                }
                catch (ConcurrencyException e)
                {
                    return Results.StatusCode(e.RowDeleted ? StatusCodes.Status410Gone : StatusCodes.Status409Conflict);
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
                .Produces(StatusCodes.Status409Conflict)
                .Produces(StatusCodes.Status410Gone)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithTags(Tags)
                .IncludeInOpenApi()
                .RequireCors(CORSPolicy);
        }
    }
}
