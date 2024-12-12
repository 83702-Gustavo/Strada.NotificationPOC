namespace Strada.Notification.API.DTOs.Request;

public class AtualizarSegurancaDto
{
    public int TempoExpiracaoJwt { get; set; }
    public bool LogElasticSearch { get; set; }
    public string NivelLog { get; set; }
}