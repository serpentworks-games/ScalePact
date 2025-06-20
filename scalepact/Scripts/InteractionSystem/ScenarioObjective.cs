using Godot;
using System;

namespace Scalepact.InteractionSystem
{
    public partial class ScenarioObjective : Node3D
    {
        [Export] public string ObjectiveName { get; private set; }
        [Export] public int ObjectiveRequiredCount { get; private set; }
        [Export] public SendInteractionCommand SendOnObjectiveProgressed { get; private set; }
        [Export] public SendInteractionCommand SendOnObjectiveCompleted { get; private set; }

        public int ObjectiveCurrentCount { get; private set; }
        public bool ObjectiveCompleted { get; private set; }
        public bool ObjectiveEventFired { get; private set; }

        public void CompleteObjective()
        {
            ObjectiveCurrentCount += 1;

            if (ObjectiveCurrentCount >= ObjectiveRequiredCount)
            {
                ObjectiveCompleted = true;
                if (!ObjectiveEventFired) SendOnObjectiveCompleted.SendInteraction();
                ObjectiveEventFired = true;
            }
            else
            {
                SendOnObjectiveProgressed.SendInteraction();
            }
        }


    }
}