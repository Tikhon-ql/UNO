using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UNO.Models;

namespace UNO.Helper
{
    [DataContract]
    public class TransmitMessage
    {
        [DataMember]
        public string SenderNickname { get; set; }
        [DataMember]
        public Card TopCard { get; set; }
    }
}
