using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
   
    public enum InfoType { Exp, Level, Kill, Time, Health, Skill } //ȭ�鿡 ǥ���� ���� ���� ����
    public InfoType type; //HUD���� ������ ���� ����
    public ItemData skillItemData; // ��ų ������ ������
    public int skillLevel; //��ų ����
        
       

    Text myText; //Text ������Ʈ    
    Image skillIcon; //��ų ������ �̹���
    Text skillLevelText; //��ų ���� �ؽ�Ʈ
    public Image expBar;
    public Image healthBar;
    
    

    private void Awake()
    {
        myText = GetComponent<Text>();
        

        if (type == InfoType.Skill)
        {
            skillIcon = transform.Find("Icon").GetComponent<Image>();
            skillLevelText = transform.Find("SkillLevelText").GetComponent<Text>();
        }
                
    } 

    private void LateUpdate()
    {
        //���õ� ���� ������ ���� UI ������Ʈ
        switch(type)
        {
            case InfoType.Exp:
                //����ġ �����̴� ������Ʈ
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                expBar.fillAmount = curExp/ maxExp;
                
                break;

            case InfoType.Level:
                //���� �ؽ�Ʈ ������Ʈ
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level) ;
                break;

            case InfoType.Time:
                //���� �ð� �ؽ�Ʈ ������Ʈ
                float elapsedTime = GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(elapsedTime / 60);
                int sec = Mathf.FloorToInt(elapsedTime % 60); //% = ������
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case InfoType.Kill:
                //óġ �� �ؽ�Ʈ ������Ʈ
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;

            case InfoType.Health:
                //ü�� �����̴� ������Ʈ
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxhealth;
                healthBar.fillAmount = curHealth / maxHealth;
                break;
            case InfoType.Skill:
                //��ų UI ������Ʈ
                UpdateSkillUI();
                break;

        }

    }

    void UpdateSkillUI()
    {       
        //��ų �����ܰ� ���� ������Ʈ
        skillIcon.sprite = skillItemData.itemIcon;
        skillLevelText.text = "Lv." + (skillLevel);
    }

    
}
