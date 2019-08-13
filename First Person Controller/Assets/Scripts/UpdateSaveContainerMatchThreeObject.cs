using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSaveContainerMatchThreeObject : UpdateSaveContainer
{
    public override void Add()
    {
        if (!SaveContainer.matchThreeObjects.Exists(gameObject.Equals))
        {
            SaveContainer.matchThreeObjects.Add(gameObject);
        }
    }
    public override void Remove()
    {
        if (SaveContainer.matchThreeObjects.Exists(gameObject.Equals))
        {
            SaveContainer.matchThreeObjects.Remove(gameObject);
        }
    }
}
