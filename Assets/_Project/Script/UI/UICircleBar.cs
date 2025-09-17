using UnityEngine;
using UnityEngine.UI;
using CM = GameManagerStatic.Main.ColorManager;

public class UICircleBar : MonoBehaviour
{
    //[SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;

    void Start()
    {
        _fill.color = CM.GetSceneColor();
    }

    public void SetFill(int amount, int amountMax) => Fill((float)amount / amountMax);

    public void SetFill(float amount, float amountMax) => Fill(amount / amountMax);

    private void Fill(float normal)
    {
        normal = Mathf.Clamp01(normal);
        _fill.fillAmount = normal;
        //_fill.color = _gradient.Evaluate(normal);
    }

    public void SetFillMin() => Fill(0f);
}
