using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Regs.Infrastructure {
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app) => app.UseExceptionHandler(builder => builder.Run(async context => await HandleError(context)));

        private static async Task HandleError(HttpContext context)
        {
            var exHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
            var ex = exHandlerFeature?.Error;
            var (message, statusCode) = ex.GenerateMessageAndStatusCode();

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = MediaTypeNames.Text.Plain;
            await context.Response.WriteAsync(message);
        }

        private static (string message, HttpStatusCode statusCode) GenerateMessageAndStatusCode(this Exception ex)
        {
            string message;
            var statusCode = HttpStatusCode.InternalServerError;

            switch (ex)
            {
                case null:
                    message = "Unknown Error Occured";
                    break;
                case TaskCanceledException _:
                    message = "Request to host timed out, please try again later";
                    statusCode = HttpStatusCode.GatewayTimeout;
                    break;
                case HttpRequestException hre when (hre.InnerException is SocketException se && se.SocketErrorCode == SocketError.HostNotFound):
                    message = "Unable to reach host, please verify connection to the internet and try again.";
                    statusCode = HttpStatusCode.GatewayTimeout;
                    break;
                case UnauthorizedAccessException uae:
                    message = uae.Message;
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                default:
                    message = ex.Message;
                    break;
            }

            return (message, statusCode);
        }
    }
}