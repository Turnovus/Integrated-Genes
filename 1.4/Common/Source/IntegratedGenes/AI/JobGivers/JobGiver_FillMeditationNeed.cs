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
    class JobGiver_FillMeditationNeed : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Need_GeneticMeditation need = pawn.needs.TryGetNeed<Need_GeneticMeditation>();

            bool shouldMeditateNormally =
                    (pawn.psychicEntropy == null ||
                        pawn.psychicEntropy.CurrentPsyfocus < Math.Min(
                            0.8f, pawn.psychicEntropy.TargetPsyfocus
                        )
                    ) &&
                    (need == null || need.CurLevel > Need_Genetic.THRESH_LOW);

            if (shouldMeditateNormally)
                return MeditationUtility.GetMeditationJob(pawn);
            return GetIntrospectionJob(pawn);
        }

        public static Job GetIntrospectionJob(Pawn pawn)
        {
            MeditationSpotAndFocus spot = MeditationUtility.FindMeditationSpot(pawn);
            if (!spot.IsValid)
                return null;

            JobDef def = JobDefOf.Meditate; // TODO: Reign

            Job job = JobMaker.MakeJob(def, spot.spot, null, spot.focus);

            return job;
        }
    }
}
