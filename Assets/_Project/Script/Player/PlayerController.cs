using UnityEngine;
using UnityEngine.Events;
using SM = GameManagerStatic.Main.StringManager;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerGroundCheck))]
[RequireComponent(typeof(LifeController))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerGroundCheck _playerGroundCheck;
    private LifeController _lifeController;
    private Transform _camera;

    private O_SlicePosition _slicePosition;
    private Vector3 _lastPositionSlice;

    private float _horizontal;
    private float _vertical;
    private bool _isVerticalNegative;
    public bool IsVerticalNegative
    {
        get => _isVerticalNegative;
        private set => _isVerticalNegative = value;
    }

    private float _lenght;
    public float Lenght
    {
        get => _lenght;
        private set => _lenght = value;
    }

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _sprintSpeed = 5f;
    [SerializeField] private float _silentSpeed = 1f;
    private float _gravityMagFixedInverse;
    [SerializeField] private float _jumpForce = 6f;
    [SerializeField] private float _rotationSpeed = 5f;

    private bool _isSprint;
    private bool _canSprint;
    private bool _isSilent;
    private bool _canJump;

    //Eventi
    public UnityEvent<bool> onSprintMode;
    public UnityEvent<bool> onSilentMode;
    public UnityEvent onCanJump;

    void Awake()
    {
        _playerGroundCheck = GetComponent<PlayerGroundCheck>();
        _lifeController = GetComponent<LifeController>();
        _rb = GetComponent<Rigidbody>();

        _camera = Camera.main.transform;

        _gravityMagFixedInverse = Physics.gravity.magnitude * Time.fixedDeltaTime;
    }

    void Start()
    {
        onSilentMode.Invoke(_isSilent);
    }

    private void SprintActive()
    {
        _isSprint = true;
        onSprintMode?.Invoke(_isSprint);
        if (_isSilent)
        {
            _isSilent = false;
            onSilentMode?.Invoke(_isSilent);
        }
    }

    //Gestire gli Input qui
    void Update()
    {
        _horizontal = Input.GetAxis(SM.InputHorizontal());
        _vertical = Input.GetAxis(SM.InputVertical());
        if (_vertical < 0 - Mathf.Epsilon)
        {
            IsVerticalNegative = true;
        }
        else
        {
            IsVerticalNegative = false;
        }

        //Se sprinto non posso essere silenzioso e se sono silenzioso non sprinto
        //Modi per entrare in SprintMode
        if (Input.GetButtonDown(SM.InputSprint()))
        {
            if (_vertical > 0f + Mathf.Epsilon)
            {
                SprintActive();
            }
            else
            {
                _canSprint = true;
            }
        }
        else if (_vertical > 0f + Mathf.Epsilon && _canSprint)
        {
            _canSprint = false;
            SprintActive();
        }
        //Modo per uscire e rientrare in SprintMode all'occorenza
        else if (_vertical < 0f - Mathf.Epsilon && _isSprint)
        {
            _isSprint = false;
            onSprintMode?.Invoke(_isSprint);
            _canSprint = true;
        }
        //Modi per uscire dalla SprintMode
        else if (Input.GetButtonUp(SM.InputSprint()))
        {
            if (_canSprint)
            {
                _canSprint = false;
            }
            else if (_isSprint)
            {
                _isSprint = false;
                onSprintMode?.Invoke(_isSprint);
            }
        }
        else if (Input.GetButtonDown(SM.InputSilentMode()))
        {
            _isSilent = !_isSilent;
            onSilentMode?.Invoke(_isSilent);
            if (_isSprint)
            {
                _isSprint = false;
                onSprintMode?.Invoke(_isSprint);
            }
        }

        if (Input.GetButtonDown(SM.InputJump()) && _playerGroundCheck.IsGrounded)
        {
            _canJump = true;
            _playerGroundCheck.IsJumpTrue();
            onCanJump?.Invoke();
        }

        //Rotazione solo in movimento
        if (Lenght > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, CamYaw(), _rotationSpeed * Time.deltaTime);
        }
    }

    private Quaternion CamYaw() => Quaternion.Euler(0f, _camera.localEulerAngles.y, 0f);

    private void ChangeVelocityY(float velocityY)
    {
        Vector3 velocity = _rb.velocity;
        velocity.y = velocityY;
        _rb.velocity = velocity;
    }

    void FixedUpdate()
    {
        //Movimento verticale
        if (_playerGroundCheck.IsCoyoteTime)
        {
            ChangeVelocityY(_gravityMagFixedInverse);
        }

        if (_canJump)
        {
            _canJump = false;
            ChangeVelocityY(_jumpForce);
        }

        //Movimento orizzontale
        Lenght = new Vector3(_horizontal, 0f, _vertical).sqrMagnitude;
        if (Lenght > 0f + Mathf.Epsilon || _slicePosition != null)
        {
            if (Lenght > 1f)
            {
                Lenght = Mathf.Sqrt(Lenght);
                _horizontal /= Lenght;
                _vertical /= Lenght;
            }
            Vector3 direction = new Vector3(_horizontal, 0f, _vertical);

            float speed;
            if (_isSilent)
            {
                speed = _silentSpeed * Time.fixedDeltaTime;
            }
            //Scatto solo in avanti
            else if (_isSprint)
            {
                speed = _sprintSpeed * Time.fixedDeltaTime;
            }
            else
            {
                speed = _speed * Time.fixedDeltaTime;
            }

            Vector3 deltaSlice = Vector3.zero;
            if (_slicePosition != null)
            {
                deltaSlice = _slicePosition.transform.position - _lastPositionSlice;
                _lastPositionSlice = _slicePosition.transform.position;
            }
            
            _rb.MovePosition(_rb.position + CamYaw() * direction * speed + deltaSlice);
        }
        InFallGameOver();
    }

    public bool GetIsSilent() => _isSilent;

    private void InFallGameOver()
    {
        if (_rb.velocity.y < -30f)
        {
            _lifeController.TakeDamage(100);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _slicePosition = collision.collider.GetComponent<O_SlicePosition>();
        if (_slicePosition != null)
        {
            _lastPositionSlice = _slicePosition.transform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _slicePosition = null;
    }
}
