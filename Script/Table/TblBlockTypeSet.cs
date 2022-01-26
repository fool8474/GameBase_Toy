using Script.InGame;
using Script.Manager.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.Table
{
    public class DefBlockTypeSet : DefBase
    {
        public readonly PuzzleBlockType BlockType;

        public DefBlockTypeSet(int id, string blockType) : base(id)
        {
            BlockType = (PuzzleBlockType)Enum.Parse(typeof(PuzzleBlockType), blockType);
        }
    }

    public class  TblBlockTypeSet : TblBase
    {
        public string BlockType { get; set; }

        public override (int id, DefBase def) Build()
        {
            var defBlockTypeSet = new DefBlockTypeSet(Id, BlockType);
            return (Id, defBlockTypeSet);
        }
    }
}
