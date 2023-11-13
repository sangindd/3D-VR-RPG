using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventManager : MonoBehaviour
{
    #region 상점NPC와 상호작용 됐을때
    public delegate void ShopNPCInteraction(Npc npc);
    public static event ShopNPCInteraction OnShopNPCInteraction;
    public static void HandleOnShopNPCInteraction(Npc npc)
    {
        OnShopNPCInteraction(npc);
    }
    #endregion

    //#region 상점NPC로부터 ShopSlots 스크립트에 Item 데이터를 가져올때
    //public delegate void NPCItemRequest(ObjectiveData[] items);
    //public static event NPCItemRequest OnNPCItemRequest;
    //public static void HandleOnNPCItemReques(ObjectiveData[] items)
    //{
    //    OnNPCItemRequest(items);
    //}
    //#endregion
}
