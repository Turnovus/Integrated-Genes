using System.Collections.Generic;
using System.Linq;
using RimWorld;
using VEF.Abilities;
using Verse;
using AbilityDef = VEF.Abilities.AbilityDef; // Thanks, Sarg.
using VanillaPsycastsExpanded;

namespace IntegratedGenes.VPECompat
{
    public class Gene_PsylinkGiverVPE : Gene_PsylinkGiver
    {
        public override void GiveCompletelyRandomAbility()
        {
            CompAbilities comp = pawn.GetComp<CompAbilities>();
            if (comp == null)
                return;

            Hediff_PsycastAbilities psycast = pawn.Psycasts();
            if (psycast == null)
                return;

            IEnumerable<AbilityDef> obtainableAbilites = GetVPEObtainableAbilities(comp);
            if (obtainableAbilites.EnumerableNullOrEmpty())
                return;

            AbilityDef ability = obtainableAbilites.RandomElementByWeight(a =>
                (float)((psycast.unlockedPaths.Contains(a.Psycast()?.path) ? 20.0 : 1.0) *
                        (this.HasAnyPrereqForAbility(a) ? 20.0 : 1.0))
            );
            comp.GiveAbility(ability);
            SendAbilityLetter(ability.LabelCap, ability.description);
            
            PsycasterPathDef path = AbilityExtensionPsycastUtility.Psycast(ability)?.path;
            if (path == null || psycast.unlockedPaths.Contains(path))
                return;
            psycast.UnlockPath(path);
        }
        
        public bool HasAnyPrereqForAbility(AbilityDef ability)
        {
            CompAbilities comp = pawn.GetComp<CompAbilities>();
            if (comp == null)
                return false;

            AbilityExtension_Psycast psycast = ability.Psycast();
            if (psycast == null || psycast.prerequisites.NullOrEmpty())
                return true;

            foreach (AbilityDef prerequisite in psycast.prerequisites)
            {
                if (comp.HasAbility(prerequisite))
                    return true;
            }

            return false;
        }
        
        public IEnumerable<AbilityDef> GetVPEObtainableAbilities(CompAbilities comp)
        {
            int psylinkLevel = pawn.GetPsylinkLevel();
            
            return comp == null
                ? null
                : DefDatabase<AbilityDef>.AllDefs.ToList().Where(a =>
                {
                    if (a.Psycast() == null)
                        return false;
                    
                    int? level = a.GetModExtension<AbilityExtension_Psycast>()?.level;
                    return level.GetValueOrDefault() <= psylinkLevel && !comp.HasAbility(a);
                });
        }
    }
}