using Strada.Notification.Application.DTOs;
using Strada.Notification.Domain.Entities;

namespace Strada.Notification.Application.Services;

public class ReportsService
{
    private readonly IEnumerable<Domain.Entities.Notification> _notificacoes; 
    private readonly IEnumerable<AppClient> _appsClientes; 
    private readonly IEnumerable<Provider> _provedores; 

    public ReportsService(IEnumerable<Domain.Entities.Notification> notificacoes, IEnumerable<AppClient> appsClientes, IEnumerable<Provider> provedores)
    {
        _notificacoes = notificacoes;
        _appsClientes = appsClientes;
        _provedores = provedores;
    }

    public IEnumerable<RelatorioPorAppDto> GerarRelatorioPorApp()
    {
        var relatorio = _appsClientes.Select(app => new RelatorioPorAppDto
        {
            NomeApp = app.Nome,
            TotalEnviadas = _notificacoes.Count(n => n.Recipient.StartsWith(app.Nome)), 
            TotalEntregues = _notificacoes.Count(n => n.Recipient.StartsWith(app.Nome) && n.Status == "Entregue"),
            TotalFalhas = _notificacoes.Count(n => n.Recipient.StartsWith(app.Nome) && n.Status.StartsWith("Falha")),
            TempoMedioEntrega = _notificacoes
                .Where(n => n.Recipient.StartsWith(app.Nome) && n.Status == "Entregue")
                .Average(n => (n.DeliveredAt - n.CreatedAt)?.TotalSeconds ?? 0)
        });

        return relatorio;
    }

    public IEnumerable<RelatorioPorProvedorDto> GerarRelatorioPorProvedor()
    {
        var relatorio = _provedores.Select(provedor => new RelatorioPorProvedorDto
        {
            NomeProvedor = provedor.Name,
            TotalEnviadas = _notificacoes.Count(n => n.Provider == provedor.Name),
            TotalEntregues = _notificacoes.Count(n => n.Provider == provedor.Name && n.Status == "Entregue"),
            TotalFalhas = _notificacoes.Count(n => n.Provider == provedor.Name && n.Status.StartsWith("Falha")),
            TempoMedioResposta = _notificacoes
                .Where(n => n.Provider == provedor.Name && n.Status == "Entregue")
                .Average(n => (n.DeliveredAt - n.CreatedAt)?.TotalSeconds ?? 0)
        });

        return relatorio;
    }
}