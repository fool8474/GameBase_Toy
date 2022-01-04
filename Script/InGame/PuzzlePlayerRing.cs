using Script.InGame.PuzzleBlock;
using Script.Manager.Util.Log;
using System.Collections.Generic;
using UnityEngine;

namespace Script.InGame
{
    public class PuzzlePlayerRing : MonoBehaviour
    {
        private List<PuzzleRingRow> _ringList;

        public PuzzlePlayerRing(int ringCount = 3)
        {
            _ringList = new List<PuzzleRingRow>();
            for (var i = 0; i < ringCount; i++)
            {
                _ringList.Add(new PuzzleRingRow());
            }
        }

        public void AddBlock(PuzzleBlockBase block, int ringIdx, int idx)
        {
            if (ringIdx >= _ringList.Count)
            {
                Log.WF(LogCategory.PUZZLE, "Ring Idx {0} is not valid", ringIdx);
                return;
            }

            _ringList[ringIdx].AddBlock(block, idx);
        }

        public int GetBlockCount(int ringIdx)
        {
            return _ringList[ringIdx].GetBlockCount();
        }
    }
}
