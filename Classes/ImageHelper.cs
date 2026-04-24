using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace WpfBowlingClub.Classes
{
    public static class ImageHelper
    {
        private static string GetImageFolder()
        {
            string appData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "WpfBowlingClub");

            string imgFolder = Path.Combine(appData, "ProductImages");
            if (!Directory.Exists(imgFolder))
                Directory.CreateDirectory(imgFolder);

            return imgFolder;
        }

        public static string SaveImage(string sourcePath)
        {
            if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath))
                return null;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(sourcePath);
            string destPath = Path.Combine(GetImageFolder(), fileName);

            File.Copy(sourcePath, destPath, true);
            return fileName;
        }

        public static BitmapImage LoadImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return GetDefaultImage();

            string fullPath = Path.Combine(GetImageFolder(), fileName);

            if (!File.Exists(fullPath))
                return GetDefaultImage();

            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(fullPath);
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }

        private static BitmapImage GetDefaultImage()
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/Resources/noimage.png", UriKind.RelativeOrAbsolute);
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }
    }
}