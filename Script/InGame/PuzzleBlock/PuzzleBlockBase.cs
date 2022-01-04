using Script.Manager.Util.Log;
using System.Collections.Generic;
using UnityEngine;

namespace Script.InGame.PuzzleBlock
{
    public class PuzzleBlockBase : MonoBehaviour 
    {
        private RectTransform _parentRt;
        private RectTransform _rt;

        public void Initialize(RectTransform parentRt, List<string> valList = null)
        {
            _rt = GetComponent<RectTransform>();

            _parentRt = parentRt;
            _rt.SetParent(parentRt);
            
            _rt.position = parentRt.position;

            InitializeSpecificBlock(valList);
        }

        public void SetPos(int bigRingIdx, int smallRingIdx, int ringBlockCnt)
        {
            var radius = 160 + 90 * bigRingIdx;
            var angle = ((float)smallRingIdx / ringBlockCnt) * 360;
            var x = Mathf.Sin(angle * (Mathf.PI / 180f)) * radius;
            var y = Mathf.Cos(angle * (Mathf.PI / 180f)) * radius;

            _rt.anchoredPosition = new Vector2(_parentRt.anchoredPosition.x + x, _parentRt.anchoredPosition.y + y);
        }
        protected virtual void InitializeSpecificBlock(List<string> valList) { }
    }
}