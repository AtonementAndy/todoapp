using TodoApp.Application.Common;

namespace TodoApp.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
                return Results.Ok();

            return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "One or more validations errors occurred.",
                extensions: new Dictionary<string, object?>
                {
                    { "errors", result.Errors.Select(e => new { e.Code, e.Description }) }
                }
            );
        }
    }
}
