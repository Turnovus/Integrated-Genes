using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class DefExtension_PsychicGene : DefModExtension
    {
#pragma warning disable CS0649
        public float? grantPsylinkAgeYears = null;
        public float triggerIntervalYears = 1f;
        public int? maxLevel = null;
#pragma warning restore CS0649
    }
}
