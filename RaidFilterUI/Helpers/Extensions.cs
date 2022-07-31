using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raid.DataModel;
using RaidArtifactsFilter;
using RaidArtifactsFilter.Extensions;
using RaidFilterUI.Forms;

namespace RaidFilterUI.Helpers
{
    public static class Extensions
    {
        public static ArtifactControlModel ToArtifactControlModel(this Artifact artifact)
        {

            return new ArtifactControlModel()
            {
                Level = artifact.Level,
                Rank = artifact.GetRankNumber(),
                Rarity = artifact.GetRarityColor(),
                ImgName = $"{artifact.GetSetKind()}_{(artifact.KindId != "Cloak" ? artifact.KindId : "Amulett")}.png",
                Stats = artifact.SecondaryBonuses
                    .Reverse()
                    .Append(artifact.PrimaryBonus)
                    .Reverse()
                    .Select(stat => new StatControlModel(
                        LocalizationHelper.GetShortStat(stat.KindId),
                        stat.Value,
                        stat.Absolute,
                        stat.Level,
                        stat.GlyphPower))
                    .ToArray()
            };
        }
    }
}
