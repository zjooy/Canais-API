using Amazon.SQS;
using Canais.Application;
using Canais.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Canais.Infrastructure.Auth;
using System.Text;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

// Registro do DbContext
builder.Services.AddDbContext<CanaisDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CanaisConnection")));

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// habilita suporte para serializar List<T> como jsonb
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Canais Jurídicos API",
        Description = "API para gerenciamento de reclamações recebidas por meio de canais fisicos/sites.",
    });

    var xmlFile = "Canais.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSQS>();

builder.Services.AddApplicationDependency();

//builder.Services.AddHttpClient("LegadoClient", client =>
//{
//    client.BaseAddress = new Uri("https://214fc0a7-1b14-4986-925f-f0d3e1ae4f90.mock.pstmn.io");
//});

builder.Services.AddHttpClient("LegadoClient", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseAddress = configuration["HttpClients:LegadoClient:BaseAddress"];

    client.BaseAddress = new Uri(baseAddress);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("canaisReclamacoes", builder =>
    {
        builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var key = Encoding.ASCII.GetBytes(Key.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
var logFilePath = configuration["Logging:LogFilePath"];

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: outputTemplate)
    .WriteTo.File(
        path: logFilePath,
        rollingInterval: RollingInterval.Day,
        outputTemplate: outputTemplate
    )
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("canaisReclamacoes");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
