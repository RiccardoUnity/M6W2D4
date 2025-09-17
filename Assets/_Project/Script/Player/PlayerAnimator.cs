using UnityEngine;
using SM = GameManagerStatic.Main.StringManager;

public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerController _playerController;
    private PlayerGroundCheck _playerGroundCheck;
    private Animator _animator;

    private bool _canSprint;
    private bool _canJump;
    private bool _checkVerticalSpeed;
    private float _gravityMagnitude;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _playerController = GetComponent<PlayerController>();
        _playerController.onSprintMode.AddListener(SetCanSprint);
        _playerController.onSilentMode.AddListener(SetIsSilent);
        _playerController.onCanJump.AddListener(SetCanJump);

        _playerGroundCheck = GetComponent<PlayerGroundCheck>();
        _playerGroundCheck.onGround.AddListener(SetIsGrounded);

        _animator = GetComponentInChildren<Animator>();

        _gravityMagnitude = Physics.gravity.magnitude;
    }

    void Update()
    {
        //Movimento verticale
        if (_canJump)
        {
            _canJump = false;
            _animator.SetTrigger(SM.ParAnimatorJump());
        }

        if (_checkVerticalSpeed)
        {
            _animator.SetFloat(SM.ParAnimatorVSpeed(), _rb.velocity.y / _gravityMagnitude);
        }

        //Movimento orizontale
        if (_playerController.IsVerticalNegative)
        {
            _animator.SetFloat(SM.ParAnimatorHSpeed(), -_playerController.Lenght);
        }
        else
        {
            if (_canSprint)
            {
                _animator.SetFloat(SM.ParAnimatorHSpeed(), _playerController.Lenght * 2f);
            }
            else
            {
                _animator.SetFloat(SM.ParAnimatorHSpeed(), _playerController.Lenght);
            }
        }
    }

    public void SetCanSprint(bool value) => _canSprint = value;

    public void SetIsSilent(bool value) => _animator.SetBool(SM.ParAnimatorIsSilent(), value);

    public void SetCanJump() => _canJump = true;

    public void SetIsGrounded(bool value)
    {
        _animator.SetBool(SM.ParAnimatorIsGrounded(), value);
        _checkVerticalSpeed = !value;
    }
}
