#region COPYRIGHT
// File:     WebServer.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Test.Old
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
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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
            var applicationPath = GetApplicationPath("Machete.Web");

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = true,
                LoadUserProfile = true,
                CreateNoWindow = false,
                UseShellExecute = false,
                Arguments = string.Format("/path:\"{0}\" /port:{1}", applicationPath, "4213")
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
                //_iisProcess.CloseMainWindow();
                _iisProcess.Dispose();
            }
        }
        public static void StopIis()
        {
            if (_iisProcess != null && _iisProcess.HasExited != true)
            {
                _iisProcess.CloseMainWindow(); _iisProcess.Dispose();
            }
        }
        public static string GetApplicationPath(string applicationName)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return Path.Combine(solutionFolder, applicationName);
        }
    }
}
