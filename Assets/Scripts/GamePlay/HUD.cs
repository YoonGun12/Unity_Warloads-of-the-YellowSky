using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
   
    public enum InfoType { Exp, Level, Kill, Time, Health, Skill } //화면에 표시할 정보 유형 정의
    public InfoType type; //HUD에서 보여줄 정보 유형
    public ItemData skillItemData; // 스킬 아이템 데이터
    public int skillLevel; //스킬 레벨
        
       

    Text myText; //Text 컴포넌트    
    Image skillIcon; //스킬 아이콘 이미지
    Text skillLevelText; //스킬 레벨 텍스트
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
        //선택된 정보 유형에 따라 UI 업데이트
        switch(type)
        {
            case InfoType.Exp:
                //경험치 슬라이더 업데이트
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                expBar.fillAmount = curExp/ maxExp;
                
                break;

            case InfoType.Level:
                //레벨 텍스트 업데이트
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level) ;
                break;

            case InfoType.Time:
                //남은 시간 텍스트 업데이트
                float elapsedTime = GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(elapsedTime / 60);
                int sec = Mathf.FloorToInt(elapsedTime % 60); //% = 나머지
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case InfoType.Kill:
                //처치 수 텍스트 업데이트
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;

            case InfoType.Health:
                //체력 슬라이더 업데이트
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxhealth;
                healthBar.fillAmount = curHealth / maxHealth;
                break;
            case InfoType.Skill:
                //스킬 UI 업데이트
                UpdateSkillUI();
                break;

        }

    }

    void UpdateSkillUI()
    {       
        //스킬 아이콘과 레벨 업데이트
        skillIcon.sprite = skillItemData.itemIcon;
        skillLevelText.text = "Lv." + (skillLevel);
    }

    
}
