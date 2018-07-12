using UnityEngine;
using System.Collections;

public class GameEnumManager {

	public enum MoodBGType{
		left = 1,
		right = 2,
		middle = 3,
	}

	public enum CompleteTargetType
	{
		Score = 0,       //得分
		Collect = 1,     //收集
		FallGroup = 2,   //掉落
		Block = 3,       //障碍
	}

	public enum CellType
	{
		Knowledge = 101,
		Recreation = 102,  //娱乐
		Life = 103,        //生活
		Business = 104,    //商业

		NormalAttack = 201,//普通攻击
		FireMagic = 202,
		WindMagic = 203,
		LandMagic = 204,
		WaterMagic = 205,

		SweetHeart = 301,
		BreakHeart = 302, //减分元素
		MagicPoint = 303,
		AngrySeed = 304,
		QuietSeed = 305,

		Bomb = 401, // 范围炸弹
		RowClear = 402,
		ColumnClear = 403,
		RainbowBall = 404, //彩虹球
		StepOrTime = 999,
	}

	public enum StepOrTimeType
	{
		step = 0,
		time = 1,
	}

	//关卡地图 加成枚举
	public enum LevelAddEffectEnum{
		KnowledgeAddPercent = 21011,
		RecreationAddPercent = 21021,
		LifeAddPercent = 21031,
		BusinessAddPercent = 21041,

		AttackAddPercent = 22011,
		FireAddPercent = 22021,
		WindAddPercent = 22031,
		LandAddPercent = 22041,
		WaterAddPercent = 22051,

		DamageAddPercent = 22991, //直接伤害，火系 风系 土系 水系 加成

		KnowledgeAddNum = 21012,
		RecreationAddNum = 21022,
		LifeAddNum = 210132,
		BusinessAddNum = 21042,

		AttackAddNum = 22012,
		FireAddNum = 22022,
		WindAddNum = 22032,
		LandAddNum = 22042,
		WaterAddNum = 22052,

		AttrAddPercent = 21991,//知识 娱乐 生活 商业 加成
	}

	public enum LevelType{

		Girl = 1,
		Moster = 2,
	}

	public enum PropEffectEnum{
		AddStep = 1, // 直接增加步数
		ChangeToSpecial = 2, //改变普通元素为特殊元素
		AddStepFromEliminate = 3, //四消以上增加步数---------
		DestroySpecial = 4,//随机引爆特殊元素-------
		RefreshMap = 5,//重新生成地图元素
		ColorChange = 6, //普通元素 改变--------
		AddMagicPointFromEliminate = 7,// //四消以上增加魔力点++++
		AddMoodFromEliminate = 8,//四消以上增加心情------
		OrdinaryEliminate = 9, //随机消除N个普通元素
		AddMood = 10,// 直接增加心情 ，或者怒气-----
		ReduceAnger = 11,// 减少怒气----------------
		AddAngerFromEliminate = 12,//四消以上增加怒气-----
		BaseElementAddScoreEffect = 13,//普通元素得分加成++++
		BaseChangeAttrElement = 14,//随机将普通元素转化为属性元素
		SpecialElementChangeBase = 15,//随机将减分元素/魔力点转化为普通元素
		ReduceBreakHeartScore = 16,//减分元素扣分减少++++
	}
}
