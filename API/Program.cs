using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Build.Security.AspNetCore.Middleware.Extensions;
using Build.Security.AspNetCore.Middleware.Request;
using API;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Read values from appsettings.json
var jwtAuthority = builder.Configuration["Jwt:Authority"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var corsOrigin = builder.Configuration["Cors:Origin"];

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = jwtAuthority;
    options.Audience = jwtAudience;

});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Add OPA integration
builder.Services.AddBuildAuthorization(options =>
{
    options.Enable = true;
    options.BaseAddress = "http://192.168.106.209:8181";
    options.PolicyPath = "/barmanagement/allow";
    options.AllowOnFailure = false;
    options.Timeout = 5;
// Option to give the headers by default in the requests
    options.IncludeHeaders = true;
});

// Register your request enricher
builder.Services.AddSingleton<IRequestEnricher, SampleRequestEnricher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(options => options
    .WithOrigins(corsOrigin)
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();

// app.UseAuthorization();
//use the OPA authorization
app.UseBuildAuthorization();

app.MapControllers();
app.Run();
