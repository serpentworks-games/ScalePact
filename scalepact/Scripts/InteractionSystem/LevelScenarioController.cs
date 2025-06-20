using Godot;
using System;
using System.Linq;

namespace Scalepact.InteractionSystem
{
    public partial class LevelScenarioController : Node3D
    {
        [Export] public ScenarioObjective[] ScenarioObjectives { get; private set; }
        [Export] public SendInteractionCommand SendOnAllObjectivesCompleted { get; private set; }

        public void CompleteObjective(string name)
        {
            for (int i = 0; i < ScenarioObjectives.Length; i++)
            {
                if (ScenarioObjectives[i].ObjectiveName == name)
                {
                    ScenarioObjectives[i].CompleteObjective();
                }
            }
            for (int i = 0; i < ScenarioObjectives.Length; i++)
            {
                if (!ScenarioObjectives[i].ObjectiveCompleted) return;
            }
            SendOnAllObjectivesCompleted.SendInteraction();
        }

    }
}
