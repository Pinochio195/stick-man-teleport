using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ListWrapper<T>
{
    public List<T> List;
}

[System.Serializable]
public class ItemData
{
    public int Item_Index;
    public string Item_Name;
    public bool isStatusPay;
    public bool isEquipped;

    public ItemData(int itemIndex, string itemName, bool isStatusPay, bool isEquipped)
    {
        this.Item_Index = itemIndex;
        this.Item_Name = itemName;
        this.isStatusPay = isStatusPay;
        this.isEquipped = isEquipped;
    }
}


public class SaveGame : MonoBehaviour
{
    public static string directory = "/SaveData/";
    public static string fileName = "MyData.txt";
    public static TextAsset jsonFile;

    public static List<Item> data;

    public static void SaveSkin(List<ItemData> list)
    {
        ListWrapper<ItemData> wrapper = new ListWrapper<ItemData>();
        wrapper.List = list;
        string fullPath = Application.persistentDataPath + directory + fileName;
        Debug.Log(fullPath);
        string dir = Application.persistentDataPath + directory;
        string json = JsonUtility.ToJson(wrapper, true);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(dir + fileName, json);
    }

    public static List<ItemData> Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        ListWrapper<ItemData> wrapper = new ListWrapper<ItemData>();
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            wrapper = JsonUtility.FromJson<ListWrapper<ItemData>>(json);
        }

        return wrapper.List;
    }
    
}