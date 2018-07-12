using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Status2DCopy : CopyStatus
{
	[HideInInspector] public Stage2DCopy StageCopy = null;
	GameObject fightingMonster = null;
	
	protected override void Initialize_impl()
	{
		_mIsEnabled = true;
		_mIsFinalBattle = false;
		_mIsPermitRequestBattle = true;
		
		MCurrentCopyData = Globals.Instance.MGameDataManager.MCurrentCopyData;
		
		InitStage2D();
		RegisterSubscriber();
		
		_mPublisher.NotifyEnterCopy();
		
		Statistics.INSTANCE.CustomEventCall(Statistics.CustomEventType.EnterCopy,"CopyID",MCurrentCopyData.MCopyBasicData.CopyID);
		Globals.Instance.MTeachManager.NewTeachEnterCopy();
	}
	
	void InitStage2D()
	{
		GameObject stageRoot = GameObject.Find("Stage2DRoot");
		StageCopy = stageRoot.GetComponent<Stage2DCopy>() as Stage2DCopy;
		StageCopy.Init();
	}
	
	private void HideMovableObjs(bool hide)
	{
		if (null != StageCopy)
		{
			StageCopy.HideMovableObjs(hide);
		}
	}
	
	public override void Update()
	{
		if (!_mIsEnabled || _mPaused)
			return;
	}
	
	public override void OnRequestBattle(GameObject first, GameObject other)
	{
		if (!_mIsPermitRequestBattle)
			return;
		_mIsPermitRequestBattle = false;
		
	
	}
	
	public override void OnBattleEnd(GameData.BattleGameData.BattleResult battleResult){
		
//		//if(battleResult.BattleWinResult == GameData.BattleGameData.EBattleWinResult.ACTOR_WIN // is win
//		//	&& Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyType == 2 // boss type
//		//	&& StageCopy.GetMonsterCount() == 1){ // finall monster
//		//	OnBattleEnd_impl(battleResult);
//		//}else{
//		//	OnBattleEnd_impl(battleResult);
//		//}
		
		OnBattleEnd_impl(battleResult);
		_mPublisher.NotifyNPCFleetCount(StageCopy.GetMonsterCount());
	}
	
	protected override void OnBattleEnd_impl(GameData.BattleGameData.BattleResult battleResult)
	{
		_mIsPermitRequestBattle = true;
		
		// Clear ReinforcementData
		Globals.Instance.MGameDataManager.MActorData.RemoveReinforcementData();
		
		switch (battleResult.BattleWinResult)
		{
			// Actor win
		case GameData.BattleGameData.EBattleWinResult.ACTOR_WIN:
		case GameData.BattleGameData.EBattleWinResult.DOGFALL: // Dogfall
		{
			OnceBattleFinish();
			break;
		}
			// Monster win
		case GameData.BattleGameData.EBattleWinResult.MONSTER_WIN:
		{
			Statistics.INSTANCE.CustomEventCall(Statistics.CustomEventType.FailedCopyBattle, "CopyID", Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyID);
			break;
		}	
		
		}

		// Flush ship life
		Dictionary<long,GirlData> dictWarshipData = Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();
		List<GameData.BattleGameData.WarshipBattleEndCurrLife> listWarshipLife = GameStatusManager.Instance.MBattleStatus.MBattleResult.ListWarshipBattleEndCurrLife;
		foreach(GameData.BattleGameData.WarshipBattleEndCurrLife warshipLife in listWarshipLife)
		{
			if(dictWarshipData.ContainsKey(warshipLife.ShipID))
			{
				dictWarshipData[warshipLife.ShipID].PropertyData.Life = warshipLife.ShipCurrLife;
			}
		}
		
		// Play battle result effect
		Globals.Instance.M3DItemManager.PlayBattleEffect(battleResult, delegate()
		{
			// Monster win, give the user a good tooltips
			if (battleResult.BattleWinResult == GameData.BattleGameData.EBattleWinResult.MONSTER_WIN)
			{
				// Move the BattleFailedProcess prompt to here
				GameStatusManager.Instance.MBattleStatus.BattleFailedProcess();
			}
			else
			{
				Globals.Instance.MGUIManager.CreateWindow<GUISingleDrop>(delegate(GUISingleDrop gui)
				{
					gui.UpdateData();
				});
			}
		});
		
		// Handle the 2d stage logic
		StageCopy.OnOnceBattleEnd(battleResult, fightingMonster);
		
		// Request update the GUIMain ships blood
		Globals.Instance.MGameDataManager.MActorData.NotifyWarshipUpdate();
		
		_mIsFinalBattle = battleResult.IsFinalBattle;
		fightingMonster = null;
	}
	
	public override void DisplayCopyDrops()
	{
		if (!_mIsFinalBattle)
			return;
		
		CopyBattleFinish();
	}
	
	protected override void CopyBattleFinish()
	{	
		Globals.Instance.MTaskManager.GoToAccomplishTalk(delegate()
		{
			// Reslove the multiple reset _mIsEnabled and SetFingerEventActive
			_mIsEnabled = false;
			SetFingerEventActive(false);
		
			//Globals.Instance.MGUIManager.CreateWindow<GUICopyStatistics>(delegate(GUICopyStatistics gui)
			//{
			//	gui.UpdateData();
			//});
		});
	}
	
	void ReturnLastestPort()
	{
		GUILoading loading = Globals.Instance.MGUIManager.GetGUIWindow<GUILoading>();
		if (null != loading)
		{
			loading.SetVisible(true);
			loading.Progress = 0.0f;
			loading.MLoadingState = LoadingState.REQUEST_CHANGE_SCENE;
			loading.LoadingType = GUILoading.ELoadingType.ENTER_PORT;
		}
		
		long roleID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
		int seaID = Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID;
		NetSender.Instance.RequestEnterPort(roleID, seaID);
	}
}
