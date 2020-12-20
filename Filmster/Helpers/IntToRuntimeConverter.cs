﻿using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Helpers
{
    public class IntToRuntimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var totalMinutes = (int)value;
            if (totalMinutes == 0)
            {
                return string.Empty;
            }

            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            string h = "Media_RuntimeHours".GetLocalized();
            string m = "Media_RuntimeMinutes".GetLocalized();

            if (hours < 1)
            {
                return $"{minutes}{m}";
            }
            else
            {
                return $"{hours}{h} {minutes}{m}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
