using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SaveManager
{
    public static string directory = "/SaveData/";
    public static string Weapon1 = "Weapon1.txt";
    public static string Weapon2 = "Weapon2.txt";
    public static string Skin1 = "Skin1.txt";
    public static string Skin2 = "Skin2.txt";

    public static void SaveWeapon1(List<Item> so)
    {
        ListWrapper<DataWrapper> wrapper = new ListWrapper<DataWrapper>();
        wrapper.List = new List<DataWrapper>();
        for (int i = 0; i < so.Count; i++)
        {
            wrapper.List.Add(new DataWrapper());
            wrapper.List[i].Item_Index = so[i].Item_Index;
            wrapper.List[i].Item_Name = so[i].Item_Name;
            wrapper.List[i].isStatusPay = so[i].isStatusPay;
            wrapper.List[i].isEquipped = so[i].isEquipped;
        }
        string fullPath = Application.persistentDataPath + directory + Weapon1;
        //Debug.Log(fullPath);
        string dir = Application.persistentDataPath + directory;
        string json = JsonUtility.ToJson(wrapper, true);
        Debug.Log(json);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(dir + Weapon1, json);
    }
    public static void SaveWeapon2(List<Item> so)
    {
        ListWrapper<DataWrapper> wrapper = new ListWrapper<DataWrapper>();
        wrapper.List = new List<DataWrapper>();
        for (int i = 0; i < so.Count; i++)
        {
            wrapper.List.Add(new DataWrapper());
            wrapper.List[i].Item_Index = so[i].Item_Index;
            wrapper.List[i].Item_Name = so[i].Item_Name;
            wrapper.List[i].isStatusPay = so[i].isStatusPay;
            wrapper.List[i].isEquipped = so[i].isEquipped;
        }
        string fullPath = Application.persistentDataPath + directory + Weapon2;
        //Debug.Log(fullPath);
        string dir = Application.persistentDataPath + directory;
        string json = JsonUtility.ToJson(wrapper, true);
        Debug.Log(json);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(dir + Weapon2, json);
    }

    public static void SaveSkin1(List<Item> so)
    {
        ListWrapper<DataWrapper> wrapper = new ListWrapper<DataWrapper>();
        wrapper.List = new List<DataWrapper>();
        for (int i = 0; i < so.Count; i++)
        {
            wrapper.List.Add(new DataWrapper());
            wrapper.List[i].Item_Index = so[i].Item_Index;
            wrapper.List[i].Item_Name = so[i].Item_Name;
            wrapper.List[i].isStatusPay = so[i].isStatusPay;
            wrapper.List[i].isEquipped = so[i].isEquipped;
        }
        string fullPath = Application.persistentDataPath + directory + Skin1;
        //Debug.Log(fullPath);
        string dir = Application.persistentDataPath + directory;
        string json = JsonUtility.ToJson(wrapper, true);
        Debug.Log(json);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(dir + Skin1, json);
    }
    public static void SaveSkin2(List<Item> so)
    {
        ListWrapper<DataWrapper> wrapper = new ListWrapper<DataWrapper>();
        wrapper.List = new List<DataWrapper>();
        for (int i = 0; i < so.Count; i++)
        {
            wrapper.List.Add(new DataWrapper());
            wrapper.List[i].Item_Index = so[i].Item_Index;
            wrapper.List[i].Item_Name = so[i].Item_Name;
            wrapper.List[i].isStatusPay = so[i].isStatusPay;
            wrapper.List[i].isEquipped = so[i].isEquipped;
        }
        string fullPath = Application.persistentDataPath + directory + Skin2;
        //Debug.Log(fullPath);
        string dir = Application.persistentDataPath + directory;
        string json = JsonUtility.ToJson(wrapper, true);
        Debug.Log(json);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(dir + Skin2, json);
    }

    public static List<Item> LoadWeapon1()
    {
        string fullPath = Application.persistentDataPath + directory + Weapon1;
        ListWrapper<DataWrapper> wrapper = new ListWrapper<DataWrapper>();
        List<Item> so = new List<Item>();
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            wrapper = JsonUtility.FromJson<ListWrapper<DataWrapper>>(json);
        for (int i = 0; i < wrapper.List.Count; i++)
        {
            so.Add(new Item());
            so[i].Item_Index = wrapper.List[i].Item_Index;
            so[i].Item_Name = wrapper.List[i].Item_Name;
            so[i].isStatusPay = wrapper.List[i].isStatusPay;
            so[i].isEquipped = wrapper.List[i].isEquipped;
        }
                
        }
        else
        {
            //Debug.Log("File không tồn tại , tạo trò chơi mới");
        }

        return so;
    }    
    public static List<Item> LoadWeapon2()
    {
        string fullPath = Application.persistentDataPath + directory + Weapon2;
        ListWrapper<DataWrapper> wrapper = new ListWrapper<DataWrapper>();
        List<Item> so = new List<Item>();
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            wrapper = JsonUtility.FromJson<ListWrapper<DataWrapper>>(json);

        for (int i = 0; i < wrapper.List.Count; i++)
        {
            so.Add(new Item());
            so[i].Item_Index = wrapper.List[i].Item_Index;
            so[i].Item_Name = wrapper.List[i].Item_Name;
            so[i].isStatusPay = wrapper.List[i].isStatusPay;
            so[i].isEquipped = wrapper.List[i].isEquipped;
        }
                
        }
        else
        {
            //Debug.Log("File không tồn tại , tạo trò chơi mới");
        }
        return so;
    }
    public static List<Item> LoadSkin1()
    {
        string fullPath = Application.persistentDataPath + directory + Skin1;
        ListWrapper<DataWrapper> wrapper = new ListWrapper<DataWrapper>();
        List<Item> so = new List<Item>();
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            wrapper = JsonUtility.FromJson<ListWrapper<DataWrapper>>(json);
            for (int i = 0; i < wrapper.List.Count; i++)
            {
                so.Add(new Item());
                so[i].Item_Index = wrapper.List[i].Item_Index;
                so[i].Item_Name = wrapper.List[i].Item_Name;
                so[i].isStatusPay = wrapper.List[i].isStatusPay;
                so[i].isEquipped = wrapper.List[i].isEquipped;
            }
                
        }
        else
        {
            //Debug.Log("File không tồn tại , tạo trò chơi mới");
        }

        return so;
    }
    public static List<Item> LoadSkin2()
    {
        string fullPath = Application.persistentDataPath + directory + Skin2;
        ListWrapper<DataWrapper> wrapper = new ListWrapper<DataWrapper>();
        List<Item> so = new List<Item>();
        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            wrapper = JsonUtility.FromJson<ListWrapper<DataWrapper>>(json);
                
        for (int i = 0; i < wrapper.List.Count; i++)
        {
            so.Add(new Item());
            so[i].Item_Index = wrapper.List[i].Item_Index;
            so[i].Item_Name = wrapper.List[i].Item_Name;
            so[i].isStatusPay = wrapper.List[i].isStatusPay;
            so[i].isEquipped = wrapper.List[i].isEquipped;
        }
        }
        else
        {
            //Debug.Log("File không tồn tại , tạo trò chơi mới");
        }

        return so;
    }
}