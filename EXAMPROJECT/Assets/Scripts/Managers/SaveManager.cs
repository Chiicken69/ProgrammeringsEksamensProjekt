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
