using UnityEngine;
using CM = GameManagerStatic.Main.ColorManager;
using SM = GameManagerStatic.Main.StringManager;

public class Turret : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private int _damageBullet = 1;
    [SerializeField] private float _speedBullet = 5;
    [SerializeField] private float _lifeTimeBullet = 5f;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 0.5f;
    private float _lastFireTime;

    private MeshRenderer _meshRenderer;
    PlayerController _playerController;
    private bool _isPlayerSilent;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        _lastFireTime = Time.time - _fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive && other.tag.Equals(SM.TagPlayer()))
        {
            SetMaterial(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals(SM.TagPlayer()))
        {
            if (_playerController == null)
            {
                _playerController = other.GetComponent<PlayerController>();
                _isPlayerSilent = _playerController.GetIsSilent();
                _playerController.onSilentMode.AddListener(SetIsPlayerSilent);
            }

            if (_isActive && _lastFireTime + _fireRate < Time.time && !_isPlayerSilent)
            {
                _lastFireTime = Time.time;
                //Meglio dividerlo? ...
                PoolManager.instance.GetBullet().SetUp(_damageBullet, _speedBullet, _lifeTimeBullet, _firePoint);
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

    public void SetIsActive(bool value)
    {
        _isActive = value;
        SetMaterial(value);
    }

    private void SetMaterial(bool lightColor)
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

    public void SetIsPlayerSilent(bool value) => _isPlayerSilent = value;
}
