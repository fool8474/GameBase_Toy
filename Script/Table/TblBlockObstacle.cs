using Script.Manager.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Script.Table
{
    public class DefBlockObstacle : DefBase
    {
        public readonly string SpriteName;
        public readonly Color SpriteColor;

        public DefBlockObstacle(int id, string spriteName, float rColor, float gColor, float bColor, float aColor) : base(id)
        {
            SpriteName = spriteName;
            SpriteColor = new Color(rColor, gColor, bColor, aColor);
        }
    }

    public class TblBlockObstacle : TblBase
    {
        public string SpriteName { get; set; }
        public float RColor { get; set; }
        public float GColor { get; set; }
        public float BColor { get; set; }
        public float AColor { get; set; }

        public override (int id, DefBase def) Build()
        {
            var defBlockObstacle = new DefBlockObstacle(Id, SpriteName, RColor, GColor, BColor, AColor);
            return (Id, defBlockObstacle);
        }
    }
}
