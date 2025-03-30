﻿using Microsoft.UI.Xaml.Data;
using System;

namespace ArtAttack.Utils
{
    public class StringToPaymentMethodConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null) return false;
            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is bool isChecked && isChecked ? parameter.ToString() : null;
        }
    }
}
