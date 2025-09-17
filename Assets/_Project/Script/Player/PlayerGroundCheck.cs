using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerGroundCheck : MonoBehaviour
{
    private CapsuleCollider _capsuleCollider;

    private Vector3 _deltaSpherePosition;
    private float _radiusSphere;
    private LayerMask _groundLayer = (1 << 0);

    private float _coyoteTimeStart;
    [SerializeField] private float _coyoteDuration = 1f;
    private bool _isCoyoteTime;
    public bool IsCoyoteTime
    {
        get => _isCoyoteTime;
        private set => _isCoyoteTime = value;
    }

    private bool _isGrounded;
    public bool IsGrounded
    {
        get => _isGrounded;
        private set => _isGrounded = value;
    }
    public UnityEvent<bool> onGround;

    private bool _isJump;
    private byte _jumpFrame;

    void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        float deep = _capsuleCollider.radius / 5f;
        _radiusSphere = _capsuleCollider.radius - (deep);
        _deltaSpherePosition = -Physics.gravity.normalized * (_radiusSphere - deep / 2f);
    }

    void Start()
    {
        onGround?.Invoke(IsGrounded);
    }

    public void IsJumpTrue() => _isJump = true;

    private bool IsCheckSphere()
    {
        return Physics.CheckSphere(transform.position + _deltaSpherePosition, _radiusSphere, _groundLayer, QueryTriggerInteraction.Ignore);
    }

    void Update()
    {
        if (_isJump)
        {
            _isJump = false;
            _jumpFrame = 5;
            
            _coyoteTimeStart = 0;
            IsCoyoteTime = false;
        }
        //Questo else if e il successivo prevengono che il frame dopo aver saltato imposti _isGrounded = true e un problema con l'animazione
        else if (_jumpFrame == 1)
        {
            IsGrounded = false;
            onGround?.Invoke(IsGrounded);
            _jumpFrame = 0;
        }
        else if (_jumpFrame != 0)
        {
            _jumpFrame--;
        }
        else
        {
            if (_coyoteTimeStart != 0)
            {
                if (Time.time - _coyoteTimeStart > _coyoteDuration)
                {
                    _coyoteTimeStart = 0;
                    IsCoyoteTime = false;
                    if (!IsCheckSphere())
                    {
                        IsGrounded = false;
                        onGround?.Invoke(IsGrounded);
                    }
                }
            }
            else
            {
                if (IsCheckSphere())
                {
                    if (IsGrounded == false)
                    {
                        IsGrounded = true;
                        onGround?.Invoke(IsGrounded);
                    }
                }
                else if (IsGrounded)
                {
                    _coyoteTimeStart = Time.time;
                    IsCoyoteTime = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + _deltaSpherePosition, _radiusSphere);
    }
}
