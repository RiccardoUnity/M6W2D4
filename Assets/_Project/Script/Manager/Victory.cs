using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using SM = GameManagerStatic.Main.StringManager;

public class Victory : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerTimer _playerTimer;
    [SerializeField] private CinemachineFreeLook _camera;
    [SerializeField] private bool _useEndMusic;
    private bool _isActive;
    private float _currentTime;

    public UnityEvent onWin;

    //Usato in Unity Event, non cancellare di nuovo
    public void SetIsActive(bool value) => _isActive = value;

    void Awake()
    {
        CoinManager.Instance.onAllCoinPicked.AddListener(() => SetIsActive(true));
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isActive && other.tag.Equals(SM.TagPlayer()))
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > 1f)
            {
                PlayerWin();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isActive && other.tag.Equals(SM.TagPlayer()))
        {
            _currentTime = 0f;
        }
    }

    private void PlayerWin()
    {
        _playerController.enabled = false;
        _playerTimer.enabled = false;
        _camera.enabled = false;
        CoinManager.Instance.OnPlayerWinLevel();
        LifeManager.Instance.OnPlayerWinLevel();
        if (_useEndMusic)
        {
            AudioManager.Instance.End();
        }
        onWin?.Invoke();
        _isActive = false;
    }
}
