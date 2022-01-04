using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Script.UI.Component.Effect;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Script.UI.Component
{
    public class AnButton : Button
    {
        [SerializeField] private float _clickDelay = 0.3f;
        [SerializeField] private float _pressedUniformScaleOffset = 0.04f;
        [SerializeField] private List<GameObject> _buttonEffect;
        [SerializeField] private Ripple _rippleEffect;
        
        private float _clickedTime;
        private float _pressedTime;

        private Action _hoverEvent;
        private Vector3 _scale;
        
        private TweenerCore<Vector3, Vector3, VectorOptions> _tweenScale;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            Init();
            
            EffectVisible(false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            InstantClearState();
            
            _tweenScale?.Kill();
            
            EffectVisible(false);
        }

        private void ActiveRipple(Vector2 pos)
        {
            if (_rippleEffect == null)
            {
                return;
            }
            
            _rippleEffect.InitPos(pos);
            _rippleEffect.DoTween();
            _rippleEffect.gameObject.SetActive(true);
        }

        private void Init()
        {
            _clickedTime = 0f;
            _pressedTime = 0f;
         
            var rt = GetComponent<RectTransform>();

            var localScale = rt.localScale;
            _scale = new Vector3(localScale.x - _pressedUniformScaleOffset, localScale.y - _pressedUniformScaleOffset, 1f);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            PlayButtonClickAnimation();
            ActiveRipple(Input.mousePosition);

            if (IsButtonDelayed() == false)
            {
                return;
            }
            
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            base.OnPointerDown(eventData);
            _pressedTime = Time.realtimeSinceStartup;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            
            _hoverEvent?.Invoke();
        }

        private void PlayButtonClickAnimation()
        {
            var rt = GetComponent<RectTransform>();
            
            _tweenScale?.Rewind();
            _tweenScale?.Kill();

            _tweenScale = rt.DOScale(_scale, colors.fadeDuration)
                .SetEase(Ease.InOutBounce)
                .SetUpdate(true)
                .SetAutoKill(false);
            
            _tweenScale?.PlayForward();
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (IsButtonDelayed() == false)
            {
                return;
            }

            _clickedTime = Time.realtimeSinceStartup;

            base.OnPointerClick(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (IsButtonDelayed() == false)
            {
                return;
            }
            
            base.OnSubmit(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            ReleaseTween();
            EffectVisible(true);
            base.OnPointerUp(eventData);
        }

        private void ReleaseTween(bool immediate = false)
        {
            _pressedTime = 0f;
            
            if (immediate)
            {
                _tweenScale?.Rewind();
            }

            else
            {
                _tweenScale?.PlayBackwards();
            }
        }
        
        private void EffectVisible(bool visible)
        {
            if (_buttonEffect == null)
            {
                return;
            }
            
            foreach (var effect in _buttonEffect)
            {
                effect.SetActive(visible);
            }
        }
        
        private bool IsButtonDelayed()
        {
            return Time.realtimeSinceStartup - _clickedTime > _clickDelay;
        }
        
        public void OnHover(Action hoverEvent)
        {
            _hoverEvent = hoverEvent;
        }
    }
}