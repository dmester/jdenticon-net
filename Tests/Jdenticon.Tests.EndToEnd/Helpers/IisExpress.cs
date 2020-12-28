using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jdenticon.Tests.EndToEnd.Helpers
{
    public class IisExpress : IDisposable
    {
        private Process process;

        public IisExpress(string appPath, Action<string, bool> logger)
        {
            Port = GetFreePort();

            StartIis(appPath, logger);

            if (!WaitForListening(15000))
            {
                try
                {
                    process.Kill();
                    process.Dispose();
                    process = null;
                }
                catch
                {
                    // Best effort
                }

                throw new TimeoutException("IIS Express startup timed out.");
            }
        }

        public int Port { get; private set; }

        private void StartIis(string appPath, Action<string, bool> logger)
        {
            var iisExpressPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "IIS Express", "iisexpress.exe");

            var arguments = "/path:\"" + appPath + "\" /port:" + Port + " /systray:false";

            process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = iisExpressPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                }
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data != null) logger(e.Data, false);
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null) logger(e.Data, true);
            };

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
        }

        private bool WaitForListening(int timeoutMilliseconds)
        {
            if (PortIsListening(Port))
            {
                return true;
            }

            const int intervalMilliseconds = 250;
            var iterations = timeoutMilliseconds / intervalMilliseconds;

            for (var i = 0; i < iterations; i++)
            {
                Thread.Sleep(intervalMilliseconds);

                if (PortIsListening(Port))
                {
                    return true;
                }
            }

            return false;
        }

        private int GetFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            try
            {
                listener.Start();
                return ((IPEndPoint)listener.LocalEndpoint).Port;
            }
            finally
            {
                listener.Stop();
            }
        }

        private bool PortIsListening(int port)
        {
            using (var client = new TcpClient())
            {
                try
                {
                    return client.ConnectAsync("localhost", port).Wait(2000);
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Dispose()
        {
            process.Kill();
            process.Close();
        }
    }
}
