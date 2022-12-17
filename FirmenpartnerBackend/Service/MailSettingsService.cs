using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;

namespace FirmenpartnerBackend.Service
{
    public class MailSettingsService : IMailSettingsService
    {
        private readonly ApiDbContext dbContext;

        public MailSettingsService(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<MailSetting> GetSetting(string key)
        {
            MailSetting? entry = await dbContext.MailSettings.FindAsync(key);

            if (entry != null)
            {
                return entry;
            }
            else
            {
                throw new KeyNotFoundException($"Could not find a settings entry with key {key}.");
            }
        }

        public async Task<MailSetting> SetSetting(string key, string value)
        {
            MailSetting? entry = await dbContext.MailSettings.FindAsync(key);

            if (entry != null)
            {
                entry.Value = value;
                await dbContext.SaveChangesAsync();
                return entry;
            }
            else
            {
                throw new KeyNotFoundException($"Could not find a settings entry with key {key}.");
            }
        }
    }
}
