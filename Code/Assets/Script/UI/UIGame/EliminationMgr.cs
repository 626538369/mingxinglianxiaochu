using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class SquareBlocks {
	public SquareTypes block;
	public SquareTypes obstacle;

}
public enum FindSeparating
{
	NONE = 0,
	HORIZONTAL,
	VERTICAL
}

public enum SquareTypes
{
	NONE = 0,
	EMPTY, //1
	BLOCK,//2   可移动元素的一个冰块
	WIREBLOCK,//3   不可移动的基础元素冰块  
	SOLIDBLOCK,//4   不可移动没有元素的元素
	DOUBLEBLOCK,//5    可移动元素的双倍冰块
	UNDESTROYABLE,//6   石头 
	THRIVING//7   不可移动 没有元素 只要存在 每回合 增加一个新的
}
public enum Target {
	SCORE,
	COLLECT,
	INGREDIENT,
	BLOCKS
}

public enum LIMIT {
	MOVES,
	TIME
}

public enum Ingredients {
	None = 0,
	Ingredient1 = 501,
	Ingredient2 = 502
}

public enum CollectItems {
	None = 0,
	Item101 = 101, //知识类 1
	Item102 = 102, //娱乐 2
	Item103 = 103, //生活 3
	Item104 = 104, //商业 4

	Item201 = 201,// 普通攻击
	Item202 = 202,// 火系魔法
	Item203 = 203,// 风系魔法
	Item204 = 204,// 土系魔法
	Item205 = 205,// 水系魔法

	Item301 = 301, //甜蜜之心 6
	Item302 = 302, //破碎之心 7
	Item303 = 303, //魔力点 5
	Item304 = 304,// 怒气种子
	Item305 = 305,// 平静种子

	Item401 = 401,// 炸弹
	Item402 = 402,// 行消除
	Item403 = 403,// 列消除
	Item404 = 404,// 彩虹球
}

public enum GameStatusEnum{
	Map = -1,
	PrepareGame = 0,//准备游戏中
	Playing = 1, // 游戏开始
	PreWinAnimations = 2, //过关后，处理界面可消除状态
	PreFailed = 3,
	GameOver = 4, //游戏失败
	Win = 5, //游戏过关
	RegenLevel = 6, //重新生成元素
	Pause = 7,

}

public class EliminationMgr : MonoBehaviour {

	public static EliminationMgr Instance;
	private GameObject mGameField;
	public Transform GameField{
		get{
			if(mGameField == null){
				mGameField = new GameObject();
				mGameField.AddComponent<AI> ().Init ();
				mGameField.transform.position = new Vector3(-100f,3f,2f);
//				mGameField.layer = 5;//5 ui层
			}
			return mGameField.transform;
		}
	}

	CombineManager combineManager;
	public Camera XiaoChuCamera;
	public GUIPhotoGraph MGUIPhotoGraph;

	// 用来判难断 地图信息  是否加载完成
	public bool levelLoaded;
	// 地图 条件 类型
	public Target target;
	// limitType 游戏结束类型判定   Limit 类型的数值 
	public LIMIT limitType;
	private int mLimit = 0;
	public int Limit{
		get{ return mLimit;}
		set{ 
			mLimit = value;
			if(limitType == LIMIT.MOVES){
				SurplusStepText.text = mLimit.ToString ();
			}
		}
	}

	public int moveID;

	// 地图 可以出现的基础元素种类
	public int colorLimit;
	public List<int> colorLimitLst = new List<int> ();
	public Dictionary<int,int> colorLimitNumDic = new Dictionary<int, int> ();


	public List<GameEnumManager.CellType> BaseElement = new List<GameEnumManager.CellType> ();
	public List<GameEnumManager.CellType> AttrElement = new List<GameEnumManager.CellType> ();

	//格子的宽 高 
	public float squareWidth = 1.2f;
	public float squareHeight = 1.2f;
	// 格子上面的 元素物体 prefab 
	public GameObject itemPrefab;
	// 格子 的prefab  以及 对应的 底图 ， 2个底图只是为了看起来不一样，没什么实际作用
	public GameObject squarePrefab;

	//格子上面 的block障碍物 ，doubleBlock 2层block障碍物的时候 第二层 换一张底图 
	public GameObject blockPrefab;
	public Sprite doubleBlock;
	/// <summary>
	/// WIREBLOCK 冰冻,SOLIDBLOCK 木箱 ,UNDESTROYABLE 石头 ,THRIVING 水珠
	/// </summary>
	public GameObject wireBlockPrefab;
	public GameObject solidBlockPrefab;
	public GameObject undesroyableBlockPrefab;
	public GameObject thrivingBlockPrefab;

	public Sprite squareSprite;
	public Sprite squareSprite1;

	/// <summary>
	/// ingrCountTarget  收集的数量    收集某种元素
	/// ingrTarget    收集的类型 Target.INGREDIENT   不可消除的特殊元素
	/// collectItems   收集类型 Target.COLLECT       现在指的是基础元素
	/// ingrediendSprites 是收集类型的 底图合集
	/// </summary>
	public int[] ingrCountTarget = new int[2];
	public int[] ingrMaxCountTarget = new int[2];
	public Ingredients[] ingrTarget = new Ingredients[2];
	public CollectItems[] collectItems = new CollectItems[2];

//	public GameObject GirlCondition;
	// 过关 收集元素的父节点
	public GameObject ingrObject;
	public UISprite Ingr1;
//	public UILabel MaxIngr1Text;
	public UILabel Ingr1Text;
	public UISprite Ingr2;
//	public UILabel MaxIngr2Text;
	public UILabel Ingr2Text;

	//过关 地形相关的收集类型
	public UISprite BlocksObject;
	public UILabel BlocksNum;
//	public UILabel BlocksCompleteNum;
//	//过关 要求分数
//	public GameObject scoreTargetObject;

//	public Sprite[] ingrediendSprites;
	public SpriteDatabase IngrediendData;
	public SpriteDatabase blockData;

	/// <summary>
	/// 消除元素可获得的分数
	/// </summary>
	public int scoreForBlock = 100;
	public int scoreForWireBlock = 100;
	public int scoreForSolidBlock = 100;
	public int scoreForThrivingBlock = 100;

	private int scoreForMagicItem = 1;
//	private int scoreForSweetItem = 1;
	private float scoreForBreakItem = 1f;
//	private int scoreForAngrySeed = 1;
//	private int scoreForQuietSeed = 1;

	private int scoreForKnowledgeItem = 10;
	private int scoreForRecreationItem = 10;
	private int scoreForLifeItem = 10;
	private int scoreForBusinessItem = 10;

	private int scoreForAttackItem = 10;
	private int scoreForFireMagicItem = 10;
	private int scoreForWindMagicItem = 10;
	private int scoreForLandMagicItem = 10;
//	private int scoreForWaterMagicItem = 10;

	private const int scoreSpeicalFinal = 3;
	private const int scoreFinal = 6;

	public const int GenerateSpecialElements = 2500;
	/// <summary>
	/// 当前的得分
	/// </summary>
	public static int Score;
	public int stars = 0;
	//过关星星分数， 分别对应 1 2 3 星
	public int star1;
	public int star2;
	public int star3;
	//1,2,3 星 对应的开启星星 
	public GameObject Star1Obj;
	public GameObject star1Anim;
	public GameObject Star2Obj;
	public GameObject star2Anim;
	public GameObject Star3Obj;
	public GameObject star3Anim;
//	public UILabel StarScoreText; //当前星星等级需要分数
	public UISlider ScoreSlider; //当前星星得分
	public UILabel CurrentScoreText; //当前得分

	public UILabel ConditionText;//需求条件文本
	public UILabel SurplusStepText; // 剩余步数

	public UISprite InformationIcon;
	public UILabel InformationText;//情报信息

	public UILabel MagicText;
	private int magicPower = 0;
	public int MagicPower{
		get{ return magicPower;}
		set{ 
			magicPower = value;
			MagicText.text = magicPower.ToString ();

			MGUIPhotoGraph.UpdatePropMagicStatus ();
		}
	}

	public int maxRows = 9;
	public int maxCols = 9;
	public Vector2 firstSquarePosition;
	public Square[] squaresArray; // 固定格子位置数组
	/// <summary>
	///固定格子 类型  block 为一层障碍物  obstacle  为2层障碍物 
	///  一层类型 是 SquareTypes.NONE 没有任何东西 , 正常基础元素 ，有一层障碍 ，有2层障碍
	///  二层类型 	WIREBLOCK 冰冻,SOLIDBLOCK 木箱 ,UNDESTROYABLE 石头 ,THRIVING 水珠
	/// </summary>
	private SquareBlocks[] levelSquaresFile = new SquareBlocks[81];

	/// <summary>
	/// 最后拖动的 与 最后 交换 的Item
	/// </summary>
	public Item lastDraggedItem;
	public Item lastSwitchedItem;
	/// <summary>
	/// 需要销毁的的列表 
	/// </summary>
	public List<Item> destroyAnyway = new List<Item> ();
	/// <summary>
	/// 当前所有可消除的组合Item
	/// </summary>
	List<List<Item>> combinedItems = new List<List<Item>> ();

	/// <summary>
	/// 可自动生长（水珠） ，当本回合没有消除这种类型的Item 时检测是否扩展
	/// </summary>
	public bool thrivingBlockDestroyed;

	/// <summary>
	/// 当为true时 说明 收集的物品还在移动中，等移动结束在进行相关检测  -- 比如游戏完成与失败
	/// </summary>
//	bool ingredientFly;
	public List<GameObject> CollectingLst = new List<GameObject> ();

	public List<ScoreItem> addScoreItemFly = new List<ScoreItem>();

	//自定义字段  ，为了在需要的时候不能拖动  , 下面的dragBlocked 也是这个作用 不过区别是在消除处理结束后  会自动设置dragBlocked 为false
	//这个新加的  不会自动设置， 手动设置开启 关闭 ，有开启 就必须有关闭，不然会卡死 
	private bool dragLock = false;
	public bool MDragLock{
		get{ 
			return dragLock;
		}
		set{ 
			dragLock = value;
		}
	}


	// 拖动 锁定 界面
	private bool dragBlocked;
	public bool DragBlocked {
		get {
			return dragBlocked;
		}
		set {
			dragBlocked = value;
		}
	}
	public int targetBlocks;
	public int TargetBlocks {
		get {
			return targetBlocks;
		}
		set {
			if (targetBlocks < 0)
				targetBlocks = 0;
			targetBlocks = value;

			BlocksNum.text = "[393837]" + TargetMaxBlocks+"  ([CA6363]"+(TargetMaxBlocks - targetBlocks)+"[393837]/"+TargetMaxBlocks+")";
		}
	}
	public int TargetMaxBlocks;

	//判断是否在 掉落中 掉落的时候不能操作
	public bool onlyFalling;

	public Sprite outline1;
	public Sprite outline2;
	public Sprite outline3;

	private GameStatusEnum gameStatus = GameStatusEnum.Map;

	//消除地图所拥有的情报加成效果
//	private List<sg.GS2C_Task_Complete_Res.Level_Attr> LevelAddEffectAttr;
	private Dictionary<int , int> LevelAddEffectAttr = new Dictionary<int, int>();

	//消除地图 心情 影响 效果
	private g_level_info.g_level_infoElement gLevelInfo = null;

	public GameStatusEnum GameStatus{
		get{return gameStatus;}
		set{ 
			gameStatus = value;

			if(gameStatus == GameStatusEnum.PrepareGame){
				//可以显示一些准备的信息界面
			}else if(gameStatus == GameStatusEnum.Playing){
				//处理一些逻辑，比如说关闭准备界面  什么的 

				CurrentScoreText.text = "[403136]"+Globals.Instance.MDataTableManager.GetWordText("6012")+" "+ Score.ToString();

				StartCoroutine (AI.THIS.CheckPossibleCombines ());
			}else if(gameStatus == GameStatusEnum.PreWinAnimations){
				// 游戏过关后 处理 ， 引爆现在有特效，以及消耗剩余步数
				CheckPreWinAnimations ();
			}else if(gameStatus == GameStatusEnum.PreFailed){
				// 游戏失败后 处理 
//				GameField.position = new Vector3(-100f,3f,2f);
				GamePreFieldTipsObj.ShowPreFieldTips ();
			}
			else if(gameStatus == GameStatusEnum.GameOver){

				NetSender.Instance.C2GSTaskQuitReq(mTaskId);
				Debug.LogError ("失败-------------" + Score);
			}else if(gameStatus == GameStatusEnum.Win){
				Debug.LogError ("成功-------------" + Score);
//				GameField.position = new Vector3(-100f,3f,2f);
				NetSender.Instance.RequestTaskCompleteReq (mTaskId,EliminationMgr.Score,EliminationMgr.Instance.stars);
			}else if(gameStatus == GameStatusEnum.Pause){
//				GameField.position = new Vector3(-100f,3f,2f);
			}
		}
	}

	public GamePreFieldTips GamePreFieldTipsObj;
//	public UsePropDialog UsePropDialogObj;

	public UITexture EliminationBg;

	private int mTaskId;
	private PlayerData playerData;
	private Dictionary<int,Dictionary<int, AttrGradeConfig.AttrGradeElement>> mAttrGradeDic = new Dictionary<int, Dictionary<int, AttrGradeConfig.AttrGradeElement>>();
	void Awake(){
		playerData = Globals.Instance.MGameDataManager.MActorData;
		AttrGradeConfig mAttrGradeConfig = Globals.Instance.MDataTableManager.GetConfig<AttrGradeConfig> ();
		Dictionary<int, AttrGradeConfig.AttrGradeElement> mDic = mAttrGradeConfig.GetAttrGradeElementList ();
		foreach(KeyValuePair<int, AttrGradeConfig.AttrGradeElement> v in mDic){
			if (mAttrGradeDic.ContainsKey (v.Value.AttrType)) {
				if(!mAttrGradeDic[v.Value.AttrType].ContainsKey(v.Value.AttrGrade)){
					mAttrGradeDic [v.Value.AttrType].Add (v.Value.AttrGrade ,v.Value);
				}
			} else {
				Dictionary<int, AttrGradeConfig.AttrGradeElement> dic = new Dictionary<int, AttrGradeConfig.AttrGradeElement> ();
				dic.Add (v.Value.AttrGrade , v.Value);
				mAttrGradeDic.Add (v.Value.AttrType ,dic);
			}
		}
	}

	void Start () {

		Instance = this;
		combineManager = new CombineManager ();


		XiaoChuCamera.orthographicSize = 6.55f;
	}

	//正在游戏中，并且可拖动的状态
	public bool IsDragBlock(){
		if(gameStatus == GameStatusEnum.Playing){
			return !DragBlocked && !dragLock;
		}
		return false;
	}


	public void ReStart(){

		foreach (Transform item1 in GameField.transform) {
			Destroy (item1.gameObject);
		}

		MGUIPhotoGraph.LoadPropInfo (mTaskId);

		GameStatus = GameStatusEnum.RegenLevel;

		TaskConfig.TaskObject element = null;
		TaskConfig task = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		bool hasData = task.GetTaskObject(mTaskId, out element);
		if (!hasData)
			return;	
		if(element.Progress_Count <= 0){
			return;	
		}

		if (element.Style_Effect != null && element.Style_Effect != "") {
			LevelAddEffectAttr.Clear ();
			List<int> limitLst = StrParser.ParseDecIntList (element.Style_Effect, 0);
			if (limitLst.Count == 2) {
				LevelAddEffectAttr.Add (limitLst [0], limitLst [1]);	
				InformationText.text = getAttrAddEffectDesc (limitLst [0], limitLst [1]);
			}
		} else {
			InformationText.text = Globals.Instance.MDataTableManager.GetWordText("6026");
		}

		string level = element.Posture_Group;
		Reset ();

		GameField.transform.position = Vector3.zero;
		firstSquarePosition = GameField.transform.position;

		TargetBlocks = 0;

		squaresArray = new Square[maxCols * maxRows];
		LoadDataFromLocal (level);
		for (int row = 0; row < maxRows; row++) {
			for (int col = 0; col < maxCols; col++) {
				if (levelSquaresFile [row * maxCols + col].block == SquareTypes.BLOCK)
					TargetBlocks++;
				else if (levelSquaresFile [row * maxCols + col].block == SquareTypes.DOUBLEBLOCK)
					TargetBlocks += 2;
			}
		}
		TargetMaxBlocks = TargetBlocks;

		g_level_info item = Globals.Instance.MDataTableManager.GetConfig<g_level_info>();
		gLevelInfo = item.Getg_level_infoElementList()[level];

		if(gLevelInfo.BaseElement != null && gLevelInfo.BaseElement != ""){

			colorLimitLst = StrParser.ParseDecIntList (gLevelInfo.BaseElement,0);
		}
		if(gLevelInfo.ElementLimit != null && gLevelInfo.ElementLimit != ""){
			string[] elementStr = gLevelInfo.ElementLimit.Split(new char[]{'|'});
			for(int i = 0 ; i < elementStr.Length ; i++){
				List<int> limitLst = StrParser.ParseDecIntList (elementStr[i],0);
				if(limitLst.Count == 2 && limitLst[0] > 0 && limitLst[1] > 0){
					colorLimitNumDic.Add (limitLst[0],limitLst[1]);
				}
			}
		}

		if(gLevelInfo.InitElement != null && gLevelInfo.InitElement != ""){
			string[] elementStr = gLevelInfo.InitElement.Split(new char[]{'|'});
			for (int i = 0; i < elementStr.Length; i++) {
				string[] slotStr = elementStr[i].Split(new char[]{','});
				if(slotStr.Length == 3){
					int colSlot = int.Parse(slotStr [0]);
					int rowSlot = int.Parse(slotStr [1]);
					int colorSlot = int.Parse(slotStr [2]);
					AddGenerateSpecialItem (colSlot,rowSlot,colorSlot);	
				}
			}
		}

		EliminationBg.mainTexture = Resources.Load("UIAtlas/" + gLevelInfo.LevelBg,typeof(Texture2D)) as Texture2D;


		InitTargets ();

		Debug.Log ("本次地图 有 block  = " + TargetBlocks);
		InitLevel ();


		CurrentScoreText.text = element.Dress_Part_Effect;
		EliminationMgr.Instance.GameFieldAnimationEndStartGame ();
	}

	public void Init(int taskId){

		mTaskId = taskId;

		TaskConfig.TaskObject element = null;
		TaskConfig task = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		bool hasData = task.GetTaskObject(taskId, out element);
		if (!hasData)
			return;	
		if(element.Progress_Count <= 0){
			return;	
		}

		if (element.Style_Effect != null && element.Style_Effect != "") {
			LevelAddEffectAttr.Clear ();
			List<int> limitLst = StrParser.ParseDecIntList (element.Style_Effect, 0);
			if (limitLst.Count == 2) {
				LevelAddEffectAttr.Add (limitLst [0], limitLst [1]);	
				InformationIcon.gameObject.SetActive (true);
				InformationText.transform.localPosition = new Vector3 (40f,0f,0f);
				InformationText.text = getAttrAddEffectDesc (limitLst [0], limitLst [1]);
			}
		} else {
			InformationIcon.gameObject.SetActive (false);
			InformationText.transform.localPosition = Vector3.zero;
			InformationText.text = Globals.Instance.MDataTableManager.GetWordText("6026");
		}


		PrepareGame (element.Posture_Group);

		CurrentScoreText.text = element.Dress_Part_Effect;
	}

	public void PrepareGame (string level) {

		GameStatus = GameStatusEnum.PrepareGame;

		Reset ();

		GameField.transform.position = Vector3.zero;
		firstSquarePosition = GameField.transform.position;

		TargetBlocks = 0;

		squaresArray = new Square[maxCols * maxRows];
		LoadDataFromLocal (level);
		for (int row = 0; row < maxRows; row++) {
			for (int col = 0; col < maxCols; col++) {
				if (levelSquaresFile [row * maxCols + col].block == SquareTypes.BLOCK)
					TargetBlocks++;
				else if (levelSquaresFile [row * maxCols + col].block == SquareTypes.DOUBLEBLOCK)
					TargetBlocks += 2;
			}
		}
		TargetMaxBlocks = TargetBlocks;

		g_level_info item = Globals.Instance.MDataTableManager.GetConfig<g_level_info>();
		gLevelInfo = item.Getg_level_infoElementList()[level];

		if(gLevelInfo.BaseElement != null && gLevelInfo.BaseElement != ""){

			colorLimitLst = StrParser.ParseDecIntList (gLevelInfo.BaseElement,0);
		}
		if(gLevelInfo.ElementLimit != null && gLevelInfo.ElementLimit != ""){
			string[] elementStr = gLevelInfo.ElementLimit.Split(new char[]{'|'});
			for(int i = 0 ; i < elementStr.Length ; i++){
				List<int> limitLst = StrParser.ParseDecIntList (elementStr[i],0);
				if(limitLst.Count == 2 && limitLst[0] > 0 && limitLst[1] > 0){
					colorLimitNumDic.Add (limitLst[0],limitLst[1]);
				}
			}
		}

		if(gLevelInfo.InitElement != null && gLevelInfo.InitElement != ""){
			string[] elementStr = gLevelInfo.InitElement.Split(new char[]{'|'});
			for (int i = 0; i < elementStr.Length; i++) {
				string[] slotStr = elementStr[i].Split(new char[]{','});
				if(slotStr.Length == 3){
					int colSlot = int.Parse(slotStr [0]);
					int rowSlot = int.Parse(slotStr [1]);
					int colorSlot = int.Parse(slotStr [2]);
					AddGenerateSpecialItem (colSlot,rowSlot,colorSlot);	
				}
			}
		}

		EliminationBg.mainTexture = Resources.Load("UIAtlas/" + gLevelInfo.LevelBg,typeof(Texture2D)) as Texture2D;


		InitTargets ();

		Debug.Log ("本次地图 有 block  = " + TargetBlocks);
		InitLevel ();
	}

	void InitTargets () {
		BlocksObject.gameObject.SetActive (false);
		ingrObject.SetActive (false);
		ConditionText.gameObject.SetActive (false);


		if (ingrCountTarget [0] > 0 || ingrCountTarget [1] > 0) {

			ingrObject.SetActive (true);
			if (target == Target.INGREDIENT) {
				if (ingrCountTarget [0] > 0 && ingrCountTarget [1] > 0 && ingrTarget [0] == ingrTarget [1]) {
					ingrCountTarget [0] += ingrCountTarget [1];
					ingrCountTarget [1] = 0;
					ingrTarget [1] = Ingredients.None;
				}
				Ingr1.spriteName = "item" + (int)ingrTarget [0];
				Ingr2.spriteName = "item"+(int)ingrTarget [1];
			} else if (target == Target.COLLECT) {
				if (ingrCountTarget [0] > 0 && ingrCountTarget [1] > 0 && collectItems [0] == collectItems [1]) {
					ingrCountTarget [0] += ingrCountTarget [1];
					ingrCountTarget [1] = 0;
					collectItems [1] = CollectItems.None;
				}

				Ingr1.spriteName = "item" + (int)collectItems [0];
				Ingr2.spriteName = "item"+(int)collectItems [1];
			}
			Ingr1.transform.localPosition = new Vector3 (Ingr1.transform.localPosition.x ,40f,Ingr1.transform.localPosition.z);
			Ingr2.transform.localPosition = new Vector3 (Ingr2.transform.localPosition.x ,-40f,Ingr2.transform.localPosition.z);
			if (ingrCountTarget [0] == 0 && ingrCountTarget [1] > 0) {
				Ingr1.gameObject.SetActive (false);
				Ingr2.transform.localPosition = new Vector3 (Ingr2.transform.localPosition.x ,0f,Ingr2.transform.localPosition.z);
			} else if (ingrCountTarget [0] > 0 && ingrCountTarget [1] == 0) {
				Ingr2.gameObject.SetActive (false);
				Ingr1.transform.localPosition = new Vector3 (Ingr1.transform.localPosition.x ,0f,Ingr1.transform.localPosition.z);
			}

			Ingr1Text.text = "[393837]" + ingrMaxCountTarget[0]+"  ([CA6363]"+(ingrMaxCountTarget[0] - ingrCountTarget[0])+"[393837]/"+ingrMaxCountTarget[0]+")";
			Ingr2Text.text = "[393837]" + ingrMaxCountTarget[1]+"  ([CA6363]"+(ingrMaxCountTarget[1] - ingrCountTarget[1])+"[393837]/"+ingrMaxCountTarget[1]+")";
		}

		if (targetBlocks > 0) {
			BlocksObject.gameObject.SetActive (true);
			if(EliminationMgr.Instance.target == Target.BLOCKS){
				BlocksObject.spriteName = "block";
				TargetBlocks = targetBlocks;
			}
		} else if (ingrCountTarget [0] == 0 && ingrCountTarget [1] == 0) {
			ConditionText.gameObject.SetActive (true);
			ConditionText.text = star1.ToString ();	
		}
	}


	void Reset(){
		Score = 0;
		stars = 0;
//		Moods = 0;
		MagicPower = 0;

		star1Anim.SetActive (false);
		star2Anim.SetActive (false);
		star3Anim.SetActive (false);

		collectItems [0] = CollectItems.None;
		collectItems [1] = CollectItems.None;

		ingrTarget [0] = Ingredients.None;
		ingrTarget [1] = Ingredients.None;

		ingrCountTarget [0] = 0;
		ingrCountTarget [1] = 0;

		ingrMaxCountTarget [0] = 0;
		ingrMaxCountTarget [1] = 0;

		TargetBlocks = 0;

		ScoreSlider.value = 0;
		CurrentScoreText.text = "[403136]"+ Globals.Instance.MDataTableManager.GetWordText("6012")+" "+ "0";

		colorLimitLst.Clear ();
		colorLimitNumDic.Clear ();
		foreach(ScoreItem v in addScoreItemFly){
			if(v != null)
				Destroy (v.gameObject);
		}
		addScoreItemFly.Clear ();

		foreach(GameObject v in CollectingLst){
			if(v != null)
				Destroy (v);
		}
		CollectingLst.Clear ();
		GenerateSpecialItemDic.Clear ();
		SpecialPropEffect.Clear ();
		SpecialPropScoreEffect.Clear ();
	}

	void InitLevel () {

		GenerateLevel ();
		GenerateOutline ();  // 描边的 先等等写
		ReGenLevel ();
		if (limitType == LIMIT.TIME) {
			StopCoroutine (TimeTick ());
			StartCoroutine (TimeTick ());
		}

		GameField.gameObject.SetActive (true);
	}
	IEnumerator TimeTick () {
		while (true) {
			if (GameStatus == GameStatusEnum.Playing) {
				if (EliminationMgr.Instance.limitType == LIMIT.TIME) {
					EliminationMgr.Instance.Limit--;
					if (IsAllItemsFallDown ())
						CheckWinLose ();
				}
			}
			if (GameStatus == GameStatusEnum.PrepareGame)
				yield break;
			yield return new WaitForSeconds (1);
		}
	}

	bool IsAllItemsFallDown () {
//		if (gameStatus == GameState.PreWinAnimations)
		if (GameStatus == GameStatusEnum.PrepareGame)
			return true;
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			Item itemComponent = item.GetComponent<Item> ();
			if (itemComponent == null) {
				return false;
			}
			if (itemComponent.falling)
				return false;
		}
		return true;
	}
	//检查游戏结果
	public void CheckWinLose () {

		if(IsAddScoreFly() || !IsAllItemsFallDown () || IsCollectingFly() || IsIngredientFalling ()){
			return;
		}
		if (Limit <= 0) {
			bool lose = false;
			Limit = 0;

			if (EliminationMgr.Instance.target == Target.BLOCKS && EliminationMgr.Instance.TargetBlocks > 0) {
				lose = true;
			} else if (EliminationMgr.Instance.target == Target.COLLECT && (EliminationMgr.Instance.ingrCountTarget [0] > 0 || EliminationMgr.Instance.ingrCountTarget [1] > 0)) {
				lose = true;
			} else if (EliminationMgr.Instance.target == Target.INGREDIENT && (EliminationMgr.Instance.ingrCountTarget [0] > 0 || EliminationMgr.Instance.ingrCountTarget [1] > 0)) {
				lose = true;
			}
			if (EliminationMgr.Score < EliminationMgr.Instance.star1) {
				lose = true;

			}
			if (lose) {
				//失败处理
				GameStatus = GameStatusEnum.PreFailed;
			}
			else if (EliminationMgr.Score >= EliminationMgr.Instance.star1 && EliminationMgr.Instance.target == Target.SCORE) {
				
				GameStatus = GameStatusEnum.PreWinAnimations;
			} else if (EliminationMgr.Score >= EliminationMgr.Instance.star1 && EliminationMgr.Instance.target == Target.BLOCKS && EliminationMgr.Instance.TargetBlocks <= 0) {
				
				GameStatus = GameStatusEnum.PreWinAnimations;
			} else if (EliminationMgr.Score >= EliminationMgr.Instance.star1 && EliminationMgr.Instance.target == Target.COLLECT && (EliminationMgr.Instance.ingrCountTarget [0] <= 0 && EliminationMgr.Instance.ingrCountTarget [1] <= 0)) {
				
				GameStatus = GameStatusEnum.PreWinAnimations;
			} else if (EliminationMgr.Score >= EliminationMgr.Instance.star1 && EliminationMgr.Instance.target == Target.INGREDIENT && (EliminationMgr.Instance.ingrCountTarget [0] <= 0 && EliminationMgr.Instance.ingrCountTarget [1] <= 0)) {
				GameStatus = GameStatusEnum.PreWinAnimations;
			}
		} else {
			bool win = false;
			if (EliminationMgr.Instance.target == Target.BLOCKS && EliminationMgr.Instance.TargetBlocks <= 0) {
				win = true;
			} else if (EliminationMgr.Instance.target == Target.COLLECT && (EliminationMgr.Instance.ingrCountTarget [0] <= 0 && EliminationMgr.Instance.ingrCountTarget [1] <= 0)) {
				win = true;
			} else if (EliminationMgr.Instance.target == Target.INGREDIENT && (EliminationMgr.Instance.ingrCountTarget [0] <= 0 && EliminationMgr.Instance.ingrCountTarget [1] <= 0)) {
				win = true;
			}else if(EliminationMgr.Instance.target == Target.SCORE){
				if (EliminationMgr.Score >= EliminationMgr.Instance.star1) {
					win = true;
				}
			}
			if (EliminationMgr.Score < EliminationMgr.Instance.star1) {
				win = false;

			}
			if (win) {
				GameStatus = GameStatusEnum.PreWinAnimations;
			}
		}
	}


	public void NoMatches () {
		StartCoroutine (NoMatchesCor ());
	}

	IEnumerator NoMatchesCor () {
		if (gameStatus == GameStatusEnum.Playing) {
//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.noMatch);

//			GameObject.Find ("Canvas").transform.Find ("NoMoreMatches").gameObject.SetActive (true);
			gameStatus = GameStatusEnum.RegenLevel;
			yield return new WaitForSeconds (0.2f);
			ReGenLevel ();
		}
	}

	public void ReGenLevel () {

		dragLock = true;
		DragBlocked = true;
		if (gameStatus != GameStatusEnum.Playing && gameStatus != GameStatusEnum.RegenLevel)
			DestroyItems ();
		else if (gameStatus == GameStatusEnum.RegenLevel)
			DestroyItems (true);

		StartCoroutine (RegenMatches ());

	}
	public void DestroyItems (bool withoutEffects = false) {

		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if (item != null) {
				if (item.GetComponent<Item> ().currentType != ItemsTypes.INGREDIENT && item.GetComponent<Item> ().currentType == ItemsTypes.NONE) {
					if (!withoutEffects)
						item.GetComponent<Item> ().DestroyItem ();
					else
						item.GetComponent<Item> ().SmoothDestroy ();
				}
			}
		}
	}
	/// <summary>
	///  这个方法  当使用默认参数时 ，是重新生成新的 ，在开始生成新地图时，也会把能直接消除的处理一下
	///  另一种情况 是在消除之后 生成新的Item ,  调用这里，会检测新生成Item 如果又是可以直接消除 就处理一下，
	///  不管是开始生成 还是消除后生成的   都需要保证刚生成新的Item之间不能有直接消除的情况 
	/// </summary>
	/// <returns>The matches.</returns>
	/// <param name="onlyFalling">If set to <c>true</c> only falling.</param>
	IEnumerator RegenMatches (bool onlyFalling = false) {
		if (gameStatus == GameStatusEnum.RegenLevel) {
			yield return new WaitForSeconds (0.5f);
		}
		if (!onlyFalling)
			GenerateNewItems (false);
		else
			EliminationMgr.Instance.onlyFalling = true;

		yield return new WaitForFixedUpdate ();
		// onlyFalling 为false 时 ，处理的整个地图中所有课消除的地方， 为true 时 ，处理的时 刚生成的
		List<List<Item>> combs = GetMatches ();
		do {
			foreach (List<Item> comb in combs) {
				int colorOffset = 0;
				foreach (Item item in comb) {
					item.GenColor (item.color + colorOffset);
					colorOffset++;
				}
			}
			combs = GetMatches ();
		} while (combs.Count > 0);
		yield return new WaitForFixedUpdate ();
//		SetPreBoosts ();
		if (!onlyFalling){
			dragLock = false;
			DragBlocked = false;
		}
			
		EliminationMgr.Instance.onlyFalling = false;
		if (gameStatus == GameStatusEnum.RegenLevel)
			gameStatus = GameStatusEnum.Playing;
	}
	/// <summary>
	/// 创建新的 基础元素 
	/// </summary>
	/// <param name="falling">If set to <c>true</c> falling.</param>
	void GenerateNewItems (bool falling = true) {
		for (int col = 0; col < maxCols; col++) {
			for (int row = maxRows - 1; row >= 0; row--) {
				if (GetSquare (col, row) != null) {
					if (!GetSquare (col, row).IsNone () && GetSquare (col, row).CanGoInto () && GetSquare (col, row).item == null) {
						if ((GetSquare (col, row).item == null && !GetSquare (col, row).IsHaveSolidAbove ()) || !falling) {
							GetSquare (col, row).GenItem (falling);
						}
					}
				}
			}
		}
	}
	/// <summary>
	///  获取 对应位置的格子 ，Mathf.Clamp 当小于最小值时 取最小值，大于最大值时 取最大值， 范围内返回自己
	/// 主要是为了防止数据问题
	/// </summary>
	/// <returns>The square.</returns>
	/// <param name="col">Col.</param>
	/// <param name="row">Row.</param>
	/// <param name="safe">If set to <c>true</c> safe.</param>
	public Square GetSquare (int col, int row, bool safe = false) {
		if (!safe) {
			if (row >= maxRows || col >= maxCols)
				return null;
			return squaresArray [row * maxCols + col];
		} else {
			row = Mathf.Clamp (row, 0, maxRows - 1);
			col = Mathf.Clamp (col, 0, maxCols - 1);
			return squaresArray [row * maxCols + col];
		}
	}

	/// <summary>
	///  检查地图上所有位置 ，返回所有满足条件的消除
	/// </summary>
	private List<List<Item>> newCombines;
	public Hashtable countedSquares;
	public List<List<Item>> GetMatches (FindSeparating separating = FindSeparating.NONE, int matches = 3) {
		newCombines = new List<List<Item>> ();
		countedSquares = new Hashtable ();
		countedSquares.Clear ();
		for (int col = 0; col < maxCols; col++) {
			for (int row = 0; row < maxRows; row++) {
				if (GetSquare (col, row) != null) {
					if (!countedSquares.ContainsValue (GetSquare (col, row).item)) {
						List<Item> newCombine = GetSquare (col, row).FindMatchesAround (separating, matches, countedSquares);
						if (newCombine.Count >= matches)
							newCombines.Add (newCombine);
					}
				}
			}
		}
		return newCombines;
	}

	/// <summary>
	/// 加载 基础的格子 显示出来 
	/// </summary>
	private void GenerateLevel () {
		bool chessColor = false;
		Vector3 fieldPos = new Vector3 (-maxCols / 2.75f, maxRows / 2.75f, -10);
		for (int row = 0; row < maxRows; row++) {
			if (maxCols % 2 == 0)
				chessColor = !chessColor;
			for (int col = 0; col < maxCols; col++) {
				CreateSquare (col, row, chessColor);
				chessColor = !chessColor;
			}

		}
		AnimateField (fieldPos);

	}
	void CreateSquare (int col, int row, bool chessColor = false) {
		GameObject square = null;
		square = Instantiate (squarePrefab, firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
		if (chessColor) {
			square.GetComponent<SpriteRenderer> ().sprite = squareSprite1;
		}
		square.transform.SetParent (GameField);
		square.transform.localPosition = firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight);
		squaresArray [row * maxCols + col] = square.GetComponent<Square> ();
		square.GetComponent<Square> ().row = row;
		square.GetComponent<Square> ().col = col;
		square.GetComponent<Square> ().type = SquareTypes.EMPTY;
		if (levelSquaresFile [row * maxCols + col].block == SquareTypes.EMPTY) {
			CreateObstacles (col, row, square, SquareTypes.NONE);
		} else if (levelSquaresFile [row * maxCols + col].block == SquareTypes.NONE) {
			square.GetComponent<SpriteRenderer> ().enabled = false;
			square.GetComponent<Square> ().type = SquareTypes.NONE;

		} else if (levelSquaresFile [row * maxCols + col].block == SquareTypes.BLOCK) {
			GameObject block = Instantiate (blockPrefab, firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
			block.transform.SetParent (square.transform);
			block.transform.localPosition = new Vector3 (0, 0, -0.01f);
			square.GetComponent<Square> ().block.Add (block);
			square.GetComponent<Square> ().type = SquareTypes.BLOCK;
			block.GetComponent<SpriteRenderer> ().sortingOrder = 1;
			CreateObstacles (col, row, square, SquareTypes.NONE);
		} else if (levelSquaresFile [row * maxCols + col].block == SquareTypes.DOUBLEBLOCK) {
			GameObject block = Instantiate (blockPrefab, firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
			block.transform.SetParent (square.transform);
			block.transform.localPosition = new Vector3 (0, 0, -0.01f);
			square.GetComponent<Square> ().block.Add (block);
			square.GetComponent<Square> ().type = SquareTypes.BLOCK;
			block.GetComponent<SpriteRenderer> ().sortingOrder = 1;

			block = Instantiate (blockPrefab, firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
			block.transform.SetParent (square.transform);
			block.transform.localPosition = new Vector3 (0, 0, -0.01f);
			square.GetComponent<Square> ().block.Add (block);
			square.GetComponent<Square> ().type = SquareTypes.BLOCK;
			block.GetComponent<SpriteRenderer> ().sprite = doubleBlock;
			block.GetComponent<SpriteRenderer> ().sortingOrder = 2;

			CreateObstacles (col, row, square, SquareTypes.NONE);
		}

	}
	void CreateObstacles (int col, int row, GameObject square, SquareTypes type) {
		if ((levelSquaresFile [row * maxCols + col].obstacle == SquareTypes.WIREBLOCK && type == SquareTypes.NONE) || type == SquareTypes.WIREBLOCK) {
			GameObject block = Instantiate (wireBlockPrefab, firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
			block.transform.SetParent (square.transform);
			block.transform.localPosition = new Vector3 (0, 0, -0.5f);
			square.GetComponent<Square> ().block.Add (block);
			square.GetComponent<Square> ().type = SquareTypes.WIREBLOCK;
			block.GetComponent<SpriteRenderer> ().sortingOrder = 3;
		} else if ((levelSquaresFile [row * maxCols + col].obstacle == SquareTypes.SOLIDBLOCK && type == SquareTypes.NONE) || type == SquareTypes.SOLIDBLOCK) {
			GameObject block = Instantiate (solidBlockPrefab, firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
			block.transform.SetParent (square.transform);
			block.transform.localPosition = new Vector3 (0, 0, -0.5f);
			square.GetComponent<Square> ().block.Add (block);
			block.GetComponent<SpriteRenderer> ().sortingOrder = 3;
			square.GetComponent<Square> ().type = SquareTypes.SOLIDBLOCK;
		} else if ((levelSquaresFile [row * maxCols + col].obstacle == SquareTypes.UNDESTROYABLE && type == SquareTypes.NONE) || type == SquareTypes.UNDESTROYABLE) {
			GameObject block = Instantiate (undesroyableBlockPrefab, firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
			block.transform.SetParent (square.transform);
			block.transform.localPosition = new Vector3 (0, 0, -0.5f);
			square.GetComponent<Square> ().block.Add (block);
			square.GetComponent<Square> ().type = SquareTypes.UNDESTROYABLE;
		} else if ((levelSquaresFile [row * maxCols + col].obstacle == SquareTypes.THRIVING && type == SquareTypes.NONE) || type == SquareTypes.THRIVING) {
			GameObject block = Instantiate (thrivingBlockPrefab, firstSquarePosition + new Vector2 (col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
			block.transform.SetParent (square.transform);
			block.transform.localPosition = new Vector3 (0, 0, -0.5f);
			block.GetComponent<SpriteRenderer> ().sortingOrder = 3;
			if (square.GetComponent<Square> ().item != null)
				Destroy (square.GetComponent<Square> ().item.gameObject);
			square.GetComponent<Square> ().block.Add (block);
			square.GetComponent<Square> ().type = SquareTypes.THRIVING;
		}
	}

	void GenerateOutline () {
		int row = 0;
		int col = 0;
		for (row = 0; row < maxRows; row++) { //down
			SetOutline (col, row, 0);
		}
		row = maxRows - 1;
		for (col = 0; col < maxCols; col++) { //right
			SetOutline (col, row, 90);
		}
		col = maxCols - 1;
		for (row = maxRows - 1; row >= 0; row--) { //up
			SetOutline (col, row, 180);
		}
		row = 0;
		for (col = maxCols - 1; col >= 0; col--) { //left
			SetOutline (col, row, 270);
		}
		col = 0;
		for (row = 1; row < maxRows - 1; row++) {
			for (col = 1; col < maxCols - 1; col++) {
				//  if (GetSquare(col, row).type == SquareTypes.NONE)
				SetOutline (col, row, 0);
			}
		}
	}


	void SetOutline (int col, int row, float zRot) {
		Square square = GetSquare (col, row, true);
		if (square.type != SquareTypes.NONE) {
			if (row == 0 || col == 0 || col == maxCols - 1 || row == maxRows - 1) {
				GameObject outline = CreateOutline (square);
				SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
				outline.transform.localRotation = Quaternion.Euler (0, 0, zRot);
				if (zRot == 0)
					outline.transform.localPosition = Vector3.zero + Vector3.left * 0.425f;
				if (zRot == 90)
					outline.transform.localPosition = Vector3.zero + Vector3.down * 0.425f;
				if (zRot == 180)
					outline.transform.localPosition = Vector3.zero + Vector3.right * 0.425f;
				if (zRot == 270)
					outline.transform.localPosition = Vector3.zero + Vector3.up * 0.425f;
				if (row == 0 && col == 0) {   //top left
					spr.sprite = outline3;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 180);
					outline.transform.localPosition = Vector3.zero + Vector3.left * 0.015f + Vector3.up * 0.015f;
				}
				if (row == 0 && col == maxCols - 1) {   //top right
					spr.sprite = outline3;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 90);
					outline.transform.localPosition = Vector3.zero + Vector3.right * 0.015f + Vector3.up * 0.015f;
				}
				if (row == maxRows - 1 && col == 0) {   //bottom left
					spr.sprite = outline3;
					outline.transform.localRotation = Quaternion.Euler (0, 0, -90);
					outline.transform.localPosition = Vector3.zero + Vector3.left * 0.015f + Vector3.down * 0.015f;
				}
				if (row == maxRows - 1 && col == maxCols - 1) {   //bottom right
					spr.sprite = outline3;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 0);
					outline.transform.localPosition = Vector3.zero + Vector3.right * 0.015f + Vector3.down * 0.015f;
				}
			} else {
				//top left
				if (GetSquare (col - 1, row - 1, true).type == SquareTypes.NONE && GetSquare (col, row - 1, true).type == SquareTypes.NONE && GetSquare (col - 1, row, true).type == SquareTypes.NONE) {
					GameObject outline = CreateOutline (square);
					SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
					spr.sprite = outline3;
					outline.transform.localPosition = Vector3.zero + Vector3.left * 0.015f + Vector3.up * 0.015f;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 180);
				}
				//top right
				if (GetSquare (col + 1, row - 1, true).type == SquareTypes.NONE && GetSquare (col, row - 1, true).type == SquareTypes.NONE && GetSquare (col + 1, row, true).type == SquareTypes.NONE) {
					GameObject outline = CreateOutline (square);
					SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
					spr.sprite = outline3;
					outline.transform.localPosition = Vector3.zero + Vector3.right * 0.015f + Vector3.up * 0.015f;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 90);
				}
				//bottom left
				if (GetSquare (col - 1, row + 1, true).type == SquareTypes.NONE && GetSquare (col, row + 1, true).type == SquareTypes.NONE && GetSquare (col - 1, row, true).type == SquareTypes.NONE) {
					GameObject outline = CreateOutline (square);
					SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
					spr.sprite = outline3;
					outline.transform.localPosition = Vector3.zero + Vector3.left * 0.015f + Vector3.down * 0.015f;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 270);
				}
				//bottom right
				if (GetSquare (col + 1, row + 1, true).type == SquareTypes.NONE && GetSquare (col, row + 1, true).type == SquareTypes.NONE && GetSquare (col + 1, row, true).type == SquareTypes.NONE) {
					GameObject outline = CreateOutline (square);
					SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
					spr.sprite = outline3;
					outline.transform.localPosition = Vector3.zero + Vector3.right * 0.015f + Vector3.down * 0.015f;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 0);
				}


			}
		} else {
			bool corner = false;
			if (GetSquare (col - 1, row, true).type != SquareTypes.NONE && GetSquare (col, row - 1, true).type != SquareTypes.NONE) {
				GameObject outline = CreateOutline (square);
				SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
				spr.sprite = outline2;
				outline.transform.localPosition = Vector3.zero;
				outline.transform.localRotation = Quaternion.Euler (0, 0, 0);
				corner = true;
			}
			if (GetSquare (col + 1, row, true).type != SquareTypes.NONE && GetSquare (col, row + 1, true).type != SquareTypes.NONE) {
				GameObject outline = CreateOutline (square);
				SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
				spr.sprite = outline2;
				outline.transform.localPosition = Vector3.zero;
				outline.transform.localRotation = Quaternion.Euler (0, 0, 180);
				corner = true;
			}
			if (GetSquare (col + 1, row, true).type != SquareTypes.NONE && GetSquare (col, row - 1, true).type != SquareTypes.NONE) {
				GameObject outline = CreateOutline (square);
				SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
				spr.sprite = outline2;
				outline.transform.localPosition = Vector3.zero;
				outline.transform.localRotation = Quaternion.Euler (0, 0, 270);
				corner = true;
			}
			if (GetSquare (col - 1, row, true).type != SquareTypes.NONE && GetSquare (col, row + 1, true).type != SquareTypes.NONE) {
				GameObject outline = CreateOutline (square);
				SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
				spr.sprite = outline2;
				outline.transform.localPosition = Vector3.zero;
				outline.transform.localRotation = Quaternion.Euler (0, 0, 90);
				corner = true;
			}


			if (!corner) {
				if (GetSquare (col, row - 1, true).type != SquareTypes.NONE) {
					GameObject outline = CreateOutline (square);
					SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
					outline.transform.localPosition = Vector3.zero + Vector3.up * 0.395f;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 90);
				}
				if (GetSquare (col, row + 1, true).type != SquareTypes.NONE) {
					GameObject outline = CreateOutline (square);
					SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
					outline.transform.localPosition = Vector3.zero + Vector3.down * 0.395f;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 90);
				}
				if (GetSquare (col - 1, row, true).type != SquareTypes.NONE) {
					GameObject outline = CreateOutline (square);
					SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
					outline.transform.localPosition = Vector3.zero + Vector3.left * 0.395f;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 0);
				}
				if (GetSquare (col + 1, row, true).type != SquareTypes.NONE) {
					GameObject outline = CreateOutline (square);
					SpriteRenderer spr = outline.GetComponent<SpriteRenderer> ();
					outline.transform.localPosition = Vector3.zero + Vector3.right * 0.395f;
					outline.transform.localRotation = Quaternion.Euler (0, 0, 0);
				}
			}
		}
	}

	GameObject CreateOutline (Square square) {
		GameObject outline = new GameObject ();
		outline.name = "outline";
		outline.layer = 20;
		outline.transform.SetParent (square.transform);
		outline.transform.localPosition = Vector3.zero;
		SpriteRenderer spr = outline.AddComponent<SpriteRenderer> ();
		spr.sprite = outline1;
		spr.sortingOrder = 1;
		return outline;
	}

	void AnimateField (Vector3 pos) {
		GameFieldFit ();
		GameField.transform.position = new Vector3 (-2000f, maxRows / 2.75f, 2f);
	}

	private void GameFieldFit(){
		float aspect = (float)Screen.height / (float)Screen.width;
		XiaoChuCamera.orthographicSize = 5.3f;
		aspect = (float)Math.Round (aspect, 2);
		if (aspect >= 1.77f) {
			XiaoChuCamera.orthographicSize = 7f;    //16:9
		}else if (aspect >= 1.66f && aspect < 1.77f){
			XiaoChuCamera.orthographicSize = 6.6f;    
		}else if (aspect >= 1.6f && aspect < 1.66f){
			XiaoChuCamera.orthographicSize = 6.25f;      //16:10
		}else if (aspect >= 1.5f && aspect < 1.6f){
			XiaoChuCamera.orthographicSize = 5.9f;    //3:2
		}else if (aspect >= 1.33f && aspect < 1.5f){
			XiaoChuCamera.orthographicSize = 5.25f;                  //4:3
		}else if (aspect < 1.33f){
			XiaoChuCamera.orthographicSize = 4.9f;        //4:3
		}
	}

	public void GameFieldAnimationEndStartGame(){

		GameFieldFit ();
		GameField.transform.position = new Vector3 (-maxCols / 2.75f, maxRows / 2.75f, 2f);
		GameStatus = GameStatusEnum.Playing;

		Debug.Log ("GameFieldAnimationEndStartGame ---- ");
	}

	void EndAnimGamField()
	{
		Debug.LogError ("现在可以开始游戏了 ！！！！ " + GameStatus);
	}

	/// <summary>
	///  获取 周围 3行 3列 的Item
	/// </summary>
	/// <returns>The items around.</returns>
	/// <param name="square">Square.</param>
	public List<Item> GetItemsAround (Square square) {
		int col = square.col;
		int row = square.row;
		List<Item> itemsList = new List<Item> ();
		for (int r = row - 1; r <= row + 1; r++) {
			if(r >= maxRows)
				continue;
			for (int c = col - 1; c <= col + 1; c++) {
				if(c >= maxCols)
					continue;
				itemsList.Add (GetSquare (c, r, true).item);
			}
		}
		return itemsList;
	}

	public void LoadDataFromLocal (string currentLevel) {
		levelLoaded = false;

		TextAsset mapText = Resources.Load ("Levels/" + currentLevel) as TextAsset;
		if (mapText == null) {
			mapText = Resources.Load ("Levels/" + currentLevel) as TextAsset;
		}
		ProcessGameDataFromString (mapText.text);
	}

	void ProcessGameDataFromString (string mapText) {
		string[] lines = mapText.Split (new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

		int mapLine = 0;
		foreach (string line in lines) {
			//check if line is game mode line
			if (line.StartsWith ("MODE")) {
				//Replace GM to get mode number, 
				string modeString = line.Replace ("MODE", string.Empty).Trim ();
				//then parse it to interger
				target = (Target)int.Parse (modeString);
				//Assign game mode
			} else if (line.StartsWith ("SIZE ")) {
				string blocksString = line.Replace ("SIZE", string.Empty).Trim ();
				string[] sizes = blocksString.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				maxCols = int.Parse (sizes [0]);
				maxRows = int.Parse (sizes [1]);
				squaresArray = new Square[maxCols * maxRows];

				levelSquaresFile = new SquareBlocks[maxRows * maxCols];
				for (int i = 0; i < levelSquaresFile.Length; i++) {

					SquareBlocks sqBlocks = new SquareBlocks ();
					sqBlocks.block = SquareTypes.EMPTY;
					sqBlocks.obstacle = SquareTypes.NONE;

					levelSquaresFile [i] = sqBlocks;
				}

			} else if (line.StartsWith ("LIMIT")) {
				string blocksString = line.Replace ("LIMIT", string.Empty).Trim ();
				string[] sizes = blocksString.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				limitType = (LIMIT)int.Parse (sizes [0]);
				Limit = int.Parse (sizes [1]);
			} else if (line.StartsWith ("COLOR LIMIT ")) {
				string blocksString = line.Replace ("COLOR LIMIT", string.Empty).Trim ();
				colorLimit = int.Parse (blocksString);
			}

			//check third line to get missions
			else if (line.StartsWith ("STARS")) {
				string blocksString = line.Replace ("STARS", string.Empty).Trim ();
				string[] blocksNumbers = blocksString.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				star1 = int.Parse (blocksNumbers [0]);
				star2 = int.Parse (blocksNumbers [1]);
				star3 = int.Parse (blocksNumbers [2]);

				float baseX = 786f / EliminationMgr.Instance.star3;
				float baseX2 = 786f / 2f;
				float star1X = baseX * EliminationMgr.Instance.star1 - baseX2;
				Star1Obj.transform.localPosition = new Vector3 (star1X , Star1Obj.transform.localPosition.y,Star1Obj.transform.localPosition.z);
				float star2X = baseX * EliminationMgr.Instance.star2 - baseX2;
				Star2Obj.transform.localPosition = new Vector3 (star2X , Star2Obj.transform.localPosition.y,Star2Obj.transform.localPosition.z);
				float star3X = baseX * EliminationMgr.Instance.star3 - baseX2;
				Star3Obj.transform.localPosition = new Vector3 (star3X , Star3Obj.transform.localPosition.y,Star3Obj.transform.localPosition.z);

			} else if (line.StartsWith ("COLLECT COUNT ")) {
				string blocksString = line.Replace ("COLLECT COUNT", string.Empty).Trim ();
				string[] blocksNumbers = blocksString.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < blocksNumbers.Length; i++) {
					ingrCountTarget [i] = int.Parse (blocksNumbers [i]);
					ingrMaxCountTarget[i] = int.Parse (blocksNumbers [i]);
				}
			} else if (line.StartsWith ("COLLECT ITEMS ")) {
				string blocksString = line.Replace ("COLLECT ITEMS", string.Empty).Trim ();
				string[] blocksNumbers = blocksString.Split (new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < blocksNumbers.Length; i++) {
					if (target == Target.INGREDIENT)
						ingrTarget [i] = (Ingredients)int.Parse (blocksNumbers [i]);
					else if (target == Target.COLLECT)
						collectItems [i] = (CollectItems)int.Parse (blocksNumbers [i]);

				}
			} else { //Maps
				string[] st = line.Split (new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < st.Length; i++) {
					levelSquaresFile [mapLine * maxCols + i].block = (SquareTypes)int.Parse (st [i] [0].ToString ());
					levelSquaresFile [mapLine * maxCols + i].obstacle = (SquareTypes)int.Parse (st [i] [1].ToString ());
				}
				mapLine++;
			}
		}
		levelLoaded = true;
	}


	void Update () {

		if (GameStatus == GameStatusEnum.Playing) {
			if (Input.GetMouseButtonDown (0)) {
				Collider2D hit = Physics2D.OverlapPoint (XiaoChuCamera.ScreenToWorldPoint(Input.mousePosition));
				if (hit != null) {
					Item item = hit.gameObject.GetComponent<Item> ();
					if (!dragLock&&!DragBlocked && GameStatus == GameStatusEnum.Playing) {  // 点击的是元素 ，并且 没有在拖动中，并且正在游戏中
//						if (LevelManager.THIS.ActivatedBoost.type == BoostType.Bomb && item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT) {
//							SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.boostBomb);
//							LevelManager.THIS.DragBlocked = true;
//							GameObject obj = Instantiate (Resources.Load ("Prefabs/Effects/bomb"), item.transform.position, item.transform.rotation) as GameObject;
//							obj.GetComponent<SpriteRenderer> ().sortingOrder = 4;
//							obj.GetComponent<BoostAnimation> ().square = item.square;
//							LevelManager.THIS.ActivatedBoost = null;
//						} else if (LevelManager.THIS.ActivatedBoost.type == BoostType.Random_color && item.currentType != ItemsTypes.BOMB) {
//							SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.boostColorReplace);
//							LevelManager.THIS.DragBlocked = true;
//							GameObject obj = Instantiate (Resources.Load ("Prefabs/Effects/random_color_item"), item.transform.position, item.transform.rotation) as GameObject;
//							obj.GetComponent<BoostAnimation> ().square = item.square;
//							obj.GetComponent<SpriteRenderer> ().sortingOrder = 4;
//							LevelManager.THIS.ActivatedBoost = null;
//						} else if (item.square.type != SquareTypes.WIREBLOCK) {
//							item.dragThis = true;
//							item.mousePos = item.GetMousePosition ();
//							item.deltaPos = Vector3.zero;
//						}

						if (item.square.type != SquareTypes.WIREBLOCK) {
							item.dragThis = true;
							item.mousePos = item.GetMousePosition ();
							item.deltaPos = Vector3.zero;
						}
					}
				}

			} else if (Input.GetMouseButtonUp (0)) {
				Collider2D hit = Physics2D.OverlapPoint (XiaoChuCamera.ScreenToWorldPoint (Input.mousePosition));
				if (hit != null) {
					Item item = hit.gameObject.GetComponent<Item> ();
					item.dragThis = false;
					item.switchDirection = Vector3.zero;
				}

			}
		}
	}

	/// <summary>
	/// 增加 道具效果    现有就是加时间 
	/// </summary>
	public void AddsecEffect(){
		if (EliminationMgr.Instance.GameStatus == GameStatusEnum.Playing)
			Limit += 5;
	}

	/// <summary>
	/// ingrCountTarget  收集的数量    收集某种元素
	/// ingrTarget    收集的类型 Target.INGREDIENT   不可消除的特殊元素
	/// collectItems   收集类型 Target.COLLECT       现在指的是基础元素
	///    
	/// </summary>
	/// <param name="_item">Item.</param>
	public void CheckCollectedTarget (GameObject _item) {
		for (int i = 0; i < 2; i++) {
			if (ingrCountTarget [i] > 0) {
				if (_item.GetComponent<Item> () != null) {
					if (_item.GetComponent<Item> ().currentType == ItemsTypes.NONE) {
						if (_item.GetComponent<Item> ().color == (int)collectItems [i]) {
							GameObject item = new GameObject ();
							item.transform.position = new Vector3(_item.transform.position.x,_item.transform.position.y,2);
							item.transform.localScale = Vector3.one / 2f;
							SpriteRenderer spr = item.AddComponent<SpriteRenderer> ();
							spr.sprite = IngrediendData.find ("item"+_item.GetComponent<Item> ().color); //_item.GetComponent<Item> ().items [_item.GetComponent<Item> ().color];
//							spr.sortingLayerName = "UI";
							spr.sortingOrder = 9;
							item.gameObject.layer = LayerMask.NameToLayer("ItemSprite");

							StartCoroutine (StartAnimateIngredient (item, i));
						}
					} else if (_item.GetComponent<Item> ().currentType == ItemsTypes.INGREDIENT) {
						if (_item.GetComponent<Item> ().color == (int)ingrTarget [i]) {
							GameObject item = new GameObject ();
							item.transform.position = new Vector3(_item.transform.position.x,_item.transform.position.y,2);
							item.transform.localScale = Vector3.one / 2f;
							SpriteRenderer spr = item.AddComponent<SpriteRenderer> ();
							spr.sprite = _item.transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite;
//							spr.sortingLayerName = "UI";
							spr.sortingOrder = 9;
							item.gameObject.layer = LayerMask.NameToLayer("ItemSprite");
							StartCoroutine (StartAnimateIngredient (item, i));
						}
					}

				}
			}
		}	
		if (targetBlocks > 0) {
			if (_item.GetComponent<Square> () != null) {
				GameObject item = new GameObject ();
				item.transform.position = new Vector3(_item.transform.position.x,_item.transform.position.y,2);
				// item.transform.localScale = Vector3.one / 2f;
				SpriteRenderer spr = item.AddComponent<SpriteRenderer> ();
				spr.sprite = _item.GetComponent<SpriteRenderer> ().sprite;
//				spr.sortingLayerName = "UI";
				spr.sortingOrder = 9;
				item.gameObject.layer = LayerMask.NameToLayer("ItemSprite");
				StartCoroutine (StartAnimateIngredient (item, 0));

			}
		}
	}

	IEnumerator StartAnimateIngredient (GameObject item, int i) {
		if (ingrCountTarget [i] > 0)
			ingrCountTarget [i]--;

//		ingredientFly = true;
		CollectingLst.Add (item);
		GameObject[] ingr = new GameObject[2];
		ingr [0] = Ingr1.gameObject;
		ingr [1] = Ingr2.gameObject;
		if (targetBlocks > 0) {
			ingr [0] = BlocksObject.gameObject;
			ingr [1] = BlocksObject.gameObject;
		}
		AnimationCurve curveX = new AnimationCurve (new Keyframe (0, item.transform.localPosition.x), new Keyframe (0.4f, (ingr [i].transform.position.x/1024f)*7f/((float)Screen.width/1152f)));
		AnimationCurve curveY = new AnimationCurve (new Keyframe (0, item.transform.localPosition.y), new Keyframe (0.5f, (ingr [i].transform.position.y/1024f)*7f/((float)Screen.width/1152f)));
		curveY.AddKey (0.2f, item.transform.localPosition.y + UnityEngine.Random.Range (-2, 0.5f));
		float startTime = Time.time;
		Vector3 startPos = item.transform.localPosition;
		float speed = UnityEngine.Random.Range (0.4f, 0.6f);
		float distCovered = 0;
		while (distCovered < 0.5f) {
			distCovered = (Time.time - startTime) * speed;
			item.transform.localPosition = new Vector3 (curveX.Evaluate (distCovered), curveY.Evaluate (distCovered), 2);
			item.transform.Rotate (Vector3.back, Time.deltaTime * 1000);
			yield return new WaitForFixedUpdate ();
		}

		if(ingrMaxCountTarget[0] > 0)
		{
			if (ingrCountTarget [0] == 0) {
				Ingr1Text.text = "[393837]" + ingrMaxCountTarget[0]+ "  "+ Globals.Instance.MDataTableManager.GetWordText ("6016");
			} else {
				Ingr1Text.text = "[393837]" + ingrMaxCountTarget[0]+"  ([CA6363]"+(ingrMaxCountTarget[0] - ingrCountTarget[0])+"[393837]/"+ingrMaxCountTarget[0]+")";
			}
		}
		if(ingrMaxCountTarget[1] > 0)
		{
			if (ingrCountTarget [1] == 0) {
				Ingr2Text.text = "[393837]" + ingrMaxCountTarget[1] + "  " + Globals.Instance.MDataTableManager.GetWordText ("6016");
			}else{
				Ingr2Text.text = "[393837]" + ingrMaxCountTarget[1]+"  ([CA6363]"+(ingrMaxCountTarget[1] - ingrCountTarget[1])+"[393837]/"+ingrMaxCountTarget[1]+")";
			}
		}
		CollectingLst.Remove (item);
		Destroy (item);
		if(TargetMaxBlocks > 0)
			TargetBlocks = targetBlocks;
		if (GameStatus == GameStatusEnum.Playing)//1.6.1
			CheckWinLose ();
//		ingredientFly = false;
	}
		


	bool IsIngredientFalling () {//1.6.1
		if (gameStatus == GameStatusEnum.PreWinAnimations)
			return true;
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			Item itemComponent = item.GetComponent<Item> ();
			if (itemComponent != null) {
				if (itemComponent.falling && itemComponent.currentType == ItemsTypes.INGREDIENT)
					return true;
			}
		}
		return false;
	}


	/// <summary>
	///  当彩虹球 与 行，列 或者 炸弹 消除的时候 ，把所有同颜色的元素  改变为对应类型的行，列  炸弹消除元素
	///  如果 是普通元素 就直接 销毁掉
	/// </summary>
	/// <param name="p">P.</param>
	/// <param name="nextType">Next type.</param>
	public void SetTypeByColor (int p, ItemsTypes nextType) {
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if (item.GetComponent<Item> ().color == p) {
				if (nextType == ItemsTypes.HORIZONTAL_STRIPPED || nextType == ItemsTypes.VERTICAL_STRIPPED)
					item.GetComponent<Item> ().NextType = (ItemsTypes)UnityEngine.Random.Range (1, 3);
				else
					item.GetComponent<Item> ().NextType = nextType;

				item.GetComponent<Item> ().ChangeType ();
				if (nextType == ItemsTypes.NONE)
					destroyAnyway.Add (item.GetComponent<Item> ());
			}
		}
	}

	/// <summary>
	/// 20180411 当彩虹球与行 列  炸弹交换时  随机找7个元素 改变为对应特殊元素 ，引爆
	/// 
	///  顺序消除 
	/// </summary>
	public void DestroyStartBombsSpeciel(ItemsTypes nextType ,Item item1,Item item2){
		StartCoroutine (DestroyBombsSpeciel(nextType,item1,item2));
	}

	IEnumerator DestroyBombsSpeciel(ItemsTypes nextType ,Item item1,Item item2){
		dragLock = true;
		List<Item> items = GetRandomItems (7);
		Vector3 v1 = item1.transform.position;
		foreach (Item item in items) {
			item.NextType = nextType;
			item.ChangeType ();
			Vector3 v = item.transform.position;
//			GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/line"),v1, Quaternion.identity) as GameObject;
			GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/Tron"),item1.transform.position, Quaternion.identity) as GameObject;
			UVChainLightning uv = partc2.GetComponent<UVChainLightning>();
			uv.target.DOMove(v,0.5f).OnComplete(delegate() {
				DestroyObject(partc2);
			});
			yield return new WaitForSeconds (0.5f);
		}

		if(items.IndexOf(item2) < 0){
			items.Add (item2);
		}
		yield return new WaitForSeconds (1f);
		List<Item> DestroyPackage = new List<Item> ();
		foreach (Item item in items) {
			if (nextType == ItemsTypes.HORIZONTAL_STRIPPED) {

				List<Item> itemsList = EliminationMgr.Instance.GetRow(item.square.row);
				foreach (Item it in itemsList) {
					if (it != null&&it.currentType != ItemsTypes.INGREDIENT) {
						if(DestroyPackage.IndexOf(item) < 0){
							DestroyPackage.Add (item);
						}
					}
				}

			} else if (nextType == ItemsTypes.VERTICAL_STRIPPED) {

				List<Item> itemsList = EliminationMgr.Instance.GetColumn(item.square.col);
				foreach (Item it in itemsList) {
					if (it != null&&it.currentType != ItemsTypes.INGREDIENT) {
						if(DestroyPackage.IndexOf(item) < 0){
							DestroyPackage.Add (item);
						}
					}
				}
			} else if (nextType == ItemsTypes.PACKAGE) {
				List<Item> itemsList = EliminationMgr.Instance.GetItemsAround (item.square);
				foreach (Item it in itemsList) {
					if (it != null) {
						if (it.currentType != ItemsTypes.INGREDIENT) {
							if(DestroyPackage.IndexOf(item) < 0){
								DestroyPackage.Add (item);
							}
						}
					}
				}
			}
		}
		item1.DestroyItem ();
		foreach(Item it in DestroyPackage){
			it.DestroyItem (true,0, "destroy_package");
		}

		FindMatches ();
//		int num = 0;
//		while (items.Count > num) { //1.6
//			Item item = items[num];
//			item.DestroyItem ();
//			dragBlocked = true;
//			yield return new WaitForSeconds (0.1f);
//			FindMatches ();
//			while (dragBlocked)
//				yield return new WaitForFixedUpdate ();
//			num++;
//			Debug.Log ("items.Count = " + items.Count + "| num =" + num + "|dragBlocked ="+dragBlocked);
//		}
//		while (dragBlocked || GetMatches ().Count > 0)
//			yield return new WaitForFixedUpdate();
//
//		Debug.Log ("|dragBlocked ="+dragBlocked);

		dragLock = false;
	}

	/// <summary>
	/// 20180321 当彩虹球与行 列  炸弹交换时  随机找7个元素 改变为对应特殊元素 ，引爆
	/// 
	/// 同步消除
	/// </summary>
	public void DestroyBombs(ItemsTypes nextType){

		int count = 7;
		List<Item> list = new List<Item> ();
		List<Item> list2 = new List<Item> ();
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");

		foreach (GameObject item in items) {
			if (item.GetComponent<Item> ().currentType == ItemsTypes.NONE && item.GetComponent<Item> ().NextType == ItemsTypes.NONE && !item.GetComponent<Item> ().destroying) {
				list.Add (item.GetComponent<Item> ());
			}
		}
		if (list.Count < count)
			count = list.Count;
		while (list2.Count < count) {
			Item newItem = list [UnityEngine.Random.Range (0, list.Count)];
			if (list2.IndexOf (newItem) < 0) {
				list2.Add (newItem);
			}
		}

		if(list2.Count < 1){
			return;
		}
		Debug.Log ("DestroyBombs   list2 = " + list2.Count);
		StartCoroutine (ChangeTypeAndDestroyTor(list2,nextType));
	}


	IEnumerator ChangeTypeAndDestroyTor (List<Item> list2,ItemsTypes nextType) {

		List<int> HStripped = new List<int> ();
		List<Item> DestroyPackage = new List<Item> ();
		foreach(Item item in list2){
			item.NextType = nextType;
			item.ChangeTypeAndDestroy ();

			if (nextType == ItemsTypes.HORIZONTAL_STRIPPED) {
				if (!HStripped.Contains (item.square.row)) {
					HStripped.Add (item.square.row);
				}
			} else if (nextType == ItemsTypes.VERTICAL_STRIPPED) {
				if (!HStripped.Contains (item.square.col)) {
					HStripped.Add (item.square.col);
				}
			} else if (nextType == ItemsTypes.PACKAGE) {
				List<Item> itemsList = EliminationMgr.Instance.GetItemsAround (item.square);
				foreach (Item it in itemsList) {
					if (it != null) {
						if (it.currentType != ItemsTypes.BOMB && it.currentType != ItemsTypes.INGREDIENT) {
							if(DestroyPackage.IndexOf(item) < 0){
								DestroyPackage.Add (item);
							}
						}
					}
				}
			}

			yield return new WaitForSeconds (0.5f);
		}

		if(nextType == ItemsTypes.HORIZONTAL_STRIPPED){
			foreach(int v in HStripped){
				DestroySpecifyHorizontal (v);
				yield return new WaitForSeconds (1f);
			}
		}else if(nextType == ItemsTypes.VERTICAL_STRIPPED){
			foreach(int v in HStripped){
				DestroyVertical (v);
				yield return new WaitForSeconds (1f);
			}
		}else if(nextType == ItemsTypes.PACKAGE){
			foreach(Item it in DestroyPackage){
				it.DestroyItem (true,0, "destroy_package");
			}
		}

		FindMatches ();
	}

	/// <summary>
	/// 销毁指定列
	/// </summary>
	public void DestroyVertical (int col) {
		//		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.strippedExplosion);
		EliminationMgr.Instance.StrippedShow (gameObject, false);
		List<Item> itemsList = EliminationMgr.Instance.GetColumn (col);
		foreach (Item item in itemsList) {
			if (item != null) {
				if (item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT)
					item.DestroyItem (true);
			}
		}
		List<Square> sqList = EliminationMgr.Instance.GetColumnSquaresObstacles (col);
		foreach (Square item in sqList) {
			if (item != null)
				item.DestroyBlock ();
		}
	}

	/// <summary>
	/// 销毁指定行
	/// </summary>
	/// <param name="row">Row.</param>
	public void DestroySpecifyHorizontal (int row) {
//		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.strippedExplosion);
		EliminationMgr.Instance.StrippedShow (gameObject, true);

		List<Item> itemsList = EliminationMgr.Instance.GetRow (row);
		foreach (Item item in itemsList) {
			if (item != null) {
				if (item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT)
					item.DestroyItem (true);
			}
		}
		List<Square> sqList = EliminationMgr.Instance.GetRowSquaresObstacles (row);
		foreach (Square item in sqList) {
			if (item != null)
				item.DestroyBlock ();
		}
	}


	/// <summary>
	///  消除全屏Item
	/// </summary>
	/// <param name="col">Col.</param>
	public void DestroyDoubleBomb (int col) {
		StartCoroutine (DestroyDoubleBombCor (col));
		StartCoroutine (DestroyDoubleBombCorBack (col));
	}

	/// <summary>
	/// 销毁 当前列 右边的所有Item
	/// </summary>
	/// <returns>The double bomb cor.</returns>
	/// <param name="col">Col.</param>
	IEnumerator DestroyDoubleBombCor (int col) {
		for (int i = col; i < maxCols; i++) {
			List<Item> list = GetColumn (i);
			foreach (Item item in list) {
				if (item != null)
					item.DestroyItem (true,0, "", true);
			}
			yield return new WaitForSeconds (0.3f);

		}
		if (col <= maxCols - col - 1)
			FindMatches ();
	}
	/// <summary>
	/// 当前列 右边 所有的Item
	/// </summary>
	/// <returns>The double bomb cor back.</returns>
	/// <param name="col">Col.</param>
	IEnumerator DestroyDoubleBombCorBack (int col) {
		for (int i = col - 1; i >= 0; i--) {
			List<Item> list = GetColumn (i);
			foreach (Item item in list) {
				if (item != null)
					item.DestroyItem (true,0, "", true);
			}
			yield return new WaitForSeconds (0.3f);

		}
		if (col > maxCols - col - 1)
			FindMatches ();
	}
	/// <summary>
	///  获得 当前列 所有的元素，也就是整列元素
	/// </summary>
	/// <returns>The column.</returns>
	/// <param name="col">Col.</param>
	public List<Item> GetColumn (int col) {
		List<Item> itemsList = new List<Item> ();
		for (int row = 0; row < maxRows; row++) {
			itemsList.Add (GetSquare (col, row, true).item);
		}
		return itemsList;
	}

	public void FindMatches () {
		StartCoroutine (FallingDown ());
	}
	/// <summary>
	/// 检测掉落
	/// </summary>
	/// <returns>The down.</returns>
	IEnumerator FallingDown () {

		bool nearEmptySquareDetected = false;
		int combo = 0;
		AI.THIS.allowShowTip = false;
		List<Item> it = GetItems ();
		for (int i = 0; i < it.Count; i++) {
			Item item = it [i];
			if (item != null) {
				//停止动画器播放模式。当播放停止时，Avatar恢复从游戏逻辑获得控制权。
				item.anim.StopPlayback ();
			}
		}
		while (true) {

			yield return new WaitForSeconds (0.1f);

			combinedItems.Clear ();
			combinedItems = combineManager.GetCombine (); //GetMatches();  //1.6

			if (combinedItems.Count > 0)
				combo++;

			foreach (List<Item> desrtoyItems in combinedItems) {
				if(desrtoyItems.Count>3)
				{
					List<Item> itemLst = new List<Item>();
					Item itemMain = null;
					foreach (Item item in desrtoyItems) {//TODO items not destroy					
						if(item.NextType != ItemsTypes.NONE&& item.NextType != ItemsTypes.INGREDIENT)
						{
							itemMain = item;
						}
						if(item.NextType == ItemsTypes.NONE)
						{
							itemLst.Add(item);
						}
					}
					if(itemMain!=null)
					{
						foreach (Item item in itemLst)
						{							
							item.transform.DOMove(itemMain.transform.position,0.1f);
						}
					}
					yield return new WaitForSeconds (0.1f);
				}
			}


			Debug.Log ("可消除的组合 = " + combinedItems.Count);
			// 删除 可消除的组合 Item						
			foreach (List<Item> desrtoyItems in combinedItems) {
				int count = 0;
				foreach (Item item in desrtoyItems) {//TODO items not destroy
					if(desrtoyItems.Count > 3)
					{
						if (item.currentType != ItemsTypes.NONE)
							yield return new WaitForSeconds (0.1f);
						if(item.NextType != ItemsTypes.NONE)
						{
							int score = EliminationMgr.Instance.getBaseEliminateScore (item.currentType ,item.color , desrtoyItems.Count);
							item.DestroyItem (true,score);  //destroy items safely
						}
						else
						{
							item.DestroyItem ();  //destroy items safely
						}
					}
					else
					{
						if (item.currentType != ItemsTypes.NONE)
							yield return new WaitForSeconds (0.1f);
						if(count==1)
						{
							int score = EliminationMgr.Instance.getBaseEliminateScore (item.currentType ,item.color , desrtoyItems.Count);
							item.DestroyItem (true,score);  //destroy items safely
						}
						else
						{
							item.DestroyItem ();  //destroy items safely
						}
						count++;
					}
				}
				EliminationMgr.Instance.CheckEliminatePropEffect (desrtoyItems.Count);
			}
			Debug.Log ("destroyAnyway= " + destroyAnyway.Count);
			foreach (Item item in destroyAnyway) {
				item.DestroyItem (true,0, "", true);  //destroy items safely
			}
		
			destroyAnyway.Clear ();



			if (lastDraggedItem != null) {
				if (lastDraggedItem.NextType != ItemsTypes.NONE) {
					yield return new WaitForSeconds (0.5f);
				}
				lastDraggedItem = null;
			}
			// 等待 正在销毁的Item 效果播放结束
			while (!IsAllDestoyFinished ()) {
				yield return new WaitForSeconds (0.1f);
			}
			Debug.Log ("销毁的Item 效果播放结束 ");
			//falling down    掉落 ，20  可以理解为最多能掉落20个格子
			for (int i = 0; i < 20; i++) {   //just for testing
				for (int col = 0; col < maxCols; col++) {
					for (int row = maxRows - 1; row >= 0; row--) {   //need to enumerate rows from bottom to top
						if (GetSquare (col, row) != null)
							GetSquare (col, row).FallOut ();
					}
				}
			}
			if (!nearEmptySquareDetected)
				yield return new WaitForSeconds (0.2f);
			//检测收集的特殊元素 
			CheckIngredient ();
			for (int col = 0; col < maxCols; col++) {
				for (int row = maxRows - 1; row >= 0; row--) {
					if (GetSquare (col, row) != null) {
						if (!GetSquare (col, row).IsNone ()) {
							if (GetSquare (col, row).item != null) {
								GetSquare (col, row).item.StartFalling ();
							}
						}
					}
				}
			}
			yield return new WaitForSeconds (0.2f);
			GenerateNewItems ();
			StartCoroutine (RegenMatches (true));
			yield return new WaitForSeconds (0.1f);
			while (!IsAllItemsFallDown ()) {
				yield return new WaitForSeconds (0.1f);
			}

			//detect near empty squares to fall into
			nearEmptySquareDetected = false;

			for (int col = 0; col < maxCols; col++) {
				for (int row = maxRows - 1; row >= 0; row--) {
					if (GetSquare (col, row) != null) {
						if (!GetSquare (col, row).IsNone ()) {
							if (GetSquare (col, row).item != null) {
								if (GetSquare (col, row).item.GetNearEmptySquares ())
									nearEmptySquareDetected = true;

							}
						}
					}
				}
			}
			// 等待所有 的掉落 效果 完毕 
			while (!IsAllItemsFallDown ()) {//2.0
				yield return new WaitForSeconds (0.1f);
			}

			if (destroyAnyway.Count > 0)
				nearEmptySquareDetected = true;
			if (GetMatches ().Count <= 0 && !nearEmptySquareDetected)
				break;
		}

		// 销毁 ，当Item 所在的格子 信息 对应的Item 不等于 当前Item 时 
		List<Item> item_ = GetItems ();
		for (int i = 0; i < it.Count; i++) {
			Item item1 = item_ [i];
			if (item1 != null) {
				if (item1 != item1.square.item) {
					Destroy (item1.gameObject);
				}
			}
		}

		//thrive thriving blocks  如果没有消除水珠类型Item 检测
		// 找出水珠 周围的普通格子 普通元素 生成 SquareTypes.THRIVING 水珠类型  然后跳出循环
		if (!thrivingBlockDestroyed) {
			bool thrivingBlockSelected = false;
			for (int col = 0; col < maxCols; col++) {
				if (thrivingBlockSelected)
					break;
				for (int row = maxRows - 1; row >= 0; row--) {
					if (thrivingBlockSelected)
						break;
					if (GetSquare (col, row) != null) {
						if (GetSquare (col, row).type == SquareTypes.THRIVING) {
							List<Square> sqList = GetSquare (col, row).GetAllNeghbors ();
							foreach (Square sq in sqList) {
								if (sq.CanGoInto () && UnityEngine.Random.Range (0, 1) == 0 && sq.type == SquareTypes.EMPTY) {
									if (sq.item != null) {//1.6.1
										if (sq.item.currentType == ItemsTypes.NONE) {//1.6.1

											CreateObstacles (sq.col, sq.row, sq.gameObject, SquareTypes.THRIVING);

											thrivingBlockSelected = true;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}


		thrivingBlockDestroyed = false;

//		// 检测游戏结果
//		if (gameStatus == GameState.Playing && !ingredientFly)
//			LevelManager.THIS.CheckWinLose ();
		if(GameStatus == GameStatusEnum.Playing){
			EliminationMgr.Instance.CheckWinLose ();
		}


//		// 当有多个消除时，显示good great 等提示
//		if (combo > 2 && gameStatus == GameState.Playing) {
//			gratzWords [UnityEngine.Random.Range (0, gratzWords.Length)].SetActive (true);
//			combo = 0;
//		}

		CheckItemsPositions ();//1.6.1
		DragBlocked = false;

//		// 提示
		if (gameStatus == GameStatusEnum.Playing)
			StartCoroutine (AI.THIS.CheckPossibleCombines ());
	}

	public List<Item> GetRandomItems (int count) {
		List<Item> list = new List<Item> ();
		List<Item> list2 = new List<Item> ();
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if (item.GetComponent<Item> ().currentType == ItemsTypes.NONE && item.GetComponent<Item> ().NextType == ItemsTypes.NONE && !item.GetComponent<Item> ().destroying) {
				list.Add (item.GetComponent<Item> ());
			}
		}
		if (list.Count < count)
			count = list.Count;
		while (list2.Count < count) {

			try {
				Item newItem = list [UnityEngine.Random.Range (0, list.Count)];
				if (list2.IndexOf (newItem) < 0) {
					list2.Add (newItem);
				}
			} catch (Exception ex) {
				GameStatus = GameStatusEnum.Win;
				Debug.Log("赢了？？？？？？？");
			}
		}
		return list2;
	}

	//随机得到一定数量的普通元素
	public List<Item> GetRandomBaseItems (int count) {
		List<Item> list = getGetAllBaseElementItems ();
		List<Item> list2 = new List<Item> ();
		if (list.Count < count)
			count = list.Count;
		while (list2.Count < count) {
			try {
				Item newItem = list [UnityEngine.Random.Range (0, list.Count)];
				if (list2.IndexOf (newItem) < 0) {
					list2.Add (newItem);
				}
			} catch (Exception ex) {
				GameStatus = GameStatusEnum.Win;
				Debug.Log("赢了？？？？？？？");
			}
		}
		return list2;
	}

	// 检查Item的位置 ，矫正跟格子的对应位置
	void CheckItemsPositions () {//1.6.1
		List<Item> items = GetItems ();
		foreach (var item in items) {
			if (item)
				item.transform.position = item.square.transform.position + Vector3.back * 0.2f;
		}
	}

	/// <summary>
	/// 获取 地图中所有的Item
	/// </summary>
	/// <returns>The items.</returns>
	public List<Item> GetItems () {
		List<Item> itemsList = new List<Item> ();
		for (int row = 0; row < maxRows; row++) {
			for (int col = 0; col < maxCols; col++) {
				if (GetSquare (col, row) != null)
					itemsList.Add (GetSquare (col, row, true).item);
			}
		}
		return itemsList;
	}

	//检测 当所有的需要销毁的Item 销毁完毕  animationFinished 这个等于true时 说明特效展示完了
	bool IsAllDestoyFinished () {
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		Debug.Log ("IsAllDestoyFinished   items = " + items.Length);
		foreach (GameObject item in items) {
			Item itemComponent = item.GetComponent<Item> ();
			if (itemComponent == null) {
				
				return false;
			}
			if (itemComponent.destroying && !itemComponent.animationFinished)
				return false;
		}
		return true;
	}

	/// <summary>
	/// 找出所有列的最下方的位置 
	/// 如果最下方位置 的类型为收集的元素 ，则加入 删除队列，等待处理
	/// </summary>
	public void CheckIngredient () {
		int row = maxRows;
		List<Square> sqList = GetBottomRow ();
		foreach (Square sq in sqList) {
			if (sq.item != null) {
				if (sq.item.currentType == ItemsTypes.INGREDIENT) {
					destroyAnyway.Add (sq.item);
				}
			}
		}
	}
	/// <summary>
	/// o o o o
	/// o   o  
	/// 找出所有列的最下方的位置 
	/// </summary>
	/// <returns>The bottom row.</returns>
	public List<Square> GetBottomRow () {
		List<Square> itemsList = new List<Square> ();
		int listCounter = 0;
		for (int col = 0; col < maxCols; col++) {
			for (int row = maxRows - 1; row >= 0; row--) {
				Square square = GetSquare (col, row, true);
				if (square.type != SquareTypes.NONE) {
					itemsList.Add (square);
					listCounter++;
					break;
				}
			}
		}
		return itemsList;
	}

	/// <summary>
	///  行 或者  列 消除 特效
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="horrizontal">If set to <c>true</c> horrizontal.</param>
	public void StrippedShow (GameObject obj, bool horrizontal) {
//		GameObject effect = Instantiate (stripesEffect, obj.transform.position, Quaternion.identity) as GameObject;
//		if (!horrizontal)
//			effect.transform.Rotate (Vector3.back, 90);
//		Destroy (effect, 1);

		Debug.Log ("行 或者  列 消除 特效  horrizontal = " + horrizontal);
	}

	/// <summary>
	/// 获得指定行的所有元素
	/// </summary>
	/// <returns>The row.</returns>
	/// <param name="row">Row.</param>
	public List<Item> GetRow (int row) {
		List<Item> itemsList = new List<Item> ();
		for (int col = 0; col < maxCols; col++) {
			itemsList.Add (GetSquare (col, row, true).item);
		}
		return itemsList;
	}

	/// <summary>
	/// 获得指定行的非普通元素
	/// </summary>
	/// <returns>The row squares obstacles.</returns>
	/// <param name="row">Row.</param>
	public List<Square> GetRowSquaresObstacles (int row) {
		List<Square> itemsList = new List<Square> ();
		for (int col = 0; col < maxCols; col++) {
			if (GetSquare (col, row, true).IsHaveDestroybleObstacle ())
				itemsList.Add (GetSquare (col, row, true));
		}
		return itemsList;
	}

	/// <summary>
	/// 获得指定列的 非 普通 元素
	/// </summary>
	/// <returns>The column squares obstacles.</returns>
	/// <param name="col">Col.</param>
	public List<Square> GetColumnSquaresObstacles (int col) {
		List<Square> itemsList = new List<Square> ();
		for (int row = 0; row < maxRows; row++) {
			if (GetSquare (col, row, true).IsHaveDestroybleObstacle ())
				itemsList.Add (GetSquare (col, row, true));
		}
		return itemsList;
	}

	/// <summary>
	/// 获得基础元素的消除分数 
	/// </summary>
	/// <returns>The base eliminate score.</returns>
	/// <param name="type">Type.</param>
	/// <param name="count">Count.</param>
	public int getBaseEliminateScore(ItemsTypes itemType ,int type , int count){
		int colorScore = 0;
		if(itemType == ItemsTypes.NONE && count >= 3){
			if(AttrElement.Contains((GameEnumManager.CellType)type)){
				colorScore = count * (count -1) * getAttrGradeAddEffect(type); 
				int addEffect = getLevelAddEffectAttr(type,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += addEffect + propEffect;
			}else if(type == (int)GameEnumManager.CellType.NormalAttack){
				colorScore = count * (count -1) * scoreForAttackItem; 
				int addEffect = getLevelAddEffectAttr(type,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += addEffect + propEffect;
			}else if(type == (int)GameEnumManager.CellType.FireMagic){
				colorScore = count * (count -1) * scoreForFireMagicItem;
				int addEffect = getLevelAddEffectAttr((int)GameEnumManager.CellType.FireMagic,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += addEffect + propEffect;
			}else if(type == (int)GameEnumManager.CellType.WindMagic){
				colorScore = count * (count -1) * scoreForWindMagicItem; 
				int addEffect = getLevelAddEffectAttr((int)GameEnumManager.CellType.WindMagic,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}else if(type == (int)GameEnumManager.CellType.LandMagic){
				colorScore = count * (count -1) * scoreForLandMagicItem; 
				int addEffect = getLevelAddEffectAttr((int)GameEnumManager.CellType.LandMagic,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}else if(type == (int)GameEnumManager.CellType.MagicPoint){
				colorScore = count * (count -1) * scoreForMagicItem; 
				int addEffect = getLevelAddEffectAttr((int)GameEnumManager.CellType.LandMagic,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}else if(type == (int)GameEnumManager.CellType.BreakHeart){
				colorScore = (int)(((count * (count -1) * scoreForBreakItem)/100f) * EliminationMgr.Score); 
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect;
				if(colorScore < 0){
					colorScore = 0;
				}
			}
		}
		return colorScore;
	}

	/// <summary>
	/// 获得因特殊元素  所消除的普通元素分数
	/// </summary>
	/// <returns>The special eliminate score.</returns>
	public int getSpecialEliminateScore(ItemsTypes itemType ,int type){
		int colorScore = 0;
		if(itemType == ItemsTypes.NONE){
			if (AttrElement.Contains ((GameEnumManager.CellType)type)) {
				colorScore = getAttrGradeAddEffect(type) * scoreFinal; 
				int addEffect = getLevelAddEffectAttr(type,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}else if(type == (int)GameEnumManager.CellType.NormalAttack){
				colorScore = scoreForAttackItem * scoreFinal; 
				int addEffect = getLevelAddEffectAttr(type,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}else if(type == (int)GameEnumManager.CellType.FireMagic){
				colorScore = scoreForFireMagicItem * scoreFinal; 
				int addEffect = getLevelAddEffectAttr(type,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}else if(type == (int)GameEnumManager.CellType.WindMagic){
				colorScore = scoreForWindMagicItem * scoreFinal; 
				int addEffect = getLevelAddEffectAttr(type,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}else if(type == (int)GameEnumManager.CellType.LandMagic){
				colorScore = scoreForLandMagicItem * scoreFinal; 
				int addEffect = getLevelAddEffectAttr(type,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}else if(type == (int)GameEnumManager.CellType.BreakHeart){
				colorScore = 0;
			}else if(type == (int)GameEnumManager.CellType.MagicPoint){
				colorScore = scoreForMagicItem * scoreSpeicalFinal; 
				int addEffect = getLevelAddEffectAttr(type,colorScore);
				int propEffect = getPropEffectScore (type,colorScore);
				colorScore += propEffect + addEffect;
			}
		}
		return colorScore;
	}

	/// <summary>
	/// 增加分数 调用
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="pos">Position.</param>
	/// <param name="color">Color.</param>
	public void PopupScore (int type ,int value) {
		if (type == (int)GameEnumManager.CellType.BreakHeart) {
			Score -= value;
			if(Score < 0){
				Score = 0;
			}
		} else if(type == (int)GameEnumManager.CellType.MagicPoint){
			AddMagicPoint (value);
		}else {
			Score += value;
		}
		CheckStars ();
		CurrentScoreText.text = "[403136]"+ Globals.Instance.MDataTableManager.GetWordText("6012")+" "+ Score.ToString ();
		if(GameStatus == GameStatusEnum.Playing){
			EliminationMgr.Instance.CheckWinLose ();
		}
	}
	void CheckStars () {

		if (Score >= star1 && stars <= 0) {
			stars = 1;
		}
		if (Score >= star2 && stars <= 1) {
			stars = 2;
		}
		if (Score >= star3 && stars <= 2) {
			stars = 3;
		}
		ScoreSlider.value = (Score*1f) / star3;	

		if (Score >= star1) {
			star1Anim.SetActive (true);
		} else {
			star1Anim.SetActive (false);
		}
		if (Score >= star2) {
			star2Anim.SetActive (true);
		} else {
			star2Anim.SetActive (false);
		}
		if (Score >= star3) {
			star3Anim.SetActive (true);
		} else {
			star3Anim.SetActive (false);
		}
	}

	/// <summary>
	/// 获取当前消除地图的情报加成
	/// </summary>
	/// <returns>The level add effect attr.</returns>
	/// <param name="type">Type.</param>
	public int getLevelAddEffectAttr(int type , int baseScore){
		int rtn = 0;
		if(LevelAddEffectAttr != null  && LevelAddEffectAttr.Count > 0){
			foreach(KeyValuePair<int ,int> v in LevelAddEffectAttr){
				rtn = getAttrAddEffect (type ,v.Key ,baseScore, v.Value);
			}
			Debug.Log ("获得情报加成 = " + rtn);
		}
		return rtn;
	}

	//计算加成后结果
	private int getAttrAddEffect(int color , int addType , int baseScore , int addNum){
		int rtn = 0;
		GameEnumManager.LevelAddEffectEnum attr = (GameEnumManager.LevelAddEffectEnum)addType;
		switch(attr){
		case GameEnumManager.LevelAddEffectEnum.KnowledgeAddPercent:
			if(color == (int)GameEnumManager.CellType.Knowledge){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.RecreationAddPercent:
			if(color == (int)GameEnumManager.CellType.Recreation){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.LifeAddPercent:
			if(color == (int)GameEnumManager.CellType.Life){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.BusinessAddPercent:
			if(color == (int)GameEnumManager.CellType.Business){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.AttackAddPercent:
			if(color == (int)GameEnumManager.CellType.NormalAttack){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.FireAddPercent:
			if(color == (int)GameEnumManager.CellType.FireMagic){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.WindAddPercent:
			if(color == (int)GameEnumManager.CellType.WindMagic){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.LandAddPercent:
			if(color == (int)GameEnumManager.CellType.LandMagic){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.WaterAddPercent:
			if(color == (int)GameEnumManager.CellType.WaterMagic){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.KnowledgeAddNum:
			if(color == (int)GameEnumManager.CellType.Knowledge){
				rtn = addNum;
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.RecreationAddNum:
			if(color == (int)GameEnumManager.CellType.Recreation){
				rtn = addNum;
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.LifeAddNum:
			if(color == (int)GameEnumManager.CellType.Life){
				rtn = addNum;
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.BusinessAddNum:
			if(color == (int)GameEnumManager.CellType.Knowledge){
				rtn = addNum;
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.AttackAddNum:
			if(color == (int)GameEnumManager.CellType.NormalAttack){
				rtn = addNum;
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.FireAddNum:
			if(color == (int)GameEnumManager.CellType.FireMagic){
				rtn = addNum;
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.WindAddNum:
			if(color == (int)GameEnumManager.CellType.WindMagic){
				rtn = addNum;
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.LandAddNum:
			if(color == (int)GameEnumManager.CellType.LandMagic){
				rtn = addNum;
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.WaterAddNum:
			if(color == (int)GameEnumManager.CellType.WaterMagic){
				rtn = addNum;
			}
			break;

		case GameEnumManager.LevelAddEffectEnum.DamageAddPercent:
			if(color == (int)GameEnumManager.CellType.WaterMagic ||color == (int)GameEnumManager.CellType.NormalAttack||
				color == (int)GameEnumManager.CellType.FireMagic ||color == (int)GameEnumManager.CellType.WindMagic||
				color == (int)GameEnumManager.CellType.LandMagic){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		case GameEnumManager.LevelAddEffectEnum.AttrAddPercent:
			if(color == (int)GameEnumManager.CellType.Knowledge ||color == (int)GameEnumManager.CellType.Recreation||
				color == (int)GameEnumManager.CellType.Life ||color == (int)GameEnumManager.CellType.Business){
				rtn = (int)(baseScore *addNum/100f);
			}
			break;
		}
		if(rtn < 0){
			int total = baseScore + rtn;
			if (total < 0) {
				rtn = 0;
			} else {
				rtn = total;
			}
		}
		return rtn;
	}

	public int getMoodEffectScore(int type , int baseScore){
		int rtn = 0;


		return rtn;
	}

	public int getPropEffectScore(int type, int baseScore){
		int rtn = 0;
		GameEnumManager.CellType colorType = (GameEnumManager.CellType)type;
		int addNum = getSpecialPropScoreEffect (colorType);
		if(addNum > 0){
			rtn = (int)(baseScore * (addNum /100f));
		}
		return rtn;
	}

	private string getAttrAddEffectDesc(int addType , int addNum){
		string rtn = "";
		GameEnumManager.LevelAddEffectEnum attr = (GameEnumManager.LevelAddEffectEnum)addType;
		switch(attr){
		case GameEnumManager.LevelAddEffectEnum.KnowledgeAddPercent:
			InformationIcon.spriteName = "item101";
			rtn = Globals.Instance.MDataTableManager.GetWordText("6027") + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.RecreationAddPercent:
			InformationIcon.spriteName = "item102";
			rtn = Globals.Instance.MDataTableManager.GetWordText("6028") + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.LifeAddPercent:
			InformationIcon.spriteName = "item103";
			rtn = Globals.Instance.MDataTableManager.GetWordText("6029") + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.BusinessAddPercent:
			InformationIcon.spriteName = "item104";
			rtn = Globals.Instance.MDataTableManager.GetWordText("6030") + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.AttackAddPercent:
			rtn = "直接攻击加成" + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.FireAddPercent:
			rtn = "火系伤害加成" + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.WindAddPercent:
			rtn = "风系伤害加成" + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.LandAddPercent:
			rtn = "土系伤害加成" + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.WaterAddPercent:
			rtn = "水系伤害加成" + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.KnowledgeAddNum:
			rtn = "知识类得分增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.RecreationAddNum:
			rtn = "娱乐类得分增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.LifeAddNum:
			rtn = "生活类得分增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.BusinessAddNum:
			rtn = "商业类得分增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.AttackAddNum:
			rtn = "直接攻击伤害增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.FireAddNum:
			rtn = "火系伤害增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.WindAddNum:
			rtn = "风系伤害增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.LandAddNum:
			rtn = "土系伤害增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.WaterAddNum:
			rtn = "水系伤害增加" + addNum + "点";
			break;
		case GameEnumManager.LevelAddEffectEnum.DamageAddPercent:
			rtn = "五种攻击元素加成" + addNum + "%";
			break;
		case GameEnumManager.LevelAddEffectEnum.AttrAddPercent:
			rtn = "4种属性元素加成" + addNum + "%";
			break;
		}
		return rtn;
	}


	private void CheckPreWinAnimations(){

		if(limitType == LIMIT.MOVES){
			if (Limit <= 0) {
				GameStatus = GameStatusEnum.Win;
			} else {

				StartCoroutine (PreWinAnimationsCor());
			}
		}else if(limitType == LIMIT.TIME){
			//剩余时间   换算成 分数 累加上去   在这做一个减时间加分的效果 然后设置完成



			GameStatus = GameStatusEnum.Win;
		}
	}


	public bool IsAddScoreFly(){
		if(addScoreItemFly.Count > 0){
			return true;
		}
		return false;
	}

	public bool IsCollectingFly(){
		if(CollectingLst.Count > 0){
			return true;
		}
		return false;
	}

	IEnumerator PreWinAnimationsCor () {
//		if (!InitScript.Instance.losingLifeEveryGame)
//			InitScript.Instance.AddLife (1);
//		GameObject.Find ("Canvas").transform.Find ("CompleteLabel").gameObject.SetActive (true);

		Debug.Log ("显示 消除完成 提示界面 -------------------");
		yield return new WaitForSeconds (1);

		List<Item> items = GetRandomItems (limitType == LIMIT.MOVES ? Limit : 8);
		foreach (Item item in items) {
			if (limitType == LIMIT.MOVES)
				Limit--;

//			Vector3 vec3 = item.transform.position;
//			GameObject obj = Instantiate(Resources.Load("SpecialEffects/Score"),vec3, Quaternion.identity) as GameObject;
//			ScoreItem Scoreobj = obj.GetComponent<ScoreItem>();
//			Scoreobj.Init(0 ,EliminationMgr.GenerateSpecialElements);

			EliminationMgr.Instance.PopupScore(0,EliminationMgr.GenerateSpecialElements);

			item.NextType = (ItemsTypes)UnityEngine.Random.Range (1, 4);
			item.ChangeType ();
			yield return new WaitForSeconds (0.5f);
		}
		yield return new WaitForSeconds (0.3f);
		while (GetAllExtaItems ().Count > 0 && GameStatus != GameStatusEnum.Win) { //1.6
			Item item = GetAllExtaItems () [0];
			item.DestroyItem ();
			dragBlocked = true;
			yield return new WaitForSeconds (0.1f);
			FindMatches ();
			yield return new WaitForSeconds (1f);
			while (dragBlocked)
				yield return new WaitForFixedUpdate ();
		}
		yield return new WaitForSeconds (1f);
		while (dragBlocked || GetMatches ().Count > 0)
			yield return new WaitForSeconds (0.2f);

//		GameObject.Find ("Canvas").transform.Find ("CompleteLabel").gameObject.SetActive (false);
//
//		GameObject.Find ("Canvas").transform.Find ("PreCompleteBanner").gameObject.SetActive (true);

		Debug.Log ("所有的特殊元素都消除 结束了   == " + Score);

		yield return new WaitForSeconds (1f);
//		GameObject.Find ("Canvas").transform.Find ("PreCompleteBanner").gameObject.SetActive (false);
//		if (PlayerPrefs.GetInt (string.Format ("Level.{0:000}.StarsCount", currentLevel), 0) < stars)
//			PlayerPrefs.SetInt (string.Format ("Level.{0:000}.StarsCount", currentLevel), stars);
//		if (Score > PlayerPrefs.GetInt ("Score" + currentLevel)) {
//			PlayerPrefs.SetInt ("Score" + currentLevel, Score);
//		}
		Debug.Log ("需要在这显示结算界面了   == " + Score);
		#if PLAYFAB || GAMESPARKS
		NetworkManager.dataManager.SetPlayerScore (currentLevel, Score);
		NetworkManager.dataManager.SetPlayerLevel (currentLevel + 1);
		NetworkManager.dataManager.SetStars ();
		#endif

		GameStatus = GameStatusEnum.Win;
	}

	//  返回可以生成的颜色列表 
	public List<int> getBaseColorLimitLst(){
		Dictionary<int,int> tempColorLimitNumDic = new Dictionary<int, int> ();
		foreach(KeyValuePair<int,int> v in colorLimitNumDic){
			tempColorLimitNumDic.Add (v.Key,v.Value);
		}

		if (tempColorLimitNumDic.Count > 0) {
			List<Item> itemLst = new List<Item> ();
			GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
			foreach (GameObject item in items) {
				if (item.GetComponent<Item> ().currentType == ItemsTypes.NONE && item.GetComponent<Item> ().NextType == ItemsTypes.NONE && !item.GetComponent<Item> ().destroying) {
					if (tempColorLimitNumDic.ContainsKey (item.GetComponent<Item> ().color)) {
						tempColorLimitNumDic [item.GetComponent<Item> ().color] -= 1;
					}
				}
			}
			List<int> rtnColor = new List<int> ();
			foreach (int v in EliminationMgr.Instance.colorLimitLst) {
				if (tempColorLimitNumDic.ContainsKey (v) && tempColorLimitNumDic [v] <= 0) {
					continue;
				}
				rtnColor.Add (v);
			}
			return rtnColor;	
		} else {
			return EliminationMgr.Instance.colorLimitLst;
		}
	}

	public bool getIngredientimitLst(int ingrKey){
		int ingrValue = -1;
		foreach(KeyValuePair<int,int> v in colorLimitNumDic){
			if(v.Key == ingrKey){
				ingrValue = v.Value;
			}
		}
		if (ingrValue > 0) {
			List<Item> itemLst = new List<Item> ();
			GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
			foreach (GameObject item in items) {
				if (item.GetComponent<Item> ().currentType == ItemsTypes.INGREDIENT && !item.GetComponent<Item> ().destroying) {
					if (ingrKey == item.GetComponent<Item> ().color) {
						ingrValue -= 1;
					}
				}
			}

			foreach(KeyValuePair<int,Dictionary<int,int>> v1 in GenerateSpecialItemDic){
				foreach(KeyValuePair<int,int> v2 in v1.Value){
					if(v2.Value == ingrKey){
						ingrValue -= 1;
					}	
				}
			}

			return ingrValue > 0 ? true : false;	
		} else {
			return true;
		}
	}


	// 获取  当前的特殊元素 
	List<Item> GetAllExtaItems () {
		List<Item> list = new List<Item> ();
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if (item.GetComponent<Item> ().currentType != ItemsTypes.NONE && item.GetComponent<Item> ().currentType != ItemsTypes.INGREDIENT) {
				list.Add (item.GetComponent<Item> ());
			}
		}

		return list;
	}

	public List<Item> GetIngredients (int i) {
		List<Item> list = new List<Item> ();
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if (item.GetComponent<Item> ().currentType == ItemsTypes.INGREDIENT && item.GetComponent<Item> ().color == (int)EliminationMgr.Instance.ingrTarget [i]) {
				list.Add (item.GetComponent<Item> ());
			}
		}
		return list;
	}


	private List<Item> getGetAllSpecialColorItems(int color){
		List<Item> list = new List<Item> ();
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if(!item.GetComponent<Item> ().destroying){
				if (item.GetComponent<Item> ().currentType == ItemsTypes.NONE && item.GetComponent<Item> ().NextType == ItemsTypes.NONE&&
					item.GetComponent<Item> ().color == color) {
					list.Add (item.GetComponent<Item> ());
				}
			}
		}
		return list;
	}

	//获得所有的基础元素 
	private List<Item> getGetAllBaseElementItems(){
		List<Item> list = new List<Item> ();
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if(!item.GetComponent<Item> ().destroying){
				if (item.GetComponent<Item> ().currentType == ItemsTypes.NONE && item.GetComponent<Item> ().NextType == ItemsTypes.NONE&&
					BaseElement.Contains((GameEnumManager.CellType)item.GetComponent<Item> ().color)) {
					list.Add (item.GetComponent<Item> ());
				}
			}
		}
		return list;
	}
	//获得所有的属性元素
	private List<Item> getGetAllAttrElementItems(){
		List<Item> list = new List<Item> ();
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if(!item.GetComponent<Item> ().destroying){
				if (item.GetComponent<Item> ().currentType == ItemsTypes.NONE && item.GetComponent<Item> ().NextType == ItemsTypes.NONE&&
					AttrElement.Contains((GameEnumManager.CellType)item.GetComponent<Item> ().color)) {
					list.Add (item.GetComponent<Item> ());
				}
			}
		}
		return list;
	}

	// -------------------------道具实现 ------------------------------//

	List<List<int>> SpecialPropEffect = new List<List<int>> ();
	//消除数量影响的道具效果
	private void CheckEliminatePropEffect(int eliminateNum){
		foreach(List<int> v in SpecialPropEffect){
			if(eliminateNum >= v[1]){
//				if(v[0] == (int)GameEnumManager.PropEffectEnum.AddStepFromEliminate){
//					AddStepEffect (v[2]);
//				}else 
				if(v[0] == (int)GameEnumManager.PropEffectEnum.AddMagicPointFromEliminate){
					AddMagicPoint (v[2]);
				}
//				else if(v[0] == (int)GameEnumManager.PropEffectEnum.AddMoodFromEliminate ||
//					v[0] == (int)GameEnumManager.PropEffectEnum.AddAngerFromEliminate){
////					AddMood (v[2]);
//				}	
			}
		}
	}

	List<List<int>> SpecialPropScoreEffect = new List<List<int>> ();
	private int getSpecialPropScoreEffect(GameEnumManager.CellType elementType){
		int rtn = 0;
		foreach (List<int> v in SpecialPropScoreEffect) {
			if (v [0] == (int)GameEnumManager.PropEffectEnum.BaseElementAddScoreEffect) {
				if(BaseElement.Contains(elementType)){
					rtn += v[2];
				}
			}else if (v [0] == (int)GameEnumManager.PropEffectEnum.ReduceBreakHeartScore) {
				if(elementType == GameEnumManager.CellType.BreakHeart){
					rtn -= v[2];
				}
			}
		}
		return rtn;
	}

	public void UsePropMethod(GPropInfo.PropInfoElement propInfo,UITexture PropIcon){
		string[] propStr = propInfo.PropEffect.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries );
		for(int i = 0; i < propStr.Length ; i++){
			List<int> lst = StrParser.ParseDecIntList (propStr[i],-1);
			if(lst.Count == 3){
				if(lst[0] == (int)GameEnumManager.PropEffectEnum.AddStep){
					AddStepEffect (lst[2]);
				}else if(lst[0] == (int)GameEnumManager.PropEffectEnum.ChangeToSpecial){
					RandomChangeElement (lst[1],lst[2]);
				}
	//			else if(lst[0] == (int)GameEnumManager.PropEffectEnum.AddStepFromEliminate){
	//				SpecialPropEffect.Add (lst);
	//			}
	//			else if(lst[0] == (int)GameEnumManager.PropEffectEnum.DestroySpecial){
	//				DestroySpecialElement(lst[2]);
	//			}
				else if(lst[0] == (int)GameEnumManager.PropEffectEnum.RefreshMap){
					NoMatches ();
				}
	//			else if(lst[0] == (int)GameEnumManager.PropEffectEnum.ColorChange){
	//				ChangeElementType (lst[1],lst[2]);
	//			}
				else if(lst[0] == (int)GameEnumManager.PropEffectEnum.AddMagicPointFromEliminate){
					SpecialPropEffect.Add (lst);
				}
	//			else if(lst[0] == (int)GameEnumManager.PropEffectEnum.AddMoodFromEliminate){
	//				SpecialPropEffect.Add (lst);
	//			}
				else if(lst[0] == (int)GameEnumManager.PropEffectEnum.OrdinaryEliminate){
					DirectEliminate (lst[2]);
				}
	//			else if(lst[0] == (int)GameEnumManager.PropEffectEnum.AddAngerFromEliminate){
	//				SpecialPropEffect.Add (lst);
	//			}
				else if(lst[0] == (int)GameEnumManager.PropEffectEnum.BaseElementAddScoreEffect){
					SpecialPropScoreEffect.Add (lst);
				}else if(lst[0] == (int)GameEnumManager.PropEffectEnum.BaseChangeAttrElement){
					BaseChangeAttrElementEffect (lst[2]);
				}else if(lst[0] == (int)GameEnumManager.PropEffectEnum.SpecialElementChangeBase){
					SpecialElementChangeBaseEffect (lst[2]);
				}else if(lst[0] == (int)GameEnumManager.PropEffectEnum.ReduceBreakHeartScore){
					SpecialPropScoreEffect.Add (lst);
				}
				GUIPhotoGraph guiPhotoGraph = Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph>();
				if(guiPhotoGraph!=null)
				{					
					if(lst[0] == (int)GameEnumManager.PropEffectEnum.AddMagicPointFromEliminate
						||lst[0] == (int)GameEnumManager.PropEffectEnum.BaseElementAddScoreEffect
						||lst[0] == (int)GameEnumManager.PropEffectEnum.ReduceBreakHeartScore)
					{
						guiPhotoGraph.SetBuffInfo(lst[0],Globals.Instance.MDataTableManager.GetWordText(propInfo.PropDesc),PropIcon);
					}
				}
			}
		}
		MagicPower -= propInfo.MagicPoint;
//		Globals.Instance.MGUIManager.ShowSimpleCenterTips("使用成功",true);
	}

	// 增加步数
//	private void AddStepEffect(int num){
//
//		EliminationMgr.Instance.Limit += num;
//	}
	private void AddStepEffect(int num){
		StartCoroutine (AddStepEffectIE (num));

	}
	IEnumerator AddStepEffectIE (int num) {
		yield return new WaitForSeconds (0.2f);
		for (int i = 0; i < num; i++) {
			EliminationMgr.Instance.Limit++;
			yield return new WaitForSeconds (0.1f);
		}
	}
	// 销毁特殊元素   参数为数量
	private void DestroySpecialElement(int num){
		List<Item> lst = GetAllExtaItems ();
		if(num > lst.Count){
			num = lst.Count;
		}
		if(num > 0){
			List<Item> destroyItem = new List<Item> ();
			while(destroyItem.Count < num){
				int index = UnityEngine.Random.Range (0,lst.Count);
				if(destroyItem.IndexOf(lst[index]) < 0){
					destroyItem.Add (lst[index]);
				}
			}

			foreach(Item it in destroyItem){
				if (it.currentType == ItemsTypes.BOMB) {
					if (EliminationMgr.Instance.GetRandomItems (1).Count > 0) {
						Item item2 = EliminationMgr.Instance.GetRandomItems (1) [0];
						it.CheckChocoBomb (it , item2);
					}
					else
						it.DestroyItem (true);
				} else {
					it.DestroyItem (true);
				}

			}

			EliminationMgr.Instance.FindMatches ();
		}
	}

	// 将普通元素改变为  需要的类型
	private void RandomChangeElement(int type,int num){

		List<Item> lst = GetRandomBaseItems (num);
		if(lst.Count > 0){
			foreach (Item item in lst) {
				ItemsTypes change = ItemsTypes.NONE;
				if(type == (int)GameEnumManager.CellType.RainbowBall){
					change = ItemsTypes.BOMB;
				}else if(type == (int)GameEnumManager.CellType.Bomb){
					change = ItemsTypes.PACKAGE;
				}else if(type == (int)GameEnumManager.CellType.RowClear){
					change = ItemsTypes.HORIZONTAL_STRIPPED;
				}else if(type == (int)GameEnumManager.CellType.ColumnClear){
					change = ItemsTypes.VERTICAL_STRIPPED;
				}
				item.NextType = change;
				item.ChangeType ();
			}
		}
	}

	//刷新地图
	private void RefreshMap(){

		NoMatches ();
	}

	//增加 magicPoint 魔力点
	private void AddMagicPoint(int magicPoint){

		MagicPower += magicPoint;
	}

	// 一次性  消除num个普通元素
	private void DirectEliminate(int num){
		List<Item> lst = getGetAllBaseElementItems ();
		List<Item> attrLst = getGetAllAttrElementItems ();
		foreach(Item v in attrLst){
			lst.Add (v);
		}

		if(lst.Count < num){
			num = lst.Count;
		}

		List<Item> lst1 = new List<Item> ();
		if(num > 0){
			while(lst1.Count < num){
				int index = UnityEngine.Random.Range (0,lst.Count);
				if(lst1.IndexOf(lst[index]) < 0){
					lst1.Add (lst[index]);
				}
			}
			foreach(Item it in lst1){
				it.DestroyItem (true);
			}
		}
		FindMatches ();
	}


	//把 所有的这种类型的color，改为普通的元素 
	private void ChangeElementType(int color ,int changeColor){
		if (EliminationMgr.Instance.colorLimitLst.Contains (color)) {
			if (changeColor > 0 && EliminationMgr.Instance.colorLimitLst.Contains (changeColor)) {
				List<Item> colorLst = getGetAllSpecialColorItems (color);
				foreach(Item it in colorLst){
					it.SetColor (changeColor);
				}
			} else {
				List<int> lst = new List<int> ();
				if(color!=(int)GameEnumManager.CellType.Knowledge&&
					EliminationMgr.Instance.colorLimitLst.Contains((int)GameEnumManager.CellType.Knowledge)){
					lst.Add ((int)GameEnumManager.CellType.Knowledge);
				}
				if(color!=(int)GameEnumManager.CellType.Recreation&&
					EliminationMgr.Instance.colorLimitLst.Contains((int)GameEnumManager.CellType.Recreation)){
					lst.Add ((int)GameEnumManager.CellType.Recreation);
				}
				if(color!=(int)GameEnumManager.CellType.Life&&
					EliminationMgr.Instance.colorLimitLst.Contains((int)GameEnumManager.CellType.Life)){
					lst.Add ((int)GameEnumManager.CellType.Life);
				}
				if(color!=(int)GameEnumManager.CellType.Business&&
					EliminationMgr.Instance.colorLimitLst.Contains((int)GameEnumManager.CellType.Business)){
					lst.Add ((int)GameEnumManager.CellType.Business);
				}
				int index = UnityEngine.Random.Range (0,lst.Count);
				int changeColor1 = lst[index];
				List<Item> colorLst = getGetAllSpecialColorItems (color);
				foreach(Item it in colorLst){
					it.SetColor (changeColor1);
				}
			}
			FindMatches ();
		}
	}


	private void BaseChangeAttrElementEffect(int count){
		List<Item> lst = GetRandomBaseItems (count);
		foreach(Item it in lst){
			int index = UnityEngine.Random.Range (0,AttrElement.Count);
			GameEnumManager.CellType changeColor = AttrElement[index];
			it.SetColor ((int)changeColor);
		}
		FindMatches ();
	}
//	private void BaseChangeAttrElementEffect(int count){
//		StartCoroutine (BaseChangeAttrElementEffectIE (count));
//	}
//	IEnumerator BaseChangeAttrElementEffectIE (int count) {
//		yield return new WaitForSeconds (0.1f);
//		List<Item> lst = GetRandomBaseItems (count);
//		foreach(Item it in lst){			
//			int index = UnityEngine.Random.Range (0,AttrElement.Count);
//			GameEnumManager.CellType changeColor = AttrElement[index];
//			it.SetColor ((int)changeColor);
//			yield return new WaitForSeconds (0.1f);
//		}
//		FindMatches ();
//	}

	private void SpecialElementChangeBaseEffect(int count){

		List<Item> list = new List<Item> ();
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if(!item.GetComponent<Item> ().destroying){
				if (item.GetComponent<Item> ().currentType == ItemsTypes.NONE && item.GetComponent<Item> ().NextType == ItemsTypes.NONE&&
					((GameEnumManager.CellType)item.GetComponent<Item> ().color == GameEnumManager.CellType.BreakHeart || 
						(GameEnumManager.CellType)item.GetComponent<Item> ().color == GameEnumManager.CellType.MagicPoint)) {
					list.Add (item.GetComponent<Item> ());
				}
			}
		}
		if(list.Count < count){
			count = list.Count;
		}
		List<Item> lst1 = new List<Item> ();
		if(count > 0){
			while(lst1.Count < count){
				int index = UnityEngine.Random.Range (0,list.Count);
				if(lst1.IndexOf(list[index]) < 0){
					lst1.Add (list[index]);
				}
			}
			foreach(Item it in lst1){
				int index = UnityEngine.Random.Range (0,BaseElement.Count);
				GameEnumManager.CellType changeColor = BaseElement[index];
				it.SetColor ((int)changeColor);
			}
		}
		FindMatches ();
	}

	Dictionary<int,Dictionary<int,int>> GenerateSpecialItemDic = new Dictionary<int, Dictionary<int, int>> ();
	public void AddGenerateSpecialItem(int col , int row ,int color){

		if (GenerateSpecialItemDic.ContainsKey (col)) {
			if (GenerateSpecialItemDic [col].ContainsKey (row)) {
				GenerateSpecialItemDic [col] [row] = color;
			} else {
				GenerateSpecialItemDic [col].Add (row ,color);
			}
		} else {
			Dictionary<int,int> dic = new Dictionary<int, int> ();
			dic.Add (row,color);
			GenerateSpecialItemDic.Add (col,dic);
		}
	}

	public int getGenerateSpecialItemColor(int col , int row){
		if (GenerateSpecialItemDic.ContainsKey (col)) {
			if (GenerateSpecialItemDic [col].ContainsKey (row)) {
				int rtnColor = GenerateSpecialItemDic [col] [row];
				if(rtnColor > 0){
					GenerateSpecialItemDic [col] [row] = -1;
				}
				return rtnColor;
			}
		}
		return -1;
	}


	public void OnClose(){
		Destroy (GameField.gameObject);
		if(XiaoChuCamera != null)
			Destroy (XiaoChuCamera.gameObject);
		if(EliminationBg != null)
			Destroy (EliminationBg.gameObject);
	}

	private int getAttrGradeAddEffect(int type){
		float rtnScore = 0f;
		int scoreRatio = getAttrGradeRatio (type);
		if(type == (int)GameEnumManager.CellType.Knowledge){
			rtnScore = EliminationMgr.Instance.scoreForKnowledgeItem * ( 1 + (scoreRatio*1f/100f));
		}else if(type == (int)GameEnumManager.CellType.Recreation){
			rtnScore = EliminationMgr.Instance.scoreForRecreationItem * ( 1 + (scoreRatio*1f/100f));
		}else if(type == (int)GameEnumManager.CellType.Life){
			rtnScore = EliminationMgr.Instance.scoreForLifeItem * (1 + (scoreRatio*1f/100f));
		}else if(type == (int)GameEnumManager.CellType.Business){
			rtnScore = EliminationMgr.Instance.scoreForBusinessItem * ( 1 + (scoreRatio*1f/100f));
		}
		Debug.LogError ("计算属性得分 = " + type + "|" + rtnScore);
		return (int)rtnScore;
	}

	private int getAttrGradeRatio(int type ){
		int grade = 0;
		int rtnRatio = 0;
		if(mAttrGradeDic.ContainsKey(type)){
			int attrNum = GetPlayerAttributeNum (type);
			foreach(KeyValuePair<int , AttrGradeConfig.AttrGradeElement> v in mAttrGradeDic[type]){
				if(attrNum > v.Value.NeedAttrNum && grade < v.Value.AttrGrade){
					grade = v.Value.AttrGrade;
					rtnRatio = v.Value.ScoreRatio;
				}
			}
		}
		return rtnRatio;
	}

	private int GetPlayerAttributeNum(int type)
	{
		int mNum = 0;
		switch(type)
		{
		case (int)GameEnumManager.CellType.Knowledge:
			mNum = playerData.starData.nRoleYanJi + playerData.starData.equipYanJi;
			break;
		case (int)GameEnumManager.CellType.Recreation:
			mNum = playerData.starData.nRoleDongGan + playerData.starData.equipDongGan;
			break;
		case (int)GameEnumManager.CellType.Life:
			mNum = playerData.starData.nRoleXueShi + playerData.starData.equipXueShi;
			break;
		case (int)GameEnumManager.CellType.Business:
			mNum = playerData.starData.nRoleYiTai + playerData.starData.equipYiTai;
			break;
		}
		return mNum;
	}


	void OnDestory(){

		Instance = null;
	}
}
