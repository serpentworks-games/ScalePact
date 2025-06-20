using Godot;
using Scalepact.InteractionSystem.ScenarioHandlers;

namespace Scalepact.InteractionSystem.Actions
{
    public partial class UpdateObjectiveAction : InteractionAction
    {
        [Export] LevelScenarioController levelScenarioController;
        [Export] string objectiveName;

        public override void PerformInteraction()
        {
            levelScenarioController.CompleteObjective(objectiveName);
        }
    }
}