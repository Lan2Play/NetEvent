namespace NetEvent.Server.Configuration
{
    public class EmailConfig
    {
        public SmtpConfig? SmtpConfig { get; set; }

        public SendGridConfig? SendGridConfig { get; set; }
    }
}
