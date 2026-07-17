using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace XrayGUI
{
    public class WindowsProxyService
    {
        private const string RegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";

        [DllImport("wininet.dll")]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_REFRESH = 37;


        public void EnableProxy(string host = "127.0.0.1", int port = 10808)
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);

            if (key == null)
                throw new Exception("Não foi possível acessar o registro");

            key.SetValue("ProxyEnable", 1);
            key.SetValue("ProxyServer", $"{host}:{port}");
            Refresh();
        }

        public void DisableProxy()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);

            if (key == null)
                return;

            key.SetValue("ProxyEnable", 0);

            Refresh();
        }

        private void Refresh()
        {
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);

            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }
    }

}