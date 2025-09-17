using Cinemachine;
using UnityEngine;

public class CinemachineOption : MonoBehaviour
{
    private CinemachineFreeLook _cinemachine;
    [SerializeField] private int _maxSensibility = 1000;
    [SerializeField] private float _minSensibility = 0;

    void Awake()
    {
        _cinemachine = GetComponent<CinemachineFreeLook>();
    }

    public void SetInverse(bool value) => _cinemachine.m_YAxis.m_InvertInput = !value;

    public void SetSensibility(float value) => _cinemachine.m_XAxis.m_MaxSpeed = Mathf.Lerp(_minSensibility, _maxSensibility, value);
}
