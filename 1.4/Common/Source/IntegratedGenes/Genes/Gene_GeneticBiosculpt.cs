using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class Gene_GeneticBiosculpt : Gene
    {
        public const int INTERVAL = 200;

        private bool isPawnSick = false;
        private HediffDef hediffInt;

        public HediffDef SicknessHediff
        {
            get
            {
                if (hediffInt == null)
                {
                    hediffInt = def.GetModExtension<DefExtension_HediffAnnual>()
                        .hediffDef;

                    if (hediffInt == null)
                    {
                        Log.Error("Biosculpt gene has no hediff!");
                        hediffInt = HediffDefOf.Flu;
                    }
                }
                return hediffInt;
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (!pawn.IsHashIntervalTick(INTERVAL) || pawn.Suspended) return;

            long daysSinceReversal =
                -pawn.ageTracker.AgeReversalDemandedDeadlineTicks /
                GenDate.TicksPerDay;
            if (daysSinceReversal < 5L)
                RemoveHediff();
            else
                AddHediff();
        }

        public override void PostAdd()
        {
            base.PostAdd();
            isPawnSick = false;
        }

        public override void PostRemove()
        {
            base.PostRemove();
            RemoveHediff();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref isPawnSick, "isPawnSick");
        }

        public void AddHediff()
        {
            if (isPawnSick) return;
            pawn.health.AddHediff(SicknessHediff);
            isPawnSick = true;
        }

        public void RemoveHediff()
        {
            if (!isPawnSick) return;
            Hediff hediff = null;
            foreach (Hediff h in pawn.health.hediffSet.hediffs)
                if (hediff.def == SicknessHediff)
                {
                    hediff = h;
                    break;
                }
            if (hediff != null)
                pawn.health.RemoveHediff(hediff);

            isPawnSick = false;
        }
    }
}
