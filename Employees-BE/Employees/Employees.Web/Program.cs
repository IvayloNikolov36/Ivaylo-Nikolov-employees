using Employees.Services.Contracts;
using Employees.Services.CSVHelper;
using Employees.Services.Implementations;
using Employees.Web.Infrastructure.Extensions;
using Employees.Web.Infrastructure.Middlewares;
using static Employees.Web.Infrastructure.WebConstants;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<ICsvParserService, CsvParserService>();
builder.Services.AddScoped<IEmployesImportService, EmployesImportService>();

builder.Services.AddCors(corsOptions => corsOptions.Configure());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
