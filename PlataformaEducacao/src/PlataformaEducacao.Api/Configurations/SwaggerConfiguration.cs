using Microsoft.OpenApi.Models;
namespace PlataformaEducacao.Api.Configurations;

public static class SwaggerConfiguration
{
    public static WebApplicationBuilder AddSwaggerConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(opt =>
        {   
            opt.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Plataforma Educação API",
                Description = "Essa API faz parte do Módulo 3 - Arquitetura, Modelagem e Qualidade de Software do MBA DevXpert Full Stack.",
                Contact = new OpenApiContact() { Name = "Karina Esparza", Email = "karinaesparza296@gmail.com" }
            });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Insira o token JWT desta forma: Bearer {seu token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    []
                }
            });
        });

        return builder;
    }
}