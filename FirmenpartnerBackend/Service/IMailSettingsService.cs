using FirmenpartnerBackend.Models.Data;

namespace FirmenpartnerBackend.Service
{
    public interface IMailSettingsService
    {
        Task<MailSetting> GetSetting(string key);
        Task<MailSetting> SetSetting(string key, string value);
    }
}
