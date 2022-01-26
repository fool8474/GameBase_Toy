using Script.Manager.Util.Log;
using System;

namespace Script.Util
{
    public static class Parser
    {
        public static bool TryParse<T, V>(T target, out V result) where V : class
        {
            if (target is V == false)
            {
                Log.EF(LogCategory.PARSER, "{0} to {1} is not valid parse type", typeof(T), typeof(V));
                result = default;
                return false;
            }

            result = target as V;
            return true;
        }

        public static V ConvertTo<T, V>(T target) 
            where T : IConvertible 
            where V : IConvertible
        {
            V rtnValue = default;
            try
            {
                if(typeof(V).IsEnum)
                {
                    rtnValue = (V)Enum.Parse(typeof(V), target.ToString());
                }

                else
                {
                    rtnValue = (V)Convert.ChangeType(target, typeof(V));
                }
            }

            catch
            {
                Log.EF(LogCategory.PARSER, "Cannot convert {0} to type {1}", target.ToString(), typeof(V));
            }

            return rtnValue;
        }
    }
}