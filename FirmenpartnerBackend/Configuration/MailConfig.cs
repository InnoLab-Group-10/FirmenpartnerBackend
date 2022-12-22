namespace FirmenpartnerBackend.Configuration
{
    public class MailConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderMail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TemplatePath { get; set; }
    }
}
