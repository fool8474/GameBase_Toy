using Cysharp.Threading.Tasks;
using Script.InGame.PuzzleBlock;
using Script.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Script.InGame.PuzzleBlockFactory
{
    public interface IPuzzleBlockFactory
    {
        UniTask<PuzzleBlockBase> GenerateBlock(ObjPoolMgr objPool, RectTransform parentRt, List<string> valList);
    }
}
