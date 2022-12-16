using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HellHades.ArtifactExtractor.Models;
using RaidArtifactsFilter;
using RaidArtifactsFilter.Extensions;
using RaidFilterUI.Forms;

namespace RaidFilterUI.Helpers
{
    public static class Extensions
    {
        public static ArtifactControlModel ToArtifactControlModel(this Artifact artifact, bool isActive)
        {
            return new ArtifactControlModel()
            {
                Id = artifact.Id,
                Level = artifact.Level,
                IsActivate = isActive,
                Rank = artifact.GetRankNumber(),
                Rarity = artifact.GetRarityColor(),
                ImgName = $"{artifact.GetSetKind()}_{(artifact.Kind != ArtifactKind.Cloak ? artifact.Kind : "Amulett")}.png",
                Stats = artifact.SecondaryBonuses
                    .ToArray()
                    .Reverse().Append(artifact.PrimaryBonus)
                    .Reverse()
                    .Select(stat => new StatControlModel(
                        LocalizationHelper.GetShortStat(stat.Kind.ToString()),
                        stat.Value,
                        stat.IsAbsolute,
                        stat.Level,
                        stat.Enhancement))
                    .ToArray()
            };
        }
    }
}
