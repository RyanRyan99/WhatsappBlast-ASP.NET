using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace whatsappmobil.sender
{
    public class SenderBaileys
    {
        
    }
    public class DataSessionBaileys
    {
        public string qr { get; set; }
        public string status { get; set; }
    }

    public class RootSessionBaileys
    {
        public bool success { get; set; }
        public string message { get; set; }
        public DataSessionBaileys data { get; set; }
    }
}