using EMS.API.Middleware;
using EMS.Domain.Database;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, _, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);

        var mongoLogs = context.Configuration.GetConnectionString("MongoLogs");
        if (!string.IsNullOrWhiteSpace(mongoLogs))
        {
            // v7 sink: database from URL path (ems-logs); default collection name is "log"
            configuration.WriteTo.MongoDBBson(mongoLogs);
        }
    });

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name!)));

    var mongoLogsCs = builder.Configuration.GetConnectionString("MongoLogs");
    var healthChecks = builder.Services.AddHealthChecks()
        .AddDbContextCheck<AppDbContext>(name: "database");

    if (!string.IsNullOrWhiteSpace(mongoLogsCs))
    {
        builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoLogsCs));
        var mongoDbName = MongoUrl.Create(mongoLogsCs).DatabaseName ?? "ems-logs";
        healthChecks.AddMongoDb(
            clientFactory: sp => sp.GetRequiredService<IMongoClient>(),
            databaseNameFactory: _ => mongoDbName,
            name: "mongodb");
    }

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseMiddleware<CorrelationIdMiddleware>();

    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? "");
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
            if (httpContext.Items.TryGetValue("CorrelationId", out var cid) && cid is string s)
                diagnosticContext.Set("CorrelationId", s);
        };
    });

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapHealthChecks("/health");
    app.MapControllers();

    Log.Information("EMS.API starting ({Environment})", app.Environment.EnvironmentName);

    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
