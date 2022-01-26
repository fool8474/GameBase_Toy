using Script.Table;
using Script.Manager.CSV;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Assets.Script.Util;
using Script.InGame.PuzzleBlockFactory;
using Script.Inject;
using Script.InGame.PuzzleBlock;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Script.Manager.Puzzle
{
    public class PuzzleProcessor : ScriptMgr 
    {
        private DefStage _currentStage;
        private PuzzleBlockFactoryMgr _puzzleBlockFactoryMgr;
        private List<RectTransform> _playerRTList;

        public override void Inject()
        {
            _puzzleBlockFactoryMgr = Injector.GetInstance<PuzzleBlockFactoryMgr>();
        }

        public void SetStage(int stageId, List<RectTransform> playerRTList)
        {
            _playerRTList?.Clear();
            _playerRTList = playerRTList ;

            if(TableMgr.TryGetDef(stageId, out _currentStage) == false)
            {
                Log.EF(LogCategory.PUZZLE, "Cannot load stage from stageId {0}", stageId);
                return;
            }
        }

        public async UniTask<PuzzleBlockBase> GetNewPuzzleBlock(int playerNum, int ringNum, int posNum)
        {
            var idx = RandomUtil.RandomIndex(_currentStage.BlockInfo.ProbabilityList);
            var id = _currentStage.BlockInfo.BlockIdList[idx];

            if (TableMgr.TryGetDef<DefBlockTypeSet>(id, out var blockTypeSet) == false)
            {
                return null;
            }

            switch (blockTypeSet.BlockType)
            {
                case InGame.PuzzleBlockType.NORMAL:
                    {
                        var block  = await _puzzleBlockFactoryMgr.GenerateBlock<PuzzleBlockNormal>(id);
                        block.SetPos(_playerRTList[playerNum], ringNum, posNum, 9);
                        return block;
                    }
                case InGame.PuzzleBlockType.OBSTACLE:
                    {
                        var block = await _puzzleBlockFactoryMgr.GenerateBlock<PuzzleBlockObstacle>(id);
                        block.SetPos(_playerRTList[playerNum], ringNum, posNum, 9);
                        return block;
                    }
            }

            return null;
        }
    }
}
