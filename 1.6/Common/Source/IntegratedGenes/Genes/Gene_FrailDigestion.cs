using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace IntegratedGenes
{
    class Gene_FrailDigestion : Gene
    {
        public float VomitChance => def.GetModExtension<DefExtension_Float>().value;

        public static void TryForceVomit(Pawn pawn)
        {
            if (!pawn.Spawned)
            {
                // Simulate vomiting for non-spawned pawns
                Need_Food food = pawn.needs.food;
                if (food != null)
                    food.CurLevel *= 0.8f;
                return;
            }
            
            Job vomit = JobMaker.MakeJob(JobDefOf.Vomit);
            pawn.jobs.StartJob(vomit, JobCondition.InterruptForced, resumeCurJobAfterwards: true);
        }

        public override void Notify_IngestedThing(Thing thing, int numTaken)
        {
            base.Notify_IngestedThing(thing, numTaken);

            if (thing.def.IsRawHumanFood() && Rand.Chance(VomitChance))
                TryForceVomit(pawn);
        }
    }
}
