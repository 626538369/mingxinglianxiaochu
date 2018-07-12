using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Status2DBattle : BattleStatus 
{
	GUIBattleMain guiBattleMain = null;
	
	private event TeachEventDelegate m_battleEndEvent;
	
	public override void Initialize()
	{
		GUIRadarScan.Hide();
		
		// tzz added
		// close the EXIT_COPY dialog to prevent from click ok to send exit copy scene
		//
		GUIDialog.Destroy();
	
		InitializeImpl();
	}
	
	public override void Release()
	{
		//ResetValue();
		base.Release();
	}
	
	public override void Update()
	{
		if(_isPauseBattle)
			return;
		
		if (_isBattleEnd)
			return;
		
		UpdateBattleLogic();
		
		// float stepRatio = (float)battleStepFlag / _battleResult.BattleSteps.Count;
		//Debug.Log("当前步数  " + battleStepFlag + "/" + _battleResult.BattleSteps.Count + "  : " + stepRatio);
	}
	
	protected override void InitializeImpl()
	{
		ResetValue();
		
		InitBattleData();
		
		_mPublisher.NotifyEnterBattle();
	}
	
	public override void InitBattleData()
	{
		GUIRadarScan.Hide();
		
		BeginBattleLogic();
		
		_mBattleState = EBattleState.DO_STEP;
	}
	
	public override void BeginBattleLogic()
	{
		Globals.Instance.MGUIManager.CreateWindow<GUIBattleMain>(delegate(GUIBattleMain gui)
		{
			guiBattleMain = gui;
			guiBattleMain.BeginBattleLogic();
			_isPauseBattle = false;
			
		});
	}
	
	public override void UpdateBattleLogic()
	{
		if (null != guiBattleMain)
		{
			guiBattleMain.UpdateBattleLogic();
		}
	}
	
	public override void OnOneBattleStepEnd()
	{
	}
	
	// The end battle logic
	public override void EndBattleLogic()
	{
		Release();
		
		if(m_battleEndEvent != null){
			m_battleEndEvent(TeachBattleEvent.e_battleEnd);
	    	m_battleEndEvent = null;

		}
		
		if (null != guiBattleMain)
			guiBattleMain.Close();
		guiBattleMain = null;
		
		Globals.Instance.MTeachManager.NewBattleEndEvent();
		
		string sceneName = string.Empty;
		switch (_battleResult.BattleType)
		{
		case GameData.BattleGameData.BattleType.TASK_TEACH:
		{
			break;
		}
		case GameData.BattleGameData.BattleType.COPY_BATTLE:
		{
			GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_COPY);
			break;
		}
		case GameData.BattleGameData.BattleType.ARENA_BATTLE:
		{
			GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_PORT);
			break;
		}
		case GameData.BattleGameData.BattleType.PORT_VIE_BATTLE:
		{
			GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_PORT);
			
			Globals.Instance.MPortVieManager.sIsVieBattling = false;
			//GUIPortVie guiVie = Globals.Instance.MGUIManager.GetGUIWindow<GUIPortVie>();
			//if(Globals.Instance.MPortVieManager.sIsShowVieRewardView)
			//{
			//	Globals.Instance.MPortVieManager.sIsShowVieRewardView = false;
			//	Globals.Instance.MPortVieManager.CreateGUIPortVieReward();
			//	guiVie.Close();
			//}
			//else
			//{
			//	guiVie.SetVisible(true);
			//	//test whether to continue the next battle
			//	guiVie.BattleAnimationEnd();
			//}
			break;
		}
		case GameData.BattleGameData.BattleType.PROT_DEFENSE_BATTLE:
		{
			// if(Globals.Instance.MPortDefenseManager.puIsShowWaveResult)
			// {
			// 	GUIBattle t = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
			// 	if (null != t)
			// 		t.UpdateWaveResultDialogData();
			// }
			// else
			// {
			// 	Globals.Instance.MPortDefenseManager.UpdateDefenseBattle();
			// }
			break;
		}
		case GameData.BattleGameData.BattleType.TASK_BATTLE:
		{
			Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
			{
				gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
				gui.SetDialogType(EDialogType.TASK_BATTLE, null);
			},EDialogStyle.DialogOk
			);
			break;
		}
		}
		
		base._isBattleEnd = true;
		_mPublisher.NotifyLeaveBattle(this.MBattleResult);
	}
	
	public override void OnBeAttacked(WarshipL warship, Skill skill)
	{
		// // tzz added
		// // for teach first fire in Teach first enter scene
		// // check TeachFirstEnterGame.cs for detail
		// if(m_teachFirstFireEvent != null){
		// 	m_teachFirstFireEvent(TeachBattleEvent.e_firstFire);
		// 	m_teachFirstFireEvent = null;
		// 	
		// 	battleStepDuration = 0.0f;
		// 	_mDelayTime = DELAY_TIME;
		// }
	}
	
	public override void OnWarshipDeath(WarshipL warship)
	{
		// if(m_teachOwnShipDownEvent != null ){
		// 	m_teachOwnShipDownEvent(TeachBattleEvent.e_ownShipDown);
		// 	m_teachOwnShipDownEvent = null;
		// 	
		// 	battleStepDuration = 0.0f;
		// 	_mDelayTime = DELAY_TIME;
		// }
	}
	
	
	//! tzz added for battle fialed
	public override void BattleFailedProcess(){
		// If the reinforce has not open, return the workflow
		int teachResult = Globals.Instance.MTeachManager.NewGetTeachStep("x25");
		if (teachResult < 1)
			return;

	}
	
	public override void SetTeachEventDelegateType(TeachBattleEvent _type,TeachEventDelegate _delegate){
		base.SetTeachEventDelegateType(_type,_delegate);
		switch(_type){
		case TeachBattleEvent.e_battleEnd:
			m_battleEndEvent = _delegate;
			break;
		}
	}

}
