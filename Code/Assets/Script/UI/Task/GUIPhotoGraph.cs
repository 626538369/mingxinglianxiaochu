using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIPhotoGraph : GUIWindowForm
{
	public Transform gridBuff;
	public BuffDesc buffdesc;
	public CharacterCustomizeOne MCharacterCustomizeOne;
	public UIButton BackBtn;

	private int[][] ClothTypeNum = new int[][]
	{
		new int[]{1}, 			// 发型和帽子0//
		new int[]{2,12,14},		//泳装//
		new int[]{3},			//下装//
		new int[]{5},			//鞋子//
		new int[]{8},			//袜子//
		new int[]{4},			//手套//
	};
	public List<PropItem> MPropItemLst;
	private Dictionary<int,PropItem> MPropItemDic = new Dictionary<int, PropItem> ();

	public GameObject GameInfo;
	public GameObject BgInfo;
	public GameObject ReadyInfo;
	public UIButton StartBtn;
	public UIButton ChangleClothBtn;
	public GameObject TotalMagicObj;

	public GameSuccessReward mGameSuccessReward;


	ISubscriber _mBuyPetFood = null;
	ISubscriber _mPetInteractive = null;
	ItemSlotData MagnifierSlotData = null;
	ItemSlotData SlowMotionData = null;
	ItemSlotData mPetTipsData = null;

	TaskConfig taskConfig;

	protected override void Awake()
	{
		if (null == Globals.Instance.MGUIManager)
			return;
		
		base.Awake();
		base.enabled = true;



		Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.TASKCAMERA);
		if(SoundManager.CurrentPlayingMusicAudio != null)
		{
			Destroy(SoundManager.CurrentPlayingMusicAudio.gameObject);
			SoundManager.CurrentPlayingMusicAudio = null;
		}

		for(int i = 0 ; i < 6;i++){
			for(int j = 0 ;j < ClothTypeNum[i].Length ; j++){
				
				MPropItemDic.Add (ClothTypeNum[i][j] ,MPropItemLst[i]);
			}
		}


		UIEventListener.Get (BackBtn.gameObject).onClick += delegate(GameObject go) {
			EliminationMgr.Instance.GameStatus = GameStatusEnum.Pause;
			Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui){
				gui.SetDialogType(EDialogType.CommonType,null);
				gui.SetText("确定放弃？");
				gui.CancelEvent += delegate() {
					EliminationMgr.Instance.GameFieldAnimationEndStartGame();	
				};
			},EDialogStyle.DialogOkCancel,delegate() {
				EliminationMgr.Instance.GameStatus = GameStatusEnum.GameOver;
			});	
		};

		UIEventListener.Get (StartBtn.gameObject).onClick += delegate(GameObject go) {
				
			GameInfo.SetActive (true);
			TotalMagicObj.SetActive (true);
			BgInfo.transform.localPosition = new Vector3 (0f,780f,0f);
			ReadyInfo.SetActive (false);
			foreach(KeyValuePair<int,PropItem> v in MPropItemDic){
				v.Value.UpdateInfo();
			}
			MCharacterCustomizeOne.gameObject.SetActive(false);

			EliminationMgr.Instance.GameFieldAnimationEndStartGame();
		};
		UIEventListener.Get (ChangleClothBtn.gameObject).onClick += delegate(GameObject go) {
			Globals.Instance.MGUIManager.CreateWindow<GUIChangeCloth>(delegate(GUIChangeCloth Change)
				{
					this.IsReturnMainScene = false;
					this.Close();
					NGUITools.SetActive(Change.ChangeBtn.gameObject,false);
					UISprite exitBtn = Change.ExitBtn.gameObject.GetComponent<UISprite>();
					exitBtn.spriteName = "ButtonFanhui3Normol";
					Change.ExitBtn.normalSprite = "ButtonFanhui3Normol";
					Change.ExitBtn.hoverSprite = "ButtonFanhui3Normol";
					Change.ExitBtn.pressedSprite = "ButtonFanhui3Normol";
					Change.ExitBtn.disabledSprite = "ButtonFanhui3Normol";
					Change.CloseChangeClothEvent += delegate()
					{
						GUIGuoChang.Show();
						Globals.Instance.MGUIManager.CreateWindow<GUIPhotoGraph>(delegate(GUIPhotoGraph gui)
							{
								GUIGuoChang.SetTweenPlay(0,delegate() {

								});
								GUIMain guimain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
								if(guimain != null)
								{
									guimain.SetVisible(false);					
								}
								gui.DrawReadyView();
							});
					};
				});
		};

		taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig> ();
	}

	Dictionary<int,BuffDesc> buffdic = new Dictionary<int, BuffDesc>();
	public void SetBuffInfo(int id,string desc ,UITexture t)
	{
		if (buffdic.ContainsKey (id)) {
			return;
		} else {
			BuffDesc Item = GameObject.Instantiate(buffdesc) as BuffDesc;
			Item.init (t,desc);
			Item.transform.SetParent (gridBuff,false);
			Item.transform.localPosition = new Vector3 (-(buffdic.Count * 90), 0, Item.transform.position.z);
			buffdic.Add (id, Item);
		}
	
	}
	protected override void Start()
	{
		base.Start();
	}

	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		this.GUILevel = 20;
		

		PlayerData playerData = Globals.Instance.MGameDataManager.MActorData;
		MCharacterCustomizeOne.generageCharacterFormPlayerData(playerData);
		MCharacterCustomizeOne.changeCharacterAnimationController("General_Idle");
		MCharacterCustomizeOne.transform.localEulerAngles = new Vector3 (0f,180f,0f);
	}
	
	protected override void Update()
	{
		base.Update();
	}
	
	void SwitchingScenes()
	{
		string sceneName = Globals.Instance.MSceneManager.GetSceneName();
		Globals.Instance.MSoundManager.PlaySceneSound(sceneName);
	}
	

	
	void OnDestroy()
	{
		base.OnDestroy();
		
		if (this.IsReturnMainScene) {
			if(Globals.Instance.MSceneManager != null){
				Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);
			}
		}
		if(EliminationMgr.Instance != null){
			EliminationMgr.Instance.OnClose ();
		}

		if(MCharacterCustomizeOne != null){
			GameObject.DestroyImmediate(MCharacterCustomizeOne.gameObject);
		}

		if (Globals.Instance.MSceneManager != null) {
			Globals.Instance.MSceneManager.mTaskCameramControl.transform.localPosition = Vector3.zero;
			Globals.Instance.MSceneManager.mTaskCameramControl.transform.localEulerAngles = Vector3.zero;
			Globals.Instance.MSceneManager.mTaskCamera.fieldOfView = 15;
		}

		UnregisterSubscribers();
		
		Resources.UnloadUnusedAssets();
		System.GC.Collect();	
		
	}
	
	void UnregisterSubscribers()
	{
		if(_mBuyPetFood != null)
		{
			_mBuyPetFood.Unsubscribe();
		}
		_mBuyPetFood = null;
		if(_mPetInteractive != null)
		{
			_mPetInteractive.Unsubscribe();
		}
		_mPetInteractive = null;
	}
	
	// 开始界面----//
	public void DrawReadyView()
	{
		GUIMission.StorageTaskInfo sto = Globals.Instance.MTaskManager.mTaskDailyData.StorageTaskInfo;
		int taskId = sto.taskId;
//		Debug.LogError ("task = " + taskId);
		GameInfo.SetActive (false);
		TotalMagicObj.SetActive (false);
		BgInfo.transform.localPosition = new Vector3 (0f,903f,0f);
		ReadyInfo.SetActive (true);

		EliminationMgr.Instance.Init (taskId);

		LoadPropInfo (taskId);
	}


	public void LoadPropInfo(int taskId){

		TaskConfig.TaskObject element = null;
		bool hasData = taskConfig.GetTaskObject(taskId, out element);
		if (!hasData)
			return;	
		PlayerData playerData = Globals.Instance.MGameDataManager.MActorData;
		Dictionary<int,ItemSlotData> clothDic = playerData.ClothDatas;

		if(element.Progress_Count > 0){
			List<int> themeLst = StrParser.ParseDecIntList (element.Theme_Effect,0);
			List<int> limitLst = StrParser.ParseDecIntList (element.Material_Effect,0); //可使用的衣服类型
			foreach(KeyValuePair<int,PropItem> v in MPropItemDic){
				if (limitLst.Contains (v.Key)) {
					if (clothDic.ContainsKey (v.Key)) {
						ItemSlotData slotData = clothDic [v.Key];
						if (slotData.MItemData != null && themeLst.Contains (slotData.MItemData.BasicData.ItemConfigElement.Item_Style)) {
							MPropItemDic [v.Key].InitUse (slotData);
						} else {
							MPropItemDic [v.Key].InitNoLock (slotData);	
						}
					} else {
						MPropItemDic [v.Key].InitNoUse ();	
					}
				} else {
					MPropItemDic [v.Key].NoOpenProp ();	
				}
			}
		}
	}


	public void TaskCompleteRes(sg.GS2C_Task_Complete_Res res){
	
		mGameSuccessReward.ShowSuccessReward (res);
		
	}

	public void UpdatePropMagicStatus(){
		foreach(KeyValuePair<int,PropItem> v in MPropItemDic){
			v.Value.UpdateInfo();
		}
	}

	private AudioSource BuildAudioObj(string name, bool loop)
	{
		AudioClip clip = Resources.Load(name) as AudioClip;
		if (null == clip)
		{
			Debug.Log("Cann't search the sound resource " + name);
			return null;
		}
		
		if (!clip.isReadyToPlay)
		{
			Debug.Log("Cann't decompress the sound resource " + name);
		}
		
		clip.name = name;
		
		GameObject go = new GameObject();
		go.name = name;
		AudioSource source = go.AddComponent<AudioSource>() as AudioSource;
		
		source.clip = clip;
		source.loop = loop;
		source.volume = GameDefines.Setting_MusicVol / 100.0f;

		return source;
	}
}
