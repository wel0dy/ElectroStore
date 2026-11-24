using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ElectroStoreApp.Helpers
{
    public static class ImageLoader
    {
        private const string ImagesFolder = "Images";
        private const string PlaceholderFileName = "picture.png";

        public static BitmapImage Load(string fileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string imageName = PlaceholderFileName;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                imageName = fileName;
            }

            string path = Path.Combine(baseDirectory, ImagesFolder, imageName);

            if (!File.Exists(path))
            {
                path = Path.Combine(baseDirectory, ImagesFolder, PlaceholderFileName);
            }

            if (!File.Exists(path))
            {
                return null;
            }

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(path, UriKind.Absolute);
            image.EndInit();
            image.Freeze();
            return image;
        }
    }
}