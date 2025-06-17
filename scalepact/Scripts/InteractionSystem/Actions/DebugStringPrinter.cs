using Godot;

namespace Scalepact.InteractionSystem.Actions
{
    /// <summary>
    /// This is just a debug helper action that allows text to be printed
    /// via the interaction system. 
    /// Handy to make sure that the InteractionArea works
    /// </summary>
    public partial class DebugStringPrinter : InteractionAction
    {
        [Export] string textToPrint = "";

        public override void PerformInteraction()
        {
            GD.Print(textToPrint);
        }
    }
}