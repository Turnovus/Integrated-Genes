using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class StatPart_CertaintyGenetic : StatPart
    {
#pragma warning disable CS0649
        GeneDef gene;
        float factorPrimary;
        float factorSecondary;
#pragma warning restore CS0649

        public bool ActiveForPawn(Pawn p) =>
            p.ideo != null && p.genes != null && p.genes.HasGene(gene);

        public bool IsPawnInMainIdeo(Pawn p) =>
            p.Faction?.ideos?.PrimaryIdeo != null && p.ideo?.Ideo != null && p.Faction.ideos.PrimaryIdeo == p.ideo.Ideo;

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!req.HasThing) return;
            Pawn p = req.Thing as Pawn;
            if (!ActiveForPawn(p)) return;
            val *= IsPawnInMainIdeo(p) ? factorPrimary : factorSecondary;
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (!req.HasThing) return null;
            Pawn p = req.Thing as Pawn;
            if (!ActiveForPawn(p))
                return null;
            string s = "StatPartExplanationGeneticCertainty".Translate(
                gene.LabelCap,
                IsPawnInMainIdeo(p) ?
                    "StatPartExplanationGeneticCertaintyPrimary".Translate() :
                    "StatPartExplanationGeneticCertaintySecondary".Translate()
            );
            s += ": " + (IsPawnInMainIdeo(p) ? factorPrimary : factorSecondary)
                .ToStringByStyle(
                    ToStringStyle.PercentZero, ToStringNumberSense.Factor
                );
            return s;
        }
    }
}
