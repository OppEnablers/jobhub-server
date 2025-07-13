
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using JobHubServer.Firebase;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;

namespace JobHubServer
{
    public class Program
    {
        private const string ProjectId = "jobhub-a52ac";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(c =>
                {
                    c.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
                });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Firebase and Firestore
            builder.Services.AddSingleton(FirebaseApp.Create(
                new AppOptions()
                {
                    Credential = GoogleCredential.GetApplicationDefault()
                }));
            builder.Services.AddSingleton(FirestoreDb.Create(ProjectId));

            // Firebase custom token authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, FirebaseAuthHandler>(JwtBearerDefaults.AuthenticationScheme, c => { });
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("JobSeekers", policy => policy.RequireClaim("user_type", ["jobseeker"]))
                .AddPolicy("Employer", policy => policy.RequireClaim("user_type", ["employer"]));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
