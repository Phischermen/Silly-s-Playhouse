using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpdateSaveContainerAmmo : UpdateSaveContainer
{
    public override void Add()
    {
        if (!SaveContainer.ammoObjects.Exists(gameObject.Equals))
        {
            SaveContainer.ammoObjects.Add(gameObject);
        }
    }
    public override void Remove()
    {
        if (SaveContainer.ammoObjects.Exists(gameObject.Equals))
        {
            SaveContainer.ammoObjects.Remove(gameObject);
        }
    }
}
