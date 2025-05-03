using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

public class SaveManager : MonoBehaviour
{


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
[ContextMenu("Load all")]
    public void CallLoadAll()
    {
        //find save file
        string path = Path.Combine(Application.persistentDataPath, "savegame.json");
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found at: " + path);
            return;
        }
        
        // Read and parse the JSON save file :)
        string json = File.ReadAllText(path);
        SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);
        
        Debug.Log("Loaded JSON with " + (wrapper.items?.Count ?? 0) + " items");

        //find all loadable behaviours in scene and index them
        var existing = new Dictionary<string, ILoadable>();
        MonoBehaviour[] Behaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        for (int i = 0; i < Behaviours.Length; i++)
        {
            ILoadable loadable = Behaviours[i] as ILoadable;
            if (loadable != null)
            {
                existing[loadable.GetId()] = loadable;
            }
        }
        
        // apply the data

        int applied = 0;
        foreach (var item in wrapper.items)
        {
            if (existing.ContainsKey(item.id))
            {
                existing[item.id].LoadData(item);
                applied++;
            }
            else
            {
                Debug.LogWarning($"No Iloadable with ID '{item.id}' in scene");
            }
        }
        Debug.Log($"Applied data to {applied}/{wrapper.items.Count} loadable objects");
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
   
}
