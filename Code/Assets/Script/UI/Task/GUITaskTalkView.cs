using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TALKSTATE
{
	BEFORE = 1,
	MIDDLE = 2,
	ACCOMPLISH = 3,
	COMPLETE = 4,
	Other = 5
}

public enum TALKTYE
{
	TALK1 = 1, 
	TALK2 = 2,
	TALK3 = 3,
	TALK4 = 4,
	TALK5 = 5,
}

public class GUITaskTalkView : GUIWindow
{
	//public GameObject mBGUp = null;
	public GameObject mBGDown = null;
	public PackedSprite mPacked = null;
	public UILabel mShowText = null;
	private TypewriterEffect mTypewriterEffect = null;
	public UILabel mSpeaker = null;
	
	public GameObject TaskDialogGameObject;
	public GameObject TaskLabelGameObject;
	public GameObject CameraGameObject;
	
//	public UITexture spriteTalkBackground;
	
	AudioSource mDialogSoundSource;
	
	/// <summary>
	/// 任务背景图
	/// </summary>/
	public UITexture textureBackgroundScene;
	public Transform talkBgPosition;
	public UITexture StrangerTexture;

	public GameObject taksLableItemPrefab;
	public CharacterCustomizeOne mCharacterCustomizeOne;
	public CharacterCustomizeOne mCharacterCustomizeNPC;
	
	private CharacterCustomizeOne mCharacterCustomizeCurrent;
	public UIButton mMaskLayer;
	public UIButton SkipBtn;
	public UIButton SkipBtnAll;
	public UITexture npcIcon;
//	public UIButton CameraBtn;
//	public UIButton CameraExitBtn;
//	public UIButton CameraSaveBtn;
//	public UIButton CameraShareBtn;

	public UITexture mCurrenTexture;
	private Texture2D texture2d;
	
	private GameObject mModelScene;
	public UISprite  FinishedTipSprite;
	
	private Vector3 mUpBeginPos;
	private Vector3 mUpDestPos;
	private Vector3 mDownBeginPos;
	private Vector3 mDownDestPos;	
	
	private float mAniPlayCurTime = 0.0f;
	private float mAniPlayDurTime = 0.5f;
	bool mAniPlayIng = false;
	
	private static readonly Vector3		BGUPPos_Dest	= new Vector3(0,272,0);
	private static readonly Vector3		BGUPPos_Begin	= new Vector3(0,380,0);
	
	private static readonly Vector3		BGDownPos_Dest	= new Vector3(0,-270,0);
	private static readonly	Vector3		BGDownPos_Begin	= new Vector3(0,-470,0);
	
	private static readonly Vector3[]	Text_Postion	= 
	{
		new Vector3(-205.0f,52.67f,-0.1f),
		new Vector3(-205.0f,52.67f,-0.1f),
	};
	

	
	private static readonly Vector3[]	Avatar_Position_One = 
	{
		new Vector3(4f,-320f,700f),
		new Vector3(4f,-320f,700f),
		new Vector3(4f,-320f,700f),
		
	};	
	
	private static readonly Vector3[]	Avatar_ROTATION_One = 
	{
		new Vector3(1.0f,-180f,0.0f),
		new Vector3(1.0f,-180f,0.0f),
		new Vector3(0f,-180f,0.0f)
	};
	
	private static readonly Vector3[] LabelItem_Positon = 
	{
		new Vector3(0,50f,-1f),
		new Vector3(0,-140f,-1f),
		new Vector3(0,-330f,-1f),
		new Vector3(0,240f,-1f),
		new Vector3(0,430f,-1f),
	};
		
	
	public delegate void TalkCallBackDelegate();
	
	[HideInInspector] public TalkCallBackDelegate mTalkCallBackDelegate = null;
	
	public delegate void ForTZZDelegate();
	[HideInInspector] public ForTZZDelegate mForTZZDelegate = null;
	
	private TaskConfig.TaskObject CurTask = null;
	private TALKSTATE CurState = TALKSTATE.BEFORE;
	//private List<TalkInfo> talkList = new List<TalkInfo>();
	private Dictionary<int, List<TaskDialogConfig.TaskDialogObject>> taskTalksIDDic;
	private List<TaskDialogConfig.TaskDialogObject> talkList = null;
	
	private int curIndex = 0;
	
	private bool mTaskCameraPlaying = false;
	private Animation mTaskCameraAnimation = null;
	private string mTaskCameraAnimationName = "";
	private bool mMustPlayEffect = false;
	private bool mHeadEffected = false;
	
	private float mSpeakerTime = 0.0f;
	private bool  mSpeakering = false;
	private const float mSpeakSpram = 0.15f;
	private int TalkTimes = 0;

	private int cacheFatherTaskId = 0;
		//---------------------------------------------------------------
	protected override void Awake()
	{
		if (null == Globals.Instance.MGUIManager)
			return;
		
		base.Awake();
		NGUITools.SetActive(SkipBtn.gameObject,false);
		base.enabled = true;
		
		NGUITools.SetActive(CameraGameObject,false);
		
		UIEventListener.Get(mMaskLayer.gameObject).onClick += PressedButton;
		UIEventListener.Get(SkipBtn.gameObject).onClick += OnPressedSkipBtn;
		UIEventListener.Get(SkipBtnAll.gameObject).onClick += OnPressedSkipAllBtn;
//		UIEventListener.Get(CameraBtn.gameObject).onClick += OnPressedCameraBtn;
		
		
//		UIEventListener.Get(CameraExitBtn.gameObject).onClick += OnPressedCameraExitBtn;
//		UIEventListener.Get(CameraSaveBtn.gameObject).onClick += OnPressedCameraSaveBtnBtn;
//		UIEventListener.Get(CameraShareBtn.gameObject).onClick += OnPressedShareBtn;
				
	}
	
	protected virtual void Start ()
	{
		base.Start();
	}
	
	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		

		this.GUILevel = 5;
		
				
		TaskDialogConfig taskTalksIDConfig = Globals.Instance.MDataTableManager.GetConfig<TaskDialogConfig>();
		taskTalksIDConfig.GeTTaskDialogDic(out taskTalksIDDic);
		
 		Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.TASKCAMERA);
		
		PlayerData playerData =  Globals.Instance.MGameDataManager.MActorData;	
		mCharacterCustomizeOne.generageCharacterFormPlayerData(playerData);
//		NGUITools.SetActive(mCharacterCustomizeOne.gameObject,false);
		SetActive(mCharacterCustomizeOne,false);

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
	}
	
	public void PlayLocalTalk(int talkID,TalkCallBackDelegate talkCallBackDelegate)
	{
		ResetData();
		if(taskTalksIDDic.ContainsKey(talkID))
		{
			talkList = taskTalksIDDic[talkID];
		
			mTalkCallBackDelegate = talkCallBackDelegate;
			
			SetText(talkList[curIndex]);

			GUIGuoChang.SetTweenPlay(0,delegate() {
				
			});
		}
	}
	
	public void UpdateData(int taskid, TalkCallBackDelegate talkCallBackDelegate)
	{
		ResetData();
				
		TaskConfig taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		Dictionary<int, TaskConfig.TaskObject> taskObjectDic;
		taskConfig.GeTaskObjectList(out taskObjectDic);
		
		CurTask = taskObjectDic[taskid];
		
		for(int i = 0; i < Globals.Instance.MTaskManager._mUnfinishList.Count; i++)
		{
			if(taskid == Globals.Instance.MTaskManager._mUnfinishList[i].Task_ID)
			{
				CurState = Globals.Instance.MTaskManager._mUnfinishList[i].State;
			}
		}
		
		int talkid = CurTask.Task_Talk_ID;
		
	
		Globals.Instance.M3DItemManager.EZ3DItemParent.localScale = Vector3.zero;
		
		mTalkCallBackDelegate = talkCallBackDelegate;
		
		if(taskTalksIDDic.ContainsKey(talkid))
		{
			talkList = taskTalksIDDic[talkid];
			
			SetText(talkList[curIndex]);
		
			//TalkAniIn();
			
			if(GameStatusManager.Instance.MGameState == GameState.GAME_STATE_COPY)
			{
				GameStatusManager.Instance.MCurrentGameStatus.Pause();
			}
			
		}

	}
	
	public void TalkAniOut()
	{
		if(!mAniPlayIng)
		{
			mAniPlayIng = true;
			mUpBeginPos = BGUPPos_Dest;
			mUpDestPos = BGUPPos_Begin;
			mDownBeginPos = BGDownPos_Dest;
			mDownDestPos = BGDownPos_Begin;
		}
	}
	
	public void TalkAniIn()
	{
		if(!mAniPlayIng)
		{
			mAniPlayIng = true;
			mUpBeginPos = BGUPPos_Begin;
			mUpDestPos = BGUPPos_Dest;
			mDownBeginPos = BGDownPos_Begin;
			mDownDestPos = BGDownPos_Dest;
		}
	}
	

	public void Update()
	{
		if (mTaskCameraPlaying)
		{
			if (mTaskCameraAnimation != null && !mTaskCameraAnimation.IsPlaying(mTaskCameraAnimationName))
			{
				if (talkList[curIndex].effectIDTail > 0)
				{
					
					///设置 SpriteScenePreground效果的////
					TweenGroupConfig tweenGroupConfig = Globals.Instance.MDataTableManager.GetConfig<TweenGroupConfig>();
					if (tweenGroupConfig.IsPregroundGroup("SpriteScenePreground",talkList[curIndex].effectIDTail))
					{
						GameObject priorGameObj = Globals.Instance.MSceneManager.mTaskCamera.transform.Find("SpriteScenePreground").gameObject;
						if (priorGameObj != null)
						{
							NGUITools.SetActive(priorGameObj,true);
						}
					}
					
					TweenGroup tweenGroup = textureBackgroundScene.GetComponent<TweenGroup>();
					if (tweenGroup == null)
						tweenGroup = textureBackgroundScene.gameObject.AddComponent<TweenGroup>();
					tweenGroup.setTweenGroupID(talkList[curIndex].effectIDTail);
					tweenGroup.playTweenAnimation();
					tweenGroup.TweenFinishedEvents += OnTweenGroupFinishendEvent;
				}
				else{
					PressedButton(null);
				}
				
				mTaskCameraPlaying = false;
			}
		}
		
//		if (mSpeakering )
//		{
//			
//			mSpeakerTime -= Time.deltaTime;
//			if (mSpeakerTime < 0.0f)
//			{
//				Animator animator = mCharacterCustomizeCurrent.getCharacterAnimator();
//				if (animator.layerCount > 2)
//				{
//					animator.SetInteger("index",0);
//				}
//				mSpeakering = false;
//			}
//		}
		return ;
	}
	
	private void SendTaskMessage()
	{

	}
	
	public void ResetData()
	{
		curIndex = 0;
		CurTask = null;
		CurState = TALKSTATE.BEFORE;
		
		mAniPlayCurTime = 0;
		mAniPlayIng = false;
		mHeadEffected = false;
		mMustPlayEffect = false;
		
//		NGUITools.SetActive(FinishedTipSprite.gameObject,false);
		
		Globals.Instance.MSceneManager.mTaskCamera.enabled = true;
		Globals.Instance.MSceneManager.mTaskCamera.targetTexture = null;
		Globals.Instance.MSceneManager.mMainCamera.enabled = false;
		
		GUITaskTrack tasktrack = Globals.Instance.MGUIManager.GetGUIWindow<GUITaskTrack>();
		if(tasktrack != null){
				tasktrack.SetVisible(false);					
		}
		
		
		GUIBuildExplore guiBuildExplore = Globals.Instance.MGUIManager.GetGUIWindow<GUIBuildExplore>();
		if (guiBuildExplore != null)
			guiBuildExplore.SetVisible(false);
		
		GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
		if (guiMain != null)
			guiMain.SetVisible(false);
		
		NGUITools.SetActive(TaskLabelGameObject,false);
		
//		NGUITools.SetActive(spriteTalkBackground.gameObject,true);
	
//		NGUITools.SetActive(mMaskLayer.gameObject,true);
		NGUITools.SetActive(mSpeaker.gameObject,true);
		

	}
	
	protected override void OnDestroy ()
	{
		base.OnDestroy ();
	}
	
	public  void DestroyThisGUI ()
	{
		
		GameObject skyGameObj = GameObject.Find("PortSky");
		if (skyGameObj != null)
			GameObject.Destroy(skyGameObj);
		
		GameObject priorGameObj = Globals.Instance.MSceneManager.mTaskCamera.transform.Find("SpriteScenePreground").gameObject;
		if (priorGameObj != null)
		{
			Destroy(priorGameObj.GetComponent<TweenAlpha>());
			NGUITools.SetActive(priorGameObj,false);
		}
		
		HelpUtil.DelListInfo (textureBackgroundScene.transform);
		if(null != textureBackgroundScene)
		{
			GameObject.DestroyImmediate(textureBackgroundScene.gameObject);
		}
		
		 
		
		GameObject.Destroy(mCharacterCustomizeOne.gameObject);
		GameObject.Destroy(mCharacterCustomizeNPC.gameObject);
		
		mCharacterCustomizeCurrent = null;
		
		 Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);

	
		if(CurTask != null && CurTask.Task_Category != (int)TaskManager.TaskCategory.EXPLORE)
		{
			string sceneName = Globals.Instance.MSceneManager.GetSceneName();
			Globals.Instance.MSoundManager.PlaySceneSound(sceneName);
		}
		
		base.Close();
		OnDestroy();
		
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}
	
	private void OnPressedSkipBtn(GameObject obj)
	{
		if (mTaskCameraPlaying)
		{
			mTaskCameraPlaying = false;
			mMustPlayEffect = false;
			if (mTaskCameraAnimation != null && mTaskCameraAnimation.IsPlaying(mTaskCameraAnimationName))
			{
				Globals.Instance.MSceneManager.mTaskCameramControl.transform.localPosition = new Vector3(0,0,0);
				Globals.Instance.MSceneManager.mTaskCameramControl.transform.localEulerAngles = Vector3.zero;
				mTaskCameraAnimation.Stop(mTaskCameraAnimationName);
				PressedButton(null);
			}
		}
	}
	
	private void OnPressedSkipAllBtn(GameObject obj)
	{
		curIndex = talkList.Count - 1;
		mMustPlayEffect = false;
		PressedButton(null);
	}
	
	private void OnPressedCameraBtn(GameObject obj)
	{
		var rtW = Screen.width/2;
		var rtH = Screen.height/2;

		texture2d = GUIDormitory.CaptureCamera(Globals.Instance.MSceneManager.mTaskCamera,new Rect(0,0, rtW, rtH));
		mCurrenTexture.mainTexture = texture2d;
		if (mTaskCameraPlaying)
		{
			mTaskCameraAnimation[mTaskCameraAnimationName].speed = 0;
			if (mCharacterCustomizeCurrent.getCharacterAnimator() != null)
				mCharacterCustomizeCurrent.getCharacterAnimator().enabled = false;
		}
//		NGUITools.SetActive(CameraBtn.gameObject,false);
		NGUITools.SetActive(CameraGameObject,true);
	}
	
	private void OnPressedCameraExitBtn(GameObject obj)
	{
		NGUITools.SetActive(CameraGameObject,false);
//		NGUITools.SetActive(CameraBtn.gameObject,true);
		if (mTaskCameraPlaying)
		{
			mTaskCameraAnimation[mTaskCameraAnimationName].speed = 1;
			if (mCharacterCustomizeCurrent.getCharacterAnimator() != null)
				mCharacterCustomizeCurrent.getCharacterAnimator().enabled = true;
		}
	}
	
	private void OnPressedCameraSaveBtnBtn(GameObject obj)
	{
		 // 最后将这些纹理数据，成一个png图片文件  
	    byte[] bytes =  texture2d.EncodeToPNG();  
		string cptrAddr = "Screenshot" + System.DateTime.Now.Second.ToString() + ".png";
	    string filename = Application.persistentDataPath + "/" + cptrAddr;  
	    System.IO.File.WriteAllBytes(filename, bytes);  
	    Debug.Log(string.Format("截屏了一张照片: {0}", filename));   
		
		
		
		if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			GUIRadarScan.Show();
			//U3dAppStoreSender.AppSavePhoth(filename);
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
		{
			GUIRadarScan.Show();
			StartCoroutine(InvokeAndroidCameraSaveDelegate());
		}
	}

	IEnumerator InvokeAndroidCameraSaveDelegate()
	{
		yield return new WaitForSeconds(0.2f);
		// 最后将这些纹理数据，成一个png图片文件  //
		byte[] bytes =  texture2d.EncodeToPNG();  
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
	
	private void OnPressedShareBtn(GameObject obj)
	{
		  // 最后将这些纹理数据，成一个png图片文件  
	    byte[] bytes =  texture2d.EncodeToPNG();  
		string cptrAddr = "Screenshot" + System.DateTime.Now.Second.ToString() + ".png";
	    string filename = Application.persistentDataPath + "/" + cptrAddr;  
	    System.IO.File.WriteAllBytes(filename, bytes);  
	    Debug.Log(string.Format("截屏了一张照片: {0}", filename));   
	
		Globals.Instance.MTaskManager.IsGetShareReward = true;
		
		if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			//U3dAppStoreSender.ShareMyPhoto(filename);
		}
		else if(GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
		{
			AndroidSDKAgent.showShare(filename);
		}
	}
	
	private void PressedButton(GameObject obj)
	{
		// tzz added
		// the talk list will be null if used in Teach scene
		// check TeachFirstEnterGame.cs for detail
		
		if (mTypewriterEffect == null)
			return;
		
		if (mTypewriterEffect.IsTypewriterIng())	
		{
			mTypewriterEffect.FinishTypewriter();
			return;
		}
		
		if (mMustPlayEffect)
		{
			if (FinishedTipSprite.gameObject.active)
			{
				if (talkList[curIndex].effectIDTail > 0)
				{
					///设置 SpriteScenePreground效果的////
					TweenGroupConfig tweenGroupConfig = Globals.Instance.MDataTableManager.GetConfig<TweenGroupConfig>();
					if (tweenGroupConfig.IsPregroundGroup("SpriteScenePreground",talkList[curIndex].effectIDTail))
					{
						GameObject priorGameObj = Globals.Instance.MSceneManager.mTaskCamera.transform.Find("SpriteScenePreground").gameObject;
						if (priorGameObj != null)
						{
							NGUITools.SetActive(priorGameObj,true);
						}
					}
					
					TweenGroup tweenGroup = textureBackgroundScene.GetComponent<TweenGroup>();
					if (tweenGroup == null)
						tweenGroup = textureBackgroundScene.gameObject.AddComponent<TweenGroup>();
					tweenGroup.setTweenGroupID(talkList[curIndex].effectIDTail);
					tweenGroup.playTweenAnimation();
					tweenGroup.TweenFinishedEvents += OnTweenGroupFinishendEvent;
					
					NGUITools.SetActive(TaskDialogGameObject,false);
					
				}
			}
			return;
		}
		
		if(talkList == null || talkList.Count == 0 || curIndex >= talkList.Count || mAniPlayIng  /*|| CurTask == null*/){
			if(mForTZZDelegate != null)
			{
				mForTZZDelegate();
			}
			return;
		}
		//对话结束
		if(curIndex == talkList.Count - 1)
		{
			if (CurTask == null)
			{
//				DestroyThisGUI();
				if(mTalkCallBackDelegate != null)
				{
					mTalkCallBackDelegate();
					mTalkCallBackDelegate = null;
				}
				return;
			}
			int nCurTaskID = CurTask.Task_ID;
			
			if (!mClentTask)
			{
				if (!ProcessorTaskBefoeFinished(nCurTaskID))
				{
					SendTaskMessage();
				}
			}
			
//			DestroyThisGUI();
			if(mTalkCallBackDelegate != null)
			{
				mTalkCallBackDelegate();
				mTalkCallBackDelegate = null;
			}
			
			if(GameStatusManager.Instance.MGameState == GameState.GAME_STATE_COPY)
				GameStatusManager.Instance.MCurrentGameStatus.Resume();
			
			//TalkAniOut();
			return;
		}
		else
		{
			if (talkList[curIndex].talkTpye == (int)TALKTYE.TALK4)
			{	
				NGUITools.SetActive(TaskDialogGameObject,false);
				if (talkList[curIndex].effectIDTail > 0)
				{
					///设置 SpriteScenePreground效果的////
					TweenGroupConfig tweenGroupConfig = Globals.Instance.MDataTableManager.GetConfig<TweenGroupConfig>();
					if (tweenGroupConfig.IsPregroundGroup("SpriteScenePreground",talkList[curIndex].effectIDTail))
					{
						GameObject priorGameObj = Globals.Instance.MSceneManager.mTaskCamera.transform.Find("SpriteScenePreground").gameObject;
						if (priorGameObj != null)
						{
							NGUITools.SetActive(priorGameObj,true);
						}
					}
					
					TweenGroup tweenGroup = textureBackgroundScene.GetComponent<TweenGroup>();
					if (tweenGroup == null)
						tweenGroup = textureBackgroundScene.gameObject.AddComponent<TweenGroup>();
					tweenGroup.setTweenGroupID(talkList[curIndex].effectIDTail);
					tweenGroup.playTweenAnimation();
					tweenGroup.TweenFinishedEvents += OnCGTweenGroupFinishendEvent;
					mMustPlayEffect = true;
					curIndex++;
					return;
					
				}
			}
			
			curIndex++;
		}
		
		SetText(talkList[curIndex]);
	}
	
	private void SetText(TaskDialogConfig.TaskDialogObject talkInfo)
	{

		HelpUtil.HideListInfo (textureBackgroundScene.transform,true);
		if (mModelScene != null)
			GameObject.DestroyObject(mModelScene);
		
		if (mDialogSoundSource != null)
			mDialogSoundSource.Stop();
		
		Globals.Instance.MSceneManager.mTaskCameramControl.transform.localPosition = new Vector3(0,0,0);
		Globals.Instance.MSceneManager.mTaskCameramControl.transform.localEulerAngles = Vector3.zero;
		
		
		NGUITools.SetActive(TaskDialogGameObject,true);
		NGUITools.SetActive(FinishedTipSprite.gameObject,false);
		NGUITools.SetActive(SkipBtn.gameObject,false);
		NGUITools.SetActive(npcIcon.transform.parent.gameObject , false);
		
		UIWidget uiWidget = textureBackgroundScene.GetComponentInChildren<UIWidget>();
		UIPanel uiPanel = textureBackgroundScene.GetComponent<UIPanel>();
		uiWidget.alpha = 1f;
		uiPanel.alpha = 1f;
		
		string atlasPath = "UIAtlas/" + talkInfo.talkBgPicture;
		textureBackgroundScene.mainTexture = Resources.Load(atlasPath,typeof(Texture2D)) as Texture2D;
		Globals.Instance.MSoundManager.PlaySceneSound(talkInfo.talkBgPicture);
	
		
		mMustPlayEffect = false;
//     effectIDHead 淡入淡出效果 没用到
//		if (talkList[curIndex].effectIDHead > 0 && mHeadEffected == false)
//		{
//			mMustPlayEffect = true;
//			NGUITools.SetActive(TaskDialogGameObject,false);
//			
//			if (talkInfo.talkTpye == (int)TALKTYE.TALK1)
//			{
//				if (talkInfo.girlID1 == 0 && talkInfo.girlID2 < 0 )
//				{
//					NGUITools.SetActive(mCharacterCustomizeOne.gameObject,true);
//					NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,false);
//					mCharacterCustomizeCurrent = mCharacterCustomizeOne;
//					mCharacterCustomizeOne.setAnimationOneState(true);
//					mCharacterCustomizeOne.changeCharacterAnimationController(talkInfo.girlAnimation1);
//					mCharacterCustomizeOne.transform.position = Avatar_Position_One[0];
//					mCharacterCustomizeOne.transform.localEulerAngles = Avatar_ROTATION_One[0];
//				}
//				else if (talkInfo.girlID2 != 0 && talkInfo.girlID1 < 0)
//				{
//					if(talkInfo.girlID2 == 9999)
//					{
//						NGUITools.SetActive(mCharacterCustomizeOne.gameObject,false);
//						NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,false);
//
//						npcIcon.mainTexture = Resources.Load("UIAtlas/" + talkInfo.girlAnimation2,typeof(Texture2D)) as Texture2D;
//						NGUITools.SetActive(npcIcon.transform.parent.gameObject , true);
//					}
//					else
//					{
//						NGUITools.SetActive(mCharacterCustomizeOne.gameObject,false);
//						NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,true);
//						mCharacterCustomizeCurrent = mCharacterCustomizeNPC;
//						NPCConfig npcConfig = Globals.Instance.MDataTableManager.GetConfig<NPCConfig>();
//						NPCConfig.NPCObject npcObject ;
//						npcConfig.GetNPCObject(talkInfo.girlID2,out npcObject);
//						mCharacterCustomizeNPC.ResetCharacter();
//						mCharacterCustomizeNPC.generateCharacterFromConfig(npcObject.NPCGender,"D0101",npcObject.NpcAppearance,npcObject.NpcEquips);
//						mCharacterCustomizeNPC.setAnimationOneState(true);
//						mCharacterCustomizeNPC.changeCharacterAnimationController(talkInfo.girlAnimation2);
//						if (npcObject.NPCGender == (int)PlayerGender.GENDER_DOG)
//						{
//							mCharacterCustomizeNPC.transform.position = Avatar_Position_One[2];
//							mCharacterCustomizeNPC.transform.localEulerAngles = Avatar_ROTATION_One[2];
//						}
//						else
//						{
//							mCharacterCustomizeNPC.transform.position = Avatar_Position_One[1];
//							mCharacterCustomizeNPC.transform.localEulerAngles = Avatar_ROTATION_One[1];
//						}
//					}
//				}
//			}
//			
//			///设置 SpriteScenePreground效果的////
//			TweenGroupConfig tweenGroupConfig = Globals.Instance.MDataTableManager.GetConfig<TweenGroupConfig>();
//			if (tweenGroupConfig.IsPregroundGroup("SpriteScenePreground",talkList[curIndex].effectIDHead))
//			{
//				GameObject priorGameObj = Globals.Instance.MSceneManager.mTaskCamera.transform.Find("SpriteScenePreground").gameObject;
//				if (priorGameObj != null)
//				{
//					NGUITools.SetActive(priorGameObj,true);
//				}
//			}
//			
//			TweenGroup tweenGroup = textureBackgroundScene.GetComponent<TweenGroup>();
//			if (tweenGroup == null)
//				tweenGroup = textureBackgroundScene.gameObject.AddComponent<TweenGroup>();
//			tweenGroup.setTweenGroupID(talkList[curIndex].effectIDHead);
//			tweenGroup.playTweenAnimation();
//			tweenGroup.TweenFinishedEvents += OnHeadTweenGroupFinishendEvent;
//			return;
//		}
		
		if (talkList[curIndex].effectIDTail > 0)
		{
			mMustPlayEffect = true;
		}
		
		///无女孩模型的对话//
		if(talkInfo.talkTpye == (int)TALKTYE.TALK3)
		{			
			TalkTimes=0;
			string text = getTaskTalkContent(talkInfo.talkContent);
			string icon = ""; 
			int type = talkInfo.talkTpye;
			mSpeaker.text = getSpeakerName(talkInfo.talkCaptionName);
			SetText(text,icon,type,talkInfo.talkMusic);
//			NGUITools.SetActive(mCharacterCustomizeOne.gameObject,false);
			SetActive(mCharacterCustomizeOne,false);
//			NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,false);
			SetActive(mCharacterCustomizeNPC,false);
		}
		///一个女孩模型的对话//
		else if (talkInfo.talkTpye == (int)TALKTYE.TALK1)
		{
			if (talkInfo.girlID1 >= 0 && talkInfo.girlID2 < 0)
			{
				if(talkInfo.girlID1 == 9999)
				{
//					NGUITools.SetActive(mCharacterCustomizeOne.gameObject,false);
					SetActive(mCharacterCustomizeOne,false);
//					NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,false);	
					SetActive(mCharacterCustomizeNPC,false);
					npcIcon.mainTexture = Resources.Load("UIAtlas/" + talkInfo.girlAnimation1,typeof(Texture2D)) as Texture2D;
					NGUITools.SetActive(npcIcon.transform.parent.gameObject , true);
				}
				else
				{
					TalkTimes++;
					mCharacterCustomizeOne.transform.localPosition = new Vector3(-0.05f,-0.3f,-5.0f);
//					NGUITools.SetActive(mCharacterCustomizeOne.gameObject,true);
//					NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,false);
					SetActive(mCharacterCustomizeOne,true);
					SetActive(mCharacterCustomizeNPC,false);
					mCharacterCustomizeCurrent = mCharacterCustomizeOne;
					mCharacterCustomizeOne.setAnimationOneState(true);
//					if(TalkTimes>=2)
//					{
//						PlayerData playerData =  Globals.Instance.MGameDataManager.MActorData;	
//						mCharacterCustomizeOne.ResetCharacter();
//						mCharacterCustomizeOne.generageCharacterFormPlayerData(playerData);
//					}
					mCharacterCustomizeOne.changeCharacterAnimationController(talkInfo.girlAnimation1);
					mCharacterCustomizeOne.transform.position = Avatar_Position_One[0];
					mCharacterCustomizeOne.transform.localEulerAngles = Avatar_ROTATION_One[0];
				}
			
			}
			if (talkInfo.girlID1 < 0 && talkInfo.girlID2 > 0)
			{
				TalkTimes = 0;
				if(talkInfo.girlID2 == 9999)
				{
//					NGUITools.SetActive(mCharacterCustomizeOne.gameObject,false);
//					NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,false);
					SetActive(mCharacterCustomizeOne,false);
					SetActive(mCharacterCustomizeNPC,false);
					npcIcon.mainTexture = Resources.Load("UIAtlas/" + talkInfo.girlAnimation2,typeof(Texture2D)) as Texture2D;
					NGUITools.SetActive(npcIcon.transform.parent.gameObject , true);
				}
				else
				{
					mCharacterCustomizeOne.transform.localPosition = new Vector3(2.0f,-0.3f,-5.0f);
//					NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,true);
					SetActive(mCharacterCustomizeNPC,true);
					mCharacterCustomizeCurrent = mCharacterCustomizeNPC;
					NPCConfig npcConfig = Globals.Instance.MDataTableManager.GetConfig<NPCConfig>();
					NPCConfig.NPCObject npcObject ;
					npcConfig.GetNPCObject(talkInfo.girlID2,out npcObject);
					mCharacterCustomizeNPC.ResetCharacter();
					mCharacterCustomizeNPC.generateCharacterFromConfig(npcObject.NPCGender,"D0101",npcObject.NpcAppearance,npcObject.NpcEquips);
					mCharacterCustomizeNPC.setAnimationOneState(true);
					mCharacterCustomizeNPC.changeCharacterAnimationController(talkInfo.girlAnimation2);
					if (npcObject.NPCGender == (int)PlayerGender.GENDER_DOG)
					{
						mCharacterCustomizeNPC.transform.position = Avatar_Position_One[2];
						mCharacterCustomizeNPC.transform.localEulerAngles = Avatar_ROTATION_One[2];
					}
					else
					{
						mCharacterCustomizeNPC.transform.position = Avatar_Position_One[1];
						mCharacterCustomizeNPC.transform.localEulerAngles = Avatar_ROTATION_One[1];
					}
				}
			}
			if (talkInfo.girlID1 < 0)
			{
//				NGUITools.SetActive(mCharacterCustomizeOne.gameObject,false);
				SetActive(mCharacterCustomizeOne,false);
			}
			if (talkInfo.girlID2 < 0)
			{
//				NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,false);
				SetActive(mCharacterCustomizeNPC,false);
			}
			
			string text = getTaskTalkContent(talkInfo.talkContent);
			string icon = ""; 
			int type = talkInfo.talkTpye;
			mSpeaker.text = getSpeakerName(talkInfo.talkCaptionName);
			SetText(text,icon,type,talkInfo.talkMusic);
			string meStr = Globals.Instance.MDataTableManager.GetWordText(4001);
			if (talkInfo.talkCaptionName != meStr && talkInfo.talkCaptionName != "")
			{
					characterCustomizeSpeakState(true);
			}
			else{
					characterCustomizeSpeakState(false);
			}
		}
		///CG对话//
		else if (talkInfo.talkTpye == (int)TALKTYE.TALK4)
		{	
			NGUITools.SetActive(TaskDialogGameObject,false);
			mCharacterCustomizeOne.ResetCharacter();
			
			if (talkList[curIndex].effectIDTail > 0)
			{
				mMustPlayEffect = false;
			}
		}
		///特写对话//
		else if (talkInfo.talkTpye == (int)TALKTYE.TALK5)
		{		

			NGUITools.SetActive(TaskDialogGameObject,false);
//			NGUITools.SetActive(mMaskLayer.gameObject,false);
			
			uiWidget.alpha = 0.0f;
			uiPanel.alpha = 0.0f;
			
			if (talkInfo.talkModlePrefab != "")
			{
				Object aModelSceneObj = Resources.Load("Scene/Prefabs/" + talkInfo.talkModlePrefab,typeof(Object)) as Object;
				mModelScene = GameObject.Instantiate(aModelSceneObj,Vector3.one,Quaternion.identity) as GameObject;
			}
						
			if (talkInfo.girlID1 > 0)
			{
//				NGUITools.SetActive(mCharacterCustomizeOne.gameObject,false);
				SetActive(mCharacterCustomizeOne,false);
				if(talkInfo.girlID1 == 9999)
				{

				}
				else
				{					
//					NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,true);
					SetActive(mCharacterCustomizeNPC,true);
					mCharacterCustomizeCurrent = mCharacterCustomizeNPC;
					NPCConfig npcConfig = Globals.Instance.MDataTableManager.GetConfig<NPCConfig>();
					NPCConfig.NPCObject npcObject ;
					npcConfig.GetNPCObject(talkInfo.girlID1,out npcObject);
					mCharacterCustomizeNPC.generateCharacterFromConfig(npcObject.NPCGender,"D0101",npcObject.NpcAppearance,npcObject.NpcEquips);
					mCharacterCustomizeNPC.setAnimationOneState(true);
					mCharacterCustomizeNPC.changeCharacterAnimationController(talkInfo.girlAnimation1);
				}
			}
			else if (talkInfo.girlID1 == 0)
			{
//				NGUITools.SetActive(mCharacterCustomizeOne.gameObject,true);
//				NGUITools.SetActive(mCharacterCustomizeNPC.gameObject,false);
				SetActive(mCharacterCustomizeOne,true);
				SetActive(mCharacterCustomizeNPC,false);
				mCharacterCustomizeCurrent = mCharacterCustomizeOne;
				mCharacterCustomizeOne.setAnimationOneState(true);
				mCharacterCustomizeOne.changeCharacterAnimationController(talkInfo.girlAnimation1);
			}
			
			
			mTaskCameraAnimation = Globals.Instance.MSceneManager.mTaskCamera.gameObject.GetComponent<Animation>();
			mTaskCameraAnimation.Play(getTaskTalkContent(talkInfo.talkContent));
			mTaskCameraAnimationName = getTaskTalkContent(talkInfo.talkContent);
			mTaskCameraPlaying  = true;
			NGUITools.SetActive(SkipBtn.gameObject,true); 
		}

		if(talkInfo.MoodBG1 != null&&!talkInfo.MoodBG1.Equals(""))
		{
			if(middleMoodsDic.ContainsKey(talkInfo.MoodBG1))
			{
				middleMoodsDic[talkInfo.MoodBG1].SetActive(true);
			}
			else
			{
				GameObject moodObj = Resources.Load ("Prefabs/" + talkInfo.MoodBG1, typeof(GameObject)) as GameObject;
				GameObject moodObj2 = GameObject.Instantiate (moodObj, Vector3.one, Quaternion.identity) as GameObject;
				moodObj2.transform.SetParent(textureBackgroundScene.transform,false);
				//			Globals.Instance.MSceneManager.SetMoodPosion(moodObj2.transform,GameEnumManager.MoodBGType.middle);
				middleMoodsDic.Add(talkInfo.MoodBG1,moodObj2);
			}
		}

	}
	private Dictionary<string,GameObject> middleMoodsDic = new Dictionary<string, GameObject>();
	private void SetActive(CharacterCustomizeOne c, bool isShow)
	{
		if(c != null)
		{
			c.SetActive(isShow);
		}
	}
	public void SetText(string text,string icon,int type,string soundName){
		
		if(mShowText != null)
		{
			mShowText.text = text;
			if (mTypewriterEffect == null)
				mTypewriterEffect = mShowText.gameObject.GetComponent<TypewriterEffect>();
			mTypewriterEffect.EnabledTypewriter();
			mTypewriterEffect.TypeWriteStopEvent += OnTypeWriteStopEvent;

			if (soundName != "")
			 	mDialogSoundSource = Globals.Instance.MSoundManager.PlayTaskSoundEffect("Sounds/TaskSound/" + soundName);			
		}
		
	}
	

	private void characterCustomizeSpeakState(bool isSpeak)
	{
		if(mCharacterCustomizeCurrent == null)
		{
			return;
		}
		Animator animator = mCharacterCustomizeCurrent.getCharacterAnimator();
		if(animator != null&&animator.layerCount > 2)
		{
			if(isSpeak)
			{
				animator.SetInteger("index" , 1);
			}
			else
			{
				animator.SetInteger("index" , 0);
			}
		}
	}


	public void showTaskLables(sg.GS2C_Task_GetLabels_Res res)
	{
		NGUITools.SetActive(npcIcon.transform.parent.gameObject , false);
		string atlasPath = "UIAtlas/" + "Home1";
		textureBackgroundScene.mainTexture = Resources.Load(atlasPath,typeof(Texture2D)) as Texture2D;

		NGUITools.SetActive(SkipBtnAll.gameObject , false);

		Task_Label taskLabelConfig = Globals.Instance.MDataTableManager.GetConfig<Task_Label>();

//		NGUITools.SetActive(TaskDialogGameObject , false);
		NGUITools.SetActive(TaskLabelGameObject , true);

		cacheFatherTaskId = res.fatherTaskId;

		HelpUtil.DelListInfo(TaskLabelGameObject.transform);
		for(int i = 0; i < res.tasks.Count; i++)
		{
			int taskLabelID = res.tasks[i];

			GameObject taskLabelItem = GameObject.Instantiate(taksLableItemPrefab) as GameObject;
			taskLabelItem.transform.parent = TaskLabelGameObject.transform;
			taskLabelItem.transform.localPosition = LabelItem_Positon[i];
			taskLabelItem.transform.localScale = Vector3.one;


			UILabel taskLabel = taskLabelItem.transform.Find("TaskLabel").GetComponent<UILabel>();
			UISprite needSprite = taskLabelItem.transform.Find("NeedSprite").GetComponent<UISprite>();
			UILabel needLabel = needSprite.transform.Find("NeedLabel").GetComponent<UILabel>();

			Task_Label.TaskLabelElement element = taskLabelConfig.GetTaskLabelElement(taskLabelID);
			if(element == null)
			{
				return;
			}
			taskLabel.text = element.Title ;
			NGUITools.SetActive(needSprite.gameObject , false);

			UIButton btn = taskLabelItem.transform.GetComponent<UIButton>();
			btn.Data = taskLabelID;
			UIEventListener.Get(btn.gameObject).onClick += OnClickTaskLabelItemBtn;
		}

		GUIGuoChang.Hide();
	}


	private void OnClickTaskLabelItemBtn(GameObject obj)
	{
		UIButton btn = obj.transform.GetComponent<UIButton>();
		int taskid = (int)btn.Data;
		Globals.Instance.MTaskManager.StartNextTask(taskid,false,cacheFatherTaskId);
	}

	private void OnTypeWriteStopEvent()
	{
		mTypewriterEffect.TypeWriteStopEvent -= OnTypeWriteStopEvent;
		NGUITools.SetActive(FinishedTipSprite.gameObject,true);

		characterCustomizeSpeakState(false);
	}
	
	
	private void OnTweenGroupFinishendEvent(GameObject gameObj,bool isAutoJump)
	{
		TweenGroup tweenGroup = textureBackgroundScene.GetComponent<TweenGroup>();
		tweenGroup.TweenFinishedEvents -= OnTweenGroupFinishendEvent;
		
		mMustPlayEffect = false;
		if (isAutoJump)
		{
		    PressedButton(null);
		}
	}
	
	private void OnHeadTweenGroupFinishendEvent(GameObject gameObj,bool isAutoJump)
	{
		TweenGroup tweenGroup = textureBackgroundScene.GetComponent<TweenGroup>();
		tweenGroup.TweenFinishedEvents -= OnHeadTweenGroupFinishendEvent;
		
		mMustPlayEffect = false;
		mHeadEffected = true;
		SetText(talkList[curIndex]);
		mHeadEffected = false;
	}
	
	private void OnCGTweenGroupFinishendEvent(GameObject gameObj,bool isAutoJump)
	{
		TweenGroup tweenGroup = textureBackgroundScene.GetComponent<TweenGroup>();
		tweenGroup.TweenFinishedEvents -= OnCGTweenGroupFinishendEvent;
		mMustPlayEffect = false; 
		SetText(talkList[curIndex]);
	}
	
	
	public bool ProcessorTaskBefoeFinished(int nCurTaskID)
	{		
		if (TaskConfig._taskReNameGirlDict.ContainsKey(nCurTaskID))
		{
			return true;
		}

		return false;
	}
	
	private string getSpeakerName(string configName)
	{
		string meStr = Globals.Instance.MDataTableManager.GetWordText(4001);
		string colorStr = "";
//		if (configName != meStr && configName != "")
//		{
//			colorStr = "[Ff6979]";
//		}
//		else
//		{
//			colorStr = "[5D6D9E]";
//		}
		int npcLogicID = StrParser.ParseDecInt(configName,0);
		if (npcLogicID != 0)
		{
			Dictionary<long,GirlData> dicWarShipData =  Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();
			foreach (GirlData girlData in dicWarShipData.Values)
			{
//				if (girlData.BasicData.LogicID == npcLogicID)
//					return colorStr + girlData.BasicData.Name;
			}
		}
		return colorStr + configName;
	}
	
	public static  string getTaskTalkContent(string content)
	{
		string IconTag = "[Name:";
		string RichEnd = "end]";
		while (content.Contains(IconTag))
		{
			int startIndex = content.IndexOf(IconTag);
			int endIndex = content.IndexOf(RichEnd);
			int count = endIndex - startIndex + RichEnd.Length;
			string iconStr = content.Substring(startIndex,count);
			string nameStr = "";
			
			string [] strList = iconStr.Split(':');
			if (strList.Length > 2)
			{
				int npcLogicID = StrParser.ParseDecInt(strList[1],-1);
				if (npcLogicID !=-1 && npcLogicID != 0 )
				{
					Dictionary<long,GirlData> dicWarShipData =  Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();
					foreach (GirlData girlData in dicWarShipData.Values)
					{
//						if (girlData.BasicData.LogicID == npcLogicID)
//							nameStr =  girlData.BasicData.Name;
					}
				}
				else if (npcLogicID == 0)
				{
					nameStr = Globals.Instance.MGameDataManager.MActorData.BasicData.Name;
				}
			}
	
			
			content = content.Remove(startIndex,count);
			content = content.Insert(startIndex,nameStr);
								
		}
	
		return content;
		
	}




	
	public bool ClentTask 
	{
		get{ return mClentTask;}
		set{ mClentTask = value;}
	}
	
	/// 是否是心动回忆等 只是本地播放的任务对话///
	private bool mClentTask = false;
}
