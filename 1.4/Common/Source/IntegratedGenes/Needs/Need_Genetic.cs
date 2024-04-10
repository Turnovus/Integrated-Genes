using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace IntegratedGenes
{
    abstract class Need_Genetic : Need
    {
        public const float NEED_INTERVAL = 150f;
        public const float BASE_RATE =
            NEED_INTERVAL / GenDate.TicksPerDay;
        public const float THRESH_CRIT = 0.1f;
        public const float THRESH_LOW = 0.2f;
        public const float THRESH_MIN = 0.4f;
        public const float THRESH_HIGH = 0.8f;

        public int? NeedStage
        {
            get
            {
                if (curLevelInt <= THRESH_CRIT) return 0;
                if (curLevelInt <= THRESH_LOW) return 1;
                if (curLevelInt <= THRESH_MIN) return 2;
                if (curLevelInt > THRESH_HIGH) return 3;
                return null;
            }
        }

        public bool IsResting =>
            pawn.needs.rest != null && pawn.needs.rest.Resting;

        public DefExtension_NeedGenetic Ext =>
            def.GetModExtension<DefExtension_NeedGenetic>();

        public float RateFall => BASE_RATE * (Ext?.fallFactor ?? 1f);
        public float RateRise => BASE_RATE * (Ext?.riseFactor ?? 1f);

        public Need_Genetic(Pawn pawn)
            : base(pawn)
        {
            threshPercents = new List<float>()
            {
                THRESH_CRIT,
                THRESH_LOW,
                THRESH_MIN,
                THRESH_HIGH,
            };
        }

        public override void SetInitialLevel() => CurLevel = 0.75f;
    }
}
