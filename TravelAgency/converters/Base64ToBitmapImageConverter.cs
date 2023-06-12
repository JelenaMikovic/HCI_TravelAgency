using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using TravelAgency.model;

namespace TravelAgency.converters
{
    public class Base64StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64Image)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = new MemoryStream(System.Convert.FromBase64String(base64Image));
                bitmapImage.EndInit();
                return bitmapImage;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapImage bitmapImage)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder(); // Change the encoder type as per your requirement
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(memoryStream);

                    byte[] imageBytes = memoryStream.ToArray();
                    string base64String = System.Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
            return null;
        }
    }
}