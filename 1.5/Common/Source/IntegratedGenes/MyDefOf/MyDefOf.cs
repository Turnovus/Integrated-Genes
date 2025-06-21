using RimWorld;
using Verse;

namespace IntegratedGenes
{
    [DefOf]
    public static class MyDefOf
    {
#pragma warning disable CS0649
        [MayRequireIdeology]
        public static HediffDef Turn_Hediff_TerrifiedFaintingSpell;
        
        public static MentalBreakDef Turn_MentalBreak_TerrifiedFaintingSpell;
        
        [MayRequireRoyalty]
        public static NeedDef Turn_Need_GeneticMeditation;
        [MayRequireIdeology]
        public static NeedDef Turn_Need_GeneticGauranlen;
        
        public static StatDef Turn_Stat_TerrifiedFaintingInterval;
#pragma warning restore CS0649

        static MyDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MyDefOf));
        }
    }
}