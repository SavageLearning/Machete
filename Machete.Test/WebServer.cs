#region COPYRIGHT
// File:     WebServer.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Test
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Machete.Test
{
    // Stops and starts web server for Selenium integration tests
    public class WebServer
    {
        private readonly string iisPath;
        private readonly int iisPort;
        private readonly string virtualDirectory;
        private Process iisXProcess;

        public WebServer(string physicalPath, int port)
            : this(physicalPath, port, "")
        {
        }

        public WebServer(string physicalPath, int port, string virtualDirectory)
        {
            this.iisPort = port;
            this.iisPath = physicalPath.TrimEnd('\\');
            this.virtualDirectory = virtualDirectory;
        }

        public void Start()
        {
            ProcessStartInfo _psi = new ProcessStartInfo()
            {
                ErrorDialog = false,
                CreateNoWindow = true,
                UseShellExecute = false,
                Arguments = string.Format("/path:\"{0}\" /port:{1}", this.iisPath, this.iisPort, virtualDirectory)
            };
            
            string iisServerPath = (!string.IsNullOrEmpty(_psi.EnvironmentVariables["programfiles(x86)"]) ? _psi.EnvironmentVariables["programfiles(x86)"] : _psi.EnvironmentVariables["programfiles"]) + "\\IIS Express\\iisexpress.exe";

            _psi.FileName = iisServerPath;

            this.iisXProcess = new Process() { StartInfo = _psi };
            
            this.iisXProcess.Start();
        }

        public void Stop()
        {
            if (this.iisXProcess == null)
            {
                throw new InvalidOperationException("Start() must be called before Stop()");
            }

            this.iisXProcess.Kill();
        }
    }
}
