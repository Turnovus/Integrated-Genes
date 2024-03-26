using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class Hediff_InspiringLeadership : HediffWithComps
    {
        public static Dictionary<Pawn, float> inspirations = new Dictionary<Pawn, float>();

        public override bool ShouldRemove
        {
            get
            {
                foreach (HediffComp comp in comps)
                {
                    if (comp.CompShouldRemove)
                        return true;
                }
                return inspirations.NullOrEmpty();
            }
        }

        public override string LabelInBrackets => "InspiredLeadershipLabelBrackets".Translate(base.LabelInBrackets, Severity.ToStringPercent());

        public override string GetTooltip(Pawn pawn, bool showHediffsDebugInfo)
        {
            StringBuilder str = new StringBuilder();
            str.Append(base.GetTooltip(pawn, showHediffsDebugInfo));
            str.AppendLine();

            foreach (Pawn p in inspirations.Keys)
                str.AppendLine("InspiredLeadershipInspirationListing".Translate(p.LabelShortCap, inspirations[p].ToStringPercent()));

            return str.ToString();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref inspirations, "inspirations", LookMode.Reference);
        }

        public void AddSource(Pawn from, float effect)
        {
            inspirations[from] = effect;
            BumpTimer();
            AdjustSeverity();
        }

        public void BumpTimer()
        {
            HediffComp_Disappears comp = this.TryGetComp<HediffComp_Disappears>();
            if (comp == null)
                return;

            comp.ticksToDisappear = Math.Max(comp.ticksToDisappear, (int)(comp.disappearsAfterTicks * 0.33f));
        }

        public void AdjustSeverity()
        {
            float n = 0;
            foreach (Pawn p in inspirations.Keys)
                n += inspirations[p];
            Severity = n;
        }
    }
}
