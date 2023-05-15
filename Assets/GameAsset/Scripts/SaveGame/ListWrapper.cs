using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ListWrapper
{
    public List<DataWrapper> List ;
}
[System.Serializable]
public class DataWrapper
{
    public int Item_Index;
    public string Item_Name;
    public bool isStatusPay; //"isPurchased" xác định xem mục đã được mua hay chưa.
    public bool isEquipped; //"isEquipped" xác định xe

    public DataWrapper(int itemIndex, string itemName, bool isStatusPay, bool isEquipped)
    {
        Item_Index = itemIndex;
        Item_Name = itemName;
        this.isStatusPay = isStatusPay;
        this.isEquipped = isEquipped;
    }

    public DataWrapper()
    {
    }
}
