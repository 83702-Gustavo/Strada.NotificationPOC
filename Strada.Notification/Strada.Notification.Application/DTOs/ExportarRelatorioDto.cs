namespace Strada.Notification.Application.DTOs;

public class ExportarRelatorioDto
{
    public string TipoRelatorio { get; set; } // Tipo do relatório (e.g., "PorApp", "PorProvedor")
    public string Formato { get; set; } // Formato de exportação (e.g., "PDF", "CSV")
    public FiltroRelatorioDto Filtros { get; set; } // Filtros aplicados ao relatório
}