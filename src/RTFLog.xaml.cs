#region SearchAThing.Wpf.Toolkit, Copyright(C) 2016-2018 Lorenzo Delana, License under MIT
/*
* The MIT License(MIT)
* Copyright(c) 2016-2018 Lorenzo Delana, https://searchathing.com
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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace SearchAThing.Wpf.Toolkit
{
    public partial class RTFLog : UserControl
    {

        #region AutoScroll
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register("ÄutoScroll",
                typeof(bool), typeof(RTFLog), new FrameworkPropertyMetadata(true));

        public bool AutoScroll
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }
        #endregion

        public RTFLog()
        {
            InitializeComponent();

            doc = new FlowDocument();
            rtf.Document = doc;
        }

        FlowDocument doc;
        Paragraph para;

        Brush black = new SolidColorBrush(Colors.Black);
        Brush darkgreen = new SolidColorBrush(Colors.DarkGreen);
        Brush darkorange = new SolidColorBrush(Colors.DarkOrange);
        Brush red = new SolidColorBrush(Colors.Red);

        public enum LogColor
        {
            normal,
            warning,
            error,
            success
        }

        /// <summary>
        /// clear log content
        /// </summary>
        public void Clear()
        {
            doc.Blocks.Clear();
        }

        /// <summary>
        /// append log text
        /// </summary>        
        public void Append(string msg, bool newline = false, LogColor color = LogColor.normal)
        {
            var run = new Run() { Text = msg };
            if (color != LogColor.normal)
            {
                switch (color)
                {
                    case LogColor.warning: run.Foreground = darkorange; break;
                    case LogColor.error: run.Foreground = red; break;
                    case LogColor.success: run.Foreground = darkgreen; break;
                }
            }
            if (para == null)
            {
                para = new Paragraph(run) { Margin = new Thickness(0) };
                doc.Blocks.Add(para);
            }
            else
                para.Inlines.Add(run);

            if (newline) para = null;
            if (AutoScroll) rtf.ScrollToEnd();
        }

    }
}
