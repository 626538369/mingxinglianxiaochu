using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUISingleDrop : GUIWindow
{	
	
	public GameObject DropItemPrefab ;
	public GameObject DropItemIconPrefab;
	public GameObject DropItemCommonIconPrefab;
	
	public static  GameObject mTalkTargetObj;
	public static UIButton mTalkButton;
	
	public static List<int> mWordConfigList = new List<int>();
	public static List<string> mWordNameList = new List<string>();
	public static int mWordConfigIter = 0;
	public static bool mShowWordList = false;
	
	float mDelay = 2;
	float mCurTime = 0;
	bool mFinishAlpha = false;
	int	  mShowNumMax = 4;
	int   mShowNum = 0;
	int   mShowBookNum = 0;
	ItemConfig mItemConfig;
	ItemConfig.ItemElement mElement = null;
	
	public const  string ShopGreenColor = "148,73,255,255";
	public const  string ShopBrownColor = "58,73,255,255";
	public const  string FavoratPinkColor = "343,73,255,255";
	public const  string JobPurpleColor = "256,73,255,255";

	public delegate void OnDropFinishedEvent(GameObject gameObj);
	[HideInInspector] public  event GUISingleDrop.OnDropFinishedEvent DropFinishedEvents = null;
	
	public  delegate  void  OnTalkFinishedEvent();
	[HideInInspector]  static public   event  GUISingleDrop.OnTalkFinishedEvent  TalkFinishedEvents = null;
	
	public GameObject ObjectSubjectFinish;
	public UILabel UILableSubject;
	public UITexture TextureSubject;
	public static List<DropBooks> DropBooksList = new List<DropBooks>();
	public UIButton BookFinish;
	
	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		
		this.gameObject.transform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		this.gameObject.transform.localPosition = new Vector3(0, 0, 0);
		base.GUILevel = 2;
		
		mItemConfig = Globals.Instance.MDataTableManager.GetConfig<ItemConfig>();
		
		NGUITools.SetActive(ObjectSubjectFinish.gameObject,false);
		UIEventListener.Get(BookFinish.gameObject).onClick = OnClickBookFinish;
	}
		
	void Update()
	{
		if (dropItemList.Count>0)
		{
			mCurTime += Time.deltaTime;
			if (mCurTime >= mDelay)	
			{
				Debug.Log("playDropAnimationOne mShowNum is :" + mShowNum );
				playDropAnimationOne();
				mCurTime = 0;
			}
		}
		
		if(DropBooksList.Count > 0)
		{	
			ShowBookInformation();
		}
	}
	
	private void ShowBookInformation()
	{
		if(DropBooksList.Count <= 0)
		{
			return;
		}
		
		NGUITools.SetActive(ObjectSubjectFinish.gameObject,true);
		DropBooks dropbook = DropBooksList[0];
		DropBooksList.RemoveAt(0);

		bool IsHas = mItemConfig.GetItemElement(dropbook.BookID, out mElement);
		if (!IsHas){
				return;
		}
		
		string texturePath = "Icon/ItemIcon/" + mElement.Icon;
		TextureSubject.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
		string itemName = "[00ff00]"+Globals.Instance.MDataTableManager.GetWordText(mElement.NameID)+ "[-]";
		UILableSubject.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(4013),itemName);	
	}
	
	private void OnClickBookFinish(GameObject obj)
	{
		mShowBookNum = 0;
		if (mShowNum == 0)
		{
			if (DropFinishedEvents != null)
			{
				DropFinishedEvents(gameObject);
				DropFinishedEvents = null;
				GUISingleDrop guiSingleDrop = Globals.Instance.MGUIManager.GetGUIWindow<GUISingleDrop>();
				if (guiSingleDrop != null)
				{
					guiSingleDrop.Close();
				}
			}
			Globals.Instance.MTeachManager.NewGUISingleDropFinishedEvent();
		}
	}
	
	private void  playDropAnimationOne()
	{
		if (dropItemList.Count <= 0)
			return;
		
		DropItem dropItem = dropItemList[0];
		dropItemList.RemoveAt(0);
		if (dropItem.type == GUISingleDrop.ItemType.ITEM_ICON)
		{
			GameObject bodyPartObj = GameObject.Instantiate(DropItemIconPrefab) as GameObject;
			bodyPartObj.name = "DropItemPrefab";
			UILabel dropText = bodyPartObj.transform.Find("UILableGot").GetComponent<UILabel>();
			UILabel dropNumText = bodyPartObj.transform.Find("UILableGotNum").GetComponent<UILabel>();
			UITexture dropTexture = bodyPartObj.transform.Find("Texture").GetComponent<UITexture>();
			UITexture backgrond = bodyPartObj.transform.Find("SpriteBG").GetComponent<UITexture>();
			int dorpItem = (int)dropItem.obj;
			
			bool IsHas = mItemConfig.GetItemElement(dorpItem, out mElement);
			if (!IsHas){
					return;
			}
			
			dropText.text = GetPrName(TextItemName.ITEM) + " " + Globals.Instance.MDataTableManager.GetWordText(mElement.NameID);
			dropNumText.text =  GUIFontColor.DarkBlue + " + " +  dropItem.num.ToString();
			string texturePath = "Icon/ItemIcon/" + mElement.Icon;
			dropTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
			bodyPartObj.transform.parent = gameObject.transform;
			bodyPartObj.transform.localPosition = new Vector3(0,-600f,-3f);
			bodyPartObj.transform.localScale = Vector3.one;
			TweenGroup tweenGroup = bodyPartObj.GetComponent<TweenGroup>();
			tweenGroup.playTweenAnimation();
			tweenGroup.TweenFinishedEvents += OnTweenGroupFinishedEvent;
			backgrond.material.color = StrParser.ParseColor(dropItem.BGSpriteColor);
		}
		else if (dropItem.type == GUISingleDrop.ItemType.MONEY_ICON)
		{
			GameObject bodyPartObj = GameObject.Instantiate(DropItemCommonIconPrefab) as GameObject;
			bodyPartObj.name = "DropItemPrefab";
			UILabel dropText = bodyPartObj.transform.Find("UILableContent").GetComponent<UILabel>();
			UILabel dropCountText = bodyPartObj.transform.Find("UILableCount").GetComponent<UILabel>();
			UISprite dropTexture = bodyPartObj.transform.Find("Texture").GetComponent<UISprite>();
			UITexture backgrond = bodyPartObj.transform.Find("SpriteBG").GetComponent<UITexture>();
			dropText.text = (string)dropItem.obj;
			if(-1 != dropItem.CharacterSize)
				dropText.transform.localScale = new Vector3(dropItem.CharacterSize,dropItem.CharacterSize,dropText.transform.localScale.z) ;
			if(-1 != dropItem.MaxLeft)
				dropText.transform.localPosition = new Vector3(dropText.transform.localPosition.x + dropItem.MaxLeft,dropText.transform.localPosition.y,dropText.transform.localPosition.z) ;		
			dropTexture.spriteName = dropItem.spritename;
			dropCountText.text = GUIFontColor.DarkBlue + " + " + dropItem.num.ToString();
			bodyPartObj.transform.parent = gameObject.transform;
			bodyPartObj.transform.localPosition = new Vector3(0,-600f,-3f);
			bodyPartObj.transform.localScale = Vector3.one;
			TweenGroup tweenGroup = bodyPartObj.GetComponent<TweenGroup>();
			tweenGroup.playTweenAnimation();
			tweenGroup.TweenFinishedEvents += OnTweenGroupFinishedEvent;
			backgrond.material.color = StrParser.ParseColor(dropItem.BGSpriteColor);
		}
		else if (dropItem.type == GUISingleDrop.ItemType.SELF_TEXTURE)
		{
			GameObject bodyPartObj = GameObject.Instantiate(DropItemIconPrefab) as GameObject;
			bodyPartObj.name = "DropItemPrefab";
			UILabel dropText = bodyPartObj.transform.Find("UILableGot").GetComponent<UILabel>();
			UILabel dropNumText = bodyPartObj.transform.Find("UILableGotNum").GetComponent<UILabel>();
			UITexture dropTexture = bodyPartObj.transform.Find("Texture").GetComponent<UITexture>();
			UITexture backgrond = bodyPartObj.transform.Find("SpriteBG").GetComponent<UITexture>();
			dropText.text  = (string)dropItem.obj;
			
			dropNumText.text =  GUIFontColor.DarkBlue + " + " + dropItem.num.ToString();
						
			dropTexture.mainTexture = Resources.Load(dropItem.spritename,typeof(Texture2D)) as Texture2D;
			bodyPartObj.transform.parent = gameObject.transform;
			bodyPartObj.transform.localPosition = new Vector3(0,-600f,-3f);
			bodyPartObj.transform.localScale = Vector3.one;
			TweenGroup tweenGroup = bodyPartObj.GetComponent<TweenGroup>();
			tweenGroup.playTweenAnimation();
			tweenGroup.TweenFinishedEvents += OnTweenGroupFinishedEvent;
			backgrond.material.color = StrParser.ParseColor(dropItem.BGSpriteColor);
		}
		else
		{
			GameObject bodyPartObj = GameObject.Instantiate(DropItemPrefab) as GameObject;
			bodyPartObj.name = "DropItemPrefab";
			UILabel dropText = bodyPartObj.transform.Find("UILable").GetComponent<UILabel>();
			UITexture backgrond = bodyPartObj.transform.Find("SpriteBG").GetComponent<UITexture>();
			dropText.text = (string)dropItem.obj;
			bodyPartObj.transform.parent = gameObject.transform;
			bodyPartObj.transform.localPosition = new Vector3(0,-600f,-3f);
			bodyPartObj.transform.localScale = Vector3.one;
			TweenGroup tweenGroup = bodyPartObj.GetComponent<TweenGroup>();
			tweenGroup.playTweenAnimation();
			tweenGroup.TweenFinishedEvents += OnTweenGroupFinishedEvent;
			backgrond.material.color = StrParser.ParseColor(dropItem.BGSpriteColor);
		}
	}
	
	private void OnTweenGroupFinishedEvent(GameObject tweenGameObj,bool isAutoJump)
	{
		TweenGroup tweenGroup = tweenGameObj.GetComponent<TweenGroup>();
		tweenGroup.TweenFinishedEvents -= OnTweenGroupFinishedEvent;
		mShowNum --;
		Debug.Log("OnTweenGroupFinishedEvent mShowNum is :" + mShowNum );
		if (mShowNum == 0&&mShowBookNum == 0)
		{
			Debug.Log("OnTweenGroupFinishedEvent   mShowNum == 0 mCurTime is :" + mCurTime );
			if (DropFinishedEvents != null)
			{
				DropFinishedEvents(gameObject);
				DropFinishedEvents = null;
				GUISingleDrop guiSingleDrop = Globals.Instance.MGUIManager.GetGUIWindow<GUISingleDrop>();
				if (guiSingleDrop != null)
				{
					guiSingleDrop.Close();
				}
			}
			Globals.Instance.MTeachManager.NewGUISingleDropFinishedEvent();
		}
		GameObject.DestroyObject(tweenGameObj);
	}
	
	public static List<DropItem> dropItemList = new List<DropItem>();
	public void UpdateData()
	{
		for(int i=0;i<dropItemList.Count;i++)
		{
			DropItem dropItem = dropItemList[i];
			if (dropItem.type == GUISingleDrop.ItemType.ITEM_ICON)
			{
				bool IsHas = mItemConfig.GetItemElement((int)dropItem.obj, out mElement);
				if (!IsHas){
					continue;
				}
				if(mElement.ItemCategory == 16)
				{
					GUISingleDrop.DropBooks tDropBook = new GUISingleDrop.DropBooks();
					tDropBook.BookID = mElement.LogicID;
					GUISingleDrop.DropBooksList.Add(tDropBook);
					dropItemList.RemoveAt(i);
				}
			}				
		}
		
		mShowNum = dropItemList.Count;
		mShowBookNum = DropBooksList.Count;
		Debug.Log("UpdateData mShowNum is :" + mShowNum );
		
		if (mShowNum == 0&&mShowBookNum == 0)
		{
			this.Close();
			return;
		}
		
		
		///当mCurTime >0 表明有正在播放着的drop这里就不在默认播一个了，否则就不return 然后播一个///
		if (mCurTime > 0)
		{
			Debug.Log("UpdateData  mShowNum++ mShowNum is :" + mShowNum );
			Debug.Log("UpdateData  mShowNum++ mCurTime is :" + mCurTime );
			return;
		}
		///让它等于 mDelay 则update立即开始执行 playDropAnimationOne////
		mCurTime = mDelay - 0.2f;
		//playDropAnimationOne();	
		
		

	}
	
	public static void ShowTalkObj(int wordID,string name = "")
	{	
		Transform targetTran = Globals.Instance.M3DItemManager.EZ3DItemParent;
		if(mTalkTargetObj ==  null)
		{
			mTalkTargetObj = (GameObject)Instantiate(Globals.Instance.MTeachManager.mTalkObjectPrefab) as GameObject;
			mTalkTargetObj.name = "GUISingleDrop:TalkObject";
			mTalkTargetObj.transform.parent = targetTran;
			mTalkTargetObj.transform.localPosition =  new Vector3(0,-558,-480);
			mTalkTargetObj.transform.localScale = Vector3.one;
			UIButton dialogBtn = mTalkTargetObj.transform.Find("TextureBG").GetComponent<UIButton>();

			UIEventListener.Get(dialogBtn.gameObject).onClick += OnTalkBtnClick;
		}
		NGUITools.SetActive(mTalkTargetObj,true);
		UILabel uiLabel = mTalkTargetObj.transform.Find("UILable").GetComponent<UILabel>();
		UILabel uiLabelName  = mTalkTargetObj.transform.Find("UILableName").GetComponent<UILabel>();
		uiLabel.text = Globals.Instance.MDataTableManager.GetWordText(wordID);
		uiLabelName.text = name;
		
	
	}
	
	public static void ShowTalkObj(string  wordstr,string name = "")
	{
		Transform targetTran = Globals.Instance.M3DItemManager.EZ3DItemParent;
		if(mTalkTargetObj ==  null)
		{
			mTalkTargetObj = (GameObject)Instantiate(Globals.Instance.MTeachManager.mTalkObjectPrefab) as GameObject;
			mTalkTargetObj.name = "GUISingleDrop:TalkObject";
			mTalkTargetObj.transform.parent = targetTran;
			mTalkTargetObj.transform.localPosition =  new Vector3(0,-558,-480);
			mTalkTargetObj.transform.localScale = Vector3.one;
			UIButton dialogBtn = mTalkTargetObj.transform.Find("TextureBG").GetComponent<UIButton>();

			UIEventListener.Get(dialogBtn.gameObject).onClick += OnTalkBtnClick;
		}
		NGUITools.SetActive(mTalkTargetObj,true);
		UILabel uiLabel = mTalkTargetObj.transform.Find("UILable").GetComponent<UILabel>();
		UILabel uiLabelName  = mTalkTargetObj.transform.Find("UILableName").GetComponent<UILabel>();
		uiLabel.text = wordstr;
		uiLabelName.text = name;
		
	}
	
	public static void ShowTalkObj(List<int> wordIDList ,List<string> nameList,string talkBGstr = "")
	{
		if (wordIDList.Count == 0)
			return;
		Transform targetTran = Globals.Instance.M3DItemManager.EZ3DItemParent;
		if(mTalkTargetObj ==  null)
		{
			mTalkTargetObj = (GameObject)Instantiate(Globals.Instance.MTeachManager.mTalkObjectPrefab) as GameObject;
			mTalkTargetObj.name = "GUISingleDrop:TalkObject";
			mTalkTargetObj.transform.parent = targetTran;
			mTalkTargetObj.transform.localPosition =  new Vector3(0,-558,-480);
			mTalkTargetObj.transform.localScale = Vector3.one;
			UIButton dialogBtn = mTalkTargetObj.transform.Find("TextureBG").GetComponent<UIButton>();

			UIEventListener.Get(dialogBtn.gameObject).onClick += OnTalkBtnClick;
		}
		mWordConfigList = wordIDList;
		mWordNameList = nameList;
		mWordConfigIter = 0;
		mShowWordList = true;
		NGUITools.SetActive(mTalkTargetObj,true);
		UILabel uiLabel = mTalkTargetObj.transform.Find("UILable").GetComponent<UILabel>();
		UILabel uiLabelName  = mTalkTargetObj.transform.Find("UILableName").GetComponent<UILabel>();
		uiLabel.text = Globals.Instance.MDataTableManager.GetWordText(mWordConfigList[mWordConfigIter]);
		string name = "";
		if (mWordConfigIter < mWordNameList.Count)
		{
			name = mWordNameList[mWordConfigIter];
		}				
		uiLabelName.text = name;
		UITexture talkBgTexture = mTalkTargetObj.transform.Find("TextureBG").GetComponent<UITexture>();
		NGUITools.SetActive(talkBgTexture.gameObject,false);
		if (talkBGstr != "")
		{		
			NGUITools.SetActive(talkBgTexture.gameObject,true);
			string atlasPath = "UIAtlas/" + talkBGstr;
			talkBgTexture.mainTexture = Resources.Load(atlasPath,typeof(Texture2D)) as Texture2D;
		}
	}
	
	private static void OnTalkBtnClick(GameObject obj)
	{
		if (mShowWordList)
		{
			mWordConfigIter++;
			if (mWordConfigIter >= mWordConfigList.Count)
			{
				NGUITools.SetActive(mTalkTargetObj,false);
				Globals.Instance.MTeachManager.NewGUISingleDropFinishedEvent();
				
				if (TalkFinishedEvents != null)
				{
					TalkFinishedEvents();
					TalkFinishedEvents = null;
				}
				
				mShowWordList = false;
				return;
			}
			else{
				UILabel uiLabel = mTalkTargetObj.transform.Find("UILable").GetComponent<UILabel>();
				UILabel uiLabelName  = mTalkTargetObj.transform.Find("UILableName").GetComponent<UILabel>();
				uiLabel.text = Globals.Instance.MDataTableManager.GetWordText(mWordConfigList[mWordConfigIter]);
				string name = "";
				if (mWordConfigIter < mWordNameList.Count)
				{
					name = mWordNameList[mWordConfigIter];
				}				
				uiLabelName.text = name;
			}
		}
		else
		{
			NGUITools.SetActive(mTalkTargetObj,false);
			Globals.Instance.MTeachManager.NewGUISingleDropFinishedEvent();
		
			if (TalkFinishedEvents != null)
			{
				TalkFinishedEvents();
				TalkFinishedEvents = null;
				GUISingleDrop guiSingleDrop = Globals.Instance.MGUIManager.GetGUIWindow<GUISingleDrop>();
				if (guiSingleDrop != null)
				{
					guiSingleDrop.Close();
				}
			}
		}
	}
	
	public static string GetPrName(TextItemName name)
	{
		if(name == TextItemName.EXP)
		{
			return Globals.Instance.MDataTableManager.GetWordText(11008);
		}
		else if(name == TextItemName.MONEY)
		{
			return Globals.Instance.MDataTableManager.GetWordText(11007);
		}
		else if(name == TextItemName.INGOT)
		{
			return Globals.Instance.MDataTableManager.GetWordText(11018);
		}
		else if(name == TextItemName.OIL)
		{
			return Globals.Instance.MDataTableManager.GetWordText(10840013);
		}
		else if(name == TextItemName.TECHNOLOGY)
		{
			return Globals.Instance.MDataTableManager.GetWordText(20400013);
		}
		else if(name == TextItemName.FEAT)
		{
			return Globals.Instance.MDataTableManager.GetWordText(22500003);
		}
		else if(name == TextItemName.CONTRIBUTION)
		{
			return Globals.Instance.MDataTableManager.GetWordText(20400010);
		}
		
		else if(name == TextItemName.NOTHING)
		{
			return Globals.Instance.MDataTableManager.GetWordText(22700002);
		}
		else if(name == TextItemName.REPAIR)
		{
			return Globals.Instance.MDataTableManager.GetWordText(22700004);
		}
		else if(name == TextItemName.MINES)
		{
			return Globals.Instance.MDataTableManager.GetWordText(22700005);
		}
		else if (name == TextItemName.EXPLORE)
		{
			return Globals.Instance.MDataTableManager.GetWordText(11009);
		}
		else if (name == TextItemName.DATE_PLACE)
		{
			return Globals.Instance.MDataTableManager.GetWordText(11010);
		}
		else if (name == TextItemName.EXPLORE_PLACE)
		{
			return Globals.Instance.MDataTableManager.GetWordText(11011);
		}
		else if (name == TextItemName.INTIMACY)
		{
			return Globals.Instance.MDataTableManager.GetWordText(4005);
		}
		else if (name == TextItemName.DATINGGIRL)
		{
			return Globals.Instance.MDataTableManager.GetWordText(15013);
		}
		else if (name == TextItemName.ITEM)
		{
			return Globals.Instance.MDataTableManager.GetWordText(11012);
		}
		else if (name == TextItemName.STUDY_PROPERTY)
		{
			return Globals.Instance.MDataTableManager.GetWordText(9033);
		}
		return "";
	}
	
	public enum TextItemName
	{
		NOTHING,
		EXP,
		MONEY,
		INGOT,
		OIL,
		TECHNOLOGY,
		FEAT,
		CONTRIBUTION,
		REPAIR,
		MINES,
		EXPLORE,
		DATE_PLACE,
		EXPLORE_PLACE,
		INTIMACY,  ///好感度
		DATINGGIRL,
		ITEM,
		STUDY_PROPERTY,
	}
	
	public enum ItemType
	{
		ITEM_TEXT, ///纯文字 类型的///
		ITEM_ICON, // Texture 类型的///
		MONEY_ICON,//Common Sprite 类型的///
		SELF_TEXTURE,// 完全定制的Texture 类型的///
	}
	
	public class DropItem
	{
		public ItemType type;
		public object obj;
		public int num;
		public string spritename;
		public string BGSpriteColor = "171,248,255,255";
		public float MaxLeft = -1.0f;
		public float CharacterSize = -1.0f;
	}
	
	public class DropBooks
	{
		public int BookID;
	}
	

	public UIButton DropItemSrc = null;

	

}


