using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raid.DataModel;

namespace RaidArtifactsFilter
{
    public static class ArgumentTranslator
    {
        public static char Separator = '|';

        public static string[] ResolveStat(this string stat)
        {
            if (!Configuration.Instance.ArgumentStatTransfer.ContainsKey(stat))
            {
                throw new Exception($"The configuration for the stat \"{stat}\" isn't found.");
            }

            return Configuration.Instance.ArgumentStatTransfer[stat].Split(Separator);
        }

        public static string[] ResolveArtifactType(this string type)
        {
            if (!Configuration.Instance.ArgumentArtifactTypeTransfer.ContainsKey(type))
            {
                throw new Exception($"The configuration for the artifact type \"{type}\" isn't found.");
            }

            return Configuration.Instance.ArgumentArtifactTypeTransfer[type].Split(Separator);
        }

        public static int ResolveRarityNumber(this Func<Artifact, string> argAction, Artifact item)
        {
            var rarity = argAction.ResolveArgument(item);
            return Configuration.Instance.ArtifactRarityTransfer.ContainsKey(rarity) ? Configuration.Instance.ArtifactRarityTransfer[rarity] : 0;
        }

        public static string[] ResolveSet(this Func<Artifact, string> argAction, Artifact item)
        {
            var set = argAction.ResolveArgument(item);
            return set.ResolveSet();
        }

        public static string[] ResolveSet(this string set)
        {
            return Configuration.Instance.ArgumentArtifactSetKindTransfer.ContainsKey(set) ? Configuration.Instance.ArgumentArtifactSetKindTransfer[set].Split(Separator) : new []{ set };
        }

        public static int ResolveNumber(this Func<Artifact, string> argAction, Artifact item)
        {
            var arg = argAction.ResolveArgument(item);
            if (!int.TryParse(arg, out var num))
            {
                throw new Exception($"The argument \"{arg}\" isn't a number.");
            }

            return num;
        }

        public static string ResolveArgument(this Func<Artifact, string> argAction, Artifact item)
        {
            return argAction(item);
        }
    }
}
