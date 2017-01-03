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

    /*
     * example:
     * 
     * follow enable the Run textblock only if
     * - the project results a not-null object
     * - AND
     * - the IsRunning property is not true
     * 
     * <TextBlock Text="Run" Style="{DynamicResource HyperlinkTextBlk}" MouseLeftButtonDown="runProject_click">
     *   <TextBlock.IsEnabled>
     *     <MultiBinding Converter="{StaticResource MathOpConverterMulti}" ConverterParameter="and">
     *       <Binding Path="CurrentProject" ConverterParameter="false" Converter="{StaticResource ObjectNullBoolConverter}" ElementName="window"/>
     *       <Binding Path="IsRunning" ConverterParameter="false" Converter="{StaticResource BoolInvertConverter}" ElementName="window"/>
     *     </MultiBinding>
     *   </TextBlock.IsEnabled>
     * </TextBlock>
     */
    public class MathOpConverterMulti : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return false;
            if (parameter == null) return false;

            if (values.Any(r => r == DependencyProperty.UnsetValue)) return false;

            var pars = ((string)parameter).ToLower().Split(' ');

            var vi = 0;
            bool res = true;

            foreach (var p in pars)
            {
                switch (p)
                {
                    case "istrue":
                        {
                            res = res && (bool)values[vi];
                            ++vi;
                        }
                        break;

                    case "isfalse":
                        {
                            res = res&& !(bool)values[vi];
                            ++vi;
                        }
                        break;

                    case "and":
                        {
                            res = res && ((bool)values[vi] && (bool)values[vi + 1]);
                            vi += 2;
                        }
                        break;

                    case "or":
                        {
                            res = res && ((bool)values[vi] || (bool)values[vi + 1]);
                            vi += 2;
                        }
                        break;

                    case "eq":
                        {
                            if (values[vi] == null || values[vi + 1] == null)
                                res = false;
                            else
                                res = res && (values[vi].Equals(values[vi + 1]));
                            vi += 2;
                        }
                        break;

                    case "neq":
                        {
                            if (values[vi] == null || values[vi + 1] == null)
                                res = true;
                            else
                                res = res && !(values[vi].Equals(values[vi + 1]));
                            vi += 2;
                        }
                        break;

                    case "gt":
                        {
                            res = res && (System.Convert.ToDouble(values[vi]) > System.Convert.ToDouble(values[vi + 1]));
                            vi += 2;
                        }
                        break;

                    case "gte":
                        {
                            res = res && (System.Convert.ToDouble(values[vi]) >= System.Convert.ToDouble(values[vi + 1]));
                            vi += 2;
                        }
                        break;

                    case "lt":
                        {
                            res = res && (System.Convert.ToDouble(values[vi]) < System.Convert.ToDouble(values[vi + 1]));
                            vi += 2;
                        }
                        break;

                    case "lte":
                        {
                            res = res && (System.Convert.ToDouble(values[vi]) <= System.Convert.ToDouble(values[vi + 1]));
                            vi += 2;
                        }
                        break;

                }

            }

            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
