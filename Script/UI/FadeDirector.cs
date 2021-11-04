using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Script.UI
{
    public class FadeDirector : MonoBehaviour
    {
        public CanvasGroup MainCanvas;
        public RectTransform MainView;
        public float Duration = 1f;

        public async UniTask SetVisible(bool isVisible)
        {
            if (isVisible)
            {
                await UniTask.WhenAll(FadeIn(), ScaleUp());
            }

            else
            {
                await UniTask.WhenAll(FadeOut(), ScaleDown());
            }
        }

        private async UniTask ScaleUp()
        {
            var completeScale = false;
            MainView.localScale = Vector3.zero;
            MainView.DOScale(Vector3.one, Duration).OnComplete(() =>
            {
                completeScale = true;
            });

            await UniTask.WaitUntil(() => completeScale);
        }

        private async UniTask ScaleDown()
        {
            var completeScale = false;
            MainView.localScale = Vector3.one;
            MainView.DOScale(Vector3.zero, Duration).OnComplete(() =>
            {
                completeScale = true;
            });

            await UniTask.WaitUntil(() => completeScale);
        }
        
        private async UniTask FadeIn()
        {
            var completeFade = false;
            MainCanvas.alpha = 0;
            DOTween.To(alpha => MainCanvas.alpha = alpha, MainCanvas.alpha, 1, Duration).OnComplete(() =>
            {
                completeFade = true;
            });

            await UniTask.WaitUntil(() => completeFade);
        }


        private async UniTask FadeOut()
        {
            var completeFade = false;
            MainCanvas.alpha = 1;
            DOTween.To(alpha => MainCanvas.alpha = alpha, MainCanvas.alpha, 0, Duration).OnComplete(() =>
            {
                completeFade = true;
            });
            
            await UniTask.WaitUntil(() => completeFade);
        }
    }
}