namespace Strada.Notification.Domain.Entities;

public class Relatorio
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Tipo { get; private set; } // Geral, Por Tipo, Por Provedor
    public DateTime DataCriacao { get; private set; }
    public string CaminhoArquivo { get; private set; } // Local onde o arquivo foi salvo

    public Relatorio(string nome, string tipo, string caminhoArquivo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do relatório não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("O tipo do relatório não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(caminhoArquivo))
            throw new ArgumentException("O caminho do arquivo não pode ser nulo ou vazio.");

        Id = Guid.NewGuid();
        Nome = nome;
        Tipo = tipo;
        DataCriacao = DateTime.UtcNow;
        CaminhoArquivo = caminhoArquivo;
    }
}