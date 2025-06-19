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
    class MentalBreakWorker_SlaveOrPrisoner : MentalBreakWorker
    {
        public override bool BreakCanOccur(Pawn pawn) =>
            pawn.Spawned &&
            base.BreakCanOccur(pawn) &&
            //HasGene(pawn)  &&
            (pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony);

        public bool HasGene(Pawn pawn)
        {
            GeneDef gene = def.requiredGene;
            if (gene == null)
                return false;

            return pawn.genes?.HasActiveGene(gene) == true;
        }
    }
}
