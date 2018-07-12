using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameData;
public class FormationCell : MonoBehaviour 
{
	public UIToggle eventBtn;
	
	public KnightAvatarSlot avatarSlot;
	public UISprite plusIcon;
	
	public SpriteText eyePropText;
	
	public int fmtLogicId = -1;
	public FormationData.SingleLocation singleCellData = null;
	public GirlData warshipData = null;
	
	// void Awake()
	// {
	// 	avatarSlot.IgnoreCollider(true);
	// }
	
	public void SetValueChangedDelegate(UIEventListener.VoidDelegate del)
	{
		UIEventListener.Get(eventBtn.gameObject).onClick +=del;
		avatarSlot.SetValueChangedDelegate(del);
	}
	
	public void AddDragDropDelegate(UIEventListener.ObjectDelegate del)
	{
		avatarSlot.AddDragDropDelegate(del);
	}
	
	public void SetChecked(bool check)
	{
		eventBtn.isChecked = check;
		avatarSlot.SetChecked(check);
	}
	
	public void UpdateCellData(FormationData fmtData, FormationData.SingleLocation cellData)
	{
		warshipData = null;
		
		singleCellData = cellData;
		eventBtn.Data = cellData;
		
		if (null == cellData)
		{
			NGUITools.SetActive(avatarSlot.transform.gameObject,true);
			NGUITools.SetActive(plusIcon.gameObject,false);
			NGUITools.SetActive(eyePropText.transform.parent.gameObject,false);
		}
		else
		{
			fmtLogicId = cellData._locationX + cellData._locationY * FormationData.MaxCellRow;
			
			if(cellData.isArrayEye)
			{				
				NGUITools.SetActive(eyePropText.transform.parent.gameObject,true);
				SetEyeBuffData(cellData);
			}
			else
			{
				NGUITools.SetActive(eyePropText.transform.parent.gameObject,false);
			}
			
			if(cellData.canUse)
			{
				// Has ship
				if(fmtData._dictFormationLocationShip.ContainsKey(fmtLogicId))
				{
					long tShipID = fmtData._dictFormationLocationShip[fmtLogicId];
					if(Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList().ContainsKey(tShipID))
					{
						GirlData tWarshipData = Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList()[tShipID];
						warshipData = tWarshipData;
						
						NGUITools.SetActive(avatarSlot.gameObject,false);
						avatarSlot.UpdateSlot(tWarshipData);
						avatarSlot.eventBtn.Data = cellData;
						
						NGUITools.SetActive(plusIcon.gameObject,false);
					}
					else
					{
						NGUITools.SetActive(plusIcon.gameObject,true);
					}
				}
				else
				{
					NGUITools.SetActive(avatarSlot.gameObject,true);
					NGUITools.SetActive(plusIcon.gameObject,true);
				}
			}
			else
			{				
				NGUITools.SetActive(avatarSlot.gameObject,true);
				NGUITools.SetActive(plusIcon.gameObject,false);
			}
		}
	}
	
	public void UpdateCellData(FormationData.SingleLocation cellData, GirlData shipData)
	{
		singleCellData = cellData;
		eventBtn.Data = cellData;
		
		// Lock
		if (null == cellData)
		{

			NGUITools.SetActive(avatarSlot.gameObject,true);
			NGUITools.SetActive(plusIcon.gameObject,false);
			NGUITools.SetActive(eyePropText.transform.parent.gameObject,false);
		}
		else
		{
			if(cellData.isArrayEye)
			{
				NGUITools.SetActive(eyePropText.transform.parent.gameObject,true);
				SetEyeBuffData(cellData);
			}
			else
			{
				NGUITools.SetActive(eyePropText.transform.parent.gameObject,false);
			}
			
			warshipData = shipData;
			if (null != shipData)
			{
				NGUITools.SetActive(avatarSlot.gameObject,false);
				avatarSlot.UpdateSlot(shipData);
				
				// Specific setting, unify the UIButton event with its parent
				avatarSlot.eventBtn.Data = cellData;
				
				NGUITools.SetActive(plusIcon.gameObject,false);
			}
			else
			{
				NGUITools.SetActive(avatarSlot.gameObject,true);
				NGUITools.SetActive(plusIcon.gameObject,true);
			}
		}
	}
	
	void SetEyeBuffData(FormationData.SingleLocation cellData)
	{
		string textCol = GUIFontColor.Red255000000;
		if (cellData.buffData.Name.Contains("+"))
			textCol = GUIFontColor.Cyan000255204;
		
		int influenceVal = cellData.buffData.Influence1.Value;
		string wordHit = Globals.Instance.MDataTableManager.GetWordText(10420032);
		string wordDodge = Globals.Instance.MDataTableManager.GetWordText(10420033);
		string wordCrit = Globals.Instance.MDataTableManager.GetWordText(10420034);
		if (cellData.buffData.Name.Contains(wordHit)
			|| cellData.buffData.Name.Contains(wordDodge)
			|| cellData.buffData.Name.Contains(wordCrit))
		{
			// Ensure the display value is least 1 or -1
			if (Mathf.Abs(influenceVal) % 100 < 50)
			{
				if (influenceVal < 0)
					influenceVal -= 50;
				else 
					influenceVal += 50;
			}
			
			// Round the value
			influenceVal = Mathf.RoundToInt(influenceVal / 100.0f);
		}
		
		eyePropText.Text = textCol + cellData.buffData.Name + influenceVal.ToString();
	}
	
	readonly string[] PinzhiIconNames = new string[]{"KuangDing", "KuangBing", "KuangYi", "KuangJia", "KuangShen"};
}
