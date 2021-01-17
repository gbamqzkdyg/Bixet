using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bixet
{
    public enum Endian
    {
        BigEndian,
        SmallEndian
    }

    public enum LengthUnit
    {
        Byte,
        Bit
    }

    public enum BitexValueType
    {
        Number, 
        String
    }
}
