using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetTimeBackend.Contexts;
using System.Text;
using PetTimeBackend.IRepositories;
using PetTimeBackend.IServices;
using PetTimeBackend.Services;
using PetTimeBackend.Repositories;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using FirebaseAdmin;
using Google.Apis.FirebaseCloudMessaging.v1;

using NUnit.Util;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Hangfire.SqlServer;
using System;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder ()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

string credentialsPath = config["GoogleCloud:ApplicationCredentialsPath"];
string connectionString = config.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services.AddControllers();
System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
builder.Services.AddSingleton(new FCMService(credentialsPath));
builder.Services.AddSingleton<GoogleMapsService>();
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddDbContext<PetTimeContext>(
    item => item.UseSqlServer(connectionString));

builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IVaccinationRepository, VaccinationRepository>();
builder.Services.AddScoped<IVaccinationService, VaccinationService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddSingleton(new FirestoreService("triple-grove-413413"));
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(Directory.GetCurrentDirectory(), credentialsPath))
});

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            UsePageLocksOnDequeue = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddHangfireServer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
app.MapControllers();
app.MapHangfireDashboard();
app.Run();
