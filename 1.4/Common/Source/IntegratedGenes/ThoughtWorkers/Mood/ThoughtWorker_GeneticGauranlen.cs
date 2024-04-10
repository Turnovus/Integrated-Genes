using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedGenes
{
    class ThoughtWorker_GeneticGauranlen : ThoughtWorker_NeedGenetic
    {
        public override NeedDef Need =>
            MyDefOf.NeedDefOf.Turn_Need_GeneticGauranlen;
    }
}
