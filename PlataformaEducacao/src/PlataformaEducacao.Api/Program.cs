using System.Diagnostics;
using PlataformaEducacao.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

//Debugger.Launch();
builder.AddDbContextConfiguration()
        .AddApiConfiguration()
        .RegisterServices()
        .AddJwtConfiguration()
        .AddSwaggerConfiguration();

var app = builder.Build();

var enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger");

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("*");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseDbMigrationHelper();

app.Run();

public partial class Program { }
