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

        public DefBlockTypeSet(PuzzleBlockType blockType)
        {
            BlockType = blockType;
        }
    }

    public class  TblBlockTypeSet : TblBase
    {
        PuzzleBlockType BlockType { get; set; }

        public override (int id, DefBase def) Build()
        {
            var defBlockTypeSet = new DefBlockTypeSet(BlockType);
            return (Id, defBlockTypeSet);
        }
    }
}
