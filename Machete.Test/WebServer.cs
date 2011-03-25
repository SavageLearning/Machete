using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Machete.Test
{
    // Stops and starts web server for WatiN integration tests
    public class WebServer
    {
        private readonly string physicalPath;
        private readonly int port;
        private readonly string virtualDirectory;
        private Process webServerProcess;

        public WebServer(string physicalPath, int port)
            : this(physicalPath, port, "")
        {
        }

        public WebServer(string physicalPath, int port, string virtualDirectory)
        {
            this.port = port;
            this.physicalPath = physicalPath.TrimEnd('\\');
            this.virtualDirectory = virtualDirectory;
        }

        public void Start()
        {
            webServerProcess = new Process();
            //const string webDevServerPath = @"c:\Windows\Microsoft.NET\Framework\v2.0.50727\WebDev.WebServer.exe";
            const string webDevServerPath = @"C:\Program Files (x86)\Common Files\Microsoft Shared\DevServer\10.0\WebDev.WebServer40.exe";
            string arguments = string.Format("/port:{0} /path:\"{1}\" /vpath:{2}", port, physicalPath, virtualDirectory);
            webServerProcess.StartInfo = new ProcessStartInfo(webDevServerPath, arguments);
            webServerProcess.Start();
        }

        public void Stop()
        {
            if (webServerProcess == null)
            {
                throw new InvalidOperationException("Start() must be called before Stop()");
            }
            webServerProcess.Kill();
        }
    }
}
