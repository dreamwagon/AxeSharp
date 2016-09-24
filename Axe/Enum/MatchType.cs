using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public enum MatchType
    {
        Exact,
        All,
        AnyOf,
        None,
        NotAll
    }

    public class MatchTypeUtility
    {
        public static bool IsMatch( int input, int to, MatchType type )
        {
            switch (type)
            {
                case MatchType.Exact:
                    return (input == to);
                case MatchType.All:
                    return (input & to) == to;
                case MatchType.AnyOf:
                    return (input & to) != 0;
                case MatchType.None:
                    return (input & to) == 0;
                case MatchType.NotAll:
                    return (input & to) != to;
            }
            return false;
        }

        public static bool IsMatch(long input, long to, MatchType type)
        {
            switch (type)
            {
                case MatchType.Exact:
                    return (input == to);
                case MatchType.All:
                    return (input & to) == to;
                case MatchType.AnyOf:
                    return (input & to) != 0;
                case MatchType.None:
                    return (input & to) == 0;
                case MatchType.NotAll:
                    return (input & to) != to;
            }
            return false;
        }
    }

}
