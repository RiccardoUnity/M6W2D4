using UnityEngine;

public class O_HeadRotation : O_Obstacle
{
    enum Axis
    {
        Picht,
        Yaw,
        Roll
    }

    [SerializeField] private Axis _axis = Axis.Yaw;
    //Posso cambiare il pivot per la rotazione con _head
    [SerializeField] private Transform _head;
    [SerializeField] private int _angleMax = 90;
    [SerializeField] private int _angleMin = -90;
    private float _angle;
    private float _angleStart;
    [SerializeField] private bool _isContinuous;

    public override void ResetData()
    {
        switch (_axis)
        {
            case Axis.Picht:
                _angleStart = _head.localEulerAngles.x;
                break;
            case Axis.Yaw:
                _angleStart = _head.localEulerAngles.y;
                break;
            case Axis.Roll:
                _angleStart = _head.localEulerAngles.z;
                break;
        }

        _angle = _angleStart;
    }

    protected override void TransformChange()
    {
        if (_isContinuous)
        {
            if (_isInverse)
            {
                _angle -= _speed * Time.deltaTime;
                if (_angle < 360 + _angleMin)
                {
                    _angle += 360;
                }
            }
            else
            {
                _angle += _speed * Time.deltaTime;
                if (_angle > 360 + _angleMax)
                {
                    _angle -= 360;
                }
            }
        }
        else
        {
            if (_isInverse)
            {
                _angle -= _speed * Time.deltaTime;
                if (_angle < _angleStart + _angleMin)
                {
                    _isInverse = false;
                    _isActive = (_isOnce) ? false : true;
                    SetMaterial(_isActive);
                }
            }
            else
            {
                _angle += _speed * Time.deltaTime;
                if (_angle > _angleStart + _angleMax)
                {
                    _isInverse = true;
                    _isActive = (_isOnce) ? false : true;
                    SetMaterial(_isActive);
                }
            }
        }

        switch (_axis)
        {
            case Axis.Picht:
                _head.localRotation = Quaternion.Euler(_angle, 0f, 0f);
                break;
            case Axis.Yaw:
                _head.localRotation = Quaternion.Euler(0, _angle, 0f);
                break;
            case Axis.Roll:
                _head.localRotation = Quaternion.Euler(0, 0f, _angle);
                break;
        }
    }
}
