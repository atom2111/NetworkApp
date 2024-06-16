using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary
{
    public interface IMessageSourceClient
    {
        void Connect();
        void SendMessage(string message);
    }
}

