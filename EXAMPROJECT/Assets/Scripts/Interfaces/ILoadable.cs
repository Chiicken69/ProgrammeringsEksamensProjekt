using UnityEngine;

public interface ILoadable
{
    string GetId();
    string GetTypeKey();
    void LoadData(SaveItem data);
}
