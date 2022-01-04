using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Util
{
    public static class RandomUtil
    {
        public static int RandomIndex(List<int> probList, int sumValue)
        {
            var targetValue = Random.Range(0, sumValue);

            var sum = 0f;

            for (int i = 0; i < probList.Count; i++)
            {
                sum += probList[i];

                if (sum >= targetValue)
                {
                    return i;
                }
            }

            return probList.Count - 1;
        }

        public static int RandomIndex(List<float> probList, float sumValue)
        {
            var targetValue = Random.Range(0, sumValue);
            
            var sum = 0f;

            for (int i = 0; i < probList.Count; i++)
            {
                sum += probList[i];

                if(sum >= targetValue)
                {
                    return i;
                }
            }

            return probList.Count - 1;
        }

        public static T RandomTaget<T>(List<T> randomList, List<float> probList, float sumValue)
        {
            
            // TODO
        }

        public static T RandomTarget<T>(List<T> randomList)
        {
            return randomList[Random.Range(0, randomList.Count)];
        }

        private static float GetSum(List<float> probList)
        {
            var rtnVal = 0f;
            foreach (var currVal in probList)
            {
                rtnVal += currVal;
            }

            return rtnVal;
        }

        private static int GetSum(List<int> probList)
        {
            var rtnVal = 0;
            foreach (var currVal in probList)
            {
                rtnVal += currVal;
            }

            return rtnVal;
        }
    }
}
