using System;
using DG.Tweening;
using UnityEngine;

namespace Script.UI.Component
{
    public enum UIAnimType
    {
        NONE = 0,
        
        FADE,
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,
        SCALE_UP_ALL,
        SCALE_UP_VERTICAL,
        SCALE_UP_HORIZONTAL,
        
        END = 1000,
    }
    
    [Serializable]
    public class UIAnim
    {
        public UIAnimType AnimType;
        public Ease EaseType;
        public float Time;
    }
    
    public class AnUI : MonoBehaviour
    {
        public UIAnim StartAnim;
        public UIAnim EndAnim;
        
        public virtual void OnAnimation()
        {
            switch (StartAnim.AnimType)
            {
                case UIAnimType.FADE:
                    break;
                case UIAnimType.MOVE_UP:
                    break;
                case UIAnimType.MOVE_DOWN:
                    break;
                case UIAnimType.MOVE_LEFT:
                    break;
                case UIAnimType.MOVE_RIGHT:
                    break;
                case UIAnimType.SCALE_UP_ALL:
                    break;
                case UIAnimType.SCALE_UP_VERTICAL:
                    break;
                case UIAnimType.SCALE_UP_HORIZONTAL:
                    break;
                case UIAnimType.NONE:
                    break;
                default:
                    break;
            }       
        }

        public virtual void OffAnimation()
        {
            switch (EndAnim.AnimType)
            {
                case UIAnimType.FADE:
                    break;
                case UIAnimType.MOVE_UP:
                    break;
                case UIAnimType.MOVE_DOWN:
                    break;
                case UIAnimType.MOVE_LEFT:
                    break;
                case UIAnimType.MOVE_RIGHT:
                    break;
                case UIAnimType.SCALE_UP_ALL:
                    break;
                case UIAnimType.SCALE_UP_VERTICAL:
                    break;
                case UIAnimType.SCALE_UP_HORIZONTAL:
                    break;
                case UIAnimType.NONE:
                    break;
                default:
                    break;
            }
        }
    }
}