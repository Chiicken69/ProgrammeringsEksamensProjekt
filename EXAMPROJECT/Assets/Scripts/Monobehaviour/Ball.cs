using System;
using TMPro;
using UnityEngine;
using Color = System.Drawing.Color;

public class Ball : MonoBehaviour, ISaveable, ILoadable
{
    
    
    // Isaveable implementation
    public string id;
    private  Vector2 position;
    
    public SaveItem SaveData()
    {
        return new SaveItem(){id = this.id, position = this.transform.position};
    }

    
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
    public TextMeshProUGUI text;
    public Transform TempTrans;
    
    private void Awake()
    {
        FindReferences();
    }

    private void Update()
    {
        UpdateText();
    }

   
    private void FindReferences()
    {
        //bit of a weird way to get a component ina grandchild.. but it works
        TempTrans = this.GetComponentInChildren<Transform>();
        text = TempTrans.GetComponentInChildren<TextMeshProUGUI>();
        if (TempTrans == null)
        {
            Debug.LogWarning("Trans is null");
        }
        if (text == null)
        {
            Debug.LogWarning("Text is null");
        }
    }

    private void UpdateText()
    {
        text.text = "x: " + transform.position.x + ", y: " + transform.position.y;
    }
    
    




   
}
