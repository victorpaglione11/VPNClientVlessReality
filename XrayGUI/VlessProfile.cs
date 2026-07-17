namespace XrayGUI
{
    public class VlessProfile
    {
        public Guid Id { get; set; }

        public string Host { get; set; } = "";

        public int Port { get; set; }

        public string PublicKey { get; set; } = "";

        public string ShortId { get; set; } = "";

        public string Fingerprint { get; set; } = "";

        public string ServerName { get; set; } = "";

        public string Flow { get; set; } = "";

        public string Network { get; set; } = "tcp";

        public string Security { get; set; } = "reality";

        public string Remark { get; set; } = "";
    }
}
