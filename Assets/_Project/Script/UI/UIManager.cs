using UnityEngine;
using UnityEngine.UI;
using CM = GameManagerStatic.Main.ColorManager;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _victory;
    [SerializeField] private GameObject _option;

    [SerializeField] private Button[] _buttons;

    void Awake()
    {
        if (_gameOver != null)
        {
            _gameOver.SetActive(false);
        }
        if (_victory != null)
        {
            _victory.SetActive(false);
        }
        if (_option != null)
        {
            _option.SetActive(false);
        }
    }

    public void ChangeColorButtons()
    {
        ColorBlock colorBlock = _buttons[0].colors;
        colorBlock.normalColor = CM.GetSceneColor();
        colorBlock.highlightedColor = CM.GetLightSceneColor();
        colorBlock.selectedColor = CM.GetSceneColor();

        foreach (Button button in _buttons)
        {
            button.colors = colorBlock;
        }
    }

    public void GameOver()
    {
        _option.SetActive(false);
        _gameOver.SetActive(true);
    }

    public void Victory()
    {
        _option.SetActive(false);
        _victory.SetActive(true);
    }

    public void Option(bool value)
    {
        _option.SetActive(value);
        Time.timeScale = (value ? 0 : 1);
    }
}
