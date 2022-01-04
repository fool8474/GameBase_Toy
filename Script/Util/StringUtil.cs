using Script.Manager.Util.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.Util
{
    public static class StringUtil
    {
        public static string[] SplitToArray(char[] seps, string target)
        {
            return target.Split(seps);
        }

        public static List<string> SplitToList(char[] seps, string target)
        {
            return target.Split(seps).ToList();
        }

        public static T[] SplitToArray<T>(char [] seps, string target) where T : IConvertible
        {
            var splited = target.Split(seps);

            var rtnArray = new T[splited.Length];

            for(int i=0; i<splited.Length; i++)
            {
                rtnArray[i] = Parser.ConvertTo<string, T>(splited[i]);
            }

            return rtnArray;
        }

        public static List<T> SplitToList<T>(char [] seps, string target) where T : IConvertible
        {
            var splited = target.Split(seps);

            var rtnList = new List<T>(splited.Length);

            for (int i = 0; i < splited.Length; i++)
            {
                rtnList[i] = Parser.ConvertTo<string, T>(splited[i]);
            }

            return rtnList;
        }
    }
}
