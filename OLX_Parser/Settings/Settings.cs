using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OLX_Parser
{
    partial class Settings
    {
        static string XmlPath = "./Settings.xml";

        private static Settings _Instance = null;
        public static Settings Instance
        {
            get
            {
                if (_Instance == null)
                    Load();

                return _Instance;

            }
            private set => _Instance = value;
        }

        public static void Load(string path = null) => Load(out _Instance, path);
        private static void Load(out Settings programSettings, string path = null)
        {
            programSettings = null;

            var serializer = new XmlSerializer(typeof(Settings));

            try
            {
                IgnoreSaving = true;

                var xmlData = ValidateXML(path ?? XmlPath);
                programSettings = serializer.Deserialize(new StringReader(xmlData)) as Settings;

                // arrays to hashsets
                programSettings.MonitoredWebsitesSet = new HashSet<SettingsMonitoredWebsite>(programSettings.MonitoredWebsites);
                foreach (var monitoredWebsite in programSettings.MonitoredWebsitesSet)
                    monitoredWebsite.SeenAddressesSet = new HashSet<string>(monitoredWebsite.SeenAddresses);

            }
            catch (Exception e)
            {
                // work offline
                programSettings = new Settings();

                //if (File.Exists(XmlPath.ToString()))
                //{
                //    var result = System.Windows.Forms.MessageBox.Show(
                //                $"{e.Message}", $"Existing yet corrupt settings file. Try fixing?",
                //                System.Windows.Forms.MessageBoxButtons.YesNo,
                //                System.Windows.Forms.MessageBoxIcon.Error);

                //    if (result != System.Windows.Forms.DialogResult.Yes)
                //    {
                //        programSettings = new Settings();
                //        Save(programSettings);
                //    }
                //}
                //else
                //{
                //    programSettings = new Settings();
                //    Save(programSettings);
                //}

                //try
                //{
                //    var xmlData = ValidateXML(XmlPath);
                //    programSettings = serializer.Deserialize(new StringReader(xmlData)) as Settings;
                //}
                //catch (Exception ee)
                //{
                //    System.Windows.Forms.MessageBox.Show("Could not fix settings file.", $"Please try removing the old settings file. {ee.InnerException} Working without persistent settings.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                //    programSettings = new Settings();
                //}
            }
            finally
            {
                IgnoreSaving = false;
            }
        }

        private static bool IgnoreSaving = false;
        private static TimeSpan SavingTimeout = TimeSpan.FromMilliseconds(500);
        public static void SaveAsync(bool saveImmediately = false, string path = null)
        {
            // especially important if Instance is yet not evaluated
            if (IgnoreSaving && !saveImmediately)
                return;

            Save(Instance, saveImmediately, path);
        }
        private static void Save(Settings programSettings, bool saveImmediately = false, string path = null)
        {
            if (IgnoreSaving && !saveImmediately)
                return;

            IgnoreSaving = true;

            if (saveImmediately)
            {
                // hashsets to arrays
                foreach (var monitoredWebsite in programSettings.MonitoredWebsitesSet)
                    monitoredWebsite.SeenAddresses = monitoredWebsite.SeenAddressesSet.ToArray();
                programSettings.MonitoredWebsites = programSettings.MonitoredWebsitesSet.ToArray();

                var writer = new StreamWriter(path ?? XmlPath);
                var serializer = new XmlSerializer(typeof(Settings));
                IgnoreSaving = false;
                serializer.Serialize(writer, programSettings);
                writer.Close();
#if DEBUG
                Console.Beep();
#endif
            }
            else
            {
                UniversalAlgorithms.PlanExecution(SavingTimeout, async (object sender, ElapsedEventArgs e) =>
                {
                    await Task.Run(() => SaveAsync(true, path));
                });
            }
        }

        private static string ValidateXML(string xmlFilename)
        {
            var xmlData = File.ReadAllText(xmlFilename);
            var xsdData = Properties.Resources.Xsd;//File.ReadAllText(XsdPath.ToString());
            var document = XDocument.Parse(xmlData);
            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(XmlSchema.Read(new StringReader(xsdData), (o, e) =>
            {
                if (e.Exception != null)
                    throw e.Exception;
            }));

            document.Validate(schemaSet, (o, e) =>
            {
                if (e.Severity == XmlSeverityType.Error)
                    throw e.Exception;
            });

            return xmlData;
        }
    }


    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Settings
    {
        private SettingsInnerSettings _InnerSettings = new SettingsInnerSettings();
        public SettingsInnerSettings InnerSettings
        {
            get => _InnerSettings;
            set => _InnerSettings = value;
        }

        [XmlIgnore]
        public HashSet<SettingsMonitoredWebsite> MonitoredWebsitesSet = new HashSet<SettingsMonitoredWebsite>();
        private SettingsMonitoredWebsite[] _MonitoredWebsites = new SettingsMonitoredWebsite[0];
        [System.Xml.Serialization.XmlArrayItemAttribute("MonitoredWebsite", IsNullable = false)]
        public SettingsMonitoredWebsite[] MonitoredWebsites
        {
            get => _MonitoredWebsites;
            set => _MonitoredWebsites = value;
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingsInnerSettings : INotifyPropertyChanged
    {
        private bool _Speak = false;
        public bool Speak
        {
            get => _Speak;
            set { _Speak = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speak))); Settings.SaveAsync(); }
        }

        private uint _Break = 5000;
        public uint Break
        {
            get => _Break;
            set { _Break = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Break))); Settings.SaveAsync(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SettingsMonitoredWebsite : INotifyPropertyChanged
    {
        private string _Title;
        public string Title
        {
            get => _Title;
            set { _Title = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title))); }
        }

        private string _Address;
        public string Address
        {
            get => _Address;
            set { _Address = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address))); }
        }

        private string _PhoneNumber;
        public string PhoneNumber
        {
            get => _PhoneNumber;
            set { _PhoneNumber = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PhoneNumber))); }
        }

        [XmlIgnore]
        public HashSet<string> TemporarySeenAddressesSet = new HashSet<string>();
        [XmlIgnore]
        public HashSet<string> SeenAddressesSet = new HashSet<string>();
        private string[] _SeenAddresses = new string[0];
        [System.Xml.Serialization.XmlArrayItemAttribute("Address", IsNullable = false)]
        public string[] SeenAddresses
        {
            get => _SeenAddresses;
            set => _SeenAddresses = value;
        }

        [XmlIgnore]
        public WebClient webClient;

        private long _PingWeb;
        [XmlIgnore]
        public long PingWeb
        {
            get => _PingWeb;
            set { _PingWeb = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PingWeb))); }
        }

        private double _PingWork;
        [XmlIgnore]
        public double PingWork
        {
            get => _PingWork;
            set { _PingWork = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PingWork))); }
        }

        private DateTime _LastObserved;
        [XmlIgnore]
        public DateTime LastObserved
        {
            get => _LastObserved;
            set { _LastObserved = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastObserved))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
