using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Speech.Synthesis;
using System.Timers;
using System.Net.NetworkInformation;

namespace OLX_Parser
{
    partial class MainWindow
    {
        public async void StartMonitoringWebsitesAsync(IEnumerable<SettingsMonitoredWebsite> observableWebsites)
        {
            if (await UniversalAlgorithms.IsOnlineAsync())
            {
                foreach (var website in observableWebsites)
                    RegisterWebsiteAsync(website);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Brak połączenia z internetem.", "Brak połączenia", System.Windows.Forms.MessageBoxButtons.OK);
                System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
                {
                    System.Windows.Application.Current?.Shutdown();
                });
            }
        }

        private void RegisterWebsiteAsync(SettingsMonitoredWebsite website)
        {
            website.webClient = new WebClient() { Encoding = Encoding.UTF8 };
            MonitoredWebsites.Add(website);
            RecheckWebsiteAsync(website);
        }

        public void RecheckWebsites(object sender, EventArgs e) => RecheckWebsites();

        public void RecheckWebsites()
        {
            foreach (var website in MonitoredWebsites)
            {
                // if not for too long...
                RecheckWebsiteAsync(website);
            }
        }

        public async void RecheckWebsiteAsync(SettingsMonitoredWebsite website, string response = null)
        {
            // what if it is not monitored anymore?
            if (website.webClient.IsBusy)
                return;

            var startTimeOfPinging = DateTime.Now;

            try
            {
                var task = website.webClient.DownloadStringTaskAsync(website.Address);

                if (response != null)
                {
                    NewOfferItem notifiedOffer = null;
                    int insertedIndex = -1;
                    await Task.Run(() =>
                    {
                        if (website.Title == LoadingTitle)
                        {
                            foreach (Match match in DefaultOLXTitleRegex.Matches(response))
                            {
                                website.Title = match.Groups["title"].Value;
                            }

                            // extremely brutal yet correct hack
                            if (website.Title == LoadingTitle)
                                website.Title += " ";
                        }

                        foreach (Match match in DefaultOLXBodyRegex.Matches(response))
                        {
                            if (!website.SeenAddressesSet.Contains(match.Groups["address"].Value) && !website.TemporarySeenAddressesSet.Contains(match.Groups["address"].Value))
                            {
                                website.TemporarySeenAddressesSet.Add(match.Groups["address"].Value);
                                var newOffer = new NewOfferItem()
                                {
                                    ResultMatch = match,
                                    Website = website,
                                };
                                System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
                                {
                                    if (notifiedOffer == null)
                                    {
                                        notifiedOffer = newOffer;
                                        insertedIndex = NewMatches.Count;
                                    }

                                    NewMatches.Add(newOffer);
                                    AppendDetailsAsync(newOffer);
                                });
                            }
                        }

                        website.PingWork = (DateTime.Now - startTimeOfPinging).Milliseconds;
                    });

                    if (notifiedOffer != null)
                        System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
                        {
                            NotifyAboutNewOffer(notifiedOffer, insertedIndex);
                        });
                }// endif response != null

                await task;

                website.LastObserved = DateTime.Now;
                website.PingWeb = (website.LastObserved - startTimeOfPinging).Milliseconds;

                var timeSpan = TimeSpan.FromMilliseconds(Math.Max(0, Settings.Instance.InnerSettings.Break - website.PingWeb));

                if (task.Result == null || task.Result == string.Empty)
                {
                    return;
                }
                else if (timeSpan.TotalMilliseconds == 0 || response == null)
                {
                    RecheckWebsiteAsync(website, task.Result);
                }
                else
                {
                    UniversalAlgorithms.PlanExecution(timeSpan, (object sender, ElapsedEventArgs e) =>
                    {
                        RecheckWebsiteAsync(website, task.Result);
                    });
                }

            }
            catch (WebException e)
            {
            }
            catch (TaskCanceledException e)
            {
            }
        }

        private async void AppendDetailsAsync(NewOfferItem newOffer)
        {
            try
            {
                await Task.Run(async () =>
                    {
                        var response = string.Empty;
                        var sb = new StringBuilder();

                        using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                            response = await client.DownloadStringTaskAsync(newOffer.ResultMatch.Groups["address"].Value);

                        foreach (Match match in OLXOfferDetailsRegex.Matches(response))
                            sb.Append(match.Groups["details"].Value);

                        var dateAdded = new DateTime();
                        int iter = 0;
                        foreach (Match match in OLXDateAddedRegex.Matches(response))
                        {
                            DateTime.TryParse(match.Groups["datetime"].Value,
                                new System.Globalization.CultureInfo("pl-PL", true),
                                System.Globalization.DateTimeStyles.None,
                                out dateAdded);
                            ++iter;
                        }
                        newOffer.DateAdded = dateAdded;

                        var matches = OLXDetailsWithoutTagsRegex.Matches(sb.ToString());
                        sb.Clear();

                        foreach (Match match in matches)
                            sb.Append(match.Groups["withouttags"].Value);

                        var unfinishedDescription = sb.ToString();
                        unfinishedDescription = new Regex(@"\s+").Replace(unfinishedDescription, " ");
                        unfinishedDescription = new Regex(@"(^\s+)|(\s+$)").Replace(unfinishedDescription, string.Empty);
                        newOffer.Details = unfinishedDescription;

                        newOffer.AdditionalImagesMatch = OLXDetailedImagesRegex.Matches(response);
                    });
            }
            catch (WebException e)
            {
            }
        }

        private void NotifyAboutNewOffer(NewOfferItem newOffer, int insertedIndex)
        {
            Synthesizer.SpeakAsyncCancelAll();

            Tabs.SelectedIndex = 1;

            if (NewMatches.Count > 0)
            {
                NewOffers.CurrentItem = NewOffers.Items[insertedIndex];
                NewOffers.SelectedIndex = insertedIndex;
                NewOffers.ScrollIntoView(NewOffers.Items[insertedIndex]);
            }

            if (WindowState == System.Windows.WindowState.Minimized)
                WindowState = System.Windows.WindowState.Normal;

            Activate();
            Topmost = true;

            Console.Beep();
            System.Media.SystemSounds.Asterisk.Play();

            if (Settings.Instance.InnerSettings.Speak)
            {
                var sb = new StringBuilder();
                if (newOffer.ResultMatch.Groups["price"].Value.ToLower() == "za darmo")
                {
                    sb.Append("Nowa darmowa oferta. ");
                }
                else if (newOffer.ResultMatch.Groups["price"].Value != "")
                {
                    sb.Append($"Nowa oferta w cenie {newOffer.ResultMatch.Groups["price"].Value}. ");
                }
                else
                {
                    sb.Append($"Nowa oferta bez określonej ceny. ");
                }
                sb.Append(newOffer.ResultMatch.Groups["description"].Value);
                Synthesizer.SpeakAsync(sb.ToString());
            }
        }

        private bool CheckIfValidHost(string address, string host, out Uri returnValue)
        {
            try
            {
                returnValue = new Uri(address);
                return returnValue.Host == host;
            }
            catch (Exception e)
            {
                returnValue = null;
                return false;
            }
        }

        private static Regex ReplaceSizeRegex = new Regex(@"_\d+?x\d+?_", RegexOptions.Compiled);
        public static string EnlargeSource(string source, string desiredSize = "2000x2000") => ReplaceSizeRegex.Replace(source, $"_{desiredSize}_");
    }
}
