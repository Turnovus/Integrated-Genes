using RimWorld;
using Verse;

namespace IntegratedGenes
{
    public abstract class ConditionalStatAffector_Ideo : ConditionalStatAffecter
    {
        public abstract bool AffectsPawn(Pawn p);
        
        public override bool Applies(StatRequest req) =>
            req.HasThing && req.Thing is Pawn pawn && AffectsPawn(pawn);
    }

    public class ConditionalStatAffector_MajorityIdeo : ConditionalStatAffector_Ideo
    {
        public override bool AffectsPawn(Pawn p) => StaticUtil.IsPawnInMainIdeo(p);

        public override string Label => "StatsReport_MajorityIdeo".Translate();
    }
    
    public class ConditionalStatAffector_MinorityIdeo : ConditionalStatAffector_Ideo
    {
        public override bool AffectsPawn(Pawn p) => !StaticUtil.IsPawnInMainIdeo(p);

        public override string Label => "StatsReport_MinorityIdeo".Translate();
    }
}