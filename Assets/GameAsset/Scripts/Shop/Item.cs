using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class Item : MonoBehaviour
{
      public int Item_Index;
     public string Item_Name;
     public bool isStatusPay;//"isPurchased" xác định xem mục đã được mua hay chưa.
     public bool isEquipped; //"isEquipped" xác định xem mục đã được trang bị hay chưa.
     public Item(int itemIndex, string itemName, bool isStatusPay, bool isEquipped)
     {
          this.Item_Index = itemIndex;
          this.Item_Name = itemName;
          this.isStatusPay = isStatusPay;
          this.isEquipped = isEquipped;
     }

     public Item()
     {
     }
}