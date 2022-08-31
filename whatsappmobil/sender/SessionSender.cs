using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace whatsappmobil.sender
{
    public class SessionSender
    {
    }
    public class Sender
    {
        public string sender { get; set; }
        public string number { get; set; }
        public string message { get; set; }
        public string caption { get; set; }
        public string file { get; set; }

    }

    public class SenderGroup
    {
        public string sender { get; set; }
        public string name { get; set; }
        public string message { get; set; }

    }

    public class SenderGetChat
    {
        public string sender { get; set; }
        public string number { get; set; }
    }

    public class SenderPrivateChat
    {
        public string sender { get; set; }
    }

    public class SenderChatPersonal
    {
        public string sender { get; set; }
        public string number { get; set; }
        public string message { get; set; }
    }

    public class SenderChatPersonalMedia
    {
        public string sender { get; set; }
        public string number { get; set; }
        public string caption { get; set; }
        public string file { get; set; }
    }

    public class SenderReply
    {
        public string sender { get; set; }
    }
    public class SenderInsertReply
    {
        public string sender { get; set; }
        public string content { get; set; }
        public string messagecontent { get; set; }
    }

    public class SenderSingleChat
    {
        public string sender { get; set; }
        public string number { get; set; }
        public string message { get; set; }

    }
    public class SenderSingleChatMedia
    {
        public string sender { get; set; }
        public string number { get; set; }
        public string caption { get; set; }
        public string file { get; set; }
    }
    public class SenderGroupChat
    {
        public string sender { get; set; }
        public string name { get; set; }
        public string message { get; set; }
    }

    public class AddSessionBaileys
    {
        public string id { get; set; }
        public string isLegacy { get; set; }
    }
}