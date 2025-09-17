using UnityEngine;
using SM = GameManagerStatic.Main.StringManager;
using CM = GameManagerStatic.Main.ColorManager;

public class T_CoinPlate : T_PressurePlate
{
    private AudioSource _audioSource;
    [SerializeField] private Transform _coin;
    private Light _light;
    private bool _hasCoin = true;
    private Vector3 _rotation = new Vector3(0f, 15f, 0f);

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
        _light = GetComponentInChildren<Light>();
        _light.color = CM.GetLightSceneColor();
        CoinManager.Instance.AddCoinInList(this);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (_hasCoin && other.tag.Equals(SM.TagPlayer()))
        {
            CoinManager.Instance.CoinPickUp();
            _coin.gameObject.SetActive(false);
            _light.gameObject.SetActive(false);
            _audioSource.Play();
            onEnter?.Invoke();
            EnterRenderer();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (_hasCoin && other.tag.Equals(SM.TagPlayer()))
        {
            _hasCoin = false;
            onExit?.Invoke();
            ExitRenderer();
        }
    }

    void Update()
    {
        if (_hasCoin)
        {
            _coin.Rotate(_rotation * Time.deltaTime, Space.Self);
        }
    }
}
