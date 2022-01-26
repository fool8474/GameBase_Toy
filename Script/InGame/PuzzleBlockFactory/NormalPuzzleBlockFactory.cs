using Cysharp.Threading.Tasks;
using Script.Util;
using Script.InGame.PuzzleBlock;

namespace Script.InGame.PuzzleBlockFactory
{
    public class NormalPuzzleBlockFactory : PuzzleBlockFactoryBase
    {
        public override async UniTask<PuzzleBlockBase> GetNewBlock(int blockId)
        {
            return await GenerateBlock<PuzzleBlockNormal>(blockId, AddressableID.PUZZLE_BLOCK_NORMAL);
        }
    }
}
