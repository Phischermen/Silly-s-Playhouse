using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script needs to be executed before whatever script triggers the room reset
//Goto Project Settings > Script Execution Order
public class ChildrenResetter : MonoBehaviour
{
    
    GameObject Clones;
    [SerializeField][Tooltip("Gameobjects that will be ignored by this script")]
    private Transform[] transformBlackList; 
    public HashSet<Transform> transformBlackListSet = new HashSet<Transform>();
    private bool cloneQueued;
    
    void Start()
    {
        foreach (Transform item in transformBlackList)
        {
            transformBlackListSet.Add(item);
        } 
        Clones = new GameObject("Clones");
        Clones.transform.parent = transform;
        Clones.SetActive(false);
        CloneChildren();
    }

    public void Update()
    {
        //A queue is necessary because gameobjects are not destroyed until the end of the frame
        if (cloneQueued )
        {
            CloneChildren();
            cloneQueued = false;
        }
    }

    //Delete children, reparent the children of Clones, and queue a clone
    public void ResetRoom()
    {
        DeleteChilderen();
        int n = Clones.transform.childCount;
        for (int i = 0; i < n; ++i)
        {
            Transform child = Clones.transform.GetChild(0);
            child.parent = transform;

        }
        cloneQueued = true;
    }

    //Clone gameobject's children and make them children of the inactive Clones gameobject
    public void CloneChildren()
    {
        int n = transform.childCount;
        for (int i = 0; i < n; ++i)
        {
            Transform child = transform.GetChild(i);
            if(childIsMeantForCloning(child))
            {
                Transform clone = Instantiate(child,Clones.transform,true);
                clone.name = child.name;
            }
        }
    }

    //Delete children, except for ones on the blacklist.
    public void DeleteChilderen()
    {
        int n = transform.childCount;
        for (int i = 0; i < n; ++i)
        {
            Transform child = transform.GetChild(i);
            if (childIsMeantForCloning(child))
            {
                Destroy(child.gameObject);
            }
        }
    }

    //Checks blacklist and if transform is the parent of the clones
    private bool childIsMeantForCloning(Transform child)
    {
        return child.gameObject != Clones && !transformBlackListSet.Contains(child);
    }
}
