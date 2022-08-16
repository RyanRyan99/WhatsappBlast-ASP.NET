using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace whatsappmobil.sender
{
    public class ResponseReply
    {
        public string content { get; set; }
        public string messagecontent { get; set; }
        public string id { get; set; }
    }

    public class RootReply
    {
        public bool status { get; set; }
        public List<ResponseReply> response { get; set; }
    }
}