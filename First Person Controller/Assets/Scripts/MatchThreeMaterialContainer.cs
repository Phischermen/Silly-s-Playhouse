using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Match 3 Material Container", fileName = "New M3 material container")]
public class MatchThreeMaterialContainer : ScriptableObject
{
    
    public Material[] baseMaterials;
    public Material[] colorMaterials = new Material[4];
    public GameObject[] destructionPrefab = new GameObject[4];
}
