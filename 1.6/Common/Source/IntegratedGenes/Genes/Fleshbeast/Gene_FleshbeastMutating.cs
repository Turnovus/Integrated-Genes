using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;

namespace IntegratedGenes
{
    public class Gene_FleshbeastMutating : Gene
    {
        private const int CheckIntervalTicks = 6000;

        public class MutationGroup
        {
            public BodyPartDef bodyPart;
            public List<HediffDef> randomMutations = new List<HediffDef>();
            public List<HediffDef> otherMutations = new List<HediffDef>();

            public List<HediffDef> AllMutations
            {
                get
                {
                    List<HediffDef> result = new List<HediffDef>();
                    result.AddRange(randomMutations);
                    result.AddRange(otherMutations);
                    return result;
                }
            }
        }

        public Extension_FleshbeastMutating Extension => def.GetModExtension<Extension_FleshbeastMutating>();

        public List<MutationGroup> DynamicMutationGroups =>
            Extension.mutations.Where(m => !m.randomMutations.Empty()).ToList();

        private bool HasAnyMutation
        {
            get
            {
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                    if (hediff.Part != null && IsMutation(hediff.def, hediff.Part))
                        return true;
                return false;
            }
        }

        private void TryMutate()
        {
            MutationGroup mutation = Extension.mutations.RandomElement();
            IEnumerable<BodyPartRecord> possibleParts = pawn.RaceProps.body.AllParts.Where(r => r.def == mutation.bodyPart);

            if (possibleParts.EnumerableNullOrEmpty())
                return;

            BodyPartRecord targetPart = possibleParts.RandomElement();
            HediffDef desiredMutation = mutation.randomMutations.RandomElement();
            if (!CanMutate(targetPart, desiredMutation))
                return;

            HediffDef removedMutation = ClearMutations(targetPart);
            string removedString = removedMutation?.label ?? targetPart.Label;
            
            IntegratedFleshbeastUtility.DoMutation(pawn, targetPart, desiredMutation);
            if (!PawnUtility.ShouldSendNotificationAbout(pawn))
                return;

            TaggedString mutationMessage = "MessageGeneCausedMutation".Translate(
                pawn.Named("PAWN"),
                removedString,
                desiredMutation.label);
            Messages.Message(mutationMessage, pawn, MessageTypeDefOf.NeutralEvent);
        }

        private bool CanMutate(BodyPartRecord part, HediffDef desiredMutation)
        {
            if (!pawn.health.hediffSet.TryGetDirectlyAddedPartFor(part, out Hediff addedPart))
                return true;
            return addedPart.def != desiredMutation && IsMutation(addedPart.def, part);
        }

        private MutationGroup MutationsForPart(BodyPartDef part)
        {
            foreach (MutationGroup mutation in Extension.mutations)
                if (mutation.bodyPart == part)
                    return mutation;
            return null;
        }

        private bool IsMutation(HediffDef def, BodyPartRecord part)
        {
            MutationGroup mutation = MutationsForPart(part.def);
            if (mutation == null)
                return false;
            return mutation.AllMutations.Contains(def);
        }

        private HediffDef ClearMutations(BodyPartRecord part)
        {
            HediffDef lastRemoved = null;
            for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
            {
                Hediff hediff = pawn.health.hediffSet.hediffs[i];
                if (hediff.Part != part || !IsMutation(hediff.def, part))
                    continue;
                lastRemoved = hediff.def;
                pawn.health.RemoveHediff(hediff);
            }
            return lastRemoved;
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (!DebugSettings.ShowDevGizmos) yield break;

            yield return new Command_Action
            {
                defaultLabel = "DEV: Try Mutate",
                action = new Action(() => TryMutate()),
            };
        }

        public override void TickInterval(int delta)
        {
            if (!pawn.IsHashIntervalTick(CheckIntervalTicks, delta))
                return;

            if (!Rand.MTBEventOccurs(Extension.mutationMtbYears, GenDate.TicksPerYear, CheckIntervalTicks))
                return;

            TryMutate();
        }

        public override void PostAdd()
        {
            base.PostAdd();

            if (pawn.relations == null || pawn.relations.everSeenByPlayer)
                return;

            float yearsMutating = pawn.ageTracker.AgeBiologicalYearsFloat - def.minAgeActive;
            if (yearsMutating <= 0)
                return;

            float yearsPerMutation = Extension.mutationMtbYears * Extension.backgroundPawnMutationMtbFactor;
            float simMutationsPerInterval = Extension.backroundMutationPerIntervalRandomRange.RandomInRange;
            float mutationIntervals = yearsMutating / yearsPerMutation;
            int numMutations = Mathf.RoundToInt(simMutationsPerInterval * mutationIntervals);

            for (int i = 0; i < numMutations; i++)
                TryMutate();
        }
    }

    public class Extension_FleshbeastMutating : DefModExtension
    {
        public List<Gene_FleshbeastMutating.MutationGroup> mutations = new List<Gene_FleshbeastMutating.MutationGroup>();
        public float mutationMtbYears = 1f;
        public float backgroundPawnMutationMtbFactor = 1f;
        public FloatRange backroundMutationPerIntervalRandomRange;
    }
}
