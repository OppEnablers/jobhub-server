using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

using models;

var builder = WebApplication.CreateBuilder(args);
var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "FireBaseServiceKey.json");
System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton(FirestoreDb.Create("jobhub-a52ac"));

var app = builder.Build();

// Initialize Firebase Admin SDK
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.GetApplicationDefault()
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/", () => "good")
    .WithName("IndexRoute");

app.MapGet("/v1/company", async (FirestoreDb firestoreDb) =>
{
    // Get from Firestore Database /company
    var companiesRef = firestoreDb.Collection("company");
    var snapshot = await companiesRef.GetSnapshotAsync();

    var companies = snapshot.Documents.Select(doc => doc.ConvertTo<Company>()).ToList();
    return Results.Ok(companies);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
