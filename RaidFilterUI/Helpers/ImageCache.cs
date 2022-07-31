using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RaidFilterUI
{
    internal class ImageCache
    {
        public static ImageCache _instance;
        public static ImageCache Instance => _instance ??= new ImageCache();

        private string DirPath = Directory.GetCurrentDirectory();
        private string ImgPath = "aPictures";

        private Dictionary<string, ImageSource> _images = new();

        public ImageSource GetImage(string name)
        {
            if (_images.ContainsKey(name))
                return _images[name];

            try
            {
                _images.Add(name, new BitmapImage(new Uri(Path.Combine(DirPath, ImgPath, name))));
            }
            catch
            {
                return null;
            }

            return _images[name];
        }
    }
}
