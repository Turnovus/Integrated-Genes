using System;
using RimWorld;
using Verse;

namespace IntegratedGenes
{
    class Need_GeneticMeditation : Need_Genetic
    {
        public float solace = 0f;

        public bool IsMeditating
        {
            get
            {
                //Caravan caravan = pawn.GetCaravan();
                //if (caravan != null && !caravan.)
                return pawn.psychicEntropy.IsCurrentlyMeditating;
            }
        }

        public override int GUIChangeArrow
        {
            get
            {
                if (IsFrozen) return 0;
                if (IsMeditating) return 1;
                if (pawn.InBed())
                    return CurLevel < THRESH_MIN + 0.05 ? 1 : 0;
                return -1;
            }
        }

        public override string GetTipString()
        {
            string str = base.GetTipString();

            str += "\n\n";

            str += "NeedGeneMeditationSolace".Translate(CurInstantLevel.ToStringPercent());

            return str;
        }

        public override float CurInstantLevel => solace;

        public Need_GeneticMeditation(Pawn pawn) : base(pawn) { }

        public override void NeedInterval()
        {
            float solaceRate;

            if (IsFrozen) return;

            if (IsMeditating)
            {
                CurLevel += RateRise;
                solaceRate = CurLevel >= 0.98f ? 0.25f : 0.15f;
                solace = Math.Min(1f, solace + RateRise * solaceRate);
                return;
            }

            solace = Math.Max(0, solace - RateFall * 0.25f);

            if (pawn.InBed())
            {
                if (CurLevel < THRESH_MIN + 0.05)
                    CurLevel += RateRise * (IsResting ? 0.25f : 0.5f);
            }
            else if (!IsResting)
            {
                CurLevel = Math.Max(CurLevel - RateFall, solace);
            }
        }
    }
}
