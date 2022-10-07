using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UNO.Enum
{
    [DataContract]
    public enum Value
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Reverse = 10,
        Skip = 11,
        Plus2 = 12,
        ChooseColor = 13,
        ChooseColorPlus4 = 14,
        None = 15
    }
}
