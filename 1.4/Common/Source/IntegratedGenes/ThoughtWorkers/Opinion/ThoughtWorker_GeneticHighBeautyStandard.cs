using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class ThoughtWorker_GeneticHighBeautyStandard : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(
            Pawn pawn,
            Pawn other
        )
        {
            if (!other.RaceProps.Humanlike ||
                !RelationsUtility.PawnsKnowEachOther(pawn, other) ||
                PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn)
            )
                return ThoughtState.Inactive;

            float pawnBeauty = pawn.GetStatValue(StatDefOf.PawnBeauty);
            float otherBeauty = other.GetStatValue(StatDefOf.PawnBeauty);

            if (otherBeauty >= 2f || pawnBeauty <= otherBeauty)
                return ThoughtState.Inactive;

            return ThoughtState.ActiveAtStage(0);
        }
    }
}
