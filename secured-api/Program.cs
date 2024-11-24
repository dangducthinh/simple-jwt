using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SecuredApi;
using SecuredApi.AppSetting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Secured API", Version = "v1" });

    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter a valid token"
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, new string[] { } }
    };

    options.AddSecurityRequirement(securityRequirement);

    // Add OperationFilter to include token in requests
    options.OperationFilter<SwaggerBearerAuthOperationFilter>();
});
builder.Services.AddControllers();

// Bind JWT settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null) jwtSettings = new();
builder.Services.AddSingleton(jwtSettings);

var jwtVerifyKey = File.ReadAllText(@"C:\RSA_key\verifyKey.pem");
var rsa = RSA.Create();
rsa.ImportFromPem(jwtVerifyKey.ToCharArray());
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new RsaSecurityKey(rsa),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = false,
        ValidateLifetime = true
    };

    x.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var token = context.SecurityToken as JwtSecurityToken;
            await FileLogger.LogAsync($"Token vailidation passed with access token: {token?.RawData}");
        },
        OnAuthenticationFailed = async context =>
        {
            string tokenString = string.Empty;
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                tokenString = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }
            var message = context.Exception is SecurityTokenExpiredException
                ? $"Token expired: {context.Exception.Message} at {DateTime.Now.ToLongTimeString()}. Token: {tokenString}"
                : $"Token validation failed: {context.Exception.Message} at {DateTime.Now.ToLongTimeString()}. Token: {tokenString}";
            await FileLogger.LogAsync(message);
        },
        OnChallenge = async context =>
        {
            if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
                await FileLogger.LogAsync($"Request not include token!");
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Add redirection to Swagger UI
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
        }
        else
        {
            await next();
        }
    });
}
app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
