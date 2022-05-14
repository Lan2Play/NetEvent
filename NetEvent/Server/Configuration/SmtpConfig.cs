namespace NetEvent.Server.Configuration
{
    public class SmtpConfig
    {
        public string EmailSenderAddress { get; set; } = default!;

        public string Host { get; set; } = default!;

        public string Username { get; set; } = default!;

        public string Password { get; set; } = default!;

        public int Port { get; set; }
    }
}
