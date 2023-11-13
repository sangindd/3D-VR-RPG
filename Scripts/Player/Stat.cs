using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    // ����
    Dictionary<string, int> stat = new Dictionary<string, int>()
    { { "strength", 1 },
        {"dexterity",1 },
        {"intelligence",1 } };
    public int Strength
    {
        get
        {
            return stat["strength"];
        }
    }

    public int Dexterity
    {
        get
        {
            return stat["dexterity"];
        }
    }

    public int Intelligence
    {
        get
        {
            return stat["intelligence"];
        }
    }

    [SerializeField]
    private int point = 3; //���� ����Ʈ
    public float Point { get { return point; } }

    [SerializeField]
    private float defence; //����
    public float Defence { get { return defence; } set { defence = value; } }

    public ScriptablePlayer scriptablePlayer;

    private void Awake()
    {
        scriptablePlayer = Resources.Load<ScriptablePlayer>("Stat");
        scriptablePlayer.Intelligence = 1;
        scriptablePlayer.Strength = 1;
    }

    public void StatUpdateScriptable()
    {
        scriptablePlayer.Intelligence = stat["intelligence"];
        scriptablePlayer.Strength = stat["strength"];
    }

    //���� ����
    public void StatUpdate(string name)
    {
        stat[name]++;
        point--;
        StatUpdateScriptable();
    }

    public void StatUpdate()
    {
        stat[name]++;
    }

    //���߿� ������ �߰��Ǹ� ������
    public void UpdatePlusStats(int strValue = 1, int dexValue = 1, int intValue = 1, int defenceValue = 1) //�������� ���ݰ���
    {
        stat["strength"] += strValue;
        stat["dexterity"] += dexValue;
        stat["intelligence"] += intValue;
        defence += defenceValue;
        StatUpdateScriptable();
    }
    public void UpdateMinusStats(int strValue = 1, int dexValue = 1, int intValue = 1, int defenceValue = 1)
    {
        stat["strength"] -= strValue;
        stat["dexterity"] -= dexValue;
        stat["intelligence"] -= intValue;
        defence -= defenceValue;
        StatUpdateScriptable();
    }

    //������ �� ����Ʈ �߰�
    public void LevelUp()
    {
        point += 3;
        UIManager.instance.statUi.ButtonActiveSetting();
        UIManager.instance.statUi.StatTextUpdate();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("������"))
        {
            point += 5;
            UIManager.instance.statUi.ButtonActiveSetting();
            UIManager.instance.statUi.StatTextUpdate();
        }
    }
}
