using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLevelUI : MonoBehaviour
{
    public Image[] skillIcons;
    public GameObject[] skillLevelBars; //스킬 레벨바 이미지들
    private List<Item> acquiredSkills = new List<Item>();

    public void AddSkill(Item newSkill)
    {
        if (newSkill.data.itemType == ItemData.ItemType.Heal) return;

        bool skillExists = false;

        foreach(Item skill in acquiredSkills)
        {
            if (skill.data == newSkill.data)
            {
                skill.level = newSkill.level;
                skillExists = true;
                break;
            }
        }

        if(!skillExists)
        {
            acquiredSkills.Add(newSkill);
        }
        
        UpdateSkillUI();

    }

    public void UpdateSkillUI()
    {
        
        for (int i = 0; i < acquiredSkills.Count && i < skillIcons.Length; i++)
        {
            skillIcons[i].sprite = acquiredSkills[i].data.itemIcon;            
            skillIcons[i].gameObject.SetActive(true);

            for(int j = 0; j <5; j++)
            {
                skillLevelBars[i*5 + j].SetActive(acquiredSkills[i].level >j);
            }
        }
    }
}
