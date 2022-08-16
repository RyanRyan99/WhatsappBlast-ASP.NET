using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace whatsappmobil.sender
{
    public class Chat
    {
        //Untuk Mendapatkan Display Percakapan Chat 
    }
    #region Chat2
    public class Data
    {
        public Id id { get; set; }
        public int rowId { get; set; }
        public string body { get; set; }
        public string type { get; set; }
        public int t { get; set; }
        public From from { get; set; }
        public To to { get; set; }
        public string self { get; set; }
        public int ack { get; set; }
        public bool invis { get; set; }
        public bool star { get; set; }
        public bool isFromTemplate { get; set; }
        public List<object> pollOptions { get; set; }
        public List<object> mentionedJidList { get; set; }
        public bool isVcardOverMmsDocument { get; set; }
        public bool isForwarded { get; set; }
        public bool hasReaction { get; set; }
        public int ephemeralStartTimestamp { get; set; }
        public string disappearingModeInitiator { get; set; }
        public bool productHeaderImageRejected { get; set; }
        public int lastPlaybackProgress { get; set; }
        public bool isDynamicReplyButtonsMsg { get; set; }
        public bool isMdHistoryMsg { get; set; }
        public bool requiresDirectConnection { get; set; }
        public bool pttForwardedFeaturesEnabled { get; set; }
        public bool isEphemeral { get; set; }
        public bool isStatusV3 { get; set; }
        public List<object> links { get; set; }
        public string subtype { get; set; }
        public ProtocolMessageKey protocolMessageKey { get; set; }
        public string notifyName { get; set; }
        public bool? isNewMsg { get; set; }
        public bool? recvFresh { get; set; }
        public string thumbnail { get; set; }
        public int? richPreviewType { get; set; }
        public bool? broadcast { get; set; }
        public QuotedMsg quotedMsg { get; set; }
        public string quotedStanzaID { get; set; }
        public QuotedParticipant quotedParticipant { get; set; }
        public bool? ephemeralOutOfSync { get; set; }
        public string inviteGrpType { get; set; }
    }

    public class From
    {
        public string server { get; set; }
        public string user { get; set; }
        public string _serialized { get; set; }
    }

    public class Id
    {
        public bool fromMe { get; set; }
        public string remote { get; set; }
        public string id { get; set; }
        public string _serialized { get; set; }
    }

    public class ProtocolMessageKey
    {
        public bool fromMe { get; set; }
        public Remote remote { get; set; }
        public string id { get; set; }
        public string _serialized { get; set; }
    }

    public class QuotedMsg
    {
        public string type { get; set; }
        public string body { get; set; }
    }

    public class QuotedParticipant
    {
        public string server { get; set; }
        public string user { get; set; }
        public string _serialized { get; set; }
    }

    public class Remote
    {
        public string server { get; set; }
        public string user { get; set; }
        public string _serialized { get; set; }
    }

    public class Response
    {
        public Data _data { get; set; }
        public Id id { get; set; }
        public int ack { get; set; }
        public bool hasMedia { get; set; }
        public string body { get; set; }
        public string type { get; set; }
        public int timestamp { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string deviceType { get; set; }
        public bool isForwarded { get; set; }
        public int forwardingScore { get; set; }
        public bool isStatus { get; set; }
        public bool isStarred { get; set; }
        public bool fromMe { get; set; }
        public bool hasQuotedMsg { get; set; }
        public List<object> vCards { get; set; }
        public List<object> mentionedIds { get; set; }
        public bool isGif { get; set; }
        public bool isEphemeral { get; set; }
        public List<object> links { get; set; }
        public bool? broadcast { get; set; }
    }

    public class Root
    {
        public List<Response> response { get; set; }
    }

    public class To
    {
        public string server { get; set; }
        public string user { get; set; }
        public string _serialized { get; set; }
    }
    #endregion
}