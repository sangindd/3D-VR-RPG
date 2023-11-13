using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField] Stat stat;
    [SerializeField] Player player;
    [SerializeField] Hp hp;

    //�ѤѤѤѤѤѤѤѽ��ݵ�ѤѤѤѤѤѤѤ�
    [SerializeField]
    private TextMeshProUGUI strText;
    [SerializeField]
    private TextMeshProUGUI dexText;
    [SerializeField]
    private TextMeshProUGUI intText;
    [SerializeField]
    private TextMeshProUGUI unusedPointsText;

    //�ѤѤѤѤѤѽ������ͽ��ѤѤѤѤѤѤ�
    [SerializeField]
    private TextMeshProUGUI dmgText; //������
    [SerializeField]
    private TextMeshProUGUI defText; //����
    [SerializeField]
    private TextMeshProUGUI hpText; //ü�¹� �߾ӿ������� 111/111
    [SerializeField]
    private TextMeshProUGUI expText; //����ġ�� �߾ӿ������� 111/ 111

    [SerializeField]
    private TextMeshProUGUI criText; //크리확률
    [SerializeField]
    private TextMeshProUGUI cdrText; //쿨감

    //�ѤѤѤѤѤ�hp��, ����ġ�٤ѤѤѤѤѤ�
    [SerializeField]
    private Image hpBar;
    [SerializeField]
    private Image expBar;

    [SerializeField]
    private Button[] buttons;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        hp = player.gameObject.GetComponent<Hp>();
        stat = player.gameObject.GetComponent<Stat>();
        StatTextUpdate();
        buttons = GetComponentsInChildren<Button>();
        ButtonActiveSetting();
    }

    private void Update()
    {
        StartCoroutine(Process());
    }
    public void StatUpdate(string name)
    {
        stat.StatUpdate(name);
    }


    public void HpbarUpdate() //hp�� �̹��� ���� / ���߿� �������� �ǰݵ����� �ֱ�
    {
        hpText.text = hp.CurrentHp.ToString() + " / " + hp.MaxHp.ToString();
        hpBar.fillAmount = (float)hp.CurrentHp / hp.MaxHp;
    }

    public void EXPUpdate() //����ġ�� �̹�������
    {
        expText.text = player.CurExp.ToString() + " / " + player.MaxExp.ToString();
        expBar.fillAmount = (float)(player.CurExp / player.MaxExp);
    }


    public void ButtonActiveSetting() //����Ʈ�� 0�̸� ��ư�Ⱥ��̰�  ��ư�̺�Ʈ���� ���.
    {
        if (stat.Point > 0)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void StatTextUpdate() //�ؽ�Ʈ ������Ʈ ��ư�̺�Ʈ���� ���.
    {
        strText.text = stat.Strength.ToString();
        dexText.text = stat.Dexterity.ToString();
        intText.text = stat.Intelligence.ToString();
        unusedPointsText.text = stat.Point.ToString();
        defText.text = stat.Defence.ToString();
        criText.text = player.cri.ToString() + "%";
        cdrText.text = player.cdr.ToString() + "%";
        if (UIInteractor.instance.GrabInteractableObject != null)
            try {
                dmgText.text = (player.dmg + UIInteractor.instance.GrabInteractableObject.GetComponent<Damage>().GetDamage()).ToString();
            }
            catch
            {
                dmgText.text = player.dmg.ToString();
            }
        else
            dmgText.text = player.dmg.ToString();

    }

    IEnumerator Process()
    {
        while (true)
        {
            HpbarUpdate();
            EXPUpdate();
            StatTextUpdate();
            yield return new WaitForSeconds(0.2f);
        }
    }


}
