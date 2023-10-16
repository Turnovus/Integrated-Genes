using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class ThoughtWorker_IsEnslaved : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            return p.IsSlaveOfColony ? ThoughtState.ActiveAtStage(0) : ThoughtState.Inactive;
        }
    }
}
