using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGun : Gun
{
    
    public override void fire1(Ray ray)
    {
        RaycastHit hit_info;
        bool object_found = Physics.Raycast(ray, out hit_info, float.MaxValue, LayerMask.GetMask("Default", "InteractSolid"));
        if (object_found && hit_info.transform.tag == "Interactable")
        {
            MatchThreeObject matchThreeObject = hit_info.transform.gameObject.GetComponent<MatchThreeObject>();
            string message = "Coords: \n";
            for (int i = 0; i < matchThreeObject.coord.Length; ++i)
            {
                message += matchThreeObject.coord[i];
                message += "\n";
            }
            Debug.Log(message);
        }
    }
    
}
