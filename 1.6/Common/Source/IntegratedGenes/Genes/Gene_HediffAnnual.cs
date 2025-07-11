using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class Gene_HediffAnnual : Gene
    {
        public int cooldownTicks = 0;

        DefExtension_HediffAnnual Ext =>
            def.GetModExtension<DefExtension_HediffAnnual>();

        HediffDef SicknessHediff =>
            Ext.hediffDef;

        public string LetterString =>
            Ext.letterString;

        public bool IsPawnSick
        {
            get
            {
                return pawn.health.hediffSet.HasHediff(SicknessHediff);
            }
        }

        public override void TickInterval(int delta)
        {
            cooldownTicks = Math.Max(cooldownTicks - delta, 0);

            if (GenDate.DaysPassed < 40) return;

            if (cooldownTicks <= 0 && Rand.MTBEventOccurs(GenDate.TicksPerYear / Ext.timesPerYear, 1f, 1f))
                MakeSickNow();
        }

        public void MakeSickNow()
        {
            if (IsPawnSick) return;

            pawn.health.AddHediff(SicknessHediff);
            if (!PawnUtility.ShouldSendNotificationAbout(pawn)) return;

            Find.LetterStack.ReceiveLetter(
                "LetterLabelTraitDisease".Translate(SicknessHediff.label),
                "LetterGeneDisease".Translate(
                    pawn.LabelCap, SicknessHediff.label, pawn.Named("PAWN"))
                    .AdjustedFor(pawn) + "\n\n" + LetterString + 
                    "\n\n" + "LetterGeneDiseaseCause".Translate() +
                    def.label,
                LetterDefOf.NegativeEvent,
                pawn);

            cooldownTicks = Ext.cooldownTicks;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (!DebugSettings.ShowDevGizmos) yield break;

            Command_Action commandAction = new Command_Action
            {
                defaultLabel = "DEV: Make Sick",
                action = new Action(() => MakeSickNow())
            };
            yield return commandAction;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref cooldownTicks, "cooldownTicks");
        }
    }
}
