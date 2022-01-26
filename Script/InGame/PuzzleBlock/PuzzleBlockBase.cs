using Script.Inject;
using Script.Manager;
using Script.Manager.Util.Log;
using Script.Table;
using System.Collections.Generic;
using UnityEngine;

namespace Script.InGame.PuzzleBlock
{
    public class PuzzleBlockBase : MonoBehaviour 
    {
        protected AtlasMgr _atlasMgr;

        private RectTransform _parentRt;
        private RectTransform _rt;

        public void Initialize(int blockId)
        {
            _atlasMgr = Injector.GetInstance<AtlasMgr>();

            _rt = GetComponent<RectTransform>();
            GetBlockData(blockId);
            InitializeBlockWithData();
        }

        public void SetPos(RectTransform parentRt, int ringIdx, int posIdx, int blockCnt)
        {
            _parentRt = parentRt;
            _rt.SetParent(parentRt);

            var radius = 160 + 90 * ringIdx;
            var angle = ((float)posIdx / blockCnt) * 360;
            var x = Mathf.Sin(angle * (Mathf.PI / 180f)) * radius;
            var y = Mathf.Cos(angle * (Mathf.PI / 180f)) * radius;

            _rt.anchoredPosition = new Vector2(_parentRt.anchoredPosition.x + x, _parentRt.anchoredPosition.y + y);
        }

        protected virtual void GetBlockData(int blockId) {}
        protected virtual void InitializeBlockWithData() { }
    }
}