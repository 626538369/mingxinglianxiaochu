using UnityEngine;
using System.Collections;

public class KnightPortaitSlot : MonoBehaviour 
{
	public SpriteText nameText;
	public PackedSprite portaitIcon;
	public PackedSprite pinzhiIcon;
	
	public PackedSprite skillBG;
	public PackedSprite skillIcon;
	
	[HideInInspector] public GirlData warshipData;
	
	public void Hide(bool hide)
	{
		gameObject.SetActiveRecursively(!hide);
	}
	
	public void UpdateSlot(GirlData data)
	{
		// Vertical text, so don't use [#FFFFFFFF]
//		Color col = GUIFontColor.ConvertColor(GirlBasicData.GetDisplayColor(data.BasicData.RarityLevel));
//		nameText.Text = data.BasicData.Name;
//		nameText.SetColor(col);
		
//		string icon = data.BasicData.Icon.Replace("Avatar", "");
//		icon = "Big" + icon;
//		portaitIcon.PlayAnim(icon);
//		pinzhiIcon.PlayAnim(PinzhiIconNames[data.BasicData.RarityLevel - 1]);
		
		UpdateSkillData(data);
	}
	
	public void UpdateSlot(string name, string avatarIcon)
	{
		skillBG.transform.localScale = Vector3.one;
		skillIcon.transform.localScale = Vector3.one;
		pinzhiIcon.transform.localScale = Vector3.one;
		
		// Vertical text, so don't use [#FFFFFFFF]
		if (string.IsNullOrEmpty(name))
		{
			nameText.transform.parent.localScale = Vector3.zero;
		}
		else
		{
			nameText.transform.parent.localScale = Vector3.one;
			int rarityLevel = 3;
			Color col = GUIFontColor.ConvertColor(PackageUnit.GetItemDataNameColor(rarityLevel));
			nameText.Text = name;
			nameText.SetColor(col);
		}
		
		string icon = avatarIcon.Replace("Avatar", "");
		icon = "Big" + icon;
		portaitIcon.PlayAnim(icon);
	}
	
	void UpdateSkillData(GirlData data)
	{
		if (-1 == data.WarshipGeneralID)
		{
			skillIcon.transform.localScale = Vector3.one;
		}
		else
		{
			skillIcon.transform.localScale = Vector3.zero;
			
			GeneralData glData = Globals.Instance.MGameDataManager.MActorData.GetGeneralData(data.WarshipGeneralID);
			if (null != glData)
			{
				foreach (SkillData skillData in glData.SkillDatas.Values)
				{
					skillIcon.PlayAnim(skillData.BasicData.SkillIcon);
					break;
				}
			}
		}
	}
	
	bool IsInBattleFormation(GirlData data)
	{
		bool isFind = false;
		PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
		
		GameData.FormationData fmtData = null;
		if (actorData.MHoldFormationDataList.TryGetValue(actorData.currSelectFormationID, out fmtData))
		{
			foreach (int shipId in fmtData._dictFormationLocationShip.Values)
			{
				if (shipId == data.roleCardId)
				{
					isFind = true;
					break;
				}
			}
		}
		
		return isFind;
	}
		
	static readonly string[] PinzhiIconNames = new string[]{"PinzhiDing", "PinzhiBing", "PinzhiYi", "PinzhiJia", "PinzhiShen"};
}
