using Godot;

namespace Scalepact.InteractionSystem.Actions
{
    public partial class ToggleNodeAction : InteractionAction
    {
        [Export] Node3D[] nodesToToggle;

        public override void PerformInteraction()
        {
            foreach (var node in nodesToToggle)
            {
                node.Visible = !node.Visible;

                switch (node.ProcessMode)
                {
                    case ProcessModeEnum.Inherit:
                        node.ProcessMode = ProcessModeEnum.Disabled;
                        break;
                    case ProcessModeEnum.Disabled:
                        node.ProcessMode = ProcessModeEnum.Inherit;
                        break;
                    case ProcessModeEnum.Pausable:
                    case ProcessModeEnum.WhenPaused:
                    case ProcessModeEnum.Always:
                        break;


                }
            }
        }
    }
}