using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class Need_GeneticGauranlen : Need_Genetic
    {
        //public new const float RATE_FALL = RATE_RISE;

        const int MAX_RETENTION = (int)(GenDate.TicksPerHour * 1.5);
        const int MIN_RETENTION = (int)(GenDate.TicksPerHour * -2.5);

        private int retention = 0;

        public bool IsNearTree
        {
            get
            {
                if (!pawn.Spawned) return false;
                foreach(
                    Thing tree in pawn.Map.listerThings.ThingsOfDef(
                        ThingDefOf.Plant_TreeGauranlen)
                )
                {
                    if (pawn.Position.InHorDistOf(tree.Position, 15f))
                        return true;
                }
                return false;
            }
        }

        public override int GUIChangeArrow
        {
            get
            {
                if (IsFrozen) return 0;
                if (IsNearTree) return 1;
                if (pawn.InBed())
                    return CurLevel < THRESH_LOW + 0.05 ? 1 : 0;
                if (retention > 0) return 0;
                return -1;
            }
        }

        public Need_GeneticGauranlen(Pawn pawn) : base(pawn) { }

        public override void NeedInterval()
        {
            if (IsFrozen) return;

            if (IsNearTree)
            {
                retention = Math.Max(retention, MIN_RETENTION / 2);
                CurLevel += RateRise;
                retention += (int)NEED_INTERVAL;
                retention = Math.Min(retention, MAX_RETENTION);
            }
            else if (pawn.InBed())
            {
                if (CurLevel < THRESH_LOW + 0.05)
                    CurLevel += RateRise * (IsResting ? 0.25f : 0.5f);
            }
            else if (!IsResting)
            {
                retention -= (int)NEED_INTERVAL;
                retention = Math.Max(retention, MIN_RETENTION);
                if (retention < 0)
                    CurLevel -=
                        RateFall * Math.Abs((float)retention / MIN_RETENTION);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref retention, "retention");
        }

        public override string GetTipString()
        {
            if (!DebugSettings.ShowDevGizmos)
                return base.GetTipString();
            return base.GetTipString() +
                "\nRetention : " + retention.ToString();
        }
    }
}
