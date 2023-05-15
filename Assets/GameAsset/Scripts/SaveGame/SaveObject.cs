using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject
{
    public int Item_Index;
    public string Item_Name;
    public bool isStatusPay; //"isPurchased" xác định xem mục đã được mua hay chưa.
    public bool isEquipped; //"isEquipped" xác định xem mục đã được trang bị hay chưa.
    public SaveObject(int itemIndex, string itemName, bool isStatusPay, bool isEquipped)
    {
         this.Item_Index = itemIndex;
         this.Item_Name = itemName;
         this.isStatusPay = isStatusPay;
         this.isEquipped = isEquipped;
    }
}