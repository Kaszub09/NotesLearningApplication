using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NotesLearningApplication.Server.Middleware {
    public class ErrorHandlingMiddleware : IMiddleware {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            try {
                await next.Invoke(context);
            }
            catch(KeyNotFoundException e) {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"Error: {e.Message}");
            } catch (ArgumentException e) {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"Error: {e.Message}");
            } catch (Exception e) {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"unexpected error");
            }

        }
    }
}
