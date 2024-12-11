using Strada.Notification.Domain.Enums;

namespace Strada.Notification.Domain.Entities;

public class Notificacao
{
    public Guid Id { get; private set; }
    public string Destinatario { get; private set; }
    public string Mensagem { get; private set; }
    public TipoNotificacao Tipo { get; private set; }
    public DateTime DataCriacao { get; private set; }

    public Notificacao(string destinatario, string mensagem, TipoNotificacao tipo)
    {
        if (string.IsNullOrWhiteSpace(destinatario))
            throw new ArgumentException("O destinatário não pode ser nulo ou vazio.");

        if (string.IsNullOrWhiteSpace(mensagem))
            throw new ArgumentException("A mensagem não pode ser nula ou vazia.");

        if (mensagem.Length > 500)
            throw new ArgumentException("A mensagem não pode exceder 500 caracteres.");

        Id = Guid.NewGuid();
        Destinatario = destinatario;
        Mensagem = mensagem;
        Tipo = tipo;
        DataCriacao = DateTime.UtcNow;
    }

    public void AtualizarMensagem(string novaMensagem)
    {
        if (string.IsNullOrWhiteSpace(novaMensagem))
            throw new ArgumentException("A mensagem não pode ser nula ou vazia.");

        if (novaMensagem.Length > 500)
            throw new ArgumentException("A mensagem não pode exceder 500 caracteres.");

        Mensagem = novaMensagem;
    }
}