using Infraestructura.Excepciones;
using System.Net;
using System.Text.Json;

namespace ProductoCatalogo.Meddlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = "Ocurrió un error interno en el servidor.";

            switch (exception)
            {
                case DomainExcepciones:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var response = new
            {
                status = context.Response.StatusCode,
                error = message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
