using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using RimWorld.Planet;
//using VanillaPsycastsExpanded;

namespace IntegratedGenes
{
    public class Gene_PsylinkGiver : Gene
    {
        public const int CheckInterval = 60;

        DefExtension_PsychicGene Ext =>
            def.GetModExtension<DefExtension_PsychicGene>();

        public float TriggerIntervalYears => Ext?.triggerIntervalYears ?? 1f;

        public float GrantPsylinkAgeYears => Ext?.grantPsylinkAgeYears ?? 1f;

        public bool CanGiveNewPsycast => Ext?.grantPsylinkAgeYears != null;

        public int NumAbilites => pawn.abilities.abilities.Count();

        public bool PlayerCarrier => pawn.Faction?.IsPlayer == true;

        public bool IsCarrierInBackground =>
            pawn.SpawnedOrAnyParentSpawned || pawn.IsPlayerControlledCaravanMember();

        public double GiveAbilityMtbFactor =>
            //This curve means the first ability will take x1 years to occur,
            // while the last one (29th) will take x7.5 years
            Math.Pow(2D/1.5D, Math.Floor(NumAbilites / 4D));

        public override void Tick()
        {
            base.Tick();

            if (!pawn.IsHashIntervalTick(CheckInterval))
                return;

            if (!IsCarrierInBackground || StaticUtil.Settings.doUnspawnedLevels)
            {
                if (Rand.MTBEventOccurs(GenDate.TicksPerYear * TriggerIntervalYears, 1f, CheckInterval))
                    UpgradePsylink();

                if (Rand.MTBEventOccurs(
                    GenDate.TicksPerYear * TriggerIntervalYears * (float)GiveAbilityMtbFactor,
                    1f, CheckInterval
                ))
                {
                    GiveCompletelyRandomAbility();
                }
            }

            if (CanGiveNewPsycast && (!IsCarrierInBackground || StaticUtil.Settings.doUnspawnedPsylink) &
                Rand.MTBEventOccurs (
                    GrantPsylinkAgeYears / pawn.genes.BiologicalAgeTickFactor,
                    GenDate.TicksPerYear,
                    CheckInterval
                )
            )
            {
                GiveNewPsylink(pawn);
            }
        }

        public override void PostAdd()
        {
            base.PostAdd();

            if (!CanGiveNewPsycast)
                return;

            if (pawn.relations == null || pawn.relations.everSeenByPlayer)
                return;

            if (
                (PlayerCarrier && !StaticUtil.Settings.doNewColonistLink) ||
                (!PlayerCarrier && !StaticUtil.Settings.doOutsiderLink)
            )
            {
                return;
            }

            if (Rand.MTBEventOccurs(GrantPsylinkAgeYears * 3f, GenDate.TicksPerYear, pawn.ageTracker.AgeBiologicalTicks))
                GiveNewPsylink(pawn);
        }

        public virtual void UpgradePsylink()
        {
            if (!pawn.HasPsylink) return;
            pawn.ChangePsylinkLevel(1);
        }

        public virtual void GiveCompletelyRandomAbility()
        {
            IEnumerable<AbilityDef> abilities = GetAllObtainableAbilities();
            if (abilities.EnumerableNullOrEmpty()) return;

            AbilityDef ability = abilities.RandomElement();
            pawn.abilities.GainAbility(ability);

            SendAbilityLetter(ability.LabelCap, ability.description);
        }

        public virtual IEnumerable<AbilityDef> GetAllObtainableAbilities()
        {
            return DefDatabase<AbilityDef>.AllDefs.ToList().Where( a =>
                a.level > 0 &&
                a.level <= pawn.GetPsylinkLevel() &&
                !pawn.abilities.abilities.Any(b => b.def == a));
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (!DebugSettings.ShowDevGizmos) yield break;

            Command_Action linkAction = new Command_Action
            {
                defaultLabel = "DEV: Upgrade Psylink",
                action = new Action(() => UpgradePsylink())
            };
            yield return linkAction;
            Command_Action newLinkAction = new Command_Action
            {
                defaultLabel = "DEV: Grant Psylink",
                action = new Action(() => GiveNewPsylink(pawn))
            };
            Command_Action abilityAction = new Command_Action
            {
                defaultLabel = "DEV: Random Ability",
                action = new Action(() => GiveCompletelyRandomAbility())
            };
            yield return abilityAction;
        }

        public void SendAbilityLetter(string name, string desc)
        {
            if (!PawnUtility.ShouldSendNotificationAbout(pawn)) return;

            Find.LetterStack.ReceiveLetter(
                "LetterLabelGeneGaveAbility".Translate(pawn.LabelShortCap),
                "LetterGeneGaveAbility".Translate(
                    pawn.LabelShortCap, name, desc, def.LabelCap
                ),
                LetterDefOf.PositiveEvent,
                pawn
            );
        }

        public static void GiveNewPsylink(Pawn pawn)
        {
            if (!pawn.HasPsylink)
                pawn.ChangePsylinkLevel(1);
        }
    }
}
