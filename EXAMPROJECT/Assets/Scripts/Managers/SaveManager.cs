
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
     
    
    public GameObject[] prefabs;
    [SerializeField] public Dictionary<string, GameObject> prefabByType;

    private void Awake()
    {
        //singleton pattern :3
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        
        // build the dictionary so key=typeName, value=prefab
        prefabByType = new Dictionary<string, GameObject>();
        foreach (var prefab in prefabs)
        {
            prefabByType[prefab.name] = prefab;
            Debug.Log("Added: (" + prefab.name + ") to the dictionary" );
            
        }
        
    }


    /// <summary>
    /// Searches all behaviours in scene and saves all objects with interface Isaveable to data list
    /// </summary>
    /// <returns></returns>
    [ContextMenu("Save all")]
    
    public void CallSaveOnAll()
    {
        var Data = new List<SaveItem>();
        MonoBehaviour[] allBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        for (int i = 0; i < allBehaviours.Length; i++)
        {
            //safe cast all behaviours til Isaveable. Null hvis behaviour ikke implementere isaveable
            ISaveable saveable = allBehaviours[i] as ISaveable;

            if (saveable != null)
            {
               SaveItem item = saveable.SaveData();
                Data.Add(item);
            }
            
        }
        Debug.Log("Scanned: " + allBehaviours.Length + " behaviours");
        Debug.Log(Data.Count + " items added to data list");
        WriteToDisk(SerializeToJson(WrapData(Data)));
    }
    /// <summary>
    /// Deletes all objects in scene with Iloadable and instantiates new objects with data from save file
    /// </summary>
    /// <returns></returns>
[ContextMenu("Load all")]
    public void CallLoadAll()
    {
        string path = Path.Combine(Application.persistentDataPath, "savegame.json");
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found at: " + path);
            return;
        }

        // Step 1: Load the data
        string json = File.ReadAllText(path);
        SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);
        Debug.Log("Loaded JSON with " + (wrapper.items?.Count ?? 0) + " items");

        // Step 2: Destroy all existing ILoadable objects
        CleanUpScene();

        // Step 3: Recreate everything from save
        int applied = 0;
        foreach (var item in wrapper.items)
        {
            if (prefabByType.TryGetValue(item.type, out GameObject prefab))
            {
                GameObject tempGameObject = BallSpawner.instance.SpawnBall(prefab);
                ILoadable loadable = tempGameObject.GetComponent<ILoadable>();
                if (loadable != null)
                {
                    loadable.LoadData(item);
                    applied++;
                }
                else
                {
                    Debug.LogError($"Prefab {item.type} has no ILoadable component.");
                }
            }
            else
            {
                Debug.LogWarning($"No prefab found for type {item.type}");
            }
        }

        Debug.Log($"Instantiated and applied data to {applied}/{wrapper.items.Count} objects");
    }

    private string SerializeToJson(SaveWrapper WrappedData)
    {
        string json = JsonUtility.ToJson(WrappedData, true);
        return json;
    }

    private void WriteToDisk(string json)
    {
        string path = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log("Saving to: " + Application.persistentDataPath);
        File.WriteAllText(path, json);
        

    }

    private SaveWrapper WrapData(List<SaveItem> data)
    {
        return new SaveWrapper {items = data };
        
        
    }
    /// <summary>
    /// Searches all behaviours and deletes objects with Iloadable interface
    /// </summary>
    /// <returns></returns>
    private void CleanUpScene()
    {
        MonoBehaviour[] behaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        for (int i = 0; i < behaviours.Length; i++)
        {
            ILoadable loadable = behaviours[i] as ILoadable;
            if (loadable != null)
            {
                Destroy(behaviours[i].gameObject);
            }
        }
    }

}


