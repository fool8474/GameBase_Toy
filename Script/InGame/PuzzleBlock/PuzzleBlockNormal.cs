using Script.Inject;
using Script.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Script.InGame.PuzzleBlock
{
    public class PuzzleBlockNormal : PuzzleBlockBase
    {
        [SerializeField] private Image _blockImg;
        
        private AtlasMgr _atlasMgr;

        protected override void InitializeSpecificBlock(List<string> valList) 
        {
            _atlasMgr = Injector.GetInstance<AtlasMgr>();

            var sprite = _atlasMgr.GetSprite(AtlasType.UI_PUZZLE, valList[0]);

            _blockImg.sprite = sprite;
            _blockImg.color = new Color(float.Parse(valList[1]), float.Parse(valList[2]), float.Parse(valList[3]), 117);
        }
    }
}
