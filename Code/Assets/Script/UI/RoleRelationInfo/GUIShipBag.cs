using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIShipBag : GUIWindowForm 
{
	public GameObject WealthGroup;
	UITweener mCurrentTweenr;
	
	public UIButton ExitBtn;
	public UIButton BackBtn;
	public UIButton BackBtn1;
	public UIButton ForbitBtn;
	public UIButton ForbitBtn1;
	public UIButton ShopBtn;
	public UITexture BackGroundTexture;
	//FF
	public GameObject FriendFrame;
	public GameObject FriendItem;
	public UIScrollView FFScrollView;
	public UITable FriendTable;
	public GameObject FFMoveScrollView;
	public GameObject NoFriendObj;
	
	enum Status
	{
		START = 0,
		SELL = 1,
		SEND = 2,
	}
	Status mCurrentStatus = Status.START;
	class CurrentBagData
	{
		public  ItemSlotData itemSlotData;
		public  bool bNeedReduce = false;
	}
	CurrentBagData mCurrentBagData;
	//ItemSlotData mCurrentBagData;

	public UILabel NumLabel;
	public UILabel TitleLabel;
	int SailNum
	{
		set
		{
			
			if(value <= MaxSailNum && value >= 1)
			{
				NumLabel.text = value.ToString();
				_sailNum = value;
				if(mCurrentStatus == Status.SELL)
				{
					ItemSlotData InData = mCurrentBagData.itemSlotData;
					long itemLogicID = InData.MItemData.BasicData.LogicID;
					ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
					ItemConfig.ItemElement element = null;
					bool IsHas = config.GetItemElement(itemLogicID, out element);
					TitleLabel.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(5027),_sailNum,_sailNum * element.SellPrice);
				}
				else if(mCurrentStatus == Status.SEND)
				{
					TitleLabel.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(5026),_sailNum);
				}
				AddBtn.isEnabled = value != MaxSailNum;
				SubtractBtn.isEnabled = value != 1;	
			}
		}
		get{return _sailNum;}
	}
	int _sailNum = 1;
	int MaxSailNum = 1;
	
	public GameObject NoteFrame;
	public UIButton AddBtn;
	public UIButton CancelBtn;
	public UIButton EnterBtn;
	public UIButton SubtractBtn;

	public UIButton BackToClothBtn;
	public UIToggle BoyBtn;
	public UIToggle GirlBtn;
	public UIToggle OtherBtn;
	public GameObject CMFMove;
	public GameObject CMFItem;
	public GameObject ClothingMenuFrame;
	//CLF
	int mMaxGouNum = 20;
	int mGoupNum
	{
		set
		{
			if(value < 0)
				return;
			List<CurrentBagData> MyGiftList = new List<CurrentBagData> ();
			int iLogicID = -1;
			int iNum = -1;
			foreach(ItemSlotData Data in mMyGift[mClothType])//礼包中衣物//
			{
				CurrentBagData TempBagData = new CurrentBagData();
				if(iLogicID != Data.MItemData.BasicData.LogicID &&! IsHaveThisEquip(Data))//第一个，而且身上没有这种装备//
				{
					iNum = Data.MItemData.BasicData.Count -1;
					iLogicID = Data.MItemData.BasicData.LogicID;
					TempBagData.bNeedReduce = true;
				}
				else
				{
					iNum = Data.MItemData.BasicData.Count;
					TempBagData.bNeedReduce = false;
				}
				if(iNum <= 0)
					continue;
				TempBagData.itemSlotData = Data;
				MyGiftList.Add(TempBagData);
			}
			NGUITools.SetActive(CLFLabel.gameObject,MyGiftList.Count == 0);
		
			int LastGroupNum = MyGiftList.Count%mMaxGouNum;
			int Max = 0;
			if(MyGiftList.Count > mMaxGouNum)
			{
				Max = MyGiftList.Count/mMaxGouNum - 1;
				if(LastGroupNum > 10)//最后一组如果少于10个添加到上一组//
					Max ++;
			}
	
			if(value > Max)
				return;
			int TempFrom = value;
			TempFrom *= mMaxGouNum;
			if(TempFrom > MyGiftList.Count - 1)
				return;
			
			
			int TempTo = value + 1;
			if(value == Max && LastGroupNum <= 10)//如果最后一组少于10，则从当前显示完//
			{
				TempTo = MyGiftList.Count -1;
			}
			else
			{
				TempTo *= mMaxGouNum;
			}
			iLogicID = -1;
			iNum = -1;
			HelpUtil.DelListInfo(CLFTable.transform);
			for(int i = TempFrom; i <= TempTo && i <= MyGiftList.Count -1 ;++i )
			{
				ItemSlotData Data = MyGiftList[i].itemSlotData;
				CurrentBagData TempBagData = MyGiftList[i];

				if(true == TempBagData.bNeedReduce)
					iNum = Data.MItemData.BasicData.Count - 1;
				else
					iNum = Data.MItemData.BasicData.Count;
						
				if(iNum <= 0)
					continue;
				
				GameObject Item = GameObject.Instantiate(CLFItem) as GameObject;
				Item.name = i.ToString();
				UIToggle Toggle = Item.GetComponent<UIToggle>();
				TempBagData.itemSlotData = Data;	
				Toggle.Data = TempBagData;
				Item.transform.parent = CLFTable.transform;
				Item.transform.localScale = Vector3.one;
				Item.transform.localPosition = new Vector3(TempTo - i,TempTo - i,0);
				
				UITexture ClothTexture = Item.GetComponent<UITexture>();
				string texturePath = "Icon/ItemIcon/" + Data. MItemData.BasicData.Icon;
				ClothTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
				UILabel NumLabel = Item.transform.Find("NumLabel").GetComponent<UILabel>();
				NumLabel.text = iNum .ToString();		
				UIEventListener.Get(Toggle.gameObject).onClick += delegate(GameObject Obj)
				{
					if(UITweener.current != null)
						return;
					SetTweenActive(ClothInfoMationFrame,true,null);
					UIToggle InToggle = Obj.GetComponent<UIToggle>();
					mCurrentBagData = (CurrentBagData)InToggle.Data;
					ItemSlotData InData = mCurrentBagData.itemSlotData;
					
					long itemLogicID = InData.MItemData.BasicData.LogicID;
					ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
					ItemConfig.ItemElement element = null;
					bool IsHas = config.GetItemElement(itemLogicID, out element);
								
					CIMFClothName.text = Globals.Instance.MDataTableManager.GetWordText(element.NameID);
//					CIMFActNum.text = element.RoleArt.ToString();
	                CIMFBuyMoneyNum.text = element.SellPrice.ToString();
					CIMFCharmNum.text = element.Girl_Charm.ToString();
					string IntexturePath = "Icon/ItemIcon/" + InData. MItemData.BasicData.Icon;
					CIMFClothIcon.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;			
				};
			}
			CLFTable.repositionNow = true;
			CLFTable.Reposition();
			CLFScrollView.ResetPosition();
			_goupNum = value;
		}
		get{return _goupNum;}
	}
	int _goupNum;
	public UIScrollView CLFScrollView;
	public UITable CLFTable ;
	public GameObject ClothingListFrame;
	public GameObject CLFItem;
	public GameObject CLFMove;
	public UILabel CLFLabel;
	public enum ClothType
	{
		GIRL = 0,//女装//
		BOY = 1,//男装//	
		OTHER = 2,//其他//
	}
	ClothType mClothType;
	//CIMF
	public GameObject ClothInfoMationFrame;
	public UILabel CIMFClothName;
	public UILabel CIMFActNum;
	public UILabel CIMFBuyMoneyNum;
	public UILabel CIMFCharmNum;
	public UITexture CIMFClothIcon;
	public UIButton GiftClothBtn;
	public UIButton SailBtn;
	
	Dictionary<ClothType,List<ItemSlotData>> mMyGift = new Dictionary<ClothType, List<ItemSlotData>>();
	//-------------------------------------------------
	protected override void Awake()
	{		
		if(!Application.isPlaying || null == Globals.Instance.MGUIManager) return;
	
		base.Awake();
		base.enabled = true;
	
		UIEventListener.Get(ShopBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			Globals.Instance.MGUIManager.CreateWindow<GUIClothShop>(delegate(GUIClothShop ClothShop)
			{
				UISprite exitBtn = ClothShop.HomeBtn.gameObject.GetComponent<UISprite>();
//				exitBtn.spriteName = "ButtonFanhui3Normol";
//				ClothShop.HomeBtn.normalSprite = "ButtonFanhui3Normol";
//				ClothShop.HomeBtn.hoverSprite = "ButtonFanhui3Normol";
//				ClothShop.HomeBtn.pressedSprite = "ButtonFanhui3Normol";
//				ClothShop.HomeBtn.disabledSprite = "ButtonFanhui3Normol";
				Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);
				NetSender.Instance.C2GSRequestShopItems(Globals.Instance.mShopDataManager.ShopID,(int)Globals.Instance.MGameDataManager.MActorData.BasicData.Gender);
				ResetAllTweener();
				NGUITools.SetActive(this.gameObject,false);
				ClothShop.CloseShopEvent += delegate()
				{
					if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus)
					{
						GUIMain main = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
						if(main != null){
							main.SetVisible(false);	
						}
					}
					Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.TASKCAMERA);
					UpdataList();
					NGUITools.SetActive(this.gameObject,true);
					SetTweenActive(CMFMove,true,null);
				};
			});
		};
		UIEventListener.Get(BackToClothBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			GUIRadarScan.Show();
			Globals.Instance.MGUIManager.CreateWindow(delegate(GUIChangeCloth changeCloth)
			{	
				GUIRadarScan.Hide();
				this.IsReturnMainScene = false;
				this.Close();
			});
		};
		UIEventListener.Get(BoyBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			SetTweenActive(CMFMove,false,null);
			DisplayClothList(ClothType.BOY);
		};
		UIEventListener.Get(GirlBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			SetTweenActive(CMFMove,false,null);
			DisplayClothList(ClothType.GIRL);
		};
		UIEventListener.Get(OtherBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			SetTweenActive(CMFMove,false,null);
			DisplayClothList(ClothType.OTHER);
		};
		UIEventListener.Get(BackBtn.gameObject).onClick += OnClickBackBtn;
		UIEventListener.Get(BackBtn1.gameObject).onClick += OnClickBackBtn;
		UIEventListener.Get(ExitBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			this.Close();
		};
		UIEventListener.Get(GiftClothBtn.gameObject).onClick += delegate(GameObject Obj)//赠送//
		{
			NGUITools.SetActive(BackToClothBtn.gameObject,false);
			NGUITools.SetActive(ShopBtn.gameObject,false);
			NGUITools.SetActive(ExitBtn.gameObject,false);
			NGUITools.SetActive(WealthGroup,false);
			NGUITools.SetActive(BackBtn.gameObject,false);
		
			mCurrentStatus = Status.SEND;//注意这句代码的位置//
			if(mCurrentBagData.bNeedReduce == true)
				MaxSailNum = mCurrentBagData.itemSlotData.MItemData.BasicData.Count - 1;
			else
				MaxSailNum = mCurrentBagData.itemSlotData.MItemData.BasicData.Count;
			SailNum = 1;
			SetTweenActive(ClothInfoMationFrame,false,null);
			SetTweenActive(CLFMove,false,null);
	
			SetTweenActive(FFMoveScrollView,true,null);
		};
		UIEventListener.Get(SailBtn.gameObject).onClick += delegate(GameObject Obj)//卖//
		{
			mCurrentStatus = Status.SELL;//注意这句代码的位置//
			if(mCurrentBagData.bNeedReduce == true)
				MaxSailNum = mCurrentBagData.itemSlotData.MItemData.BasicData.Count - 1;
			else
				MaxSailNum = mCurrentBagData.itemSlotData.MItemData.BasicData.Count;
			SailNum = 1;
			SetTweenActive(ClothInfoMationFrame,false,null);
			NGUITools.SetActive(ForbitBtn.gameObject,true);
			SetTweenActive(NoteFrame,true,delegate()
			{
				CancelBtn.isEnabled = true;
				EnterBtn.isEnabled = true;
			});
			SetTweenActive(CLFMove,false,null);
		};
		UIEventListener.Get(AddBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			SailNum ++;
		};
		UIEventListener.Get(SubtractBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			SailNum--;
		};
		UIEventListener.Get(EnterBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			EnterBtn.isEnabled = false;
			CancelBtn.isEnabled = false;
			mCurrentBagData.itemSlotData.MItemData.BasicData.Count = SailNum;
			if(mCurrentStatus == Status.SELL)
			{
				NetSender.Instance.RequestSellItem(mCurrentBagData.itemSlotData);
			}
			else if(mCurrentStatus == Status.SEND)
			{

			}
			SetTweenActive(NoteFrame,false,delegate(){
				NGUITools.SetActive(ForbitBtn.gameObject,false);
			});
			NGUITools.SetActive(WealthGroup,true);
			//SetTweenActive(CMFMove,true);
		};
		UIEventListener.Get(CancelBtn.gameObject).onClick += delegate(GameObject Obj)
		{
			CancelBtn.isEnabled = false;
			EnterBtn.isEnabled = false;
			SetTweenActive(NoteFrame,false,delegate()
			{
				NGUITools.SetActive(ForbitBtn.gameObject,false);
				NGUITools.SetActive(ShopBtn.gameObject,true);
				NGUITools.SetActive(ExitBtn.gameObject,true);
				NGUITools.SetActive(WealthGroup,true);
			});
			SetTweenActive(CLFMove,true);
			SetTweenActive(ClothInfoMationFrame.gameObject,true);
			
			SetTweenActive(NoteFrame,false,delegate(){
				NGUITools.SetActive(ForbitBtn.gameObject,false);
			});
		};
		CLFScrollView.onDragFinished = OnScrollViewFinished;
		SetTweenActive(CMFMove,true,null);
		
	}
	
	protected override void OnDestroy()
	{	
		base.OnDestroy();
		if (this.IsReturnMainScene) {
			if (Globals.Instance.MSceneManager != null) {
				Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);
			}
		}
			
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus )
		{
			PortStatus port = ((PortStatus)GameStatusManager.Instance.MCurrentGameStatus);
			PlayerData playerData =  Globals.Instance.MGameDataManager.MActorData;	
			port.characterCustomizeOne.ResetCharacter();
			port.characterCustomizeOne.generageCharacterFormPlayerData(playerData);
		}
	}
	
	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		
		this.GUILevel = 10;	
		UpdataList();
		PlayerData playerData =  Globals.Instance.MGameDataManager.MActorData;		
		string texturePath = "";
		if(playerData.BasicData.Gender == PlayerGender.GENDER_MALE)
			texturePath = "UIAtlas/BG-Nanshengsushe";
		else if(playerData.BasicData.Gender == PlayerGender.GENDER_FEMALE)
			texturePath = "UIAtlas/BG-Nvshengsushe";
		BackGroundTexture .mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
	}

	/// <summary>
	/// tzz modified to prevent this GUI Destroy
	/// Raises the close event.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	protected override void OnClose( GameObject obj)
	{
		base.OnClose(obj);	
	}
	private void OnClickExit(GameObject obj)
	{
		this.Close();
	}
	void OnClickBackBtn(GameObject Obj)
	{
		Debug.Log("UITweener.current = " + (UITweener.current != null).ToString());
		if(UITweener.current != null)
			return ;
		SetTweenActive(ClothInfoMationFrame,false,null);
		SetTweenActive(CLFMove,false,null);
		SetTweenActive(CMFMove,true,null);
		SetTweenActive(FFMoveScrollView,false,delegate()
		{
			NGUITools.SetActive(BackToClothBtn.gameObject,true);
			NGUITools.SetActive(ShopBtn.gameObject,true);
			NGUITools.SetActive(ExitBtn.gameObject,true);
			NGUITools.SetActive(WealthGroup,true);
			NGUITools.SetActive(BackBtn.gameObject,true);
		});
	}
	public void UpdataList()
	{
		mMyGift.Clear();
		List<ItemSlotData> dataList = ItemDataManager.Instance.GetItemDataList(ItemSlotType.CLOTH_BAG);
		dataList.Sort(MySort);
//		dataList.Sort(delegate(ItemSlotData ItemSlotData0,ItemSlotData ItemSlotDat1)
//		{
//			if(ItemSlotData0.MItemData == null || ItemSlotDat1.MItemData == null)
//				return 0;
//			if(ItemSlotData0.MItemData.BasicData.SuitSex != ItemSlotDat1.MItemData.BasicData.SuitSex)//性别排序//
//				return ItemSlotData0.MItemData.BasicData.SuitSex > ItemSlotDat1.MItemData.BasicData.SuitSex ? 1 : -1;
//			else
//			{
//				if(ItemSlotData0.MItemData.BasicData.LogicID != ItemSlotDat1.MItemData.BasicData.LogicID)//LogicID排序//
//				{
//					return ItemSlotData0.MItemData.BasicData.LogicID > ItemSlotDat1.MItemData.BasicData.LogicID? 1 :-1;
//				}
//				else
//				{
//					return  ItemSlotData0.MItemData.BasicData.Count > ItemSlotDat1.MItemData.BasicData.Count? -1 ://数量排序//
//						(ItemSlotData0.MItemData.BasicData.Count == ItemSlotDat1.MItemData.BasicData.Count ? 0 : 1);
//				}
//			}
//			return 0;
//		});
 		foreach (var item in dataList)
		{
			if(null != item.MItemData)
			{

				ClothType temp = (ClothType) item.MItemData.BasicData.SuitSex;
				if(item.MItemData.BasicData.Count <= 0)
					continue;
				
				//Debug.Log("Name = " + item.MItemData.BasicData.Name + "Num = " + item.MItemData.BasicData.Count.ToString() );
				if(mMyGift.ContainsKey(temp))
				{
					mMyGift[temp].Add(item);
				}
				else
				{
					List<ItemSlotData> TempList = new List<ItemSlotData>();
					TempList.Add(item);
					mMyGift.Add(temp,TempList);
				}
			}
		}
		for(int i = 0; i <= (int)ClothType.OTHER;++i)
		{
			if(mMyGift.ContainsKey((ClothType)i))
				mMyGift[(ClothType)i].Sort(MySort);
		}
		
	}
	int MySort(ItemSlotData ItemSlotData0,ItemSlotData ItemSlotDat1) 
	{
			if(ItemSlotData0.MItemData == null || ItemSlotDat1.MItemData == null)
				return 0;
			if(ItemSlotData0.MItemData.BasicData.SuitSex != ItemSlotDat1.MItemData.BasicData.SuitSex)//性别排序//
				return ItemSlotData0.MItemData.BasicData.SuitSex > ItemSlotDat1.MItemData.BasicData.SuitSex ? 1 : -1;
			else
			{
				if(ItemSlotData0.MItemData.BasicData.LogicID != ItemSlotDat1.MItemData.BasicData.LogicID)//LogicID排序//
				{
					return ItemSlotData0.MItemData.BasicData.LogicID > ItemSlotDat1.MItemData.BasicData.LogicID? -1 :1;
				}
				else
				{
					return  ItemSlotData0.MItemData.BasicData.Count > ItemSlotDat1.MItemData.BasicData.Count? 1 ://数量排序//
						(ItemSlotData0.MItemData.BasicData.Count == ItemSlotDat1.MItemData.BasicData.Count ? 0 : -1);
				}
			}
			return 0;
	}

	bool IsHaveThisEquip(ItemSlotData data)
	{
		PlayerData playerData =  Globals.Instance.MGameDataManager.MActorData;	
		int type = data.MItemData.BasicData.JuniorType;
		if(!playerData.ClothDatas.ContainsKey(type))
			return false;
		return playerData.ClothDatas[type].MItemData.BasicData.LogicID == data.MItemData.BasicData.LogicID;
	}
	public void DisplayClothList(ClothType Type,int goupNum = 0)
	{	
		mClothType = Type;
		_goupNum = 0;
		SetTweenActive(CLFMove,true,null);
		SetTweenActive(CMFMove,false,null);
		HelpUtil.DelListInfo(CLFTable.transform);
		if(!mMyGift.ContainsKey(Type))
		{
			NGUITools.SetActive(CLFLabel.gameObject,true);	
			return;
		}
		NGUITools.SetActive(CLFLabel.gameObject,false);	
		int i = 0;
		int iLogicID = -1;
		int iNum = -1;
		//int Flag = 0;
		List<ItemSlotData> MyGiftList = mMyGift[Type];
		mGoupNum = goupNum;
	}
	void SetTweenActive(GameObject go, bool state,iTween.EventDelegate onActiveFinished = null)
	{
		
		UITweener [] TempTweener = go.GetComponents<UITweener>();
		UITweener  Tweener0 = null;
		UITweener  Tweener1 = null;
		foreach(UITweener Data in TempTweener)
		{
			if(0 == Data.tweenGroup)
				Tweener0 = Data;
			if(1 == Data.tweenGroup)
				Tweener1 = Data;
		}
		if(true == state)
		{
			float Factor = Tweener0.tweenFactor;
			if(Factor == 0)
			{	
				NGUITools.SetActive(ForbitBtn1.gameObject,true);
				Tweener0.Toggle ();
				EventDelegate.Add( Tweener0.onFinished ,delegate{
					Tweener1.tweenFactor = 0;
					NGUITools.SetActive(ForbitBtn1.gameObject,false);
					if (onActiveFinished != null)
						onActiveFinished();
				},true);
			}
		}
		else
		{
			float Factor = Tweener1.tweenFactor;
			if(Factor == 0 && Tweener0.tweenFactor == 1)
			{
				NGUITools.SetActive(ForbitBtn1.gameObject,true);
				Tweener1.Toggle ();
				EventDelegate.Add( Tweener1.onFinished ,delegate{
					Tweener0.tweenFactor = 0;
					NGUITools.SetActive(ForbitBtn1.gameObject,false);
					if (onActiveFinished != null)
						onActiveFinished();
				},true);
			}
		}
	}
	public void ReceiveSellItem()
	{
		UpdataList();
		SetTweenActive(CMFMove,false,null);
		SetTweenActive(CLFMove,true);
		List<ItemSlotData> MyGiftList = mMyGift[mClothType];
		int iLogicID = -1;
		int iNum = -1;
		int iCount = 0;
		foreach(ItemSlotData Data in MyGiftList)//礼包中衣物//
		{
			if(iLogicID != Data.MItemData.BasicData.LogicID &&! IsHaveThisEquip(Data))//第一个，而且身上没有这种装备//
			{
				iNum = Data.MItemData.BasicData.Count -1;
				iLogicID = Data.MItemData.BasicData.LogicID;
			}
			else
			{
				iNum = Data.MItemData.BasicData.Count;
				
			}
			if(iNum <= 0)
				continue;
			iCount++;
		}
		int Max = 0;
		int LastGroupNum = iCount%mMaxGouNum;
		if(iCount > mMaxGouNum)
		{
			Max = iCount/mMaxGouNum - 1;
			if(LastGroupNum > 10)//最后一组如果少于10个添加到上一组//
				Max ++;
		}
		if(_goupNum > Max)
			_goupNum = Max;
		DisplayClothList(mClothType,_goupNum);
	}
	public void ReceiveGiveGiftRes()
	{
		UpdataList();
		SetTweenActive(CMFMove,false,null);
		SetTweenActive(CLFMove,true);
		List<ItemSlotData> MyGiftList = mMyGift[mClothType];
		int iLogicID = -1;
		int iNum = -1;
		int iCount = 0;
		foreach(ItemSlotData Data in MyGiftList)//礼包中衣物//
		{
			if(iLogicID != Data.MItemData.BasicData.LogicID &&! IsHaveThisEquip(Data))//第一个，而且身上没有这种装备//
			{
				iNum = Data.MItemData.BasicData.Count -1;
				iLogicID = Data.MItemData.BasicData.LogicID;
			}
			else
			{
				iNum = Data.MItemData.BasicData.Count;
				
			}
			if(iNum <= 0)
				continue;
			iCount++;
		}
		int Max = 0;
		int LastGroupNum = iCount%mMaxGouNum;
		if(iCount > mMaxGouNum)
		{
			Max = iCount/mMaxGouNum - 1;
			if(LastGroupNum > 10)//最后一组如果少于10个添加到上一组//
				Max ++;
		}
		if(_goupNum > Max)
			_goupNum = Max;
		DisplayClothList(mClothType,_goupNum);

	}

	void ResetTweener(GameObject Obj)
	{
		UITweener [] TempTweener = Obj.GetComponents<UITweener>();
		foreach(UITweener data in TempTweener)
		{
			if(data.tweenGroup == 0)
			{
				data.ResetToBeginning();
			}
			data.tweenFactor = 0;	
		}
	}
	void ResetAllTweener()
	{
		ResetTweener(FFMoveScrollView.gameObject);
		ResetTweener(NoteFrame.gameObject);
		ResetTweener(CMFMove.gameObject);
		ResetTweener(CLFMove.gameObject);
		ResetTweener(ClothInfoMationFrame.gameObject);
	}
	void OnScrollViewFinished()
	{
		if(CLFScrollView.ShouldMoveEndDown)
		{
			mGoupNum++;
			CLFScrollView.SetShouldMoveEnd();
		}
		else if(CLFScrollView.ShouldMoveEndUp)
		{
			mGoupNum--;
			CLFScrollView.SetShouldMoveEnd();
		}
	}
}
