using Script.InGame.PuzzleBlock;
using System.Collections.Generic;
using UnityEngine;

namespace Script.InGame
{
    public class PuzzleRingRow : MonoBehaviour 
    {
        private List<PuzzleBlockBase> _blockList;
        private List<PuzzlePopArea> _targetBlockList;
        
        public PuzzleRingRow()
        {
            _blockList = new List<PuzzleBlockBase>();
            _targetBlockList = new List<PuzzlePopArea>();
        }

        public void AddRing(int count)
        {
            for (int i = 0; i < count; i++)
            {
            }
        }

        public void AddBlock(PuzzleBlockBase block, int idx)
        {
            if (idx >= _blockList.Count)
            {
                _blockList.Add(block);
                return;
            }

            _blockList.Insert(idx, block);
        }

        public int GetBlockCount()
        {
            return _blockList.Count;
        }
    }
}
