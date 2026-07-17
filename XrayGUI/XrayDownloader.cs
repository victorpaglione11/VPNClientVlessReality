using System.IO;
using System.IO.Compression;
using System.Net.Http;

namespace XrayGUI
{
    public class XrayDownloader
    {
        private const string Url = "https://github.com/XTLS/Xray-core/releases/download/v25.7.26/Xray-windows-64.zip";

        private readonly string _coreFolder =
            Path.Combine(AppContext.BaseDirectory, "Core");

        public async Task DownloadAsync()
        {
            Directory.CreateDirectory(_coreFolder);

            string zip = Path.Combine(_coreFolder, "xray.zip");

            using HttpClient http = new();

            var bytes = await http.GetByteArrayAsync(Url);

            await File.WriteAllBytesAsync(zip, bytes);

            ZipFile.ExtractToDirectory(zip, _coreFolder, true);

            File.Delete(zip);
        }

        public async Task EnsureInstalled()
        {
            string exe = Path.Combine(_coreFolder, "xray.exe");

            if (!File.Exists(exe))
                await DownloadAsync();
        }
    }
}
