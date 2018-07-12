using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage2DCopy : Stage2D
{
	//-----------------------------------------------------------------
	public Transform sceneRoot;
	public Transform monsterRoot;
	public Transform chestsRoot;
	
	public UIButton screenEvtBtn = null;
	public KnightAvatarSlot actor = null;
	public PackedSprite WebwayGates = null;
	//-----------------------------------------------------------------
	
	static readonly Vector3 offsetCenter = new Vector3(5.0f, 5.0f, 0.0f);
	static readonly Vector3 moveSpeed = new Vector3(50f, 30f, 1000f);
	
	[HideInInspector] public const int PageCnt = 2;
	Vector3 sizePerScenePage = new Vector3(GUIManager.DEFAULT_SCREEN_WIDTH,GUIManager.DEFAULT_SCREEN_HEIGHT, 0.0f);
	Vector3 minBackgroundPos, maxBackgroundPos;
	Vector3 mBackgroundInitPos;
	Vector3 mActorDestPos;
	
	List<GameObject> monsterObjs = new List<GameObject>();
	List<GameObject> chestObjs = new List<GameObject>();
	[HideInInspector] public bool mActorMoving = false;
	[HideInInspector] public bool mSceneMoving = false;
	
	Vector3 actorStartPoint = Vector3.zero;
	
	bool isResponseInput = true;
	
	//-----------------------------------------------------------------
	public override void Init()
	{
		Vector3 localScale = Vector3.one;
		localScale.x = Globals.Instance.MGUIManager.widthRatio;
		localScale.y = Globals.Instance.MGUIManager.heightRatio;
		transform.localScale = localScale;
		
		sizePerScenePage.x = GUIManager.DEFAULT_SCREEN_WIDTH * Globals.Instance.MGUIManager.widthRatio;
		sizePerScenePage.y = GUIManager.DEFAULT_SCREEN_HEIGHT * Globals.Instance.MGUIManager.heightRatio;
		
		// Limit rect
		minBackgroundPos = -0.5f * sizePerScenePage;
		minBackgroundPos.y = -0.5f * sizePerScenePage.y;
		
		maxBackgroundPos = ((float)PageCnt - 0.5f) * sizePerScenePage;
		maxBackgroundPos.y = 0.5f * sizePerScenePage.y;
		
		InitBackground();
		
		InitActor();
		InitMonsterObjs();
		InitChests();
		
		InitIndicator();
		HideWebwayGates(true, Vector3.zero);
		
		//screenEvtBtn.SetInputDelegate(OnScreenBtnInputDel);
		UIButton gateEventBtn = (UIButton)WebwayGates.transform.Find("EventNullBtn164_164").GetComponent(typeof(UIButton));
		//gateEventBtn.SetInputDelegate(OnScreenBtnInputDel);
	}
		
	void OnDestroy()
	{
		childMonsters.Clear();
		childChests.Clear();
		
		monsterObjs.Clear();
		chestObjs.Clear();
	}
	
	void setMoveControlTime(Vector3 offset)
	{
		float dist = (offset.x)* (offset.x) +  (offset.y)* (offset.y) ;
		float widthRatio = Globals.Instance.MGUIManager.widthRatio;
	    float time = dist / (300*300*widthRatio*widthRatio);
		actor.MoveCtl.setTime(time);
		sceneMoveCtl.setTime(time);
	}
	
//void OnScreenBtnInputDel(ref POINTER_INFO ptr)
//{
//	if (!isResponseInput)
//		return;
//	
//	if (ptr.evt == POINTER_INFO.INPUT_EVENT.PRESS
//		// || ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE
//		|| ptr.evt == POINTER_INFO.INPUT_EVENT.DRAG)
//	{
//		actor.MoveCtl.Stop();
//		sceneMoveCtl.Stop();
//		
//		Vector3 objPos = actor.transform.position;
//		Vector3 actorGUIPos = GUIManager.WorldToGUIPoint(Globals.Instance.MGUIManager.MGUICamera, objPos);
//	
//		Vector3 inputGUIPos = GUIManager.ScreenToGUIPoint(ptr.devicePos);
//		inputGUIPos = GetLimitGUIPos(inputGUIPos);
//		
//		Vector3 offset = inputGUIPos - actorGUIPos;
//		beginMoveOffset(offset);
//	}
//}
	
	public void moveToTargetPostion(Vector3 targetPos)
	{
		 Vector3 objPos = actor.transform.position;
		 Vector3 offset = targetPos - objPos;
		 beginMoveOffset(offset);
	}
	
	private void beginMoveOffset(Vector3 offset)
	{
		Vector3 objPos = actor.transform.position;
		offset.z = 0;
			
		setMoveControlTime(offset);
		// Actor
		float halfSizeX = 0.5f * actor.Size.x;
		float halfSizeY = 0.5f * actor.Size.y;
		
		Vector3 actorCurrentPos = objPos + (mBackgroundInitPos - sceneMoveCtl.transform.position);
		Vector3 blessOffsetPos = blessOffset(actorCurrentPos,offset);
		mActorDestPos = objPos + offset + (mBackgroundInitPos - sceneMoveCtl.transform.position);
		if (IsNeedMoveActor(actorCurrentPos))
		{
			if (offset.x < minBackgroundPos.x + halfSizeX)
				offset.x = minBackgroundPos.x + halfSizeX;
			if (offset.y < minBackgroundPos.y + halfSizeY)
				offset.y = minBackgroundPos.y + halfSizeY;
			
			if (offset.x > maxBackgroundPos.x - halfSizeX)
				offset.x = maxBackgroundPos.x - halfSizeX;
			if (offset.y > maxBackgroundPos.y - halfSizeY)
				offset.y = maxBackgroundPos.y - halfSizeY;
		
			objPos += offset;
			actor.MoveCtl.MoveTo(objPos,delegate(){
				update();
				//Debug.Log("11111111111111111111IsNeedMoveActor()");
			});
			mActorMoving = true;
		}
		else
		{
			//if (!inputGUIPos.y.Equals(0.0f))
			{
				if (offset.y < minBackgroundPos.y + halfSizeY)
					offset.y = minBackgroundPos.y + halfSizeY;
				if (offset.y > maxBackgroundPos.y - halfSizeY)
					offset.y = maxBackgroundPos.y - halfSizeY;
			}
			
			objPos.y += offset.y;
			actor.MoveCtl.MoveTo(objPos,delegate() {
				update();
			});
		}
		
		if (IsNeedMoveScene(actorCurrentPos))
		{
			halfSizeX = 0.5f * sizePerScenePage.x;
			halfSizeY = 0.5f * sizePerScenePage.y;
			if (offset.x < minBackgroundPos.x + -halfSizeX)
				offset.x = minBackgroundPos.x + -halfSizeX;
			if (offset.y < minBackgroundPos.y + halfSizeY)
				offset.y = minBackgroundPos.y + halfSizeY;
			
			if (offset.x > maxBackgroundPos.x - halfSizeX)
				offset.x = maxBackgroundPos.x - halfSizeX;
			if (offset.y > maxBackgroundPos.y - halfSizeY)
				offset.y = maxBackgroundPos.y - halfSizeY;
			
			objPos = sceneMoveCtl.transform.position;
			offset.y = 0;
			objPos -= offset;
			sceneMoveCtl.MoveTo(objPos,delegate() {
				update();
				//Debug.Log("11111111111111111111IsNeedMoveScene()");
			});
			mSceneMoving = true;
		}
		
		if (null != indicator)
		{
			// Set the indicator position
			objPos = indicator.transform.position;
			objPos += offset;
			indicator.transform.position = objPos;
		}
	}
	
	private Vector3 blessOffset(Vector3 actorPos,Vector3 offset)
	{
			float halfSizeX = 0.5f * actor.Size.x;
			float halfSizeY = 0.5f * actor.Size.y;
			if (IsNeedMoveActor(actorPos))
			{
				if (offset.x < minBackgroundPos.x + halfSizeX)
					offset.x = minBackgroundPos.x + halfSizeX;
				if (offset.y < minBackgroundPos.y + halfSizeY)
					offset.y = minBackgroundPos.y + halfSizeY;
				
				if (offset.x > maxBackgroundPos.x - halfSizeX)
					offset.x = maxBackgroundPos.x - halfSizeX;
				if (offset.y > maxBackgroundPos.y - halfSizeY)
					offset.y = maxBackgroundPos.y - halfSizeY;
	     	}
		
			if (IsNeedMoveScene(actorPos))
			{
				halfSizeX = 0.5f * sizePerScenePage.x;
				halfSizeY = 0.5f * sizePerScenePage.y;
				if (offset.x < minBackgroundPos.x + -halfSizeX)
					offset.x = minBackgroundPos.x + -halfSizeX;
				if (offset.y < minBackgroundPos.y + halfSizeY)
					offset.y = minBackgroundPos.y + halfSizeY;
				
				if (offset.x > maxBackgroundPos.x - halfSizeX)
					offset.x = maxBackgroundPos.x - halfSizeX;
				if (offset.y > maxBackgroundPos.y - halfSizeY)
					offset.y = maxBackgroundPos.y - halfSizeY;
		}
		
		return offset;
		
	}
	
	bool IsNeedMoveActor(Vector3 actorGUIPos)
	{
		// At left half of first page  or at half right or end page
		if (actorGUIPos.x <= (minBackgroundPos.x + 0.5f * sizePerScenePage.x))
		{
			return true;
		}
		
		if (actorGUIPos.x >= (maxBackgroundPos.x - 0.5f * sizePerScenePage.x))
		{
			return true;
		}
		
		return false;
	}
	
	bool IsNeedMoveScene(Vector3 actorGUIPos)
	{
		if (actorGUIPos.x > (minBackgroundPos.x + 0.5f * sizePerScenePage.x)
			&& actorGUIPos.x < (maxBackgroundPos.x - 0.5f * sizePerScenePage.x))
		{
			return true;
		}
		
		return false;
	}
	
	Vector3 GetLimitGUIPos(Vector3 inputGUIPos)
	{
		if (inputGUIPos.x < minBackgroundPos.x)
			inputGUIPos.x = minBackgroundPos.x;
		if (inputGUIPos.y < minBackgroundPos.y)
			inputGUIPos.y = minBackgroundPos.y;
		
		if (inputGUIPos.x > maxBackgroundPos.x)
			inputGUIPos.x = maxBackgroundPos.x;
		if (inputGUIPos.y > maxBackgroundPos.y)
			inputGUIPos.y = maxBackgroundPos.y;
		
		return inputGUIPos;
	}
	
	public void update()
	{
		Vector3 objPos = actor.transform.position;
		Vector3 actorCurrentPos = objPos + (mBackgroundInitPos - sceneMoveCtl.transform.position);
		if (mActorMoving && IsNeedMoveScene(actorCurrentPos))
		{
				actor.MoveCtl.Stop();
				Vector3 offset =  actor.MoveCtl.getDestPosition() - actor.transform.position;
				setMoveControlTime(offset);
				objPos = sceneMoveCtl.transform.position;
				offset.y = 0;
				objPos -= offset;
				sceneMoveCtl.MoveTo(objPos);
			    mActorMoving  = false;
			   mSceneMoving = true;
		}
		if ( mSceneMoving && IsNeedMoveActor(actorCurrentPos))
		{
			sceneMoveCtl.Stop();
					
			Vector3 offset = mActorDestPos - actorCurrentPos;
			setMoveControlTime(offset);
			objPos += offset;
			actor.MoveCtl.MoveTo(objPos);
			mSceneMoving = false;
			mActorMoving = true;
		}
	}
	
	ObjMoveControl sceneMoveCtl = null;
	void InitBackground()
	{
		sceneMoveCtl = sceneRoot.gameObject.GetComponent<ObjMoveControl>() as ObjMoveControl;
		if (null == sceneMoveCtl)
			sceneMoveCtl = sceneRoot.gameObject.AddComponent<ObjMoveControl>() as ObjMoveControl;
		
		sceneMoveCtl.MoveSpeed = moveSpeed;
		mBackgroundInitPos = sceneMoveCtl.transform.position;
	}
	
	void InitActor()
	{
		PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
		
		GirlData wsData = actorData.GetActorFakeWarshipData();
		if (null != wsData)
			actor.UpdateSlot(wsData);
		else
			actor.UpdateAvatarIcon("");
		actor.IsNeedDynamicAdjustColl = true; // up and down circle
		
		TagMaskDefine.SetTagRecuisively(actor.gameObject, TagMaskDefine.GFAN_ACTOR);
		
		// Add move controller
		actor.MoveCtl = actor.gameObject.GetComponent<ObjMoveControl>() as ObjMoveControl;;
		if (null == actor.MoveCtl) 
			actor.MoveCtl = actor.gameObject.AddComponent<ObjMoveControl>() as ObjMoveControl;
		actor.MoveCtl.MoveSpeed = moveSpeed;
		
		//add rigid body 
		Rigidbody rigidbody = actor.gameObject.AddComponent<Rigidbody>() as Rigidbody;
		if(rigidbody)
		{
			rigidbody.useGravity = false;
			rigidbody.isKinematic = true;
		}
			
		BoxCollider coll = actor.gameObject.GetComponent<BoxCollider>() as BoxCollider;
		if (null == coll) coll = actor.gameObject.AddComponent<BoxCollider>() as BoxCollider;
		if(coll)
		{
			coll.isTrigger = true;
			coll.center = new Vector3(0,0,0);
			coll.size =actor.eventBtn.GetComponent<Collider>().bounds.size;
		}
		
		actorStartPoint = actor.transform.position;
	}
	
	const float EliteCardScale = 1.15f;
	const float BossCardScale = 1.25f;
	void InitMonsterObjs()
	{
		childMonsters.Clear();
		
		// Hide all buildings
		foreach (Transform child in monsterRoot)
		{
			child.gameObject.SetActiveRecursively(false);
			childMonsters.Add(child);
		}
		childMonsters.Sort(delegate(Transform a, Transform b)
		{
			// Sort by name
			return string.Compare(a.name, b.name);
		});
		
		CopyData cpData = Globals.Instance.MGameDataManager.MCurrentCopyData;
		CopyMonsterData.MonsterData monsterData = null;
		for (int i = 0; i < cpData.MCopyMonsterData.MMonsterDataList.Count; i++)
		{
			monsterData = cpData.MCopyMonsterData.MMonsterDataList[i];
			
			InitOneMonster(childMonsters[i], monsterData);
			
			if (i == cpData.MCopyMonsterData.MMonsterDataList.Count - 1)
			{
				if (cpData.MCopyBasicData.CopyType == 1)
				{
					childMonsters[i].transform.localScale = new Vector3(EliteCardScale, EliteCardScale, 1.0f);
				}
				else if (cpData.MCopyBasicData.CopyType == 2)
				{
					childMonsters[i].transform.localScale = new Vector3(BossCardScale, BossCardScale, 1.0f);
				}
			}
		}
	}
	
	void InitOneMonster(Transform tf, CopyMonsterData.MonsterData data)
	{
		tf.gameObject.SetActiveRecursively(true);
		
		UIButton btn = (UIButton)tf.GetComponent(typeof(UIButton));
		//btn.SetInputDelegate(OnScreenBtnInputDel);
		
		MonsterSet monster = tf.gameObject.GetComponent<MonsterSet>() as MonsterSet;
		if (null == monster) monster = tf.gameObject.AddComponent<MonsterSet>() as MonsterSet;
		
		TagMaskDefine.SetTagRecuisively(tf.gameObject, TagMaskDefine.GFAN_MONSTER);
			
		monster.UpdateData(data);
		monsterObjs.Add(monster.gameObject);
	}
	
	public void DestroyMonster(GameObject monster)
	{
		monsterObjs.Remove(monster);
		HelpUtil.DestroyObject(monster);
	}
	
	public void DestroyAllmonsterObjs()
	{
		foreach (GameObject go in monsterObjs)
		{
			HelpUtil.DestroyObject(go);
		}
		monsterObjs.Clear();
	}
	
	public int GetMonsterCount()
	{
		return monsterObjs.Count;
	}
	
	public List<GameObject> GetMonsterList()
	{
		return monsterObjs;
	}
	
	void InitChests()
	{
		childChests.Clear();
		
		if (chestsRoot == null)
			return;
		
		foreach (Transform child in chestsRoot)
		{
			child.gameObject.SetActiveRecursively(false);
			childChests.Add(child);
		}
		childChests.Sort(delegate(Transform a, Transform b)
		{
			// Sort by name
			return string.Compare(a.name, b.name);
		});
		
		CopyData cpData = Globals.Instance.MGameDataManager.MCurrentCopyData;
		
		int upperLimit = Mathf.Min(childChests.Count, cpData.MCopyChestData.ChestIDList.Count);
		for (int i = 0; i < upperLimit; i++)
		{
			int id = cpData.MCopyChestData.ChestIDList[i];
			InitOneRandomChest(childChests[i], id);
		}
	}
	
	void InitOneRandomChest(Transform tf, int chestId)
	{
		tf.gameObject.SetActiveRecursively(true);
		
		UIButton btn = (UIButton)tf.GetComponent(typeof(UIButton));
		//btn.SetInputDelegate(OnScreenBtnInputDel);
		
		PackedSprite anim = tf.GetComponentInChildren<PackedSprite>() as PackedSprite;
		if (null != anim)
			anim.PlayAnim(0); // Default chest appearance
		
		ChestTrigger trigger = tf.gameObject.AddComponent<ChestTrigger>() as ChestTrigger;
		trigger.ChestID = chestId;
		trigger.BoxSize = new Vector3(anim.width, anim.height, 1.0f);
	}
	
	public void DestroyChest(GameObject go)
	{
		chestObjs.Remove(go);
		GraphicsTools.ManualDestroyObject(go, false);
	}
	
	public void DestroyAllChests()
	{
		foreach (GameObject go in chestObjs)
		{
			HelpUtil.DestroyObject(go);
		}
		chestObjs.Clear();
	}
	
	public int GetChestCount()
	{
		return chestObjs.Count;
	}
	
	void InitIndicator()
	{
		if (null == indicator)
		{
			GameObject obj = GameObject.Find("Indicator");
			if (null != obj)
				indicator = obj.GetComponent<PackedSprite>() as PackedSprite;
		}
		
		if (null == indicator)
		{
			Debug.Log("Has to much Indicator GameObject???");
			return;
		}
	}
	
	void ClearIndicator()
	{
		indicator = null;
	}
	
	void InitRandomChests()
	{
	}
	
	public void OnOnceBattleEnd(GameData.BattleGameData.BattleResult battleResult, GameObject fightMonsterObj)
	{
		Vector3 monsterPos = Vector3.zero;
		switch (battleResult.BattleWinResult)
		{
		case GameData.BattleGameData.EBattleWinResult.ACTOR_WIN:
		case GameData.BattleGameData.EBattleWinResult.DOGFALL: // Dogfall
		{
			monsterPos = fightMonsterObj.transform.position;
			DestroyMonster(fightMonsterObj);
			break;
		}
		case GameData.BattleGameData.EBattleWinResult.MONSTER_WIN:
		{
			// Direct back to orignal
			actor.transform.position = actorStartPoint;
			sceneRoot.transform.position = mBackgroundInitPos;
			
			mActorMoving = false;
			mSceneMoving = false;
			break;
		}	
		
		}
		
		HideMovableObjs(false);
		if (battleResult.IsFinalBattle)
			HideWebwayGates(false, monsterPos);
	}
	
	public void HideMovableObjs(bool hide)
	{
		actor.Hide(hide);
		actor.IgnoreCollider(hide);
		
		for (int i = 0; i < monsterObjs.Count; i++)
		{
			monsterObjs[i].SetActiveRecursively(!hide);
		}
		
		if (hide)
		{
			isResponseInput = false;
			
			mActorMoving = false;
			mSceneMoving = false;
			actor.MoveCtl.Stop();
			sceneMoveCtl.Stop();
		}
		else
		{
			isResponseInput = true;
		}
	}
	
	public void HideWebwayGates(bool hide, Vector3 pos)
	{
		if (null == WebwayGates)
			return;
		
		WebwayGates.gameObject.SetActiveRecursively(!hide);
		if (!hide)
		{
			LeaveCopyTrigger leaveTrigger = WebwayGates.gameObject.GetComponent<LeaveCopyTrigger>() as LeaveCopyTrigger;
			if (null == leaveTrigger)
				leaveTrigger = WebwayGates.gameObject.AddComponent<LeaveCopyTrigger>() as LeaveCopyTrigger;
			leaveTrigger.SetSize(Vector3.zero, new Vector3(0.5f * WebwayGates.width, 0.5f * WebwayGates.height, 0.0f));
			
			// pos.x += 0.5f * WebwayGates.width;
			// WebwayGates.transform.position = new Vector3(pos.x, pos.y, pos.z);
			WebwayGates.transform.localScale = Vector3.one;
		}
	}

	PackedSprite indicator = null;
	List<Transform> childMonsters = new List<Transform>();
	List<Transform> childChests = new List<Transform>();
}
