using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.Component
{
    public class AnPieChart : MaskableGraphic
    {
        [SerializeField] private List<PieChartDataNode> _dataList;

        [SerializeField] private bool _isIndicatorShowValue;
        [SerializeField] private GameObject _indicatorGo;

        [SerializeField] [Range(0.01f, 1.0f)] private float _innerThickness;
        [SerializeField] [Range(0, 150)] private float _borderThickness = 5;
        [SerializeField] private Color32 _borderColor = new Color32(255, 255, 255, 255);

        private const int _segments = 720;

        private readonly Vector2[] _uv = {
            new Vector2(0, 0), 
            new Vector2(0, 1), 
            new Vector2(1, 0), 
            new Vector2(1, 1)
        };
        
            
        [Serializable]
        public class PieChartDataNode
        {
            public string Name = "Chart Item";
            public float Value = 10;
            public Color32 Color = new Color32(255, 255, 255, 255);
            public Image IndicatorImage;
            public TextMeshProUGUI IndicatorText;
        }

        protected override void Awake()
        {
            base.Awake();
            SetIndicatorData();
        }

        private void Update()
        {
            _borderThickness = Mathf.Clamp(_borderThickness, -75, rectTransform.rect.width / 3.333f);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            
            if (_dataList.Count == 0)
            {
                return;
            }

            var prevX = Vector2.zero;
            var prevY = Vector2.zero;
            
            Vector2 pos0, pos1, pos2, pos3;
            
            // Set center & Thickness 
            var rect = rectTransform.rect;
            var frameOuter = rect.width;
            var frameInner = rect.width * _innerThickness;

            var borderOuter = rect.width + _borderThickness;
            var borderInner = rect.width * _innerThickness + _borderThickness;

            var dataIdx = 0;
            var currentValue = _dataList[0].Value;
            var total = _dataList.Sum(data => data.Value);
            var fillColor = _dataList[0].Color;

            for (var i = 0; i < _segments + 1; i++)
            {
                var rad = Mathf.Deg2Rad * i * 0.5f;
                var cos = Mathf.Cos(rad);
                var sin = Mathf.Sin(rad);

                pos0 = prevX;
                pos1 = new Vector2(frameOuter * cos, frameOuter * sin);
                pos2 = new Vector2(borderOuter * cos, borderOuter * sin);
                pos3 = prevY;

                // 값에 따라 색 조정
                if (i > currentValue / total * _segments)
                {
                    if (dataIdx < _dataList.Count - 1)
                    {
                        dataIdx += 1;
                        currentValue += _dataList[dataIdx].Value;
                        fillColor = _dataList[dataIdx].Color;
                    }
                }
                
                // fill
                vh.AddUIVertexQuad(SetVbo(new[] 
                    {
                        pos0, 
                        pos1, 
                        pos2 * borderInner / borderOuter,
                        pos3 * borderInner / borderOuter
                    }, _uv, fillColor));
                
                // border Inner
                vh.AddUIVertexQuad(SetVbo(
                    new [] {pos0, pos1, pos2, pos3},
                    _uv,
                    _borderColor
                    ));
                
                // border Outer
                vh.AddUIVertexQuad(SetVbo(new []
                {
                    pos0 * frameInner / frameOuter,
                    pos1 * frameInner / frameOuter,
                    pos2 * borderInner / borderOuter,
                    pos3 * borderInner / borderOuter
                }, _uv, _borderColor));

                prevX = pos1;
                prevY = pos2;
            }
        }

        private UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs, Color32 color)
        {
            var vbo = new UIVertex[4];

            for (var i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }

            return vbo;
        }

        public void AddNewItem()
        {
            var item = new PieChartDataNode();
            var parentTr = _indicatorGo.transform;

            if (parentTr.childCount != 0)
            {
                var tempIdx = parentTr.childCount - 1;

                var tempIndicator = parentTr.GetChild(tempIdx).gameObject;
                var newIndicator = Instantiate(tempIndicator, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                
                newIndicator.transform.SetParent(parentTr, false);
                newIndicator.gameObject.name = "Indicator_" + (tempIdx + 1);

                item.IndicatorImage = newIndicator.GetComponentInChildren<Image>();
                item.IndicatorText = newIndicator.GetComponentInChildren<TextMeshProUGUI>();
                item.Name = "Chart Item " + (tempIdx + 1);
            }

            _dataList.Add(item);
        }
        
        #region Indicator
        public void SetIndicatorData()
        {
            foreach (var currData in _dataList)
            {
                if (currData.IndicatorImage != null)
                {
                    currData.IndicatorImage.color = currData.Color;
                }

                if (currData.IndicatorText == null)
                {
                    continue;
                }
                
                if (_isIndicatorShowValue)
                {
                    currData.IndicatorText.text = currData.Name + " " + currData.Value;
                }

                else
                {
                    currData.IndicatorText.text = currData.Name;
                }
            }

            if (_indicatorGo != null)
            {
                SyncIndicatorUI().Forget();
            }
        }
        
        private async UniTaskVoid SyncIndicatorUI()
        {
            await UniTask.Delay(100);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_indicatorGo.GetComponentInParent<RectTransform>());
        }
        #endregion
    }
}