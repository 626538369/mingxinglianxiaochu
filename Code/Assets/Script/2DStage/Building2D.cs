using UnityEngine;
using System.Collections;

public class Building2D : Building
{
	public Building2D(BuildingData data) : base(data)
	{
	}
	
	public override void Initialize()
	{
		// Now use the _mData.ModelNam field to search GameObject in current scene
		_mGameObject = GameObject.Find(_mData.NPCIcon);
		//_mGameObject.transform.localScale = Vector3.one;
		
		if (null == _mGameObject)
		{
			Debug.LogError("Not found the building " + _mData.ModelName);
			return;
		}
		
		TagMaskDefine.SetTagRecuisively(_mGameObject, TagMaskDefine.GFAN_NPC);
		
		SyncPropComponet();
		//SyncBuildingSet();
	}
	
	public override void Release()
	{
	}
	
	public override void ForceMoveTo(ref Vector3 destPosition)
	{
	}
	
	public override void ForceRotateTo(ref Quaternion orientation)
	{
	}
	
	void SyncBuildingSet()
	{
		BuildingSet blSet =_mGameObject.GetComponent<BuildingSet>() as BuildingSet;
		if (null == blSet)
			blSet =_mGameObject.AddComponent<BuildingSet>() as BuildingSet;	
		
		blSet.UpdateData(_mData);
		blSet.SetValueChangedDel(delegate(GameObject go){
			Globals.Instance.MNpcManager.InteractWith(this);
		});
	}
	
	void SyncPropComponet()
	{
		// Add Property Component
		_mProperty = _mGameObject.AddComponent( typeof(BuildingProperty) ) as BuildingProperty;
		
		_mProperty.ID = _mData.ID;
		_mProperty.LogicID = _mData.LogicID;
		_mProperty.Type = _mData.Type;
		_mProperty.Name = _mData.Name;
		
		_mProperty.NpcIcon = _mData.NPCIcon;
		_mProperty.NpcName = _mData.NPCName;
		
		_mProperty.ModelName = _mData.ModelName;
		
		_mProperty.FunctionID = _mData.FunctionID;
		_mProperty.BuffID = _mData.BuffID;
		_mProperty.CampID = _mData.CampID;
	}
	
	void Create3DName()
	{		
		string showText = GUIFontColor.Yellow + _mProperty.Name;
	}
}