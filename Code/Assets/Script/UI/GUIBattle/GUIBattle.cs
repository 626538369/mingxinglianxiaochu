using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GUIBattle : GUIWindow 
{
	//---------------------------------------------
	public GameObject BuffIconTp;
	public float BUFF_HORIZITAL_SPACE = -5.0f;
	public float BUFF_VERTICAL_SPACE = 10.0f;
	public int BUFF_COUNT_PER_ROW = 5;
	
	public PackedSprite bSkillEffectFont;
	public PackedSprite bNumber;
	public SpriteText skillText;
	public UISlider progressBarBlood;
	EQuickBattleBtnType quickBtnType = EQuickBattleBtnType.Invalid;
	
	public enum EButtonType
	{
		BtnWaveOk,
		BtnGetReward,
		BtnDefenseExit
	}
	
	public enum EQuickBattleBtnType
	{
		Visible,
		VisibleDelay,
		Invalid
	}
		
	protected void Awake()
	{
		base.Awake();
	}
	
	protected void OnDestroy()
	{
		_mCurrentWarShip = null;
		UnRegisterSubscriber();
		base.OnDestroy();
	}
	
	protected void Start()
	{
		base.Start();
		if(!Application.isPlaying) return;
		InitializeGUI();
		//SetQuickFightCameraButtonVisible(false);
	}
	
	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		
		// tzz added for custom camera button
		//m_customCameraBtn = transform.FindChild("CustomCameraBtn").GetComponent<UIToggle>();
		//m_customCameraBtn.SetToggleState(BattleStatus.CustomCameraState?1:0);
		//UIEventListener.Get(m_customCameraBtn).onClick += OnCustomCameraBtnClick;
		
		_mRootTransform = this.transform;
		_mRootTransform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		_mRootTransform.localPosition = new Vector3(0.0f, 0, 10f);
		
		_mQuickFightBtn = _mRootTransform.Find("QuickFightBtn").GetComponent<UIButton>() as UIButton;
		UIEventListener.Get(_mQuickFightBtn.gameObject).onClick += OnClickQuickFightBtn;
						
		_mBuffInfoRoot = _mRootTransform.Find("BuffInfo");
		
		_mDescriptionText = _mRootTransform.Find("Description").GetComponent<UIButton>() as UIButton;
		
		_mShipInfoRoot = _mRootTransform.Find("WarshipInfo");
		
		_mWarshipName = _mShipInfoRoot.Find("NameText").GetComponent( typeof(UIButton) ) as UIButton;
		_mWarshipLife = _mShipInfoRoot.Find("LifeText").GetComponent( typeof(UIButton) ) as UIButton;
		_mWarshipDander = _mShipInfoRoot.Find("DanderText").GetComponent( typeof(UIButton) ) as UIButton;
		
		// _mSkillBtn = _mShipInfoRoot.FindChild("SkillIcon").GetComponent( typeof(UIButton) ) as UIButton;
		UIEventListener.Get(_mSkillBtn.gameObject).onClick += OnClickSkillBtn ;
		// _mSkillIcon = _mSkillBtn.transform.FindChild("Icon").GetComponent( typeof(PackedSprite) ) as PackedSprite;
		
		_mShipInfoRoot.gameObject.SetActiveRecursively(false);
		_mDescriptionText.gameObject.SetActiveRecursively(false);
		
		UIButton btn = BuffIconTp.GetComponent<UIButton>() as UIButton;
		_mBuffIconWidth = btn.GetComponent<Collider>().bounds.size.x;
		_mBuffIconHeight =  btn.GetComponent<Collider>().bounds.size.y;
		
		InitBuffData();
		SetShipInfoVisible(false);
		
		RegisterSubscriber();
		base.GUILevel = 8;
		uiPortDefense = transform.Find("UIPortDefense");
		//uiPortDefense.gameObject.SetActiveRecursively(false);
		// if this battle is port defense 
		if(GameStatusManager.Instance.MBattleStatus.MBattleResult.BattleType == GameData.BattleGameData.BattleType.PROT_DEFENSE_BATTLE)
		{
			moveHeadPanelL = uiPortDefense.Find("MoveHeadPanelL");
			moveHeadPanelR = uiPortDefense.Find("MoveHeadPanelR");
			VS = uiPortDefense.Find("VS");
			VS.gameObject.SetActiveRecursively(false);
			bottomHead = uiPortDefense.Find("BottomHead");
			bottomHead.gameObject.SetActiveRecursively(false);
			portDefenseDialog = uiPortDefense.Find("PortDefenseDialog");
			InitPortDefenseSprite();
		}
		else
		{
			uiPortDefense.gameObject.SetActiveRecursively(false);
		}

		//init the ship blood control
		_bloodControl = new BattleBloodControl();
		_bloodControl.effectFontPreb = bSkillEffectFont;
		_bloodControl.numberPreb = bNumber;
		_bloodControl.textSkillPreb = skillText;
		_bloodControl.progressBarBlood = progressBarBlood;
		
		//调用数据
		UpdateData();
	}
	
	public void UpdateData()
	{
		if(GameStatusManager.Instance.MBattleStatus.MBattleResult.BattleType == GameData.BattleGameData.BattleType.PROT_DEFENSE_BATTLE)
		{
			if(Globals.Instance.MPortDefenseManager.puShowPK)
			{
				Globals.Instance.MPortDefenseManager.puShowPK = false;
			}
			else
			{
				GameStatusManager.Instance.MBattleStatus.InitBattleData();
				UpdateBattleDefUIData();
			}
		}
		else
		{
			//模拟战斗数据
			GameStatusManager.Instance.MBattleStatus.InitBattleData();
		}
	}
	
	// init the ui of port defense
	public void InitPortDefenseSprite()
	{
		for(int i = 0; i < 3; i++)
		{
			AvatarL[i] = moveHeadPanelL.Find("HeadBG" + (i + 1)).Find("Avatar").GetComponent(typeof(PackedSprite)) as PackedSprite;
			NameL[i] = moveHeadPanelL.Find("HeadBG" + (i + 1)).Find("Name").GetComponent(typeof(SpriteText)) as SpriteText; 
			AvatarR[i] = moveHeadPanelR.Find("HeadBG" + (i + 1)).Find("Avatar").GetComponent(typeof(PackedSprite)) as PackedSprite;
			NameR[i]= moveHeadPanelR.Find("HeadBG" + (i + 1)).Find("Name").GetComponent(typeof(SpriteText)) as SpriteText; 
		}
		
		textAssistantBG =  uiPortDefense.Find("PackedSprite15").GetComponent(typeof(PackedSprite)) as PackedSprite;
		textAssistant = uiPortDefense.Find("PackedSprite15/TextAssistant").GetComponent(typeof(SpriteText)) as SpriteText;
		textAssistantBG.gameObject.SetActiveRecursively(false);
		SpriteText textOutTitle = uiPortDefense.Find("BattleOutInfo").Find("TextTitle").GetComponent(typeof(SpriteText)) as SpriteText;
		
		textOutTitle.Text = GUIFontColor.Gold254221001 + Globals.Instance.MDataTableManager.GetWordText(23400025);
		
		textOutInfo = uiPortDefense.Find("BattleOutInfo").Find("TextInfo").GetComponent(typeof(SpriteText)) as SpriteText;
	
		for(int i = 0; i < 3; i++)
		{
			Transform headBG = bottomHead.Find("HeadBGSmall" + (i + 1));
			avatarDown[i] = headBG.Find("Avatar").GetComponent(typeof(PackedSprite)) as PackedSprite;
			nameDown[i] = headBG.Find("Name").GetComponent(typeof(SpriteText)) as SpriteText;
			battleInfoDown[i] = headBG.Find("Info").GetComponent(typeof(SpriteText)) as SpriteText;
		}
		textWave = uiPortDefense.Find("BattleWaveBG").Find("Text").GetComponent(typeof(SpriteText)) as SpriteText;
		
		
		// port defense wave result dialog
		btnWaveResultOk = portDefenseDialog.Find("ButtonOK").GetComponent(typeof(UIButton)) as UIButton;
         //UIEventListener.Get(	btnWaveResultOk.gameObject).onClick += ButtonUpDelegate;
			
		int tIndex = 1;
		while(tIndex < 4)
		{
			Transform tempPanner = portDefenseDialog.Find("Panner" + tIndex);
			int tSubIndex = 0;
			while(tSubIndex < 6)
			{
				textPanner[tIndex - 1,tSubIndex] = tempPanner.Find("Info" + tSubIndex).GetComponent(typeof(SpriteText)) as SpriteText;
				tSubIndex++;
			}
			tIndex++;
		}
		
		tIndex = 1;
		while(tIndex < 3)
		{
			Transform temp = portDefenseDialog.Find("Help" + tIndex);
			helpInfo[tIndex - 1] = temp.gameObject;
			int tSubIndex = 0;
			while(tSubIndex < 8)
			{
				textHelpInfo[tIndex - 1,tSubIndex] = temp.Find("Info" + tSubIndex).GetComponent(typeof(SpriteText)) as SpriteText;
				tSubIndex++;
			}
			tIndex++;
		}
		
		textJiangli = portDefenseDialog.Find("TextJiangli").GetComponent(typeof(SpriteText)) as SpriteText;
		
		tipBG = portDefenseDialog.Find("Tips").gameObject;
		textTip = tipBG.transform.Find("Info").GetComponent(typeof(SpriteText)) as SpriteText;
		
		textWaveTitle = portDefenseDialog.Find("Title").GetComponent(typeof(SpriteText)) as  SpriteText;
		
		portDefenseDialog.gameObject.SetActiveRecursively(false);
	}
	
	public IEnumerator DelayUpdateData()
	{
		yield return new WaitForSeconds(1);
		moveHeadPanelL.gameObject.SetActiveRecursively(false);
		moveHeadPanelR.gameObject.SetActiveRecursively(false);
		VS.gameObject.SetActiveRecursively(false);
		
		GameStatusManager.Instance.MBattleStatus.InitBattleData();
		yield return new WaitForSeconds(1);
		UpdateBattleDefUIData();
	}
	
	
	public void UpdateBattleDefUIData()
	{
		
	}
	
	string RoleID2RoleName(long roleID)
	{

		return string.Empty;
	}
	

	
	public void UpdateWaveResultDialogData()
	{	
	
	}
	
	public void ButtonUpdelegate(GameObject obj)
	{

	}
	
	//goto port and show the ui of portdefense
	IEnumerator ToPortDefenseUI(int timeDelay)
	{
		yield return new WaitForSeconds(timeDelay);
		GUIBattle gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
		if (null != gui)
			gui.Close();
		
		GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_PORT);
		Globals.Instance.MSceneManager.mMainCamera.transform.position = BattleStatus.CamOrignalPos;
		
		// show the gui port defense view
		Globals.Instance.MPortDefenseManager.EndDefenseBattle();
	}
	
	protected override void Update ()
	{
		base.Update ();
	}
	
	
	/**
	 * tzz added
	 * set visible of quickFightButton for teach secne
	 * check TeachFirstEnterGame.cs for detail
	 */ 
	public void SetQuickFightCameraButtonVisible(bool _visible){
		if(_mQuickFightBtn != null){
			_mQuickFightBtn.gameObject.SetActiveRecursively(_visible);
		}
		
		//if(m_customCameraBtn != null){
		//	m_customCameraBtn.gameObject.SetActiveRecursively(_visible);
		//}
	}
	
	public void UpdateData(WarshipL ship)
	{
		// Update buff information
		_mCurrentWarShip = ship;
		
		GirlData data = _mCurrentWarShip.GirlData;
		if (null == data)
			return;
		
		_mShipInfoRoot.gameObject.SetActiveRecursively(true);
//		SetWarshipName(data.BasicData.Name);
		SetWarshipLife(data.PropertyData.Life, data.PropertyData.MaxLife);
		SetWarshipDander(data.PropertyData.Power, data.PropertyData.MaxPower);
		
		// Check skill
		SetSkillInfo(data);
		
		UpdateBuffData(ship);
	}
	
	public void UpdateShipInfo(WarshipL ship)
	{
		GirlData data = ship.GirlData;
		if (null == data)
		{
			return;
		}
		
		SetShipInfoVisible(true);
		SetWarshipLife(ship.GirlData.PropertyData.Life, ship.GirlData.PropertyData.MaxLife);
		SetWarshipDander(ship.GirlData.PropertyData.Power, ship.GirlData.PropertyData.MaxPower);
		SetSkillInfo(data);
	}
	
	public void SetShipInfoVisible(bool visible)
	{
		_mShipInfoRoot.gameObject.SetActiveRecursively(visible);
		
		if (!visible)
		{
			_mDescriptionText.gameObject.SetActiveRecursively(false);
		}
	}
	
	private void InitBuffData()
	{
		foreach (Transform tf in _mBuffInfoRoot)
		{
			GameObject.Destroy(tf.gameObject);
		}
		
		Dictionary<int, BuffData> dataList = GameStatusManager.Instance.MBattleStatus.MBattleResult.AttackerBuffDataList;
		if (0 == dataList.Count)
			return;		
		int row = 0;
		int col = 0;
		foreach (BuffData data in dataList.Values)
		{
			CreateBuffIcon(data, row, col);
			
			col++;
			if (col == BUFF_COUNT_PER_ROW)
			{
				row++;
				col = 0;
			}
		}
		
		if(GameStatusManager.Instance.MBattleStatus.MBattleResult.BattleType != GameData.BattleGameData.BattleType.PORT_VIE_BATTLE) return;
		// vie inspire buff
		if(Globals.Instance.MPortVieManager.puVieDiamondInspire > 0)
		{
			CreateBuffIcon(EBufferType.BUFFER_INSPIRE,row,col);
			col++;
			if (col == BUFF_COUNT_PER_ROW)
			{
				row++;
				col = 0;
			}
		}
		
		if(Globals.Instance.MPortVieManager.puVieDiamondOrder > 0)
		{
			CreateBuffIcon(EBufferType.BUFFER_ORDER,row,col);
		}
	}
	
	private void UpdateBuffData(WarshipL ws)
	{
		foreach (Transform tf in _mBuffInfoRoot)
		{
			GameObject.Destroy(tf.gameObject);
		}
		
		Dictionary<int, BuffData> dataList = null;
		if (ws.Property.WarshipIsAttacker)
			dataList = GameStatusManager.Instance.MBattleStatus.MBattleResult.AttackerBuffDataList;
		else
			dataList = GameStatusManager.Instance.MBattleStatus.MBattleResult.AttackedBuffDataList;
		
		if (0 == dataList.Count)
			return;
		
		int row = 0;
		int col = 0;
		foreach (BuffData data in dataList.Values)
		{
			CreateBuffIcon(data, row, col);
			
			col++;
			if (col == BUFF_COUNT_PER_ROW)
			{
				row++;
				col = 0;
			}
		}
		
		if(GameStatusManager.Instance.MBattleStatus.MBattleResult.BattleType != GameData.BattleGameData.BattleType.PORT_VIE_BATTLE) return;
		// vie inspire buff
		if(Globals.Instance.MPortVieManager.puVieDiamondInspire > 0)
		{
			CreateBuffIcon(EBufferType.BUFFER_INSPIRE,row,col);
			col++;
			if (col == BUFF_COUNT_PER_ROW)
			{
				row++;
				col = 0;
			}
		}
		
		if(Globals.Instance.MPortVieManager.puVieDiamondOrder > 0)
		{
			CreateBuffIcon(EBufferType.BUFFER_ORDER,row,col);
		}
	}
	
	private void CreateBuffIcon(BuffData data, int row, int col)
	{
		GameObject go = GameObject.Instantiate(BuffIconTp) as GameObject;
		go.name = data.Name;
		go.transform.parent = _mBuffInfoRoot;
		go.transform.localScale = Vector3.one;
		
		float x = (_mBuffIconWidth - -5) * col - 0.5f * _mBuffIconWidth;
		float y = -(_mBuffIconHeight + 5) * row - 0.5f * _mBuffIconHeight;
		// x *= Globals.Instance.MGUIManager.widthRatio;
		// y *= Globals.Instance.MGUIManager.heightRatio;
		go.transform.localPosition = new Vector3(x, y, -1f);
		
		UIButton btn = go.GetComponent<UIButton>() as UIButton;
		UIEventListener.Get(btn.gameObject).onPress += OnClickBuffIcon ;
		btn.Data = data;
		
		PackedSprite icon = btn.transform.Find("Icon").GetComponent<PackedSprite>() as PackedSprite;
		icon.PlayAnim(data.Icon);
	}
	
	
	// BY LSJ
	enum EBufferType
	{
		BUFFER_INSPIRE,
		BUFFER_ORDER,
	}
	// BY LSJ	 
	// port vie or port defense inspire and order buffer
	void CreateBuffIcon(EBufferType type, int row, int col)
	{
		GameObject go = GameObject.Instantiate(BuffIconTp) as GameObject;
		go.name = type.ToString();
		go.transform.parent = _mBuffInfoRoot;
		go.transform.localScale = Vector3.one;
		
		float x = (_mBuffIconWidth - -5) * col - 0.5f * _mBuffIconWidth;
		float y = -(_mBuffIconHeight + 5) * row - 0.5f * _mBuffIconHeight;
		
		go.transform.localPosition = new Vector3(x, y, -1f);
		
		UIButton btn = go.GetComponent<UIButton>() as UIButton;
		UIEventListener.Get(btn.gameObject).onPress += OnClickBuffIcon ;
		btn.Data = type;
		
		PackedSprite icon = btn.transform.Find("Icon").GetComponent<PackedSprite>() as PackedSprite;
		string iconName = string.Empty;
		switch(type)
		{
		case EBufferType.BUFFER_INSPIRE:
			iconName = "Buf9901";
			break;
		case EBufferType.BUFFER_ORDER:
			iconName = "Buf9801";
			break;
		}
		icon.PlayAnim(iconName);
	}
	
	private void SetWarshipName(string name)
	{
		_mWarshipName.GetComponent<GUIText>().text = GUIFontColor.Color14H + name;
	}
	
	private void SetWarshipLife(int currentLife, int maxLife)
	{
		// tzz added for negative number or max life
		currentLife = Mathf.Clamp(currentLife,0,maxLife);
		
		string wordText = Globals.Instance.MDataTableManager.GetWordText(20400007);
		string val = wordText + currentLife + "/" + maxLife;
		_mWarshipLife.GetComponent<GUIText>().text = GUIFontColor.Color2H + val;
	}
	
	private void SetWarshipDander(int currentDander, int maxDander)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(20400008);
		string val = wordText + currentDander + "/" + maxDander;
		_mWarshipDander.GetComponent<GUIText>().text = GUIFontColor.PureRed + val;
	}
	
	private void SetDescription(string val)
	{
		_mDescriptionText.gameObject.SetActiveRecursively(true);
		_mDescriptionText.GetComponent<GUIText>().text = GUIFontColor.Color14H + val;
	}
	
	void SetSkillInfo(GirlData data)
	{
		// Check skill
		_mSkillBtn.gameObject.SetActiveRecursively(false);
//		if (data.BasicData.IsNpc)
//		{
//			SkillConfig config = Globals.Instance.MDataTableManager.GetConfig<SkillConfig>();
//			SkillConfig.SkillObject element = null;
//			bool hasData = config.GetSkillObject(data.BasicData.NpcSkillID, out element);
//			if (!hasData)
//			{
//				return;
//			}
//			
//			_mSkillBtn.gameObject.SetActiveRecursively(true);
//			_mSkillIcon.PlayAnim(element.SkillIconID);
//			string wordText = Globals.Instance.MDataTableManager.GetWordText(element.SkillDescID);
//			_mSkillBtn.Data = wordText;
//			
//			_mDescriptionText.gameObject.SetActiveRecursively(false);
//		}
//		else
//		{
//			if (data.HasGeneral)
//			{
//				GeneralData glData = Globals.Instance.MGameDataManager.MActorData.GetGeneralData(data.WarshipGeneralID);
//				foreach (SkillData skillData in glData.SkillDatas.Values)
//				{
//					_mSkillBtn.gameObject.SetActiveRecursively(true);
//					_mSkillIcon.PlayAnim(skillData.BasicData.SkillIcon);
//					_mSkillBtn.Data = skillData.BasicData.SkillDesc;
//					break;
//				}
//			}
//		}
	}
	
	private void RegisterSubscriber()
	{
		// Add EventListener
		if (null == _mWarshipBeAttacked)
		{
			_mWarshipBeAttacked = EventManager.Subscribe(WarshipPublisher.NAME + ":" + WarshipPublisher.EVENT_BE_ATTACKED);
			_mWarshipBeAttacked.Handler = delegate (object[] args)
			{
				WarshipL dShip = (WarshipL)args[0];
				Skill dSkill = (Skill)args[1];
				
				// Check if this target value is current select target
				if (_mCurrentWarShip != dShip)
					return;
				
				SetWarshipLife(_mCurrentWarShip.GirlData.PropertyData.Life, _mCurrentWarShip.GirlData.PropertyData.MaxLife);
				SetWarshipDander(_mCurrentWarShip.GirlData.PropertyData.Power, _mCurrentWarShip.GirlData.PropertyData.MaxPower);
			};
		}
	
		if (null == _mWarshiDeath)
		{
			_mWarshiDeath = EventManager.Subscribe(WarshipPublisher.NAME + ":" + WarshipPublisher.EVENT_DEATH);
			_mWarshiDeath.Handler = delegate (object[] args)
			{
				WarshipL dShip = (WarshipL)args[0];
				if (_mCurrentWarShip != dShip)
					return;
				
				SetShipInfoVisible(false);
			};
		}
	}
	
	private void UnRegisterSubscriber()
	{
		if (null != _mWarshipBeAttacked)
			_mWarshipBeAttacked.Unsubscribe();
		_mWarshipBeAttacked = null;
		
		if (null != _mWarshiDeath)
			_mWarshiDeath.Unsubscribe();
		_mWarshiDeath = null;
	}
	
	private void OnClickSkillBtn(GameObject obj)
	{
		UIButton targetBtn = obj.transform.GetComponent<UIButton>();
		if(targetBtn.Data != null)
		{
			string wordText = (string)targetBtn.Data;
			
			SetDescription(wordText);
			
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/Button");
		}
	}
	
	bool isBubbleVisible = false;
	private void OnClickQuickFightBtn(GameObject obj)
	{
		Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/Button");
		if(quickBtnType == EQuickBattleBtnType.VisibleDelay && !isBubbleVisible)
		{
			isBubbleVisible = true;
			Bubble b = transform.Find("ChatBubble/BG").GetComponent(typeof(Bubble)) as  Bubble;
			b.transform.parent.gameObject.SetActiveRecursively(true);
			b.SetText(Globals.Instance.MDataTableManager.GetWordText(11030057));
			StartCoroutine("BubbleInvisible");
		}
		else if(quickBtnType == EQuickBattleBtnType.Visible)
		{
			GameStatusManager.Instance.MBattleStatus.EndBattleLogic();
		}
	}
	
	IEnumerator BubbleInvisible()
	{
		yield return new WaitForSeconds(2);
		isBubbleVisible = false;
		Bubble b = transform.Find("ChatBubble/BG").GetComponent(typeof(Bubble)) as  Bubble;
		b.transform.parent.gameObject.SetActiveRecursively(false);
	}
	
	private void OnClickBuffIcon(GameObject go, bool pressed)
	{
		if (pressed)
		{
			UIButton btn = go.transform.GetComponent<UIButton>() as UIButton;
			
			if(btn.Data.GetType() == typeof(EBufferType))
			{
				string tText2Show = string.Empty;
				EBufferType type = (EBufferType)btn.Data;
				switch(type)
				{
				case EBufferType.BUFFER_INSPIRE:
					tText2Show = string.Format(Globals.Instance.MDataTableManager.GetWordText(23600049),Globals.Instance.MPortVieManager.puVieDiamondInspire);
					break;
				case EBufferType.BUFFER_ORDER:
					tText2Show = string.Format(Globals.Instance.MDataTableManager.GetWordText(23600050),Globals.Instance.MPortVieManager.puVieDiamondOrder);
					break;
				}
				SetDescription(tText2Show);
			}
			else
			{
				BuffData data = btn.Data as BuffData;
				if (null == data)
					return;
				
				SetDescription(data.Descript);
			}
		}
	}
	
	//! tzz added for custom camera toggle button event  
	private void OnCustomCameraBtnClick(GameObject obj){
		
		//BattleStatus.CustomCameraState = (m_customCameraBtn.StateName == "two");
			
		//KeyFrameInfo[] t_cameraInfo = GameStatusManager.Instance.MBattleStatus.m_battleCameraTrack.keyFrameInfos;
		//KeyFrameInfo t_endPos = t_cameraInfo[t_cameraInfo.Length - 1];
		//
		//CameraTrackEndPos = t_endPos.Position;
		//
		//CameraTrack.ITweenMoveTo(Globals.Instance.MSceneManager.mMainCamera.gameObject,t_endPos,null,0.5f);		
	}
	
	// by lsj control the quick battle btn when to visible or invisible
	
	public void ControlQuickBattleBtn()
	{
		//if(m_customCameraBtn != null){
		//	m_customCameraBtn.gameObject.SetActiveRecursively(true);
		//}
		
		CopyData curr = Globals.Instance.MGameDataManager.MCurrentCopyData;
		
		switch(GameStatusManager.Instance.MBattleStatus.MBattleResult.BattleType)
		{
		case GameData.BattleGameData.BattleType.ARENA_BATTLE:
		case GameData.BattleGameData.BattleType.PORT_VIE_BATTLE:
		case GameData.BattleGameData.BattleType.PROT_DEFENSE_BATTLE:
		case GameData.BattleGameData.BattleType.TASK_BATTLE:
			quickBtnType = EQuickBattleBtnType.VisibleDelay;
			break;
		case GameData.BattleGameData.BattleType.COPY_BATTLE:
			if(curr.MCopyBasicData.CopyScore < 100)
			{
				quickBtnType = EQuickBattleBtnType.VisibleDelay;
			}
			else if(curr.MCopyBasicData.CopyScore == 100)
			{
				quickBtnType = EQuickBattleBtnType.Visible;
			}
			break;
		}
		//if the player is vip
		if(Globals.Instance.MGameDataManager.MActorData.VipData.Level > 0)
		{
			quickBtnType = EQuickBattleBtnType.Visible;
		}
			
		QuickBattleBtnVisible(quickBtnType);
	}
	
	public void QuickBattleBtnVisible(EQuickBattleBtnType type)
	{
		switch(type)
		{
		case EQuickBattleBtnType.Visible:
			if(_mQuickFightBtn != null)
			{
				_mQuickFightBtn.gameObject.SetActiveRecursively(true);
				//_mQuickFightBtn.SetColor(Color.white);
			}
			break;
		case EQuickBattleBtnType.VisibleDelay:
			_mQuickFightBtn.gameObject.SetActiveRecursively(true);
			//_mQuickFightBtn.SetColor(Color.gray);
			break;
		}
	}
	
	public void QuickBattleBtnVisibleDelay()
	{
		quickBtnType = EQuickBattleBtnType.Visible;
		//_mQuickFightBtn.SetColor(Color.white);
		if(isBubbleVisible)
		{
			isBubbleVisible = false;
			Bubble b = transform.Find("ChatBubble/BG").GetComponent(typeof(Bubble)) as  Bubble;
			b.transform.parent.gameObject.SetActiveRecursively(false);
		}
	}
	
	/// <summary>
	/// tzz added
	/// Gets the buffer info root for NewEffectFont TouXiChengGong style
	/// </summary>
	/// <returns>
	/// The buffer info root.
	/// </returns>
	public Transform GetBufferInfoRoot(){
		return _mBuffInfoRoot;
	}
	private Transform _mRootTransform;
	
	private UIButton _mQuickFightBtn;
	
	private Transform _mBuffInfoRoot;
	private float _mBuffIconWidth;
	private float _mBuffIconHeight;
	// private UIButton _mBuffIconBG1;
	// private UIButton _mBuffIconBG2;
	// private UIButton _mBuffIconBG3;
	// private PackedSprite _mBuffIcon1;
	// private PackedSprite _mBuffIcon2;
	// private PackedSprite _mBuffIcon3;
	
	private UIButton _mDescriptionText;
	
	[HideInInspector]public static Vector3		CameraTrackEndPos = Vector3.zero;
	
	//! custom camera
	//private UIToggle m_customCameraBtn = null;
	
	private bool m_customCameraMoveState = false;
	
	private Transform _mShipInfoRoot;
	private UIButton _mWarshipName;
	private UIButton _mWarshipLife;
	private UIButton _mWarshipDander;
	
	public UIButton _mSkillBtn;
	public PackedSprite _mSkillIcon;
	[HideInInspector] public BattleBloodControl _bloodControl;
	
	//gui of port defense
	private Transform uiPortDefense;
	private Transform moveHeadPanelL;
	
	private PackedSprite[] AvatarL = new PackedSprite[3];
	private SpriteText[] NameL = new SpriteText[3];
	private Transform moveHeadPanelR;
	private PackedSprite[] AvatarR = new PackedSprite[3];
	private SpriteText[] NameR = new SpriteText[3];

	private Transform VS;
	private Transform bottomHead;
	private PackedSprite[] avatarDown = new PackedSprite[3];
	private SpriteText[] nameDown = new SpriteText[3];
	private SpriteText[] battleInfoDown = new SpriteText[3];
	
	
	private Transform portDefenseDialog;
	private SpriteText textOutInfo;
	private SpriteText textWave;
	private PackedSprite textAssistantBG;
	private SpriteText textAssistant;
	// wave result dialog
	private UIButton btnWaveResultOk;
	private SpriteText textWaveTitle;
		
	private GameObject[] helpInfo = new GameObject[2];
	private SpriteText[,] textHelpInfo = new SpriteText[2,8];
	private SpriteText[,] textPanner = new SpriteText[3,6];
	private SpriteText textJiangli;
	private GameObject tipBG;
	private SpriteText textTip;
	WarshipL _mCurrentWarShip = null;
	// Event Observer
	ISubscriber _mWarshipBeAttacked = null;
	ISubscriber _mWarshiDeath = null;
}
