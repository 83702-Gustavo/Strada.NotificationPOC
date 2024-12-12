namespace Strada.Notification.Application.DTOs;

public class FiltroRelatorioDto
{
    public DateTime? DataInicio { get; set; } // Data inicial do filtro (opcional)
    public DateTime? DataFim { get; set; } // Data final do filtro (opcional)
    public string? NomeApp { get; set; } // Nome do app cliente (opcional)
    public string? NomeProvedor { get; set; } // Nome do provedor (opcional)
    public string? TipoNotificacao { get; set; } // Tipo de notificação (e.g., SMS, Email) (opcional)
}