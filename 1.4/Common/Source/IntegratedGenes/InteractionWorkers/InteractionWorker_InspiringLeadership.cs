using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    class InteractionWorker_InspiringLeadership : InteractionWorker
    {

        public DefExtension_LeadershipTuning Extension => interaction.GetModExtension<DefExtension_LeadershipTuning>();

        public GeneDef RequiredGene => Extension?.requiredGene;

        public float InteractionWeight => Extension?.interactionWeight ?? 1f;

        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            if (RequiredGene == null)
                return 0f;
            if (
                initiator.genes?.HasGene(RequiredGene) != true ||
                recipient.genes?.HasGene(RequiredGene) == true
            )
                return 0f;

            if (!initiator.HasAnyTitleOrRole())
                return 0f;

            return InteractionWeight;
        }

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
        {
            base.Interacted(initiator, recipient, extraSentencePacks, out letterText, out letterLabel, out letterDef, out lookTargets);

            Hediff hediff = recipient.health.hediffSet.GetFirstHediffOfDef(Extension.hediff);
            if (hediff == null)
                hediff = recipient.health.AddHediff(Extension.hediff);

            Hediff_InspiringLeadership leadership = hediff as Hediff_InspiringLeadership;
            if (leadership == null)
                return;

            leadership.AddSource(initiator, GetWorkSpeedOffsetFrom(initiator));
        }

        public float GetWorkSpeedOffsetFrom(Pawn pawn)
        {
            float n = 0f;

            StaticUtil.ERoleType roleType = pawn.RoleType();
            switch (roleType)
            {
                case StaticUtil.ERoleType.Leader:
                    n += Extension.offsetForFactionLeader;
                    break;
                case StaticUtil.ERoleType.Moralist:
                    n += Extension.offsetForIdeoMoralist;
                    break;
                case StaticUtil.ERoleType.Specialist:
                    n += Extension.offsetForIdeoSpecialist;
                    break;
                case StaticUtil.ERoleType.None:
                    break;
            }

            n += pawn.RoyalTitleLevel() * Extension.offsetForTitleLevel;

            return n;
        }
    }
}
