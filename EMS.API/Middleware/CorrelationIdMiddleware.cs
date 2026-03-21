using System.Diagnostics;
using Serilog.Context;

namespace EMS.API.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault()
            ?? Guid.NewGuid().ToString("N");

        context.Response.Headers.Append(HeaderName, correlationId);
        context.Items["CorrelationId"] = correlationId;

        var traceId = Activity.Current?.TraceId.ToString();
        var path = context.Request.Path.Value;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("TraceId", traceId))
        using (LogContext.PushProperty("RequestPath", path))
        {
            await next(context);
        }
    }
}
