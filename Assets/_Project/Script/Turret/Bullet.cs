using System.Collections;
using UnityEngine;

//Visto che le pool si fanno per risparmiare risorse, vedo se riesco a renderlo ancora più efficiente ...
[RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour
{
    private bool _isActive = true;
    private IEnumerator _shoot;
    private WaitUntil _waitIsShot;
    private bool _isShot;
    private AudioSource _audioSource;
    private SphereCollider _sphereCollider;
    private LayerMask _layerMask = (1 << 0);

    private float _lifeTime = 2.5f;
    private float _currentLifeTime;
    private float _speed;
    private int _damage;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _sphereCollider = GetComponent<SphereCollider>();

        _shoot = Shoot();
        _waitIsShot = new WaitUntil(() => _isShot);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        StartCoroutine(_shoot);
    }

    void OnDisable()
    {
        StopCoroutine(_shoot);
    }

    public void SetUp(int damage, float speed, float lifeTime, Transform firePoint)
    {
        gameObject.SetActive(true);
        _damage = damage;
        _speed = speed;
        _lifeTime = lifeTime;
        transform.position = firePoint.position;
        transform.forward = firePoint.forward;
        _currentLifeTime = _lifeTime;
        _audioSource.Play();
        //Debug.Log("Shot " + Time.time.ToString("F2"), gameObject);
        _isShot = true;
    }

    IEnumerator Shoot()
    {
        while (_isActive)
        {
            yield return _waitIsShot;
            //Debug.Log("Start", gameObject);
            yield return null;   //IMPORTANTE NON TOGLIERE
            while (_currentLifeTime > 0f)
            {
                _currentLifeTime -= Time.deltaTime;
                transform.position += transform.forward * _speed * Time.deltaTime;
                //Rigidbody fa lo stesso controllo, ma è più pesante
                if (Physics.CheckSphere(transform.position, _sphereCollider.radius, _layerMask, QueryTriggerInteraction.Ignore))
                {
                    _currentLifeTime = 0f;
                }
                yield return null;
            }

            _isShot = false;
            gameObject.SetActive(false);
            //Debug.Log("Pause", gameObject);
            PoolManager.instance.ReturnToPool(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        LifeController life = other.GetComponentInParent<LifeController>();
        if (life != null)
        {
            life.TakeDamage(_damage);
        }
        _currentLifeTime = 0f;
    }

    public void SetIsActive(bool value) => _isActive = value;
}
