using System.Collections.Generic;
using RimWorld;
using Verse;

namespace IntegratedGenes
{
    public class Gene_FleshbeastBurster : Gene
    {
        private Extension_FleshbeastBurster Extension => def.GetModExtension<Extension_FleshbeastBurster>();
        
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);

            int spawns = 0;
            foreach (BodyPartRecord part in pawn.def.race.body.AllParts)
            {
                if (!Extension.countParts.Contains(part.def))
                    continue;

                if (TryConsumePart(part))
                    spawns++;
            }
            
            if (!pawn.SpawnedOrAnyParentSpawned)
                return;
            
            IntVec3 position = pawn.PositionHeld;
            Map map = pawn.MapHeld;
            for (int i = 0; i < spawns; i++) SpawnRandomFleshbeast(position, map);
            FleshbeastUtility.MeatExplosionSize size = FleshbeastUtility.ExplosionSizeFor(pawn);
            FleshbeastUtility.MeatSplatter(spawns * 3, position, map, size);
        }

        private bool TryConsumePart(BodyPartRecord part)
        {
            if (pawn.health.hediffSet.PartIsMissing(part))
                return false;

            Hediff addedPart;
            if (pawn.health.hediffSet.TryGetDirectlyAddedPartFor(part, out addedPart))
            {
                if (!addedPart.def.organicAddedBodypart)
                    return false;
            }
            
            if (!Rand.Chance(Extension.spawnChancePerLimb))
                return false;

            return pawn.health.AddHediff(HediffDefOf.MissingBodyPart, part) != null;
        }

        private void SpawnRandomFleshbeast(IntVec3 position, Map map)
        {
            PawnKindDef pawnKind =
                Extension.pawnKindsByWeight.Keys.RandomElementByWeight(p => Extension.pawnKindsByWeight[p]);
            
            // Most of the following is copied from RimWorld.FleshbeastUtility.SpawnFleshbeastFromPawn, but without the
            // corpse-destroying code
            Pawn spawnedPawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, Faction.OfEntities,
                fixedBiologicalAge: 0f, fixedChronologicalAge: 0f));

            CompInspectStringEmergence emergence = spawnedPawn.TryGetComp<CompInspectStringEmergence>();
            if (emergence != null)
                emergence.sourcePawn = pawn;

            GenSpawn.Spawn(spawnedPawn, position, map);
        }
    }

    public class Extension_FleshbeastBurster : DefModExtension
    {
        public Dictionary<PawnKindDef, float> pawnKindsByWeight = new Dictionary<PawnKindDef, float>();
        public List<BodyPartDef> countParts = new List<BodyPartDef>();
        public float spawnChancePerLimb;
    }
}