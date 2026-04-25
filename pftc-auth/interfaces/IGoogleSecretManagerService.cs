namespace pftc_auth.interfaces;

public interface IGoogleSecretManagerService
{
    Task<string> getSecretAsync(string secretName);
    Task LoadSecretsIntoConfigurationAsync(IConfiguration config); 
}