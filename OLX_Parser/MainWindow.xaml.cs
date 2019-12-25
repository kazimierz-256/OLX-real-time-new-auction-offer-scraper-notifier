using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.IO;
using System.Timers;

namespace OLX_Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            System.Net.ServicePointManager.DefaultConnectionLimit = 100;

            SettingsTab.DataContext = Settings.Instance.InnerSettings;
            DataContext = this;

            InitializeSpeech();
            StartMonitoringWebsitesAsync(Settings.Instance.MonitoredWebsitesSet);
            UniversalAlgorithms.PlanExecution(TimeSpan.FromSeconds(15), (object sender, ElapsedEventArgs e) =>
            {
                CancelAllLostConnections();
            }, true);
        }

        private async void CancelAllLostConnections(double secondsTimeout = 15, bool definitelyConnectedToInternet = false)
        {
            foreach (var website in MonitoredWebsites)
            {
                if ((DateTime.Now - website.LastObserved).TotalSeconds > secondsTimeout)
                {
                    if (definitelyConnectedToInternet)
                    {
                        // reset connection
                        website.webClient?.CancelAsync();
                        website.webClient?.Dispose();
                        website.webClient = new System.Net.WebClient() { Encoding = Encoding.UTF8 };
                        RecheckWebsiteAsync(website);
                    }
                    else
                    {
                        // make sure the internet works
                        var connected = await UniversalAlgorithms.IsOnlineAsync();
                        if (connected)
                        {
                            CancelAllLostConnections(secondsTimeout, true);
                        }
                        else
                        {
                            // could be a too drastic message
                            System.Windows.Forms.MessageBox.Show("Stracono połączenie z internetem", "Brak połączenia", System.Windows.Forms.MessageBoxButtons.OK);
                            System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
                            {
                                System.Windows.Application.Current?.Shutdown();
                            });
                        }
                        return;
                    }
                }
            }
        }

        private void InitializeSpeech()
        {
            Synthesizer = new SpeechSynthesizer()
            {
                Rate = 0,
                Volume = 100,
            };

            foreach (var voice in Synthesizer.GetInstalledVoices())
                if (voice.VoiceInfo.Culture.ToString() == "pl-PL")
                    Synthesizer.SelectVoice(voice.VoiceInfo.Name);
        }

        private void RegisterWebsiteFromInput(object sender, RoutedEventArgs e)
        {
            if (!IsWebsiteInputValid(out Uri tmpUri))
                return;

            var settingsWebsite = new SettingsMonitoredWebsite()
            {
                Address = tmpUri.ToString(),
                Title = LoadingTitle,
                PhoneNumber = PhoneInput.Text,
                SeenAddressesSet = new HashSet<string>(),
            };

            Settings.Instance.MonitoredWebsitesSet.Add(settingsWebsite);
            Settings.SaveAsync();
            RegisterWebsiteAsync(settingsWebsite);

            PhoneInput.Text = PhoneWatermark;
            WebsiteInput.Text = WebsiteWatermark;
        }

        private void EnableDisableAddButton(object sender, TextChangedEventArgs e) => RegisterButton.IsEnabled = IsWebsiteInputValid(out _);

        private bool IsWebsiteInputValid(out Uri tmpUri)
        {
            tmpUri = null;
            var input = WebsiteInput.Text;
            bool answer = input != WebsiteWatermark &&
                        input != string.Empty &&
                        PhoneInput.Text != PhoneWatermark &&
                        PhoneInput.Text != string.Empty &&
                        OLXUserWebsiteRegex.IsMatch(input) &&
                        CheckIfValidHost($"{input}", "www.olx.pl", out tmpUri);

            // avoid duplicate addresses
            if (!answer)
                return false;

            foreach (var settingsMonitoredWebsite in Settings.Instance.MonitoredWebsitesSet)
                if (settingsMonitoredWebsite.Address == tmpUri.ToString())
                    return false;

            return answer;
        }

        private void UnregisterWebsite(object sender, RoutedEventArgs e) => UnregisterWebsite((sender as Button)?.Tag as SettingsMonitoredWebsite);

        private void UnregisterWebsite(SettingsMonitoredWebsite website, bool enableRecoveryAndSave = true)
        {
            if (enableRecoveryAndSave)
                WebsiteToRestore = website;

            website.webClient.CancelAsync();
            website.webClient.Dispose();

            MonitoredWebsites.Remove(website);
            Settings.Instance.MonitoredWebsitesSet.Remove(website);

            if (enableRecoveryAndSave)
                Settings.SaveAsync();

            RestorePerson.IsEnabled = true;
        }

        private void RestoreWebsite(object sender, RoutedEventArgs e)
        {
            if (WebsiteToRestore == null || MonitoredWebsites.Contains(WebsiteToRestore))
                return;

            var website = WebsiteToRestore;
            WebsiteToRestore = null;
            RestorePerson.IsEnabled = false;

            Settings.Instance.MonitoredWebsitesSet.Add(website);
            Settings.SaveAsync();

            RegisterWebsiteAsync(website);

        }

        private void IgnoreNewOffer(object sender, RoutedEventArgs e)
        {
            var ignoredMatch = (sender as Button)?.Tag as NewOfferItem;

            // watch out when the website is no longer monitored
            ignoredMatch.Website.SeenAddressesSet.Add(ignoredMatch.ResultMatch.Groups["address"].Value);
            Settings.SaveAsync();

            int latestPosition = NewOffers.SelectedIndex;

            NewMatches.Remove(ignoredMatch);

            if (NewMatches.Count > 0)
            {
                NewOffers.SelectedIndex = Math.Min(latestPosition, NewOffers.Items.Count - 1);
                NewOffers.CurrentItem = NewOffers.Items[NewOffers.SelectedIndex];
            }
        }

        private void ToggleWaterMark(TextBox textBox, string watermark, bool userEdits)
        {
            if (userEdits)
            {
                if (textBox.Text == watermark)
                {
                    textBox.Foreground = Brushes.Black;
                    textBox.Text = string.Empty;
                }
            }
            else
            {
                if (textBox.Text == string.Empty)
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = watermark;
                }
            }
        }

        private void WebsiteInput_Loaded(object sender, RoutedEventArgs e) => ToggleWaterMark(sender as TextBox, WebsiteWatermark, false);
        private void WebsiteInput_LostFocus(object sender, RoutedEventArgs e) => ToggleWaterMark(sender as TextBox, WebsiteWatermark, false);
        private void WebsiteInput_GotFocus(object sender, RoutedEventArgs e) => ToggleWaterMark(sender as TextBox, WebsiteWatermark, true);

        private void PhoneInput_Loaded(object sender, RoutedEventArgs e) => ToggleWaterMark(sender as TextBox, PhoneWatermark, false);
        private void PhoneInput_LostFocus(object sender, RoutedEventArgs e) => ToggleWaterMark(sender as TextBox, PhoneWatermark, false);
        private void PhoneInput_GotFocus(object sender, RoutedEventArgs e) => ToggleWaterMark(sender as TextBox, PhoneWatermark, true);

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Synthesizer.SpeakAsyncCancelAll();

            foreach (var website in MonitoredWebsites)
                if (website.webClient.IsBusy)
                {
                    website.webClient.CancelAsync();
                    website.webClient.Dispose();
                }
        }

        private void EnlargeImageCommand(NewOfferItem newOfferResult)
        {
            if (newOfferResult.AdditionalImagesMatch == null || newOfferResult.AdditionalImagesMatch.Count == 0)
                return;

            var enlargedImage = new LargeImage.EnlargedImage(newOfferResult)
            {
                Owner = this,
            };
            enlargedImage.Show();
        }

        private void EnlargeImageCommand(object sender, RoutedEventArgs e) => EnlargeImageCommand((sender as Control)?.Tag as NewOfferItem);

        private void EnlargeImageCommand(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                EnlargeImageCommand((sender as Image)?.Tag as NewOfferItem);
            // fix when user wants to open contextmenu
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = sender as ScrollViewer;
            scv?.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void CreateBackup(object sender, RoutedEventArgs e)
        {
            using (var saveDialog = new System.Windows.Forms.SaveFileDialog()
            {
                Filter = "All files|*.*",
                DefaultExt = ".xml",
                FilterIndex = 1,
                RestoreDirectory = true,
                CheckFileExists = false,
            })
            {
                var result = saveDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;

                Settings.SaveAsync(true, saveDialog.FileName);
            }
        }

        private void RestoreBackup(object sender, RoutedEventArgs e)
        {
            using (var openDialog = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = "Xml files|*.xml",
                DefaultExt = ".xml",
                FilterIndex = 1,
                RestoreDirectory = true,
                CheckFileExists = true,
            })
            {
                var result = openDialog.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;

                // cancels all users, but keeps not ignored offers
                while (MonitoredWebsites.Count > 0)
                {
                    foreach (var monitoredWebsite in MonitoredWebsites)
                    {
                        UnregisterWebsite(monitoredWebsite, false);
                        break;
                    }
                }

                WebsiteToRestore = null;
                RestorePerson.IsEnabled = false;

                Settings.Load(openDialog.FileName);
                SettingsTab.DataContext = Settings.Instance.InnerSettings;

                StartMonitoringWebsitesAsync(Settings.Instance.MonitoredWebsitesSet);
                Settings.SaveAsync();
            }
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Window_MouseMove(object sender, RoutedEventArgs e) => Topmost = false;

        private void CopyAddress(object sender, RoutedEventArgs e)
        {
            var newOfferItem = ((sender as Control)?.Tag as NewOfferItem);
            var address = newOfferItem.ResultMatch.Groups["address"].Value;

            if (OLXOfferAddressRegex.IsMatch(address))
            {
                System.Diagnostics.Process.Start(address);
            }
            else
            {
                // if the address is not as expected it is just copied into the clipboard
                Clipboard.SetText(newOfferItem.ResultMatch.Groups["address"].Value);
            }
        }

        private void Input_Drop(object sender, DragEventArgs e)
        {
            (sender as TextBox).Text = (string) e.Data.GetData(DataFormats.StringFormat);
            e.Handled = true;
        }
    }
}
