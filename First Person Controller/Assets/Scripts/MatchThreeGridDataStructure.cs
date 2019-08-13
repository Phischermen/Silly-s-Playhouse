using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchThreeGridDataStructure : MonoBehaviour
{
    Dictionary<Vector3Int,HashSet<GameObject>> match3_grid = new Dictionary<Vector3Int, HashSet<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Add(Vector3Int coord,GameObject obj)
    {
        if (match3_grid.ContainsKey(coord) == true)
        {
            match3_grid[coord].Add(obj);
        }
        else
        {
            match3_grid.Add(coord,new HashSet<GameObject>());
            match3_grid[coord].Add(obj);
        }
            
    }
    public GameObject[] Get(Vector3Int coord)
    {
        if(match3_grid.ContainsKey(coord) == true)
        {
            HashSet<GameObject> cell = match3_grid[coord];
            GameObject[] collection = new GameObject[cell.Count];
            match3_grid[coord].CopyTo(collection);
            return collection;
        }
        else
        {
            return null;
        }
    }
    public void Remove(Vector3Int coord, GameObject obj)
    {
        if (match3_grid.ContainsKey(coord) == true)
        {
            HashSet<GameObject> cell = match3_grid[coord];
            cell.Remove(obj);
            if(cell.Count == 0)
            {
                match3_grid.Remove(coord);
            }
        }
    }
}
