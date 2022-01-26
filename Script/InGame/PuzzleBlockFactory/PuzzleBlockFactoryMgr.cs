using Cysharp.Threading.Tasks;
using Script.Inject;
using Script.Manager;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using System.Collections.Generic;
using Script.InGame.PuzzleBlock;
using System;

namespace Script.InGame.PuzzleBlockFactory
{
    public class PuzzleBlockFactoryMgr : ScriptMgr
    {
        public Dictionary<Type, PuzzleBlockFactoryBase> _factoryDic;

        private ObjPoolMgr _objPoolMgr;

        public override void Initialize()
        {
            base.Initialize();

            _factoryDic = new Dictionary<Type, PuzzleBlockFactoryBase>();
            RegistFactory();
        }

        public override void Inject()
        {
            _objPoolMgr = Injector.GetInstance<ObjPoolMgr>();
        }

        private void RegistFactory()
        {
            _factoryDic.Add(typeof(PuzzleBlockNormal), new NormalPuzzleBlockFactory());
            _factoryDic.Add(typeof(PuzzleBlockObstacle), new ObstaclePuzzleBlockFactory());
            _factoryDic.Add(typeof(PuzzlePopArea), new TargetAreaPuzzleBlockFactory());
        }

        public async UniTask<T> GenerateBlock<T>(int blockId) where T : PuzzleBlockBase 
        {
            if(_factoryDic.TryGetValue(typeof(T), out var factory) == false)
            {
                Log.WF(LogCategory.PUZZLE, "Cannot load factory from blockType {0}", typeof(T));
                return null;
            }

            return await factory.GetNewBlock(blockId) as T;
        }
    }
}
