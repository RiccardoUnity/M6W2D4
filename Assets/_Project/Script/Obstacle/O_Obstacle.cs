using UnityEngine;
using SM = GameManagerStatic.Main.StringManager;
using CM = GameManagerStatic.Main.ColorManager;

public abstract class O_Obstacle : MonoBehaviour
{
    [SerializeField] protected bool _isActive = true;   //Serve NON toglierla di nuovo! ( x il me del futuro XD )
    [SerializeField] protected bool _activeOnAllCoinPicked;
    [SerializeField] protected bool _isOnce;
    [SerializeField] protected bool _isInverse;
    [SerializeField] private bool _overrideTrigger;
    [SerializeField] protected float _delay;
    protected float _currentDelay;
    [SerializeField] protected float _speed;
    [SerializeField] private MeshRenderer _meshRenderer;

    protected virtual void Awake()
    {
        _currentDelay = _delay;
        if (_overrideTrigger)
        {
            SetMaterial(true);
        }
        ResetData();
    }

    void Start()
    {
        if (_activeOnAllCoinPicked)
        {
            CoinManager.Instance.onAllCoinPicked.AddListener(() => SetIsActive(true));
        }
    }

    public abstract void ResetData();

    void Update()
    {
        if (_isActive && _overrideTrigger)
        {
            if (_currentDelay > 0f)
            {
                _currentDelay -= Time.deltaTime;
            }
            else
            {
                TransformChange();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive && other.tag.Equals(SM.TagPlayer()))
        {
            _currentDelay = _delay;
            SetMaterial(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isActive && !_overrideTrigger && other.tag.Equals(SM.TagPlayer()))
        {
            if (_currentDelay > 0f)
            {
                _currentDelay -= Time.deltaTime;
            }
            else
            {
                TransformChange();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isActive && other.tag.Equals(SM.TagPlayer()))
        {
            SetMaterial(false);
        }
    }

    protected void SetMaterial(bool lightColor)
    {
        if (_meshRenderer != null && _meshRenderer.materials.Length > 1)
        {
            if (lightColor)
            {
                _meshRenderer.materials[1].color = CM.GetLightSceneColor();
            }
            else
            {
                _meshRenderer.materials[1].color = CM.GetSceneColor();
            }
        }
    }

    protected abstract void TransformChange();

    public void SetIsActive(bool value)
    {
        _isActive = value;
        SetMaterial(value);
    }

    public void SetIsOnce(bool value) => _isOnce = value;

    public void SetIsInverse(bool value) => _isInverse = value;

    public void ResetCurrentDelay() => _currentDelay = _delay;
}
