using Defines.PoolDefines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class  WaterMillPlatform : MonoBehaviour, IPoolable
{
    private Vector3 position;
    public Vector3 Position
    {
        get => position;
        set
        {
            position = value;
            transform.localPosition = value;
        }
    }
    private WaterMill parent;

    public void SetWaterMill(WaterMill _parent)
    {
        parent = _parent;
    }
}

public partial class WaterMillPlatform : MonoBehaviour, IPoolable
{
    [SerializeField] private PoolType type;
    IPoolable.ReturnToPool _returnToPool;
    IPoolable.ReturnToPool IPoolable.returnToPool { get => _returnToPool; set => _returnToPool = value; }

    IPoolable IPoolable.Create(IPoolable.ReturnToPool returnToPool)
    {
        IPoolable clone = Instantiate(this);
        this._returnToPool = returnToPool;
        return clone;
    }

    void IPoolable.Dequeue()
    {
        gameObject.SetActive(true);
    }

    void IPoolable.Enqueue()
    {
        gameObject.SetActive(false);
        _returnToPool.Invoke(type, this);
    }
}
