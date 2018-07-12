using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class GUIPreLogin :  GUIWindow 
{
	public UIGrid TTGrid;
	public UIScrollView TTScrollView;
	
	//public UIDragPanelContents listContainerTP;
	public UIButton gameServerTP;
	
	public UISprite backgroundText3;
	
	public string controllName;
	
	/// <summary>
	/// The person list.
	/// </summary>
	//美女模型说话//
	private int nameHash = Animator.StringToHash("speak.EmptySpeak");
	private int nameHash1 = 0;
	public GameObject PreatyDialogue;
	public GameObject PreatyDialogueDog;
	public UILabel    PreatyLabel;
	public UILabel    DogSaid;
	public UIButton   [] PersonBtn;
	//public GameObject [] PersonObject;
	
	public UIButton ClickGirlBtn;
	private bool    bIsError = true;//是否是错误提示//
	//直接登陆//
	public UIButton PlayBtn;
	//账号登陆//
	public UIButton AccountsPlayBtn;

	//角色数量//
	protected int mPersonNumber = 3;

	//最近服务器按钮//
	public UIButton ResentlyServerBtn;
	public UIButton ReturnServerBtn;
	//ServerLabel//
	public UILabel ResentlyServerLabel;

	public GameObject ServerObj;
	
	public UIGrid ServerScrollList;
	
	//public 	UIDraggablePanel gsScrollListPanelItem;
	//服务器选择按钮,开始界面中//
	public UIButton SeleteServerBtn;
	//服务器选择的名称// 
	public UILabel SeleteServerLabel;
	//服务器选择界面//
	public GameObject SeleteServerFrame;
	
	public UIButton ZhuXiaoBtn;
	public UIButton ZhuCeBtn;
	public UIButton ZhuCeBtn2;//账号登陆上的注册按钮//
	public UIButton DengLuBtn;
	
	public UIButton ReturnBtn;
	
	public UIButton LoadReturnBtn;
	
	public GameObject XueJiZhuCeFrame;
	
	public UILabel CurrentServerName;
	
	private sg.LS2C_Login_Get_Server_Res.LoginGameServer mServer;
	//注册全新账号//
	public UIButton RegisterNewAccountBtn;
	//注册并绑定已有账号//
	public UIButton RegisterAndBindAccountBtn;

	public GameObject ZhucePanel;
	
	public Transform welcomPanel;
	//------------------------------------------------------------

	public GameObject ServerZonePrefab;
	
	public GameObject mGameObjectL;
	public GameObject mGameObjectR;
	public GameObject mGameObjectCenter;
	public GameObject mGameObjectCen1;


	public UILabel AccountLabel;
	public UIButton SwitchAccountBtn;
	// 选择帐号类型界面 //
	public GameObject ChoiceAccount;
	public  UIButton ChoiceAccountReturnBtn;
	public UIButton ChoiceAccountZhuCeBtn;
	public UIButton OldAccountBtn;
	public UIButton TouristsLandingBtn;

	public GameObject AnnouncementInfor;
	public UILabel AnnouncementLabel;
	public UIButton AnnouncementInforCloseBtn;
	public UIButton AnnouncementInforOpenBtn;

	//游戏设置界面//
	public GameObject SetWindow;
	public UIButton OpenSetWindowBtn;
	public UIButton ClosSetwindoweBtn;
	public UIToggle Definition_Toggle;
	public UIToggle Sound_Toggle;
	public Transform Definition_Ball;
	public Transform Sound_Ball;
	public UILabel accontDesc;

	private static readonly string FIRST_REGISTER_ACCOUNT_DONE_FILE = "firstregisteraccountdone.txt";
	private bool m_RegisterAccountDone = false;
	//-------------------------------------------------------
	public enum ChildPanelType
	{
		None,
		GfanAcc,
		Welcom,
		GameServerList,
	}

	
	//---------------------------------------------------------------
	protected override void Awake()
	{
		if (null == Globals.Instance.MGUIManager)
			return;
		
		base.Awake();
	
		
		UIEventListener.Get(ZhuXiaoBtn.gameObject).onClick += OnClickZhuXiaoBtn;
	
		UIEventListener.Get(ZhuCeBtn.gameObject).onClick += OnClickZhuCeBtn;
		
		UIEventListener.Get(ZhuCeBtn2.gameObject).onClick += OnClickZhuCeBtn2;
		
		UIEventListener.Get(DengLuBtn.gameObject).onClick += OnClickDengLuBtn;
		
		UIEventListener.Get(PlayBtn.gameObject).onClick += OnClickPlayBtn;
		
		UIEventListener.Get(AccountsPlayBtn.gameObject).onClick += OnClickAccountsPlayBtn;
		
		UIEventListener.Get(SeleteServerBtn.gameObject).onClick += OnClickSeleteServerBtn;
		
		UIEventListener.Get(ResentlyServerBtn.gameObject).onClick+= OnClickResentlyServerBtn;
		
		UIEventListener.Get(ReturnServerBtn.gameObject).onClick+= OnClickResentlyServerBtn;
			
		UIEventListener.Get(ReturnBtn.gameObject).onClick+= OnClickReturnBtn;
		
		UIEventListener.Get(LoadReturnBtn.gameObject).onClick+= OnClickLoadReturnBtnBtn;
			
		UIEventListener.Get(RegisterNewAccountBtn.gameObject).onClick += OnClickRegisterNewAccountBtn;
		
		UIEventListener.Get(RegisterAndBindAccountBtn.gameObject).onClick += OnClickRegisterAndBindAccountBtn;
		
//		UIEventListener.Get(ClickGirlBtn.gameObject).onClick += OnClickClickGirlBtn;

		UIEventListener.Get(SwitchAccountBtn.gameObject).onClick += OnClickSwitchAccountBtn;

		UIEventListener.Get(ChoiceAccountZhuCeBtn.gameObject).onClick += OnClickZhuCeBtn;

		UIEventListener.Get(OldAccountBtn.gameObject).onClick += OnClickDengLuBtn;

		UIEventListener.Get(TouristsLandingBtn.gameObject).onClick += OnClickTouristsLandingBtn;

		UIEventListener.Get(ChoiceAccountReturnBtn.gameObject).onClick += delegate(GameObject go) {
			NGUITools.SetTweenActive(ChoiceAccount,false,delegate {			
				NGUITools.SetActive(welcomPanel.gameObject,true);
			});
		};
		UIEventListener.Get(AnnouncementInforCloseBtn.gameObject).onClick += delegate(GameObject go) {
			NGUITools.SetActive(AnnouncementInfor , false);
		};
		UIEventListener.Get(AnnouncementInforOpenBtn.gameObject).onClick += delegate(GameObject go) {
			NGUITools.SetActive(AnnouncementInfor , true);
		};

		if (!GameDefines.Setting_UserDefined)
		{
			///default set use machine physical level//
			SetBallPsition(Definition_Ball,GameDefines.HighPerformenceSystem);
			GameDefines.Setting_ScreenQuality = GameDefines.HighPerformenceSystem;
		}
		else
		{
			SetBallPsition(Definition_Ball,GameDefines.Setting_ScreenQuality);
		}

		Definition_Toggle.value= GameDefines.Setting_ScreenQuality;

		UIEventListener.Get (OpenSetWindowBtn.gameObject).onClick += OpenSetWindow;
		UIEventListener.Get (ClosSetwindoweBtn.gameObject).onClick += CloseSetWindow;
		UIEventListener.Get (Definition_Toggle.gameObject).onClick += ChangeDefinition;
		UIEventListener.Get (Sound_Toggle.gameObject).onClick += ChangeSound;
		Invoke("HideNull",1.5f);
	
		for (int i =0; i< mPersonNumber; i++)
		{
			UIButton btn = PersonBtn[i];
			btn.Data = i;
			UIEventListener.Get(btn.gameObject).onClick += OnClickPersonBtn;
		}

		SetDefault ();
	}
	
	private void OnClickPersonBtn(GameObject obj)
	{
		UIButton btn = obj.GetComponent<UIButton>();
		int iValue =(int) btn.Data;
	}
	
	
	protected virtual void Start ()
	{
		base.Start();

		NGUITools.SetActive(ZhucePanel,false);
		ResentlyServerLabel.text = Globals.Instance.MLSNetManager.CurrGameServer.name;
		SeleteServerLabel.text  = Globals.Instance.MLSNetManager.CurrGameServer.name;	
		if(!string.IsNullOrEmpty(GameDefines.Setting_LoginName))
		{
			AccountLabel.text = GameDefines.Setting_LoginName;
		}else{
			AccountLabel.text = Globals.Instance.MDataTableManager.GetWordText(35070);
		}
	}
	void Update()
	{
	
	}
	public OrbitCamera camera;
	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		base.GUILevel = 4;	
		camera = Globals.Instance.MSceneManager.mMainCamera.transform.parent.GetComponent<OrbitCamera>();
		camera.enabled = false;

		Globals.Instance.MSceneManager.mMainCamera.enabled = false;
		Globals.Instance.MSceneManager.mTaskCamera.enabled = false;
		
		Globals.Instance.MSoundManager.PlaySceneSound("Loading");
	}



	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		if(Globals.Instance.MSceneManager!=null){
			Globals.Instance.MSceneManager.mMainCamera.enabled = true;
			Globals.Instance.MSceneManager.mTaskCamera.enabled = true;
		}
	}
	
	void HideNull()
	{
		UIButton NullButton = this.transform.Find("ButtonNull").GetComponent<UIButton>();
		NGUITools.SetActive(NullButton.gameObject,false);
	}
	
	void AutoSkipAccount()
	{
		SetVisible(false);
		ThirdPartyPlatform.AtlanticLogin(false, GameDefines.Setting_LoginName, GameDefines.Setting_LoginPass);
	}
	

	public void OnTipsUpdate()
	{
		mGameObjectCen1.transform.localPosition -=  new Vector3 (0, 15*(mGameObjectCen1.transform.localScale.y  -1),0);
	}
	public void OnTipsFinshed1()
	{
		SwitchToPanel(ChildPanelType.GameServerList);
		mGameObjectCen1.transform.localScale = new Vector3(1,1,1);
		mGameObjectCen1.transform.localPosition = new Vector3 (0,0,0);
	}
	public void UpdateGUI()
	{
		SwitchToPanel(ChildPanelType.Welcom);
	}
	
	public void UpdateGUINonePlatform()
	{
		if (!IsRegisterAccountDone())
		{
			SetVisible(true);

			welcomPanel.gameObject.SetActiveRecursively(false);
			//gsListPanel.gameObject.SetActiveRecursively(false);
		}
		else{
			SwitchToPanel(ChildPanelType.Welcom);
		}
		
	}
	
	private void RegisterAccountEvent(GameObject obj)
	{
		Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
		{
				gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
				gui.SetDialogType(EDialogType.REGISTER_ACCOUNT, null);
		},EDialogStyle.DialogOk, delegate() 
		{
			//registerAccountBtn.transform.localScale = Vector3.zero;
		
			SetFirstEnterGameDone();
		
			SwitchToPanel(ChildPanelType.Welcom);
		}
		);
	}
	
	public bool IsRegisterAccountDone(){
		if(!m_RegisterAccountDone){
			
			// read from filesystem
			try{
				string tFilename = Application.persistentDataPath + "/" + Globals.Instance.MLSNetManager.CurrGameServer.id + FIRST_REGISTER_ACCOUNT_DONE_FILE;
				FileStream t_file = new FileStream(tFilename,FileMode.Open,FileAccess.Read);
				t_file.Close();
				m_RegisterAccountDone = true;
				
			}catch(System.Exception ex){
				m_RegisterAccountDone = false;
				Debug.Log("IsRegisterAccountDone is false " + ex.Message);
			}
		}
		
		return m_RegisterAccountDone;
	}
	
	public void SetFirstEnterGameDone(){
		// S filesystem
		try{
			string tFilename = Application.persistentDataPath + "/" + Globals.Instance.MLSNetManager.CurrGameServer.id + FIRST_REGISTER_ACCOUNT_DONE_FILE;
			FileStream t_file = new FileStream(tFilename,FileMode.Create,FileAccess.Write);
			t_file.Close();
		}catch(System.Exception ex){
			Debug.LogError( FIRST_REGISTER_ACCOUNT_DONE_FILE + " create failed! Error: " + ex.Message);
		}
		
		m_RegisterAccountDone = true;
	}
	
	public void EnterWelcomGUI()
	{	
		GUIRadarScan.Hide();
		SwitchToPanel(ChildPanelType.Welcom);
		PlayGame();
	}
	
	public void enterAccountGUI()
	{
		GUIRadarScan.Hide();
		SwitchToPanel(ChildPanelType.Welcom);
	}

	public void CheckAnnouncement(string str)
	{
		AnnouncementLabel.text = str;
		string lastLoginTime = TaskManager.getDailyFristLoginTime();
		if(lastLoginTime == "" || System.DateTime.Parse(lastLoginTime).Day != System.DateTime.Now.Day || 
		   System.DateTime.Parse(lastLoginTime).Month != System.DateTime.Now.Month ||
		   System.DateTime.Parse(lastLoginTime).Year != System.DateTime.Now.Year )
		{
			NGUITools.SetActive(AnnouncementInfor , true);
		}
		TaskManager.SetDailyFristLoginTime(System.DateTime.Now.ToString());
	}

	void SwitchToPanel(ChildPanelType type)
	{

		welcomPanel.gameObject.SetActiveRecursively(type == ChildPanelType.Welcom);
		if (type == ChildPanelType.Welcom)
			UpdateWelcomGUI();

	}
	
	void UpdateAccountGUI()
	{
		//enterGameBtn.transform.localScale = Vector3.zero;
		if (GameDefines.Setting_IsAutoLogin)
		{
			SwitchToPanel(ChildPanelType.None);
			AutoSkipAccount();
		}
		else
		{

		}
	}
	
	void UpdateWelcomGUI()
	{
		DisplayButton();
		if (Globals.Instance.IsSwitchServer)
		{
			Globals.Instance.IsSwitchServer = false;
			SwitchToPanel(ChildPanelType.GameServerList);
		}
		else
		{
		}
	}
	
	void UpdatePlayerData()
	{

	}
	

	
	void OnConfirmGameServer(GameObject obj)
	{
		UIButton targetBtn = obj.transform.GetComponent<UIButton>();
		sg.LS2C_Login_Get_Server_Res.LoginGameServer gameServer = (sg.LS2C_Login_Get_Server_Res.LoginGameServer)targetBtn.Data;
		Globals.Instance.MLSNetManager.CurrGameServer = gameServer;
		
		EnterWelcomGUI();
		// SelectGSBtn(obj);
	}
	
	private void OnClickSwitchAccountBtn(GameObject obj) 
	{
		NGUITools.SetActive(welcomPanel.gameObject,false);

		NGUITools.SetTweenActive(ChoiceAccount,true,delegate {			
		});
	}
	
	private void OnClickZhuXiaoBtn(GameObject obj) 
	{
		GameDefines.Setting_LoginName = "";
		GameDefines.Setting_LoginPass = "";
		GameDefines.Setting_IsGuest = true;
		//gfanAccPanel.gameObject.SetActiveRecursively(false);
		welcomPanel.gameObject.SetActiveRecursively(true);
		//gsListPanel.gameObject.SetActiveRecursively(false);	
		HideWholeDialogue();
		DisplayButton();
	}
	
	private void OnClickZhuCeBtn(GameObject obj)
	{	
		HideWholeDialogue();
		NGUITools.SetActive(welcomPanel.gameObject,false);
		NGUITools.SetActive(ChoiceAccount,false);

		NGUITools.SetTweenActive(XueJiZhuCeFrame,true,delegate {			
		});
		NGUITools.SetActive(SeleteServerBtn.gameObject,false);
		NGUITools.SetActive(accontDesc.gameObject,Globals.Instance.MSceneManager.isEmail);
	}
	
	private void OnClickZhuCeBtn2(GameObject obj)
	{	
		HideWholeDialogue();
		welcomPanel.gameObject.SetActiveRecursively(false);
		NGUITools.SetTweenActive(ZhucePanel,false,delegate {			
		});
		NGUITools.SetTweenActive(XueJiZhuCeFrame,true,delegate {			
		});
		NGUITools.SetActive(SeleteServerBtn.gameObject,false);
	}
	private void OnClickRegisterNewAccountBtn(GameObject obj)
	{
		UIInput IDLabel = XueJiZhuCeFrame.transform.Find("IDInput").GetComponent<UIInput>();
		UIInput PassWordLabel = XueJiZhuCeFrame.transform.Find("PassWordInput").GetComponent<UIInput>();
		UIInput ConfirmPassWordLabel = XueJiZhuCeFrame.transform.Find("ConfirePWInput").GetComponent<UIInput>();
		GameObject PointOut = XueJiZhuCeFrame.transform.Find("BeautySprit").gameObject;
		UILabel PointOutLabel = XueJiZhuCeFrame.transform.Find("BeautySprit").Find("DialogueLabel").GetComponent<UILabel>();
		
		string account = IDLabel.value;//accountInput.text;
		string password = PassWordLabel.value;// passwordInput.text;
		string confirm = ConfirmPassWordLabel.value;

		if( string.IsNullOrEmpty(account)  )
		{
			//NGUITools.SetActive(PointOut,true);
			bIsError = true;
			DogSaid.text = GUIFontColor.Red255000000 + "账号不能为空"; 
			DisplayDialogueDog(true);
			return;
		}
		else if(account.Length > 20 || account.Length < 4 || !System.Text.RegularExpressions.Regex.IsMatch(account,@"^[A-Za-z0-9\.\@]+$"))
		{
			//NGUITools.SetActive(PointOut,true);
			bIsError = true;
			if(Globals.Instance.MSceneManager.isEmail)
				DogSaid.text = GUIFontColor.Red255000000 + "账号格式不正确"; 
			else
				DogSaid.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10004); 
			DisplayDialogueDog(true);
			return;
		}
		else if(password.Length > 16 || password.Length < 6 || !System.Text.RegularExpressions.Regex.IsMatch(password,@"^[A-Za-z0-9\.\@]+$"))
		{
			//NGUITools.SetActive(PointOut,true);
			bIsError = true;
			DogSaid.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10005); 
			DisplayDialogueDog(true);
			return;
		}
		else if(!string.Equals(password,confirm))
		{
			//NGUITools.SetActive(PointOut,true);
			bIsError = true;
			DogSaid.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10006); 
			DisplayDialogueDog(true);
			return;
		}
	
		if(Globals.Instance.MSceneManager.isEmail)
		{
			if(!IsEmail(account))
			{
				bIsError = true;
				DogSaid.text = GUIFontColor.Red255000000 + "请使用邮箱注册账号"; 
				DisplayDialogueDog(true);
				return;
			}
		}

		string deviceUniqueId = GameDefines.systemUDID;
				
		string cooperateId = GameDefines.PlatformApp;
				
		if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay) 
		{
			cooperateId = GameDefines.PlatformWPay;
			deviceUniqueId = SystemInfo.deviceUniqueIdentifier;
		}
				
				
		
		GameDefines.TempSetting_LoginName = account;
		GameDefines.TempSetting_LoginPass = password;
		GameDefines.TempSetting_IsGuest = false;
//		DisplayDialogue(true);
		NetSender.Instance.C2GSRegisterAccount(account,password,"mailadress",deviceUniqueId,cooperateId,GameDefines.GameVersion,1,0,false,"");
	}

	public bool IsEmail(string str)
	{
		Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");  
		return r.IsMatch(str);

//		if (r.IsMatch(str))
//		{
//			Debug.LogError("This is a true email");
//			return true;
//		}
//		else
//		{
//			Debug.LogError("This is not a email");
//			return false;
//		}
	}


	private void OnClickRegisterAndBindAccountBtn(GameObject obj)
	{
		UIInput IDLabel = XueJiZhuCeFrame.transform.Find("IDInput").GetComponent<UIInput>();
		UIInput PassWordLabel = XueJiZhuCeFrame.transform.Find("PassWordInput").GetComponent<UIInput>();
		UIInput ConfirmPassWordLabel = XueJiZhuCeFrame.transform.Find("ConfirePWInput").GetComponent<UIInput>();
		GameObject PointOut = XueJiZhuCeFrame.transform.Find("BeautySprit").gameObject;
		UILabel PointOutLabel = XueJiZhuCeFrame.transform.Find("BeautySprit").Find("DialogueLabel").GetComponent<UILabel>();
		
		string account = IDLabel.value;//accountInput.text;
		string password = PassWordLabel.value;// passwordInput.text;
		string confirm = ConfirmPassWordLabel.value;

		if( string.IsNullOrEmpty(account)  )
		{
			//NGUITools.SetActive(PointOut,true);
			bIsError = true;
			DogSaid.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10004); 
			DisplayDialogueDog(true);
			return;
		}
		else if(account.Length > 20 || account.Length < 4 || !System.Text.RegularExpressions.Regex.IsMatch(account,@"^[A-Za-z0-9\.\@]+$"))
		{
			//NGUITools.SetActive(PointOut,true);
			bIsError = true;
			DogSaid.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10004); 
			DisplayDialogueDog(true);
			return;
		}
		else if(password.Length > 16 || password.Length < 6 || !System.Text.RegularExpressions.Regex.IsMatch(password,@"^[A-Za-z0-9\.\@]+$"))
		{
			//NGUITools.SetActive(PointOut,true);
			bIsError = true;
			DogSaid.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10005); 
			DisplayDialogueDog(true);
			return;
		}
		else if(!string.Equals(password,confirm))
		{
			//NGUITools.SetActive(PointOut,true);
			bIsError = true;
			DogSaid.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10006); 
			DisplayDialogueDog(true);
			return;
		}

		string deviceUniqueId = GameDefines.systemUDID;

		string cooperateId = GameDefines.PlatformApp;

		if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay) 
		{
			cooperateId = GameDefines.PlatformWPay;
			deviceUniqueId = SystemInfo.deviceUniqueIdentifier;
		}
		
		GameDefines.TempSetting_LoginName = account;
		GameDefines.TempSetting_LoginPass = password;
		GameDefines.TempSetting_IsGuest = false;
		NetSender.Instance.C2GSRegisterAccount(account,password,"mailadress",deviceUniqueId,cooperateId,GameDefines.GameVersion,3,0,true,GameDefines.System_GuestAccount);
	}

	private void OnClickTouristsLandingBtn(GameObject obj)
	{
		GameDefines.Setting_LoginName = "";
		GameDefines.Setting_LoginPass = "";
		GameDefines.Setting_IsGuest = true;

		OnClickPlayBtn(null);
	}
	
	private void OnClickDengLuBtn(GameObject obj)
	{
		NGUITools.SetActive(ChoiceAccount,false);
		NGUITools.SetTweenActive(ZhucePanel,true,delegate {			
		});		
		HideWholeDialogue();
		welcomPanel.gameObject.SetActiveRecursively(false);
	}
			
	private void OnClickPlayBtn(GameObject obj)
	{
		GUIRadarScan.Show();
		string account = "";
		string password = "";
		
		// Enter the user srp6 test and verify workflow
		if(string.IsNullOrEmpty(GameDefines.Setting_LoginName))
		{
			GameDefines.Setting_IsGuest = true;
			GameDefines.TempSetting_LoginName = account;
			GameDefines.TempSetting_LoginPass = password;
			Globals.Instance.MLSNetManager.RequestLoginChallenge("", "");
		}
		else
		{
			account = GameDefines.Setting_LoginName;
			password = GameDefines.Setting_LoginPass;
			GameDefines.Setting_IsGuest = false;
			//GameDefines.Setting_LogingServer = mServer.name;
			GameDefines.TempSetting_LoginName = account;
			GameDefines.TempSetting_LoginPass = password;
			Globals.Instance.MLSNetManager.RequestLoginChallenge(account, password,0);
		}

		//bannerView.Destroy();
	}
	
	private void OnClickAccountsPlayBtn(GameObject obj)
	{
		
		UIInput IDLabel = ZhucePanel.transform.Find("IDInput").GetComponent<UIInput>();
		UIInput PassWordLabel= ZhucePanel.transform.Find("PassWordInput").GetComponent<UIInput>();
		
		
		if( string.IsNullOrEmpty(IDLabel.value)  )
		{
			bIsError = true;
			PreatyLabel.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10008); 
			DisplayDialogue(true);
			return;
		}
		else if(IDLabel.value.Length > 20 || IDLabel.value.Length < 4 || !System.Text.RegularExpressions.Regex.IsMatch(IDLabel.value,@"^[A-Za-z0-9\.\@]+$"))
		{
			bIsError = true;
			PreatyLabel.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10004); 
			DisplayDialogue(true);
			return;
		}
		else if(PassWordLabel.value.Length > 16 || PassWordLabel.value.Length < 6 || !System.Text.RegularExpressions.Regex.IsMatch(PassWordLabel.value,@"^[A-Za-z0-9\.\@]+$"))
		{
			bIsError = true;
			PreatyLabel.text = GUIFontColor.Red255000000 + Globals.Instance.MDataTableManager.GetWordText(10005); 
			DisplayDialogue(true);
			return;
		}
		
		
		GameDefines.TempSetting_LoginName = IDLabel.value;
		GameDefines.TempSetting_LoginPass = PassWordLabel.value;
		GameDefines.Setting_IsGuest = false;
		Globals.Instance.MLSNetManager.RequestLoginChallenge(IDLabel.value, PassWordLabel.value,0);

	}
	
	private void OnClickSeleteServerBtn(GameObject obj)
	{
		NGUITools.SetActive(SeleteServerBtn.gameObject,false);
		NGUITools.SetActive(XueJiZhuCeFrame,false);
		NGUITools.SetActive(ZhucePanel,false);	
//		NGUITools.SetActive(SeleteServerFrame,true);	
//		welcomPanel.gameObject.SetActiveRecursively(false);
		NGUITools.SetActive(welcomPanel.gameObject,false);
		HelpUtil.DelListInfo(ServerScrollList.transform);
		ServerScrollList.repositionNow = true; 
		int i = 0;
		GameObject serverObject = null;
		UILabel label;
		UIButton imageButton = null;
		foreach (sg.LS2C_Login_Get_Server_Res.LoginGameServer gameServer in Globals.Instance.MLSNetManager.AllGameServers)
		{
			serverObject = GameObject.Instantiate(ServerObj) as GameObject;		
			serverObject.transform.parent = ServerScrollList.transform;
			serverObject.transform.localPosition = new Vector3(0,0,-25.0f);
			serverObject.name = "ServerBtn" + i.ToString();

			label = serverObject.transform.Find("Label").GetComponent<UILabel>();
			label.text = gameServer.name;
			imageButton = serverObject.transform.GetComponent<UIButton>();
			imageButton.Data = gameServer;
			
			UISprite hotSprite = serverObject.transform.Find("HotSprit").GetComponent<UISprite>();
			NGUITools.SetActive(hotSprite.gameObject,gameServer.recommendState == 2);	
		
			UISprite newSprite = serverObject.transform.Find("NewSprit").GetComponent<UISprite>();
			NGUITools.SetActive(newSprite.gameObject,gameServer.recommendState == 1);
		
			
			UIEventListener.Get(imageButton.gameObject).onClick += OnClickImageServerBtn;
			//serverObject.transform.localPosition = new Vector3(0,0, -10);
			serverObject.transform.localScale = Vector3.one;
			++i;
		}
//	    TweenPosition tw = TweenPosition.Begin(SeleteServerFrame,0.8f,new Vector3(0,0,0));
			
		ResentlyServerLabel.text = Globals.Instance.MLSNetManager.CurrGameServer.name;
		SeleteServerLabel.text = Globals.Instance.MLSNetManager.CurrGameServer.name;
		ServerScrollList.repositionNow = true;
		NGUITools.SetTweenActive(SeleteServerFrame,true,delegate {			
		});
		//gsScrollListPanelItem.ResetPosition();
		

	}
	
	private void OnClickResentlyServerBtn(GameObject obj)
	{
		HideWholeDialogue();
		NGUITools.SetTweenActive(SeleteServerFrame,false,delegate {	
			ShowAndHide();
		});	
	}
	private void OnClickReturnBtn(GameObject obj)
	{
		NGUITools.SetTweenActive(ZhucePanel,false,delegate {	
			ShowAndHide();
		});	
		HideWholeDialogue();

	}
	
	private void OnClickLoadReturnBtnBtn(GameObject obj)
	{
		NGUITools.SetActive(ZhucePanel,false);	
		NGUITools.SetTweenActive(XueJiZhuCeFrame,false,delegate {
			ShowAndHide();
		});
		HideWholeDialogue();

	}
	void ShowAndHide()
	{
		welcomPanel.gameObject.SetActiveRecursively(true);
		NGUITools.SetActive(SeleteServerBtn.gameObject,true);
		DisplayButton();
	}
	private void OnClickImageServerBtn(GameObject obj)
	{
		UIButton btn = obj.GetComponent<UIButton>();
		mServer  =(sg.LS2C_Login_Get_Server_Res.LoginGameServer) btn.Data;
		Globals.Instance.MLSNetManager.CurrGameServer = mServer;
		HideWholeDialogue();
		SeleteServerLabel.text = mServer.name;
		ResentlyServerLabel.text = mServer.name;
		NGUITools.SetTweenActive(SeleteServerFrame,false,delegate {	
			ShowAndHide();
		});
	}
	
//    private void OnClickClickGirlBtn(GameObject obj)
//	{
//		bIsError = false;
//		Debug.Log("hate");
//	}
	
	private void DisplayDialogue(bool bIsDisplay = true)
	{
		NGUITools.SetActive(PreatyDialogue,bIsDisplay);
		TweenScale []tweenScales = PreatyDialogue.transform.GetComponents<TweenScale>();
	
		EventDelegate.Add(tweenScales[1].onFinished , delegate (){
			NGUITools.SetActive(PreatyDialogue,false);	
		});
		tweenScales[0].ResetToBeginning();
		tweenScales[0].Play(true);
		tweenScales[1].ResetToBeginning();
		tweenScales[1].Play(true);
		
		
		
		//tweenScales[1].enabled = true;

		if(bIsDisplay)
		{
		
			if(!bIsError)
			{
				int index = Random.Range(12001,12010);
				string wordText = Globals.Instance.MDataTableManager.GetWordText(index);
				PreatyLabel.text = "    " + wordText;
			}
			else
			{
				
			}
		
		}
	}
	private void DisplayDialogueDog(bool bIsDisplay = true)
	{
		NGUITools.SetActive(PreatyDialogueDog,bIsDisplay);
		TweenScale []tweenScales = PreatyDialogueDog.transform.GetComponents<TweenScale>();
	
		EventDelegate.Add(tweenScales[1].onFinished , delegate (){
			NGUITools.SetActive(PreatyDialogueDog,false);	
		});
		tweenScales[0].ResetToBeginning();
		tweenScales[0].Play(true);
		tweenScales[1].ResetToBeginning();
		tweenScales[1].Play(true);
		
		
		
		//tweenScales[1].enabled = true;

		if(bIsDisplay)
		{
		
			if(!bIsError)
			{
				int index = Random.Range(12001,12010);
				string wordText = Globals.Instance.MDataTableManager.GetWordText(index);
				DogSaid.text = "    " + wordText;
			}
			else
			{
				
			}
		
		}
	}
	private void HideWholeDialogue()
	{
		NGUITools.SetActive(PreatyDialogueDog,false);
		NGUITools.SetActive(PreatyDialogue,false);			
	}
	private void PlayGame()
	{
		Globals.Instance.MGSNetManager.Disconnect();
		long accId = Globals.Instance.MGameDataManager.MActorData.PlayerID;
		int serverId = Globals.Instance.MLSNetManager.CurrGameServer.id;
		NetSender.Instance.RequestLoginConfirmGameServer(accId, serverId);
	}
	private void DisplayButton()
	{
		UILabel label = ZhuXiaoBtn.transform.Find("UILable").GetComponent<UILabel>();
		if(string.IsNullOrEmpty(GameDefines.Setting_LoginName))
		{
			NGUITools.SetActive(DengLuBtn.gameObject,true);
			NGUITools.SetActive(label.gameObject,false);
			NGUITools.SetActive(ZhuXiaoBtn.gameObject,false);
			
		}
		else
		{
			NGUITools.SetActive(DengLuBtn.gameObject,false);
			NGUITools.SetActive(label.gameObject,true);
			NGUITools.SetActive(ZhuXiaoBtn.gameObject,true);
			label.text = GameDefines.Setting_LoginName;
			NGUITools.SetActive(RegisterAndBindAccountBtn.gameObject,false);
		}
	}
	public void SetCurrentServerName()
	{
		CurrentServerName.text = Globals.Instance.MLSNetManager.CurrGameServer.name;
	}
	public void ErrorMessage(int iError)
	{

		bIsError = true;
		string text = Globals.Instance.MDataTableManager.GetErrorCodeText(iError);
		DogSaid.text = text;
		DisplayDialogueDog(true);
		return;	
	}
	public void LoadingErrorMessage(int iError)
	{
		//Globals.Instance.MGUIManager.ShowErrorTips(iError);
		bIsError = true;
		string text = Globals.Instance.MDataTableManager.GetErrorCodeText(iError);
		PreatyLabel.text = GUIFontColor.Red255000000 + text;
		DisplayDialogue(true);
//		Globals.Instance.MGUIManager.ShowSimpleCenterTips(iError);
	}

	public void OpenSetWindow(GameObject obj)
	{
		NGUITools.SetActive (SetWindow, true);
	}
	public void CloseSetWindow(GameObject obj)
	{
		NGUITools.SetActive (SetWindow, false);
	}

	public void ChangeDefinition(GameObject obj)
	{
		GameDefines.Setting_UserDefined = true;
		bool state = obj.GetComponent<UIToggle> ().value;
		SetBallPsition(Definition_Ball,state);
		if (state) {
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(9001);
			GameDefines.Setting_ScreenQuality = true;
		} else {
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(9002);
			GameDefines.Setting_ScreenQuality = false;
		}

		GameDefines.WriteConfigFile ();
	}
	//public static int	Setting_MusicVol		= 100;
	//public static int	Setting_SoundVol		= 100;
	public void ChangeSound(GameObject obj)
	{
		bool state = obj.GetComponent<UIToggle> ().value;
		SetBallPsition(Sound_Ball,state);
		if (state) {
			GameDefines.Setting_MusicVol=100;
			GameDefines.Setting_SoundVol=100;
			SoundManager.CurrentPlayingMusicAudio.volume=1;
		} else {
			GameDefines.Setting_MusicVol=0;
			GameDefines.Setting_SoundVol=0;
			SoundManager.CurrentPlayingMusicAudio.volume=0;
		}
		GameDefines.WriteConfigFile ();
	}

	public void SetBallPsition(Transform ball,bool state)
	{
		if (state)
			ball.localPosition = new Vector3 (50, -1, 0);
		else
			ball.localPosition = new Vector3 (-50, -1, 0);
	}
	private void SetDefault()
	{
		//设置声音
		if (GameDefines.Setting_MusicVol == 100) {
			SetBallPsition (Sound_Ball, true);
			Sound_Toggle.value=true;
		} else if(GameDefines.Setting_MusicVol == 0) {
			SetBallPsition(Sound_Ball,false);
			Sound_Toggle.value=false;
		}
	}
}
