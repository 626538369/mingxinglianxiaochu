using UnityEngine;
using System.Collections;

public class Building
{
	//-----------------------------------------------------------
	public const float NAME_HEIGHT_OFFSET = 20.0f;
	//-----------------------------------------------------------
	
	public string GameObjectTag
	{
		get 
		{ 
			if (null == _mGameObject)
				return null;
			
			return _mGameObject.tag; 
		}
		set 
		{
			if (_mGameObject != null)
				_mGameObject.tag = value; 
		}
	}
	
	public GameObject U3DGameObject
	{
		get { return _mGameObject; }
	}
	
	public BuildingProperty Property
	{
		get { return _mProperty; }
	}
	
	public int InstanceID
	{
		get { return _mInstanceID; }
	}
	
	public Building(BuildingData data)
	{
		_mData = data;
		_mInstanceID = data.LogicID;
		_mModelName = data.ModelName;
	}
	
	public virtual void Initialize()
	{
		if (null == _mModelName)
		{
			Debug.LogWarning("Building::Initialize is not complete.");	
			return;
		}
		
		Object ob = Resources.Load(_mModelName);
		if (null == ob)
		{
			Debug.LogWarning("The resource " + _mModelName + " is not exist?");	
			return;
		}
		
		_mGameObject = GameObject.Instantiate(ob, Vector3.zero, Quaternion.identity) as GameObject;
		
		if(_mGameObject == null){
			Debug.LogWarning("The resource " + _mModelName + " is Empty prefab!");
			return;
		}
		
		_mGameObject.name = _mData.Name;
		GameObject.DontDestroyOnLoad(_mGameObject);
		
		TagMaskDefine.SetTagRecuisively(_mGameObject, TagMaskDefine.GFAN_NPC);
		
		AddPropertyComp();
		Create3DName();
	}
	
	public virtual void Release()
	{
		GameObject.DestroyObject(_mGameObject);
		Globals.Instance.M3DItemManager.DestroyEZ3DItem(_mEZ3DName.gameObject);
	}
	
	public virtual void ForceMoveTo(ref Vector3 destPosition)
	{
		_mGameObject.transform.position = destPosition;
	}
	
	public virtual void ForceRotateTo(ref Quaternion orientation)
	{
		_mGameObject.transform.rotation = orientation;
	}
	
//	public void UpdateNamePosition()
//	{
//		Vector3 worldPos = _mGameObject.transform.position;
//		worldPos.y += NAME_HEIGHT_OFFSET;
//		_mEZ3DName.Reposition(worldPos);
//	}
	
	public void SetHighLightMaterial()
	{
		int matCount = _mGameObject.GetComponent<Renderer>().sharedMaterials.Length;
		for (int i = 0; i < matCount; ++i)
		{
			Material mat = _mGameObject.GetComponent<Renderer>().sharedMaterials[i];
			if ( mat.HasProperty("_MainColor") )
			{
				mat.SetColor("_MainColor", Color.white);
			}
		}
	}
	
	private void AddPropertyComp()
	{
		// Add Property Component
		_mProperty = _mGameObject.AddComponent( typeof(BuildingProperty) ) as BuildingProperty;
		
		// Fill in BuildingProperty
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
	
	private void Create3DName()
	{		
		string showText = GUIFontColor.Yellow + _mProperty.Name;
		_mEZ3DName = Globals.Instance.M3DItemManager.Create3DSimpleText(_mGameObject, showText,NAME_HEIGHT_OFFSET, delegate(GameObject obj) 
		{
			InteractWith(_mProperty.Type);
		});
		
		_mEZ3DName.gameObject.name = _mProperty.Name;
	}
	
	private void Show3DName(bool show)
	{
		_mEZ3DName.gameObject.SetActiveRecursively(show);
	}
	
	private void InteractWith(EBuildingType type)
	{
		if(Globals.Instance.MTaskManager.GoToTalk(_mProperty.LogicID, delegate(){
			if(type == EBuildingType.DEFENCE_FACILITY || type == EBuildingType.MARINE_BOARD)
			{
				Globals.Instance.MTeachManager.NewBuildingClickedEvent(_mProperty.Name);
			}
		}))
		{
			return;
		}	
		
		Globals.Instance.MTeachManager.NewBuildingClickedEvent(_mProperty.Name);
		
		switch (type)
		{
		case EBuildingType.TTRN:
		{
		
			break;
		}
		case EBuildingType.DEFENCE_FACILITY:
		{
			break;
		}
		case EBuildingType.MATERIALS_CENTRE:
		{
			int seaID = Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID;
			NetSender.Instance.RequestShopInfo(seaID, 0);
			break;
		}
		case EBuildingType.DIRECTORATE:
		{
			int seaID = Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID;
			NetSender.Instance.RequestGeneralGetHarborGeneral(seaID);
			break;
		}
		case EBuildingType.PLAYER_PORT:
		{
			NetSender.Instance.RequestPlayerDockInfo();
			break;
		}
		case EBuildingType.MARINE_BOARD:
		{
			string prompt = Globals.Instance.MDataTableManager.GetWordText(11030062);
			Globals.Instance.MNpcManager.ShowBuildingTips(this, prompt);
			break;
		}
		
		case EBuildingType.SHIPYARD:
		{
			int seaID = Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID;
			NetSender.Instance.RequestShipBlueprintEngineerGetCdTime(seaID);
			break;
		}
		
		case EBuildingType.TECHNOLOGY:
		{
			long actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
			// NetSender.Instance.RequestGirlInfoList(actorID);
			NetSender.Instance.RequestGetTechnology();
			break;
		}
		case EBuildingType.OIL_REFINERY:
		{
			string sFormat = Globals.Instance.MDataTableManager.GetWordText(11030059);
			string prompt = string.Format(sFormat, Globals.Instance.MGameDataManager.MCurrentPortData.UserInvestData.Contribution);
			
			sFormat = Globals.Instance.MDataTableManager.GetWordText(11030060);
			prompt += "\n";
			prompt += GUIFontColor.White255255255 + string.Format(sFormat, 
				Globals.Instance.MGameDataManager.MCurrentPortData.UserInvestData.OilOutput);
			Globals.Instance.MNpcManager.ShowBuildingTips(this, prompt);
			break;
		}
		case EBuildingType.STATUE:
		{

			break;
		}
			
		}
	}
	
	// Class variable value
	private int _mInstanceID;
	protected GameObject _mGameObject = null;
	protected EZ3DSimipleText _mEZ3DName = null;
	protected BuildingProperty _mProperty = null;
	protected BuildingData _mData = null;
	
	protected string _mModelName = null;
}
