using UnityEngine;
using UnityEngine.UI;
using CM = GameManagerStatic.Main.ColorManager;

public class UIMaterial : MonoBehaviour
{
    private Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
        _image.material.color = CM.GetSceneColor();
    }
}
