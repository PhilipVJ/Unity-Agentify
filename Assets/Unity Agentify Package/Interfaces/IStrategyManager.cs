using System.Collections.Generic;

namespace Assets.Unity_Agentify_Package
{
    public interface IStrategyManager
    {
        bool EnableBehaviourSwitcing { get; set; }
        bool EnableMultipleStrategies { get; set; }
        object StateObject { get; set; }

        void AddAdditionalStrategy(BehaviourStrategy strategy);
        void DeleteAdditionStrategy(BehaviourStrategy strategy);
        List<BehaviourStrategy> GetAllStrategies();
        BehaviourStrategy GetDefaultStrategy();
        BehaviourStrategy GetStrategy();
    }
}