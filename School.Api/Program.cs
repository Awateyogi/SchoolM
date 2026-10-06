using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchoolApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Add services
builder.Services.AddControllers();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();// Enable CORS for React frontend
builder.Services.AddScoped<IDivisionRepository, DivisionRepository>();
builder.Services.AddScoped<IMarkRepository, MarkRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IFeeRepository, FeeRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React dev server
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger configuration (only in Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API V1");
        c.RoutePrefix = "swagger";
    });
}


// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

app.UseAuthorization();

// Map controllers
app.MapControllers();


// Minimal test endpoint
app.MapGet("/", () => "API is running!");

// Run the app
app.Run();


////////var builder = WebApplication.CreateBuilder(args);

////////builder.Services.AddControllers();
////////builder.Services.AddEndpointsApiExplorer();
////////builder.Services.AddSwaggerGen();
////////builder.Services.AddSwaggerGen();

////////app.UseSwagger();
////////app.UseSwaggerUI(c =>
////////{
////////    c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API V1");
////////});


////////var app = builder.Build();

////////// Enable Swagger
//////////app.UseSwagger();
//////////app.UseSwaggerUI();

////////app.UseHttpsRedirection();
////////app.UseAuthorization();
////////app.MapControllers();


////////// Minimal test endpoint
////////app.MapGet("/", () => "API is running!");

////////app.Run();

//////var builder = WebApplication.CreateBuilder(args);

//////// Add services

//////builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
//////builder.Services.AddSwaggerGen();           // Generates swagger.json
//////builder.Services.AddControllers();

//////builder.Services.AddCors(options =>
//////{
//////    options.AddPolicy("AllowReactApp",
//////        policy =>
//////        {
//////            policy.WithOrigins("http://localhost:3000") // React dev server
//////                  .AllowAnyHeader()
//////                  .AllowAnyMethod();
//////        });
//////});

//////var app = builder.Build();
//////if (app.Environment.IsDevelopment())
//////{
//////    app.UseSwagger();
//////    app.UseSwaggerUI();
//////}


//////app.UseHttpsRedirection();
//////app.UseCors("AllowReactApp");  // ? add this before UseAuthorization
//////app.UseAuthorization();
//////app.MapControllers();



//////// Enable Swagger UI
////////app.UseSwagger();
////////app.UseSwaggerUI(c =>
////////{
////////    c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API V1");
////////    c.RoutePrefix = string.Empty; // optional: swagger at root URL
////////});


////////app.MapGet("/", () => "API is running!");
//////app.Run();

