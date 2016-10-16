using NimbleFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Model
{
    public class ModuleModel
    {
        public bool IsCheck { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public IQMessage Invoker { get; set; }
    }
}
