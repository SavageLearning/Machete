using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MWS.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private System.ComponentModel.IContainer components = null;
        private System.ServiceProcess.ServiceProcessInstaller MacheteServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller MacheteServiceInstaller;

        public ProjectInstaller()
        {
            this.MacheteServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.MacheteServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // MacheteServiceProcessInstaller
            // 
            this.MacheteServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.MacheteServiceProcessInstaller.Password = null;
            this.MacheteServiceProcessInstaller.Username = null;
            // 
            // MacheteServiceInstaller
            // 
            this.MacheteServiceInstaller.ServiceName = "MacheteWindowsService";
            this.MacheteServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.MacheteServiceProcessInstaller,
            this.MacheteServiceInstaller});
            EventLogInstaller installer = FindInstaller(this.Installers);
            if (installer != null)
            {
                installer.Log = EVcfg.log; // enter your event log name here
            }
        }

        private EventLogInstaller FindInstaller(InstallerCollection installers)
        {
            foreach (Installer installer in installers)
            {
                if (installer is EventLogInstaller)
                {
                    return (EventLogInstaller)installer;
                }

                EventLogInstaller eventLogInstaller = FindInstaller(installer.Installers);
                if (eventLogInstaller != null)
                {
                    return eventLogInstaller;
                }
            }
            return null;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
