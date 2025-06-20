using Godot;

namespace Scalepact.InteractionSystem
{
    public partial class GameplayCounter : InteractionAction
    {
        [Export] public int CurrentCount { get; private set; } = 0;
        [Export] public int TargetCount { get; private set; } = 3;

        [ExportCategory("Send On Counter Increment")]
        [Export] SendInteractionCommand sendOnCounterIncrement;
        [Export] InteractionAction[] actionsPerformedOnCounterIncrement;
        [ExportCategory("Send On Counter Target Reached")]
        [Export] SendInteractionCommand sendOnCounterTargetReached;
        [Export] InteractionAction[] actionsPerformedOnCounterTargetReached;

        public override void PerformInteraction()
        {
            CurrentCount += 1;
            if (CurrentCount >= TargetCount)
            {
                if (actionsPerformedOnCounterTargetReached != null)
                {
                    foreach (var item in actionsPerformedOnCounterTargetReached)
                    {
                        item.PerformInteraction();
                    }
                }

                sendOnCounterTargetReached?.SendInteraction();

                isTriggered = true;
            }
            else
            {
                if (actionsPerformedOnCounterIncrement != null)
                {
                    foreach (var item in actionsPerformedOnCounterIncrement)
                    {
                        item.PerformInteraction();
                    }
                }

                sendOnCounterIncrement?.SendInteraction();
                isTriggered = false;
            }
        }
    }
}