using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private int _sizeStackPool = 5;
    [SerializeField] private Bullet _bullet;
    private Queue<Bullet> _bulletsPool = new Queue<Bullet>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        StackBullets();
    }

    private void StackBullets()
    {
        Bullet bullet;
        for (int i = 0; i < _sizeStackPool; i++)
        {
            bullet = Instantiate(_bullet, transform);
            _bulletsPool.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        if (_bulletsPool.Count == 0)
        {
            StackBullets();
        }
        return _bulletsPool.Dequeue();
    }

    public void ReturnToPool(Bullet bullet)
    {
        _bulletsPool.Enqueue(bullet);
    }
}
