using Script.Manager.CSV;
using Script.Table;

namespace Script.Table
{
    public class DefStage : DefBase
    {
        public DefStageBlockInfo BlockInfo;
        
        private int _useBlockId;

        public DefStage(int id, int useBlockId) : base(id)
        {
            _useBlockId = useBlockId;
        }
        
        public override void Build()
        {
            TableMgr.TryGetDef(_useBlockId, out BlockInfo);
        }
    }

    public class TblStage : TblBase
    {
        public int UseBlockId { get; set; }

        public override (int id, DefBase def) Build()
        {
            var defStage = new DefStage(Id, UseBlockId);
            return (Id, defStage);
        }
    }
}
