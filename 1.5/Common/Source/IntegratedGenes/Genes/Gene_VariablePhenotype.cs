using System.Collections.Generic;
using System.Linq;
using Verse;

namespace IntegratedGenes
{
    public class Gene_VariablePhenotype : Gene
    {
        private const int MinMetabolism = -5;
        
        private bool generated = false;

        private Extension_VariablePhenotype Extension => def.GetModExtension<Extension_VariablePhenotype>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref generated, "variablePhenotypeGenerated");
        }

        public override void Tick()
        {
            if (generated)
                return;
            
            DoGeneGeneration();
            generated = true;
        }

        private void DoGeneGeneration()
        {
            int geneCount = Extension.xenogeneCount.RandomInRange;
            for (int i = 0; i < geneCount; i++)
            {
                IEnumerable<GeneDef> validGeneDefs =
                    DefDatabase<GeneDef>.AllDefsListForReading.Where(CanPickRandomGene);

                GeneDef[] geneDefs = validGeneDefs as GeneDef[] ?? validGeneDefs.ToArray();
                if (geneDefs.EnumerableNullOrEmpty())
                    break;
                
                GeneDef chosenDef = geneDefs.RandomElementWithFallback();

                if (chosenDef == null)
                    break;

                pawn.genes.AddGene(chosenDef, true);
            }
        }

        private bool CanPickRandomGene(GeneDef geneDef)
        {
            if (!geneDef.passOnDirectly || !geneDef.canGenerateInGeneSet)
                return false;

            if (geneDef.biostatArc > 0)
                return false;

            if (geneDef == def)
                return false;

            if (geneDef.biostatMet + GetBiostatMet(pawn) < MinMetabolism)
                return false;

            if (geneDef.prerequisite != null && !pawn.genes.HasActiveGene(geneDef.prerequisite))
                return false;
            
            if (pawn.genes.HasEndogene(geneDef) || pawn.genes.HasXenogene(geneDef))
                return false;
            
            return true;
        }

        private int GetBiostatMet(Pawn pawn)
        {
            int x = 0;
            foreach (Gene gene in pawn.genes.GenesListForReading)
                if (!gene.Overridden)
                    x += gene.def.biostatMet;
            return x;
        }
    }

    public class Extension_VariablePhenotype : DefModExtension
    {
        public IntRange xenogeneCount;
    }
}