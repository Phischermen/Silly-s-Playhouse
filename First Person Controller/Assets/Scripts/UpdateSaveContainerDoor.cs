using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSaveContainerDoor : UpdateSaveContainer
{
    public bool isVent = false;
    public override void Add()
    {
        if (isVent)
        {
            if (!SaveContainer.vents.Exists(gameObject.Equals))
            {
                SaveContainer.vents.Add(gameObject);
            }
        }
        else
        {
            if (!SaveContainer.doors.Exists(gameObject.Equals))
            {
                SaveContainer.doors.Add(gameObject);
            }
        }
        
    }
    public override void Remove()
    {
        if (isVent)
        {
            if (SaveContainer.vents.Exists(gameObject.Equals))
            {
                SaveContainer.vents.Remove(gameObject);
            }
        }
        else
        {
            if (SaveContainer.doors.Exists(gameObject.Equals))
            {
                SaveContainer.doors.Remove(gameObject);
            }
        }
        
    }
}
