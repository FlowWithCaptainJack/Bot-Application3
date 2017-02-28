using System;
using System.Xml.Serialization;

namespace Bot.model
{
    [XmlRoot("xml")]
    public class WeChatMessage
    {

        /// <summary>
        /// Developer webcat account
        /// </summary>
        [XmlElement("ToUserName")]
        public string DeveloperAccount { get; set; }

        /// <summary>
        /// The OpenId of user
        /// </summary>
        [XmlElement("FromUserName")]
        public string UserOpenId { get; set; }

        [XmlElement("CreateTime")]
        public int CreateCTime { get; set; }

        /// <summary>
        /// The time when the message was created
        /// </summary>
        [XmlIgnore]
        public DateTime CreateTime
        {
            get
            {
                return new DateTime(1970, 1, 1).AddSeconds(CreateCTime);
            }
            set
            {
                CreateCTime = (int)(value - new DateTime(1970, 1, 1)).TotalSeconds;
            }
        }

        /// <summary>
        /// The type of the message
        /// </summary>
        [XmlElement("MsgType")]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// The type of the event
        /// </summary>
        [XmlElement("Event")]
        public EventType? EventType { get; set; }

        /// <summary>
        /// Agent ID
        /// </summary>
        [XmlElement("AgentID")]
        public string AgentID { get; set; }

        #region Customizing menu only

        /// <summary>
        ///  The key of the menu
        /// </summary>
        [XmlElement("EventKey")]
        public string EventKey { get; set; }

        /// <summary>
        /// The url of the menu
        /// </summary>
        public string Url { get; set; }

        #endregion

        #region Scanning QRCode only

        /// <summary>
        /// The ticket of QR code
        /// </summary>
        public string Ticket { get; set; }

        #endregion

        #region User message only

        [XmlElement("MsgId")]
        /// <summary>
        /// The id of the message
        /// </summary>
        public long? MessageId { get; set; }

        /// <summary>
        /// Text message only, message content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Media message only, the id of the media resource
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// Image message only, url of the picture
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// Voice message only, the format of the voice resource
        /// </summary>
        public string Format { get; set; }

        #endregion
    }
    public enum MessageType
    {
        /// <summary>
        /// Text message
        /// </summary>
        [XmlEnum("text")]
        Text,
        /// <summary>
        /// Image message
        /// </summary>
        [XmlEnum("image")]
        Image,
        /// <summary>
        /// Voice message
        /// </summary>
        [XmlEnum("voice")]
        Voice,
        /// <summary>
        /// Video message
        /// </summary>
        [XmlEnum("video")]
        Video,
        /// <summary>
        /// Short Video message
        /// </summary>
        [XmlEnum("shortvideo")]
        ShortVideo,
        /// <summary>
        /// News message
        /// </summary>
        [XmlEnum("news")]
        News,
        /// <summary>
        /// Event trigged 
        /// </summary>
        [XmlEnum("event")]
        Event,
        [XmlEnum("mpnews")]
        Mpnews,
    }

    public enum EventType
    {
        /// <summary>
        /// User subscribes this service account
        /// </summary>
        [XmlEnum("subscribe")]
        Subscribe,
        /// <summary>
        /// User unsubscribes this service account
        /// </summary>
        [XmlEnum("unsubscribe")]
        Unsubscribe,
        /// <summary>
        /// User reported location
        /// </summary>
        [XmlEnum("LOCATION")]
        Location,
        /// <summary>
        /// User scan the QR code of this service account
        /// </summary>
        [XmlEnum("SCAN")]
        ScanQRCode,
        /// <summary>
        /// User clicks the click menu
        /// </summary>
        [XmlEnum("click")]
        Click,
        /// <summary>
        /// User clicks the view menu
        /// </summary>
        [XmlEnum("view")]
        ViewMenu,

        [XmlEnum("enter_agent")]
        EnterAgent,
    }
}
