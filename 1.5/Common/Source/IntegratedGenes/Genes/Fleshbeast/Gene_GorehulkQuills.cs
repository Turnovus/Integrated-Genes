using System.Collections.Generic;
using Verse;

namespace IntegratedGenes
{
    public class Gene_GorehulkQuills : Gene
    {
        private Extension_GorehulkQuills Extension => def.GetModExtension<Extension_GorehulkQuills>();

        public override void PostAdd()
        {
            base.PostAdd();
            TouchHediff();
        }

        public override void Tick()
        {
            base.Tick();

            if (pawn.IsHashIntervalTick(Extension.hediffCheckIntervalTicks))
                TouchHediff();
        }

        private void TouchHediff()
        {
            if (pawn.health.hediffSet.HasHediff(Extension.hediffDef))
                return;

            pawn.health.AddHediff(Extension.hediffDef);
        }
    }

    public class Extension_GorehulkQuills : DefModExtension
    {
        public HediffDef hediffDef;
        public int hediffCheckIntervalTicks = 1;
    }
}