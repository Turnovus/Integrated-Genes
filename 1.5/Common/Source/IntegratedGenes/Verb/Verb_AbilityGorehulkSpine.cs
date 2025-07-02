using RimWorld;
using Verse;
using LudeonTK;
using RimWorld.Planet;

namespace IntegratedGenes
{
    public class Ability_GorehulkSpine : Ability
    {
        private Extension_AbilityGorehulkSpine Extension =>
            def.GetModExtension<Extension_AbilityGorehulkSpine>();

        private Hediff_Quills Hediff => pawn.health.hediffSet.GetFirstHediffOfDef(Extension.hediffDef) as Hediff_Quills;
        
        public Ability_GorehulkSpine(Pawn pawn) : base(pawn) {}
        public Ability_GorehulkSpine(Pawn pawn, AbilityDef abilityDef) : base(pawn, abilityDef) {}

        private bool HasQuills
        {
            get
            {
                Hediff quills = Hediff;
                return quills != null && quills.Severity >= 1f;
            }
        }

        public override bool CanCast => base.CanCast && HasQuills;

        protected override void PreActivate(LocalTargetInfo? target)
        {
            base.PreActivate(target);
            
            Hediff_Quills quills = Hediff;
            if (quills != null)
                quills.ConsumeQuill();
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (!base.GizmoDisabled(out reason))
                return false;

            if (!HasQuills)
            {
                reason = "TODO";
                return false;
            }

            return true;
        }
    }
    
    public class Verb_AbilityGorehulkSpine : Verb_AbilityShoot
    {
        private Extension_AbilityGorehulkSpine Extension =>
            Ability.def.GetModExtension<Extension_AbilityGorehulkSpine>();

        private Hediff_Quills Hediff => Ability.pawn.health.hediffSet.GetFirstHediffOfDef(Extension.hediffDef) as Hediff_Quills;
        
        public override bool Available()
        {
            if (!base.Available())
                return false;

            Hediff quills = Hediff;
            return quills != null && quills.Severity >= 1f;
        }
        
        

        protected override bool TryCastShot()
        {
            Hediff_Quills quills = Hediff;
            if (quills == null)
                return false;
            
            bool success = base.TryCastShot();
            
            if (success)
                quills.ConsumeQuill();
            return success;
        }
    }

    public class Extension_AbilityGorehulkSpine : DefModExtension
    {
        public HediffDef hediffDef;
    }
}