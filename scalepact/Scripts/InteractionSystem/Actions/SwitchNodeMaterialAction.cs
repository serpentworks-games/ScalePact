using Godot;

namespace Scalepact.InteractionSystem.Actions
{
    public partial class SwitchNodeMaterialAction : InteractionAction
    {
        [Export] MeshInstance3D[] meshes;
        [Export] Color[] colors;

        int count;

        string albedoColorPath = "albedo_color";

        public override void PerformInteraction()
        {
            count++;
            foreach (var instance in meshes)
            {
                instance.Mesh.SurfaceGetMaterial(0).Set(albedoColorPath, colors[count % colors.Length]);
            }
        }
    }
}