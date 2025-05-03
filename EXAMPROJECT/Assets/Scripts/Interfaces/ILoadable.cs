using UnityEngine;

public interface ILoadable
{
    string GetId();
    
    void LoadData(SaveItem data);
}
