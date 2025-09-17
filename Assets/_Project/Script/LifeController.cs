using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using SM = GameManagerStatic.Main.StringManager;

public class LifeController : MonoBehaviour
{
    private int _key;

    [SerializeField] private int _hpMax;
    [SerializeField] private int _hp;
    [SerializeField] private bool _hpMaxInAwake;
    [SerializeField] private bool _iKnowThatTheLifeIsUnderZero;
    [Range(0, 5)] [SerializeField] private int _healOnCoinPickUp;

    public UnityEvent<int, int> onChangeHp;
    public UnityEvent onDeath;

    void Awake()
    {
        _key = Random.Range(int.MinValue, int.MaxValue);
        LifeManager.Instance.SetLifeController(_key, this);

        if (_hpMaxInAwake)
        {
            _hp = _hpMax;
        }

        if (_hp <= 0 && !_iKnowThatTheLifeIsUnderZero)
        {
            Debug.LogWarning($"{gameObject.name} è stato istanziato con {_hp} di vita");
        }
    }

    IEnumerator Start()
    {
        onChangeHp?.Invoke(_hp, _hpMax);

        yield return null;

        if (_healOnCoinPickUp != 0)
        {
            CoinManager.Instance.onCoinPicked.AddListener((int void0, int void1) => Heal(_healOnCoinPickUp));
        }

        if (tag.Equals(SM.TagPlayer()))
        {
            AudioManager.Instance.SetLifeController(this);
        }
    }

    public void TakeDamage(int damage)
    {
        _hp = Mathf.Max(0, _hp - Mathf.Abs(damage));
        if (_hp == 0)
        {
            AudioManager.Instance.End();
            onDeath?.Invoke();
            gameObject.SetActive(false);
        }
        else
        {
            onChangeHp?.Invoke(_hp, _hpMax);
        }
    }

    public void Heal(int heal)
    {
        _hp = Mathf.Min(_hpMax, _hp + Mathf.Abs(heal));
        onChangeHp?.Invoke(_hp, _hpMax);
    }

    //Usato solo dal LifeManager
    public void SetHp(int key, int hp)
    {
        if (key == _key)
        {
            _hp = hp;
            onChangeHp?.Invoke(_hp, _hpMax);
        }
    }

    public int GetHp() => _hp;
}
