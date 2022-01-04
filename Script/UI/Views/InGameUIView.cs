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

namespace Script.UI.Views
{
    public class InGameUIView : View<InGameUIModel>
    {
        [SerializeField] private RectTransform _leftCircleCenter;

        private PuzzleBlockFactoryMgr _puzzleBlockFactoryMgr;
        
        protected override async UniTask InitializeWithVisible()
        {
            await InitializeGame();
            await base.InitializeWithVisible();
        }

        public override void Inject(Model model)
        {
            base.Inject(model);

            _puzzleBlockFactoryMgr = Injector.GetInstance<PuzzleBlockFactoryMgr>();
        }

        private async UniTask InitializeGame()
        {
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (j % 3 == 0)
                    {
                        await GenerateNewBlock<PuzzlePopArea>(i, j);
                    }

                    await GenerateNewBlock<PuzzleBlockNormal>(i, j);
                }
            }
        }

        private async UniTask GenerateNewBlock<T>(int bigIdx, int smallIdx) where T : PuzzleBlockBase
        {
            var block = await _puzzleBlockFactoryMgr.GenerateBlock<T>(_leftCircleCenter);
            block.SetPos(bigIdx, smallIdx, 9);
        }
    }
}