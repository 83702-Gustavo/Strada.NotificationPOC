namespace Strada.Notification.Domain.Entities;

public class AppClient
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string ChaveApi { get; private set; } // Usada para autenticação
    public int LimiteNotificacoes { get; private set; } // Exemplo de limite diário
    public DateTime DataCriacao { get; private set; }

    public AppClient(string nome, string chaveApi, int limiteNotificacoes)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do aplicativo cliente não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(chaveApi))
            throw new ArgumentException("A chave da API não pode ser nula ou vazia.");

        Id = Guid.NewGuid();
        Nome = nome;
        ChaveApi = chaveApi;
        LimiteNotificacoes = limiteNotificacoes;
        DataCriacao = DateTime.UtcNow;
    }

    public void AtualizarLimiteNotificacoes(int novoLimite)
    {
        if (novoLimite < 0)
            throw new ArgumentException("O limite de notificações deve ser maior ou igual a zero.");

        LimiteNotificacoes = novoLimite;
    }
}