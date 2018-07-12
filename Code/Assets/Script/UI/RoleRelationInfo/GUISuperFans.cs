using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUISuperFans : GUIWindowForm {

	public UIButton BackHomeBtn;
	public UIScrollView SuperFansUIScrollView;
	public UIGrid SuperFansUIGrid;
	public GameObject SuperFansItem;
	public UILabel SuperFansLabel;
	
	protected override void Awake()
	{		
		if (null == Globals.Instance.MGUIManager)
			return;
		base.Awake();



		UIEventListener.Get(BackHomeBtn.gameObject).onClick += delegate(GameObject go) {

			this.Close();
		};


	}
	
	protected virtual void Start ()
	{
		base.Start();
		
	}
	
	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		
		this.GUILevel = 8;
		ShowSuperFansInfor();
	}
	 

	public void ShowSuperFansInfor()
	{
		WarshipConfig warshipConfig = Globals.Instance.MDataTableManager.GetConfig<WarshipConfig>();
		List<int> _mWarshipElementList = Globals.Instance.MGameDataManager.MActorData.WarshipList;
		Dictionary<int, WarshipConfig.WarshipObject> warshipDic = warshipConfig.getWarshipDic();
	
		int warshipCount = 5;
		if(_mWarshipElementList.Count >= 5)
		{
			warshipCount = _mWarshipElementList.Count + 1;
		}

		SuperFansLabel.text = "[1f961f]"+_mWarshipElementList.Count+"[-][000000]/"+warshipDic.Count+"[-]";
		HelpUtil.DelListInfo(SuperFansUIGrid.transform);
		for(int i = 0; i < warshipCount; i++)
		{
			GameObject superFansItem = GameObject.Instantiate(SuperFansItem)as GameObject;
			superFansItem.transform.parent = SuperFansUIGrid.transform;
			superFansItem.transform.localScale = Vector3.one;
			superFansItem.transform.localPosition = Vector3.zero;
			
			UITexture iconTexture = superFansItem.transform.Find("IconTexture").GetComponent<UITexture>();
			GameObject fansInfo = superFansItem.transform.Find("FansInfo").gameObject;
			UILabel nameLabel = fansInfo.transform.Find("NameLabel").GetComponent<UILabel>();
			UILabel describeLabel = fansInfo.transform.Find("DescribeLabel").GetComponent<UILabel>();
			UILabel fansSayLabel = fansInfo.transform.Find("FansSayLabel").GetComponent<UILabel>();
			UILabel unKnownLabel = superFansItem.transform.Find("UnKnownLabel").GetComponent<UILabel>();

			WarshipConfig.WarshipObject mPair = null;
			if(_mWarshipElementList[i]!=null&&_mWarshipElementList.Count>i &&warshipDic.TryGetValue(_mWarshipElementList[i],out mPair))
			{
				iconTexture.mainTexture =  Resources.Load("Icon/FansIcon/"+mPair.Fans_Icon,typeof(Texture2D)) as Texture2D;
				nameLabel.text = mPair.Name;
				describeLabel.text = mPair.Art_Describe;
				fansSayLabel.text = mPair.Fans_Say;
				superFansItem.name = "AAItem" + i;
				NGUITools.SetActive(unKnownLabel.gameObject , false);
				NGUITools.SetActive(fansInfo , true);
			}
			else
			{
				iconTexture.mainTexture =  Resources.Load("Icon/FansIcon/UnKnown",typeof(Texture2D)) as Texture2D;
				NGUITools.SetActive(unKnownLabel.gameObject , true);
				NGUITools.SetActive(fansInfo , false);
				superFansItem.name = "BBItem" + i;
			}
		}
		SuperFansUIGrid.sorting = UIGrid.Sorting.Custom;
		SuperFansUIGrid.repositionNow = true;
		SuperFansUIScrollView.ResetPosition();
	}

	void OnDestroy()
	{
		base.OnDestroy();	
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}
}


