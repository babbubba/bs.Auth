using bs.Auth.Api.AuthMock;
using bs.Auth.Api.Repositories;
using bs.Auth.Interfaces.Models;
using bs.Auth.Interfaces.Services;
using bs.Auth.Models;
using bs.Data;
using bs.Data.Interfaces;
using bs.Datatable.Services;
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

// First set security model
var security = new AppSecuritySettingsModel
{
    JwtRefreshTokenValidityDays= 1,
    JwtTokenValidityMinutes = 60,
    Secret = "chiave-segreta-da-cambiare-in-produzione",
    ValidateAudience = false,
    ValidateIssuer = false,
    ValidAudience = "dummy-audience",
    ValidIssuer = "dummy-issuer"
};

builder.Services.AddSingleton<IAppSecuritySettingsModel>(security);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<CustomersRepository>();
builder.Services.AddScoped<PaginatorService>();


IDbContext dbContext = new DbContext
{
    ConnectionString = "Data Source=.\\Data.db;Version=3;BinaryGuid=False;",
    DatabaseEngineType = DbType.SQLite,
    Create = false,
    Update = true,
    LookForEntitiesDllInCurrentDirectoryToo = false,
    SetBatchSize = 25
};

// Register the O.R.M. and related models mapping
builder.Services.AddBsData(dbContext);

builder.Services.AddSingleton(dbContext);



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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
