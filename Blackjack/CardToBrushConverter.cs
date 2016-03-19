using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Blackjack.Views
{
    public class CardToBrushConverter : IMultiValueConverter
    {
        static Dictionary<string, Brush> brushes = new Dictionary<string, Brush>();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Count() != 3)
                return null;

            string suitString = (string)values[0];
            string faceLetter = (string)values[1];
            bool faceDown = (bool)values[2];

            string imageSourceString = string.Empty;

            if (faceDown)
                imageSourceString = "Back";
            else
                imageSourceString = suitString + faceLetter;

            imageSourceString = "pack://application:,,,/Resources/Images/Cards/" + imageSourceString + ".png";

            if (brushes.ContainsKey(imageSourceString) == false)
                brushes.Add(imageSourceString, new ImageBrush(new BitmapImage(new Uri(imageSourceString))));

            return brushes[imageSourceString];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
