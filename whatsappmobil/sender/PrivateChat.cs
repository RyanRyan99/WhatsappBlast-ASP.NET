using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace whatsappmobil.sender
{
    public class PrivateChat
    {
        //Untuk Fetching Chat & Update Kontak Chat ke trx_whatsapp_p_contact
    }
    #region PrivateChat
    public class DescOwner
    {
        public string server { get; set; }
        public string user { get; set; }
        public string _serialized { get; set; }
    }

    public class GroupMetadata
    {
        public IdPrivateChat id { get; set; }
        public int creation { get; set; }
        public string desc { get; set; }
        public string descId { get; set; }
        public int descTime { get; set; }
        public DescOwner descOwner { get; set; }
        public bool restrict { get; set; }
        public bool announce { get; set; }
        public bool noFrequentlyForwarded { get; set; }
        public int ephemeralDuration { get; set; }
        public int size { get; set; }
        public bool support { get; set; }
        public bool suspended { get; set; }
        public bool terminated { get; set; }
        public UniqueShortNameMap uniqueShortNameMap { get; set; }
        public bool isParentGroup { get; set; }
        public bool defaultSubgroup { get; set; }
        public bool displayCadminPromotion { get; set; }
        public List<ParticipantPrivateChat> participants { get; set; }
        public List<object> pendingParticipants { get; set; }
        public List<object> pastParticipants { get; set; }
        public OwnerPrivateChat owner { get; set; }
    }

    public class IdPrivateChat
    {
        public string server { get; set; }
        public string user { get; set; }
        public string _serialized { get; set; }
    }

    public class OwnerPrivateChat
    {
        public string server { get; set; }
        public string user { get; set; }
        public string _serialized { get; set; }
    }

    public class ParticipantPrivateChat
    {
        public IdPrivateChat id { get; set; }
        public bool isAdmin { get; set; }
        public bool isSuperAdmin { get; set; }
    }

    public class ResponsePrivateChat
    {
        public IdPrivateChat id { get; set; }
        public string name { get; set; }
        public bool isGroup { get; set; }
        public int unreadCount { get; set; }
        public int timestamp { get; set; }
        public bool archived { get; set; }
        public bool pinned { get; set; }
        public bool isMuted { get; set; }
        public int muteExpiration { get; set; }
        public GroupMetadata groupMetadata { get; set; }
        public bool? isReadOnly { get; set; }
    }

    public class RootPrivateChat
    {
        public List<ResponsePrivateChat> response { get; set; }
    }

    public class UniqueShortNameMap
    {
    }
    #endregion
}