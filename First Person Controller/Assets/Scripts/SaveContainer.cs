using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveContainer : MonoBehaviour
{
    private string savePath;
    private static string latestSave;
   
    public static GameObject CharacterObject;
    public static GameObject AmmoObject;
    public static MatchThreeObjectTypes matchThreeObjectTypes;
    public static List<GameObject> ammoObjects = new List<GameObject>();
    public static List<GameObject> doors = new List<GameObject>();
    public static List<GameObject> vents = new List<GameObject>();
    public static List<GameObject> matchThreeObjects = new List<GameObject>();
    public static List<GameObject> tutorials = new List<GameObject>();
    private static MyCharacterController player;
    private void Awake()
    {
        CharacterObject = Resources.Load<GameObject>("Character");
        AmmoObject = Resources.Load<GameObject>("AmmoPickup");
        matchThreeObjectTypes = Resources.Load<MatchThreeObjectTypes>("MatchThreeObjectTypes");
        savePath = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        DirectoryInfo directory = new DirectoryInfo(savePath);
        FileInfo[] saves = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).ToArray();
        if (saves.Length > 0)
            latestSave = saves[0].Name;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharacterController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Save("quicksave.save");
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            LoadLatest();
        }
    }
    public void clear()
    {
        ammoObjects.Clear();
        doors.Clear();
        matchThreeObjects.Clear();
        tutorials.Clear();
    }
    public static Save makeSave()
    {
        Save save = new Save(ammoObjects,doors,vents,matchThreeObjects,tutorials,player.gameObject);
        return save;
    }
    public static void AutoSave()
    {
        Save("autosave.save");
    }
    
    public static void Save(string fileName)
    {
        Save save = makeSave();
        latestSave = fileName;
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saves/" + fileName;
        FileStream file = File.Create(path);
        bf.Serialize(file, save);
        Debug.Log(path);
        file.Close();
    }
    public static void Load(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/" + fileName;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            Debug.Log(path + "\nLoaded");
            file.Close();
            readSave(save);
        }
        else
        {
            Debug.Log("File does not exist:\n" + path);
        }
        
    }
    public void LoadLatest()
    {
        Load(latestSave);
    }
    public static void readSave(Save save)
    {
        //Clear existing objects
        ClearMatchThreeObjects();
        ClearAmmoObjects();
        DestroyCharacter();
        //Create Player
        playerInfo playerInfo = save.player;
        player = Instantiate(CharacterObject).GetComponent<MyCharacterController>();
        player.transform.position = playerInfo.position.GetPosition();
        Vector3 orientation = playerInfo.orientation.GetOrientation();
        player.my_camera.GetComponent<camMouseLook>().Look(orientation);
        player.SetCrouchState(playerInfo.crouched,true);
        player.health = playerInfo.health;
        GunController gunController = player.my_pickup.gunController;
        for(int i = 0;i < playerInfo.ammo.Length; ++i)
        {
            gunController.arsenal[i].gun.ammo = playerInfo.ammo[i];
            gunController.arsenal[i].is_in_arsenal = playerInfo.inArse[i];
        }
        gunController.equip_gun((GunController.gun_type)playerInfo.equip);
        //Create Match Threes
        foreach (matchThreeObjectInfo obj in save.matchThreeObjectInfos)
        {
            GameObject newObj = Instantiate(matchThreeObjectTypes.types[obj.type],GameObject.Find(obj.room).transform);
            MatchThreeObject newObjM3 = newObj.GetComponent<MatchThreeObject>();
            newObj.name = obj.name;
            newObj.transform.position = obj.position.GetPosition();
            newObjM3.match3_group_id = (MatchThreeObject.match3_group_id_names)obj.group;
            newObjM3.apply_color();
        }
        //Create Ammo Pickups
        foreach (ammoInfo obj in save.ammoInfos)
        {
            GameObject newObj = Instantiate(AmmoObject);
            PickupAmmoInteraction newObjAmmo = newObj.GetComponent<PickupAmmoInteraction>();
            newObj.name = obj.name;
            newObj.transform.position = obj.position.GetPosition();
            newObj.transform.eulerAngles = obj.rotation.GetOrientation();
            newObjAmmo.gun = (GunController.gun_type)obj.type;
            newObjAmmo.amount = obj.amount;
        }
        //Set Door Parameters
        foreach(doorInfo obj in save.doorInfos)
        {
            DoorInteraction door = GameObject.Find(obj.name).GetComponentInChildren<DoorInteraction>();
            door.twistLock(obj.locked);
            door.swingDoor(obj.open, false);
        }
        //Temporary code for setting references
        foreach (GameObject tutorial in tutorials)
        {
            tutorial.GetComponent<Tutorial>().GetUIComponents(player.gameObject);
        }
    }
    

    private static void ClearMatchThreeObjects()
    {
        foreach(GameObject obj in matchThreeObjects)
        {
            Destroy(obj);
        }
    }

    private static void ClearAmmoObjects()
    {
        foreach (GameObject obj in ammoObjects)
        {
            Destroy(obj);
        }
    }

    private static void DestroyCharacter()
    {
        if (player)
            Destroy(player.gameObject);
    }
}

[System.Serializable]
public class Save
{
    public List<ammoInfo> ammoInfos { get; private set; } = new List<ammoInfo>() ;
    public List<doorInfo> doorInfos { get; private set; } = new List<doorInfo>();
    public List<matchThreeObjectInfo> matchThreeObjectInfos { get; private set; } = new List<matchThreeObjectInfo>();
    public List<tutorialInfo> tutorialInfos { get; private set; } = new List<tutorialInfo>();
    public playerInfo player;
    public Save(List<GameObject> ammos, List<GameObject> doors, List<GameObject> vents, List<GameObject> matchThreeObjects,List<GameObject> tutorials,GameObject character)
    {
        foreach (GameObject obj in ammos)
        {
            ammoInfos.Add(new ammoInfo(obj.GetComponent<PickupAmmoInteraction>()));
        }
        foreach (GameObject obj in matchThreeObjects)
        {
            matchThreeObjectInfos.Add(new matchThreeObjectInfo(obj.GetComponent<MatchThreeObject>()));
        }
        foreach (GameObject obj in tutorials)
        {
            tutorialInfos.Add(new tutorialInfo(obj.GetComponent<Tutorial>()));
        }
        foreach (GameObject obj in doors)
        {
            doorInfos.Add(new doorInfo(obj.GetComponent<DoorInteraction>()));
        }
        foreach (GameObject obj in vents)
        {
            DoorInteraction vent = obj.GetComponentInChildren<DoorInteraction>();
            doorInfos.Add(new doorInfo(vent.transform.parent.name,vent.isOpen,vent.isLocked));
        }
        player = new playerInfo(character.GetComponent<MyCharacterController>());
    }
}
[System.Serializable]
public struct vector3S
{
    public float x;
    public float y;
    public float z;
    public vector3S(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
}
[System.Serializable]
public struct eulerS
{
    public float x;
    public float y;
    public float z;
    public eulerS(Vector3 e)
    {
        x = e.x;
        y = e.y;
        z = e.z;
    }
    public eulerS(float _x,float _y,float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    public Vector3 GetOrientation()
    {
        return new Vector3(x, y, z);
    }
}
[System.Serializable]
public struct ammoInfo
{
    public string name;
    public int amount;
    public int type;
    public vector3S position;
    public eulerS rotation;

    public ammoInfo(PickupAmmoInteraction obj)
    {
        name = obj.name;
        amount = obj.amount;
        type = (int)obj.gun;
        position = new vector3S(obj.transform.position);
        rotation = new eulerS(obj.transform.eulerAngles);
    }
}
[System.Serializable]
public struct doorInfo
{
    public string name;
    public bool open;
    public bool locked;

    public doorInfo(DoorInteraction obj)
    {
        name = obj.name;
        open = obj.isOpen;
        locked = obj.isLocked;
    }
    public doorInfo(string _name,bool _open,bool _locked)
    {
        name = _name;
        open = _open;
        locked = _locked;
    }
}
[System.Serializable]
public struct matchThreeObjectInfo
{
    public string name;
    public int type;
    public int group;
    public vector3S position;
    public string room;
    public matchThreeObjectInfo(MatchThreeObject obj)
    {
        name = obj.name;
        type = obj.match3Type;
        group = (int)obj.match3_group_id;
        position = new vector3S(obj.transform.position);
        room = obj.grid.name;
    }
}
[System.Serializable]
public struct tutorialInfo
{
    public string name;
    public tutorialInfo(Tutorial obj)
    {
        name = obj.name;
    }
}
[System.Serializable]
public struct playerInfo
{
    public int health;
    public bool[] inArse;
    public int[] ammo;
    public int equip;
    public vector3S position;
    public eulerS orientation;
    public bool crouched;
    public playerInfo(MyCharacterController obj)
    {
        health = obj.health;
        GunController gunController = obj.GetComponentInChildren<GunController>();
        ammo = new int[gunController.arsenal.Length];
        inArse = new bool[gunController.arsenal.Length];
        equip = gunController.equipped_gun_index;
        for(int i = 0;i < ammo.Length; ++i)
        {
            ammo[i] = gunController.arsenal[i].gun.ammo;
            inArse[i] = gunController.arsenal[i].is_in_arsenal;
        }
        crouched = obj.GetCrouched();
        orientation = new eulerS(obj.my_camera.transform.eulerAngles.x, obj.transform.eulerAngles.y, 0);
        position = new vector3S(obj.transform.position);
    }
}

