using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    public static class IntegratedFleshbeastUtility
    {
        public static void DoMutation(Pawn pawn, BodyPartRecord part, HediffDef mutationDef)
        {
            MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, pawn.PositionHeld, pawn.MapHeld);
            pawn.health.RestorePart(part);
            pawn.health.AddHediff(mutationDef, part);

            if (pawn.MapHeld == null || pawn.PositionHeld == null)
                return;

            for (int index = 0; index < 3; ++index)
                pawn.health.DropBloodFilth();

            FleshbeastUtility.MeatSplatter(3, pawn.PositionHeld, pawn.MapHeld);
        }
    }
}
