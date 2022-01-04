using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Component.Effect
{
    public class Ripple : MonoBehaviour
    {
        public float maxSize;
        public float time;
        public Color startColor;
        public Color lerpColor;

        private TweenerCore<Vector3, Vector3, VectorOptions> _rippleTween;
        private TweenerCore<Color, Color, ColorOptions> _colorTween;

        private RectTransform _rectTransform;
        
        private Image _colorImg;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _colorImg = GetComponent<Image>();
        }

        public void InitPos(Vector2 pos)
        {
            transform.position = pos;
            _colorImg.color = startColor;
            _rectTransform.localScale = Vector3.zero;
        }
    
        public void DoTween()
        {
            _rippleTween?.Rewind();
            _rippleTween?.Kill();
            _rippleTween = _rectTransform.DOScale(new Vector3(maxSize, maxSize, maxSize), time);
            
            _colorTween?.Rewind();
            _colorTween?.Kill();
            _colorTween = _colorImg.DOColor(lerpColor, time);

            _colorTween.OnComplete(() => gameObject.SetActive(false));
        }
    }
}