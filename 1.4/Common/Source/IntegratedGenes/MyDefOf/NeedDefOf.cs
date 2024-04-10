using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;

namespace IntegratedGenes.MyDefOf
{
    [DefOf]
    class NeedDefOf
    {
#pragma warning disable CS0649
        [MayRequireRoyalty]
        public static NeedDef Turn_Need_GeneticMeditation;
        [MayRequireIdeology]
        public static NeedDef Turn_Need_GeneticGauranlen;
#pragma warning restore CS0649

        static NeedDefOf() =>
            DefOfHelper.EnsureInitializedInCtor(typeof(NeedDefOf));
    }
}
