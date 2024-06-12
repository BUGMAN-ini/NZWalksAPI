using System.Net;

namespace NZWalksAPI.MiddleWare
{
    public class ExceptionHandlerMiddleware
    {

        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate requestDelegate)
        {
            _logger = logger;
            next = requestDelegate;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                //Log This EXception
                //return CustomErrorResponse;
                var errorid = Guid.NewGuid();
                _logger.LogError(ex, $"{errorid} : {ex.Message}");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorid,
                    ErrorMessage = "Something Went Wrong",
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }

        }
    }
}
