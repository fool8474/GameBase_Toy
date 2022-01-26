using Cysharp.Threading.Tasks;
using Script.Util;
using Script.InGame.PuzzleBlock;

namespace Script.InGame.PuzzleBlockFactory
{
    public class TargetAreaPuzzleBlockFactory : PuzzleBlockFactoryBase
    {
        public override async UniTask<PuzzleBlockBase> GetNewBlock(int blockId)
        {
            return await GenerateBlock<PuzzlePopArea>(blockId, AddressableID.PUZZLE_POP_AREA);
        }
    }
}
