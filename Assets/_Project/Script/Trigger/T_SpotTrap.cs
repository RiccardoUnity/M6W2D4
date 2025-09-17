using UnityEngine;
using CM = GameManagerStatic.Main.ColorManager;
using SM = GameManagerStatic.Main.StringManager;

public class T_SpotTrap : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private Transform _pivotHead;
    [SerializeField] private MeshRenderer _headRenderer;
    [SerializeField] private float _speedRotation = 2f;
    private bool _isPlayerIn;
    [SerializeField] private Material _on;
    [SerializeField] private Material _off;

    void Start()
    {
        _light.color = CM.GetLightSceneColor();
        _light.enabled = false;
        _headRenderer.materials[1] = _off;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(SM.TagPlayer()))
        {
            _isPlayerIn = true;
            _light.enabled = true;
            _headRenderer.materials[1] = _on;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isPlayerIn)
        {
            Vector3 target = other.transform.position + Vector3.up * 0.5f;
            Vector3 direction = target - _pivotHead.transform.position;
            Quaternion targetRot = Quaternion.LookRotation(direction);
            _pivotHead.rotation = Quaternion.Slerp(_pivotHead.rotation, targetRot, _speedRotation * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(SM.TagPlayer()))
        {
            _isPlayerIn = false;
            _light.enabled = false;
            _headRenderer.materials[1] = _off;
        }
    }
}
