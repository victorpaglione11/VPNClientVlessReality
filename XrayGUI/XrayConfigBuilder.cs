using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XrayGUI
{
    public static class XrayConfigBuilder
    {
        public static string Build(VlessProfile p)
        {
            var streamSettings = new JObject
            {
                ["network"] = p.Network,
                ["security"] = p.Security
            };

            if (p.Security?.ToLower() == "reality")
            {
                streamSettings["realitySettings"] = new JObject
                {
                    ["serverName"] = p.ServerName,
                    ["fingerprint"] = p.Fingerprint,
                    ["publicKey"] = p.PublicKey,
                    ["shortId"] = p.ShortId
                };
            }

            var config = new JObject
            {
                ["log"] = new JObject
                {
                    ["loglevel"] = "warning"
                },

                ["inbounds"] = new JArray
                {
                    new JObject
                    {
                        ["listen"] = "127.0.0.1",
                        ["port"] = 10808,
                        ["protocol"] = "socks",
                        ["settings"] = new JObject
                        {
                            ["udp"] = true
                        }
                    }
                },

                ["outbounds"] = new JArray
                {
                    new JObject
                    {
                        ["protocol"] = "vless",
                        ["settings"] = new JObject
                        {
                            ["vnext"] = new JArray
                            {
                                new JObject
                                {
                                    ["address"] = p.Host,
                                    ["port"] = p.Port,
                                    ["users"] = new JArray
                                    {
                                        new JObject
                                        {
                                            ["id"] = p.Id.ToString(),
                                            ["encryption"] = "none",
                                            ["flow"] = p.Flow
                                        }
                                    }
                                }
                            }
                        },
                        ["streamSettings"] = streamSettings
                    }
                }
            };

            return config.ToString(Formatting.Indented);
        }
    }
}