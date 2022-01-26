using Cysharp.Threading.Tasks;
using Script.Manager;
using Script.InGame.PuzzleBlock;
using Script.Inject;

namespace Script.InGame.PuzzleBlockFactory
{
    public class PuzzleBlockFactoryBase
    {
        private ObjPoolMgr _objPoolMgr;

        public PuzzleBlockFactoryBase()
        {
            _objPoolMgr = Injector.GetInstance<ObjPoolMgr>();
        }

        public virtual UniTask<PuzzleBlockBase> GetNewBlock(int blockId) { return default;  }

        protected async UniTask<T> GenerateBlock<T>(int blockId, string addrId) where T : PuzzleBlockBase
        {
            var popBlock = await _objPoolMgr.GetObject<T>(addrId);

            if (popBlock == null)
            {
                return null;
            }

            popBlock.Initialize(blockId);

            return popBlock;
        }
    }
}
