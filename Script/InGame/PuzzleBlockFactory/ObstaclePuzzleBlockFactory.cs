using Script.Util;
using Script.InGame.PuzzleBlock;
using Cysharp.Threading.Tasks;

namespace Script.InGame.PuzzleBlockFactory
{
    public class ObstaclePuzzleBlockFactory : PuzzleBlockFactoryBase
    {
        public override async UniTask<PuzzleBlockBase> GetNewBlock(int blockId)
        {
            return await GenerateBlock<PuzzleBlockObstacle>(blockId, AddressableID.PUZZLE_BLOCK_OBSTACLE);
        }
    }
}
