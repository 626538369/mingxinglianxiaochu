using UnityEngine;
using System.Collections;

public class SkillSlot : MonoBehaviour 
{
	public PackedSprite skillIcon;
	public PackedSprite legendIcon;
	public SpriteText titleText;
	public SpriteText descText;
		
	public void Hide(bool hide)
	{
		transform.gameObject.SetActiveRecursively(!hide);
	}
	
	public void UpdateSkillData(GeneralData data)
	{
		if (0 == data.SkillDatas.Values.Count)
		{
			// Hide all
			if (null != transform)
			{
				transform.gameObject.SetActiveRecursively(false);
			}
		}
		else
		{
			// Show all
			if (null != transform)
			{
				transform.gameObject.SetActiveRecursively(true);
			}
			
			foreach (SkillData skillData in data.SkillDatas.Values)
			{
				titleText.Text = skillData.BasicData.SkillName;
				descText.Text = skillData.BasicData.SkillDesc;
				
				//skillIcon.PlayAnim(skillData.BasicData.SkillIcon);
				//legendIcon.PlayAnim("DamageRange" + skillData.BasicData.SkillLegendID.ToString());
				break;
			}
		}
	}
}
