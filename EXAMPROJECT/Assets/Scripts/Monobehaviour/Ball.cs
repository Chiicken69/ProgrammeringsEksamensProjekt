using System;
using UnityEngine;
using Color = System.Drawing.Color;

public class Ball : MonoBehaviour, ISaveable, ILoadable
{
    
    // Isaveable implementation
    public string id;
    private  Vector2 position;

    
    //Iloadable implementation
    public string GetId()
    {
        return id;
    }

    public void LoadData(SaveItem data)
    {
        transform.position = new Vector2(data.position.x, data.position.y);
    }


    //ball stuff
    
    




    public SaveItem SaveData()
    {
        return new SaveItem(){id = this.id, position = this.transform.position};
    }
}
