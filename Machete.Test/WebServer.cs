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
using System.Threading;
using System.Threading.Tasks;
namespace Machete.Test
{
    public static class WebServer
    {
        private static Process _iisProcess;
        public static void StartIis()
        {
            if (_iisProcess == null)
            {
                var thread = new Thread(StartIisExpress) { IsBackground = true }; 
                thread.Start();
            }
        }
        private static void StartIisExpress()
        {
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = true,
                LoadUserProfile = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                Arguments = string.Format("/path:\"{0}\" /port:{1}", @"C:\Users\Administrator\Documents\Machete\Machete.Web\obj\publish", "4213")
            };

            var programfiles = string.IsNullOrEmpty(startInfo.EnvironmentVariables["programfiles"])
                ? startInfo.EnvironmentVariables["programfiles(x86)"]
                : startInfo.EnvironmentVariables["programfiles"];
            startInfo.FileName = programfiles + "\\IIS Express\\iisexpress.exe";
            try
            {
                _iisProcess = new Process { StartInfo = startInfo };
                _iisProcess.Start();
                _iisProcess.WaitForExit();
            }
            catch
            {
                _iisProcess.CloseMainWindow();
                _iisProcess.Dispose();
            }
        }
        public static void StopIis()
        {
            if (_iisProcess != null)
            {
                _iisProcess.CloseMainWindow(); _iisProcess.Dispose();
            }
        }
    }
}
