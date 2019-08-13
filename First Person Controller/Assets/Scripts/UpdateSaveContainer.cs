using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSaveContainer : MonoBehaviour
{
    protected void Awake()
    {
        Add();
    }
    private void OnDestroy()
    {
        Remove();
    }
    public virtual void Add()
    {
        
    }
    public virtual void Remove()
    {
        
    }
}
