using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes.MyDefOf
{
    [DefOf]
    class HediffDefOf
    {
#pragma warning disable CS0649
        [MayRequireIdeology]
        public static HediffDef Turn_Hediff_TerrifiedFaintingSpell;
#pragma warning restore CS0649

        static HediffDefOf() =>
            DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf));
    }
}
