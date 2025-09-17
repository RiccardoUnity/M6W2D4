using UnityEngine;
using CM = GameManagerStatic.Main.ColorManager;

public class T_PressurePlate : T_Trigger
{
    private MeshRenderer _meshRenderer;

    protected override void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void EnterRenderer()
    {
        _meshRenderer.materials[1].color = CM.GetLightSceneColor();
    }

    protected override void ExitRenderer()
    {
        _meshRenderer.materials[1].color = CM.GetSceneColor();
    }
}
