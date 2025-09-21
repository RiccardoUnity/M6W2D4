using UnityEngine;
using UnityEngine.Events;
using SM = GameManagerStatic.Main.StringManager;
using CM = GameManagerStatic.Main.ColorManager;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelSettings : MonoBehaviour
{
    [SerializeField] private Light _mainLight;
    [SerializeField] private Material _unlit;

    [SerializeField] private bool _resetInAwake = true;
    private bool _isInOption;
    [SerializeField] private bool _isMouseLockable = true;

    public UnityEvent onChangeColor;
    public UnityEvent onOptionEnter;
    public UnityEvent onOptionExit;

    void Awake()
    {
        if (_resetInAwake)
        {
            ChangeSceneColor();
        }
        else
        {
            ChangeColor();
        }

        if (_isMouseLockable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Start()
    {
        CoinManager.Instance.OnPlayerStartLevel();
        LifeManager.Instance.OnPlayerStartLevel();
    }

    private void ChangeColor()
    {
        _mainLight.color = CM.GetLightSceneColor();
        _unlit.color = CM.GetSceneColor();
        _unlit.SetColor("_EmissionColor", CM.GetSceneColor());
        onChangeColor.Invoke();
    }

    public void ChangeSceneColor()
    {
        CM.SetSceneColor();
        CM.SetLightSceneColor();
        ChangeColor();
    }

    public void LoadMainMenu()
    {
        CoinManager.Instance.OnPlayerReturnMainMenu();
        LifeManager.Instance.OnPlayerReturnMainMenu();
        AudioManager.Instance.Music();
        SceneManager.LoadScene(0);
    }

    public void LoadNewGame()
    {
        LifeManager.Instance.SetIsFirstLevel();
        SceneManager.LoadScene(1);
    }

    public void ReloadScene()
    {
        CoinManager.Instance.OnPlayerReloadLevel();
        LifeManager.Instance.OnPlayerReloadLevel();
        AudioManager.Instance.Music();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        CoinManager.Instance.OnPlayerReloadLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        CoinManager.Instance.OnPlayerReloadLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

#if UNITY_EDITOR
    public void Exit()
    {
        EditorApplication.ExitPlaymode(); 
    }
#endif

    public void EndGame()
    {
        _isMouseLockable = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetButtonDown(SM.InputOption()) && _isMouseLockable)
        {
            _isInOption = !_isInOption;
            if (_isInOption)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                onOptionEnter.Invoke();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                onOptionExit.Invoke();
            }
        }
    }
}
