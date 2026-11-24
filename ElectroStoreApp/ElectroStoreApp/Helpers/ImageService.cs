using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ElectroStoreApp.Helpers
{
    public static class ImageService
    {
        private const string ImagesFolder = "Images";
        private const int TargetWidth = 300;
        private const int TargetHeight = 200;

        public static string SaveResizedProductImage(string sourceFilePath)
        {
            string imagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagesFolder);
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            string newFileName = Guid.NewGuid().ToString("N") + ".jpg";
            string destPath = Path.Combine(imagesDir, newFileName);

            using (Image original = Image.FromFile(sourceFilePath))
            using (Bitmap resized = new Bitmap(TargetWidth, TargetHeight))
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.White);

                double ratioX = (double)TargetWidth / original.Width;
                double ratioY = (double)TargetHeight / original.Height;
                double ratio = Math.Min(ratioX, ratioY);

                int drawWidth = (int)Math.Round(original.Width * ratio);
                int drawHeight = (int)Math.Round(original.Height * ratio);
                int posX = (TargetWidth - drawWidth) / 2;
                int posY = (TargetHeight - drawHeight) / 2;

                g.DrawImage(original, posX, posY, drawWidth, drawHeight);
                resized.Save(destPath, ImageFormat.Jpeg);
            }

            return newFileName;
        }

        public static void DeleteProductImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;
            if (string.Equals(fileName, "picture.png", StringComparison.OrdinalIgnoreCase)) return;

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagesFolder, fileName);
            if (File.Exists(fullPath))
            {
                try
                {
                    File.Delete(fullPath);
                }
                catch
                {
                }
            }
        }
    }
}

