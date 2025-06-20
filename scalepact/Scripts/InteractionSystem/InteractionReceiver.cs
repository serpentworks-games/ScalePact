using System.Collections.Generic;
using Godot;

namespace Scalepact.InteractionSystem
{
    public partial class InteractionReceiver : Node
    {
        Dictionary<InteractionActionType, List<System.Action>> actions = [];

        public void Receive(InteractionActionType type)
        {
            if (actions.TryGetValue(type, out List<System.Action> callbacks))
            {
                foreach (var callback in callbacks)
                {
                    callback();
                }
            }
        }

        public void Register(InteractionActionType type, InteractionAction action)
        {
            if (!actions.TryGetValue(type, out List<System.Action> callbacks))
            {
                callbacks = actions[type] = new List<System.Action>();
            }
            callbacks.Add(action.OnInteract);
            //GD.Print("Added " + action + " of type " + type + " to dictionary");
        }

        public void Remove(InteractionActionType type, InteractionAction action)
        {
            actions[type].Remove(action.OnInteract);
        }
    }
}