using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raid.DataModel;

namespace RaidArtifactsFilter
{
    public class LocalizationHelper
    {
        private static IReadOnlyDictionary<string, string> _localization;

        public static void Init(IReadOnlyDictionary<string, string> localization)
        {
            _localization = localization;
        }

        public static string GetShortStat(string statKindId)
        {
            if (statKindId == "Defense")
                statKindId = "Defence";
            var key = $"l10n:hero-stats-decription/short/StatKindId?id={statKindId}#label";
            return _localization[key];
        }
    }
}
