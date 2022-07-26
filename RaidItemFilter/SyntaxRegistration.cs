using System.Linq;
using FileCompiler.PublicAPI;
using Raid.DataModel;
using RaidArtifactsFilter.Extensions;

namespace RaidArtifactsFilter
{
    public class SyntaxRegistration
    {
        public void Register()
        {
            var t = new Raid.DataModel.StaticData();

            var registry = new TokenRegistry<Artifact>();
            registry.RegisterFunctionAction(
                "Rank", (item, list) => 
                    list.Any(argAction => item.Rank == argAction(item)));

            registry.RegisterFunctionAction(
                "RankMore", (item, argAction) => 
                    item.Rank == argAction(item));

            registry.RegisterFunctionAction(
                "RankLess", (item, argAction) =>
                    item.Rank == argAction(item));

            registry.RegisterFunctionAction(
                "Rarity", (item, list) => 
                    list.Any(argAction => item.RarityId == argAction(item)));

            registry.RegisterFunctionAction(
                "RarityMore", (item, argAction) =>
                    item.RarityId == argAction(item));

            registry.RegisterFunctionAction(
                "RarityLess", (item, argAction) =>
                    item.RarityId == argAction(item));

            registry.RegisterFunctionAction(
                "Level", (item, list) => 
                    list.Any(argAction => item.Level == argAction(item).ToNumber()));

            registry.RegisterFunctionAction(
                "LevelMore", (item, argAction) =>
                    item.Level >= argAction(item).ToNumber());

            registry.RegisterFunctionAction(
                "LevelLess", (item, argAction) =>
                    item.Level < argAction(item).ToNumber());

            registry.RegisterFunctionAction(
                "MainStat", (item, list) => 
                    list.Any(argAction => item.PrimaryBonus.IsStatEquals(argAction(item))));

            registry.RegisterFunctionAction(
                "SubStat", (item, list) => 
                    list.All(argAction => item.ContainsSubStat(argAction(item))));

            registry.RegisterFunctionAction(
                "SubStatAny", (item, list) => 
                    list.Any(argAction => item.ContainsSubStat(argAction(item))));

            registry.RegisterFunctionAction(
                "Proc", (item, list) =>
                    list.Take(list.Count - 1).Aggregate(0, (prev, argAction) => prev + item.GetProcCount(argAction(item))) == list.Last()(item).ToNumber());

            registry.RegisterFunctionAction(
                "ProcMore", (item, list) =>
                    list.Take(list.Count - 1).Aggregate(0, (prev, argAction) => prev + item.GetProcCount(argAction(item))) >= list.Last()(item).ToNumber());

            registry.RegisterFunctionAction(
                "ProcLess", (item, list) =>
                    list.Take(list.Count - 1).Aggregate(0, (prev, argAction) => prev + item.GetProcCount(argAction(item))) < list.Last()(item).ToNumber());
        }


    }
}
