using System.Collections;
using UnityEngine;
using SM = GameManagerStatic.Main.StringManager;

public class Box : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _startPosition;
    private int _countCollision;

    private bool _isPlayerIn;
    private Transform _playerTransform;
    private float _angle = 30f;
    private float _cos;
    private Vector3 _lastPlayerPosition;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _startPosition = transform.position;

        _cos = Mathf.Cos(_angle * Mathf.Deg2Rad);
    }

    void OnCollisionEnter(Collision collision)
    {
        ++_countCollision;
    }

    void OnCollisionExit(Collision collision)
    {
        --_countCollision;
        if (_countCollision == 0 )
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        bool stay = true;
        yield return null;
        while (_rb.velocity.y < 0 && stay)
        {
            if (_rb.velocity.y < -5f)
            {
                stay = false;
                _rb.MovePosition(_startPosition);
            }
            yield return null;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals(SM.TagPlayer()))
        {
            _isPlayerIn = true;
            _playerTransform = collider.transform;
            _lastPlayerPosition = collider.transform.position;
            StartCoroutine(PlayerStay());
        }
    }

    //Controllo se il Player è di fronte al box e di quanto si sposta
    IEnumerator PlayerStay()
    {
        float cos;
        Vector3 distanceNorm;
        while (_isPlayerIn)
        {
            yield return null;
            //Dal punto di vista del Player
            distanceNorm = (transform.position - _playerTransform.position).normalized;
            cos = Vector3.Dot(_playerTransform.forward, distanceNorm);
            if (cos > _cos)
            {
                _rb.MovePosition(_rb.position + (_playerTransform.position - _lastPlayerPosition));
                _lastPlayerPosition = _playerTransform.position;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals(SM.TagPlayer()))
        {
            _isPlayerIn = false;
        }
    }
}
