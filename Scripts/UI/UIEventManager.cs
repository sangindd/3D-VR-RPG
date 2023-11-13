using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventManager : MonoBehaviour
{
    #region ����NPC�� ��ȣ�ۿ� ������
    public delegate void ShopNPCInteraction(Npc npc);
    public static event ShopNPCInteraction OnShopNPCInteraction;
    public static void HandleOnShopNPCInteraction(Npc npc)
    {
        OnShopNPCInteraction(npc);
    }
    #endregion

    //#region ����NPC�κ��� ShopSlots ��ũ��Ʈ�� Item �����͸� �����ö�
    //public delegate void NPCItemRequest(ObjectiveData[] items);
    //public static event NPCItemRequest OnNPCItemRequest;
    //public static void HandleOnNPCItemReques(ObjectiveData[] items)
    //{
    //    OnNPCItemRequest(items);
    //}
    //#endregion
}
