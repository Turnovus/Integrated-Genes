using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class ThoughtWorker_HatesImprisonment : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.guest?.IsPrisoner != true)
                return ThoughtState.Inactive;

            return p.guest.HostFaction?.IsPlayer == true ?
                ThoughtState.ActiveAtStage(1) :
                ThoughtState.ActiveAtStage(0);
        }
    }
}
