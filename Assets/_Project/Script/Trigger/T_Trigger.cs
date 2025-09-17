using UnityEngine;
using UnityEngine.Events;
using SM = GameManagerStatic.Main.StringManager;

public class T_Trigger : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;

    private int _count;

    protected virtual void Awake() { }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(SM.TagPlayer()) || other.tag.Equals(SM.TagBox()))
        {
            if (_count == 0)
            {
                onEnter?.Invoke();
                EnterRenderer();
            }
            _count++;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(SM.TagPlayer()) || other.tag.Equals(SM.TagBox()))
        {
            _count--;
            if (_count == 0)
            {
                onExit?.Invoke();
                ExitRenderer();
            }
        }
    }

    protected virtual void EnterRenderer() { }

    protected virtual void ExitRenderer() { }
}
