using UnityEngine;
using UnityEngine.Events;

public class PlayerTimer : MonoBehaviour
{
    [SerializeField] private float _timeMax = 300f;
    private float _currentTime;
    [Range(0, 10)] [SerializeField] private float _rechargeTimeOnCoinPickUp;

    public UnityEvent<float, float> onChangeTime;
    public UnityEvent onTimeFinish;

    void Awake()
    {
        if (_timeMax <= 0)
        {
            _timeMax = 300f;
        }

        if (_rechargeTimeOnCoinPickUp != 0)
        {
            CoinManager.Instance.onCoinPicked.AddListener((int void0, int void1) => RechargeTime(_rechargeTimeOnCoinPickUp));
        }
    }

    void Start()
    {
        onChangeTime?.Invoke(_timeMax - _currentTime, _timeMax);
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _timeMax)
        {
            onTimeFinish?.Invoke();
        }
        else
        {
            onChangeTime?.Invoke(_timeMax - _currentTime, _timeMax);
        }
    }

    public void RechargeTime(float time)
    {
        _currentTime = Mathf.Max(0, _currentTime - time);
        onChangeTime?.Invoke(_timeMax - _currentTime, _timeMax);
    }
}
