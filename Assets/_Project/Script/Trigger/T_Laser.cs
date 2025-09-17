using UnityEngine;
using UnityEngine.Events;

public class T_Laser : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private Material _on;
    [SerializeField] private Material _off;
    private Material[] _materials;

    [SerializeField] private GameObject[] _lasers;

    public UnityEvent onLaserOn;
    public UnityEvent onLaserOff;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materials = new Material[_meshRenderer.materials.Length];
        _on = _meshRenderer.sharedMaterials[1];
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = _meshRenderer.sharedMaterials[i];
        }
    }

    public void LaserOn()
    {
        _materials[1] = _on;
        _meshRenderer.materials = _materials;
        for (int i = 0; i < _lasers.Length; i++)
        {
            _lasers[i].SetActive(true);
        }
        onLaserOn?.Invoke();
    }

    public void LaserOff()
    {
        _materials[1] = _off;
        _meshRenderer.materials = _materials;
        for (int i = 0; i < _lasers.Length; i++)
        {
            _lasers[i].SetActive(false);
        }
        onLaserOff?.Invoke();
    }
}
