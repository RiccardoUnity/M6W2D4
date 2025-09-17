using UnityEngine;
using SM = GameManagerStatic.Main.StringManager;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Transform _camera;

    [SerializeField] private int _pitchMin = -20;
    [SerializeField] private int _pitchMax = 70;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -3f);
    [SerializeField] private int _mouseSensitive = 800;

    private bool _invertMouseY;
    private float _mouseX;
    private float _mouseY;
    private float _pitch;
    private float _yaw;
    private bool _isDirty;

    private float _timeLastInput;
    [SerializeField] private float _waitFromLastInput = 5f;
    [SerializeField] private int _yawStaticCam = 10;
    [SerializeField] private float _speedYawStatic = 0.1f;
    private bool _isYawStaticCam;
    private float _yawStart;
    private bool _isYawInverseMotion;

    private RaycastHit _hit;
    private LayerMask _defaultLayer = (1 << 0);
    private QueryTriggerInteraction _qti = QueryTriggerInteraction.Ignore;

    void Awake()
    {
        _camera = GetComponentInChildren<Camera>().transform;
        _camera.localPosition = _offset;
    }

    void Update()
    {
        _mouseX = Input.GetAxis(SM.InputMouseX()) * _mouseSensitive;
        _mouseY = Input.GetAxis(SM.InputMouseY()) * _mouseSensitive * ((_invertMouseY)? -1 : 1);

        if (_mouseX == 0 || _mouseY == 0)
        {
            if (!_isYawStaticCam)
            {
                _timeLastInput += Time.deltaTime;
            }

            if (_timeLastInput > _waitFromLastInput && !_isYawStaticCam)
            {
                _isYawStaticCam = true;
            }
        }
        else
        {
            _timeLastInput = 0f;
            _isYawStaticCam = false;
        }
    }

    void LateUpdate()
    {
        transform.position = _target.position;

        if (CheckWall())
        {
            _isDirty = true;
            _camera.localPosition = transform.InverseTransformPoint(_hit.normal * 0.5f + _hit.point);
        }
        else
        {
            if (_isDirty)
            {
                _isDirty = false;
                _camera.localPosition = _offset;
            }
        }

        //Se il mouse rimane fermo la camera si muove leggermente
        if (_isYawStaticCam)
        {
            if (_isYawInverseMotion)
            {
                _yaw += -_yawStaticCam * _speedYawStatic * Time.deltaTime;
                if (_yaw < _yawStart - _yawStaticCam)
                {
                    _isYawInverseMotion = false;
                }
            }
            else
            {
                _yaw += _yawStaticCam * _speedYawStatic * Time.deltaTime;
                if (_yaw > _yawStart + _yawStaticCam)
                {
                    _isYawInverseMotion = true;
                }
            }
        }
        else
        {
            _yaw += _mouseX * Time.deltaTime;
            _yawStart = _yaw;
        }

        _pitch += _mouseY * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, _pitchMin, _pitchMax);

        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        _camera.LookAt(_target.position);
    }

    private bool CheckWall() => Physics.Linecast(_target.position, transform.TransformPoint(_offset), out _hit, _defaultLayer, _qti);

    public void ChanceInverteMouseY() => _invertMouseY = !_invertMouseY;

    public void SetMouseSensitive(float value) => _mouseSensitive = (int)Mathf.Lerp(20f, 400f, value);   //Adatta per il WebGL, a PC tenere su 800
}
