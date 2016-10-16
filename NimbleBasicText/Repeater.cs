using NimbleFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NimbleBasicText
{
    public class Repeater : BasicQMessage
    {
        public Repeater()
        {
            GUID = "{E6CEB97E-DAF4-406C-91B2-F89616C03DD3}";
            AppName = "复读机";
            Priority = Priority.MEDIUM;
            Version = "1.0";
            ShowTail = true;
        }

        public override string Process(string message)
        {
            return  message;
        }
    }
}
