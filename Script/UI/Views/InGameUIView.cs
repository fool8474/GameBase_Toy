using Script.UI.Models;
using Script.Event;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Script.InGame;
using UniRx;
using Script.Util;
using Script.InGame.PuzzleBlock;
using Script.InGame.PuzzleBlockFactory;
using Script.Inject;
using Script.Manager.Puzzle;
using System.Collections.Generic;
using Script.Manager.CSV;
using Script.Table;
using Assets.Script.Util;
using System.Linq;
using Script.Manager.Util.Log;

namespace Script.UI.Views
{
    public class InGameUIView : View<InGameUIModel>
    {
        [SerializeField] private RectTransform _leftCircleCenter;
        [SerializeField] private RectTransform _rightCircleCenter;

        private PuzzleProcessor _puzzleProcessor;
        
        protected override async UniTask InitializeWithVisible()
        {
            InitializeGame();
            await base.InitializeWithVisible();
        }

        public override void Inject(Model model)
        {
            base.Inject(model);
            _puzzleProcessor = Injector.GetInstance<PuzzleProcessor>();
        }

        private void InitializeGame()
        {
            var playerRtList = new List<RectTransform>() { _leftCircleCenter, _rightCircleCenter };

            var stageDefDic = TableMgr.GetDefDic<DefStage>();
           
            if (stageDefDic == null)
            {
                return;
            }

            var stage = stageDefDic.ElementAt(RandomUtil.RandomIndex(stageDefDic.Values.Count)).Value as DefStage;
            Log.DF(LogCategory.PUZZLE, "Selected stage id {0}", stage.Id);

            _puzzleProcessor.SetStage(stage.Id, playerRtList);

            for (var p = 0; p < 2; p++)
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 9; j++)
                    {
                        GenerateNewBlock(p, i, j).Forget();
                    }
                }
            }
        }
        
        private async UniTask GenerateNewBlock(int playerNum, int ringNum, int posNum)
        {
            await _puzzleProcessor.GetNewPuzzleBlock(playerNum, ringNum, posNum);
        }
    }
}