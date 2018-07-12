  using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameData;
using GameData.PlayerImageData;

public class CharacterCustomizeOne : MonoBehaviour 
{
	private  Quaternion direction=new Quaternion(); 

	private Quaternion LQuaternion=new Quaternion(); 
	private Quaternion RQuaternion=new Quaternion(); 
	
	public   bool bflag  = false;

	private Transform target;
	private Animator animator;
	
	
	private Transform[] hips;	
	
	struct BoneTransformModel
	{
		public Vector3 Position;
		public Vector3 LocalPosition;
		public Vector3 LocalEular;
		public Vector3 LocalScale;
	};
	
	private Dictionary<string ,BoneTransformModel>  mModelBoneInfo = new Dictionary<string, BoneTransformModel>();
	
	private Dictionary<string ,Transform>  mToChangeBone = new Dictionary<string, Transform>();

	private List<GameObject> mAccessorGameObjectList = new List<GameObject>();
	
	private static  Dictionary<string , List<string> > mSkinMeshBonesList = new Dictionary<string, List<string> >();
	
	private static Dictionary<string ,Dictionary<string,Transform> > data = new Dictionary<string, Dictionary<string,Transform> >();
	
	private Dictionary<string , SkinnedMeshRenderer> targetSmr = new  Dictionary<string , SkinnedMeshRenderer>();
	
	private int mCharacterConfigId; 
	
	private int mInitHairCutItemID;
	
	private  List<int> randomIdleList;
	////////////////////////////////////////////// =   头发,   上装  下装裙  手部    脚部  长裤
	private static readonly string[] InnerPartList = {"TAXF","TAXB","TAXC","TAXD","TAXG","TAXE","TAXA",};
	private static readonly string []targetsBaseName = {"A001","Dog001","Dog001"};
	private readonly int[] AccessoryPartID = {9,14,15,16,17};
	
	private static readonly string [] HumanPaintOriginal = {"eyes001:218,218,218,255#TAXA001_a001:0,0,0,255#TAXA001_b001:96,69,69,255#TAXA001_c001:255,72,26,128#TAXA001_d001:122,65,65,180",
															"eyes001:218,218,218,255#TAXA001_a001:0,0,0,255#TAXA001_b001:96,69,69,255#TAXA001_c001:255,72,26,128#TAXA001_d001:122,65,65,180",};

	private static readonly int PartListLength = 7;
	
	private bool mLongDressUP = false;
	private bool mDownPantsWeared = false; 
	
	private string mBaraTextureName = "";
	private string mUnderTextureName = "";
	private string mSockTextureName = "";
	
	private bool mAnimationOnState = false;
	private bool mFinishedCallBack = false;
	private float mAnimatorLength = 0;
	private Vector3 mTargetInitPosition = Vector3.zero;
	
	private Vector3 mTargetInitEular = Vector3.zero;
	
	private Transform mEyeLeftTransform;
	private Transform mEyeRightTraansform;
	

	private Transform mBip001Spine1;
	private Transform mBip001Spine2;

	private Vector3 mHeadLossScale;
	
	private Transform mHead;
	private Transform mHeadDog;
	private bool mEyeTracing = false;
	
	/// 
	private Transform mMouse;
	
	private Color mBodyColor;
		
	public Camera mFaceCamera;
	
	private int mGender = 0;
	
//	private RenderTexture mFaceRenderTexture = null;
	private Texture mFaceRenderTexture = null;
	
	public MeshRenderer mEyeBrowGameObj;
	public MeshRenderer mEyeShdowGameObj;
	public MeshRenderer mFaceRedGameObj;
	public MeshRenderer mMouseGameObj;
	public MeshRenderer mFaceGameObj;

	/// 好感度没有达到一定等级,不能看到内衣	/// 
	private bool mCanSeeBaraUnderWear = true;
	private int mCanSeeBaraUnderWearLevel = 5;
	
	enum CustomBonePRSType
	{
		PositionType = 1,
		RotateType = 2,
		ScaleType = 3
	}
	
	enum CustomBoneAxisType
	{
		XType = 1,
		YType = 2,
		ZType = 3,
		XYType = 4,
		YZType = 5,
		XZType = 6,
		XYZType = 7,
		PLUSXType = 8,
		PLUSYType = 9,
		PLUSZType = 10,
	}

	enum ChangePartType
	{
		HAT_HAIR = 1,//帽子//
		SHANGYI = 2 ,//上衣//
		XIAZHUANGQUN = 3 , //下装裙//
		SHOUTAO = 4 , //手套//
		SHOES = 5, //鞋子//
		NEIYI = 6 , //内衣
		NEIKU = 7, //内裤
		SOCKET = 8, //
	}
	
	private Dictionary<int ,string>  mAccessoryModelList = new Dictionary<int, string>();

	void Awake()
	{

	}

		
	void OnDestroy()
	{
		if (target != null)
			Destroy(target.gameObject);
		
		System.GC.Collect();
	}

	static public void ReleaseData()
	{
		data.Clear();
	}

	public Transform getHeadTransform()
	{
		if (mGender == (int) PlayerGender.GENDER_DOG)
		{
			return mHeadDog;
		}
		else if (mGender == (int) PlayerGender.GENDER_FEMALE) 
		{
			return  mHead;
		}	
		return null;
	}
	
	private void InitBoneInfoFromGender(int sexGender)
	{		
		if (hips == null)
		{

			//Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/characterbase"+targetsBaseName[sexGender]);
			Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/characterbase");
			GameObject root = GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject;
			animator = root.GetComponent<Animator>();
			root.name = targetsBaseName[sexGender];
			root.transform.localScale =  Vector3.one;
			target  = root.transform;
			target.parent = this.transform;

			target.transform.localPosition = Vector3.zero;
			target.transform.localEulerAngles = Vector3.zero;

			//从目标物件取得骨架资料 (Female_Hips 的全部物件)///////
			hips = target.GetComponentsInChildren<Transform>();
			
			foreach(Transform hip in hips){
				if (!mModelBoneInfo.ContainsKey(hip.name))
				{
					BoneTransformModel boneTransformModel = new BoneTransformModel();
					boneTransformModel.Position = hip.position;
					boneTransformModel.LocalScale = hip.localScale;
					boneTransformModel.LocalPosition = hip.localPosition;
					boneTransformModel.LocalEular = hip.localEulerAngles;
					mModelBoneInfo.Add(hip.name,boneTransformModel);
					mToChangeBone.Add(hip.name,hip);
					if(hip.name == "yanL")
						mEyeLeftTransform = hip;
					if (hip.name == "yanR")
						mEyeRightTraansform = hip;
					if (hip.name == "Bip001 Head")
					{
						mHead = hip;
						mHeadLossScale = mHead.lossyScale;
					}
					else if (hip.name == "Bip001 Spine1")
						mBip001Spine1 = hip;
					else if (hip.name == "Bip001 Spine2")
						mBip001Spine2 = hip;
					else if (hip.name == "Bip001 Head Dog")
						mHeadDog = hip;
				}
	
			}
		}


		if (targetSmr.Count == 0)
		{
			for (int i=0; i<PartListLength; i++)
			{
				GameObject partObj = new GameObject();
				partObj.name = InnerPartList[i];
				partObj.transform.parent = target;
				partObj.transform.localPosition = Vector3.zero;
				partObj.transform.localEulerAngles = Vector3.zero;
				//为新建立的 GameObject 加入 SkinnedMeshRenderer，并将此 SkinnedMeshRenderer 存入 targetSmr/////
				targetSmr.Add(InnerPartList[i] , partObj.AddComponent<SkinnedMeshRenderer>());
			}
		}

		if (target != null) {
			mTargetInitPosition = target.localPosition; 
			mTargetInitEular = target.localEulerAngles;
		}


		
		if (mSkinMeshBonesList.Count == 0)
		{
			Object obj = Globals.Instance.MResourceManager.Load("Config/SkinMeshedBones");
			
			TextAsset tText = (TextAsset)obj;
			string[] meshSkinList = tText.text.Split(new char[]{'\n'});
			const string majorDelimiter = "&";
			const string assignDelimiter = "=";
			for (int i=0; i<meshSkinList.Length;i++)
			{
				string oneLineStr = meshSkinList[i];
				string[] skinMeshNameBonesName = oneLineStr.Split(majorDelimiter.ToCharArray());
				if (skinMeshNameBonesName.Length == 2)
				{
					string skinMeshName = skinMeshNameBonesName[0];
					string boneNames = skinMeshNameBonesName[1];
					string [] boneNamesArray = boneNames.Split(assignDelimiter.ToCharArray());
					List<string> boneNameList = new List<string>();
					for (int j=0; j<boneNamesArray.Length; j++)
					{
						if (boneNamesArray[j] != "" && boneNamesArray[j] != "\r" )
							boneNameList.Add(boneNamesArray[j] );
					}
					mSkinMeshBonesList.Add(skinMeshName,boneNameList);
				}
			}
		}


	}
	
	public void  generateCharacterFromConfig(int sexGender ,string animationName,string appearance="",string cloths="")
	{		
		if (mGender != sexGender)
		{
			foreach(SkinnedMeshRenderer skmr in targetSmr.Values)
			{
				GameObject.DestroyObject(skmr.gameObject);
			}

			if (target != null)
			{
				GameObject.DestroyObject(target.gameObject);

				targetSmr.Clear();
			
				mModelBoneInfo.Clear();
				mToChangeBone.Clear();
				hips = null;
				
				Resources.UnloadUnusedAssets();
				System.GC.Collect();
			}
		}

		if (4 < mCanSeeBaraUnderWearLevel) {
			mCanSeeBaraUnderWear = false;
		} else {
			mCanSeeBaraUnderWear = true;
		}

		this.InitBoneInfoFromGender(sexGender);
		mGender = sexGender;
		CharacterConfig characterConfig = Globals.Instance.MDataTableManager.GetConfig<CharacterConfig>();
		CharacterConfig.CharacterObject characterObj = new CharacterConfig.CharacterObject();
		int characterId = 1217001000 + sexGender;
		characterConfig.GetCharacterObject(characterId,out characterObj);
		
		randomIdleList =  characterObj.RandomIdleList;
		
		mBodyColor = characterObj.BodyColor;
				
		ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
		ItemConfig.ItemElement element = null;
		
		///20-25 肉体 ////
		for (int i =14; i<=19; i++)
		{
			string partModelName = characterObj.PartInfoList[i].partModelName;
			if (partModelName != "" && partModelName != "0")
			{
				string partName = partModelName.Substring(0,partModelName.Length - 3);				
				string partIDStr = partModelName.Substring(partModelName.Length -3,3);
				
				ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false);
			}
		}
		
		
		Dictionary<int,int> clothDatas = new Dictionary<int, int>();
		
		while(cloths.Contains("["))
		{
			cloths = cloths.Replace("[","");
		}
		
		while(cloths.Contains("]"))
		{
			cloths = cloths.Replace("]","");
		}
		
		string[] sectionsParam = cloths.Split(',','|');
		foreach (string sectionsStr in sectionsParam)
		{
			int clothItemID = StrParser.ParseDecInt(sectionsStr,-1);
			bool IsHas = config.GetItemElement(clothItemID, out element);
			if (!IsHas){
				continue;
			}
			clothDatas.Add(element.ItemSmallCategory,clothItemID);
		}
		
		///1-13 头发和衣服//
		for (int i=0; i<14; i++)
		{
			int  itemClothID = 0 ;
			clothDatas.TryGetValue(characterObj.PartInfoList[i].partID,out itemClothID);
			if (itemClothID != 0)
			{	
				bool IsHas1 = config.GetItemElement(itemClothID, out element);
				if (!IsHas1){
					return;
				}
				string partModelName1 = element.ModelName;
				if (partModelName1 != "" && partModelName1 != "0")
				{
					string partName = partModelName1.Substring(0,partModelName1.Length - 3);				
					string partIDStr = partModelName1.Substring(partModelName1.Length -3,3);
					
					ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,element.OutTextureName0,element.OutTextureName1,element.OutColor);
				}
			}
			else
			{
				string partModelName = characterObj.PartInfoList[i].partModelName;
				if (partModelName != "" && partModelName != "0")
				{
					string partName = partModelName.Substring(0,partModelName.Length - 3);				
					string partIDStr = partModelName.Substring(partModelName.Length -3,3);
					
					bool IsHas = config.GetItemElement(characterObj.PartInfoList[i].ItemID, out element);
					string outTextureName0 = "";
					string outTextureName1 = "";
					string outColor = "";
					if (IsHas)
					{
						outTextureName0 = element.OutTextureName0;
						outTextureName1 = element.OutTextureName1;
						outColor = element.OutColor;
					}
					ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,outTextureName0,outTextureName1,outColor);
				}
				else
				{
					bool IsHas = config.GetItemElement(characterObj.PartInfoList[i].ItemID, out element);
					string outTextureName0 = "";
					string outTextureName1 = "";
					string outColor = "";
					if (IsHas)
					{
						partModelName = element.ModelName;
						string partName = partModelName.Substring(0,partModelName.Length - 3);				
						string partIDStr = partModelName.Substring(partModelName.Length -3,3);
						outTextureName0 = element.OutTextureName0;
						outTextureName1 = element.OutTextureName1;
						outColor = element.OutColor;
						ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,outTextureName0,outTextureName1,outColor);
					}
				}
			}		
			
		}
			
	
		mCharacterConfigId = characterId;
		
		mInitHairCutItemID = characterObj.PartInfoList[0].ItemID;
		
		animator = target.gameObject.GetComponent<Animator>();
		animator.enabled = true;
		

		if(!mFaceRenderTexture)
		{
//			mFaceRenderTexture =  new RenderTexture( (int)512,(int)512,0);
//			mFaceRenderTexture.Create();
			string load = "Character/Textures/" + characterObj.PartInfoList[14].partModelName;
			mFaceRenderTexture = Resources.Load (load, typeof(Texture)) as Texture;
		}
		

//		mFaceCamera.targetTexture = mFaceRenderTexture;
		
		int materialCount = targetSmr["TAXA"].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			string testStrName = targetSmr["TAXA"].materials[i].name;
			if (targetSmr["TAXA"].materials[i].name.Contains("TAXA001 (Instance)"))
			{
				targetSmr["TAXA"].sharedMaterials[i].SetTexture("_MainTex",mFaceRenderTexture);
			}
		}
		
//		mFaceCamera.rect = new Rect(0,0,1,1);
		
		
		PlayerImageData playerImageData = new PlayerImageData();
		playerImageData.ParserCustomData(appearance);
		Dictionary<int,float> boneCustomValues = playerImageData.getCustomValues();
		for (int i=0; i<boneCustomValues.Count; i++)
		{
			this.changeCustomBone(i*2 + mGender,boneCustomValues[i]);
		}
		
		ChangeBodyColor();
		
		for(int i = 1;i < InnerPartList.Length; ++i)
		{
			string partName = InnerPartList[i];
			if (mGender == (int) PlayerGender.GENDER_DOG)
			{
				TagMaskDefine.SetTagRecuisively(target.Find(InnerPartList[i]).gameObject, TagMaskDefine.GFAN_NPC);

				BoxCollider coll = target.Find(InnerPartList[i]).gameObject.GetComponent<BoxCollider>() as BoxCollider;
				if (null == coll) coll = target.Find(InnerPartList[i]).gameObject.AddComponent<BoxCollider>() as BoxCollider;
			}

		}
		
		
		
		if (mGender == (int) PlayerGender.GENDER_DOG)
		{
			NGUITools.SetActive(mFaceCamera.gameObject, false);
		}
		else
		{
			NGUITools.SetActive(mFaceCamera.gameObject, true);
		}



	}
	
	public void generageCharacterFormPlayerData(PlayerData actorData)
	{
		if (mGender != (int)actorData.BasicData.Gender)
		{
			foreach(SkinnedMeshRenderer skmr in targetSmr.Values)
			{
				GameObject.DestroyObject(skmr.gameObject);
			}

			GameObject.DestroyObject(target.gameObject);

			targetSmr.Clear();
			
			mModelBoneInfo.Clear();
			mToChangeBone.Clear();
			hips = null;
			
			Resources.UnloadUnusedAssets();
			System.GC.Collect();


		}
		//没有亲密度  暂时不开放
		if (4 < mCanSeeBaraUnderWearLevel) {
			mCanSeeBaraUnderWear = false;
		} else {
			mCanSeeBaraUnderWear = true;
		}
		mGender = (int)actorData.BasicData.Gender;
		PlayerImageData playerImageData = new PlayerImageData();
		playerImageData.ParserCustomData(actorData.starData.nRoleAppearance);
		mBodyColor = playerImageData.nBodyColor;

		this.InitBoneInfoFromGender(mGender);
		int templateID = 1217001000 + mGender;
		CharacterConfig characterConfig = Globals.Instance.MDataTableManager.GetConfig<CharacterConfig>();
		CharacterConfig.CharacterObject characterObj = new CharacterConfig.CharacterObject();
		characterConfig.GetCharacterObject(templateID,out characterObj);
		
		randomIdleList =  characterObj.RandomIdleList;
						
		///20-25 肉体 ////
		for (int i =14; i<=19; i++)
		{
			string partModelName = characterObj.PartInfoList[i].partModelName;
			if (partModelName != "" && partModelName != "0")
			{
				string partName = partModelName.Substring(0,partModelName.Length - 3);				
				string partIDStr = partModelName.Substring(partModelName.Length -3,3);
				
				ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false);
			}
		}
		///2-13 内衣 要先特殊处理一下因为加了好感度不到5级，不能让看到内衣//
		for (int i = 5; i < 7; i++) {
			if (i == (int)ChangePartType.NEIKU) {
				continue;
			}
			ItemSlotData itemSlotData = null ;
			actorData.ClothDatas.TryGetValue(characterObj.PartInfoList[i].partID,out itemSlotData);
			if (itemSlotData != null) {
				ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
				ItemConfig.ItemElement element = null;
				bool IsHas = config.GetItemElement(itemSlotData.MItemData.BasicData.LogicID, out element);
				if (!IsHas){
					return;
				}
				if (element == null) {
					break;
				}
				string partModelName = element.ModelName;
				if (partModelName != "" && partModelName != "0") {
					string partName = partModelName.Substring (0, partModelName.Length - 3);				
					string partIDStr = partModelName.Substring (partModelName.Length - 3, 3);
					ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,element.OutTextureName0,element.OutTextureName1,element.OutColor,false);
				}
			}
		}
		
		///1-13 头发,衣服和脸装//
		for (int i=0; i<=13; i++)
		{
			if (i == (int)ChangePartType.NEIYI || i == (int)ChangePartType.NEIKU || i==(int)ChangePartType.SOCKET)
			{
				continue;
			}
			if (characterObj.PartInfoList [i].partID == 6)
				continue;
			ItemSlotData itemSlotData = null ;
			actorData.ClothDatas.TryGetValue(characterObj.PartInfoList[i].partID,out itemSlotData);
			if (itemSlotData != null)
			{
				ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
				
				ItemConfig.ItemElement element = null;
				bool IsHas = config.GetItemElement(itemSlotData.MItemData.BasicData.LogicID, out element);
				if (!IsHas){
					return;
				}
				if(element.nItemMaterial == 2 && element.ItemSmallCategory == 12){

				}
				string partModelName = element.ModelName;
				if (partModelName != "" && partModelName != "0")
				{
					string partName = partModelName.Substring(0,partModelName.Length - 3);				
					string partIDStr = partModelName.Substring(partModelName.Length -3,3);
					if(element.nItemMaterial == 2 && element.ItemSmallCategory == 12){
						
						ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,element.OutTextureName0,element.OutTextureName1,element.OutColor,true);
					}else{
						ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,element.OutTextureName0,element.OutTextureName1,element.OutColor);
					}
				}
			}
		}

		for (int i=(int)ChangePartType.NEIYI; i<=(int)ChangePartType.SOCKET; i++) 
		{
			ItemSlotData itemSlotData = null ;
			actorData.ClothDatas.TryGetValue(characterObj.PartInfoList[i].partID,out itemSlotData);
			if (itemSlotData != null)
			{
				ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
				
				ItemConfig.ItemElement element = null;
				bool IsHas = config.GetItemElement(itemSlotData.MItemData.BasicData.LogicID, out element);
				if (!IsHas){
					return;
				}
				string partModelName = element.ModelName;
				if (partModelName != "" && partModelName != "0")
				{
					string partName = partModelName.Substring(0,partModelName.Length - 3);				
					string partIDStr = partModelName.Substring(partModelName.Length -3,3);
					
					ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,element.OutTextureName0,element.OutTextureName1,element.OutColor);
				}
			}
		}

		///饰品//
		for (int assessoryIter=0; assessoryIter<AccessoryPartID.Length; assessoryIter++)
		{
			int partID = AccessoryPartID[assessoryIter];
			ItemSlotData itemSlotData = null ;
			actorData.ClothDatas.TryGetValue(partID,out itemSlotData);
			if (itemSlotData != null)
			{
				ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
				
				ItemConfig.ItemElement element = null;
				bool IsHas = config.GetItemElement(itemSlotData.MItemData.BasicData.LogicID, out element);
				if (!IsHas){
					return;
				}
				string partModelName = element.ModelName;
				if (partModelName != "" && partModelName != "0")
				{
					string partName = partModelName.Substring(0,partModelName.Length - 3);				
					string partIDStr = partModelName.Substring(partModelName.Length -3,3);
					
					ChangePart(partName,partIDStr,partID,false,element.OutTextureName0,element.OutTextureName1,element.OutColor);
				}
			}
		}
	
		mCharacterConfigId = templateID;
	
		animator = target.gameObject.GetComponent<Animator>();
		animator.enabled = true;
		
		if(!mFaceRenderTexture)
		{
//			mFaceRenderTexture =  new RenderTexture( (int)512,(int)512, 0);
//			mFaceRenderTexture.Create();
			string load = "Character/Textures/" + characterObj.PartInfoList[14].partModelName;
			mFaceRenderTexture = Resources.Load (load, typeof(Texture)) as Texture;
		}
		
//		mFaceCamera.targetTexture = mFaceRenderTexture;
		
		paintFaceTexture();
		
//		mFaceCamera.rect = new Rect(0,0,1,1);
		
		
		

		Dictionary<int,float> boneCustomValues = playerImageData.getCustomValues();
		for (int i=0; i<boneCustomValues.Count; i++)
		{
			this.changeCustomBone(i*2 + mGender,boneCustomValues[i]);
		}
		

		
		TagMaskDefine.SetTagRecuisively(mBip001Spine1.parent.gameObject, TagMaskDefine.GFAN_ACTOR);
	
		ChangeBodyColor();
			
	}
	
	public void generageCharacterOtherPlayer(int gender,string appearance,string cloths)
	{
		mGender = gender;
		this.InitBoneInfoFromGender(mGender);
		int templateID = 1217001000 + mGender;
		CharacterConfig characterConfig = Globals.Instance.MDataTableManager.GetConfig<CharacterConfig>();
		CharacterConfig.CharacterObject characterObj = new CharacterConfig.CharacterObject();
		characterConfig.GetCharacterObject(templateID,out characterObj);
		
		randomIdleList =  characterObj.RandomIdleList;
		
		mBodyColor = characterObj.BodyColor;
		
		///20-25 肉体 ////
		for (int i =14; i<=19; i++)
		{
			string partModelName = characterObj.PartInfoList[i].partModelName;
			if (partModelName != "" && partModelName != "0")
			{
				string partName = partModelName.Substring(0,partModelName.Length - 3);				
				string partIDStr = partModelName.Substring(partModelName.Length -3,3);
				
				ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false);
			}
		}
		
		ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
		ItemConfig.ItemElement element = null;
		Dictionary<int,int> clothDatas = new Dictionary<int, int>();
		
		while(cloths.Contains("["))
		{
			cloths = cloths.Replace("[","");
		}
		
		while(cloths.Contains("]"))
		{
			cloths = cloths.Replace("]","");
		}
		
		string[] sectionsParam = cloths.Split(',','|');
		foreach (string sectionsStr in sectionsParam)
		{
			int clothItemID = StrParser.ParseDecInt(sectionsStr,-1);
			bool IsHas = config.GetItemElement(clothItemID, out element);
			if (!IsHas){
				continue;
			}
			clothDatas.Add(element.ItemSmallCategory,clothItemID);
		}
		
		///1-13 头发,衣服和脸装//
		for (int i=0; i<=13; i++)
		{
			if (i == (int)ChangePartType.NEIYI || i == (int)ChangePartType.NEIKU || i==(int)ChangePartType.SOCKET)
			{
				continue;
			}
			int  itemClothID = 0 ;
			clothDatas.TryGetValue(characterObj.PartInfoList[i].partID,out itemClothID);
			if (itemClothID != 0)
			{				
				bool IsHas = config.GetItemElement(itemClothID, out element);
				if (!IsHas){
					return;
				}
				string partModelName = element.ModelName;
				if (partModelName != "" && partModelName != "0")
				{
					string partName = partModelName.Substring(0,partModelName.Length - 3);				
					string partIDStr = partModelName.Substring(partModelName.Length -3,3);
					
					ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,element.OutTextureName0,element.OutTextureName1,element.OutColor);
				}
			}
		}

		for (int i=(int)ChangePartType.NEIYI; i<=(int)ChangePartType.SOCKET; i++) 
		{
			int  itemClothID = 0 ;
			clothDatas.TryGetValue(characterObj.PartInfoList[i].partID,out itemClothID);
			if (itemClothID != 0)
			{				
				bool IsHas = config.GetItemElement(itemClothID, out element);
				if (!IsHas){
					return;
				}
				string partModelName = element.ModelName;
				if (partModelName != "" && partModelName != "0")
				{
					string partName = partModelName.Substring(0,partModelName.Length - 3);				
					string partIDStr = partModelName.Substring(partModelName.Length -3,3);
					
					ChangePart(partName,partIDStr,characterObj.PartInfoList[i].partID,false,element.OutTextureName0,element.OutTextureName1,element.OutColor);
				}
			}
		}
			
	
		mCharacterConfigId = templateID;
		
		animator = target.gameObject.GetComponent<Animator>();
		animator.enabled = true;
		
		if(!mFaceRenderTexture)
		{
//			mFaceRenderTexture =  new RenderTexture( (int)512,(int)512,0);
//			mFaceRenderTexture.Create();
			string load = "Character/Textures/" + characterObj.PartInfoList[14].partModelName;
			mFaceRenderTexture = Resources.Load (load, typeof(Texture)) as Texture;
		}
		
//		mFaceCamera.targetTexture = mFaceRenderTexture;
		
		int materialCount = targetSmr["TAXA"].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			string testStrName = targetSmr["TAXA"].materials[i].name;
			if (targetSmr["TAXA"].materials[i].name.Contains("TAXA001 (Instance)"))
			{
				targetSmr["TAXA"].sharedMaterials[i].SetTexture("_MainTex",mFaceRenderTexture);
			}
		}
		
//		mFaceCamera.rect = new Rect(0,0,1,1);
		
		
		PlayerImageData playerImageData = new PlayerImageData();
		playerImageData.ParserCustomData(appearance);
		Dictionary<int,float> boneCustomValues = playerImageData.getCustomValues();
		for (int i=0; i<boneCustomValues.Count; i++)
		{
			this.changeCustomBone(i*2 + mGender,boneCustomValues[i]);
		}
		
		mBodyColor = playerImageData.nBodyColor;
		
		ChangeBodyColor();

	}
	
//	void LateUpdate()
//	{
//		if (!mEyeTracing)
//			return;
//		Camera MyCamera = Globals.Instance.MSceneManager.mTaskCamera;
//		MyCamera.gameObject.SetActive(true);
//		Transform MyCameratransform = MyCamera.transform;	 
//		//Quaternion tempQuaternion = MyCameratransform.rotation * direction;
//		
//		Vector3 Current = (mEyeLeftTransform.rotation * Vector3.right).normalized;
//		Vector3 TargetL = (mEyeLeftTransform.position -  MyCameratransform.position   ).normalized;
//		
//		//Quaternion Change = Quaternion.FromToRotation(Current,Target);
//		// mEyeLeftTransform.LookAt(mEyeLeftTransform.position + MyCameratransform.forward);
//		//mEyeLeftTransform.localEulerAngles = new Vector3(mEyeLeftTransform.localEulerAngles.x,mEyeLeftTransform.localEulerAngles.y + 270,mEyeLeftTransform.localEulerAngles.z);
//		//mEyeLeftTransform.LookAt(MyCameratransform.position);
//		//mEyeRightTraansform.LookAt(MyCameratransform.position);
//		
//	
//		
//		Vector3 TargetR = (mEyeRightTraansform.position -  MyCameratransform.position   ).normalized;
//		Quaternion CorrectR = new Quaternion();
//		CorrectR.eulerAngles = new Vector3(0,90,0);
//		TargetR = CorrectR * TargetR;
//		mEyeRightTraansform.forward = -TargetR;
//		
//		Quaternion CorrectL = new Quaternion();
//		CorrectL.eulerAngles = new Vector3(0,90,0);
//		TargetL = CorrectL * TargetL;
//		//TargetR = CorrectR * TargetR;
//		//Vector3 newVec3 = new Vector3(-TargetL.x,TargetL.y,TargetL.z);
//		mEyeLeftTransform.forward = -TargetL  ;
//		
//	    
//		//mEyeRightTraansform.localEulerAngles = new Vector3(mEyeRightTraansform.localEulerAngles.x-6,mEyeRightTraansform.localEulerAngles.y,mEyeRightTraansform.localEulerAngles.z+10);
//		mEyeLeftTransform.Rotate(Vector3.right,180,Space.Self);
//		
//		//mEyeLeftTransform.localEulerAngles = new Vector3(358.5f,347.7f,288.7f);
//		//mEyeLeftTransform.localEulerAngles = new Vector3(mEyeLeftTransform.localEulerAngles.x,mEyeLeftTransform.localEulerAngles.y - 180,mEyeLeftTransform.localEulerAngles.z);
//		//mEyeLeftTransform.rotation = tempQuaternion;
//		//mEyeRightTraansform.rotation =  tempQuaternion;
//		
//		if(mHead.eulerAngles.y >= 60 && mHead.eulerAngles.y <= 120)
//		{
//			LQuaternion = 	mEyeLeftTransform.localRotation;
//			RQuaternion =   mEyeRightTraansform.localRotation;
//		}
//		else 
//		{
//			mEyeLeftTransform.localRotation = LQuaternion;
//			mEyeRightTraansform.localRotation = RQuaternion;
//		}
//	//	
//		
//	}


	
	private int converterToAnimaterIndex(string animationName)
	{
		animationName = animationName.Substring(1,animationName.Length -1);
		return StrParser.ParseDecInt(animationName,-1);
	}
	
	public void changeCharacterAnimation(int animationIndex)
	{
		if (animator == null)
			return;
		if (animator.runtimeAnimatorController == null)
			return;
		if (animator.runtimeAnimatorController.name.Contains("Clothes"))
		{
			Debug.Log("index is " + animationIndex);
			animator.SetInteger("index",animationIndex);
		}
	}
	
	public Animator getCharacterAnimator()
	{
		if (animator == null)
		{
			animator = target.gameObject.GetComponent<Animator>();
		}
		return animator;
	}
	
	public int getHairCutItemID()
	{
		return mInitHairCutItemID;
	}
	
	
	public void changeCharacterAnimationController(string ControllerName)
	{
		if (animator.runtimeAnimatorController == null || 
			(animator.runtimeAnimatorController != null && animator.runtimeAnimatorController.name != ControllerName))
		{
			target.localPosition = mTargetInitPosition ;
			target.localEulerAngles = mTargetInitEular;
			
			int index = ControllerName.IndexOf("_");
			if(-1 == index)
				return;
			string ControllIndex = ControllerName.Substring(0,index);
			
			Object aControllerObj = Resources.Load("Character/Controll/" + ControllIndex + "/" + ControllerName,typeof(Object)) as Object;
			RuntimeAnimatorController animationController  = (RuntimeAnimatorController)aControllerObj; 
			if (animationController == null)
				return;
			animator.runtimeAnimatorController = animationController;
			AnimatorStateInfo  animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			mFinishedCallBack = true;
			timepassed = 0.0f;
			
		}

	}
	
	public void ChangePart(int itemLogicID)
	{
		ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
		ItemConfig.ItemElement element = null;
		bool IsHas = config.GetItemElement(itemLogicID, out element);
		if (!IsHas)
			return;
		//宠物套装///
		if (element.ItemSmallCategory == 61)
		{
			string partModelName = element.ModelName;
			string[] bodyPartList = partModelName.Split('#');
			//必须包含头,身,脚三个部位///
			if (bodyPartList.Length != 3)
				return;
			string partName = bodyPartList[0].Substring(0,bodyPartList[0].Length - 3);
			string partIDStr = bodyPartList[0].Substring(bodyPartList[0].Length -3,3);
			ChangePart(partName,partIDStr,20,true,"","","");
			partName = bodyPartList[1].Substring(0,bodyPartList[1].Length - 3);
			partIDStr = bodyPartList[1].Substring(bodyPartList[1].Length -3,3);
			ChangePart(partName,partIDStr,2,true,"","","");
			partName = bodyPartList[2].Substring(0,bodyPartList[2].Length - 3);
			partIDStr = bodyPartList[2].Substring(bodyPartList[2].Length -3,3);
			ChangePart(partName,partIDStr,5,true,"","","");
		}
		
	}
	
	/// <summary>
	/// Changes the part.
	/// </summary>
	/// <param name='part'>
	/// Part.
	/// </param>
	/// <param name='item'>
	/// Item.
	/// </param>
	/// <param name='partID'>
	/// Part I.
	/// </param>
	/// <param name='changePart'>
	///  换内衣时候是否切换身体模型
	/// <param name='outTexture1'>
	/// 将模型第一个层次的材质换成此贴图
	/// <param name='outTexture2'>
	/// 将模型第二个层次的材质换成此贴图
	/// </param>
	public  void ChangePart(string part , string item,int partID,bool changePart,string outTexture1 = "", string outTexture2 = "",string outColor = "", bool isYuyi = false)
	{
		Debug.Log("ChangePart is " + part + item);	
		string skinMeshNameStr = part + item;
		if (partID == 6)
			skinMeshNameStr = "TAXB001";
		if (partID == 7)
			skinMeshNameStr = "TAXC001";
		if (partID == 8)
			skinMeshNameStr = "TAXE001";

		if (partID == 9)
		{
			changeAccessoryModel(part,item);
			return ;
		}

		//从资料中取得各部位指定编号的 SkinnedMeshRenderer//
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/" + skinMeshNameStr) ;
		GameObject skinMeshGameObj =  GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject;
		List<string> boneList = new List<string>();
		mSkinMeshBonesList.TryGetValue(skinMeshNameStr,out boneList );
		switch (partID)
		{
		case 1 : //帽子//
		case 2 : //上衣//
		case 3 : //下装裙//
		case 4 : //手套//
		case 5 : //鞋子//
				
			SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
			List<Transform> bones = new List<Transform>();
			for (int bi=0; bi<boneList.Count; bi++){
				foreach(Transform hip in hips){
					if(hip.name != boneList[bi]) continue;
					bones.Add(hip);
					break;
				}
			}
			
			SkeletonBone skeletonBon = new SkeletonBone();
			
			
			string targetPartName = InnerPartList[partID -1]; 
			// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
			targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
			targetSmr[targetPartName].quality = SkinQuality.Bone4;
			targetSmr[targetPartName].updateWhenOffscreen = true;
			targetSmr[targetPartName].bones = bones.ToArray();
			targetSmr[targetPartName].sharedMaterials = smr.sharedMaterials;
			///换裙子的时候 要将腿部的裤子去下来///
			if (partID == 3 && changePart)
				makeTAXE();
			
			
			///换裙子的时候,如果穿着长裙，还要将上身裸下来///
			if (partID == 3 && changePart && mLongDressUP)
			{
				makeTAXB();
				if (mBaraTextureName != "")
					paintBaraTexture();
				mLongDressUP = false;
				if (!mCanSeeBaraUnderWear) {
					makeFake_TWXB999 ();
				}

			}


			if (partID == 2 && changePart && mBaraTextureName!="")
				paintBaraTexture();
			
			if (partID == 2 && changePart && mLongDressUP)
			{
				mLongDressUP = false;

				if (!mCanSeeBaraUnderWear) {
					makeFake_TWXS999 ();
				}
				if (mUnderTextureName != "") {
					paintUnderWearTexture ();
				}
			}

			if (partID == 2) {
				if (mBaraTextureName != "")
					paintBaraTexture ();
				if (mUnderTextureName != "")
					paintUnderWearTexture ();
			}
			
			if (outTexture1 != "")
				changeOutSideTexture(targetSmr[targetPartName],0,outTexture1);
			
			
			if (outTexture2 != "")
				changeOutSideTexture(targetSmr[targetPartName],1,outTexture2);
			
			
			if (partID == 1 && outColor != "" )
			{
				targetSmr[targetPartName].sharedMaterials[0].SetColor("_Color",StrParser.ParseColor(outColor));
			}
			
			
			break;
			
		case 10://长裤 腿部//
		case 24:
			smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
			
			bones = new List<Transform>();
			bones.Clear();
			for (int bi=0; bi<boneList.Count; bi++){
				foreach(Transform hip in hips){
					if(hip.name != boneList[bi]) continue;
					bones.Add(hip);
					break;
				}
			}
			
			targetPartName = "TAXE"; 
			// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
			targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
			targetSmr[targetPartName].quality = SkinQuality.Bone4;
			targetSmr[targetPartName].updateWhenOffscreen = true;
			targetSmr[targetPartName].bones = bones.ToArray();
			targetSmr[targetPartName].sharedMaterials = smr.sharedMaterials;
			//if (changePart)
				//makeTAXC_NULL();
			///当穿着长裙换裤子的时候///
			if (changePart && mLongDressUP)
			{
				makeTAXB();
				if (mBaraTextureName != "")
					paintBaraTexture();
				mLongDressUP = false;
				if (!mCanSeeBaraUnderWear) {
					makeFake_TWXB999 ();
				}
			}
			
			if (partID == 10 && changePart && mSockTextureName!="")
				paintSockTexture();
				
			if (partID == 10 )
			{
				mDownPantsWeared = true;
				makeTAXC_NULL();
					
				if (outTexture1 != "")
					changeOutSideTexture(targetSmr[targetPartName],0,outTexture1);
				
				if (outTexture2 != "")
					changeOutSideTexture(targetSmr[targetPartName],1,outTexture2);
			}
			
			break;
		case 25: ///脚底板///
			makeTAXG();
			break;
		case 20 : //脸///
			smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
			
			bones = new List<Transform>();
			bones.Clear();
			for (int bi=0; bi<boneList.Count; bi++){
				foreach(Transform hip in hips){
					if(hip!=null&&hip.name != boneList[bi]) continue;
					bones.Add(hip);
					break;
				}
			}
			
			targetPartName = "TAXA"; 
			// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
			targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
			targetSmr[targetPartName].quality = SkinQuality.Bone4;
			targetSmr[targetPartName].updateWhenOffscreen = true;
			targetSmr[targetPartName].bones = bones.ToArray();
			targetSmr[targetPartName].sharedMaterials = smr.sharedMaterials;
			
			break;
		case 21: ///裸上身//
			makeTAXB();
			break;
		case 22:///裸臀部//
			makeTAXC();
			break;
		case 23 ://手//
						
			smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
			
			bones = new List<Transform>();
			bones.Clear();
			for (int bi=0; bi<boneList.Count; bi++){
				foreach(Transform hip in hips){
					if(hip.name != boneList[bi]) continue;
					bones.Add(hip);
					break;
				}
			}
			
			targetPartName = "TAXD"; 
			// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
			targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
			targetSmr[targetPartName].quality = SkinQuality.Bone4;
			targetSmr[targetPartName].updateWhenOffscreen = true;
			targetSmr[targetPartName].bones = bones.ToArray();
			targetSmr[targetPartName].sharedMaterials = smr.sharedMaterials;
			break;
			
		case 6 : //内衣//
			if (!changePart)
			{		
				string braTextureName = part + item;
				string [] braTexs = braTextureName.Split('#');
				mBaraTextureName = braTexs[0];
				if(braTexs.Length>1)
					mUnderTextureName = braTexs[1];
				
				paintBaraTexture();
				
				if (mDownPantsWeared)
				{
					makeTAXE();
					makeTAXC();
					mDownPantsWeared = false;
				}
				paintUnderWearTexture();
						
			}
			else{
				string braTextureName = part + item;
				string [] braTexs = braTextureName.Split('#');
				mBaraTextureName = braTexs[0];
				if(braTexs.Length>1)
					mUnderTextureName = braTexs[1];

				part = "TAXB";
				item = "001";

				smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
				bones = new List<Transform>();
				bones.Clear();
				for (int bi=0; bi<boneList.Count; bi++){
					foreach(Transform hip in hips){
						if(hip.name != boneList[bi]) continue;
						bones.Add(hip);
						break;
					}
				}
				
				// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
				targetSmr[part].sharedMesh = smr.sharedMesh;
				targetSmr[part].quality = SkinQuality.Bone4;
				targetSmr[part].updateWhenOffscreen = true;
				targetSmr[part].bones = bones.ToArray();
				targetSmr[part].sharedMaterials = smr.sharedMaterials;
					
				paintBaraTexture();
				
				//if (mDownPantsWeared)
				{
					makeTAXE();
					makeTAXC();
					
					mDownPantsWeared = false;
				}
				paintUnderWearTexture();
				
			}
			if (!mCanSeeBaraUnderWear) {
				makeFake_TWXB999 ();
				makeFake_TWXS999 ();
			}
			break;
		case 8 ://袜子//
			mSockTextureName = part + item;
			paintSockTexture();
	
			break;
		case 14:
			makePart14 (part, item, partID, changePart, outTexture1, outTexture2, outColor);
			break;

		case 9 :
		case 15:
		case 16:
		case 17: ///饰品//
		
			break;
		case 13 ://热裤//
			smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
			
			bones = new List<Transform>();
			bones.Clear();
			for (int bi=0; bi<boneList.Count; bi++){
				foreach(Transform hip in hips){
					if(hip.name != boneList[bi]) continue;
					bones.Add(hip);
					break;
				}
			}
			
			targetPartName = "TAXC"; 
			// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
			targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
			targetSmr[targetPartName].quality = SkinQuality.Bone4;
			targetSmr[targetPartName].updateWhenOffscreen = true;
			targetSmr[targetPartName].bones = bones.ToArray();
			targetSmr[targetPartName].sharedMaterials = smr.sharedMaterials;
			
			///当穿着长裙换裤子的时候///
			if (changePart && mLongDressUP)
			{
				makeTAXB();
				if (mBaraTextureName != "")
					paintBaraTexture();
				mLongDressUP = false;
				if (!mCanSeeBaraUnderWear) {
					makeFake_TWXB999 ();
				}
			}
			
			//光腿//
			makeTAXE();
			
			if (outTexture1 != "")
				changeOutSideTexture(targetSmr[targetPartName],0,outTexture1);
			
			if (outTexture2 != "")
				changeOutSideTexture(targetSmr[targetPartName],1,outTexture2);
			
			break;
		case 11: //连体裤//
			makePart11(part,item,partID,changePart,outTexture1,outTexture2,outColor);
			break;
		case 12: //旗袍//			
			makePart12(part,item,partID,changePart,outTexture1,outTexture2,outColor,isYuyi);

			break;
			
		case 18: //脸部妆容///
			changeFaceCustomTexture(part+item);

			break;
			
		default:

			break;
		}

		GameObject.Destroy(skinMeshGameObj);

		ChangeBodyColor();
	}
	
	private void changeAccessoryModel(string part ,string item)
	{
		string modelNaml = part + item;
		
		AccessoryConfig accessoryConfig = Globals.Instance.MDataTableManager.GetConfig<AccessoryConfig>();
		AccessoryConfig.AccessoryObject accessoryObj = new AccessoryConfig.AccessoryObject();
		accessoryConfig.GetAccessoryObject(modelNaml,out accessoryObj);
		
		List<Transform> bones = new List<Transform>();
		bones.Clear();
		foreach(Transform hip in hips){
			if(hip.name.Contains(accessoryObj.BoneName))
				bones.Add(hip);
		}
		
		GameObject tSrcGameObj = Resources.Load("Character/Model/" + modelNaml,typeof(GameObject)) as GameObject;
		GameObject go = GameObject.Instantiate(tSrcGameObj) as GameObject;
		
		mAccessorGameObjectList.Add(go);
		
		go.GetComponent<MeshFilter>().mesh = tSrcGameObj.GetComponent<MeshFilter>().sharedMesh;
		go.GetComponent<MeshRenderer>().materials = tSrcGameObj.GetComponent<MeshRenderer>().sharedMaterials;
		go.transform.parent = bones[0].transform;
		go.transform.localPosition = accessoryObj.Position;
		go.transform.localEulerAngles = accessoryObj.Oritation;
		go.transform.localScale = accessoryObj.Scale;

	}


	
	private void ChangeBodyColor()
	{
		for(int i = 1;i < InnerPartList.Length; ++i)
		{
			string bodyPart = InnerPartList[i];
			SkinnedMeshRenderer skinMeshRender = target.Find(bodyPart).gameObject.GetComponent<SkinnedMeshRenderer>() as SkinnedMeshRenderer;
			int materialCount = skinMeshRender.materials.GetLength(0);
			for (int j=0; j<materialCount; j++)
			{
				////手部现在和上身共用了一个材质///
				//if (bodyPart == "TAXD")
					//bodyPart = "TAXB";
				////脚部现在和下身共用了一个材质///
				if (bodyPart == "TAXG")
					bodyPart = "TAXE";
//				if (bodyPart == "TAXA")
//				{
//					if (mGender == (int) PlayerGender.GENDER_FEMALE ||
//					    mGender == (int) PlayerGender.GENDER_MALE )
//					{
//						mFaceGameObj.sharedMaterial.SetColor("_Color",mBodyColor);
//						continue;
//
//					}
//
//				}
				if (skinMeshRender.materials [j].name.Contains ("TAXA")||skinMeshRender.materials [j].name.Contains ("TAXD")||skinMeshRender.materials [j].name.Contains ("TAXE")) {
					skinMeshRender.sharedMaterials[j].SetColor("_Color", mBodyColor);
					skinMeshRender.sharedMaterials [j].SetColor ("_AmbColor", mBodyColor);
					continue;
				}
		
				if (skinMeshRender.materials[j].name.Contains(bodyPart) ||skinMeshRender.materials[j].name.Contains("TAXC"))
				{
					skinMeshRender.sharedMaterials[j].SetColor("_Color",mBodyColor);
//					StartCoroutine (WaitAFps (skinMeshRender.sharedMaterials[j], "_AmbColor",mBodyColor));
					skinMeshRender.sharedMaterials [j].SetColor ("_AmbColor", mBodyColor);
				}
			}
		}	
	}
//	IEnumerator WaitAFps (Material m , string c,Color bc) {
//		yield return new WaitForSeconds (0.0f);
//		m.SetColor (c, bc);
//	}
	public  void ChangeBodyColor(Color outColor)
	{
		for(int i = 1;i < InnerPartList.Length; ++i)
		{
			string bodyPart = InnerPartList[i];
			SkinnedMeshRenderer skinMeshRender = target.Find(bodyPart).gameObject.GetComponent<SkinnedMeshRenderer>() as SkinnedMeshRenderer;
			int materialCount = skinMeshRender.materials.GetLength(0);
			for (int j=0; j<materialCount; j++)
			{
				////手部现在和上身共用了一个材质///
				//if (bodyPart == "TAXD")
					//bodyPart = "TAXB";
				////脚部现在和下身共用了一个材质///
				if (bodyPart == "TAXG")
					bodyPart = "TAXE";
//				if (bodyPart == "TAXA")
//				{
//					if (mGender == (int) PlayerGender.GENDER_FEMALE ||
//					    mGender == (int) PlayerGender.GENDER_MALE )
//					{
//						mFaceGameObj.sharedMaterial.SetColor("_Color",outColor);
//						continue;
//					}
//				}
				if (skinMeshRender.materials [j].name.Contains ("TAXA")||skinMeshRender.materials [j].name.Contains ("TAXD")||skinMeshRender.materials [j].name.Contains ("TAXE")) {
					skinMeshRender.sharedMaterials[j].SetColor("_Color",outColor);
					skinMeshRender.sharedMaterials [j].SetColor ("_AmbColor", outColor);
					continue;
				}
				if (skinMeshRender.materials[j].name.Contains(bodyPart) ||skinMeshRender.materials[j].name.Contains("TAXC"))
				{
					skinMeshRender.sharedMaterials[j].SetColor("_Color",outColor);
					skinMeshRender.sharedMaterials [j].SetColor ("_AmbColor", mBodyColor);
//					StartCoroutine (WaitAFps (skinMeshRender.sharedMaterials[j], "_AmbColor",mBodyColor));
				}
			}
		}	
	}
	
	public Color GetBodyColor()
	{
		return mBodyColor;
	}
	
	public void SetBodyColor(Color color)
	{
		mBodyColor = color;
	}

	
	public void takeOffPart(string partoff , string itemoff,int partID,bool changePart)
	{
		switch (partID)
		{
		case 1 : //帽子//
		case 2 : //上衣//
		case 3 : //下装裙//
		case 4 : //手套//				
			string targetPartName = InnerPartList[partID -1]; 
			string part = targetPartName;
			string item = "001";

			//从资料中取得各部位指定编号的 SkinnedMeshRenderer//
			Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/" + part + item);
			GameObject skinMeshGameObj = GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject;
			SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
			
			List<Transform> bones = new List<Transform>();
			string skinMeshNameStr = part + item;
			List<string> boneList = new List<string>();
			mSkinMeshBonesList.TryGetValue(skinMeshNameStr,out boneList);
			for (int bi=0; bi<boneList.Count; bi++){
				foreach(Transform hip in hips){
					if(hip.name != boneList[bi]) continue;
					bones.Add(hip);
					break;
				}
			}	

			
			SkeletonBone skeletonBon = new SkeletonBone();
						
			// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
			targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
			targetSmr[targetPartName].quality = SkinQuality.Bone4;
			targetSmr[targetPartName].updateWhenOffscreen = true;
			targetSmr[targetPartName].bones = bones.ToArray();
			targetSmr[targetPartName].sharedMaterials = smr.sharedMaterials;
			
			if (partID == 2 && changePart && mBaraTextureName!="")
			{
				paintBaraTexture();
				if (!mCanSeeBaraUnderWear) {
					makeFake_TWXB999 ();
				}
			}
			if (partID == 3 && changePart && mUnderTextureName != "") {
				if (!mCanSeeBaraUnderWear) {
					makeFake_TWXS999 ();
				}
			}
		    if (mUnderTextureName != "")
			{
				paintUnderWearTexture();
			}
			GameObject.Destroy(skinMeshGameObj);
			break;
		case 5 : //鞋子//
			makeTAXG();
			break;			
		case 6 : //内衣//
			//makeTAXB();		
			break;
		case 7 ://内裤//
			//makeTAXC();
			break;
		case 8 ://袜子//
			mSockTextureName = "";
			string sockPartName = "TAXE";
			int materialCount = targetSmr[sockPartName].materials.GetLength(0);
			for (int i=0; i<materialCount; i++)
			{
				if (targetSmr[sockPartName].materials[i].name.Contains("TAXE"))
				{
					targetSmr[sockPartName].sharedMaterials[i].shader  = Shader.Find ("MMD/PMDMaterial-with-Outline-CullBack-NoCastShadow");
				}
			}
			if(null == targetSmr["TAXE"].sharedMesh)
			{
				makeTAXE();
			}
			if(null == targetSmr["TAXC"].sharedMesh)
			{
				makeTAXC();
				if (mUnderTextureName != "")
				{
					paintUnderWearTexture();
				}
			}
			break;
		
		case 10://长裤 腿部
			makeTAXE();
			makeTAXC();
			if (mUnderTextureName != "")
			{
				paintUnderWearTexture();
			}
			if (!mCanSeeBaraUnderWear) {
				makeFake_TWXS999 ();
			}
			mDownPantsWeared = false;
			break;
		case 13 ://超短裤//
			makeTAXC();
			if (mUnderTextureName != "")
			{
				paintUnderWearTexture();
			}
			if (!mCanSeeBaraUnderWear) {
				makeFake_TWXS999 ();
			}
			break;
			
		case 11://连体裤//
			makeTAXB();
			makeTAXC();
			makeTAXE();
			mLongDressUP = false; 
			
			if (mBaraTextureName != "")
			{
				paintBaraTexture();
			}
			if (mUnderTextureName != "")
			{
				paintUnderWearTexture();
			}
			if (!mCanSeeBaraUnderWear) {
				makeFake_TWXS999 ();
			}
			
			break;
		case 12: //旗袍 连衣裙//
		case 14:
			makeTAXB();
			mLongDressUP = false; 
			
			if (mBaraTextureName != "")
			{
				paintBaraTexture();
			}
			
     		if (mUnderTextureName != "")
				paintUnderWearTexture();

			if (!mCanSeeBaraUnderWear) {
				makeFake_TWXS999 ();
			}
			if (!mCanSeeBaraUnderWear) {
				makeFake_TWXB999 ();
			}
			break;
			
		case 18: //脸部妆容///
			changeFaceCustomTexture(HumanPaintOriginal[mGender]);
			break;
		case 9 :
	
		case 15:
		case 16:
		case 17: ///饰品//
			if (mAccessoryModelList.ContainsKey(partID))
			{
				string modelNaml = mAccessoryModelList[partID];
				if (modelNaml != "")
				{
					AccessoryConfig accessoryConfig = Globals.Instance.MDataTableManager.GetConfig<AccessoryConfig>();
					AccessoryConfig.AccessoryObject accessoryObj = new AccessoryConfig.AccessoryObject();
					accessoryConfig.GetAccessoryObject(modelNaml,out accessoryObj);

					List<Transform> mBoneList = new List<Transform>();
					mBoneList.Clear();
					for (int i=0; i<accessoryObj.AccessoryObjectInfoList.Count; i++)
					{
						string accBoneName = accessoryObj.AccessoryObjectInfoList[i].nBoneName;
						Transform accBone = null;
						foreach(Transform hip in hips)
						{
							if(hip.name == accBoneName)
							{
								accBone = hip;
								mBoneList.Add(accBone);
							}
							
						}
					}

					for (int i=0; i<mBoneList.Count; i++)
					{
						string localModelNaml = modelNaml + i.ToString();
						Transform boneTransform = mBoneList[i].Find(localModelNaml);
						if (boneTransform != null)
						{
							GameObject.Destroy(boneTransform.gameObject);
						}
					}


				}
				mAccessoryModelList[partID] = "";
			}
			break;
		default:

			break;
		}	
		
		ChangeBodyColor();
	}
	
	
	float timepassed = 0.0f;
	void Update()
	{
		//EyetrackerLR();
		if (!mAnimationOnState)
		{
			if (randomIdleList == null)
				return;
			timepassed += Time.deltaTime;
			if (timepassed >= 10.0f)
			{
				int randromIdleIter = Random.Range(0,randomIdleList.Count);
				this.changeCharacterAnimation(randomIdleList[randromIdleIter]);
				timepassed = 0.0f;
			}
		}
	
		
		if (mFinishedCallBack)
		{
			AnimatorStateInfo  animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);	
						
			if (mAnimatorLength == 0)
				mAnimatorLength = animatorStateInfo.length;
			timepassed += Time.deltaTime;
			//Debug.Log("timepassed is " + timepassed.ToString() + "::::mAnimatorLength"  + mAnimatorLength.ToString());
			if (timepassed >= mAnimatorLength)
			{
				mFinishedCallBack = false;
			}
		}
	}

	
	public Transform getCharacterT()
	{
		return target;
	}
	
	public void ResetFaceCustomBone()
	{
		if (hips != null)
		{
			BoneCustomConfig boneCustomConfig = Globals.Instance.MDataTableManager.GetConfig<BoneCustomConfig>();
			BoneCustomConfig.CustomElement customElement = null;
			/////EYE_CUSTOM_BIGS = 5, CUSTOM_END = 31;
			for (int i=5; i<31; i++)
			{
				bool hasItem = boneCustomConfig.GetItemElement(i*2 + mGender ,out customElement);
				if (!hasItem)
					return;
			
				for (int iBoneCount=0; iBoneCount<customElement.nBoneCount; iBoneCount++)
				{
					BoneCustomConfig.BoneItemInfo boneItemInfo = customElement.nBoneList[iBoneCount];
					string boneName = boneItemInfo.boneName;
					
					mToChangeBone[boneName].position = mModelBoneInfo[boneName].Position;
					mToChangeBone[boneName].localPosition = mModelBoneInfo[boneName].LocalPosition;
					mToChangeBone[boneName].localEulerAngles = mModelBoneInfo[boneName].LocalEular;
					mToChangeBone[boneName].localScale = mModelBoneInfo[boneName].LocalScale;
				}
			}
		}
	}
	

	public void SetActive(bool isShow)
	{
		NGUITools.SetActive(this.gameObject,isShow);
		if(!isShow)
		{
			if(target != null)
			{
				target.localPosition = mTargetInitPosition;
				target.localEulerAngles = mTargetInitEular;
			}
		}
	}
	/// <summary>
	/// 性别不变的换装等的重置
	/// </summary>//
	public void ResetCharacter()
	{
		if (hips != null)
		{
			foreach(Transform hip in hips){
				if(hip != null){
					hip.localScale = mModelBoneInfo[hip.name].LocalScale;
					hip.localEulerAngles = mModelBoneInfo[hip.name].LocalEular;
					hip.localPosition = mModelBoneInfo[hip.name].LocalPosition;
				}				
			}
		}

		if (target == null)
			return;
		
		target.localPosition = mTargetInitPosition;
		target.localEulerAngles = mTargetInitEular;
		foreach(SkinnedMeshRenderer skmr in targetSmr.Values)
		{
			skmr.sharedMesh = null;
		}
		mSockTextureName = "";
		
		mLongDressUP = false;
		mDownPantsWeared = false; 
	
		mBaraTextureName = "";
		mUnderTextureName = "";
	}
	
	private void makeTAXC()
	{	
		//屁股部分//
		string piguName = "TAXC001";
		if (mGender == (int) PlayerGender.GENDER_FEMALE)
			piguName = "TAXC001";
		else if (mGender == (int) PlayerGender.GENDER_MALE)
			piguName = "TAXC001";
		else if (mGender == (int) PlayerGender.GENDER_DOG)
			piguName = "TAXC001";
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/"+piguName);
		GameObject skinMeshGameObj = GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject;
		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();

		List<Transform> bones = new List<Transform>();
		bones.Clear();
		List<string> boneList = mSkinMeshBonesList[piguName];
		for (int bi=0; bi<boneList.Count; bi++){
			foreach(Transform hip in hips){
				if(hip.name != boneList[bi]) continue;
				bones.Add(hip);
				break;
			}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr["TAXC"].sharedMesh = smr.sharedMesh;
		targetSmr["TAXC"].quality = SkinQuality.Bone4;
		targetSmr["TAXC"].bones = bones.ToArray();
		targetSmr["TAXC"].updateWhenOffscreen = true;
		targetSmr["TAXC"].sharedMaterials = smr.sharedMaterials;
		GameObject.Destroy(skinMeshGameObj);
	}
	
	private void makeTAXC_NULL()
	{	//屁股部分//
		string piguName = "TAXC001";
		if (mGender == (int) PlayerGender.GENDER_FEMALE)
			piguName = "TAXC001";
		else if (mGender == (int) PlayerGender.GENDER_MALE)
			piguName = "TAXC001";
		else if (mGender == (int) PlayerGender.GENDER_DOG)
			piguName = "TAXC001";
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/" + piguName) ;
		GameObject skinMeshGameObj = GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject;
		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
		List<Transform> bones = new List<Transform>();
		bones.Clear();
		List<string> boneList = mSkinMeshBonesList[piguName];
		for (int bi=0; bi<boneList.Count; bi++){
			foreach(Transform hip in hips){
				if(hip.name != boneList[bi]) continue;
				bones.Add(hip);
				break;
			}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr["TAXC"].sharedMesh = null;

		GameObject.Destroy(skinMeshGameObj);

	}
	
	private void makeTAXE()
	{
		//光腿//
		string tuiName = "TAXE001";
		if (mGender == (int) PlayerGender.GENDER_FEMALE)
			tuiName = "TAXE001";
		else if (mGender == (int) PlayerGender.GENDER_MALE)
			tuiName = "TAXE001";
		else if (mGender == (int) PlayerGender.GENDER_DOG)
			tuiName = "TAXE001";
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/"+tuiName) ;

		GameObject skinMeshGameObj = GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject; 
		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
		List<Transform> bones = new List<Transform>();
		bones.Clear();
		List<string> boneList = mSkinMeshBonesList[tuiName];
		for (int bi=0; bi<boneList.Count; bi++){
			foreach(Transform hip in hips){
				if(hip.name != boneList[bi]) continue;
				bones.Add(hip);
				break;
			}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr["TAXE"].sharedMesh = smr.sharedMesh;
		targetSmr["TAXE"].quality = SkinQuality.Bone4;
		targetSmr["TAXE"].bones = bones.ToArray();
		targetSmr["TAXE"].updateWhenOffscreen = true;
		targetSmr["TAXE"].sharedMaterials = smr.sharedMaterials;
		
		if (mSockTextureName!="")
			paintSockTexture();

		GameObject.Destroy(skinMeshGameObj);
	}
	
	private void makeTAXE_NULL()
	{
		//光腿//
		targetSmr["TAXE"].sharedMesh = null;
	}
	
	private void makeTAXB()
	{
		//光上身//
		string shangShengName = "TAXB001";
		if (mGender == (int) PlayerGender.GENDER_FEMALE)
			shangShengName = "TAXB001";
		else if (mGender == (int) PlayerGender.GENDER_MALE)
			shangShengName = "TAXB001";
		else if (mGender == (int) PlayerGender.GENDER_DOG)
			shangShengName = "TAZB001";

		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/" + shangShengName);
		GameObject skinMeshGameObj = GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject; 
		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
		List<Transform> bones = new List<Transform>();
		bones.Clear();

		List<string> boneList = mSkinMeshBonesList[shangShengName];
		for (int bi=0; bi<boneList.Count; bi++){
			foreach(Transform hip in hips){
				if(hip.name != boneList[bi]) continue;
				bones.Add(hip);
				break;
			}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr["TAXB"].sharedMesh = smr.sharedMesh;
		targetSmr["TAXB"].quality = SkinQuality.Bone4;
		targetSmr["TAXB"].bones = bones.ToArray();
		targetSmr["TAXB"].sharedMaterials = smr.sharedMaterials;

		GameObject.Destroy(skinMeshGameObj);	}
	
	private void makeTAXG()
	{
		//光脚//
		string jiaoName = "TAXG001";
		if (mGender == (int) PlayerGender.GENDER_FEMALE)
			jiaoName = "TAXG001";
		else if (mGender == (int) PlayerGender.GENDER_MALE)
			jiaoName = "TAXG001";
		else if (mGender == (int) PlayerGender.GENDER_DOG)
			jiaoName = "TAXG001";
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/"+jiaoName);
		GameObject skinMeshGameObj = GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject; 
		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
		List<Transform> bones = new List<Transform>();
		bones.Clear();

		List<string> boneList = mSkinMeshBonesList[jiaoName];
		for (int bi=0; bi<boneList.Count; bi++){
			foreach(Transform hip in hips){
				if(hip.name != boneList[bi]) continue;
				bones.Add(hip);
				break;
			}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr["TAXG"].sharedMesh = smr.sharedMesh;
		targetSmr["TAXG"].quality = SkinQuality.Bone4;
		targetSmr["TAXG"].bones = bones.ToArray();
		targetSmr["TAXG"].updateWhenOffscreen = true;
		targetSmr["TAXG"].sharedMaterials = smr.sharedMaterials;
		
		if (mSockTextureName!="")
		{
			int materialCount = targetSmr["TAXG"].materials.GetLength(0);
			for (int i=0; i<materialCount; i++)
			{
				if (targetSmr["TAXG"].materials[i].name.Contains("TAXE"))
				{
					targetSmr["TAXG"].sharedMaterials[i].shader  = Shader.Find ("MyGirl/TwoTextureBlend");
					Texture t = Resources.Load("Character/Textures/" + mSockTextureName ,typeof(Texture)) as Texture;
					targetSmr["TAXG"].sharedMaterials[i].SetTexture("_BlendTex",t);
				}
			}
		}		

		GameObject.Destroy(skinMeshGameObj);
	}
	
	private void paintBaraTexture()
	{
		string	targetPartName = "TAXB";
		int materialCount = targetSmr[targetPartName].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			if (targetSmr[targetPartName].materials[i].name.Contains("TAXB"))
			{
				targetSmr[targetPartName].sharedMaterials[i].shader  = Shader.Find ("MyGirl/TwoTextureBlend");
				Texture t = Resources.Load("Character/Textures/" + mBaraTextureName ,typeof(Texture)) as Texture;
				targetSmr[targetPartName].sharedMaterials[i].SetTexture("_BlendTex",t);
			}
		}
	}
	
	private void paintUnderWearTexture()
	{
		string targetPartName = "TAXC";
		int materialCount = targetSmr[targetPartName].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			if (targetSmr[targetPartName].materials[i].name.Contains("TAXC"))
			{
				targetSmr[targetPartName].sharedMaterials[i].shader  = Shader.Find ("MyGirl/TwoTextureBlend");
				Texture t = Resources.Load("Character/Textures/" + mUnderTextureName,typeof(Texture)) as Texture;
				targetSmr[targetPartName].sharedMaterials[i].SetTexture("_BlendTex",t);
			}
		}
	}
	
	private void paintSockTexture()
	{
		string targetPartName = "TAXE";
		int materialCount = targetSmr[targetPartName].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			if (targetSmr[targetPartName].materials[i].name.Contains("TAXE"))
			{
				targetSmr[targetPartName].sharedMaterials[i].shader  = Shader.Find ("MyGirl/TwoTextureBlend");
				Texture t = Resources.Load("Character/Textures/" + mSockTextureName ,typeof(Texture)) as Texture;
				targetSmr[targetPartName].sharedMaterials[i].SetTexture("_BlendTex",t);
			}
		}
	}
	
	private void paintFaceTexture()
	{
		int materialCount = targetSmr["TAXA"].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			string testStrName = targetSmr["TAXA"].materials[i].name;
			if (targetSmr["TAXA"].materials[i].name.Contains("TAXA001 (Instance)"))
			{
				targetSmr["TAXA"].sharedMaterials[i].SetTexture("_MainTex",mFaceRenderTexture);
			}
		}
	}
	
	private void makePart11(string part , string item,int partID,bool changePart,string outTexture1 = "", string outTexture2 = "",string outColor = "")
	{
		string skinMeshNameStr = part + item;
		
		//从资料中取得各部位指定编号的 SkinnedMeshRenderer//
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/" + skinMeshNameStr) ;
		GameObject skinMeshGameObj =  GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject;
		List<string> boneList = new List<string>();
		mSkinMeshBonesList.TryGetValue(skinMeshNameStr,out boneList );
		
		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();
		
		List<Transform> bones = new List<Transform>();
		bones.Clear();
		for (int bi=0; bi<boneList.Count; bi++){
			foreach(Transform hip in hips){
				if(hip.name != boneList[bi]) continue;
				bones.Add(hip);
				break;
			}
		}
		string targetPartName = "TAXB"; 
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
		targetSmr[targetPartName].quality = SkinQuality.Bone4;
		targetSmr[targetPartName].updateWhenOffscreen = true;
		targetSmr[targetPartName].bones = bones.ToArray();
		targetSmr[targetPartName].sharedMaterials = smr.sharedMaterials;
		
		makeTAXC_NULL();
		makeTAXE_NULL();
		
		mLongDressUP = true;
		
		if (mBaraTextureName != "")
		{
			paintBaraTexture();
		}
		
		if (outTexture1 != "")
			changeOutSideTexture(targetSmr[targetPartName],0,outTexture1);
		
		if (outTexture2 != "")
			changeOutSideTexture(targetSmr[targetPartName],1,outTexture2);

		GameObject.Destroy(skinMeshGameObj);
	}
	
	private void makePart12(string part , string item,int partID,bool changePart,string outTexture1 = "", string outTexture2 = "",string outColor = "",bool _isYuyi = false)
	{

		string skinMeshNameStr = part + item;

		//从资料中取得各部位指定编号的 SkinnedMeshRenderer//
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/" + skinMeshNameStr) ;
		GameObject skinMeshGameObj =  GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject;
		List<string> boneList = new List<string>();
		mSkinMeshBonesList.TryGetValue(skinMeshNameStr,out boneList );

		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer>();

		List<Transform> bones = new List<Transform>();
		bones.Clear();
		for (int bi=0; bi<boneList.Count; bi++){
			foreach(Transform hip in hips){
				if(hip.name != boneList[bi]) continue;
				bones.Add(hip);
				break;
			}
		}

		string targetPartName = "TAXB"; 
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
		targetSmr[targetPartName].quality = SkinQuality.Bone4;
		targetSmr[targetPartName].updateWhenOffscreen = true;
		targetSmr[targetPartName].bones = bones.ToArray();
		targetSmr[targetPartName].sharedMaterials = smr.sharedMaterials;
	

		makeTAXC();
		makeTAXE();
	
		mLongDressUP = true;
		if(_isYuyi)
		{
			//穿长款是内衣内裤透明//
			int materialCount = targetSmr[targetPartName].materials.GetLength(0);
			for (int i=0; i<materialCount; i++)
			{
				if (targetSmr[targetPartName].materials[i].name.Contains("TAXB"))
				{
					targetSmr[targetPartName].materials[i].shader  =  Shader.Find("MyGirl/TwoTextureBlend") ;
					Texture t = Resources.Load("Character/Textures/" + "null" ,typeof(Texture)) as Texture;
					targetSmr[targetPartName].materials[i].SetTexture("_BlendTex",t);
				}
			}
			
			string targetPartName_ = "TAXC";
			int materialCount_ = targetSmr[targetPartName_].materials.GetLength(0);
			for (int i=0; i<materialCount_; i++)
			{
				if (targetSmr[targetPartName_].materials[i].name.Contains("TAXC"))
				{
					targetSmr[targetPartName_].materials[i].shader  = Shader.Find("MyGirl/TwoTextureBlend") ;
					Texture t = Resources.Load("Character/Textures/" + "null" ,typeof(Texture)) as Texture;
					targetSmr[targetPartName_].materials[i].SetTexture("_BlendTex",t);
				}
			}
		}
		else
		{
			if (mBaraTextureName != "")
				paintBaraTexture();
			if (mUnderTextureName != "")
				paintUnderWearTexture();
		}
//		if (mBaraTextureName != "")
//		{
//			paintBaraTexture();
//		}
//	
//	    if (mUnderTextureName != "")
//		{
//			paintUnderWearTexture();
//		}

		GameObject.Destroy(skinMeshGameObj);

		if (outTexture1 != "")
			changeOutSideTexture(targetSmr[targetPartName],0,outTexture1);

		if (outTexture2 != "")
			changeOutSideTexture(targetSmr[targetPartName],1,outTexture2);
	}


	private void makePart14 (string part, string item, int partID, bool changePart, string outTexture1 = "", string outTexture2 = "", string outColor = "")
	{

		string skinMeshNameStr = part + item;

		//从资料中取得各部位指定编号的 SkinnedMeshRenderer//
		UnityEngine.Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/" + skinMeshNameStr) ;
		GameObject skinMeshGameObj = GameObject.Instantiate (aModelSceneObj, Vector3.one, Quaternion.identity) as GameObject;
		List<string> boneList = new List<string> ();
		mSkinMeshBonesList.TryGetValue (skinMeshNameStr, out boneList);

		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer> ();

		List<Transform> bones = new List<Transform> ();
		bones.Clear ();
		for (int bi = 0; bi < boneList.Count; bi++) {
			foreach (Transform hip in hips) {
				if (hip.name != boneList [bi])
					continue;
				bones.Add (hip);
				break;
			}
		}

		string targetPartName = "TAXB"; 
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr [targetPartName].sharedMesh = smr.sharedMesh;
		targetSmr [targetPartName].quality = SkinQuality.Bone4;
		targetSmr [targetPartName].updateWhenOffscreen = true;
		targetSmr [targetPartName].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		targetSmr [targetPartName].useLightProbes = false;
		targetSmr [targetPartName].bones = bones.ToArray ();
		targetSmr [targetPartName].sharedMaterials = smr.sharedMaterials;


		makeTAXC ();
		makeTAXE ();

		mLongDressUP = true;

		//穿长款是内衣内裤透明//
		int materialCount = targetSmr [targetPartName].materials.GetLength (0);
		for (int i = 0; i < materialCount; i++) {
			if (targetSmr [targetPartName].materials [i].name.Contains ("TAXB")) {
				targetSmr [targetPartName].materials [i].shader = Shader.Find ("MyGirl/TwoTextureBlend");
				Texture t = Resources.Load ("Character/Textures/" + "null", typeof(Texture)) as Texture;
				targetSmr [targetPartName].materials [i].SetTexture ("_BlendTex", t);
			}
		}

		string targetPartName_ = "TAXC";
		int materialCount_ = targetSmr [targetPartName_].materials.GetLength (0);
		for (int i = 0; i < materialCount_; i++) {
			if (targetSmr [targetPartName_].materials [i].name.Contains ("TAXC")) {
				targetSmr [targetPartName_].materials [i].shader = Shader.Find ("MyGirl/TwoTextureBlend");
				Texture t = Resources.Load ("Character/Textures/" + "null", typeof(Texture)) as Texture;
				targetSmr [targetPartName_].materials [i].SetTexture ("_BlendTex", t);
			}
		}


		GameObject.Destroy (skinMeshGameObj);

		if (outTexture1 != "")
			changeOutSideTexture (targetSmr [targetPartName], 0, outTexture1);

		if (outTexture2 != "")
			changeOutSideTexture (targetSmr [targetPartName], 1, outTexture2);
	}
	
	public void setAnimationOneState(bool state)
	{
		mAnimationOnState = state;
	}
	
	public void changeOutSideTexture(SkinnedMeshRenderer skinnedMeshRender, int id,string textureName)
	{
		Texture t = Resources.Load("Character/Textures/" + textureName ,typeof(Texture)) as Texture;
		skinnedMeshRender.sharedMaterials[id].SetTexture("_MainTex",t);
		
	}
	
	public void changeFaceCustomTexture(string customFaceStr)
	{
		return;
		//eyes006:"155,155,155,255"#TAXA001_a001:"51,49,49,255"#TAXA001_b001:"122,65,65,255"#TAXA001_c001:"167,69,69,255"#TAXA001_d002:"244,63,63,255"
		string customParam = customFaceStr;
		string[] sectionsParam = customParam.Split('#');
		int index = 0;
		string textureName = "";
		string textureNameStr = "";
		Color  textureColor ;
		foreach (string sectionsStr in sectionsParam)
		{
			string[] keyValueParam = sectionsStr.Split(':');
			textureName = keyValueParam[0];
			if (keyValueParam.Length > 1)
				textureNameStr = keyValueParam[1];
			if (index == 0) ////美瞳
			{
				int materialCount = targetSmr["TAXA"].materials.GetLength(0);
				for (int i=0; i<materialCount; i++)
				{
					string testStrName = targetSmr["TAXA"].materials[i].name;
					if (targetSmr["TAXA"].materials[i].name.Contains("eyes001 (Instance)"))
					{
						Texture t = Resources.Load("Character/Textures/" + textureName ,typeof(Texture)) as Texture;
						targetSmr["TAXA"].sharedMaterials[i].SetTexture("_MainTex",t);
						if (textureNameStr != "")
						{
							textureColor = StrParser.ParseColor(textureNameStr);
							targetSmr["TAXA"].sharedMaterials[i].SetColor("_Color",textureColor);
						}
					}
				}
			}
			if (index == 1) ////眉毛
			{
				Texture t = Resources.Load("Character/Textures/" + textureName ,typeof(Texture)) as Texture;
				mEyeBrowGameObj.material  = Resources.Load("Character/Materials/" + "EyeBrow" ,typeof(Material)) as Material;
				mEyeBrowGameObj.material.SetTexture("_MainTex",t);
				if (textureNameStr != "")
				{
					textureColor = StrParser.ParseColor(textureNameStr);
					mEyeBrowGameObj.sharedMaterial.SetColor("_Color",textureColor);
				}
			}
			if (index == 2) ////眼影
			{
				Texture t = Resources.Load("Character/Textures/" + textureName ,typeof(Texture)) as Texture;
				mEyeShdowGameObj.material = Resources.Load("Character/Materials/" + "EyeShadow" ,typeof(Material)) as Material;
				mEyeShdowGameObj.sharedMaterial.SetTexture("_MainTex",t);
				if (textureNameStr != "")
				{
					textureColor = StrParser.ParseColor(textureNameStr);
					mEyeShdowGameObj.sharedMaterial.SetColor("_Color",textureColor);
				}
			}
			if (index == 3) ////腮红
			{
				Texture t = Resources.Load("Character/Textures/" + textureName ,typeof(Texture)) as Texture;
				mFaceRedGameObj.material = Resources.Load("Character/Materials/" + "FaceRed" ,typeof(Material)) as Material;
				mFaceRedGameObj.sharedMaterial.SetTexture("_MainTex",t);
				if (textureNameStr != "")
				{
					textureColor = StrParser.ParseColor(textureNameStr);
					mFaceRedGameObj.sharedMaterial.SetColor("_Color",textureColor);
				}
			}
			if (index == 4) ////唇彩
			{
				Texture t = Resources.Load("Character/Textures/" + textureName ,typeof(Texture)) as Texture;
				mMouseGameObj.material = Resources.Load("Character/Materials/" + "FaceMouse" ,typeof(Material)) as Material;
				mMouseGameObj.sharedMaterial.SetTexture("_MainTex",t);
				if (textureNameStr != "")
				{
					textureColor = StrParser.ParseColor(textureNameStr);
					mMouseGameObj.sharedMaterial.SetColor("_Color",textureColor);
				}
			}
			if (index == 5) ////睫毛
			{
				int materialCount = targetSmr["TAXA"].materials.GetLength(0);
				for (int i=0; i<materialCount; i++)
				{
					string testStrName = targetSmr["TAXA"].materials[i].name;
					if (targetSmr["TAXA"].materials[i].name.Contains("TWXJ002 (Instance)"))
					{
						Texture t = Resources.Load("Character/Textures/" + textureName ,typeof(Texture)) as Texture;
						targetSmr["TAXA"].sharedMaterials[i].SetTexture("_MainTex",t);
						if (textureNameStr != "")
						{
							textureColor = StrParser.ParseColor(textureNameStr);
							targetSmr["TAXA"].sharedMaterials[i].SetColor("_Color",textureColor);
						}
					}
				}
			}
			
			index ++;
		}
		
		paintFaceTexture();
	}
		

	private void makeFake_TWXB999 ()
	{
		//		UnityEngine.Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/TWXB999");
//		GameObject aModelSceneObj = PoolManager.inst.getCharacterPrefab ("TWXB999");
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/TWXB999" ) ;
		GameObject skinMeshGameObj = GameObject.Instantiate (aModelSceneObj, Vector3.one, Quaternion.identity) as GameObject; 
		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer> ();
		List<Transform> bones = new List<Transform> ();
		bones.Clear ();
		
		List<string> boneList = mSkinMeshBonesList ["TWXB999"];
		for (int bi = 0; bi < boneList.Count; bi++) {
			foreach (Transform hip in hips) {
				if (hip.name != boneList [bi])
					continue;
				bones.Add (hip);
				break;
			}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr ["TAXB"].sharedMesh = smr.sharedMesh;
		targetSmr ["TAXB"].quality = SkinQuality.Bone4;
		targetSmr ["TAXB"].bones = bones.ToArray ();
		targetSmr ["TAXB"].materials = smr.materials;
		
		GameObject.Destroy (skinMeshGameObj);
	}
	
	private void makeFake_TWXS999 ()
	{
		//		UnityEngine.Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/TWXS999");
		Object aModelSceneObj = Globals.Instance.MResourceManager.Load("Character/Prefabs/TWXS999" ) ;
//		GameObject aModelSceneObj = PoolManager.inst.getCharacterPrefab ("TWXS999");
		GameObject skinMeshGameObj = GameObject.Instantiate (aModelSceneObj, Vector3.one, Quaternion.identity) as GameObject; 
		SkinnedMeshRenderer smr = skinMeshGameObj.GetComponent<SkinnedMeshRenderer> ();
		List<Transform> bones = new List<Transform> ();
		bones.Clear ();
		
		List<string> boneList = mSkinMeshBonesList ["TWXS999"];
		for (int bi = 0; bi < boneList.Count; bi++) {
			foreach (Transform hip in hips) {
				if (hip.name != boneList [bi])
					continue;
				bones.Add (hip);
				break;
			}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr ["TAXC"].sharedMesh = smr.sharedMesh;
		targetSmr ["TAXC"].quality = SkinQuality.Bone4;
		targetSmr ["TAXC"].updateWhenOffscreen = true;
		targetSmr ["TAXC"].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		targetSmr ["TAXC"].useLightProbes = false;
		targetSmr ["TAXC"].bones = bones.ToArray ();
		targetSmr ["TAXC"].materials = smr.materials;
		
		GameObject.Destroy (skinMeshGameObj);
	}
	
	public void changeCustomBone(int boneCustomID,float sliderValue,float priorSliderValue=0)
	{
		BoneCustomConfig boneCustomConfig = Globals.Instance.MDataTableManager.GetConfig<BoneCustomConfig>();
		BoneCustomConfig.CustomElement customElement = null;
		bool hasItem = boneCustomConfig.GetItemElement(boneCustomID,out customElement);
		
		if (!hasItem)
			return ;
		Vector3 lossyScale1 = Vector3.one;
		Vector3 localScale1 = Vector3.one;
		
		Vector3 lossyScale5 = Vector3.one;
		Vector3 localScale5 = Vector3.one;
		
		float changeYf = 1f;
		float changeZf = 1f;
		
		if (boneCustomID == 6 || boneCustomID == 7)///腰围///
		{
			lossyScale5 = mBip001Spine2.lossyScale;
			localScale5 = mBip001Spine2.localScale;
			
			lossyScale1 = mBip001Spine1.lossyScale;
			localScale1 = mBip001Spine1.localScale;
			
		}
		
		if (boneCustomID == 8 || boneCustomID == 9 )///臀围///
		{
			lossyScale1 = mBip001Spine1.lossyScale;
			localScale1 = mBip001Spine1.localScale;
		}
		
		
		for (int i=0; i<customElement.nBoneCount; i++)
		{
			BoneCustomConfig.BoneItemInfo boneItemInfo = customElement.nBoneList[i];
			string boneName = boneItemInfo.boneName;
			float rangeValue = 1;
			if (sliderValue > 0)
				rangeValue = boneItemInfo.nBigValueRange;
			else 
				rangeValue = boneItemInfo.nValueRange;
			if (mToChangeBone.ContainsKey(boneName))
			{
				if (boneItemInfo.nPRS == (int)CustomBonePRSType.PositionType) ///位移操作///
				{
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.XType) 
					{
						if (boneItemInfo.nIsWorld == 0)
						{
							mToChangeBone[boneName].localPosition = new Vector3(mModelBoneInfo[boneName].LocalPosition.x + sliderValue*rangeValue,mToChangeBone[boneName].localPosition.y , mToChangeBone[boneName].localPosition.z);
						}
						else
						{
							mToChangeBone[boneName].position = mToChangeBone[boneName].position +  mToChangeBone[boneName].right * (sliderValue - priorSliderValue)*rangeValue;
						}
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.YType) 
					{
						if (boneItemInfo.nIsWorld == 0)
						{
							mToChangeBone[boneName].localPosition = new Vector3(mToChangeBone[boneName].localPosition.x,mModelBoneInfo[boneName].LocalPosition.y + sliderValue*rangeValue, mToChangeBone[boneName].localPosition.z);
						}
						else 
						{
							//Debug.Log("sliderValue is : " + sliderValue + "priorSliderValue is : " + priorSliderValue);
							mToChangeBone[boneName].position = mToChangeBone[boneName].position + new  Vector3(0,(sliderValue - priorSliderValue)*rangeValue , 0);
						}
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.ZType) 
					{
						if (boneItemInfo.nIsWorld == 0)
						    mToChangeBone[boneName].localPosition = new Vector3(mToChangeBone[boneName].localPosition.x,mToChangeBone[boneName].localPosition.y , mModelBoneInfo[boneName].LocalPosition.z  + sliderValue*rangeValue);
						else
						{
							//mToChangeBone[boneName].position = mToChangeBone[boneName].position +  mToChangeBone[boneName].right * (sliderValue - priorSliderValue)*rangeValue;
							mToChangeBone[boneName].Translate(new Vector3(0,0,sliderValue*rangeValue),Space.World);
						}
					}
				}
				else if (boneItemInfo.nPRS == (int)CustomBonePRSType.RotateType)///旋转操作///
				{
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.ZType) 
					{
						mToChangeBone[boneName].localEulerAngles = new Vector3(mToChangeBone[boneName].localEulerAngles.x,mToChangeBone[boneName].localEulerAngles.y, mModelBoneInfo[boneName].LocalEular.z  + sliderValue*rangeValue);
					}
					
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.XType) 
					{
						mToChangeBone[boneName].localEulerAngles = new Vector3(mModelBoneInfo[boneName].LocalEular.x  + sliderValue*rangeValue,mToChangeBone[boneName].localEulerAngles.y, mToChangeBone[boneName].localEulerAngles.z );
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.YType)
					{
						if (boneItemInfo.nIsWorld == 1)
							mToChangeBone[boneName].RotateAround(new Vector3(0,1,0),(sliderValue-priorSliderValue *rangeValue));
						else
							mToChangeBone[boneName].localEulerAngles = new Vector3(mToChangeBone[boneName].localEulerAngles.x,mModelBoneInfo[boneName].LocalEular.y + sliderValue*rangeValue,mToChangeBone[boneName].localEulerAngles.z);	
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.PLUSYType)
					{
						mToChangeBone[boneName].RotateAround(new Vector3(0,-1,0),(sliderValue-priorSliderValue *rangeValue));
					}
				}
				else if (boneItemInfo.nPRS == (int)CustomBonePRSType.ScaleType)///缩放操作///
				{
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.XType)
					{
						mToChangeBone[boneName].localScale =  new Vector3(mModelBoneInfo[boneName].LocalScale.x* (1 + sliderValue*rangeValue) ,mToChangeBone[boneName].localScale.y,mModelBoneInfo[boneName].LocalScale.z );
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.YType) 
					{
						mToChangeBone[boneName].localScale =  new Vector3(mToChangeBone[boneName].localScale.x ,mModelBoneInfo[boneName].LocalScale.y* (1 + sliderValue*rangeValue),mToChangeBone[boneName].localScale.z );	
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.ZType)
					{
						mToChangeBone[boneName].localScale =  new Vector3(mToChangeBone[boneName].localScale.x ,mToChangeBone[boneName].localScale.y,mModelBoneInfo[boneName].LocalScale.z * (1 + sliderValue*rangeValue));
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.XYType)
					{
						mToChangeBone[boneName].localScale =  new Vector3(mModelBoneInfo[boneName].LocalScale.x * (1 + sliderValue*rangeValue),mModelBoneInfo[boneName].LocalScale.y * (1 + sliderValue*rangeValue),mToChangeBone[boneName].localScale.z);
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.XZType)
					{
						mToChangeBone[boneName].localScale =  new Vector3(mModelBoneInfo[boneName].LocalScale.x* (1 + sliderValue*rangeValue) ,mToChangeBone[boneName].localScale.y,mModelBoneInfo[boneName].LocalScale.z * (1 + sliderValue*rangeValue));
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.YZType)
					{
						mToChangeBone[boneName].localScale =  new Vector3(mToChangeBone[boneName].localScale.x , mModelBoneInfo[boneName].LocalScale.y * (1 + sliderValue*rangeValue),mModelBoneInfo[boneName].LocalScale.z * (1 + sliderValue*rangeValue));
					}
					if (boneItemInfo.nAxis == (int)CustomBoneAxisType.XYZType)
					{
						mToChangeBone[boneName].localScale =  mModelBoneInfo[boneName].LocalScale * (1 + sliderValue*rangeValue);
					}
				}
			}
		}
		
		if (boneCustomID == 8 || boneCustomID == 9 )///臀围///
		{
			Transform parentTransform1 = mBip001Spine1.parent;
			mBip001Spine1.parent = Globals.Instance.MSceneManager.mTaskCamera.transform;
			mBip001Spine1.localScale = lossyScale1;
			mBip001Spine1.parent = parentTransform1;
			mBip001Spine1.localScale = new Vector3(localScale1.x,mBip001Spine1.localScale.y,mBip001Spine1.localScale.z);
			
			Transform parentTransform4 = mHead.parent;
			mHead.parent = Globals.Instance.MSceneManager.mTaskCameramControl.transform.parent;
			mHead.localScale = mHeadLossScale;
			mHead.parent = parentTransform4;

		}
		
		if (boneCustomID == 6 || boneCustomID == 7) ///腰围///
		{
			Transform parentTransform1 = mBip001Spine2.parent;
			mBip001Spine2.parent = Globals.Instance.MSceneManager.mTaskCameramControl.transform;
			mBip001Spine2.localScale = lossyScale5;
			mBip001Spine2.parent = parentTransform1;
			
			float changeXf = lossyScale1.x /parentTransform1.lossyScale.x;
			changeYf = lossyScale1.y /parentTransform1.lossyScale.y;
			changeZf = lossyScale1.z /parentTransform1.lossyScale.z;
			Debug.Log("changeXf is :" + changeXf);
			mBip001Spine2.localScale = new  Vector3(changeXf * localScale5.x, localScale5.y * changeYf, localScale5.z*changeZf);
			
			
		//	Transform parentTransform4 = mHead.parent;
			//mHead.parent = Globals.Instance.MSceneManager.mTaskCameramControl.transform.parent;
			//mHead.localScale = mHeadLossScale;
			//mHead.parent = parentTransform4;
			
		}
		
		if (boneCustomID == 0 || boneCustomID == 1 )///身高///
		{
			Transform parentTransform4 = mHead.parent;
			mHead.parent = Globals.Instance.MSceneManager.mTaskCameramControl.transform.parent;
			mHead.localScale = mHeadLossScale;
			mHead.parent = parentTransform4;
		}
		
		if (boneCustomID == 2 || boneCustomID == 3 )///体重///
		{
			Transform parentTransform5 = mHead.parent;
			mHead.parent = Globals.Instance.MSceneManager.mTaskCameramControl.transform.parent;
			mHead.localScale = mHeadLossScale;
			mHead.parent = parentTransform5;
			
		}
		
	}
	
	public Dictionary<string ,Transform> getAllBoneInfo()
	{
		return mToChangeBone;
	}
	

	
	public void parseAllBoneInfo(List<PositionOritation> prList)
	{
		if (prList.Count ==  hips.Length)
		{
			int i= 0;
			foreach(Transform hip in hips)
			{
				hip.localPosition = prList[i].position;
				hip.localEulerAngles = prList[i].oritation;
				i++;
			}
		}
	}
	
	public Transform getBoneTransformFormName(string boneName)
	{
		if (mToChangeBone.ContainsKey(boneName))
			return mToChangeBone[boneName];
		return null;
	}
	
	public void frozeBone(bool isFroze = true)
	{
		foreach(Transform hip in hips)
		{
			Rigidbody rigidbody = hip.GetComponent<Rigidbody>();
			if (rigidbody != null)
			{
				//if (isFroze)
				//	rigidbody.Sleep();
				//else
				//	rigidbody.WakeUp();
				//
				
				rigidbody.isKinematic = isFroze;
			}
				
		}

	}
	
	public void sleepBone(bool isSleep = true)
	{
		foreach(Transform hip in hips)
		{
			Rigidbody rigidbody = hip.GetComponent<Rigidbody>();
			if (rigidbody != null)
			{
				if (isSleep)
					rigidbody.Sleep();
				else
					rigidbody.WakeUp();
			}
		}

	}
}

public struct PositionOritation
{
	public Vector3 position;
	public Vector3 oritation;
}