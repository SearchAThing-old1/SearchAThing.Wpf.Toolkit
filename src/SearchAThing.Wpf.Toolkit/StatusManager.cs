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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace SearchAThing.Wpf.Toolkit
{

    /// <summary>
    /// Manage concurrent status set, with detect of the last status release.
    /// Example of usage : https://searchathing.com/?p=1424
    /// </summary>
    public class StatusManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        uint statusId;
        object statusIdLck;

        HashSet<uint> statusIdSet;
        Dictionary<uint, string> statusIdMsgDict;

        public StatusManager()
        {
            statusId = 0;
            statusIdSet = new HashSet<uint>();
            statusIdLck = new object();
            statusIdMsgDict = new Dictionary<uint, string>();
        }

        string _status;
        /// <summary>
        /// Bind your textblock to this property.
        /// You can change the status simply by changing this property value.
        /// </summary>
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
                }
            }
        }

        public void Clear(string defaultMessage = "Ready.")
        {
            Status = defaultMessage;
        }

        /// <summary>
        /// For long-running task in order to release the status
        /// displaying some ready message, use the NewStatus as first
        /// then release at the end with the id this function return.
        /// </summary>        
        public uint NewStatus(string msg)
        {
            var id = 0u;

            lock (statusIdLck)
            {
                id = ++statusId;
                statusIdSet.Add(statusId);
                statusIdMsgDict.Add(statusId, msg);
            }

            Status = msg;

            return id;
        }

        /// <summary>
        /// Set the given msg ready status if no other status are actually running.
        /// </summary>        
        public void ReleaseStatus(uint id, string msg = "Ready.")
        {
            var empty = false;
            var idMsg = "";

            string back_msg = null;

            lock (statusIdLck)
            {
                statusIdSet.Remove(id);
                empty = statusIdSet.Count == 0;
                if (!empty)
                {
                    back_msg = statusIdMsgDict[statusIdSet.Max()];
                }
#if DEBUG
                if (!statusIdMsgDict.ContainsKey(id)) Debugger.Break();
                idMsg = statusIdMsgDict[id];
                statusIdMsgDict.Remove(id); // avoid app crash if any
#endif
            }

            if (empty)
                Status = msg;
            else
            {
                if (back_msg != null)
                    Status = back_msg;
                else
                    Status = $"{idMsg} [done]";
            }
        }

    }

}
