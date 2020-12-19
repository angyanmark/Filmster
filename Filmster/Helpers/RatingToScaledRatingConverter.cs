﻿using System;
using Windows.UI.Xaml.Data;

namespace Filmster.Helpers
{
    public class RatingToScaledRatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var rating = (double)value;
            if(rating == 0)
            {
                return -1.0;
            }

            string parameterString = parameter as string;
            string[] parameters = parameterString.Split('|');

            if (parameters.Length != 2)
            {
                throw new ArgumentException(string.Format("There must be 2 parameters separated with \"|\". {0} is not a valid parameter.", parameter), "parameter");
            }

            if (!int.TryParse(parameters[0], out int from) || !int.TryParse(parameters[1], out int to))
            {
                throw new ArgumentException(string.Format("Parameters must be int values. {0} or {1} are not int values.", parameters[0], parameters[1]), "parameter");
            }

            var ratio = (double) to / from;
            return rating * ratio;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}