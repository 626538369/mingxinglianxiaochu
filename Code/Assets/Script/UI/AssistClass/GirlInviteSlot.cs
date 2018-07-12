//#define ZGY

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GirlInviteSlot : MonoBehaviour
{
	public GameObject InviteGirlItem;
	public UITable Table;
	public UIScrollView ScrollView;
	public GameObject NoAtristsFrame;
	public UILabel NumLabel;
	public UILabel TotalARTLabel;
	public UILabel TotalREPLabel;
	int mTotalART;
	int mTotalREP;
	public UIButton SureBtn;
	public int PublishType;
	
	readonly int MaxSeleteNum = 3;
	class MyItemData
	{
		public  UIToggle SeleteBtn;
		public  GameObject UnDoTextureObj;
		public  UILabel UnDoLabel;
		public  long ArtistID;
		public  GirlData ArtistData;
		public  GameObject ItemObj;
		public  GameObject []SequenceObj = new GameObject[3];
	}
	List<MyItemData> ItemList = new List<MyItemData> ();
	public List<long> SeleteID = new List<long>();
	int SeleteNum
	{
		get{return _seleteNum;}
		set
		{
			TotalARTLabel.text = mTotalART.ToString();
			TotalREPLabel.text = mTotalREP.ToString();
			NumLabel.text = value.ToString() + "/3";
			SureBtn.isEnabled = value != 0;
			_seleteNum = value;	
			
		}
	}
	int _seleteNum;
	
	public delegate void OnGirlSelectEvent();
	[HideInInspector] public event GirlInviteSlot.OnGirlSelectEvent GirlSelcectEvents = null;
	
	public delegate void OnLittleBtn(long girlID);
	[HideInInspector] public event GirlInviteSlot.OnLittleBtn LittleBtnEvents = null;
	
	Dictionary<long,UIToggle> mGirlCheckList = new Dictionary<long, UIToggle>();
	
	public enum ButtonType
	{
		NONE = 0,
		INVITE,
		END,
	}
	GameObject mCurrentBtnObj = null;

	public ButtonType buttonType = ButtonType.NONE;
	void Awake()
	{
		UIEventListener.Get(SureBtn.gameObject).onClick = delegate (GameObject Obj)
		{
			GirlSelcectEvents();
		};
	}
	void Start()
	{
		//UpdateGirlInviteList();

		actorGirlListUpdate = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.EVENT_GIRLLIST_UPDATE);
		actorGirlListUpdate.Handler = delegate (object[] args)
		{
			UpdateGirlInviteList();
		};
		UpdateGirlInviteList();

		Globals.Instance.MTeachManager.NewOpenWindowEvent("GirlInviteSlot");
		TotalARTLabel.text = "0";
		TotalREPLabel.text = "0";

	}
	public void ReFresh()
	{
		SeleteNum = 0;
		mTotalART = 0;
		mTotalREP = 0;
		TotalARTLabel.text = "0";
		TotalREPLabel.text = "0";
		SeleteID.Clear();
		foreach(MyItemData Data in ItemList)
		{
			if(Data.SeleteBtn.value)
			{
				Data.SeleteBtn.value = false;
			}
			NGUITools.SetActive(Data.SeleteBtn.gameObject,true);
			
		}
	}
	void OnDestroy()
	{
	
	}
	public void UpdateGirlInviteList()
	{
		HelpUtil.DelListInfo(Table .transform);
		mGirlCheckList.Clear();
		
		WarshipConfig config = Globals.Instance.MDataTableManager.GetConfig<WarshipConfig>();
		WarshipConfig.WarshipObject element = null;
		
		Dictionary<long,GirlData> dicWarShipData =  Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();
		List<GirlData> TempList = new List<GirlData> ();
		foreach (GirlData girlData in dicWarShipData.Values)
		{
			TempList.Add(girlData);
		}
		TempList.Sort(delegate(GirlData datax,GirlData datay)
		{
			if(datax.CardBase.CardRare != datay.CardBase.CardRare)
			{
				return datax.CardBase.CardRare > datay.CardBase.CardRare ? -1 : 1;
			}
			else if(datax.CardBase.CardRank != datay.CardBase.CardRank)
			{
				return datax.CardBase.CardRank > datay.CardBase.CardRank ? 1 : -1;
			}
			return 0;	
		});
		
		Artist_SkillConfig AConfig = Globals.Instance.MDataTableManager.GetConfig<Artist_SkillConfig>();
		Artist_SkillConfig.SkillObject Aelement = null;
		int i = 0; 
		SeleteNum = 0;
		ItemList.Clear();
		SeleteID.Clear();
		foreach (GirlData girlData in TempList)
		{
			if(girlData.CardBase.CardTypeId != 1230000001)
				continue;
			i ++;
			GameObject item = GameObject.Instantiate(InviteGirlItem)as GameObject;
			UIToggle checkbox = item.transform.GetComponent<UIToggle>();	
			//UIButton NextBtn = item.transform.FindChild("Tween").FindChild("GameObject").FindChild("CheckBtn").GetComponent<UIButton>();
			item.name = "GirlInviteSlotItem" + i.ToString();
			item.transform.parent = Table .transform;
			item.transform.localPosition = new Vector3(i,i,-5.0f);
			item.transform.localScale = Vector3.one;
			//NextBtn.Data = girlData.roleCardId;
			UILabel NameLabel = item.transform.Find("AllLabel").Find("NameLabel").GetComponent<UILabel>();
			NameLabel.text = girlData.CardBase.CardName;
			Transform Picture = item.transform.Find("Picture");
			//各种参数赋值//
			UILabel ARTLabel = Picture.Find("ARTSprite").Find("ARTLabel").GetComponent<UILabel>();
			ARTLabel.text = girlData.CardBase.cardArtValue.ToString();
			UILabel LVLabel = Picture.Find("LVSprite").Find("LVLabel").GetComponent<UILabel>();
			LVLabel.text = girlData.CardBase.CardRank.ToString() + "/99";
			UILabel REPLabel = Picture.Find("REPSprite").Find("REPLabel").GetComponent<UILabel>();
			REPLabel.text = girlData.CardBase.cardArtProfile.ToString();
			//HangyeDengjiA
			UISprite GradeSprite = Picture.Find("GradeSprite").GetComponent<UISprite>();
			string name = "";
			if(1 == girlData.CardBase.CardRare)
				name = "HangyeDengjiS";
			else
				name = "HangyeDengji" + ((char)('A' + girlData.CardBase.CardRare - 2)).ToString();
			
			GradeSprite.spriteName = name;
			config.GetWarshipElement(girlData.CardBase.CardId,out element);
			UITexture HeadTexture = Picture.Find("HeadTexture").GetComponent<UITexture>();
//			HeadTexture.mainTexture =  Resources.Load("Icon/ArtistIcon/" + element.Head_Icon,typeof(Texture2D)) as Texture2D;
			for(int j = 0;j < girlData.skillLis.Count;++j)
			{
				UITexture Skill = Picture.Find("SkillTexture" + j.ToString()).GetComponent<UITexture>();
				AConfig.GetSkillObject(girlData.skillLis[j],out Aelement);
				string texturePath =  "Icon/SkillIcon/" + Aelement.Skill_Icon;
				Skill.mainTexture =  Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
				NGUITools.SetActive(Skill.gameObject,true);
			}
			//UIEventListener.Get(NextBtn.gameObject).onClick = OnItemPhoneClick;
			UIToggle SeleteBtn = item.transform.Find("SeleteBtn").GetComponent<UIToggle>();
			SeleteBtn.Data = girlData;
			UIEventListener.Get(SeleteBtn.gameObject).onClick = delegate(GameObject Obj) 
			{
				UIToggle SeleteBtnIn = item.transform.Find("SeleteBtn").GetComponent<UIToggle>();
				GirlData IngirlData = (GirlData)SeleteBtnIn.Data;
				if(SeleteBtnIn.value)
				{
					if(SeleteNum < MaxSeleteNum )
					{
						mTotalART += IngirlData.CardBase.cardArtValue;
						mTotalREP += IngirlData.CardBase.cardArtProfile;
						SeleteNum++;
						SeleteID.Add(IngirlData.roleCardId);
						
					}
					
					if(SeleteNum == MaxSeleteNum)
						IsDisplayUnSelete(false);	
				}
				else
				{
					if(SeleteNum == MaxSeleteNum)
						IsDisplayUnSelete(true);
					mTotalART -= IngirlData.CardBase.cardArtValue;
					mTotalREP -= IngirlData.CardBase.cardArtProfile;
					SeleteNum--;
					SeleteID.Remove(IngirlData.roleCardId);
				}
			};
			
			MyItemData TempData = new MyItemData ();
			TempData.SeleteBtn = SeleteBtn;
			TempData.UnDoTextureObj = Picture.Find("UnDoTexture").gameObject;
			TempData.UnDoLabel = TempData.UnDoTextureObj.transform.Find("UnDoLabel").GetComponent<UILabel>();
			TempData.ArtistID = girlData.roleCardId;
			TempData.ArtistData = girlData;
			TempData.ItemObj = item;
			for(int j = 0;j < 3; ++j)
			{
				TempData.SequenceObj[j] = Picture.Find("SkillTexture" + j.ToString()).Find("Sprite").gameObject;
			}
			ItemList.Add(TempData);
			
		}
		NGUITools.SetActive(NoAtristsFrame,i == 0);
		
		Table.repositionNow = true;
		ScrollView.ResetPosition();
	}
	void  OnClickInvite(GameObject Obj)
	{
		UIImageButton ImBtn = Obj.transform.GetComponent<UIImageButton>();
		if(null != LittleBtnEvents)
		{
			LittleBtnEvents((long)ImBtn.Data);
			//LittleBtnEvents(1217001000);
		}
	}

    public bool SetCheck(long GirlID)
	{
		UIToggle Check = null;
		if(true  == mGirlCheckList.TryGetValue(GirlID,out Check))
		{
			Check.isChecked = true;
			return true;
		}
		else
		{
			return false;
		}
	}
	void IsDisplayUnSelete(bool Sure)
	{
		foreach(MyItemData Data in ItemList)
		{
			if(!Data.SeleteBtn.value)
			{
				NGUITools.SetActive(Data.SeleteBtn.gameObject,Sure);
			}
		}
	}
//	public void UpdateGirlInviteList(sg.GS2C_Publish_Artist_Res res)
//	{
//		ReFresh();
//		PublishConfig  publishConfig = Globals.Instance.MDataTableManager.GetConfig<PublishConfig>();
//		PublishConfig.MagazineObj MagazineData = null;
//		
//		Artist_SkillConfig AConfig = Globals.Instance.MDataTableManager.GetConfig<Artist_SkillConfig>();
//		Artist_SkillConfig.SkillObject Aelement = null;
//		foreach(MyItemData Data in ItemList)
//		{
//			foreach(sg.GS2C_Publish_Artist_Res.Artist_Publish InData in res.artistPublish)
//			{
//				if(InData.artistId.Contains(Data.ArtistID))//寻找已经选用的艺术家//
//				{
//					NGUITools.SetActive(Data.UnDoTextureObj,true);
//					NGUITools.SetActive(Data.SeleteBtn.gameObject,false);
//					publishConfig.GetMagazineObject(InData.publishId ,out  MagazineData);
//					Data.UnDoLabel.text = string.Format( Globals.Instance.MDataTableManager.GetWordText(22020),MagazineData.Type_SmallName);
//					break;
//				}
//			}
//
//			for(int i = 0;i < Data.ArtistData.skillLis.Count; ++i)
//			{
//				AConfig.GetSkillObject(Data.ArtistData.skillLis[i],out Aelement);
//				if(5 == Aelement.Reward_Publish_Type || PublishType == Aelement.Reward_Publish_Type)
//				{
//					NGUITools.SetActive(Data.SequenceObj[i],true);
//					//Data.particleSystem[i].Play();
//				}
//				else
//				{
//					NGUITools.SetActive(Data.SequenceObj[i],false);
//					//Data.particleSystem[i].Stop();
//				}
//			}
//		}
//		ItemList.Sort(delegate(MyItemData datax,MyItemData datay)//按照指定顺序进行排序 //
//		{
//			bool a = NGUITools.GetActive(datax.UnDoTextureObj);
//			bool b = NGUITools.GetActive(datay.UnDoTextureObj);
//			if(a != b)
//			{
//				if(true == a)
//					return -1;
//				else
//					return 1;
//				
//			}
//			else if(datax.ArtistData.CardBase.CardRare != datay.ArtistData.CardBase.CardRare)
//			{
//				return datax.ArtistData.CardBase.CardRare > datay.ArtistData.CardBase.CardRare ? -1 : 1;
//			}
//			else if(datax.ArtistData.CardBase.CardRank != datay.ArtistData.CardBase.CardRank)
//			{
//				return datax.ArtistData.CardBase.CardRank > datay.ArtistData.CardBase.CardRank ? 1 : -1;
//			}
//			return 0;	
//		});
//		for(int i = 0; i < ItemList.Count; ++i)
//		{
//			ItemList[i].ItemObj.transform.localPosition = new Vector3 (i,i,0);
//		}
//		Table.repositionNow = true;
//		ScrollView.ResetPosition();
//	}
	ISubscriber actorGirlListUpdate = null;
}