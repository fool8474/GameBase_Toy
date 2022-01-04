using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Script.UI
{
    public enum UIAnimationType
    {
        NONE = 0,
        FADE = 1,
        SCALE = 2,
        FADE_SCALE = 3,
    }
    
    public class UIAnimDirector : MonoBehaviour
    {
        public UIAnimationType startType;
        public UIAnimationType endType;
        
        public CanvasGroup MainCanvas;
        public RectTransform MainView;
        public float Duration = 1f;

        public void InitView()
        {
            MainCanvas.alpha = 1;
            MainView.localScale = Vector3.one;
            
            switch (startType)
            {
                case UIAnimationType.NONE:
                    InitNone();
                    break;
                case UIAnimationType.FADE:
                    InitFade();
                    break;
                case UIAnimationType.SCALE:
                    InitScale();
                    break;
                case UIAnimationType.FADE_SCALE:
                    InitFade();
                    InitScale();
                    break;
            }
        }
        
        public async UniTask SetVisible(bool isVisible)
        {
            if (isVisible)
            {
                await ShowAnimation();
            }

            else
            {
                await HideAnimation();
            }
        }

        private async UniTask ShowAnimation()
        {
            switch (startType)
            {
                case UIAnimationType.NONE:
                    NoneAnimCase();
                    break;
                case UIAnimationType.FADE:
                    await UniTask.WhenAll(FadeIn());
                    break;
                case UIAnimationType.SCALE:
                    await UniTask.WhenAll(ScaleUp());
                    break;
                case UIAnimationType.FADE_SCALE:
                    await UniTask.WhenAll(FadeIn(), ScaleUp());
                    break;
            }
        }

        private async UniTask HideAnimation()
        {
            switch (endType)
            {
                case UIAnimationType.FADE:
                    await UniTask.WhenAll(FadeOut());
                    break;
                case UIAnimationType.SCALE:
                    await UniTask.WhenAll(ScaleDown());
                    break;
                case UIAnimationType.FADE_SCALE:
                    await UniTask.WhenAll(FadeOut(), ScaleDown());
                    break;
            }
        }

        private void InitScale()
        {
            MainView.localScale = Vector3.zero;
        }

        private void InitNone()
        {
            MainView.localScale = Vector3.zero;
        }

        private void InitFade()
        {
            MainCanvas.alpha = 0;
        }

        private void NoneAnimCase()
        {
            MainView.localScale = Vector3.one;
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