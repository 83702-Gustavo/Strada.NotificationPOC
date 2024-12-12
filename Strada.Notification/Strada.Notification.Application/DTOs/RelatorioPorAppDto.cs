namespace Strada.Notification.Application.DTOs;

public class RelatorioPorAppDto
{
    public string NomeApp { get; set; } // Nome do aplicativo cliente
    public int TotalEnviadas { get; set; } // Total de notificações enviadas pelo app
    public int TotalEntregues { get; set; } // Total de notificações entregues
    public int TotalFalhas { get; set; } // Total de notificações que falharam
    public double TempoMedioEntrega { get; set; } // Tempo médio de entrega em segundos
}