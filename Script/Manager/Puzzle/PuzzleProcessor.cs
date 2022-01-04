using Assets.Script.Table;
using Script.Manager.CSV;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.Manager.Puzzle
{
    public class PuzzleProcessor : ScriptMgr 
    {
        private DefStage currentStage;

        public void SetStage(int stageId)
        {
            if(TableMgr.TryGetDefData(stageId, out currentStage) == false)
            {
                Log.EF(LogCategory.PUZZLE, "Cannot load stage from stageId {0}", stageId);
                return;
            }
        }

        public int GetNewPuzzleBlock()
        {
            for (int i = 0; i < currentStage.BlockInfo.BlockIdList.Count; i++)
            {
                // TODO
            }

            return 0;
        }
    }
}
