using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWS.Core
{
    //
    // http://blogs.msdn.com/b/tijujohn/archive/2013/05/22/complex-custom-configuration-section-in-web-config-in-asp-net-sample.aspx
    //
    public class MacheteWindowsServiceConfiguration : ConfigurationSection
    {
        [ConfigurationProperty(rx.Instances, IsRequired = true, IsKey = false)]
        public InstanceCollection Instances
        {
            get
            {
                return (InstanceCollection)base[rx.Instances];
            }
            set
            {
                base[rx.Instances] = value;
            }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    [ConfigurationCollection(typeof(Instance), AddItemName = rx.Instance, CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class InstanceCollection : ConfigurationElementCollection
    {
        private const string ItemName = rx.Instance;

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        protected override string ElementName
        {
            get { return ItemName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return (elementName == ItemName);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Instance)element).Name;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Instance();
        }

        new public Instance this[string productName]
        {
            get { return (Instance)base.BaseGet(productName); }
        }

        public override bool IsReadOnly()
        {
            return false;
        }

    }

    public class Instance : ConfigurationElement
    {
        [ConfigurationProperty(rx.Name, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base[rx.Name]; }
            set { base[rx.Name] = value; }
        }

        [ConfigurationProperty(rx.EmailQueue, IsRequired = false, IsKey = false)]
        public EmailQueue EmailQueue
        {
            get { return ((EmailQueue)(base[rx.EmailQueue])); }
            set { base[rx.EmailQueue] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class EmailQueue : ConfigurationElement
    {
        [ConfigurationProperty(rx.TimerIntervalSeconds, IsRequired = true)]
        public string ServiceName
        {
            get { return (string)base[rx.TimerIntervalSeconds]; }
            set { base[rx.TimerIntervalSeconds] = value; }
        }

        [ConfigurationProperty(rx.TransmitAttempts, IsRequired = true)]
        public string Uri
        {
            get { return (string)base[rx.TransmitAttempts]; }
            set { base[rx.TransmitAttempts] = value; }
        }
        [ConfigurationProperty(rx.EmailServer, IsRequired = true, IsKey = false)]
        public EmailServer EmailServer
        {
            get { return ((EmailServer)(base[rx.EmailServer])); }
            set { base[rx.EmailServer] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class EmailServer : ConfigurationElement
    {
        [ConfigurationProperty(rx.Name, IsRequired = true)]
        public string Name
        {
            get { return (string)base[rx.Name]; }
            set { base[rx.Name] = value; }
        }
        [ConfigurationProperty(rx.HostName, IsRequired = true)]
        public string HostName
        {
            get { return (string)base[rx.HostName]; }
            set { base[rx.HostName] = value; }
        }
        [ConfigurationProperty(rx.Port, IsRequired = true)]
        public int Port
        {
            get { return (int)base[rx.Port]; }
            set { base[rx.Port] = value; }
        }
        [ConfigurationProperty(rx.EnableSSL, IsRequired = true)]
        public bool EnableSSL
        {
            get { return (bool)base[rx.EnableSSL]; }
            set { base[rx.EnableSSL] = value; }
        }
        [ConfigurationProperty(rx.OutgoingAccount, IsRequired = true)]
        public string OutgoingAccount
        {
            get { return (string)base[rx.OutgoingAccount]; }
            set { base[rx.OutgoingAccount] = value; }
        }
        [ConfigurationProperty(rx.OutgoingPassword, IsRequired = true)]
        public string OutgoingPassword
        {
            get { return (string)base[rx.OutgoingPassword]; }
            set { base[rx.OutgoingPassword] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
    /// <summary>
    /// simple resource class for strings
    /// </summary>
    public static class rx
    {
        public const string Instance = "Instance";
        public const string Instances = "Instances";
        public const string Name = "Name";
        public const string EmailQueue = "EmailQueue";
        public const string TimerIntervalSeconds = "TimerIntervalSeconds";
        public const string TransmitAttempts = "TransmitAttempts";
        public const string EmailServer= "EmailServer";
        public const string HostName= "HostName";
        public const string Port= "Port";
        public const string EnableSSL= "EnableSSL";
        public const string OutgoingAccount= "OutgoingAccount";
        public const string OutgoingPassword= "OutgoingPassword";
    }
}
