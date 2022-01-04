using Script.Manager.CSV;
using Script.Table;

namespace Assets.Script.Table
{
    public class DefStage : DefBase
    {
        public DefStageBlockInfo BlockInfo;
        
        private int _useBlockId;

        public DefStage(int useBlockId)
        {
            _useBlockId = useBlockId;
        }
        
        public override void Build()
        {
            TableMgr.TryGetDefData(_useBlockId, out BlockInfo);
        }
    }

    public class TblStage : TblBase
    {
        public int UseBlockId;

        public override (int id, DefBase def) Build()
        {
            var defStage = new DefStage(UseBlockId);
            return (Id, defStage);
        }
    }
}
