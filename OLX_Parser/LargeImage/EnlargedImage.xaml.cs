using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OLX_Parser.LargeImage
{
    /// <summary>
    /// Interaction logic for EnlargedImage.xaml
    /// </summary>
    public partial class EnlargedImage : Window
    {
        public EnlargedImage()
        {
            InitializeComponent();
        }

        public EnlargedImage(MainWindow.NewOfferItem newOfferResult)
        {
            InitializeComponent();

            Title = $@"Tel.: {newOfferResult.Website.PhoneNumber} Produkt: {newOfferResult.ResultMatch.Groups["description"].Value} Użytkownik: {newOfferResult.Website.Title}";

            foreach (Match imageMatch in newOfferResult.AdditionalImagesMatch)
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(MainWindow.EnlargeSource(imageMatch.Groups["imagesrc"].Value), UriKind.Absolute);
                bitmap.EndInit();

                Images.Children.Add(new Image()
                {
                    Source = bitmap,
                    MaxWidth = 100,
                    MaxHeight = 100,
                    Style = Resources["OpacityAnimation"] as Style,
                });
            }

            MainImage.Source = (Images.Children[0] as Image)?.Source;

            foreach (Image image in Images.Children)
                image.MouseDown += (object sender, MouseButtonEventArgs e) => { MainImage.Source = (sender as Image).Source; };
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = sender as ScrollViewer;
            scv?.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void Quit(object sender, MouseButtonEventArgs e) => Close();
    }
}
