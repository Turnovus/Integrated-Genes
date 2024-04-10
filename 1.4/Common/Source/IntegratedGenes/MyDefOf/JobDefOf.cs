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
    class JobDefOf
    {
#pragma warning disable CS0649
        //[MayRequireRoyalty]
        //public static JobDef Turn_Job_Introspect;
#pragma warning restore CS0649

        static JobDefOf() =>
            DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
    }
}
