using Strada.Notification.Application.DTOs;
using Strada.Notification.Domain.Entities;

namespace Strada.Notification.Application.Interfaces;

public interface IConfigurationService
{
    IEnumerable<Provider> GetProviders();
    Provider ObterProvedorPorNome(string nome);
    void AtivarProvedor(string nome);
    void DesativarProvedor(string nome);
    void AtualizarPrioridadeProvedor(string nome, int prioridade);
    void AdicionarProvedor(Provider novoProvider);
    void RemoverProvedor(string nome);
    ConfigurationLimits GetLimits();
    void AtualizarLimites(UpdateLimitsDto dto);
    SecuritySettings ObterConfiguracoesSeguranca();
    void AtualizarConfiguracoesSeguranca(UpdateSecurityDto dto);
    Dictionary<string, object> ObterConfiguracoesGerais();
}