using FirmenpartnerBackend.Models.Data;

namespace FirmenpartnerBackend.Configuration
{
    public class MailSettings
    {
        public const string HEADER_LOGO = "header_logo";
        public const string HEADER_BG_COLOR = "header_bg_color";
        public const string HEADER_BG_IMAGE = "header_bg_image";

        public const string BODY_BG_COLOR = "body_bg_color";
        public const string BODY_COLOR = "body_color";

        public const string FOOTER_BG_COLOR = "footer_bg_color";
        public const string FOOTER_COLOR = "footer_color";
        public const string FOOTER_TEXT = "footer_text";

        public const string MAIL_BG_COLOR = "mail_bg_color";

        public static readonly List<MailSetting> DefaultMailSettings = new List<MailSetting>()
        {
            new() { Id = MailSettings.HEADER_LOGO, Value = "" },
            new() { Id = MailSettings.HEADER_BG_COLOR, Value = "#ffffff" },
            new() { Id = MailSettings.HEADER_BG_IMAGE, Value = "" },

            new() { Id = MailSettings.BODY_BG_COLOR, Value = "#ffffff" },
            new() { Id = MailSettings.BODY_COLOR, Value = "#0000000" },

            new() { Id = MailSettings.FOOTER_BG_COLOR, Value = "#5c636a" },
            new() { Id = MailSettings.FOOTER_COLOR, Value = "#ffffff" },
            new() { Id = MailSettings.FOOTER_TEXT, Value = "" },

            new() { Id = MailSettings.MAIL_BG_COLOR, Value = "#ffffff" },
        };
    }
}
