#region SearchAThing.Wpf.Toolkit, Copyright(C) 2016 Lorenzo Delana, License under MIT
/*
* The MIT License(MIT)
* Copyright(c) 2016 Lorenzo Delana, https://searchathing.com
*
* Permission is hereby granted, free of charge, to any person obtaining a
* copy of this software and associated documentation files (the "Software"),
* to deal in the Software without restriction, including without limitation
* the rights to use, copy, modify, merge, publish, distribute, sublicense,
* and/or sell copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
#endregion

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static System.Math;
using SearchAThing;
using System.Linq;

namespace SearchAThing.Wpf.Toolkit
{

    public class MathOpConverterMulti : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return false;
            if (parameter == null) return false;

            if (values.Any(r => r == DependencyProperty.UnsetValue)) return false;

            switch (((string)parameter).ToLower())
            {
                case "and": return values.Cast<bool>().All(x => x);
                case "or": return values.Cast<bool>().Any(x => x);
                case "eq": return values.Select(w => (double)w).Distinct().Count() == 1;
                case "neq": return values.Select(w => (double)w).Distinct().Count() == values.Length;
                case "gt": return values.Skip(1).Select((w, j) => (double)(values[j - 1]) > (double)w).All(w => w);
                case "gte": return values.Skip(1).Select((w, j) => (double)(values[j - 1]) >= (double)w).All(w => w);
                case "lt": return values.Skip(1).Select((w, j) => (double)(values[j - 1]) < (double)w).All(w => w);
                case "lte": return values.Skip(1).Select((w, j) => (double)(values[j - 1]) <= (double)w).All(w => w);
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
