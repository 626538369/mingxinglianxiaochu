using UnityEngine;
using System.Collections;

public class MonsterWarship : WarshipL
{
	//------------------------------------------------------------
	public MonsterProperty Property
	{
		get { return _mProperty; }
	}
	
	public VisibleTrigger VisibleTrigger
	{
		get { return _mVisibleTrigger; }
	}
	//------------------------------------------------------------
	
	public MonsterWarship(CopyMonsterData.MonsterData data) : base(null)
	{
		_mData = data;
		 base._warshipName = data.MonsterName;
		 base._warshipResourceName = data.ModelName;
	}
	
	public void Initialize(WarshipL.CreateCallback callback)
	{
		base.Initialize(delegate (WarshipL ws)
		{
			base._mMoveSpeed = 40.0f;
		
			// Set the monster fleet orientation
			Quaternion rotation = Quaternion.AngleAxis(180, Vector3.up);
			base.U3DGameObject.transform.rotation = rotation;
			
			Show3DName(true);
			
			_mVisibleTrigger = null;
			InitTriggers();
			InitAIScripts();
			
			TagMaskDefine.SetTagRecuisively(U3DGameObject, TagMaskDefine.GFAN_MONSTER);
			if (null != callback)
				callback(this);
		}
		);
	}
	
	public void Release()
	{
		base.Release();
		_mVisibleTrigger = null;
	}
	
	public void AddTriggerCallback(TriggerBase.OnTriggerEvent trigger)
	{
		BattleTrigger battleTrigger = U3DGameObject.GetComponentInChildren(typeof(BattleTrigger)) as BattleTrigger;
		if (battleTrigger == null)
			return;
		
		battleTrigger.TriggerEnterEvents += trigger;
	}
	
	public void RemoveTriggerCallback(TriggerBase.OnTriggerEvent trigger)
	{
		BattleTrigger battleTrigger = U3DGameObject.GetComponentInChildren(typeof(BattleTrigger)) as BattleTrigger;
		if (battleTrigger == null)
			return;
		
		battleTrigger.TriggerEnterEvents -= trigger;
	}
	
	public void Update()
	{
		base.Update();
	}
	
	public override void AddPropertyComp()
	{
		_mProperty = U3DGameObject.AddComponent(typeof(MonsterProperty)) as MonsterProperty;
		_mProperty.FleetID = _mData.FleetID;
		_mProperty.Name = _mData.MonsterName;
		_mProperty.DialogText = _mData.MonsterDialog;
		_mProperty.ModelName = _mData.ModelName;
	}
	
	public override void Create3DName()
	{		
		string showText = GUIFontColor.PureRed + _mProperty.Name;
		base._mEZ3DName = (EZ3DSimipleText)Globals.Instance.M3DItemManager.Create3DSimpleText(U3DGameObject, showText,WarshipL.NAME_HEIGHT_OFFSET);
		base._mEZ3DName.gameObject.name = _mProperty.Name;
	}
	
	public override void SetActive(bool visible)
	{
		base.SetActive(visible);
	}
	
	public override void Stop()
	{
		base.Stop();
		StopAllAI();
	}
	
	private void InitTriggers()
	{
		Object obj = Resources.Load("Common/DetectTrigger");
		GameObject go = GameObject.Instantiate(obj) as GameObject;
		
		// Ensure the original offset
		Vector3 localPos = go.transform.position;
		Quaternion localRot = go.transform.rotation;
		go.name = "DetectTrigger";
		go.transform.parent = U3DGameObject.transform;
		go.transform.localPosition = localPos;
		go.transform.localRotation = localRot;
		if (null != _mData)
			go.transform.localScale *= _mData.DetectionRangeFactor;
		
		obj = Resources.Load("Common/BattleTrigger");
		go = GameObject.Instantiate(obj) as GameObject;
		// Ensure the original offset
		localPos = go.transform.position;
		localRot = go.transform.rotation;
		go.name = "BattleTrigger";
		go.transform.parent = U3DGameObject.transform;
		go.transform.localPosition = localPos;
		go.transform.localRotation = localRot;
		BattleTrigger trigger = go.GetComponent<BattleTrigger>() as BattleTrigger;
		trigger.Radius = 100.0f;
		if (null != _mData)
			go.transform.localScale *= _mData.BattleRangeFactor;
		
		// VisibleTrigger
		_mVisibleTrigger =U3DGameObject.GetComponentInChildren<VisibleTrigger>() as VisibleTrigger;
	}
	
	private void InitAIScripts()
	{
		Object obj = Resources.Load(_mData.PathPointsFile);
		// Object obj = Resources.Load("PathPoints/Copy1PathPoint1");
		GameObject go = GameObject.Instantiate(obj) as GameObject;
		go.name = "AIPathPatrol";
		go.transform.parent = U3DGameObject.transform;
		
//		AIBase script = go.GetComponent<AIPathPatrol>() as AIPathPatrol;
//		script.ControlGameObj = U3DGameObject;
		
		go = new GameObject();
		go.name = "AIChase";
		go.transform.parent = U3DGameObject.transform;
		AIChase chaseAI = go.AddComponent<AIChase>() as AIChase;
		chaseAI.ControlGameObj = U3DGameObject;
		chaseAI.Speed = MoveSpeed;
	}
	
	public void StartPathPatrolAI()
	{
		if (null == U3DGameObject)
			return;
//		
//		AIBase script = U3DGameObject.GetComponentInChildren(typeof(AIChase)) as AIChase;
//		if (script != null)
//			script.Interrupt();
//		
//		script = U3DGameObject.GetComponentInChildren(typeof(AIPathPatrol)) as AIPathPatrol;
//		if (script != null)
//			script.Restart();
	}
	
	public void StartChaseAI(GameObject target)
	{
//		AIBase script = U3DGameObject.GetComponentInChildren(typeof(AIPathPatrol)) as AIPathPatrol;
//		if (script != null)
//			script.Interrupt();
//		
//		AIChase script1 = U3DGameObject.GetComponentInChildren(typeof(AIChase)) as AIChase;
//		if (script1 != null)
//		{
//			script1.TargetGameObj = target;
//			script1.Restart();
//		}
	}
	
	public void StopAllAI()
	{
//		AIBase script = U3DGameObject.GetComponentInChildren(typeof(AIPathPatrol)) as AIPathPatrol;
//		if (script != null)
//		{
//			script.Interrupt();
//		}
//		
//		script = U3DGameObject.GetComponentInChildren(typeof(AIChase)) as AIChase;
//		if (script != null)
//		{
//			script.Interrupt();
//		}
	}
	
	private MonsterProperty _mProperty;
	private CopyMonsterData.MonsterData _mData;
	private VisibleTrigger _mVisibleTrigger;
}
