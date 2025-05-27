using PlataformaEducacao.Pagamentos.AntiCorruption;

namespace PlataformaEducacao.Api.Configurations;

public static class ApiConfiguration
{
    public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<PagamentoSettings>(builder.Configuration.GetSection("Pagamentos"));

        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddCors(opt => opt.AddPolicy("*", b =>
        {
            b.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));
        return builder;
    }
}