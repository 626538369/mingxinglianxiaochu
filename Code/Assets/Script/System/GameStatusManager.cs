using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using EZGUI;

public enum GameState
{
	GAME_STATE_INVALID = 0,
	GAME_STATE_INITIAL,

	
	GAME_STATE_LOGIN,
	GAME_STATE_LOADING,
	
	GAME_STATE_ROLE_CREATE,
	
	GAME_STATE_PORT,
	GAME_STATE_COPY,
	GAME_STATE_BATTLE,
	GAME_STATE_HOME,
	
	GAME_STATE_PLAYER_DOCK,
	
	GAME_STATE_QUIT,
}

// Provide the function of our game status, switch different game status
// Only update the current GameStatus, and hide the others GameStatus
public class GameStatusManager : Singleton<GameStatusManager>
{
	// Public Access Variable Entrypoint
	public GameState MGameState
	{
		get { return _mGameState; }
		set { _mGameState = value; }
	}
	
	public GameState MGameNextState
	{
		get { return _mGameNextState; }
		set { _mGameNextState = value; }
	}
	
	public GameStatus MCurrentGameStatus
	{
		get { return _mCurrentGameStatus; }
		// set { _mCurrentGameStatus = value; }
	}
	
	public GameStatus MLastGameStatus
	{
		get { return _mLastGameStatus; }
		// set { _mLastGameStatus = value; }
	}
	
	public LoginStatus MLoginStatus
	{
		get { return _pLoginStatus; }
	}
	
	public RoleCreateStatus MRoleCreateStatus
	{
		get { return _pRoleCreateStatus; }
	}
	
	public PortStatus MPortStatus
	{
		get { return _pPortStatus; }
	}
	
	public CopyStatus MCopyStatus
	{
		get { return _pCopyStatus; }
	}
	
	public BattleStatus MBattleStatus
	{
		get { return _pBattleStatus; }
	}
	
	public PlayerDockStatus MPlayerDockStatus
	{
		get { return _pPlayerDockStatus;}
	}
	public HomeStatus MHomeStatus
	{
		get{ return _homeStatus;}
	}
	
	// This Class Section
	public GameStatusManager()
	{
	}

	public void Initialize()
	{
		_mIsPaused = false;
		
		RegisterSubscriber();
		
		_mGameState = GameState.GAME_STATE_INITIAL;
		_mGameStatusPublisher.NotifyGameStateInitial();
	}
	
	public void Release()
	{
		UnRegisterSubscriber();
		
		if (null != _pLoginStatus)
			_pLoginStatus.Release();
		if (null != _pRoleCreateStatus)
			_pRoleCreateStatus.Release();
		if (null != _pPortStatus)
			_pPortStatus.Release();
		if (null != _pCopyStatus)
			_pCopyStatus.Release();
		if (null != _pBattleStatus)
			_pBattleStatus.Release();
		
		_mIsPaused = false;
		_mGameState = GameState.GAME_STATE_INVALID;
	}
	
	/// <summary>
	/// Gets the status.
	/// </summary>
	/// <returns>
	/// The status.
	/// </returns>
	public GameState GetStatus(){
		return _mGameState;
	}
	
	public void SetGameState(GameState state)
	{
		// Release last GameState logic
		if (_mGameState == GameState.GAME_STATE_COPY
			&& state == GameState.GAME_STATE_BATTLE)
		{
			// Do not Release
		}
		else
		{
			if (_mCurrentGameStatus != null)
			{
				_mCurrentGameStatus.Release();
			}
		}
		
		GameState lastState = _mGameState;
		_mGameState = state;
		
		_mLastGameStatus = _mCurrentGameStatus;
		_mGameNextState = GameState.GAME_STATE_INVALID;
		switch (_mGameState)
		{
		case GameState.GAME_STATE_INITIAL:
		{
			_mGameStatusPublisher.NotifyGameStateInitial();
			break;
		}
		case GameState.GAME_STATE_LOADING:
		{
			_mGameStatusPublisher.NotifyGameStateLoading();
			break;
		}
		case GameState.GAME_STATE_ROLE_CREATE:
		{
			//_mGameStatusPublisher.NotifyGameStateRoleCreate();
			
			//long roleID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
			//string testRoleName = "yingpan";
			//string roleName =  testRoleName + roleID.ToString();
			//int gender = 1;
			
			//string avatarName = "AvatarMan1";
			//int templateId = 1217001000;
			//int countryID = -1;
			//NetSender.Instance.RequestCreatePlayer(roleName, gender, countryID, avatarName, templateId);
			
			//break;
			GUIGuoChang.Show();
			Globals.Instance.MGUIManager.CreateWindow<GUICreateRole>
			( 	
				delegate(GUICreateRole gui)
				{
					GUIGuoChang.SetTweenPlay(0,delegate {
						gui.UpdateZeroStep();
					});
					
				}
			);
			
			break;
		}

		case GameState.GAME_STATE_LOGIN:
		{
			_mGameStatusPublisher.NotifyGameStateLogin();
			break;
		}
		case GameState.GAME_STATE_PORT:
		{
			_mGameStatusPublisher.NotifyGameStatePort();
			//Globals.Instance.MTeachManager.NewQianZhiFinshed();
			
			_mCurrentGameStatus = _pPortStatus;
			
			if(Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>() == null)
			{
				Globals.Instance.MGUIManager.CreateWindow<GUIMain>(delegate(GUIMain gui)
				{	
					NetSender.Instance.GetLotteryInfoReq();
					Globals.Instance.MGameDataManager.MActorData.NotifyWorthUpdate();
					Globals.Instance.MGameDataManager.MActorData.NotifyPlayerInfoUpdate();
					gui.SetUISceneStatus(GUIMain.ESceneType.PORT);
					if(!Globals.Instance.MTeachManager.IsOpenTeach)
					{
						gui.FunctionButtonState();
					}
					TaskManager.ReadGameFinishDataFilename();

					NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.Recharge);
					NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.GameInner);
					NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.PromotionGift);

//					if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows)
//					{
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.Recharge);
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.GameInner);
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.PromotionGift);
//					}
//					else 	if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
//					{
//						#if UNITY_IPHONE
//						PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;	
//						string str = "101119" + Globals.Instance.MGameDataManager.MActorData.PlayerID.ToString();
//						string str2 = actorData.BasicData.Name;
//						U3dAppStoreSender.LoginKTPlay(str,str2);
//						Debug.Log("IPHONE KTPlayLogin:"+str+str2);
//						string str3 = Globals.Instance.MGameDataManager.MActorData.CYChannelID;
//						U3dAppStoreSender.LoginCYwithuserid(str3);
//						Debug.Log("IPHONE CYLogin:"+str3);
//						
//						Debug.Log("RequestVipStoreRechargeInfo");
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.Recharge);
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.GameInner);
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.PromotionGift);
//						#endif	
//					}
//					else if(GameDefines.OutputVerDefs==OutputVersionDefs.WPay)
//					{
//						PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;	
//						string str = "103214" + Globals.Instance.MGameDataManager.MActorData.PlayerID.ToString();
//						string str2 = actorData.BasicData.Name;
//						AndroidSDKAgent.LoginKTPlay(str,str2);
//						Debug.Log("ANDROID KTPlayLogin:"+str+str2);
//
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.Recharge);
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.GameInner);
////						NetSender.Instance.RequestVipStoreRechargeInfo((int)CommodityType.PromotionGift);
//					}
				});
			}
			else
			{
				Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().SetUISceneStatus(GUIMain.ESceneType.PORT);
				Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().SetVisible(true);
				Globals.Instance.MGameDataManager.MActorData.NotifyWorthUpdate();
			}
			_mCurrentGameStatus.Initialize();
				
			Globals.Instance.MTaskManager.GoToTalk(0,delegate(){
					
			});
			
			Globals.Instance.MTaskManager.UpdateTaskTrackView();
			Globals.Instance.MNpcManager.UpdateBuildMainTaskIcon();
			GUILoading guiLoading =  Globals.Instance.MGUIManager.GetGUIWindow<GUILoading>();
			if (guiLoading != null)
				guiLoading.InvokeLodingClose();
		
					
			Globals.Instance.MTeachManager.NewCheckExceptionKey();
			
			if(Globals.Instance.MTeachManager.NewGetTeachStep("x03") == 0)
			{
				Globals.Instance.MTeachManager.NewOpenTeach("x03",1);
			}
			break;
		}
		case GameState.GAME_STATE_HOME:
		{

			_mCurrentGameStatus = _homeStatus;
			_mCurrentGameStatus.Initialize();
			
			GUIBuildExplore guiBuildExplore = Globals.Instance.MGUIManager.GetGUIWindow<GUIBuildExplore>();
			if (guiBuildExplore != null)
				guiBuildExplore.Close();
			
			Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().SetVisible(false);
			break;
		}
			
		case GameState.GAME_STATE_COPY:
		{	
			_mGameStatusPublisher.NotifyGameStateCopy();
			_mCurrentGameStatus = _pCopyStatus;
		
			// Battle and Copy is mutex in some time
			if (lastState == GameState.GAME_STATE_BATTLE)
			{
				// When switch from BattleStatus, we need do something in CopyStatus
				_pCopyStatus.OnBattleEnd(_pBattleStatus.MBattleResult);
			}
			else
			{
				_mCurrentGameStatus.Initialize();
			}
			
			Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().SetVisible(true);
			Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().SetUISceneStatus(GUIMain.ESceneType.COPY);
			
			/*GUIPublicWarn gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIPublicWarn>();
			if (null != gui)
				gui.SetVisible(true);
			*/
			GUITaskTrack GUITaskTrack = Globals.Instance.MGUIManager.GetGUIWindow<GUITaskTrack>();
			if (null != GUITaskTrack)
			{
				GUITaskTrack.SetVisible(true);
				GUITaskTrack.UpdateData();
			}
			
			break;
		}
		case GameState.GAME_STATE_BATTLE:
		{
			EnterPKBattleByPlayerDock = (lastState == GameState.GAME_STATE_PLAYER_DOCK);
			
			_mGameStatusPublisher.NotifyGameStateBattle();
			_mCurrentGameStatus = _pBattleStatus;
			
			Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().SetVisible(false);
			//Globals.Instance.MGUIManager.GetGUIWindow<GUIPublicWarn>().SetVisible(false);
			
			GUITaskTrack GUITaskTrack = Globals.Instance.MGUIManager.GetGUIWindow<GUITaskTrack>();
			if (null != GUITaskTrack)
				GUITaskTrack.SetVisible(false);
			
			_mCurrentGameStatus.Initialize();
			
			break;
		}
		case GameState.GAME_STATE_PLAYER_DOCK:
		{
			_mCurrentGameStatus.Release();
			_mCurrentGameStatus = _pPlayerDockStatus;
			_mCurrentGameStatus.Initialize();
			
			Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().SetUISceneStatus(GUIMain.ESceneType.DOCK);
			
			break;
		}
			
		}
	}
	
	public void Update()
	{
		if (_mIsPaused)
			return;
		
		if (_mGameState == GameState.GAME_STATE_INVALID)
			return;
		
		if (_mCurrentGameStatus != null)
		{
			_mCurrentGameStatus.Update();
		}
	}
	
	private void RegisterSubscriber()
	{
		if (null == _mScenePrepareLoad)
		{
			_mScenePrepareLoad = EventManager.Subscribe(ScenePublisher.NAME + ":" + ScenePublisher.EVENT_PREPARE_LOAD);
			_mScenePrepareLoad.Handler = delegate(object[] args) 
			{
				string leveName = (string)(args[0]);
			};
		}
		
		if (null == _mSceneLoading)
		{
			_mSceneLoading = EventManager.Subscribe(ScenePublisher.NAME + ":" + ScenePublisher.EVENT_LOADING);
			_mSceneLoading.Handler = delegate(object[] args) 
			{
				string leveName = (string)(args[0]);
				float progress = (float)(args[1]);
			};
		}
		if (null == _mSceneLoaded)
		{
			_mSceneLoaded = EventManager.Subscribe(ScenePublisher.NAME + ":" + ScenePublisher.EVENT_LOADED);
			_mSceneLoaded.Handler = delegate(object[] args) 
			{
				string leveName = (string)(args[0]);
				
				SetGameState(_mGameNextState);
				Globals.Instance.MSoundManager.PlaySceneSound(leveName);
			};
		}
	}
	
	private void UnRegisterSubscriber()
	{
		if (null != _mScenePrepareLoad)
		{
			_mScenePrepareLoad.Unsubscribe();
		}
		_mScenePrepareLoad = null;
		
		if (null != _mSceneLoading)
		{
			_mSceneLoading.Unsubscribe();
		}
		_mSceneLoading = null;
		
		if (null != _mSceneLoaded)
		{
			_mSceneLoaded.Unsubscribe();
		}
		_mSceneLoaded = null;
	}
	
	private bool _mIsPaused;
	private GameState _mGameState;
	private GameState _mGameNextState;
	
	private GameStatus _mLastGameStatus = null;
	private GameStatus _mCurrentGameStatus = null;
	
	/// <summary>
	/// The _m enter PK battle by player dock.
	/// </summary>
	public bool		EnterPKBattleByPlayerDock = false;
	
	public LoginStatus _pLoginStatus = new LoginStatus();
	public RoleCreateStatus _pRoleCreateStatus = new RoleCreateStatus();
	public PortStatus _pPortStatus = new PortStatus();
	public HomeStatus _homeStatus = new HomeStatus();
	public CopyStatus _pCopyStatus = new CopyStatus();
	public BattleStatus _pBattleStatus = new BattleStatus();
	public PlayerDockStatus _pPlayerDockStatus	= new PlayerDockStatus();
	
	
	//game state managet publisher
	GameStatusPublisher _mGameStatusPublisher = new GameStatusPublisher();
	
	// Scene Event Observer
	ISubscriber _mScenePrepareLoad;
	ISubscriber _mSceneLoading;
	ISubscriber _mSceneLoaded;
}
