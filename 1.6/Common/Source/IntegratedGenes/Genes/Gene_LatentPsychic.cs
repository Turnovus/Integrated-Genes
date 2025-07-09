using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class Gene_LatentPsychic : Gene
    {
        public const int CheckInterval = 600;

        public float MtbGivePsylink => def.GetModExtension<DefExtension_Float>().value;

        public static void GivePsylink(Pawn pawn)
        {
            if (!pawn.HasPsylink)
                pawn.ChangePsylinkLevel(1);
        }

        public override void PostAdd()
        {
            base.PostAdd();
            if (pawn.relations == null || pawn.relations.everSeenByPlayer)
                return;

            if (Rand.MTBEventOccurs(MtbGivePsylink * 3f, GenDate.TicksPerYear, pawn.ageTracker.AgeBiologicalTicks))
                GivePsylink(pawn);
        }

        public override void TickInterval(int delta)
        {
            if (!pawn.IsHashIntervalTick(CheckInterval, delta))
                return;

            if (Rand.MTBEventOccurs(
                MtbGivePsylink / pawn.genes.BiologicalAgeTickFactor,
                GenDate.TicksPerYear,
                CheckInterval
            ))
                GivePsylink(pawn);
        }
    }
}
