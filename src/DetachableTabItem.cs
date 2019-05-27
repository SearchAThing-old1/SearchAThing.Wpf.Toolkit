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

using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Windows;

namespace OLDSearchAThing.Wpf.Toolkit
{

    public class DetachableTabItem : TabItem
    {

        public DetachableTabItem()
        {
            this.MouseDoubleClick += DetachableTabItem_MouseDoubleClick;
        }

        private void DetachableTabItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var tc = Parent as TabControl;

            if (tc == null) return;

            {
                var y = e.GetPosition(this).Y;

                if (y < 0 || y > ActualHeight) return;
            }

            var idx = tc.Items.IndexOf(this);

            var nfo = new DetachableTabItemInfo(
                tc,
                tc.Items.Cast<TabItem>().Where(r => tc.Items.IndexOf(r) < idx).ToList(),
                this);

            var content = this.Content;
            this.Content = null;
            var contentFE = content as FrameworkElement;

            tc.Items.Remove(this);

            var w = new Window()
            {
                Title = (string)Header,
                Tag = nfo
            };

            if (contentFE != null)
            {
                w.Content = content;
                w.Width = contentFE.ActualWidth;
                w.Height = contentFE.ActualHeight;
            };

            w.Closed += W_Closed;

            w.Show();
            w.Activate();
        }

        private void W_Closed(object sender, System.EventArgs e)
        {
            var w = sender as Window;
            var nfo = w.Tag as DetachableTabItemInfo;
            w.Tag = null;

            var content = w.Content;
            w.Content = null;
            nfo.DetachedTabItem.Content = content;

            var placeAfter = nfo.TabControl.Items.Cast<TabItem>()
                .Reverse()
                .FirstOrDefault(r => nfo.TabItemsBefore.Contains(r));

            var insIdx = 0;

            if (placeAfter != null) insIdx = nfo.TabControl.Items.IndexOf(placeAfter) + 1;

            nfo.TabControl.Items.Insert(insIdx, nfo.DetachedTabItem);
            nfo.TabControl.SelectedItem = nfo.DetachedTabItem;
        }

    }

    public class DetachableTabItemInfo
    {

        public TabControl TabControl { get; private set; }
        public IEnumerable<TabItem> TabItemsBefore { get; private set; }
        public TabItem DetachedTabItem { get; private set; }

        public DetachableTabItemInfo(TabControl tabControl, IEnumerable<TabItem> tabItemsBefore, TabItem detachedTabItem)
        {
            TabControl = tabControl;
            TabItemsBefore = tabItemsBefore;
            DetachedTabItem = detachedTabItem;
        }

    }

}
