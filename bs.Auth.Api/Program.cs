using bs.Auth.Api.AuthMock;
using bs.Auth.Interfaces.Models;
using bs.Auth.Interfaces.Services;
using bs.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var corsProfile = "_corsProfile";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsProfile,
                      builder =>
                      {
                          builder.WithOrigins("http://example.com",
                                              "http://localhost:4200");
                          builder.AllowAnyHeader();
                          builder.AllowAnyMethod();
                      });
});

// Add services to the container.


// First set security model
var security = new AppSecuritySettingsModel
{
    JwtTokenValidityMinutes = 60,
    Secret = "123456789012345678901234567890123456",
    ValidateAudience = false,
    ValidateIssuer = false
};

builder.Services.AddSingleton<IAppSecuritySettingsModel>(security);
builder.Services.AddScoped<IAuthService, AuthService>();

// Setting authentication using JWT Token in request header
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(options =>
     {
         options.SaveToken = true;
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters = new TokenValidationParameters()
         {
             ValidateIssuer = security.ValidateIssuer,
             ValidateAudience = security.ValidateAudience,
             ValidAudience = security.ValidAudience,
             ValidIssuer = security.ValidIssuer,
             ClockSkew = TimeSpan.Zero,// It forces tokens to expire exactly at token expiration time instead of 5 minutes later
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(security.Secret))
         };
     });

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(corsProfile);

app.UseAuthorization();

app.MapControllers();

app.Run();
