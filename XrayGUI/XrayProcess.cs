using System.Diagnostics;
using System.IO;

namespace XrayGUI
{
    public class XrayProcess
    {
        private Process? _process;

        public bool Running => _process != null && !_process.HasExited;

        public void Start()
        {
            string folder = Path.Combine(AppContext.BaseDirectory, "Core");

            _process = new Process();

            _process.StartInfo.FileName = Path.Combine(folder, "xray.exe");

            _process.StartInfo.Arguments = "-config config.json";

            _process.StartInfo.WorkingDirectory = folder;

            _process.StartInfo.UseShellExecute = false;

            _process.StartInfo.CreateNoWindow = true;

            _process.StartInfo.RedirectStandardOutput = true;

            _process.StartInfo.RedirectStandardError = true;

            _process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null)
                    Console.WriteLine(e.Data);
            };

            _process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null)
                    Console.WriteLine(e.Data);
            };

            _process.Start();

            _process.BeginOutputReadLine();

            _process.BeginErrorReadLine();
        }

        public void Stop()
        {
            if (_process == null)
                return;

            if (!_process.HasExited)
                _process.Kill(true);

            _process.Dispose();

            _process = null;
        }
    }
}
