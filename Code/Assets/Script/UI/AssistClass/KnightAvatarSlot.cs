using UnityEngine;
using System.Collections;

public class KnightAvatarSlot : MonoBehaviour 
{
	public UIToggle eventBtn;
	public UISprite knightIcon;
	public UISprite iconSelect;
	public UISprite pinzhiIcon;
	
	public GirlData warshipData;
	
	[HideInInspector] public bool IsNeedDynamicAdjustColl = false;
	[HideInInspector] public ObjMoveControl MoveCtl = null;
	[HideInInspector] public Vector3 Size
	{
		get { return size; }
	}
	
	Vector3 size = Vector3.zero;
	void Awake()
	{
		//size = new Vector3(eventBtn.collider.bounds.size.x, eventBtn.collider.bounds.size.y, 0.0f);
		SetHightlight(false);

	}
	
	void Update()
	{
		DynamicLittleModifyTransform();
	}
	
	bool modifyPlusY = false;
	Vector3 tmpPos = Vector3.zero;
	void DynamicLittleModifyTransform()
	{
		if (!IsNeedDynamicAdjustColl)
			return;
		
		tmpPos.x = transform.position.x;
		tmpPos.y = transform.position.y;
		tmpPos.z = transform.position.z;
		if (Time.frameCount % 8 == 0)
		{
			tmpPos.y = modifyPlusY ? tmpPos.y + 0.005f : tmpPos.y - 0.005f;
			modifyPlusY = !modifyPlusY;
		}
		transform.position = tmpPos;
	}
	
	public void SetValueChangedDelegate(UIEventListener.VoidDelegate del)
	{
		UIEventListener.Get(eventBtn.gameObject).onClick += del;
	}
	
	public void AddDragDropDelegate(UIEventListener.ObjectDelegate del)
	{
		eventBtn.enabled = true;
		UIEventListener.Get(eventBtn.gameObject).onDrop  += del;
	}
	
	public void IgnoreCollider(bool ignore)
	{
		if (null != eventBtn && null != eventBtn.transform.GetComponent<Collider>())
		{
			eventBtn.GetComponent<Collider>().enabled = !ignore;
		}
	}
	
	public void Hide(bool hide, bool useScale = true)
	{
		if (useScale)
		{
			NGUITools.SetActive(gameObject,hide);
		}
		else
		{
			gameObject.SetActiveRecursively(!hide);
		}
	}
	
	public void SetChecked(bool check)
	{
		eventBtn.isChecked = check;
		SetHightlight(check);
	}
	
	public void SetHightlight(bool light)
	{
		NGUITools.SetActive(iconSelect.gameObject,light);
	}
	
	public void UpdateSlot(GirlData data)
	{
		warshipData = data;
		
		SetChecked(false);
		eventBtn.Data = data;
		
		NGUITools.SetActive(pinzhiIcon.gameObject,false);
//		pinzhiIcon.spriteName =  PinzhiIconNames[data.BasicData.RarityLevel - 1];
		
//		knightIcon.spriteName = data.BasicData.Icon;
	}
	
	public void UpdateAvatarIcon(string icon)
	{
		warshipData = null;
		
		SetHightlight(false);
		NGUITools.SetActive(pinzhiIcon.gameObject,false);
		NGUITools.SetActive(iconSelect.gameObject,false);
		
		knightIcon.spriteName = icon;
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
	
	static readonly string[] PinzhiIconNames = new string[]{"KuangDing", "KuangBing", "KuangYi", "KuangJia", "KuangShen"};
}
