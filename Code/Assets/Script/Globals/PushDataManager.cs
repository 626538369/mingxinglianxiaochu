using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Push_Tpye
{
	BAG_PUSH = 1,
	GIRLDATING_PUSH = 2,
	GIRLREMAINDER_PUSH = 3,
	EMAIL_PUSH = 4,
	BUDDY_PUSH = 5,
	SHOP_PUSH = 6,
	JOB_PUSH = 7,
	SUBJECT_PUSH = 8,
	MISSION_PUSH = 9,
	DORMITORY_PUSH = 10,
	FRIEND_ENERGY = 12,
};

public class PushData
{
	public Push_Tpye pushType;
	public int pushConfigID;
	public long pushItemID;
	public int pushParam;
	public long pushTime = -1;
};


public class PushDataManager : MonoBehaviour
{
	public UISprite pushTips;
	
	public UISprite PushTipsOverLayers;
	
	public UISprite NewTips;
	
	private Transform mRootTran = null;
	
	public Transform  mCurrentReadPushTransform;
	
	public GameObject TalkObj;
	
	void Awake()
	{
	}
	
	void Start()
	{
		GameObject go = GameObject.Find("UICamera");
		if(null != go)
		{
			mRootTran = go.transform;
		}
		
		mTriggerUIList.Clear();
		mTriggerUIList.Add("GUIMyFriend");
		mTriggerUIList.Add("GUIShipBag");
		mTriggerUIList.Add("GUIDating");
		mTriggerUIList.Add("EnterPort");
		mTriggerUIList.Add("GUIBuildExplore");
		mTriggerUIList.Add("GUIStudy");
//		mTriggerUIList.Add("GUIJob");
		mTriggerUIList.Add("GUIDormitory");
		mTriggerUIList.Add("GUIPotting");
		mTriggerUIList.Add("GUIMain");
		mTriggerUIList.Add("GUIMission");
		
	}
	
	/// <summary>
	/// Gets the instance of item data manager
	/// </summary>
	/// <value>
	/// The instance.
	/// </value>

	
	/// <summary>
	/// Adds the item data.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='data'>
	/// Data.
	/// </param>
	public void AddItemData(Push_Tpye type, PushData data)
	{
		
		List<PushData> tList = GetItemDataList(type);
		tList.Add(data);
		
	}
	
	/// <summary>
	/// Removes the item data by the type and data LocationID
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='data'>
	/// Data.
	/// </param>
	public void RemoveItemData(Push_Tpye type, PushData data)
	{
		RemoveItemData(type,data.pushItemID);
	}
	
	private const string  CTbuildingTag = "EZ3DItemParent/";
	private const string  CSpliteTag = "/";
	public void RefreshPushUI(string uiName="")
	{
		if (!mTriggerUIList.Contains(uiName))
			return;
		PushDataConfig pushConfig = Globals.Instance.MDataTableManager.GetConfig<PushDataConfig>();
		PushDataConfig.PushElement pushElement = new PushDataConfig.PushElement();
		Transform targetTran = null;
		
		///当点击的不是需要推送的建筑的时候，建筑探索功能界面不需要加通知图标 直接返回

		foreach (List<PushData> pushDataList in mItemDataListArray.Values)
		{
			foreach (PushData pushData in pushDataList)
			{
				pushConfig.GetItemElement(pushData.pushConfigID,out pushElement);
				if(null !=pushElement )
				for (int i=0; i<pushElement.PushUIInfoList.Count; i++)
				{
					PushDataConfig.PushUIInfo pushUIInfo = pushElement.PushUIInfoList[i];
					
					if (pushUIInfo.targetUIName.StartsWith(CTbuildingTag))
					{
						string[] keyValues = pushUIInfo.targetUIName.Split(CSpliteTag.ToCharArray());
						targetTran = getBuildingTransform(keyValues[1]);
						string pushStr = pushData.pushConfigID.ToString() + "PushDataUISprite";
						if (targetTran != null && targetTran.Find(pushStr) == null)
						{
							UISprite jianTou = GameObject.Instantiate(PushTipsOverLayers) as UISprite;
							jianTou.name = pushStr;
							jianTou.transform.parent = targetTran;
							jianTou.transform.localPosition = pushUIInfo.targetUIPosition;
							jianTou.transform.localEulerAngles = new Vector3(0,180,0);
							jianTou.transform.localScale = 0.4f*Vector3.one;
						}
					}
					else	
					{
						if (pushUIInfo.targetUIName.Contains("GUIBuildExplore"))
						{
							if (! isClickPushBuilding())
								break;
						}
						if(pushData.pushTime > 0 && Mathf.CeilToInt(Time.time) - pushData.pushTime < 0)
						{
							break;
						}
						targetTran = mRootTran.Find(pushUIInfo.targetUIName);
						string pushStr = pushData.pushConfigID.ToString() + "PushDataUISprite";
						if (targetTran != null && targetTran.Find(pushStr) == null)
						{
							UISprite jianTou = GameObject.Instantiate(pushTips) as UISprite;
							jianTou.name = pushStr;
							jianTou.transform.parent = targetTran;
							jianTou.transform.localPosition = pushUIInfo.targetUIPosition;
							if (pushUIInfo.targetUIName.Contains("GUIMain"))
								jianTou.transform.localScale = new Vector3(1,1,1);
							else
								jianTou.transform.localScale = new Vector3(1,1,1);
							//	jianTou.MakePixelPerfect();
								
						}
					}
	
				}
			}
		}
	}
	
	public void RemovePushUI(PushData pushData)
	{
		PushDataConfig pushConfig = Globals.Instance.MDataTableManager.GetConfig<PushDataConfig>();
		PushDataConfig.PushElement pushElement = new PushDataConfig.PushElement();
		Transform targetTran = null;
		
		
		pushConfig.GetItemElement(pushData.pushConfigID,out pushElement);
		for (int i=0; i<pushElement.PushUIInfoList.Count; i++)
		{
			PushDataConfig.PushUIInfo pushUIInfo = pushElement.PushUIInfoList[i];
			
			if (pushUIInfo.targetUIName.StartsWith(CTbuildingTag))
			{
				string[] keyValues = pushUIInfo.targetUIName.Split(CSpliteTag.ToCharArray());
				targetTran = getBuildingTransform(keyValues[1]);
			}
			else	
				targetTran = mRootTran.Find(pushUIInfo.targetUIName);
		
		
			string pushStr = pushData.pushConfigID + "PushDataUISprite" ;
			if(null != targetTran)
			{
				Transform pushObjTransform = targetTran.Find(pushStr);
				if (targetTran != null && pushObjTransform != null)
				{
					GameObject.DestroyObject(pushObjTransform.gameObject);
				}
			}
	
		}
		
		if (mCurrentReadPushTransform != null)
		{
			GameObject.DestroyObject(mCurrentReadPushTransform.gameObject);
			mCurrentReadPushTransform = null;
		}
	}
	
	
	/// <summary>
	/// Removes the item data.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='locationID'>
	/// Location ID
	/// </param>
	public void RemoveItemData(Push_Tpye type, long itemGUID)
	{
		List<PushData> tList = GetItemDataList(type);
		foreach(PushData item in tList){
			if(item.pushItemID == itemGUID)
			{
				RemovePushUI(item);
				tList.Remove(item);
				RemoveItemData(type,itemGUID);//由于有多个要删除，一次无法删除//
												//此处循环次数较少，所以用了递归而没有做特殊处理//
				break;
		
			}
		}
	}
	
	/// <summary>
	/// Gets the item data.
	/// </summary>
	/// <returns>
	/// The item data.
	/// </returns>
	/// <param name='iType'>
	/// type of 
	/// </param>
	/// <param name='iLocationID'>
	/// I location I.
	/// </param>
	public PushData GetItemData(Push_Tpye type, long locationID)
	{
		List<PushData> tList = GetItemDataList(type);
		foreach(PushData item in tList){
			if(item.pushItemID == locationID){
				return item;
			}
		}
		
		return null;
	}
	
	
	
	/// <summary>
	/// Gets the item data list.
	/// </summary>
	/// <returns>
	/// The item data list.
	/// </returns>
	/// <param name='type'>
	/// Type. item solt type
	/// </param>
	public List<PushData> GetItemDataList(Push_Tpye iType)
	{
		List<PushData> tList;
		if(!mItemDataListArray.ContainsKey(iType)){
			// new a list 
			//
			tList = new List<PushData>();
			mItemDataListArray.Add(iType,tList);
			
		}else{
			tList = mItemDataListArray[iType];
		}
		
		return tList;
	}
	
	/// <summary>
	/// Removes the item data list.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	public void RemoveItemDataList(Push_Tpye type){
		GetItemDataList(type).Clear();
	}
			
	public void ClearItemDataList()
	{
		foreach(Push_Tpye itemSlotType in mItemDataListArray.Keys)
		{
			RemoveItemDataList(itemSlotType);
		}
		mItemDataListArray.Clear();
	}
	
	private Transform getBuildingTransform(string name)
	{
		Dictionary<int, Building> holdBuildingList =  GameStatusManager.Instance.MPortStatus.GetHoldBuildingList();
		if(holdBuildingList == null)
			return null;
		foreach(int key in holdBuildingList.Keys)
		{
			if(holdBuildingList[key].U3DGameObject.name == name)
			{
				return holdBuildingList[key].U3DGameObject.transform;
			}
		}
		return null;
	
	}
	
	private bool isClickPushBuilding()
	{
		Dictionary<int, Building> holdBuildingList =  GameStatusManager.Instance.MPortStatus.GetHoldBuildingList();
		if(holdBuildingList == null)
			return false;
		int buildID = Globals.Instance.MGameDataManager.MActorData.BuildID;
		int nnnbuildID = Globals.Instance.MNpcManager.getCurrentInteractBuildingLogicID();
		string currentBuildName = "";
		
		PushDataConfig pushConfig = Globals.Instance.MDataTableManager.GetConfig<PushDataConfig>();
		PushDataConfig.PushElement pushElement = new PushDataConfig.PushElement();
		
		foreach(int key in holdBuildingList.Keys)
		{
			if(key == buildID)
			{
				currentBuildName = holdBuildingList[key].U3DGameObject.name;
				break;
			}
		}
		
		if (currentBuildName == "")
			return false;
		
		foreach (List<PushData> pushDataList in mItemDataListArray.Values)
		{
			foreach (PushData pushData in pushDataList)
			{
				pushConfig.GetItemElement(pushData.pushConfigID,out pushElement);
				if(null != pushElement)
				for (int i=0; i<pushElement.PushUIInfoList.Count; i++)
				{
					PushDataConfig.PushUIInfo pushUIInfo = pushElement.PushUIInfoList[i];
					
					if (pushUIInfo.targetUIName.Contains(currentBuildName))
					{
						return true;
					}
				}
			}
		}
		
		return false;
	}
	
	bool inPushTriggerUI(string uiName)
	{
		return mTriggerUIList.Contains(uiName);
	}
	
	private List<string> mTriggerUIList = new List<string>();
	private Dictionary<Push_Tpye, List<PushData>> mItemDataListArray = new Dictionary<Push_Tpye, List<PushData>>();
	
}
