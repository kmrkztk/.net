using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace Lib
{
    public static class EnumExtensions
    {
        public static TEnum Sum<TEnum>(this IEnumerable<TEnum> enums) where TEnum : struct, Enum
        {
            var flags = 0x00;
            foreach (var e in enums) flags |= (int)(object)e;
            return (TEnum)(object)flags;
        }
    }
}
