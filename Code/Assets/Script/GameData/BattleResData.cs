using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GameData.BattleGameData
{
	/*
	public class BattleBuffer
	{
		public BattleBuffer ()
		{
		}

		public int BufferID {
			get { return _bufferID; }
			set { _bufferID = value; }
		}
			
		private int _bufferID;
	}
	*/
	public enum MoveState
	{
		MOVE = 1,
		STOP = 2,
		SINK = 3
	}
		
	public enum AttackState
	{
		NORMAL_ATTACK = 0,
		SKILL_ATTACK = 1,
		NORMAL_ATTACK_CRIT = 2,
		SKILL_ATTACK_CRIT = 3,
		IDLE = 4,
		TREAT_SKILL_ATTACKED = 5,
		REDUCE_DAMAGE_SKILL_ATTACKED = 6
	}
	
	// Be attacked state
	public enum AttackedState
	{
		NORMAL_ATTACKED = 0,
		NORMAL_CRIT = 1,
		SKILL_ATTACKED = 2,
		SKILL_CRIT = 3,
		DODGE = 4,
		TREAT_SKILL_ATTACKED = 5,
		REDUCE_DAMAGE_SKILL_ATTACKED = 6		
	}
	
	public enum BattleType
	{
		COPY_BATTLE = 0,
		ARENA_BATTLE = 1,
		PORT_VIE_BATTLE = 2,
		PROT_DEFENSE_BATTLE = 3,
		TASK_BATTLE = 4,
		TASK_TEACH =5,
	}
	
	public enum SneakAttackType
	{
		ROLE_ATTACK = 0,
		NPC_ATTACK = 1,
		FACE_ATTACK = 2,
	}
	
	// The attacked target
	public class AttackedTarget
	{
		public AttackedTarget ()
		{
		}

		public long ShipID {
			get { return _shipID; }
			set { _shipID = value; }
		}

		public int Damage {
			get { return _damage; }
			set { _damage = value; }
		}

		public AttackedState AttackedState {
			get { return _attackedState; }
			set { _attackedState = value; }
		}
			
		private long _shipID;
		private int _damage;
		private AttackedState _attackedState;
	}
	
	public class BattleShip
	{
		public BattleShip ()
		{
		}

		public long ShipID {
			get { return _shipID; }
			set { _shipID = value; }
		}

		public int CurrentLife {
			get { return _currentLife; }
			set { _currentLife = value; }
		}

		public int CurrentPower {
			get { return _currentPower; }
			set { _currentPower = value; }
		}
		
		public int CurrentFillSpeed {
			get { return _currentFillSpeed; }
			set { _currentFillSpeed = value; }
		}
		
		public string CurrentPosition {
			get { return _currentPosition; }
			set { _currentPosition = value; }
		}

		public MoveState MoveState {
			get { return _moveState; }
			set { _moveState = value; }
		}
				
		/*
		public string MoveData {
			get { return _moveData; }
			set { _moveData = value; }
		}
		*/

		public AttackState AttackState {
			get { return _attackState; }
			set { _attackState = value; }
		}

		public int SkillLogicID {
			get { return _mSkillID; }
			set { _mSkillID = value; }
		}

		public List<AttackedTarget> AttackedTargets {
			get { return _attackedTarget; }
			set { _attackedTarget = value; }
		}
		
		/// <summary>
		/// tzz added
		/// Gets or sets the battle buffers list.
		/// </summary>
		/// <value>
		/// The battle buffers list.
		/// </value>
		public List<int> BattleBuffersList {
			get { return mFireBufferList; }
			set { mFireBufferList = value; }
		}
		
			
		private long _shipID;
		private int _currentLife;
		private int _currentPower;
		private int _currentFillSpeed;
		
		private string _currentPosition;
		
		// Default is stop
		private MoveState _moveState;
		//private string _moveData;
		
		// Default is normal attack
		private AttackState _attackState;
		
		// Optional
		private int _mSkillID;
		
		// Can multiple target
		private List<AttackedTarget> _attackedTarget = new List<AttackedTarget> ();
		//private List<BattleBuffer> _battleBuffer = new List<BattleBuffer> ();
		
		/// <summary>
		/// tzz added
		/// The fire buffer list for fire buffer
		/// </summary>
		private List<int>		mFireBufferList = null;
	}
	
	public class BattleStep
	{
		public int StepID {
			get { return _stepID; }
			set { _stepID = value; }
		}
		
		public int NextStepID {
			get { return _nextStepID; }
			set { _nextStepID = value; }
		}
		
		public List<BattleShip> BattleShips {
			get { return _battleShip; }
			set { _battleShip = value; }
		}
		
		private int _stepID;
		private int _nextStepID;
		private List<BattleShip> _battleShip = new List<BattleShip> ();
	}
	
	public class WarshipBattleEndCurrLife
	{
		public long ShipID
		{
			get{return _shipID; }
			set{_shipID = value;}
		}
		
		public int ShipCurrLife
		{
			get{return _shipCurrLife; }
			set{_shipCurrLife = value;}
		}
		private long _shipID;
		private int _shipCurrLife;
	}
	
	// The battle result data
	public class BattleResult
	{
		public BattleResult ()
		{
		}
		
		public BattleType BattleType
		{
			get { return battleType; }
			set { battleType = value; }
		}
		
		public SneakAttackType SneakAttackType
		{
			get { return sneakAttackType; }
			set { sneakAttackType = value; }
		}
		
		public bool IsFinalBattle
		{
			get { return _isFinalBattle; }
			set { _isFinalBattle = value; }
		}
		
		public int ErrorCode {
			get { return _errorCode; }
			set { _errorCode = value; }
		}

		public List<BattleStep> BattleSteps {
			get { return _battleSteps; }
			set { _battleSteps = value; }
		}

		public List<Fleet> Fleets {
			get { return _fleets; }
			set { _fleets = value; }
		}
		
		public Dictionary<int, BuffData> AttackerBuffDataList
		{
			get { return attckerBuffDataList; }
			set { attckerBuffDataList = value; }
		}
		
		public Dictionary<int, BuffData> AttackedBuffDataList
		{
			get { return attackedBuffDataList; }
			set { attackedBuffDataList = value; }
		}
		
		public DropData NPCDropData 
		{
			get { return _npcDropData; }
			set { _npcDropData = value; }
		}
		
		public EBattleWinResult BattleWinResult 
		{
			get { return _battleWinResult; }
			set { _battleWinResult = value; }
		}
		
		public int ChestID
		{
			get { return _chestID; }
			set { _chestID = value; }
		}
		
		public List<WarshipBattleEndCurrLife> ListWarshipBattleEndCurrLife
		{
			get{return _listWarshipBattleEndCurrLife; }
			set{_listWarshipBattleEndCurrLife = value;}
		}
		
		/**
		 * @tzz
		 * read from the Text stream
		 * 
		 * @param _reader		reader stream (file or other stream)
		 */ 
		public void ReadFromStream(StreamReader MReader){
			
		 	int fleetCount = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 	for (int fleetIdx = 0; fleetIdx < fleetCount; ++fleetIdx)
		 	{
		 		Fleet fleetData = new Fleet();
		 		
		 		fleetData.FleetID = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 		fleetData.NPCFleet = StrParser.ParseBool( MReader.ReadLine(), false );
		 		fleetData.IsAttacker = StrParser.ParseBool( MReader.ReadLine(), false );
		 		 		
		 		int shipCount = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 		for (int shipIdx = 0; shipIdx < shipCount; ++shipIdx)
		 		{
		 			Ship shipData = new Ship();
	 				shipData.ShipID 			= StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 		 	shipData.LogicShipID 		= StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			shipData.InitialCurrentLife = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			shipData.MaxLife 			= StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			shipData.Position 			= MReader.ReadLine();
					shipData.IsNpc				= StrParser.ParseBool(MReader.ReadLine(), false );
					shipData.TypeID		= StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 
		 			fleetData.Ships.Add(shipData);
		 		}
		 		
		 		this.Fleets.Add(fleetData);
		 	}
		 	
		 	int battleStepCount = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 	for (int battleStepIdx = 0; battleStepIdx < battleStepCount; ++battleStepIdx)
		 	{
		 		BattleStep battleStep = new BattleStep();
		 		battleStep.StepID = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 		battleStep.NextStepID = StrParser.ParseDecInt(MReader.ReadLine(),-1);
		 		int battleShipCount = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 		for (int battleShipIdx = 0; battleShipIdx < battleShipCount; ++battleShipIdx)
		 		{
		 			BattleShip battleship = new BattleShip();
		 			battleship.ShipID = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			battleship.CurrentLife = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			battleship.CurrentPower = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			battleship.CurrentPosition = MReader.ReadLine();
					battleship.CurrentFillSpeed = StrParser.ParseDecInt(MReader.ReadLine(),-1);
		 			battleship.MoveState = (GameData.BattleGameData.MoveState)StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			battleship.AttackState = (GameData.BattleGameData.AttackState)StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			battleship.SkillLogicID = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
							 			
		 			int targetCount = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 			for (int targetIdx = 0; targetIdx < targetCount; ++targetIdx)
		 			{
		 				AttackedTarget target = new AttackedTarget();
		 				
		 				target.ShipID = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 				target.Damage = StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 				target.AttackedState = (AttackedState)StrParser.ParseDecInt( MReader.ReadLine(), -1 );
		 				
		 				battleship.AttackedTargets.Add(target);
		 			}
		 			
		 			battleStep.BattleShips.Add(battleship);
		 		}
		 		this.BattleSteps.Add(battleStep);
		 	}
		 	
		 	bool t_hasNPCDropItem = StrParser.ParseBool( MReader.ReadLine(), false );
		 	if (t_hasNPCDropItem)		 	{
		 		_npcDropData = new DropData();
				
				_npcDropData.BasicContribution 	= StrParser.ParseDecInt(MReader.ReadLine(),-1);
				_npcDropData.BasicExp 			= StrParser.ParseDecInt(MReader.ReadLine(),-1);
				_npcDropData.CopyScore		 	= StrParser.ParseDecInt(MReader.ReadLine(),-1);
				_npcDropData.Feat			 	= StrParser.ParseDecInt(MReader.ReadLine(),-1);
				_npcDropData.Ingot			 	= StrParser.ParseDecInt(MReader.ReadLine(),-1);
				_npcDropData.Oil			 	= StrParser.ParseDecInt(MReader.ReadLine(),-1);
				_npcDropData.Type			 	= (DropType)StrParser.ParseDecInt(MReader.ReadLine(),-1);
				
				int t_dorpNum = StrParser.ParseDecInt(MReader.ReadLine(),0);
				for(int i = 0;i < t_dorpNum;i++){
					ItemSlotData t_item = new ItemSlotData();
					t_item.LocationID			= StrParser.ParseDecInt(MReader.ReadLine(),-1);
					// TODO: read the drop item
				}
		 	}
		 	
		 	_battleWinResult 	= (EBattleWinResult)StrParser.ParseDecInt(MReader.ReadLine(),0);
		 	_chestID			= StrParser.ParseDecInt(MReader.ReadLine(),-1);
		 	_errorCode 			= StrParser.ParseDecInt( MReader.ReadLine(), -1 );
			
			// calculate fire buffer
			CalcFireBuffer();
		}
		
		/**
		 * @tzz
		 * write to text stream
		 * 
		 * @param _writer		write stream (file or other stream)
		 */ 
		public void WriteToStream(StreamWriter MWriter){
			
			MWriter.WriteLine(Fleets.Count);
		 	foreach (GameData.BattleGameData.Fleet fleetData in this.Fleets) 
		 	{
		 		// fleetData.FleetID;
		 		// fleetData.NPCFleet;
		 		
		 		MWriter.WriteLine(fleetData.FleetID);
		 		MWriter.WriteLine(fleetData.NPCFleet);
		 		MWriter.WriteLine(fleetData.IsAttacker);
		 		
		 		MWriter.WriteLine(fleetData.Ships.Count);
		 		foreach (GameData.BattleGameData.Ship shipData in fleetData.Ships)
		 		{
		 		// 	shipData.ShipID;
		 		// 	shipData.LogicShipID;
		 		// 	shipData.InitialCurrentLife;
		 		// 	shipData.MaxLife;
		 		// 	shipData.Position;
		 			
		 			MWriter.WriteLine(shipData.ShipID);
		 			MWriter.WriteLine(shipData.LogicShipID);
		 			MWriter.WriteLine(shipData.InitialCurrentLife);
		 			MWriter.WriteLine(shipData.MaxLife);
		 			MWriter.WriteLine(shipData.Position);
					MWriter.WriteLine(shipData.IsNpc);
					MWriter.WriteLine(shipData.TypeID);
		 		}
		 	}
		 	
		 	MWriter.WriteLine(this.BattleSteps.Count);
		 	foreach (GameData.BattleGameData.BattleStep battleStep in this.BattleSteps) 
		 	{
		 		// battleStep.StepID;
		 		MWriter.WriteLine(battleStep.StepID);
		 		MWriter.WriteLine(battleStep.NextStepID);
		 		MWriter.WriteLine(battleStep.BattleShips.Count);
		 		foreach (GameData.BattleGameData.BattleShip battleship in battleStep.BattleShips)
		 		{
		 			// battleship.ShipID;
		 			// battleship.CurrentLife;
		 			// battleship.CurrentPower;
		 			// battleship.CurrentPosition;
		 			// battleship.MoveState;
		 			// battleship.AttackState;
		 			// battleship.SkillLogicID;
		 			
		 			MWriter.WriteLine(battleship.ShipID);
		 			MWriter.WriteLine(battleship.CurrentLife);
		 			MWriter.WriteLine(battleship.CurrentPower);
		 			MWriter.WriteLine(battleship.CurrentPosition);
					MWriter.WriteLine(battleship.CurrentFillSpeed);
		 			MWriter.WriteLine( (int)battleship.MoveState);
		 			MWriter.WriteLine( (int)battleship.AttackState);
		 			MWriter.WriteLine(battleship.SkillLogicID);
		 			
		 			MWriter.WriteLine(battleship.AttackedTargets.Count);
		 			foreach (AttackedTarget target in battleship.AttackedTargets)
		 			{
		 				// target.ShipID;
		 				// target.Damage;
		 				// target.AttackedState;
		 				
		 				MWriter.WriteLine(target.ShipID);
		 				MWriter.WriteLine(target.Damage);
		 				MWriter.WriteLine( (int)target.AttackedState);
		 			}
		 		}
		 	}
		 	
			// npc drop data
			//
		 	if(_npcDropData == null){
				MWriter.WriteLine(false);
			}else{
				MWriter.WriteLine(true);				
				
				MWriter.WriteLine(_npcDropData.BasicContribution);
				MWriter.WriteLine(_npcDropData.BasicExp);
				MWriter.WriteLine(_npcDropData.CopyScore);
				MWriter.WriteLine(_npcDropData.Feat);
				MWriter.WriteLine(_npcDropData.Ingot);
				MWriter.WriteLine(_npcDropData.Oil);
				MWriter.WriteLine((int)_npcDropData.Type);
				
				
				MWriter.WriteLine(_npcDropData.MItemSlotDataList.Count);
				foreach(ItemSlotData item in _npcDropData.MItemSlotDataList){
					MWriter.WriteLine(item.LocationID);
					// TODO: write drop item
				}
			}
		 	
		 	MWriter.WriteLine( (int)_battleWinResult);
		 	MWriter.WriteLine( _chestID);		 	
		 	MWriter.WriteLine(ErrorCode);
		}
		
		/// <summary>
		/// Calculates the fire buffer.
		/// </summary>
		public void CalcFireBuffer(){
			
			Dictionary<long,List<int>> tStepFireBufferList = new Dictionary<long, List<int>>();
						
			for(int i = 0;i < BattleSteps.Count;i++){
				
				GameData.BattleGameData.BattleStep gameBattleStep = BattleSteps[i];
				
				for(int j = 0;j < gameBattleStep.BattleShips.Count;j++){
					GameData.BattleGameData.BattleShip gameBattleShip = gameBattleStep.BattleShips[j];
					
					
					// tzz added
					// add to fire buffer list to calculate the buffer fire time
					List<int> tShipList;
					if(!tStepFireBufferList.TryGetValue(gameBattleShip.ShipID,out tShipList)){
						tShipList = new List<int>();
						tStepFireBufferList.Add(gameBattleShip.ShipID,tShipList);
					}
					
					if(gameBattleShip.AttackState != GameData.BattleGameData.AttackState.IDLE){
						tShipList.Add(i);
					}
					
					gameBattleShip.BattleBuffersList = tShipList;
				}
			}
		}
		
		private BattleType battleType;
		private SneakAttackType sneakAttackType = GameData.BattleGameData.SneakAttackType.FACE_ATTACK;
		
		private bool _isFinalBattle = false;
		private List<Fleet> _fleets = new List<Fleet> ();
		private List<BattleStep> _battleSteps = new List<BattleStep> ();
		private Dictionary<int, BuffData> attckerBuffDataList = new Dictionary<int, BuffData>();
		private Dictionary<int, BuffData> attackedBuffDataList = new Dictionary<int, BuffData>();
		private List<WarshipBattleEndCurrLife> _listWarshipBattleEndCurrLife = new List<WarshipBattleEndCurrLife>();
		
		private DropData _npcDropData;
		private EBattleWinResult _battleWinResult;
		private int _chestID;
		
		private int _errorCode;
		
		public int TotalBattleShipNum = 0;
	}
	
	public class Ship
	{
		public Ship ()
		{
		}
   
		public int LogicShipID {
			get { return _logicShipID; }
			set { _logicShipID = value; }
		}
	    
		public long ShipID {
			get { return _shipID; }
			set { _shipID = value; }
		}
		
		public string ShipName {
			get { return _shipName; }
			set { _shipName = value; }
		}
		
		public int MaxLife
		{
			get { return _maxLife; }
			set { _maxLife = value; }
		}
		
		public int InitialCurrentLife
		{
			get { return _initialCurrentLife; }
			set { _initialCurrentLife = value; }
		}
		
		public int TypeID
		{
			get { return _warshipTypeID; }
			set { _warshipTypeID = value; }
		}
	
		public string Position 
		{
			get { return _position; }
			set { _position = value; }
		}
		
		public bool IsNpc
		{
			get { return _isNpc; }
			set { _isNpc = value; }
		}
		
		private long _shipID;
		private int _logicShipID;
		private string _shipName;
		
		private int _maxLife;
		private int _initialCurrentLife;
		private int _warshipTypeID;
			
		private string _position;
		private bool _isNpc;
		public int GeneralLogicID = 0;
		public string GeneralAvatar = "";
	}
	
	public class Fleet
	{
		public Fleet ()
		{
		}

		public long FleetID {
			get { return _fleetID; }
			set { _fleetID = value; }
		}

		public bool NPCFleet {
			get { return _nPCFleet; }
			set { _nPCFleet = value; }
		}
		
		public bool IsAttacker 
		{
			get { return _isAttacker; }
			set { _isAttacker = value; }
		}

		public List<Ship> Ships {
			get { return _ships; }
			set { _ships = value; }
		}
		
		public GameData.FormationData CurFmtData 
		{
			get { return curFmtData; }
			set { curFmtData = value; }
		}

		private bool _nPCFleet;
		private bool _isAttacker;
		private long _fleetID;
		private List<Ship> _ships = new List<Ship> ();
		
		GameData.FormationData curFmtData = null;
	}
	
	public enum EBattleWinResult
	{
		ACTOR_WIN = 0,
		MONSTER_WIN = 1,
		DOGFALL = 2,
	}
}
