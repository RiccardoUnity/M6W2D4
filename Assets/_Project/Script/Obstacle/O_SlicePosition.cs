using UnityEngine;

public class O_SlicePosition : O_Obstacle
{
    enum Position
    {
        Start,
        End,
        Middle
    }

    [SerializeField] private Position _positionStart = Position.Start;
    [SerializeField] private bool _useVectorInLocalSpace = true;
    [SerializeField] private Vector3 _start;
    [SerializeField] private Vector3 _end;
    private Vector3 _direction;
    private float _distanceSqr;
    private Vector3 _position;
    private bool _isDistanceMinOne;

    public override void ResetData()
    {
        if (_start == _end)
        {
            Debug.LogWarning("Manca il settaggio di _start o di _end", gameObject);
        }
        else
        {
            switch (_positionStart)
            {
                case Position.Start:
                    if (_useVectorInLocalSpace)
                    {
                        _end += transform.position;
                    }
                    _start = transform.position;
                    _position = _start;
                    break;
                case Position.End:
                    if (_useVectorInLocalSpace)
                    {
                        _start += transform.position;
                    }
                    _end = transform.position;
                    _position = _end;
                    break;
                case Position.Middle:
                    if (_useVectorInLocalSpace)
                    {
                        _start += transform.position;
                        _end += transform.position;
                    }
                    _position = transform.position;
                    break;
            }

            //Da _start a _end
            _direction = (_end - _start).normalized;
            _distanceSqr = (_end - _start).sqrMagnitude;
            _isDistanceMinOne = (_distanceSqr < 1f) ? true : false;
        }
    }

    //Spero di non aver sintetizzato un po' troppo ...
    protected override void TransformChange()
    {
        _position += _direction * _speed * Time.deltaTime * ((_isInverse) ? -1 : 1);
        if (_isDistanceMinOne)
        {
            //
            if ((_position - ((!_isInverse) ? _start : _end)).sqrMagnitude < _distanceSqr)
            {
                ChangeDirection();
            }
        }
        else
        {
            //
            if ((_position - ((!_isInverse) ? _start : _end)).sqrMagnitude > _distanceSqr)
            {
                ChangeDirection();
            }
        }
        transform.position = _position;
    }

    private void ChangeDirection()
    {
        _position = (_isInverse) ? _start : _end;
        _isInverse = !_isInverse;
        _isActive = (_isOnce) ? false : true;
        SetMaterial(_isActive);
    }
}
