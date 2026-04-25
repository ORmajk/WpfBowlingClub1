using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using WpfBowlingClub.Classes;

namespace WpfBowlingClub.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = value as string;
            if (string.IsNullOrEmpty(imagePath))
                return GetDefaultImage();

            return ImageHelper.LoadImage(imagePath);
        }

        private object GetDefaultImage()
        {
            try
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri("pack://application:,,,/Resources/noimage.png", UriKind.RelativeOrAbsolute);
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                return img;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}