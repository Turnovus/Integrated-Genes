using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class ThoughtWorker_GeneticMeditation : ThoughtWorker_NeedGenetic
    {
        public override NeedDef Need =>
            MyDefOf.Turn_Need_GeneticMeditation;
    }
}
