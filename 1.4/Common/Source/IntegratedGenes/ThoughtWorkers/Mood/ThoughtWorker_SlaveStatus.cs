using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class ThoughtWorker_SlaveStatus : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            // Prisoners don't care
            if (p.IsPrisoner) return ThoughtState.Inactive;
            // Slaves are happy
            if (p.IsSlave) return ThoughtState.ActiveAtStage(1);
            // Everyone else is sad
            return ThoughtState.ActiveAtStage(0);
        }
    }
}
