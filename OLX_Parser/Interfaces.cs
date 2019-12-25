using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Data;

namespace OLX_Parser
{
    public class EnlargeOLXImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => MainWindow.EnlargeSource(value as string);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class DateTimeNowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var difference = DateTime.Now - (DateTime) value;

            if (difference.TotalMinutes < 60)
            {
                return $"{Math.Floor(difference.TotalMinutes)} m. {Math.Round((difference - TimeSpan.FromMinutes(Math.Floor(difference.TotalMinutes))).TotalSeconds)} s.";
            }
            else if (difference.TotalHours < 24)
            {
                return $"{Math.Floor(difference.TotalHours)} g. {Math.Round((difference - TimeSpan.FromHours(Math.Floor(difference.TotalHours))).TotalMinutes)} m.";
            }
            else
            {
                return $"{Math.Floor(difference.TotalDays)} d. {Math.Round((difference - TimeSpan.FromDays(Math.Floor(difference.TotalDays))).TotalHours)} g.";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    public class DateTimeNowColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (System.Windows.Application.Current?.MainWindow?.Resources["OK"] as System.Windows.Media.SolidColorBrush).Color;
            var difference = (DateTime.Now - (DateTime) value).TotalHours;
            color.A = (byte) (255 / (difference / 12 + 1));
            return new System.Windows.Media.SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    partial class MainWindow
    {
        public class NewOfferItem : INotifyPropertyChanged
        {
            private MatchCollection _AdditionalImagesMatch;
            public MatchCollection AdditionalImagesMatch
            {
                get => _AdditionalImagesMatch;
                set { _AdditionalImagesMatch = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AdditionalImagesMatch))); }
            }

            private Match _ResultMatch;
            public Match ResultMatch
            {
                get => _ResultMatch;
                set { _ResultMatch = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResultMatch))); }
            }

            private SettingsMonitoredWebsite _Website;
            public SettingsMonitoredWebsite Website
            {
                get => _Website;
                set { _Website = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Website))); }
            }

            private string _Details = LoadingTitle;
            public string Details
            {
                get => _Details;
                set { _Details = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Details))); }
            }

            private Timer DateAddedTimer;
            private DateTime _DateAdded;
            public DateTime DateAdded
            {
                get => _DateAdded;
                set
                {
                    _DateAdded = value;

                    DateAddedTimer?.Stop();
                    DateAddedTimer?.Dispose();

                    var timeSpan = TimeSpan.FromSeconds(10);
                    if ((DateTime.Now - value).TotalHours >= 24)
                    {
                        timeSpan = TimeSpan.FromMinutes(10);
                    }
                    else if ((DateTime.Now - value).TotalMinutes >= 60)
                    {
                        timeSpan = TimeSpan.FromSeconds(20);
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateAdded)));
                    DateAddedTimer = UniversalAlgorithms.PlanExecution(timeSpan, (object sender, ElapsedEventArgs e) =>
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateAdded)));
                    }, true);
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }


    }
}
