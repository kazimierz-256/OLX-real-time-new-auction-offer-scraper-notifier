using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OLX_Parser
{
    partial class MainWindow
    {
        private const string LoadingTitle = "Ładowanie...";
        private const string WebsiteWatermark = "Adres użytkownika";
        private const string PhoneWatermark = "Telefon";

        private const string DefaultOLXBody = @"<table(.|\n)*?<img.*?src=""(?<imagesrc>https?://olxpl.+?\.akamaized\.net/.+?.jpg)""(.|\n)*?<a.*?href=""(?<address>https?://www.olx.pl/oferta/.+?)""(.|\n)*?<strong.*?>(?<description>(.|\n)*?)</strong>(.|\n)*?((<strong.*?>(?<price>(.|\n)*?)</strong>(.|\n)*?</table>)|(</tbody>))";
        private const string DefaultOLXTitle = @"<h(\d)(\s.*?)?>(?<title>.*?)</h(.|\n)*?Wszystkie ogłoszenia użytkownika";
        private static Regex DefaultOLXBodyRegex = new Regex(DefaultOLXBody, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex DefaultOLXTitleRegex = new Regex(DefaultOLXTitle, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private const string OLXOfferDetails = @"<div.*?=.*?textContent.*?>(.|\n)*?<p.*?>(?<details>(.|\n)*?)</p>";
        private const string OLXDetailsWithoutTags = @"(^|>)(?<withouttags>[^<>]*?)(<|$)";
        private const string OLXDetailedImages = @"<img[^>]*?src=""(?<imagesrc>https://olxpl.*?)""[^>]*?alt=""[^""]";
        private const string OLXDateAdded = @"Dodane(([^<]+?)|([^<]*</a>[^<]+))o\s*(?<datetime>[^<]+?),?\s*<small";
        private static Regex OLXOfferDetailsRegex = new Regex(OLXOfferDetails, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex OLXDetailsWithoutTagsRegex = new Regex(OLXDetailsWithoutTags, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex OLXDetailedImagesRegex = new Regex(OLXDetailedImages, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex OLXDateAddedRegex = new Regex(OLXDateAdded, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex OLXUserWebsiteRegex = new Regex(@"https?://www.olx.pl/oferty/uzytkownik/([a-z]|\d)+?/?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex OLXOfferAddressRegex = new Regex(@"https?://www.olx.pl/oferta/([a-z]|\d|-)+?\.html(#([a-z]|\d)+)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        private SettingsMonitoredWebsite WebsiteToRestore;
        private SpeechSynthesizer Synthesizer;


        private ObservableCollection<NewOfferItem> _NewMatches = new ObservableCollection<NewOfferItem>();
        public ObservableCollection<NewOfferItem> NewMatches
        {
            get => _NewMatches;
            set => _NewMatches = value;
        }

        private ObservableCollection<SettingsMonitoredWebsite> _MonitoredWebsites = new ObservableCollection<SettingsMonitoredWebsite>();
        public ObservableCollection<SettingsMonitoredWebsite> MonitoredWebsites
        {
            get => _MonitoredWebsites;
            set => _MonitoredWebsites = value;
        }
    }
}
