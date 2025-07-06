using System;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    public class StatWorker_Fainting : StatWorker
    {
        private const float TerrorIntervalDenominator = 1.15f;
        // From RimWorld.TerrorUtility
        private const int MaxTerror = 100;
        
        public override bool ShouldShowFor(StatRequest req)
        {
            GeneDef geneDef = stat.GetModExtension<DefExtension_GeneDef>().geneDef;
            Pawn pawn = req.Thing as Pawn;
            
            return pawn?.genes?.HasActiveGene(geneDef) ?? false;
        }

        public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
        {
            // We won't bother calling the super method because this stat needn't be affected by the usual factors.
            return stat.defaultBaseValue;
        }

        public override void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
        {
            base.FinalizeValue(req, ref val, applyPostProcess);

            if (req.Thing is Pawn pawn)
                val *= GetTerrorIntervalFactor(pawn);
        }

        public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
        {
            string explanation = base.GetExplanationUnfinalized(req, numberSense);

            if (req.Thing is Pawn pawn)
            {
                explanation += StatDefOf.Terror.LabelCap;
                explanation += $" ({GetTerrorLevelNonSlave(pawn)}%): {GetTerrorIntervalFactor(pawn).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Factor)}";
                explanation += "\n";
            }

            return explanation;
        }

        public override string ValueToString(float val, bool finalized, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
        {
            return ((int)(val * GenDate.TicksPerDay)).ToStringTicksToPeriod();
        }

        private static float GetTerrorIntervalFactor(Pawn pawn)
        {
            float terrorPercent = GetTerrorLevelNonSlave(pawn) / (MaxTerror * TerrorIntervalDenominator);
            return 1 - terrorPercent;
        }
        
        // Partially copied from RimWorld.TerrorUtility
        private static int GetTerrorLevelNonSlave(Pawn pawn)
        {
            if (pawn.needs?.mood?.thoughts.memories.Memories == null)
                return 0;
            
            int a = 0;
            foreach (Thought_MemoryObservationTerror terrorThought in TerrorUtility.GetTerrorThoughts(pawn))
                a += terrorThought.intensity;
            return Math.Min(a, MaxTerror);
        }
    }
}