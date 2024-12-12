namespace Strada.Notification.API.DTOs.Request;

public class AtualizarLimitesDto
{
    public int LimitePorAplicacao { get; set; }
    public int LimitePorProvedor { get; set; }
    public string Intervalo { get; set; }
}