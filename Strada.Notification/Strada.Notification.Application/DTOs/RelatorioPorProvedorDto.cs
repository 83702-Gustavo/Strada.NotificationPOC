namespace Strada.Notification.Application.DTOs;

public class RelatorioPorProvedorDto
{
    public string NomeProvedor { get; set; } // Nome do provedor (e.g., Twilio, Mailgun)
    public int TotalEnviadas { get; set; } // Total de notificações enviadas pelo provedor
    public int TotalEntregues { get; set; } // Total de notificações entregues com sucesso
    public int TotalFalhas { get; set; } // Total de notificações que falharam
    public double TempoMedioResposta { get; set; } // Tempo médio de resposta do provedor em segundos
}