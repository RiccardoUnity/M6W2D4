using UnityEngine;
using SM = GameManagerStatic.Main.StringManager;

public class AudioManager : SingletonGeneric<AudioManager>
{
    [SerializeField] private AudioClip _music;
    [SerializeField] private AudioClip _end;
    [SerializeField] private AudioClip _hitPlayer;

    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioSource _audioSourceVFX;

    protected override bool ShouldBeDestroyOnLoad() => false;

    private int _lastHp = 0;

    static AudioManager()
    {
        _useResources = true;
        _resourcesPath = SM.PathAudioManager();
    }

    void Start()
    {
        Music();
    }

    public void Music()
    {
        _audioSourceMusic.clip = _music;
        _audioSourceMusic.loop = true;
        _audioSourceMusic.Play();
    }

    public void End()
    {
        _audioSourceMusic.clip = _end;
        _audioSourceMusic.loop = false;
        _audioSourceMusic.Play();
    }

    public void SetLifeController(LifeController lifeController)
    {
        _audioSourceVFX.clip = _hitPlayer;
        _audioSourceVFX.loop = false;
        lifeController.onChangeHp.AddListener(PlayerHit);
        lifeController.onDeath.AddListener(PlayerDeath);
    }

    public void PlayerHit(int hp, int maxHp)
    {
        if (hp < _lastHp)
        {
            _audioSourceVFX.Play();
        }
        _lastHp = hp;
    }

    public void PlayerDeath()
    {
        _audioSourceVFX.Play();
    }
}
