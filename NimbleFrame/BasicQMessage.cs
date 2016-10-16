using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NimbleFrame
{
    public abstract class BasicQMessage : IQMessage
    {
        public string AppName { get; protected set; }

        public string GUID { get; protected set; }

        public Priority Priority { get; protected set; }

        public string Version { get; protected set; }

        public bool ShowTail { get; protected set; }

        public BasicQMessage()
        {
            AppName = "默认应用名称";
            GUID = "{AA6743D3-A749-4578-94AE-BEAD04E9FEAC}";
            Priority = Priority.MEDIUM;
            Version = "1.0";
            ShowTail = true;
        }

        public abstract string Process(string message);
    }
}
