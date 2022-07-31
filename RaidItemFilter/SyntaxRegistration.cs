using System.Linq;
using FileCompiler.PublicAPI;
using Raid.DataModel;
using RaidArtifactsFilter.Extensions;

namespace RaidArtifactsFilter
{
    public class SyntaxRegistration
    {
        public static void Register()
        {
            var registry = new TokenRegistry<Artifact>();

            registry.RegisterFunctionAction(
                "Type", (item, list) => 
                    list.Any(argAction => item.IsArtifactTypeEquals(argAction.ResolveArgument(item))));

            registry.RegisterFunctionAction(
                "Set", (item, list) => 
                    list.Any(argAction => item.IsArtifactSetEquals(argAction.ResolveArgument(item))));

            registry.RegisterFunctionAction(
                "Rank", (item, list) => 
                    list.Any(argAction => item.GetRankNumber() == argAction.ResolveNumber(item)));

            registry.RegisterFunctionAction(
                "RankMore", (item, argAction) => 
                    item.GetRankNumber() >= argAction.ResolveNumber(item));

            registry.RegisterFunctionAction(
                "RankLess", (item, argAction) =>
                    item.GetRankNumber() < argAction.ResolveNumber(item));

            registry.RegisterFunctionAction(
                "Rarity", (item, list) => 
                    list.Any(argAction => item.RarityId == argAction(item)));

            registry.RegisterFunctionAction(
                "RarityMore", (item, argAction) =>
                    item.GetRarityNumber() >= argAction.ResolveRarityNumber(item));

            registry.RegisterFunctionAction(
                "RarityLess", (item, argAction) =>
                    item.GetRarityNumber() < argAction.ResolveRarityNumber(item));

            registry.RegisterFunctionAction(
                "Level", (item, list) => 
                    list.Any(argAction => item.Level == argAction.ResolveNumber(item)));

            registry.RegisterFunctionAction(
                "LevelMore", (item, argAction) =>
                    item.Level >= argAction.ResolveNumber(item));

            registry.RegisterFunctionAction(
                "LevelLess", (item, argAction) =>
                    item.Level < argAction.ResolveNumber(item));

            registry.RegisterFunctionAction(
                "MainStat", (item, list) => 
                    list.Any(argAction => item.PrimaryBonus.IsStatEquals(argAction.ResolveArgument(item))));

            registry.RegisterFunctionAction(
                "SubStat", (item, list) => 
                    list.All(argAction => item.ContainsSubStat(argAction.ResolveArgument(item))));

            registry.RegisterFunctionAction(
                "SubStatAny", (item, list) => 
                    list.Any(argAction => item.ContainsSubStat(argAction.ResolveArgument(item))));

            registry.RegisterFunctionAction(
                "Proc", (item, list) =>
                    list
                        .Take(list.Count - 1)
                        .Aggregate(0, (prev, argAction) =>
                            prev + item.GetProcCount(argAction.ResolveArgument(item)))
                    == list.Last().ResolveNumber(item));

            registry.RegisterFunctionAction(
                "ProcMore", (item, list) =>
                    list
                        .Take(list.Count - 1)
                        .Aggregate(0, (prev, argAction) =>
                            prev + item.GetProcCount(argAction.ResolveArgument(item)))
                    >= list.Last().ResolveNumber(item));

            registry.RegisterFunctionAction(
                "ProcLess", (item, list) =>
                    list
                        .Take(list.Count - 1)
                        .Aggregate(0, (prev, argAction) => 
                            prev + item.GetProcCount(argAction.ResolveArgument(item))) 
                    < list.Last().ResolveNumber(item));

            registry.RegisterPropertyAction("MaxProc", artifact => 
                artifact.Level < 4 ? 0 : 
                artifact.Level < 8 ? 1 : 
                artifact.Level < 12 ? 2 : 
                artifact.Level < 16 ? 3 : 4);
        }
    }
}
