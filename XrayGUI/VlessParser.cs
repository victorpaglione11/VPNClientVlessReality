using System.Web;

namespace XrayGUI
{
    public static class VlessParser
    {
        public static VlessProfile Parse(string uri)
        {
            var u = new Uri(uri);

            var q = HttpUtility.ParseQueryString(u.Query);

            return new VlessProfile
            {
                Id = Guid.Parse(u.UserInfo),
                Host = u.Host,
                Port = u.Port,

                PublicKey = q["pbk"] ?? "",
                ShortId = q["sid"] ?? "",
                Fingerprint = q["fp"] ?? "chrome",
                ServerName = q["sni"] ?? "",
                Flow = q["flow"] ?? "",
                Network = q["type"] ?? "tcp",
                Security = q["security"] ?? "reality",

                Remark = Uri.UnescapeDataString(u.Fragment.TrimStart('#'))
            };
        }
    }
}
