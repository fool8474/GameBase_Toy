using Script.Manager.CSV;
using Script.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.Table
{
    public class DefStageBlockInfo : DefBase
    {
        public readonly List<int> BlockIdList;
        public readonly List<int> ProbabilityList;

        public DefStageBlockInfo(int id, List<int> blockIdList, List<int> probabilityList) : base(id)
        {
            BlockIdList = blockIdList;
            ProbabilityList = probabilityList;
        }
    }

    public class TblStageBlockInfo : TblBase
    {
        public string BlockIdList { get; set; }
        public string ProbabilityList { get; set; }

        public override (int id, DefBase def) Build()
        {
            var seps = new char[] { '/' };

            var defStageBlockInfo = new DefStageBlockInfo(
                Id,
                StringUtil.SplitToList<int>(seps, BlockIdList),
                StringUtil.SplitToList<int>(seps, ProbabilityList));
            
            return (Id, defStageBlockInfo);
        }
    }
}
