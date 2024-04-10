using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.AI;

namespace IntegratedGenes
{
    class JobDriver_Introspect : JobDriver_Meditate
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            IEnumerable<Toil> toils = base.MakeNewToils();

            foreach (Toil toil in toils)
            {

            }

            return toils;
        }
    }
}
