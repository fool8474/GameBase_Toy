using Script.Manager;
using Script.Manager.CSV;
using Script.Manager.Util.Log;
using Script.Table;
using UnityEngine;
using UnityEngine.UI;

namespace Script.InGame.PuzzleBlock
{
    public class PuzzleBlockObstacle : PuzzleBlockBase
    {
        [SerializeField] private Image _blockImg;

        private DefBlockObstacle _blockData;

        protected override void GetBlockData(int blockId)
        {
            if (TableMgr.TryGetDef(blockId, out _blockData) == false)
            {
                Log.EF(LogCategory.PUZZLE, "Cannot load Obstacle Block Data from table {0}", blockId);
                return;
            }
        }

        protected override void InitializeBlockWithData()
        {
            var sprite = _atlasMgr.GetSprite(AtlasType.UI_PUZZLE, _blockData.SpriteName);

            _blockImg.sprite = sprite;
            _blockImg.color = _blockData.SpriteColor;
        }
    }
}
