using CafeAPI.DataAccess.Cafe;
using CafeAPI.DataAccess.Employee;
using CafeAPI.Service.Cafe;
using CafeAPI.Service.Employee;

var builder = WebApplication.CreateBuilder(args);

// Service Registration
builder.Services.AddScoped<ICafeService, CafeService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();


builder.Services.AddScoped<ICafeDataAccess, CafeDataAccess>();
builder.Services.AddScoped<IEmployeeDataAccess, EmployeeDataAccess>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.WithOrigins("http://localhost:4200", "http://localhost:5173")  
              .AllowCredentials()
              .AllowAnyHeader()
              .SetIsOriginAllowed(_ => true)
              .AllowAnyMethod());
});


// SignalR
builder.Services.AddSignalR();

// JSON Options
builder.Services.AddMvc().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
});

// Session
builder.Services.AddSession();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession(); // Ensure session is used
app.UseCors("CorsPolicy"); // Enable CORS

app.UseAuthentication(); // Use authentication middleware
app.UseAuthorization(); // Use authorization middleware

app.MapControllers();

app.Run();
