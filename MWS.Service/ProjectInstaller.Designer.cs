namespace MWS.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
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
            this.MacheteServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.MacheteServiceProcessInstaller,
            this.MacheteServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller MacheteServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller MacheteServiceInstaller;
    }
}