using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class DefExtension_LeadershipTuning : DefModExtension
    {
#pragma warning disable CS0649
        public GeneDef requiredGene;
        public HediffDef hediff;
        public float interactionWeight;
        public float offsetForTitleLevel;
        public float offsetForFactionLeader;
        public float offsetForIdeoMoralist;
        public float offsetForIdeoSpecialist;
#pragma warning restore CS0649
    }
}
