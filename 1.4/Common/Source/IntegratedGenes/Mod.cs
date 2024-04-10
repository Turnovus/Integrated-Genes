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
    public class Mod_IntegratedGenes : Mod
    {
        public const string SettingKey = "IntegratedGenesSettings.";
        public const string SettingLabel = ".Label";
        public const string SettingDescription = ".Description";

        public ModSettings_IntegratedGenes Settings => GetSettings<ModSettings_IntegratedGenes>();

        public Mod_IntegratedGenes(ModContentPack content) : base(content) { }

        public override string SettingsCategory() => "IntegratedGenesMod".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            DoSettingsListing(listing, "doUnspawnedPsylink", ref Settings.doUnspawnedPsylink);
            DoSettingsListing(listing, "doUnspawnedLevels", ref Settings.doUnspawnedLevels);
            DoSettingsListing(listing, "doOutsiderLink", ref Settings.doOutsiderLink);
            DoSettingsListing(listing, "doNewColonistLink", ref Settings.doNewColonistLink);

            listing.End();
        }

        private static void DoSettingsListing(
            Listing_Standard list,
            string key,
            ref bool setting
        )
        {
            string label = SettingKey + key + SettingLabel;
            string description = SettingKey + key + SettingDescription;
            list.CheckboxLabeled(label.Translate(), ref setting, description.Translate());
        }
    }

    public class ModSettings_IntegratedGenes : ModSettings
    {
        public bool doUnspawnedPsylink = true;
        public bool doUnspawnedLevels = false;
        public bool doOutsiderLink = true;
        public bool doNewColonistLink = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref doUnspawnedPsylink, "doUnspawnedPsylink");
            Scribe_Values.Look(ref doUnspawnedLevels, "doUnspawnedLevels");
            Scribe_Values.Look(ref doOutsiderLink, "doOutsiderLink");
            Scribe_Values.Look(ref doNewColonistLink, "doNewColonistLink");
        }
    }
}
