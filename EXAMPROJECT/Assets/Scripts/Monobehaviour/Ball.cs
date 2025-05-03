using UnityEngine;
using Color = System.Drawing.Color;

public class Ball : MonoBehaviour, ISaveable
{
    
    // Interface implementations
    public string id;
    public Vector2 position;
    
    
    //ball stuff
    
    




    public SaveItem SaveData()
    {
        return new SaveItem(){id = this.id, position = this.transform.position};
    }
}
