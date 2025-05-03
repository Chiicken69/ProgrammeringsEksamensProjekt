using System.Collections.Generic;
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
    }
   
}
