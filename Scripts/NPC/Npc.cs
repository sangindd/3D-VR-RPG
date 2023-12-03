using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public enum NpcType { Shop, QuestNPC }
    [SerializeField]
    protected NpcType npcType;

    public NpcType npcType_
    {
        get { return npcType; }
        set { npcType = value; }
    }

    [SerializeField]
    private string Name;
    public string Name_ { get { return Name; } }

    [SerializeField]
    private bool isDialogue; //대화가있는지
    public bool IsDialogue { get { return isDialogue; } }

    //[SerializeField]
    //private List<DialogueData> dialogueDatas;
    //public List<DialogueData> DialogueDatas { get { return dialogueDatas; } }
}