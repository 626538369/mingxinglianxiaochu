using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


using Object=UnityEngine.Object;
using Random=UnityEngine.Random;

// This class can be used to create characters by combining assets.
// The assets are stored in assetbundles to minimize the assets that
// have to be downloaded.
public class CharacterGenerator : MonoBehaviour 
{
    // Stores the WWW used to retrieve available CharacterElements stored
    // in the CharacterElementDatabase assetbundle. When storing the available
    // CharacterElements in an assetbundle instead of a ScriptableObject 
    // referenced by a MonoBehaviour, changing the available CharacterElements
    // does not require a client rebuild.
    static WWW database;

    // Stores all CharacterElements obtained from the CharacterElementDatabase 
    // assetbundle, sorted by character and category.
    // character name -> category name -> CharacterElement
    static Dictionary<string, Dictionary<string, CharacterElement>> sortedElements;

    // As elements in a Dictionary are not indexed sequentially we use this list when 
    // determining the previous/next character, instead of sortedElements.
    static List<string> availableCharacters = new List<string>();

    // Stores the WWWs for retrieving the characterbase assetbundles that 
    // hold the bones and animations for a specific character.
    // character name -> WWW for characterbase.assetbundle
    static Dictionary<string, WWW> characterBaseWWWs = new Dictionary<string, WWW>();
	
	static Dictionary<string, WWW> characterTexturesWWWs = new Dictionary<string, WWW>();

    // The bones and animations from the characterbase assetbundles are loaded
    // asynchronously to avoid delays when first using them. A LoadAsync results
    // in an AssetBundleRequest which are stored here so we can check their progress
    // and use the assets they contain once they are loaded.
    // character name -> AssetBundleRequest for Character Base GameObject.
    static Dictionary<string, AssetBundleRequest> characterBaseRequests = new Dictionary<string, AssetBundleRequest>();
	
	static Dictionary<string, GameObject> characterBaseGameObjDic = new Dictionary<string, GameObject>();
	
	CharacterConfig mCharacterConfig;
	CharacterConfig.CharacterObject mCharacterObj = new CharacterConfig.CharacterObject();
	ItemConfig mItemConfig;
	
    // Stores the currently configured character which is used when downloading
    // assets and generating characters.
    private string currentCharacter = "";
	
	private int mInedxIter = 0;

    // Stores the current configuration which is used when downloading assets
    // and generating characters.
    // category name -> current character element
    Dictionary<string, CharacterElement> currentConfiguration = new Dictionary<string, CharacterElement>();

    // Used to give a more accurate download progress.
    float assetbundlesAlreadyDownloaded;
		
	
	private Vector3 mTargetInitPosition = Vector3.zero;
	
	private int mCharacterConfigId;
	
	private Animator animator;
	
	private Transform[] hips;
	
	private Transform target;
	
	private bool mLongDressUP;
	
	private bool mDownPantsWeared;
	
	private string mBaraTextureName;
	private string mUnderTextureName;
	private string mSockTextureName; 
	
	////////////////////////////////////////////// =   头发,   上装  下装裙  手部    脚部  长裤
	private static readonly string[] InnerPartList = {"TAXF","TAXB","TAXC","TAXD","TAXG","TAXE","TAXA",};
	private static readonly string[] ModelPartList = {"TAXF","TAXB","TAXC","TAXD","TAXG","TAXE","TAXA",};
	private static readonly int PartListLength = 7;
		
	private Dictionary<string , SkinnedMeshRenderer> targetSmr = new  Dictionary<string , SkinnedMeshRenderer>();
	
    // Avoid users creating instances with a new statement or before
    // sortedElements is populated.
    private CharacterGenerator()
    {
        //if (!ReadyToUse) 
          //  throw new Exception("CharacterGenerator.ReadyToUse must be true before creating CharacterGenerator instances.");
    }
	
	void Awake()
	{
		target = this.transform;
	}
	
	void OnDestroy()
	{
		Destroy(target.gameObject);
	}
	
    // The following static methods can be used to create
    // CharacterGenerator instances.
	
	public void setCurrentCharacter(string str)
	{
	 	currentCharacter = str;
	}
	
	public string getCurrentCharacter()
	{
		return currentCharacter;
	}
	
	public void addCharacterProcessor()
	{
		GameObject boneRoot = target.Find(currentCharacter).gameObject;
		boneRoot.AddComponent<CharacterProcessor>();
	}
	
	public void changeCharacterAnimation(int animationIndex)
	{
		if (animator == null)
			return;
		if (animator.runtimeAnimatorController.name.Contains("Clothes"))
		{
			Debug.Log("index is " + animationIndex);
			animator.SetInteger("index",animationIndex);
		}
	}
	
	public void changeCharacterAnimationController(string ControllerName)
	{
		if (animator.runtimeAnimatorController == null || 
			(animator.runtimeAnimatorController != null && animator.runtimeAnimatorController.name != ControllerName))
		{
			//target.localPosition = mTargetInitPosition ;
			//target.localEulerAngles = new Vector3(0,0,0);
			
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
	
			mTargetInitPosition = target.localPosition;

			
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
	
	
	public void ResetCharacter()
	{
		target.localPosition = mTargetInitPosition;
		foreach(SkinnedMeshRenderer skmr in targetSmr.Values)
		{
			skmr.sharedMesh = null;
		}
		if (mSockTextureName != null)
		{
			mSockTextureName = "";
			mBaraTextureName = "";
			mUnderTextureName = "";
		}
	}
	
	public IEnumerator  generateCharacterFromConfigSynchronous(int characterId ,string animationName)
	{		
	
		currentCharacter = "a001";
		
		if (!characterBaseWWWs.ContainsKey(currentCharacter))
		{
			var www = WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + currentCharacter + "_characterbase.assetbundle",GameDefines.AssetBundleVersion);
			characterBaseWWWs.Add(currentCharacter,www);
			yield return characterBaseWWWs[currentCharacter];
		}
	       
	
		if (!characterBaseRequests.ContainsKey(currentCharacter))
		{
			//AssetBundleRequest requestCBase = ;
			characterBaseRequests.Add(currentCharacter,CurrentCharacterBase.assetBundle.LoadAssetAsync("characterbase", typeof(GameObject)));
			//if (!characterBaseRequests[currentCharacter].isDone)
			//	return false;
			yield return characterBaseRequests[currentCharacter];
		}
		
		
		if (hips == null || hips.Length == 0)
		{
			GameObject root = (GameObject)Object.Instantiate(characterBaseRequests[currentCharacter].asset);
			animator = root.GetComponent<Animator>();
		       root.name = currentCharacter;
			root.transform.parent = target;
			
			for (int i=0; i<PartListLength; i++)
			{
				GameObject partObj = new GameObject();
				partObj.name = InnerPartList[i];
				partObj.transform.parent = root.transform;
				//为新建立的 GameObject 加入 SkinnedMeshRenderer，并将此 SkinnedMeshRenderer 存入 targetSmr
				targetSmr.Add(InnerPartList[i] , partObj.AddComponent<SkinnedMeshRenderer>());
			}
			
			//从目标物件取得骨架资料 (Female_Hips 的全部物件)
			hips = root.GetComponentsInChildren<Transform>();
		
			root.transform.localPosition = Vector3.zero;
		}
		
		if (mCharacterConfig == null )
			mCharacterConfig = Globals.Instance.MDataTableManager.GetConfig<CharacterConfig>();
		
		mCharacterConfig.GetCharacterObject(characterId,out mCharacterObj);
		
		if (mItemConfig == null)
		    mItemConfig = Globals.Instance.MDataTableManager.GetConfig<ItemConfig>();
		ItemConfig.ItemElement element = null;
		
		for (int i=0 ; i< 19; i++)
		{
			///内衣内裤袜子///
			if (i==5 || i==6 || i==7)
				continue;
			string partModelName = mCharacterObj.PartInfoList[i].partModelName;
			if (partModelName != "" && partModelName != "0")
			{
				partModelName = partModelName.ToLower();
				CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
			
				//if(!partModelElement.WWWBundle.isDone)
					//return false;
				yield return partModelElement.CreateWWWBundle(AssetbundleBaseURL,GameDefines.AssetBundleVersion);
			}
		}
		
		///20-25 肉体 ////
		for (int i =13; i<19; i++)
		{
			string partModelName = mCharacterObj.PartInfoList[i].partModelName;
			if (partModelName != "" && partModelName != "0")
			{
				string partName = partModelName.Substring(0,partModelName.Length - 3);				
				string partIDStr = partModelName.Substring(partModelName.Length -3,3);	
				partModelName = partModelName.ToLower();
				CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
			
			
				GameObject go = (GameObject)partModelElement.GetWWWBundle().assetBundle.LoadAsset("rendererobject", typeof(GameObject));
				SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.GetComponent<Renderer>();
				smr.updateWhenOffscreen = true;
				
				StringHolder stringHolder = (StringHolder)partModelElement.GetWWWBundle().assetBundle.LoadAsset("bonenames", typeof(StringHolder));
				
				ChangePartSynchronous(smr,partName,partIDStr,mCharacterObj.PartInfoList[i].partID,false,stringHolder.content);
			}
		}
		///1-13 头发和衣服//
		for (int i=0; i<13; i++)
		{
			string partModelName = mCharacterObj.PartInfoList[i].partModelName;
			///内衣内裤袜子///
			if (i==5 || i==6 || i==7)
				continue;
			if (partModelName != "" && partModelName != "0")
			{
				string partName = partModelName.Substring(0,partModelName.Length - 3);				
				string partIDStr = partModelName.Substring(partModelName.Length -3,3);
				
				bool IsHas = mItemConfig.GetItemElement(mCharacterObj.PartInfoList[i].ItemID, out element);
				string outTextureName0 = "";
				string outTextureName1 = "";
				if (IsHas)
				{
					outTextureName0 = element.OutTextureName0;
					outTextureName1 = element.OutTextureName1;
				}
				partModelName = partModelName.ToLower();
				CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
			
				
				GameObject go = (GameObject)partModelElement.GetWWWBundle().assetBundle.LoadAsset("rendererobject", typeof(GameObject));
				SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.GetComponent<Renderer>();
				smr.updateWhenOffscreen = true;
				
				StringHolder stringHolder = (StringHolder)partModelElement.GetWWWBundle().assetBundle.LoadAsset("bonenames", typeof(StringHolder));
							
				ChangePartSynchronous(smr,partName,partIDStr,mCharacterObj.PartInfoList[i].partID,false,stringHolder.content, outTextureName0,outTextureName1);
			}
		}
		
		for (int i=5; i<8; i++)
		{
			string partModelName = mCharacterObj.PartInfoList[i].partModelName;
			if (partModelName != "")
			{
				ChangePartTexture(mCharacterObj.PartInfoList[i].partID,partModelName);
			}
		}
						

		
		mCharacterConfigId = characterId;
		
		yield return true;
	}
	
	public IEnumerator  generageCharacterFormGirlDataSynchronous(GirlData girlData)
	{
		currentCharacter = "a001";
		
		if (mCharacterConfig == null )
			mCharacterConfig = Globals.Instance.MDataTableManager.GetConfig<CharacterConfig>();
//		mCharacterConfig.GetCharacterObject(girlData.BasicData.LogicID,out mCharacterObj);
		
		if (!characterBaseWWWs.ContainsKey(currentCharacter))
		{
			var www = WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + currentCharacter + "_characterbase.assetbundle",GameDefines.AssetBundleVersion);
			characterBaseWWWs.Add(currentCharacter,www);
			yield return characterBaseWWWs[currentCharacter];
		}
	    
	
		if (!characterBaseRequests.ContainsKey(currentCharacter))
		{
			//AssetBundleRequest requestCBase = ;
			characterBaseRequests.Add(currentCharacter,CurrentCharacterBase.assetBundle.LoadAssetAsync("characterbase", typeof(GameObject)));
			yield return characterBaseRequests[currentCharacter];
			
		}
		
		
		if (hips == null || hips.Length == 0)
		{
			GameObject root = (GameObject)Object.Instantiate(characterBaseRequests[currentCharacter].asset);
			animator = root.GetComponent<Animator>();
		    root.name = currentCharacter;
			root.transform.parent = target;
			
			for (int i=0; i<PartListLength; i++)
			{
				GameObject partObj = new GameObject();
				partObj.name = InnerPartList[i];
				partObj.transform.parent = root.transform;
				//为新建立的 GameObject 加入 SkinnedMeshRenderer，并将此 SkinnedMeshRenderer 存入 targetSmr
				targetSmr.Add(InnerPartList[i] , partObj.AddComponent<SkinnedMeshRenderer>());
			}
			
			//从目标物件取得骨架资料 (Female_Hips 的全部物件)
			hips = root.GetComponentsInChildren<Transform>();
		
			root.transform.localPosition = Vector3.zero;
			root.transform.localEulerAngles = Vector3.zero;
		}
		

		///2-13 衣服//
		for (int i=mInedxIter; i<19; i++)
		{
			Debug.Log("i is : " + mInedxIter.ToString());
			///内衣内裤袜子///
			if (i==5 || i==6 || i==7)
			{
				string textureName = mCharacterObj.PartInfoList[i].partModelName;
				switch (mCharacterObj.PartInfoList[i].partID)
				{
					case 6:
						mBaraTextureName = textureName;
						break;
					case 7:
						mUnderTextureName = textureName;
						break;
					case 8:
						mSockTextureName = textureName;
						break;
				}
				continue;
			}
				
			ItemSlotData itemSlotData = null ;
			girlData.ClothDatas.TryGetValue(mCharacterObj.PartInfoList[i].partID,out itemSlotData);
			if (itemSlotData != null)
			{
				if (mItemConfig == null)
					mItemConfig = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();
				
				ItemConfig.ItemElement element = null;
				bool IsHas = mItemConfig.GetItemElement(itemSlotData.MItemData.BasicData.LogicID, out element);
				if (!IsHas){
				yield return false;
				}
				string partModelName = element.ModelName;
								
				if (partModelName != "" && partModelName != "0")
				{
					partModelName = partModelName.ToLower();
					CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
					yield return partModelElement.CreateWWWBundle(AssetbundleBaseURL,GameDefines.AssetBundleVersion);
				}
			}
			else
			{
				string partModelName = mCharacterObj.PartInfoList[i].partModelName;
				if (partModelName != "" && partModelName != "0")
				{
					partModelName = partModelName.ToLower();
					CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];			
					yield return partModelElement.CreateWWWBundle(AssetbundleBaseURL,GameDefines.AssetBundleVersion);
				}
			}
		}
		
		///20-25 肉体 ////
		for (int i =13; i<19; i++)
		{
			string partModelName = mCharacterObj.PartInfoList[i].partModelName;
			if (partModelName != "" && partModelName != "0")
			{
				string partName = partModelName.Substring(0,partModelName.Length - 3);				
				string partIDStr = partModelName.Substring(partModelName.Length -3,3);	
				partModelName = partModelName.ToLower();
				CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
			
			
				GameObject go = (GameObject)partModelElement.GetWWWBundle().assetBundle.LoadAsset("rendererobject", typeof(GameObject));
				SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.GetComponent<Renderer>();
				smr.updateWhenOffscreen = true;
				
				StringHolder stringHolder = (StringHolder)partModelElement.GetWWWBundle().assetBundle.LoadAsset("bonenames", typeof(StringHolder));
				
				ChangePartSynchronous(smr,partName,partIDStr,mCharacterObj.PartInfoList[i].partID,false,stringHolder.content);
			}
		}
		///1-13 头发和衣服//
		for (int i=0; i<13; i++)
		{
			string partModelName = mCharacterObj.PartInfoList[i].partModelName;
			///内衣内裤袜子///
			if (i==5 || i==6 || i==7)
				continue;
			if (partModelName != "" && partModelName != "0")
			{
				string partName = partModelName.Substring(0,partModelName.Length - 3);				
				string partIDStr = partModelName.Substring(partModelName.Length -3,3);
				ItemConfig.ItemElement element = null;
				bool IsHas = mItemConfig.GetItemElement(mCharacterObj.PartInfoList[i].ItemID, out element);
				string outTextureName0 = "";
				string outTextureName1 = "";
				if (IsHas)
				{
					outTextureName0 = element.OutTextureName0;
					outTextureName1 = element.OutTextureName1;
				}
				partModelName = partModelName.ToLower();
				CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
			
				
				GameObject go = (GameObject)partModelElement.GetWWWBundle().assetBundle.LoadAsset("rendererobject", typeof(GameObject));
				SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.GetComponent<Renderer>();
				smr.updateWhenOffscreen = true;
				
				StringHolder stringHolder = (StringHolder)partModelElement.GetWWWBundle().assetBundle.LoadAsset("bonenames", typeof(StringHolder));
							
				ChangePartSynchronous(smr,partName,partIDStr,mCharacterObj.PartInfoList[i].partID,false,stringHolder.content, outTextureName0,outTextureName1);
			}
		}
		
		for (int i=5; i<8; i++)
		{
			string partModelName = mCharacterObj.PartInfoList[i].partModelName;
			ChangePartTexture(mCharacterObj.PartInfoList[i].partID,partModelName);
		}
							
		
		yield break;
	}
	
    // Returns the currentConfiguration as a string for easy storage.
    public string GetConfig()
    {
        string s = currentCharacter;
        foreach (KeyValuePair<string, CharacterElement> category in currentConfiguration)
            s += "|" + category.Key + "|" + category.Value.name;
        return s;
    }

    // This method downloads the CharacterElementDatabase assetbundle and populates
    // the sortedElements Dictionary from the contents. This is done at runtime as
    // ScriptableObjects do not support Dictionaries. ReadyToUse must be true before
    // you create an instance of CharacterGenerator.
	public static IEnumerator ReadyToUse()
	{
		if (database == null)
		{
			database = WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + "CharacterElementDatabase.assetbundle",GameDefines.AssetBundleVersion);
			yield return database;
		}
		
		
		if (sortedElements != null) 
		{
			yield break;
		}
		else
		{
			//if (!database.isDone) return false;
			if (database.assetBundle == null) yield return 0;
			
			
			CharacterElementHolder ceh = (CharacterElementHolder) database.assetBundle.mainAsset;
		
	
			sortedElements = new Dictionary<string, Dictionary<string, CharacterElement>>();
			foreach (CharacterElement element in ceh.content)
			{
				string[] a = element.bundleName.Split('_');
				string character = a[0];
				string category = a[1].Split('-')[0].Replace(".assetbundle", "");
	
				if (!availableCharacters.Contains(character))
					availableCharacters.Add(character);
	
				if (!sortedElements.ContainsKey(character))
					sortedElements.Add(character, new Dictionary<string, CharacterElement>());
	
				if (!sortedElements[character].ContainsKey(category))
					sortedElements[character].Add(category, element);
			}
			yield return 0;
		}
		yield break;
	}
	
	
    // Averages the download progress of all assetbundles required for the currentConfiguration,
    // and takes into account the progress at the time of the last configuration change. This 
    // way we can give a progress indication that runs from 0 to 1 even when some assets were
    // already downloaded.
    public float CurrentConfigProgress
    {
        get
        {
            float toDownload = currentConfiguration.Count + 1 - assetbundlesAlreadyDownloaded;
            if (toDownload == 0) return 1;
            float progress = CurrentCharacterBase.progress;
            foreach (CharacterElement e in currentConfiguration.Values)
                progress += e.GetWWWBundle().progress;
            return (progress - assetbundlesAlreadyDownloaded) / toDownload;
        }
    }

	
	Dictionary<string, CharacterElement> getCurrentConfiguration()
	{
		return currentConfiguration;
	}

    // Returns correct assetbundle base url, whether in the editor, standalone or
    // webplayer, on Mac or Windows.
    public static string AssetbundleBaseURL
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
                return Application.dataPath+"/assetbundles/";
            else
			{
               return GameDefines.GetUrlBase();
			}
			return GameDefines.GetUrlBase();
        }
    }

    // Returns the WWW for retrieving the assetbundle that holds the bones and animations 
    // for currentCharacter, and creates a WWW only if one doesnt exist already. 
    WWW CurrentCharacterBase
    {
        get
        {
            if (!characterBaseWWWs.ContainsKey(currentCharacter))
                characterBaseWWWs.Add(currentCharacter, new WWW(AssetbundleBaseURL + currentCharacter + "_characterbase.assetbundle"));
            return characterBaseWWWs[currentCharacter];
        }
    }
		
	
	public IEnumerator ChangePart(string part , string item,int partID,bool changePart,string outTexture1 = "", string outTexture2 = "")
	{
		if (partID == 6 || partID == 7 ||  partID == 8)
		{
			string textureName = part+item;
			StartCoroutine(ChangePartTexture(partID,textureName));
			switch (partID)
			{
				case 6:
					
					break;
				case 7:
					makeTAXE();
					break;
				case 8:
					break;
			}
			
		}
		else
		{
			string partModelName = part + item;
			if (partModelName != "" && partModelName != "0")
			{
				partModelName = partModelName.ToLower();
				CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];			
				yield return partModelElement.CreateWWWBundle(AssetbundleBaseURL,GameDefines.AssetBundleVersion);
				
				GameObject go = (GameObject)partModelElement.GetWWWBundle().assetBundle.LoadAsset("rendererobject", typeof(GameObject));
				SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.GetComponent<Renderer>();
				smr.updateWhenOffscreen = true;
					
				StringHolder stringHolder = (StringHolder)partModelElement.GetWWWBundle().assetBundle.LoadAsset("bonenames", typeof(StringHolder));
				ChangePartSynchronous(smr,part,item,partID,changePart,stringHolder.content,outTexture1,outTexture2);
				
				//GameObject.DestroyObject(go);
			}
		}
	}
	
	public  void  ChangePartSynchronous(SkinnedMeshRenderer smr ,string part , string item,int partID,bool changePart, string [] boneNames,string outTexture1 = "", string outTexture2 = "")
	{
		Debug.Log("ChangePart is " + part + item);		
		
		List<Transform> bones = new List<Transform>();
		List<Material> materials = new List<Material>();
				
		string targetPartName = "TAXB"; 
		foreach(string boneName in boneNames)
		{
			foreach(Transform hip in hips)
			{
				if(hip.name != boneName) continue;
				bones.Add(hip);
				break;
			}
		}

		switch (partID)
		{
			case 1 : //帽子//
			case 2 : //上衣//
			case 3 : //下装裙//
			case 4 : //手套//
			case 5 : //鞋子//
				targetPartName = InnerPartList[partID -1]; 
				break;
			case 10 :
			case 24 ://长裤 腿部//
				targetPartName = "TAXE";
				break;
			case 12 ://旗袍//
				targetPartName = "TAXB";
				break;
			case 13://热裤//
				targetPartName = "TAXC";	
				break;
			case 20 ://脸///
				targetPartName = "TAXA";
				break;
			case 21 :///裸上身//
				targetPartName = "TAXB";
				break;
			case 22 :///裸臀部//
				targetPartName = "TAXC";
				break;
			case 23 ://手//
				targetPartName = "TAXD";
				break;
			case 25 :///脚底板///
				targetPartName = "TAXG";
				break;
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容
		targetSmr[targetPartName].sharedMesh = smr.sharedMesh;
		targetSmr[targetPartName].quality = SkinQuality.Bone4;
		targetSmr[targetPartName].updateWhenOffscreen = true;
		targetSmr[targetPartName].bones = bones.ToArray();
		targetSmr[targetPartName].materials = smr.sharedMaterials;
		
		for (int i=0;i<targetSmr[targetPartName].materials.Length; i++)
		{
			string testShadername = targetSmr[targetPartName].materials[i].shader.name;
			targetSmr[targetPartName].materials[i].shader =  Shader.Find(testShadername);
		}
		
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
		}
		
		if (partID == 2 && changePart && mBaraTextureName!="")
			paintBaraTexture();
		
		if (partID == 2 && changePart && mLongDressUP)
		{
			mLongDressUP = false;
		}
			
		
		if (partID == 10 && changePart)
		{
			if (mLongDressUP)
			{
				makeTAXB();
				if (mBaraTextureName != "")
					paintBaraTexture();
				mLongDressUP = false;
			}
			if (mSockTextureName!="")
			{
				paintSockTexture();
			}
		}
		
		if (partID == 10 )
		{
			mDownPantsWeared = true;
			makeTAXC_NULL();
		}
		
		if (partID == 12)
		{
			makeTAXC();
			makeTAXE();
			
			mLongDressUP = true;
			
			if (mBaraTextureName != "")
			{
				StartCoroutine(paintBaraTexture());
			}
			
			if (mUnderTextureName != "")
			{
				StartCoroutine(paintUnderWearTexture());
			}
		}
		
		if (partID == 13)
		{
			if (mLongDressUP)
			{
				makeTAXB();
				if (mBaraTextureName != "")
					paintBaraTexture();
				mLongDressUP = false;
			}
			
			makeTAXE();
			
		}
		
		if (outTexture1 != "")
			StartCoroutine(changeOutSideTexture(targetSmr[targetPartName],0,outTexture1));
			
		if (outTexture2 != "")
			StartCoroutine(changeOutSideTexture(targetSmr[targetPartName],1,outTexture2));
		
	}
	
	IEnumerator ChangePartTexture(int partID,string textureName)
	{
		string targetPartName = "";
		switch (partID)
		{
			case 6:
				targetPartName = "TAXB";
				mBaraTextureName = textureName;
				break;
			case 7:
				targetPartName = "TAXC";
				mUnderTextureName = textureName;
				break;
			case 8:
				targetPartName = "TAXE";
				mSockTextureName = textureName;
				break;
		}
		
		int materialCount = targetSmr[targetPartName].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			if (targetSmr[targetPartName].materials[i].name.Contains(targetPartName))
			{
				targetSmr[targetPartName].materials[i].shader  = Shader.Find ("MyGirl/TwoTextureBlend");
				
				if (!characterTexturesWWWs.ContainsKey(textureName))
				{
					var textureWWW =  WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + textureName + ".assetbundle",GameDefines.AssetBundleVersion);
					yield return textureWWW;
					characterTexturesWWWs.Add(textureName,textureWWW);
				}
				
				
				Texture t = (Texture)characterTexturesWWWs[textureName].assetBundle.LoadAsset(textureName,typeof(Texture));
				targetSmr[targetPartName].materials[i].SetTexture("_BlendTex",t);
			}
		}
		
		yield break;
	}
	
	private void makeTAXC()
	{	//屁股部分//
		string partModelName = "taxc001";
		CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
		
		GameObject go = (GameObject)partModelElement.GetWWWBundle().assetBundle.LoadAsset("rendererobject", typeof(GameObject));
		SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.GetComponent<Renderer>();
		smr.updateWhenOffscreen = true;
		
		StringHolder stringHolder = (StringHolder)partModelElement.GetWWWBundle().assetBundle.LoadAsset("bonenames", typeof(StringHolder));
		
		List<Transform> bones = new List<Transform>();
		bones.Clear();
		foreach(string boneName in stringHolder.content){
		foreach(Transform hip in hips){
					if(hip.name != boneName) continue;
					bones.Add(hip);
					break;
				}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr["TAXC"].sharedMesh = smr.sharedMesh;
		targetSmr["TAXC"].quality = SkinQuality.Bone4;
		targetSmr["TAXC"].bones = bones.ToArray();
		targetSmr["TAXC"].materials = smr.materials;
		
		for (int i=0;i<targetSmr["TAXC"].materials.Length; i++)
		{
			string testShadername = targetSmr["TAXC"].materials[i].shader.name;
			targetSmr["TAXC"].materials[i].shader =  Shader.Find(testShadername);
		}
	}
	
	private void makeTAXB()
	{
		//光上身//
		string partModelName = "taxb001";
		CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
		GameObject go = (GameObject)partModelElement.GetWWWBundle().assetBundle.LoadAsset("rendererobject", typeof(GameObject));
		SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.GetComponent<Renderer>();
		smr.updateWhenOffscreen = true;
		
		StringHolder stringHolder = (StringHolder)partModelElement.GetWWWBundle().assetBundle.LoadAsset("bonenames", typeof(StringHolder));
		
		List<Transform> bones = new List<Transform>();
		bones.Clear();
		foreach(string boneName in stringHolder.content){
			foreach(Transform hip in hips){
				if(hip.name != boneName) continue;
				bones.Add(hip);
				break;
			}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr["TAXB"].sharedMesh = smr.sharedMesh;
		targetSmr["TAXB"].quality = SkinQuality.Bone4;
		targetSmr["TAXB"].bones = bones.ToArray();
		targetSmr["TAXB"].materials = smr.materials;
		
		for (int i=0;i<targetSmr["TAXB"].materials.Length; i++)
		{
			string testShadername = targetSmr["TAXB"].materials[i].shader.name;
			targetSmr["TAXB"].materials[i].shader =  Shader.Find(testShadername);
		}
	}
	
	private void makeTAXE()
	{
		//光腿//
		string partModelName = "taxe001";
		CharacterElement partModelElement = sortedElements[currentCharacter][partModelName];
		GameObject go = (GameObject)partModelElement.GetWWWBundle().assetBundle.LoadAsset("rendererobject", typeof(GameObject));
		SkinnedMeshRenderer smr = (SkinnedMeshRenderer)go.GetComponent<Renderer>();
		smr.updateWhenOffscreen = true;
		
		StringHolder stringHolder = (StringHolder)partModelElement.GetWWWBundle().assetBundle.LoadAsset("bonenames", typeof(StringHolder));
		
		List<Transform> bones = new List<Transform>();
		bones.Clear();
		foreach(string boneName in stringHolder.content){
		foreach(Transform hip in hips){
					if(hip.name != boneName) continue;
					bones.Add(hip);
					break;
				}
		}
		
		// 更新指定部位 GameObject 的 SkinnedMeshRenderer 内容//
		targetSmr["TAXE"].sharedMesh = smr.sharedMesh;
		targetSmr["TAXE"].quality = SkinQuality.Bone4;
		targetSmr["TAXE"].bones = bones.ToArray();
		targetSmr["TAXE"].materials = smr.materials;
		
		if (mSockTextureName!="")
		{
			ChangePartTexture(8,mSockTextureName);
		}
		
		for (int i=0;i<targetSmr["TAXE"].materials.Length; i++)
		{
			string testShadername = targetSmr["TAXE"].materials[i].shader.name;
			targetSmr["TAXE"].materials[i].shader =  Shader.Find(testShadername);
		}
	}
	
	private void makeTAXC_NULL()
	{
		//屁股部分//
		targetSmr["TAXC"].sharedMesh = null;
	}
	
	private IEnumerator paintBaraTexture()
	{
		string	targetPartName = "TAXB";
		int materialCount = targetSmr[targetPartName].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			if (targetSmr[targetPartName].materials[i].name.Contains("TAXB"))
			{
				targetSmr[targetPartName].materials[i].shader  = Shader.Find ("MyGirl/TwoTextureBlend");
				
				
				if (!characterTexturesWWWs.ContainsKey(mBaraTextureName))
				{
					var textureWWW =  WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + mBaraTextureName + ".assetbundle",GameDefines.AssetBundleVersion);
					yield return textureWWW;
					characterTexturesWWWs.Add(mBaraTextureName,textureWWW);
				}
								
				Texture t = (Texture)characterTexturesWWWs[mBaraTextureName].assetBundle.LoadAsset(mBaraTextureName,typeof(Texture));
				targetSmr[targetPartName].materials[i].SetTexture("_BlendTex",t);
			}
		}
		yield break;
	}
	
	private IEnumerator paintUnderWearTexture()
	{
		string targetPartName = "TAXC";
		int materialCount = targetSmr[targetPartName].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			if (targetSmr[targetPartName].materials[i].name.Contains("TAXC"))
			{
				targetSmr[targetPartName].materials[i].shader  = Shader.Find ("MyGirl/TwoTextureBlend");
				
				if (!characterTexturesWWWs.ContainsKey(mUnderTextureName))
				{
					var textureWWW = WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + mUnderTextureName + ".assetbundle",GameDefines.AssetBundleVersion);
					yield return textureWWW;
					characterTexturesWWWs.Add(mUnderTextureName,textureWWW);
				}
				
				Texture t = (Texture)characterTexturesWWWs[mUnderTextureName].assetBundle.LoadAsset(mUnderTextureName,typeof(Texture));

				targetSmr[targetPartName].materials[i].SetTexture("_BlendTex",t);
			}
		}
		
		yield break;
	}
	
	private IEnumerator paintSockTexture()
	{
		string targetPartName = "TAXE";
		int materialCount = targetSmr[targetPartName].materials.GetLength(0);
		for (int i=0; i<materialCount; i++)
		{
			if (targetSmr[targetPartName].materials[i].name.Contains("TAXE"))
			{
				targetSmr[targetPartName].materials[i].shader  = Shader.Find ("MyGirl/TwoTextureBlend");
				
				if (!characterTexturesWWWs.ContainsKey(mSockTextureName))
				{
					var textureWWW = WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + mSockTextureName + ".assetbundle",GameDefines.AssetBundleVersion);
					yield return textureWWW;
					characterTexturesWWWs.Add(mSockTextureName,textureWWW);
				}
				
				Texture t = (Texture)characterTexturesWWWs[mSockTextureName].assetBundle.LoadAsset(mSockTextureName,typeof(Texture));
				
				targetSmr[targetPartName].materials[i].SetTexture("_BlendTex",t);
			}
		}
		
		yield break;
	}
	
	public IEnumerator changeOutSideTexture(SkinnedMeshRenderer skinnedMeshRender, int id,string textureName)
	{
		if (!characterTexturesWWWs.ContainsKey(textureName))
		{
			var textureWWW = WWW.LoadFromCacheOrDownload(AssetbundleBaseURL + textureName + ".assetbundle",GameDefines.AssetBundleVersion);
			yield return textureWWW;
			characterTexturesWWWs.Add(textureName,textureWWW);
		}
		
		Texture t = (Texture)characterTexturesWWWs[textureName].assetBundle.LoadAsset(textureName,typeof(Texture));
		skinnedMeshRender.materials[id].SetTexture("_MainTex",t);
		
	}
	
}