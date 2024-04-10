using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class DefExtension_HediffAnnual : DefModExtension
    {
#pragma warning disable CS0649
        public HediffDef hediffDef;
        public string letterString;
        public float timesPerYear = 1f;
        public int cooldownTicks = 0;
#pragma warning restore CS0649
    }
}
