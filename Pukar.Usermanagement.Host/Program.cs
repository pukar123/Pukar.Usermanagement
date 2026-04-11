using Microsoft.OpenApi.Models;
using Pukar.Usermanagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddPukarUserManagementControllers();
builder.Services.AddPukarUserManagementApi(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pukar User Management",
        Version = "v1",
        Description = "JWT auth, refresh tokens, and admin RBAC. Use POST /api/auth/login, then Authorize with Bearer token.",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
            },
            Array.Empty<string>()
        },
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Pukar User Management v1");
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
