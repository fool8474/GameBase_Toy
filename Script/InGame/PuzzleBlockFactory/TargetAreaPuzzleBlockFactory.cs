using Cysharp.Threading.Tasks;
using Script.Manager;
using Script.Util;
using UnityEngine;
using Script.InGame.PuzzleBlock;
using System.Collections.Generic;

namespace Script.InGame.PuzzleBlockFactory
{
    public class TargetAreaPuzzleBlockFactory : IPuzzleBlockFactory
    {
        public async UniTask<PuzzleBlockBase> GenerateBlock(ObjPoolMgr objPool, RectTransform parentRt, List<string> valList)
        {
            var popBlock = await objPool.GetObject<PuzzlePopArea>(AddressableID.PUZZLE_POP_AREA);

            if (popBlock != null)
            {
                popBlock.Initialize(parentRt, valList);
            }
            
            return popBlock;
        }
    }
}
