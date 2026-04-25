using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace WpfBowlingClub.Classes
{
    public static class ImageHelper
    {
        private static string GetImageFolder()
        {
            string imgFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProductImages");
            if (!Directory.Exists(imgFolder))
                Directory.CreateDirectory(imgFolder);
            return imgFolder;
        }

        public static string SaveImage(string sourcePath, int productId)
        {
            if (string.IsNullOrEmpty(sourcePath)) return null;

            string ext = Path.GetExtension(sourcePath);
            string fileName = $"{productId}_{DateTime.Now.Ticks}{ext}";
            string destPath = Path.Combine(GetImageFolder(), fileName);
            File.Copy(sourcePath, destPath, true);
            return $"ProductImages/{fileName}";
        }

        public static BitmapImage LoadImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return GetDefaultImage();

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
            if (!File.Exists(fullPath))
                return GetDefaultImage();

            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(fullPath);
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }

        public static void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        // Заглушка - создает серый квадрат 100x100
        public static BitmapImage GetDefaultImage()
        {
            // Сначала пробуем загрузить из файла
            string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "noimage.png");
            if (File.Exists(defaultPath))
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(defaultPath);
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.EndInit();
                return img;
            }

            // Если нет файла, создаем программно
            return CreateEmptyImage();
        }

        private static BitmapImage CreateEmptyImage()
        {
            // Создаем пустое изображение 100x100
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("pack://application:,,,/Resources/noimage.png", UriKind.RelativeOrAbsolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
    }
}