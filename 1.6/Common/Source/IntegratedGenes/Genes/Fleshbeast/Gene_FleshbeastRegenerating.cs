using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using LudeonTK;
using Verse;

namespace IntegratedGenes
{
    public class Gene_FleshbeastRegenerating : Gene
    {
        private const int Interval = 1800;

        public class RegenerationGroup
        {
            public BodyPartDef bodyPart;
            public List<HediffDef> mutations = new List<HediffDef>();
        }
        
        private Extension_FleshbeastRegenerating Extension => def.GetModExtension<Extension_FleshbeastRegenerating>();
        
        public override void TickInterval(int delta)
        {
            if (pawn.IsHashIntervalTick(Interval, delta))
                RunRegeneration();
        }

        private void RunRegeneration(bool forced = false)
        {
            foreach (BodyPartRecord part in pawn.def.race.body.AllParts)
            {
                if (!Extension.mutations.Any(group => part.def == group.bodyPart))
                    continue;
                
                if (CanRegeneratePart(part))
                    TryRegeneratePart(part, forced);
            }
        }

        private bool CanRegeneratePart(BodyPartRecord part)
        {
            if (pawn.health.hediffSet.PartIsMissing(part))
                return true;
            
            float partHealth = pawn.health.hediffSet.GetPartHealth(part) / part.def.GetMaxHealth(pawn);
            if (partHealth > Extension.regenerationHealthThreshold)
                return false;
            
            return !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part);
        }

        private void TryRegeneratePart(BodyPartRecord part, bool forced = false)
        {
            if (forced || Rand.MTBEventOccurs(Extension.limbRegenerationMtbDays, GenDate.TicksPerDay, Interval))
                RegeneratePart(part);
        }

        private void RegeneratePart(BodyPartRecord part)
        {
            RegenerationGroup regeneration = Extension.mutations.FirstOrFallback(group => group.bodyPart == part.def);
            if (regeneration == null)
            {
                Log.Error("Tried to regenerate part with no defined mutations: " + part.def);
                return;
            }

            HediffDef selectedMutation = regeneration.mutations.RandomElementWithFallback();
            if (selectedMutation == null)
            {
                Log.Error("No mutation found for: " + part.def);
                return;
            }
            
            IntegratedFleshbeastUtility.DoMutation(pawn, part, selectedMutation);
            
            TaggedString mutationMessage = "MessageGeneMutantRegeneration".Translate(
                pawn.Named("PAWN"),
                part.Label,
                selectedMutation.label);
            Messages.Message(mutationMessage, pawn, MessageTypeDefOf.NeutralEvent);
        }
        
        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (!DebugSettings.ShowDevGizmos) yield break;

            yield return new Command_Action
            {
                defaultLabel = "DEV: Run Regeneration",
                action = new Action(() => RunRegeneration()),
            };
            yield return new Command_Action
            {
                defaultLabel = "DEV: Force Regeneration",
                action = new Action(() => RunRegeneration(true)),
            };
        }
    }
    
    public class Extension_FleshbeastRegenerating : DefModExtension
    {
        /// <summary>
        /// The maximum combined health of a limb or organ (including child parts) to allow regeneration
        /// </summary>
        public float regenerationHealthThreshold;
        public float limbRegenerationMtbDays;

        public List<Gene_FleshbeastRegenerating.RegenerationGroup> mutations =
            new List<Gene_FleshbeastRegenerating.RegenerationGroup>();
    }
}