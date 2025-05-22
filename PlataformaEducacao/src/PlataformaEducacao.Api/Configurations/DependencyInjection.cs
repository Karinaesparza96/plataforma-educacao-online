using MediatR;
using PlataformaEducacao.Core.DomainObjects;
using PlataformaEducacao.Core.Messages;
using PlataformaEducacao.GestaoAlunos.Aplication.Commands;
using PlataformaEducacao.GestaoAlunos.Aplication.Queries;
using PlataformaEducacao.GestaoAlunos.Aplication.Services;
using PlataformaEducacao.GestaoAlunos.Data.Context;
using PlataformaEducacao.GestaoAlunos.Data.Repositories;
using PlataformaEducacao.GestaoAlunos.Domain;
using PlataformaEducacao.GestaoConteudos.Aplication.Commands;
using PlataformaEducacao.GestaoConteudos.Aplication.Queries;
using PlataformaEducacao.GestaoConteudos.Data.Context;
using PlataformaEducacao.GestaoConteudos.Data.Repositories;
using PlataformaEducacao.GestaoConteudos.Domain;
using PlataformaEducacao.Pagamentos.AntiCorruption;
using PlataformaEducacao.Pagamentos.Business;
using PlataformaEducacao.Pagamentos.Data.Context;
using PlataformaEducacao.Pagamentos.Data.Repository;

namespace PlataformaEducacao.Api.Configurations;

public static class DependencyInjection
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        // Notifications
        builder.Services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        // Gestão de Alunos
        builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<IAlunoQueries, AlunoQueries>();
        builder.Services.AddScoped<ICertificadoPdfService, CertificadoService>();
        builder.Services.AddScoped<GestaoAlunosContext>();

        // Gestão de Conteúdos
        builder.Services.AddScoped<ICursoRepository, CursoRepository>();
        builder.Services.AddScoped<ICursoQueries, CursoQueries>();
        builder.Services.AddScoped<IAulaRepository, AulaRepository>();
        builder.Services.AddScoped<GestaoConteudosContext>();

        // Mediator
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AdicionarAlunoCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AdicionarAulaCommand>());
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PagamentoService>());

        // Pagamentos
        builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        builder.Services.AddScoped<IPagamentoService, PagamentoService>();
        builder.Services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
        builder.Services.AddScoped<IPayPalGateway, PayPalGateway>();
        builder.Services.AddScoped<PagamentoContext>();
        builder.Services.Configure<PagamentoSettings>(builder.Configuration.GetSection("Pagamentos"));

        return builder;

    }
}