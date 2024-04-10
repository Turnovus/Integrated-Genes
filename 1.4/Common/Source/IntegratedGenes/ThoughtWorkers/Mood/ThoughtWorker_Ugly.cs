using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class ThoughtWorker_Ugly : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            float beauty = p.GetStatValue(StatDefOf.PawnBeauty);

            if (beauty >= 0) return ThoughtState.Inactive;
            if (beauty >= -1.5f) return ThoughtState.ActiveAtStage(0);
            return ThoughtState.ActiveAtStage(1);
        }
    }
}
