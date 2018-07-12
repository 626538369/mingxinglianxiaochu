using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameData.PlayerImageData;
using System.IO;

public class GUICreateRole :GUIWindowForm {
	
	public  enum BoneCustomType
	{
		TALL_CUSTOM = 0 ,
		WEEIGHT_CUSTOM = 1,
		CHEST_CUSTOM = 2,
		WAIST_CUSTOM = 3,
		HIP_CUSTOM = 4,
		EYE_CUSTOM_BIGS = 5,
		EYE_CUSTOM_HIGHL = 6,
		EYE_CUSTOM_ANGLE = 7,
		EYE_CUSTOM_BETWEEN = 8,
		EYE_CUSTOM_WIDTH = 9,
		EYEBROW_CUSTOM_HIGHL = 10,
		EYEBROW_CUSTOM_WIDTH = 11,
		NOSE_CUSTOM_POSITION = 12,
		NOSE_CUSTOM_HIGHL = 13,
		NOSE_CUSTOM_BITOUWIDTH = 14,
		NOSE_CUSTOM_WEADTH= 15,
		NOSE_CUSTOM_BITOUBIGS= 16,
		MOUSE_CUSTOM_HIGHL= 17,
		MOUSE_CUSTOM_BIGS= 18,
		MOUSE_CUSTOM_XIACHUN= 19,
		MOUSE_CUSTOM_GUQI=20,
		MOUSE_CUSTOM_SHANCHUN=21,
		MOUSE_CUSTOM_SHAPE=22,
		FACE_CUSTOM_WIDHT=23,
		FACE_CUSTOM_QUANGUPOS=24,
		FACE_CUSTOM_XIABALONG=25,
		FACE_CUSTOM_BIGS=26,
		FACE_CUSTOM_QUANGUHIGHL=27,
		FACE_CUSTOM_XIABALENGTH=28,
		EYEBROW_CUSTOM_ANGLE=29,
		EYE_CUSTOM_CORNER=30,
		CUSTOM_END = 31,
	};
	
	public UISlider [] BoneCustomSliderList;
	
	public GameObject mRoleNamed;
	public UIButton mImgBtnRand;
	
	public GameObject mSexSelZereObj;
	public GameObject mSexSelObj;
	public GameObject mCustomGeneralObj;
	public GameObject mCustomFaceDetailObj;
	public GameObject mRoleNamedObj;

	
	//输入框
	public UIInput       mInputNamed;
	public UILabel       mInputLableNamed;
	

	//逻辑变量命名
	string mStrName;
	private string[] FamilyNameList=null;
	private string[] GivenNameListFemale = null;
	private string[] GivenNameListMale = null;

	public UIButton mSexSelNextBtn;
	public UIButton mCustomGeneralPriorBtn;
	public UIButton mCustomGeneralNextBtn;
	public UIButton mCustomGeneralDetailBtn;
	public UIButton mCustomDetailCancelBtn;
	public UIButton mCustomDetailSaveBtn;
	public UIButton mNamedPriorBtn;
	public UIButton mNamedEnterBtn;
	
	public UIButton mAllRandomBtn;
	public UIButton mUpLoadAvatarBtn;
	
	public UIToggle mMaleToggle;
	public UIToggle mFemaleToggle;
	
	public UIToggle mHairCutToggle;
	public UIToggle mColorToggle;
	
	public CharacterCustomizeOne characterCustomizeOne;
	
	public GameObject FaceCutomItemPrefab;
	public UIWrapContent wrapContent;
	
	public GameObject HairCutomItemPrefab;
	public UIWrapContent HiarWrapContent;
	
	public GameObject BodyColorCutomItemPrefab;
	public UIWrapContent BodyColorWrapContent;
	
	public UITexture mTestAvatarTexture;
	
	private int mCurrentSex = 0;/// default is female
	
	private static readonly Vector3[]	Character_Camera_Postion	= 
	{
		new Vector3(0f,300f,-1500f),
		new Vector3(0.08f,330f,-562f),
	};
	
	private static readonly Vector3[]	Character_Rotation = 
	{
		new Vector3(0f,175f,0f),
		new Vector3(0f,175f,0f),
	};	
	
	private List<float> SliderPriorValue = new List<float>();
	
	private List<UIToggle> mHairCutToggleList = new List<UIToggle>();
	private List<UIToggle> mBodyToggleList = new List<UIToggle>();
	
	private PlayerImageData mPlayerImageData = new PlayerImageData();
	
	public GameObject mParticleObjDanceMyself;
	public GameObject mParticleObjChangeRole;
	public GameObject mParticleObjChangeFace;

	public GameObject ShareTexture;
	public UITexture BodyTexture;
	public UITexture HeadTexture;
	public GameObject FunctionBtn;
	public UIButton ShareBtn;
	public UIButton SaveBtn;
	public UIButton NextBtn;

	public GameObject PhotosEffect;
	public UITexture PhotoTexture;
	private Texture2D BodyTexture2D;
	private Texture2D HeadTexture2D;
	private Texture2D SharePhotosTexture;

	private GameObject mCurrentSelectHairGameObj = null;
	private GameObject mCurrentSelectColorGameObj = null;
	private GameObject mCurrentSelectFaceGameObj = null;

	// -------------------------------//
	protected override void Awake()
	{		
		if(!Application.isPlaying || null == Globals.Instance.MGUIManager) return;
	
		base.Awake();
		
		base.enabled = true;

		PhotosEffect.transform.GetComponent<AudioSource>().volume *= GameDefines.Setting_MusicVol / 100.0f;

		UIEventListener.Get(ShareBtn.gameObject).onClick += delegate(GameObject go) {

			NGUITools.SetActive(FunctionBtn , false);
			StartCoroutine( ShareOrSavePhotograph(1));
		};
		UIEventListener.Get(SaveBtn.gameObject).onClick += delegate(GameObject go) {

			NGUITools.SetActive(FunctionBtn , false);
			StartCoroutine( ShareOrSavePhotograph());
		};
		UIEventListener.Get(NextBtn.gameObject).onClick += delegate(GameObject go) {

			BodyTexture.mainTexture = null;
			HeadTexture.mainTexture = null;

//			GameObject.Destroy(BodyTexture);
//			GameObject.Destroy(HeadTexture);
			BodyTexture2D = null;
			HeadTexture2D = null;
			
			Resources.UnloadUnusedAssets();
			System.GC.Collect();

			SharePhotosTexture = null;
			Resources.UnloadUnusedAssets();

			BoxCollider boxCollider = mCustomGeneralPriorBtn.GetComponent<BoxCollider>();
			boxCollider.enabled = false;
			NGUITools.SetActive(PhotosEffect , false);
			NGUITools.SetTweenActive(mCustomGeneralObj,false,delegate {
				boxCollider.enabled = true;
				NGUITools.SetActive(ShareTexture , false);
				Vector3 destPos = Character_Camera_Postion[0];
				Globals.Instance.MSceneManager.mTaskCamera.fieldOfView = 15;
				Globals.Instance.MSceneManager.mTaskCamera.transform.localEulerAngles = Globals.Instance.MSceneManager.initTaskCameraLocalEulerAngles;
				characterCustomizeOne.transform.localEulerAngles  = Character_Rotation[0];
				iTween.MoveTo(Globals.Instance.MSceneManager.mTaskCamera.gameObject,iTween.Hash("isLocal",true,"position",destPos,"time",1.0f),null,delegate(){

					NGUITools.SetTweenActive(mRoleNamedObj,true,delegate {
					    //StartCoroutine(DoLoadAssetBundle(""));
						SetRandName();
					});
				});
			});
		};

	}
	
	// Use this for initialization
	void Start () {
		
		base.Start();
		Globals.Instance.MSceneManager.mTaskCamera.transform.localPosition = Character_Camera_Postion[0];

	}
	

	// Update is called once per frame
	void Update () {
		
	
	}
	
	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		
		this.GUILevel = 4;
		
		NGUITools.SetActive(mSexSelObj,false);
		NGUITools.SetActive(mCustomGeneralObj,false);
		NGUITools.SetActive(mCustomFaceDetailObj,false);
		NGUITools.SetActive(mRoleNamedObj,false);

		NGUITools.SetActive(mParticleObjDanceMyself,true);
		NGUITools.SetActive(mParticleObjChangeFace,false);
		NGUITools.SetActive(mParticleObjChangeRole,false);
		
		transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,100);	
		
				//加载配置表文件 随机姓名词典
		Globals.Instance.MResourceManager.Load("Config/Family_name",delegate(Object obj, string error) {
			if(obj != null){	
				TextAsset tText = (TextAsset)obj;
				FamilyNameList = tText.text.Split(new char[]{','});
			}
		});
		
		Globals.Instance.MResourceManager.Load("Config/Chn_given_name_boy",delegate(Object obj, string error) {
			if(obj != null){	
				TextAsset tText = (TextAsset)obj;
				GivenNameListMale = tText.text.Split(new char[]{','});
			}
		});
		
		Globals.Instance.MResourceManager.Load("Config/Chn_given_name_girl",delegate(Object obj, string error) {
			if(obj != null){	
				TextAsset tText = (TextAsset)obj;
				GivenNameListFemale = tText.text.Split(new char[]{','});
			}
		});

		UIEventListener.Get(mImgBtnRand.gameObject).onClick += OnImgBtnRand;
		UIEventListener.Get(mSexSelNextBtn.gameObject).onClick += OnSexSelNext;
		UIEventListener.Get(mCustomGeneralPriorBtn.gameObject).onClick += OnCustomGeneralPrior;
		UIEventListener.Get(mCustomGeneralNextBtn.gameObject).onClick += OnCustomGeneralNextBtn;
		UIEventListener.Get(mCustomGeneralDetailBtn.gameObject).onClick += OnCustomGeneralDetail;
		UIEventListener.Get(mCustomDetailCancelBtn.gameObject).onClick += OnCustomDetailCancelBtn;
		UIEventListener.Get(mCustomDetailSaveBtn.gameObject).onClick += OnCustomDetailSave;
		UIEventListener.Get(mNamedPriorBtn.gameObject).onClick += OnNamePriorBtnSave;
		UIEventListener.Get(mNamedEnterBtn.gameObject).onClick += OnNameEnterBtnSave;
		UIEventListener.Get(mAllRandomBtn.gameObject).onClick += OnAllRandomBtnClick;
		UIEventListener.Get(mUpLoadAvatarBtn.gameObject).onClick += OnUpLoadAvatarBtnClick;

		
		mMaleToggle.Data = 1;
		mFemaleToggle.Data = 0;
//		UIEventListener.Get(mMaleToggle.gameObject).onClick += OnGenderSelectBtn;
//		UIEventListener.Get(mFemaleToggle.gameObject).onClick += OnGenderSelectBtn;
		
		Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.TASKCAMERA);
		
		for (int i=0; i<BoneCustomSliderList.Length; i++)
		{
			BoneCustomSliderList[i].Data = (BoneCustomType)i;
			BoneCustomSliderList[i].value = 0.5f;
			EventDelegate.Add(BoneCustomSliderList[i].onChange, BoneCustomSliderValueChange);
			SliderPriorValue.Add(BoneCustomSliderList[i].value - 0.5f);
		}
		
		///预制的头型//
		PlayerImageInitConfig playerImageInitConfig = Globals.Instance.MDataTableManager.GetConfig<PlayerImageInitConfig>();
		Dictionary<int,PlayerImageInitConfig.ImageInitData> faceImageInitList = playerImageInitConfig.GetItemElementList();
		int index1 = 0;
		foreach (PlayerImageInitConfig.ImageInitData imageData in faceImageInitList.Values)
		{
			if (imageData.nGener == mCurrentSex &&  imageData.type == "FaceParam")
			{
				GameObject gameobj =  GameObject.Instantiate(FaceCutomItemPrefab) as GameObject;
				gameobj.name = "FaceItemPrefab" + index1.ToString();
				gameobj.transform.parent = wrapContent.transform;
				//gameobj.transform.localPosition = new Vector3(wrapContent.itemSize*index1++,0,0);
				gameobj.transform.localScale = Vector3.one;
				UITexture uiTexture = gameobj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
				string texturePath = "Icon/ItemIcon/" + imageData.icon;
				uiTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
				UIToggle imageBtn = gameobj.GetComponent<UIToggle>();
				imageBtn.Data = imageData.nID;
				UIEventListener.Get(imageBtn.gameObject).onClick = OnClickFaceParamItem;
			}
		}
		wrapContent.SortBasedOnScrollMovement();
		wrapContent.WrapContent();
		
		
		///预制的发型//
		mHairCutToggleList.Clear();
		index1 = 0;
		foreach (PlayerImageInitConfig.ImageInitData imageData in faceImageInitList.Values)
		{
			if (imageData.nGener == mCurrentSex &&  imageData.type == "HairCut")
			{
				GameObject gameobj =  GameObject.Instantiate(HairCutomItemPrefab) as GameObject;
				gameobj.name = "HairCutomItemPrefab" + index1.ToString();
				gameobj.transform.parent = HiarWrapContent.transform;
				//gameobj.transform.localPosition = new Vector3(wrapContent.itemSize*index1++,0,0);
				gameobj.transform.localScale = Vector3.one;
				UITexture uiTexture = gameobj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
				string texturePath = "Icon/ItemIcon/" + imageData.icon;
				uiTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
				UIToggle imageBtn = gameobj.GetComponent<UIToggle>();
				imageBtn.Data = imageData.nID;
				UIEventListener.Get(imageBtn.gameObject).onClick = OnClickHairParamItem;
				mHairCutToggleList.Add(imageBtn);
			}
		}
		HiarWrapContent.SortBasedOnScrollMovement();
		HiarWrapContent.WrapContent();
		
		///预制的身体颜色//
		mBodyToggleList.Clear();
		index1 = 0;
		foreach (PlayerImageInitConfig.ImageInitData imageData in faceImageInitList.Values)
		{
			if (imageData.nGener == mCurrentSex &&  imageData.type == "BodyColor")
			{
				GameObject gameobj =  GameObject.Instantiate(BodyColorCutomItemPrefab) as GameObject;
				gameobj.name = "BodyColorCutomItemPrefab" + index1.ToString();
				gameobj.transform.parent = BodyColorWrapContent.transform;
				//gameobj.transform.localPosition = new Vector3(wrapContent.itemSize*index1++,0,0);
				gameobj.transform.localScale = Vector3.one;
				UITexture uiTexture = gameobj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
				string texturePath = "Icon/ItemIcon/" + imageData.icon;
				uiTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
				uiTexture.color = StrParser.ParseColor(imageData.initValue);
				UIToggle imageBtn = gameobj.GetComponent<UIToggle>();
				imageBtn.Data = imageData.nID;
				UIEventListener.Get(imageBtn.gameObject).onClick = OnClickBodyColorParamItem;
				mBodyToggleList.Add(imageBtn);
			}
		}
		BodyColorWrapContent.SortBasedOnScrollMovement();
		BodyColorWrapContent.WrapContent();

		if (!GameDefines.Setting_ScreenQuality)
		{
			GameObject mainLight = GameObject.Find("Directional light Main");
			GameObject backLight = GameObject.Find("Directional light Back");
			if (mainLight != null)
			{
				//mainLight.SetActive(false);
				Light mL = mainLight.GetComponent<Light>();
				mL.shadows =  LightShadows.None;
			}
			
			if (backLight != null)
			{
				//backLight.SetActive(false);
			}
		}

		OnGenderSelectBtn(mFemaleToggle.gameObject);
	}
	

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		if(characterCustomizeOne != null){
			GameObject.DestroyImmediate(characterCustomizeOne.gameObject);
		}
		if(Globals.Instance.MSceneManager != null){
			Globals.Instance.MSceneManager.mTaskCamera.transform.localPosition = new Vector3(0f,1f,-8f);
			Globals.Instance.MSceneManager.mTaskCamera.fieldOfView = 15;
			Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);
		}

		GameObject createrole = GameObject.Find("SceneYanboshi");
		if(createrole!=null){
			DestroyImmediate(createrole);
		}

		DestroyImmediate(mParticleObjDanceMyself);
		DestroyImmediate(mParticleObjChangeRole);
		DestroyImmediate(mParticleObjChangeFace);

		BodyTexture2D = null;
		HeadTexture2D = null;

		GameObject.DestroyImmediate(BodyTexture);
		GameObject.DestroyImmediate(HeadTexture);

		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}
	
	
	private void  OnGenderSelectBtn(GameObject Obj)
	{
		
		UIToggle tog = Obj.transform.GetComponent<UIToggle>();
		int genderID  = (int)tog.Data;
		
		if (genderID == 1)
		{
			Globals.Instance.MGUIManager.ShowSimpleCenterTips("See him next version!",true);
			return ;
		}
		
		mCurrentSex = genderID;
		

		NGUITools.SetActive(mParticleObjChangeRole,true);
		HelpUtil.SetPlarticleState(mParticleObjChangeRole,true);
		
//		if (mLightObj1.activeSelf == false)
//		{
//			////用moveto来实现依次打开 mLightObj1，mLightObj2，mLightObj3这3盏灯//
//			iTween.MoveTo(mFemaleToggle.gameObject,iTween.Hash("isLocal",true,"position",mFemaleToggle.transform.localPosition,"time",1.5f),null,delegate(){
//				NGUITools.SetActive(mLightObj1,true);
//				iTween.MoveTo(mLightObj1,iTween.Hash("isLocal",true,"position",mLightObj1.transform.localPosition,"time",1.2f),null,delegate(){
//					NGUITools.SetActive(mLightObj2,true);
//					iTween.MoveTo(mLightObj2,iTween.Hash("isLocal",true,"position",mLightObj1.transform.localPosition,"time",1.5f),null,delegate(){
//						NGUITools.SetActive(mLightObj3,true);
//					});	
//				});
//			});
//
//		}
		
		characterCustomizeOne.ResetCharacter();
		characterCustomizeOne.ResetFaceCustomBone();
		
			
		for (int i=(int)BoneCustomType.TALL_CUSTOM; i<BoneCustomSliderList.Length; i++)
		{
			UISlider clickSlider = BoneCustomSliderList[i];
			BoneCustomSliderList[i].value = 0.5f;
			SliderPriorValue[i] = 0;
		}
		
		characterCustomizeOne.generateCharacterFromConfig(mCurrentSex ,"D0101");
		characterCustomizeOne.getCharacterAnimator().SetInteger("index",10);
		characterCustomizeOne.changeCharacterAnimationController("Me_IdleCreateRole");
		characterCustomizeOne.transform.localEulerAngles = new Vector3(0,175,0);
		//characterCustomizeOne.transform.Find ("A001").localScale = new Vector3 (0.005f,0.005f,0.005f);
		NGUITools.SetTweenActive(mSexSelObj,true,delegate {
			
		});
		
		mPlayerImageData.nHairCutItemID = characterCustomizeOne.getHairCutItemID();
		
		
	}
	

	
	private void OnClickFaceParamItem(GameObject obj)
	{
		
		if (mCurrentSelectFaceGameObj != null)
		{
			UITexture uiTexture = mCurrentSelectFaceGameObj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
			uiTexture.transform.localScale = Vector3.one;
		}
		
		UIToggle btn = obj.GetComponent<UIToggle>();
		int customInitID = (int)btn.Data;
		PlayerImageInitConfig playerImageInitConfig = Globals.Instance.MDataTableManager.GetConfig<PlayerImageInitConfig>();
		PlayerImageInitConfig.ImageInitData itemElement;
		playerImageInitConfig.GetItemElement(customInitID,out itemElement);
		
		characterCustomizeOne.ResetFaceCustomBone();
		
		ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
		
		string customValue = itemElement.initValue;
		
		string[] sections = customValue.Split(',');
		int iter_section = 0;
		foreach (string section in sections)
		{
			string[] keyValues = section.Split(':');
			if (keyValues[0] == "hairCut")
			{
				int itemID = StrParser.ParseDecInt(keyValues[1],0);
				ItemConfig.ItemElement element = null;
				bool IsHas = config.GetItemElement(itemID, out element);
				if (!IsHas){
					return;
				}
				string partModelName = element.ModelName;
				if(partModelName != "0" && partModelName != "")
				{
					string partName = partModelName.Substring(0,partModelName.Length - 3);
					string partIDStr = partModelName.Substring(partModelName.Length -3,3);
					characterCustomizeOne.ChangePart(partName,partIDStr,element.ItemSmallCategory,true,element.OutTextureName0,element.OutTextureName1,element.OutColor);	
					mPlayerImageData.nHairCutItemID = itemID;
				}
				
				NGUITools.SetActive(mParticleObjChangeFace,true);
				HelpUtil.SetPlarticleState(mParticleObjChangeFace,true);
			}
			
			if (keyValues[0] == "customParam")
			{
				string customParam = keyValues[1];
				string[] sectionsParam = customParam.Split('%');
			    int i=(int)BoneCustomType.EYE_CUSTOM_BIGS;
				foreach (string sectionsStr in sectionsParam)
				{
					if (sectionsStr != "")
					{
						string[] keyValueParam = sectionsStr.Split('#');
						float boneCutomValue = StrParser.ParseFloat(keyValueParam[1],0.0f);
		
						characterCustomizeOne.changeCustomBone(StrParser.ParseDecInt(keyValueParam[0],0)*2 + mCurrentSex,boneCutomValue);
						SliderPriorValue[i] = boneCutomValue;
						mPlayerImageData.UpdateBoneCustomData(StrParser.ParseDecInt(keyValueParam[0],0),boneCutomValue);
						i++;
					}
				}
			}
			
			if (keyValues[0] == "bodyColor")
			{
				string colorStr = keyValues[1] + "," + sections[iter_section+1]  + "," + sections[iter_section+2] + "," + sections[iter_section+3] ;
				Color bodyColor = StrParser.ParseLowerColor(colorStr);
				characterCustomizeOne.ChangeBodyColor(bodyColor);
				characterCustomizeOne.SetBodyColor(bodyColor);
				mPlayerImageData.nBodyColor = bodyColor;
			}
			iter_section++;
		}
		
		mCurrentSelectFaceGameObj = obj;
		if (mCurrentSelectFaceGameObj != null)
		{
			UITexture uiTexture = mCurrentSelectFaceGameObj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
			uiTexture.transform.localScale = new  Vector3(1.19f,1.19f,1.19f);
		}
		Dictionary<int,float> boneCustomValues = mPlayerImageData.getCustomValues();
		for (int i=5; i<BoneCustomSliderList.Length; i++)
		{
			BoneCustomSliderList[i].Data = (BoneCustomType)i;
			BoneCustomSliderList[i].value = boneCustomValues[i] + 0.5f;
			EventDelegate.Add(BoneCustomSliderList[i].onChange, BoneCustomSliderValueChange);
			SliderPriorValue.Add(BoneCustomSliderList[i].value);
		}

	}
	
	
	private void OnClickHairParamItem(GameObject obj)
	{
		if (mCurrentSelectHairGameObj != null)
		{
			UITexture uiTexture = mCurrentSelectHairGameObj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
			uiTexture.SetDimensions(210,210);
		}
		
		UIToggle btn = obj.GetComponent<UIToggle>();
		int customInitID = (int)btn.Data;
		PlayerImageInitConfig playerImageInitConfig = Globals.Instance.MDataTableManager.GetConfig<PlayerImageInitConfig>();
		PlayerImageInitConfig.ImageInitData itemElement;
		playerImageInitConfig.GetItemElement(customInitID,out itemElement);
		
		string customValue = itemElement.initValue;
		int itemID = StrParser.ParseDecInt(customValue,-1);
		ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
		ItemConfig.ItemElement element = null;
		bool IsHas = config.GetItemElement(itemID, out element);
		
		
		string partModelName = element.ModelName;
		if(partModelName != "0" && partModelName != "")
		{
			string partName = partModelName.Substring(0,partModelName.Length - 3);
			string partIDStr = partModelName.Substring(partModelName.Length -3,3);
			characterCustomizeOne.ChangePart(partName,partIDStr,element.ItemSmallCategory,true,element.OutTextureName0,element.OutTextureName1,element.OutColor);	
			mPlayerImageData.nHairCutItemID = itemID;
			
			NGUITools.SetActive(mParticleObjChangeFace,true);
			HelpUtil.SetPlarticleState(mParticleObjChangeFace,true);
		}
		
		mCurrentSelectHairGameObj = obj;
		if (mCurrentSelectHairGameObj != null)
		{
			UITexture uiTexture = mCurrentSelectHairGameObj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
			uiTexture.SetDimensions(250,250);
		}
	}
	
	private void OnClickBodyColorParamItem(GameObject obj)
	{
		if (mCurrentSelectColorGameObj != null)
		{
			UITexture uiTexture = mCurrentSelectColorGameObj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
			uiTexture.SetDimensions(210,210);
		}
		UIToggle btn = obj.GetComponent<UIToggle>();
		int customInitID = (int)btn.Data;
		PlayerImageInitConfig playerImageInitConfig = Globals.Instance.MDataTableManager.GetConfig<PlayerImageInitConfig>();
		PlayerImageInitConfig.ImageInitData itemElement;
		playerImageInitConfig.GetItemElement(customInitID,out itemElement);
		
		string customValue = itemElement.initValue;
		Color bodyColor = StrParser.ParseColor(customValue);
		characterCustomizeOne.ChangeBodyColor(bodyColor);
		characterCustomizeOne.SetBodyColor(bodyColor);
		mPlayerImageData.nBodyColor = bodyColor;
		
		mCurrentSelectColorGameObj = obj;
		if (mCurrentSelectColorGameObj != null)
		{
			UITexture uiTexture = mCurrentSelectColorGameObj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
			uiTexture.SetDimensions(250,250);
		}
	}
	
	void SetRandName()
	{
		string tFamilyName = FamilyNameList[Random.Range(0, FamilyNameList.Length)];
		string tGivenName = "";
		if (mCurrentSex == 0)
			tGivenName = GivenNameListFemale[Random.Range(0, GivenNameListFemale.Length)];
		else
			tGivenName = GivenNameListMale[Random.Range(0, GivenNameListMale.Length)];
		mInputLableNamed.text = tFamilyName + tGivenName;
	}
	
	void OnImgBtnRand(GameObject obj)
	{
		mInputNamed.value = "";
		SetRandName();
	}
	
	
	void OnSexSelNext(GameObject obj)
	{
		NGUITools.SetTweenActive(mSexSelZereObj,false,delegate {
			
		});
		NGUITools.SetTweenActive(mSexSelObj,false,delegate {
			float tallValue = BoneCustomSliderList[(int)BoneCustomType.TALL_CUSTOM].value ;
			Vector3 destPos = Character_Camera_Postion[1] + new Vector3(0f,(tallValue - 0.5f)*0.06f,0f);
			Globals.Instance.MSceneManager.mTaskCamera.fieldOfView = 13;
			characterCustomizeOne.transform.localEulerAngles  = Character_Rotation[1];	
			iTween.MoveTo(Globals.Instance.MSceneManager.mTaskCamera.gameObject,iTween.Hash("isLocal",true,"position",destPos,"time",0.6f),null,delegate(){
				NGUITools.SetTweenActive(mCustomGeneralObj,true,delegate {
				
				///预制的头型//
				HelpUtil.DelListInfo(wrapContent.transform);
				PlayerImageInitConfig playerImageInitConfig = Globals.Instance.MDataTableManager.GetConfig<PlayerImageInitConfig>();
				Dictionary<int,PlayerImageInitConfig.ImageInitData> faceImageInitList = playerImageInitConfig.GetItemElementList();
				int index1 = 0;
				foreach (PlayerImageInitConfig.ImageInitData imageData in faceImageInitList.Values)
				{
					if (imageData.nGener == mCurrentSex &&  imageData.type == "FaceParam")
					{
						GameObject gameobj =  GameObject.Instantiate(FaceCutomItemPrefab) as GameObject;
						gameobj.name = "FaceItemPrefab" + index1.ToString();
						gameobj.transform.parent = wrapContent.transform;
						//gameobj.transform.localPosition = new Vector3(wrapContent.itemSize*index1++,0,0);
						gameobj.transform.localScale = Vector3.one;
						UITexture uiTexture = gameobj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
						string texturePath = "Icon/ItemIcon/" + imageData.icon;
						uiTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
						UIToggle imageBtn = gameobj.GetComponent<UIToggle>();
						imageBtn.Data = imageData.nID;
						UIEventListener.Get(imageBtn.gameObject).onClick = OnClickFaceParamItem;
					}
				}
				wrapContent.SortBasedOnScrollMovement();
				wrapContent.WrapContent();
				
				});
			});
			

		});
	}
	
	
	void OnCustomGeneralPrior(GameObject obj)
	{
		BoxCollider boxCollider = mCustomGeneralNextBtn.GetComponent<BoxCollider>();
		boxCollider.enabled = false;
		NGUITools.SetTweenActive(mCustomGeneralObj,false,delegate {
			boxCollider.enabled = true;
			Vector3 destPos = Character_Camera_Postion[0];
			Globals.Instance.MSceneManager.mTaskCamera.fieldOfView = 15;
			characterCustomizeOne.transform.localEulerAngles  = Character_Rotation[0];
			NGUITools.SetActive(mSexSelZereObj , true);
			NGUITools.SetActive(mSexSelObj , true);
			iTween.MoveTo(Globals.Instance.MSceneManager.mTaskCamera.gameObject,iTween.Hash("isLocal",true,"position",destPos,"time",1.0f),null,delegate(){

			});
		});
	}

	
	void OnCustomGeneralNextBtn(GameObject obj)
	{
		NGUITools.SetActive(mCustomGeneralObj , false);
		StartCoroutine(BodyPhotoGraphFinished());
	}

	IEnumerator HeadPhotoGraphFinished()
	{
		yield return new WaitForSeconds(0.2f);

		BodyTexture.mainTexture = BodyTexture2D;
		HeadTexture.mainTexture = HeadTexture2D;
		
		NGUITools.SetActive(ShareTexture , true);
	}


	void OnCustomGeneralDetail(GameObject obj)
	{
		BoxCollider boxCollider = mCustomGeneralPriorBtn.GetComponent<BoxCollider>();
		BoxCollider boxCollider1 = mCustomGeneralNextBtn.GetComponent<BoxCollider>();
		boxCollider.enabled = false;
		boxCollider1.enabled = false;
		
		NGUITools.SetTweenActive(mCustomGeneralObj,false,delegate {
			NGUITools.SetTweenActive(mCustomFaceDetailObj,true,delegate {
				
				boxCollider.enabled = true;
				boxCollider1.enabled = true;
				
				///预制的发型//
				HelpUtil.DelListInfo(HiarWrapContent.transform);
				mHairCutToggleList.Clear();
				PlayerImageInitConfig playerImageInitConfig = Globals.Instance.MDataTableManager.GetConfig<PlayerImageInitConfig>();
				Dictionary<int,PlayerImageInitConfig.ImageInitData> faceImageInitList = playerImageInitConfig.GetItemElementList();
				int index1 = 0;
				foreach (PlayerImageInitConfig.ImageInitData imageData in faceImageInitList.Values)
				{
					if (imageData.nGener == mCurrentSex &&  imageData.type == "HairCut")
					{
						GameObject gameobj =  GameObject.Instantiate(HairCutomItemPrefab) as GameObject;
						gameobj.name = "HairCutomItemPrefab" + index1.ToString();
						gameobj.transform.parent = HiarWrapContent.transform;
						//gameobj.transform.localPosition = new Vector3(wrapContent.itemSize*index1++,0,0);
						gameobj.transform.localScale = Vector3.one;
						UITexture uiTexture = gameobj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
						string texturePath = "Icon/ItemIcon/" + imageData.icon;
						uiTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
						UIToggle imageBtn = gameobj.GetComponent<UIToggle>();
						imageBtn.Data = imageData.nID;
						UIEventListener.Get(imageBtn.gameObject).onClick = OnClickHairParamItem;
						mHairCutToggleList.Add(imageBtn);
					}
				}
				HiarWrapContent.SortBasedOnScrollMovement();
				HiarWrapContent.WrapContent();
				
				///预制的身体颜色//
				HelpUtil.DelListInfo(BodyColorWrapContent.transform);
				mBodyToggleList.Clear();
				index1 = 0;
				foreach (PlayerImageInitConfig.ImageInitData imageData in faceImageInitList.Values)
				{
					if (imageData.nGener == mCurrentSex &&  imageData.type == "BodyColor")
					{
						GameObject gameobj =  GameObject.Instantiate(BodyColorCutomItemPrefab) as GameObject;
						gameobj.name = "BodyColorCutomItemPrefab" + index1.ToString();
						gameobj.transform.parent = BodyColorWrapContent.transform;
						//gameobj.transform.localPosition = new Vector3(wrapContent.itemSize*index1++,0,0);
						gameobj.transform.localScale = Vector3.one;
						UITexture uiTexture = gameobj.transform.Find("FaceHairTexture").GetComponent<UITexture>();
						string texturePath = "Icon/ItemIcon/" + imageData.icon;
						uiTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
						uiTexture.color = StrParser.ParseColor(imageData.initValue);
						UIToggle imageBtn = gameobj.GetComponent<UIToggle>();
						imageBtn.Data = imageData.nID;
						UIEventListener.Get(imageBtn.gameObject).onClick = OnClickBodyColorParamItem;
						mBodyToggleList.Add(imageBtn);
					}
				}
				BodyColorWrapContent.SortBasedOnScrollMovement();
				BodyColorWrapContent.WrapContent();
				
			});
		});
	}
	
	void OnCustomDetailCancelBtn(GameObject obj)
	{
		NGUITools.SetTweenActive(mCustomFaceDetailObj,false,delegate {
			NGUITools.SetTweenActive(mCustomGeneralObj,true,delegate {
				
			});
		});
	}
	
	void OnCustomDetailSave(GameObject obj)
	{
		NGUITools.SetTweenActive(mCustomFaceDetailObj,false,delegate {

			StartCoroutine(BodyPhotoGraphFinished());
		});
	}
	
	void OnNamePriorBtnSave(GameObject obj)
	{
		NGUITools.SetTweenActive(mRoleNamedObj,false,delegate {
			float tallValue = BoneCustomSliderList[(int)BoneCustomType.TALL_CUSTOM].value ;
			Vector3 destPos = Character_Camera_Postion[1]  + new Vector3(0f,(tallValue - 0.5f)*0.06f,0f);
			Globals.Instance.MSceneManager.mTaskCamera.fieldOfView = 13;
			characterCustomizeOne.transform.localEulerAngles  = Character_Rotation[1];	
			characterCustomizeOne.transform.localPosition = new Vector3(0.08f,0.2f,0f);
			iTween.MoveTo(Globals.Instance.MSceneManager.mTaskCamera.gameObject,iTween.Hash("isLocal",true,"position",destPos,"time",1.0f),null,delegate(){
				NGUITools.SetTweenActive(mCustomGeneralObj,true,delegate {
					
				});
			});
		});		
	}
	
	void OnNameEnterBtnSave(GameObject obj)
	{
		string str = mInputNamed.value ;
		if(str.Length > 6)
		{
			Globals.Instance.MGUIManager.ShowErrorTips(10009);
			return;
		}
		long roleID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
		string testRoleName = mInputLableNamed.text;
		string roleName =  testRoleName;
		int gender = mCurrentSex;
			
		string avatarName = "GirlAvatar0";
		int randomIndex = Random.Range(1,8);
		avatarName += randomIndex.ToString();
		int countryID = -1;
		
		mPlayerImageData.nBodyColor = characterCustomizeOne.GetBodyColor();
		BoneCustomConfig boneCustomConfig = Globals.Instance.MDataTableManager.GetConfig<BoneCustomConfig>();
		
			
		float scaleFactor = 0.2f;
		int   boneCustomTypeId = 0;
		float boneCustomValue = BoneCustomSliderList[boneCustomTypeId].value  - 0.5f;
		BoneCustomConfig.CustomElement customElement = null;
		bool hasItem = boneCustomConfig.GetItemElement(boneCustomTypeId*2 + mCurrentSex ,out customElement);
		mPlayerImageData.nBodyHeight = customElement.ValueBegin * (1 + boneCustomValue*scaleFactor);
		
	
		boneCustomTypeId = 1;
		boneCustomValue = BoneCustomSliderList[boneCustomTypeId].value  - 0.5f;
		hasItem = boneCustomConfig.GetItemElement(boneCustomTypeId*2 + mCurrentSex ,out customElement);
		mPlayerImageData.nBodyWeight = customElement.ValueBegin * (1 + boneCustomValue*scaleFactor);
		
		boneCustomTypeId = 2;
		boneCustomValue = BoneCustomSliderList[boneCustomTypeId].value  - 0.5f;
		hasItem = boneCustomConfig.GetItemElement(boneCustomTypeId*2 + mCurrentSex ,out customElement);
		mPlayerImageData.nChestSize = customElement.ValueBegin * (1 + boneCustomValue*scaleFactor);
		
		boneCustomTypeId = 3;
		boneCustomValue = BoneCustomSliderList[boneCustomTypeId].value - 0.5f;
		hasItem = boneCustomConfig.GetItemElement(boneCustomTypeId*2 + mCurrentSex ,out customElement);
		mPlayerImageData.nWaistSize = customElement.ValueBegin * (1 + boneCustomValue*scaleFactor);

		boneCustomTypeId = 4;
		boneCustomValue = BoneCustomSliderList[boneCustomTypeId].value - 0.5f;
		hasItem = boneCustomConfig.GetItemElement(boneCustomTypeId*2 + mCurrentSex ,out customElement);
		mPlayerImageData.nHipSize = customElement.ValueBegin * (1 + boneCustomValue*scaleFactor);
	
		GUIRadarScan.Show();

		NetSender.Instance.RequestCreatePlayer(roleName, gender, countryID, avatarName, mPlayerImageData.CustomDataToString());
	}
	
	void OnAllRandomBtnClick(GameObject obj)
	{
		characterCustomizeOne.ResetFaceCustomBone();
		for (int i=(int)BoneCustomType.EYE_CUSTOM_BIGS; i<BoneCustomSliderList.Length; i++)
		{
			SliderPriorValue[i] = 0.0f;
		}
		
		for (int i=(int)BoneCustomType.EYE_CUSTOM_BIGS; i<BoneCustomSliderList.Length; i++)
		{
			float randomValue = Random.Range(0.0f,1.0f);
			Debug.Log("randomValue is :" + randomValue);
			UISlider clickSlider = BoneCustomSliderList[i];
			BoneCustomSliderList[i].value = randomValue;
		
			BoneCustomConfig boneCustomConfig = Globals.Instance.MDataTableManager.GetConfig<BoneCustomConfig>();
			BoneCustomConfig.CustomElement customElement = null;
			bool hasItem = boneCustomConfig.GetItemElement(i*2 + mCurrentSex ,out customElement);
			if (!hasItem)
				return;
			UILabel lableText = clickSlider.transform.Find("Label").GetComponent<UILabel>();
			if (lableText != null)
				lableText.text = "";
			characterCustomizeOne.changeCustomBone(i*2 + mCurrentSex ,randomValue - 0.5f,SliderPriorValue[i]);
			SliderPriorValue[i] = randomValue - 0.5f;
			
			mPlayerImageData.UpdateBoneCustomData(i,randomValue - 0.5f);
		}
		
		int hairCutIter = Random.Range(0, mHairCutToggleList.Count);
		UIToggle selToggle = mHairCutToggleList[hairCutIter];
		if (mHairCutToggle.value)
			selToggle.value = true;
		OnClickHairParamItem(selToggle.gameObject);

		
		hairCutIter = Random.Range(0, mBodyToggleList.Count);
		UIToggle selColorToggle = mBodyToggleList[hairCutIter];
		if (mColorToggle.value)
			selColorToggle.value = true;
		OnClickBodyColorParamItem(selColorToggle.gameObject);
		
	}

	public void OnUpLoadAvatarBtnClick(GameObject obj)
	{
		//U3dAppStoreSender.OpenIphonePhoto();
	}
	
	public void OnHairCutSelectedBtn(GameObject Obj)
	{
		bool val = UIToggle.current.value;
		if (val && mCurrentSelectHairGameObj != null)
		{
			UIToggle uiCurSelHairToggle = mCurrentSelectHairGameObj.GetComponent<UIToggle>();
			uiCurSelHairToggle.value = true;
		}
	}
	
	public void OnColorSelectedBtn(GameObject Obj)
	{
		bool val = UIToggle.current.value;
		if (val && mCurrentSelectColorGameObj != null)
		{
			UIToggle uiCurSelHairToggle = mCurrentSelectColorGameObj.GetComponent<UIToggle>();
			uiCurSelHairToggle.value = true;
		}
	}
	
	void BoneCustomSliderValueChange()
	{
		UISlider clickSlider = (UISlider)UIProgressBar.current;
	
		if (clickSlider.Data == null)
			return;
		int customBoneType = (int)clickSlider.Data;

		float customValue = clickSlider.value;
		
		////没变化则退出///
		if (Mathf.Abs(customValue - 0.5f - SliderPriorValue[customBoneType]) < 0.00000000001f)
			return ;
		
		
		BoneCustomConfig boneCustomConfig = Globals.Instance.MDataTableManager.GetConfig<BoneCustomConfig>();
		BoneCustomConfig.CustomElement customElement = null;
		bool hasItem = boneCustomConfig.GetItemElement(customBoneType*2 + mCurrentSex ,out customElement);
		if (!hasItem)
			return;
	
		characterCustomizeOne.changeCustomBone(customBoneType*2 + mCurrentSex ,customValue - 0.5f,SliderPriorValue[customBoneType]);
		SliderPriorValue[customBoneType] = customValue - 0.5f;
		
		mPlayerImageData.UpdateBoneCustomData(customBoneType,customValue - 0.5f);
		
		if ( customBoneType == (int)BoneCustomType.WAIST_CUSTOM ||
			customBoneType == (int)BoneCustomType.CHEST_CUSTOM ||customBoneType == (int)BoneCustomType.HIP_CUSTOM)
		{
			UILabel lableText = clickSlider.transform.Find("Label").GetComponent<UILabel>();
			lableText.text = (int) (customElement.ValueBegin * ( 1 + (customValue - 0.5f)*0.2)) + "cm";
		}
		else if (customBoneType == (int)BoneCustomType.TALL_CUSTOM)
		{
			UILabel lableText = clickSlider.transform.Find("Label").GetComponent<UILabel>();
			lableText.text = (int) (customElement.ValueBegin * ( 1 + (customValue - 0.5f)*0.0607f)) + "cm";	
		}
		else if (customBoneType == (int)BoneCustomType.WEEIGHT_CUSTOM )
		{
			UILabel lableText = clickSlider.transform.Find("Label").GetComponent<UILabel>();
			lableText.text = (int) (customElement.ValueBegin * ( 1 + (customValue - 0.5f)*0.4)) + "kg";
		}
		else
		{
			UILabel lableText = clickSlider.transform.Find("Label").GetComponent<UILabel>();
			lableText.text = "";
		}
			

		
	}
	
	public void UpdateZeroStep()
	{
		NGUITools.SetTweenActive(mSexSelZereObj,true,delegate {

		});
	}


	IEnumerator BodyPhotoGraphFinished()
	{
		yield return new WaitForSeconds(0f);

		GameObject tex1 = PhotosEffect.transform.Find("GameObject").gameObject.transform.Find("Texture1").gameObject;
		GameObject tex2 = PhotosEffect.transform.Find("GameObject").gameObject.transform.Find("Texture2").gameObject;
		tex1.transform.localPosition = new Vector3(0f,-2000f,0f);
		TweenPosition tween1 = TweenPosition.Begin(tex1,0.3f,new Vector3(0f,-585f,0f));
		EventDelegate.Add(tween1.onFinished,delegate() {
			TweenPosition.Begin(tex1,0.3f,new Vector3(0f,-2000f,0f)).delay = 0.3f;
		},true);

		tex2.transform.localPosition = new Vector3(0f,2000f,0f);
		TweenPosition tween2 = TweenPosition.Begin(tex2,0.3f,new Vector3(0f,700f,0f));
		EventDelegate.Add(tween2.onFinished,delegate() {
			TweenPosition tween = TweenPosition.Begin(tex2,0.3f,new Vector3(0f,2000f,0f));
			tween.delay = 0.3f;
			EventDelegate.Add(tween.onFinished,delegate() {
				AgainPhotoGraph();
			},true);
		},true);

		var rtW = Screen.width/2;
		var rtH = Screen.height/2;

		HeadTexture2D = new Texture2D((int)rtW, rtH, TextureFormat.RGB24,false);
		CaptureCamera(Globals.Instance.MSceneManager.mTaskCamera,new Rect(0,0,rtW,rtH),HeadTexture2D);
		PhotoTexture.mainTexture = HeadTexture2D;
		NGUITools.SetActive(PhotosEffect , true);
	}

	public static void  CaptureCamera(Camera camera, Rect rect,Texture2D destTexture2d)   
	{  
		if (destTexture2d == null)
			return;
		// 创建一个RenderTexture对象  
		RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 16);  
		// 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
		camera.targetTexture = rt;  
		camera.Render();  
		//ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
		//ps: camera2.targetTexture = rt;  
		//ps: camera2.Render();  
		//ps: -------------------------------------------------------------------  
		
		// 激活这个rt, 并从中中读取像素。  
		RenderTexture.active = rt; 
		
		destTexture2d.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
		destTexture2d.Apply();
		destTexture2d.Compress(true);
		
		// 重置相关参数，以使用camera继续在屏幕上显示  
		camera.targetTexture = null;  
		//ps: camera2.targetTexture = null;  
		RenderTexture.active = null; // JC: added to avoid errors  
		GameObject.Destroy(rt);  	
	} 
	
	void AgainPhotoGraph()
	{
		NGUITools.SetActive(PhotosEffect , false);
		Globals.Instance.MSceneManager.mTaskCamera.fieldOfView = 15;
	
	float tallValue = BoneCustomSliderList[(int)BoneCustomType.TALL_CUSTOM].value ;
	float boneCustomValue = tallValue  - 0.5f;
	float currentHeight = 165f * (1 + boneCustomValue*0.2f);
	float defaultHeight = 165f;
	float positionY = 0.01f+(currentHeight - defaultHeight) * 0.0030303f;
	characterCustomizeOne.transform.localPosition = new Vector3(characterCustomizeOne.transform.localPosition.x,
	                                                            0.2f+positionY , characterCustomizeOne.transform.localPosition.z);
	
	iTween.MoveTo(Globals.Instance.MSceneManager.mTaskCamera.gameObject,iTween.Hash("isLocal",true,"position",new Vector3(0f,300f,-1500f),"time",1.0f),null,delegate(){
			GameObject tex1 = PhotosEffect.transform.Find("GameObject").gameObject.transform.Find("Texture1").gameObject;
			GameObject tex2 = PhotosEffect.transform.Find("GameObject").gameObject.transform.Find("Texture2").gameObject;
			tex1.transform.localPosition = new Vector3(0f,-2000f,0f);
			TweenPosition tween1 = TweenPosition.Begin(tex1,0.3f,new Vector3(0f,-585f,0f));
			EventDelegate.Add(tween1.onFinished,delegate() {
				 TweenPosition.Begin(tex1,0.3f,new Vector3(0f,-2000f,0f)).delay = 0.3f;

			},true);
			
			tex2.transform.localPosition = new Vector3(0f,2000f,0f);
			TweenPosition tween2 = TweenPosition.Begin(tex2,0.3f,new Vector3(0f,700f,0f));
			EventDelegate.Add(tween2.onFinished,delegate() {
				TweenPosition tween = TweenPosition.Begin(tex2,0.3f,new Vector3(0f,2000f,0f));
				tween.delay = 0.3f;
				EventDelegate.Add(tween.onFinished,delegate() {
					
					StartCoroutine(HeadPhotoGraphFinished());
				},true);
			},true);

			var rtW = Screen.width/2;
			var rtH = Screen.height/2;

		
			BodyTexture2D = new Texture2D((int)rtW, rtH, TextureFormat.RGB24,false);
			CaptureCamera(Globals.Instance.MSceneManager.mTaskCamera,new Rect(0,0,rtW,rtH),BodyTexture2D);
			PhotoTexture.mainTexture = BodyTexture2D;
			NGUITools.SetActive(PhotosEffect , true);
		});
	}
	public void updateAvatar(string imageData)
	{
		Texture2D testAvatar = new Texture2D(512,512,TextureFormat.RGB24,false);
		testAvatar.LoadImage(System.Convert.FromBase64String(imageData));
		mTestAvatarTexture.mainTexture = testAvatar;
	}
	
	IEnumerator DoLoadAssetBundle(string name)
	{
 
		
		yield break;
	}
	
	//创建界面时  分享 保存时 拍照//
	private IEnumerator ShareOrSavePhotograph(int type = 0)
	{
		yield return new WaitForSeconds(0f);
		if(SharePhotosTexture == null)
		{
			var rtW = Screen.width/2;
			var rtH = Screen.height/2;
			SharePhotosTexture = GUIDormitory.CaptureCamera(GameObject.Find("UICamera").GetComponent<Camera>(),new Rect(0,0,rtW,rtH));
		}
		byte[] bytes =  SharePhotosTexture.EncodeToPNG();  
		string cptrAddr = "Screenshot" + System.DateTime.Now.Second.ToString() + ".png";
		string filename = Application.persistentDataPath + "/" + cptrAddr;  
		System.IO.File.WriteAllBytes(filename, bytes);   
		if(type == 1)
		{
			Globals.Instance.MTaskManager.IsGetShareReward = false;
			if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
			{
				//U3dAppStoreSender.ShareMyPhoto(filename);
			}
			else if(GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
			{
				AndroidSDKAgent.showShare(filename);
			}
		}
		else
		{
			if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
			{
				U3dIOSSendToSdk.AppSavePhoth(filename);
			}
			else if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
			{
				GUIRadarScan.Show();
				StartCoroutine(InvokeAndroidCameraSaveDelegate());
			}
		}
		NGUITools.SetActive(FunctionBtn , true);
	}

	IEnumerator InvokeAndroidCameraSaveDelegate()
	{
		yield return new WaitForSeconds(0.2f);
		// 最后将这些纹理数据，成一个png图片文件  //
		byte[] bytes =  SharePhotosTexture.EncodeToPNG();  
		string cptrAddr = "Screenshot" + System.DateTime.Now.Second.ToString() + ".png";
		string filename = Application.persistentDataPath + "/" + cptrAddr;
		try
		{
			System.IO.File.WriteAllBytes(filename, bytes);  
			Debug.Log(string.Format("截屏了一张照片: {0}", filename));   
			AndroidSDKAgent.SavePhoto(filename,cptrAddr);
			Debug.Log("----------------------" + filename);
		}
		catch
		{
			GUIRadarScan.Hide();
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(10012);
		}
		yield return 0;
	}
} 
