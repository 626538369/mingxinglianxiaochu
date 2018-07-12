using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EGeneralProfession
{ 
	//1230000000  全可  , 1230000001  航母, 1230000002  水面舰艇, 1230000005  潜艇
	ALL_ADAPTED = 1230000000, 
	
	//航母将领
	CARRIER_GENERAL = 1230000001,
	
	// 水面舰艇将领 23467
	SURFACE_SHIP_GENERAL = 1230000002, 
	
	//潜艇将领
	SUBMARINE_GENERAL = 1230000005,
}

public enum GeneralState
{
	// 未招募，但可招募，是否与已解雇有关?
	NOT_RECRUIT = 1,
	
	// 不可招募，条件不满足等等
	CANNOT_RECRUIT = 2,
	
	// 已招募
	RECRUITED = 3,
	
	// 已解雇
	DISMISSED = 4,
}

public enum GeneralEquipLocation
{
	WEAPON = 1,
	SWORD = 2,
	CAP = 3,
	UNIFORM = 4,
	MEDAL = 5,
	BOOK = 6,
	
	MAX
}

public class GeneralBasicData
{
	public static int INVALID_ID = -1;
	public static int INVALID_WARSHIP_ID = -1; 
	public static int MAX_QUALITY_STAR_COUNT = 10; 
	
	public int LogicID;
	
	public int GeneralType;
	public int GeneralCountry;
	
	public int NameID; 
	public string Name; 
	
	public EGeneralProfession Profession;
	public string ProfessionName;
	
	public string Avatar; 
	public int Gender;
	
	public int BiographyID;
	public string Biography;
	
	public int BaseValiant;
	public int BaseCommand;
	public int BaseIntelligence;  
	
	public int SerialRecord;
	
	public int Level; 
	public int CurrentExp;
	public int NextLevelExp;
	
	public int QualityStar;
	public int RarityLevel;
	
	public bool IsAdmiral;
	
	public void FillDataFromConfig()
	{
		
	}
	
	private int CalculateQualityStar()
	{
		// Rule
		int val = BaseValiant + BaseCommand + BaseIntelligence;
		if (val >= 1 && val <= 169)
		{
			return 1;
		}
		else if (val >= 170 && val <= 209)
		{
			return 2;
		}
		else if (val >= 210 && val <= 399)
		{
			return 3;
		}
		else if (val >= 400 && val <= 599)
		{
			return 4;
		}
		else if (val >= 600 && val <= 999)
		{
			return 5;
		}
		else if (val >= 1000 && val <= 1499)
		{
			return 6;
		}
		else if (val >= 1500 && val <= 1999)
		{
			return 7;
		}
		else if (val >= 2000 && val <= 2499)
		{
			return 8;
		}
		else if (val >= 2500 && val <= 3499)
		{
			return 9;
		}
		else if (val >= 3500 && val <= 5000)
		{
			return 10;
		}
		
		return 0;
	}
}

public class GeneralPropertyData
{
	public int Life;
	public int MaxLife;

	public int Defense;

	public int Power;
	public int MaxPower = 200;
	
	public int Valiant;
	public int Command;
	public int Intelligence;  
	
	public int SeaAttack;
	public int MinSeaAttack;
	public int MaxSeaAttack;
	
	public int SubmarineAttack;
	public int MinSubmarineAttack;
	public int MaxSubmarineAttack;

	public int SkillAttack;
	
	public int Range;
	// one ten thousand 
	//  AttackRate * GirlData.RATE_SCALE_UNIT
	// public int HitRate;
	// public int CritRate;
	// public int DodgeRate;
	
	public int PowerTimeUp;
	public int PowerAttackUp;
	public int PowerAttackedUp;
	
	// Designer move the QualityStar property to here
	public int CalculateQualityStar()
	{
		// Rule
		int val = Valiant + Command + Intelligence;
				if (val >= 1 && val <= 169)
		{
			return 1;
		}
		else if (val >= 170 && val <= 209)
		{
			return 2;
		}
		else if (val >= 210 && val <= 399)
		{
			return 3;
		}
		else if (val >= 400 && val <= 599)
		{
			return 4;
		}
		else if (val >= 600 && val <= 999)
		{
			return 5;
		}
		else if (val >= 1000 && val <= 1499)
		{
			return 6;
		}
		else if (val >= 1500 && val <= 1999)
		{
			return 7;
		}
		else if (val >= 2000 && val <= 2499)
		{
			return 8;
		}
		else if (val >= 2500 && val <= 3499)
		{
			return 9;
		}
		else if (val >= 3500 && val <= 5000)
		{
			return 10;
		}
		
		
		return 0;
	}
}

public class GeneralModifyData
{
	public int MaxLifeModify;
	public int MaxLifeModifyForever;
	
	public int DefenseModify;
	public int DefenseModifyForever;
	
	public int MinSeaAttackModify;
	public int MinSeaAttackModifyForever;
	public int MaxSeaAttackModify;
	public int MaxSeaAttackModifyForever;
	
	public int MinSubmarineAttackModify;
	public int MinSubmarineAttackModifyForever;
	public int MaxSubmarineAttackModify;
	public int MaxSubmarineAttackModifyForever;
	
	public int SkillModify;
	public int SkillModifyForever;
	
	public short FillSpeedModify;
	public short FillSpeedModifyForever;
	
	public short RangeModify;
	public short RangeModifyForever;
	
	public short HitRateModify;
	public short HitRateModifyForever;
	
	public short CritRateModify;
	public short CritRateModifyForever;
	
	public short DodgeRateModify;
	public short DodgeRateModifyForever;
	
	public short PowerTimeUpModify;
	public short PowerTimeUpModifyForever;
	public short PowerAttackedUpModify;
	public short PowerAttackedUpModifyForever;
	public short PowerAttackUpModify;
	public short PowerAttackUpModifyForever;
}

public class GeneralTrainData
{
	//<train
	public int TrainCount;
	
	public int BaseChivalrousMod;
	public int BaseCommandMod;
	public int BaseIntelligenceMod;
	
	public int TempChivalrousMod;
	public int TempCommandMod;
	public int TempIntelligenceMod;
	//>
}

public class GeneralPotionData
{
	public int Chivalrous1Num;
	public int Chivalrous2Num;
	public int Chivalrous3Num;
	public int Chivalrous4Num;
	public int Chivalrous5Num;
	public int Chivalrous6Num;
	
	public int Command1Num;
	public int Command2Num;
	public int Command3Num;
	public int Command4Num;
	public int Command5Num;
	public int Command6Num;
	
	public int Intelligence1Num;
	public int Intelligence2Num;
	public int Intelligence3Num;
	public int Intelligence4Num;
	public int Intelligence5Num;
	public int Intelligence6Num;
	
	public int[] PotionTongShuaiLevel= new int[6];
	public int[] PotionYongWuLevel = new int[6];
	public int[] PotionZhiLiLevel= new int[6];
}

public class GeneralData
{
	public static readonly int JUNHUN_SLOT_COUNT = 5;
	
	public long GeneralID;
	public int ArmySoulPoint;
	// public int GeneralLogicID;
	
	public long HoldWarshipID = -1;
	public int HoldWarshipLogicID = -1;
	
	public GeneralBasicData BasicData = new GeneralBasicData();
	
	public GeneralPropertyData PropertyData = null;
	public GeneralModifyData ModifyData = null;
	public GeneralTrainData TrainData = null;
	public GeneralPotionData PotionData = null;
	
	public Dictionary<int, SkillData> SkillDatas = new Dictionary<int, SkillData>();
	public Dictionary<int, ItemSlotData> EquipmentDatas = new Dictionary<int, ItemSlotData>();
	public Dictionary<int, ItemSlotData> JunHunDatas = new Dictionary<int, ItemSlotData>();
	
	public GeneralData()
	{
		for (int i = (int)GeneralEquipLocation.WEAPON; i < (int)GeneralEquipLocation.MAX; i++)
		{
			ItemSlotData data = new ItemSlotData();
			
			data.LocationID = i;
			data.SlotType = ItemSlotType.GENERAL_EQUIPMENT;
			EquipmentDatas[data.LocationID] = data;
		}
		
		for (int i = 0; i < JUNHUN_SLOT_COUNT; i++)
		{
			ItemSlotData data = new ItemSlotData();
			
			data.LocationID = i;
			data.SlotType = ItemSlotType.JUNHUN_EQUIPMENT;
			JunHunDatas[data.LocationID] = data;
		}
	}
	
	public string GetDisplayName()
	{
		if (1 == BasicData.QualityStar || 2 == BasicData.QualityStar)
		{
			return GUIFontColor.White255255255 + BasicData.Name;
		}
		else if (3 == BasicData.QualityStar || 4 == BasicData.QualityStar)
		{
			return GUIFontColor.PureGreen + BasicData.Name;
		}
		else if (5 == BasicData.QualityStar || 6 == BasicData.QualityStar)
		{
			return GUIFontColor.PureBlue + BasicData.Name;
		}
		else if (7 == BasicData.QualityStar || 8 == BasicData.QualityStar)
		{
			return GUIFontColor.Purple + BasicData.Name;
		}
		else if (9 == BasicData.QualityStar || 10 == BasicData.QualityStar)
		{
			return GUIFontColor.Orange + BasicData.Name;
		}
		else if (11 == BasicData.QualityStar || 12 == BasicData.QualityStar)
		{
			return GUIFontColor.PureRed + BasicData.Name;
		}
		
		return GUIFontColor.White255255255 + BasicData.Name;
	}
	
	public void UpdateEquipData(ItemSlotData data)
	{
		EquipmentDatas[data.LocationID] = data;
	}
	
	public void UpdateJunHunData(ItemSlotData data)
	{
		JunHunDatas[data.LocationID] = data;
	}
	
	public void UpdateFromServerData(sg.Role_General source)
	{
		this.GeneralID = source.generalListId;
		this.ArmySoulPoint = source.armySoulPoint;
		if (null != source.roleGeneralBase)
		{
			BasicData.LogicID = source.roleGeneralBase.generalId;
			BasicData.Name = source.roleGeneralBase.admiralName;
			BasicData.Avatar = source.roleGeneralBase.admiralAvatar;
			BasicData.IsAdmiral = source.roleGeneralBase.isAdmiral;
			BasicData.Profession = (EGeneralProfession)source.roleGeneralBase.profession;
			
			BasicData.FillDataFromConfig();
		}
		
		if (null != source.roleGeneralDegree)
		{
			BasicData.Level = source.roleGeneralDegree.generalDegree;
			BasicData.CurrentExp = source.roleGeneralDegree.generalExp;
			BasicData.NextLevelExp = source.roleGeneralDegree.nextLevelExp;
		}
		
		if (null != source.roleGeneralProperty)
		{
			if (null == PropertyData)
			{
				PropertyData = new GeneralPropertyData();
			}
			
			PropertyData.Life = source.roleGeneralProperty.maxLife;
			PropertyData.MaxLife = source.roleGeneralProperty.maxLife;
			PropertyData.Defense = source.roleGeneralProperty.warShipDefense;
			
			PropertyData.Valiant = source.roleGeneralProperty.generalChivalrous;
			PropertyData.Command = source.roleGeneralProperty.generalCommand;
			PropertyData.Intelligence = source.roleGeneralProperty.generalIntelligence;
			
			PropertyData.MinSeaAttack = source.roleGeneralProperty.minSeaAttack;
			PropertyData.MaxSeaAttack = source.roleGeneralProperty.maxSeaAttack;
			PropertyData.MinSubmarineAttack = source.roleGeneralProperty.minSubmarineAttack;
			PropertyData.MaxSubmarineAttack = source.roleGeneralProperty.maxSubmarineAttack;
			PropertyData.SkillAttack = source.roleGeneralProperty.skillAddition;
			
			PropertyData.SeaAttack = (int)(0.5f * (PropertyData.MinSeaAttack + PropertyData.MaxSeaAttack));
			PropertyData.SubmarineAttack = (int)(0.5f * (PropertyData.MinSubmarineAttack + PropertyData.MaxSubmarineAttack));
			
			// PropertyData.HitRate = source.roleGeneralProperty.hitRate;
			// PropertyData.CritRate = source.roleGeneralProperty.critRate;
			// PropertyData.DodgeRate = source.roleGeneralProperty.dodgeRate;
			PropertyData.Range = source.roleGeneralProperty.range;
			
			PropertyData.Power = source.roleGeneralProperty.powerInitial;
			PropertyData.PowerTimeUp = source.roleGeneralProperty.powerTimeUp;
			PropertyData.PowerAttackUp = source.roleGeneralProperty.powerAttackUp;
			PropertyData.PowerAttackedUp = source.roleGeneralProperty.powerAttackedUp;
			
			BasicData.QualityStar = PropertyData.CalculateQualityStar();
		}
		
		if (null != source.roleGeneralCulture)
		{
			// Train
			if (null == TrainData)
			{
				TrainData = new GeneralTrainData();
			}
			
			TrainData.TrainCount = source.roleGeneralCulture.cultureNum;
			TrainData.BaseChivalrousMod = source.roleGeneralCulture.baseChivalrousMod;
			TrainData.BaseCommandMod = source.roleGeneralCulture.baseCommandMod;
			TrainData.BaseIntelligenceMod = source.roleGeneralCulture.baseIntelligenceMod;
			TrainData.TempChivalrousMod = source.roleGeneralCulture.tempChivalrousMod;
			TrainData.TempCommandMod = source.roleGeneralCulture.tempCommandMod;
			TrainData.TempIntelligenceMod = source.roleGeneralCulture.tempIntelligenceMod;
		}
		
		if (null != source.roleGeneralPharmacy)
		{
			if (null == PotionData)
			{
				PotionData = new GeneralPotionData();
			}
			
			PotionData.PotionYongWuLevel[0] = source.roleGeneralPharmacy.pharmacyChivalrous1Num;
			PotionData.PotionYongWuLevel[1] = source.roleGeneralPharmacy.pharmacyChivalrous2Num;
			PotionData.PotionYongWuLevel[2] = source.roleGeneralPharmacy.pharmacyChivalrous3Num;
			PotionData.PotionYongWuLevel[3] = source.roleGeneralPharmacy.pharmacyChivalrous4Num;
			PotionData.PotionYongWuLevel[4] = source.roleGeneralPharmacy.pharmacyChivalrous5Num;
			PotionData.PotionYongWuLevel[5] = source.roleGeneralPharmacy.pharmacyChivalrous6Num;
			
			PotionData.PotionTongShuaiLevel[0] = source.roleGeneralPharmacy.pharmacyCommand1Num;
			PotionData.PotionTongShuaiLevel[1] = source.roleGeneralPharmacy.pharmacyCommand2Num;
			PotionData.PotionTongShuaiLevel[2] = source.roleGeneralPharmacy.pharmacyCommand3Num;
			PotionData.PotionTongShuaiLevel[3] = source.roleGeneralPharmacy.pharmacyCommand4Num;
			PotionData.PotionTongShuaiLevel[4] = source.roleGeneralPharmacy.pharmacyCommand5Num;
			PotionData.PotionTongShuaiLevel[5] = source.roleGeneralPharmacy.pharmacyCommand6Num;
			
			PotionData.PotionZhiLiLevel[0] = source.roleGeneralPharmacy.pharmacyIntelligence1Num;
			PotionData.PotionZhiLiLevel[1] = source.roleGeneralPharmacy.pharmacyIntelligence2Num;
			PotionData.PotionZhiLiLevel[2] = source.roleGeneralPharmacy.pharmacyIntelligence3Num;
			PotionData.PotionZhiLiLevel[3] = source.roleGeneralPharmacy.pharmacyIntelligence4Num;
			PotionData.PotionZhiLiLevel[4] = source.roleGeneralPharmacy.pharmacyIntelligence5Num;
			PotionData.PotionZhiLiLevel[5] = source.roleGeneralPharmacy.pharmacyIntelligence6Num;
		}
		
		if (null != source.roleGeneralSkill)
		{
			SkillDatas.Clear();
			foreach (int id in source.roleGeneralSkill.skillIds)
			{
				SkillData data = new SkillData(id);
				data.FillDataFromConfig();
				
				SkillDatas[id] = data;
			}
		}
		
		foreach (sg.Item_Pack info in source.roleGeneralEquipmentList)
		{
			ItemSlotData slotData = new ItemSlotData();
			slotData.UpdateFromServerData(info);
			this.EquipmentDatas[slotData.LocationID] = slotData;
		}
		
		foreach (sg.Item_Pack info in source.roleGeneralArmySoulList)
		{
			ItemSlotData slotData = new ItemSlotData();
			slotData.UpdateFromServerData(info);
			this.JunHunDatas[slotData.LocationID] = slotData;
		}
	}
	
	/// <summary>
	/// Gets the item number by logic ID
	/// </summary>
	/// <returns>
	/// The item number by logic ID
	/// </returns>
	/// <param name='_logicID'>
	/// _logic ID
	/// </param>
	/// <param name='_loadList'>
	/// _load list fill list
	/// </param>
	public int GetItemNumByLogicID(int _logicID,List<ItemSlotData> _loadList = null){
		
		int tItemNum = 0;
		
		// equipment
		foreach(ItemSlotData item in EquipmentDatas.Values){
			if(item.MItemData != null && item.MItemData.BasicData.LogicID == _logicID){
				tItemNum++;
				if(_loadList != null){
					_loadList.Add(item);
				}
			}
		}
		
		// JunHun
		foreach(ItemSlotData item in JunHunDatas.Values){
			if(item.MItemData != null && item.MItemData.BasicData.LogicID == _logicID){
				tItemNum++;
				if(_loadList != null){
					_loadList.Add(item);
				}
			}
		}
		
		return tItemNum;
	}
}

public enum HQRecruitType
{
	RECRUIT,
	RECALL,
	DISMISS,
}

public class HQGeneralData
{
	public int RecuitMoney;
	public int RecuitGoldIngot;
	public int RecuitRankRequire;
	public GeneralState State;
	
	public GeneralData GeneralData = new GeneralData();
}

#region  Arena
//add by laraft 2012-7-23 18:47  竞技场5位玩家舰队信息
public class ArenaGeneralData
{
	public int admiralDegree;     //玩家舰队长等级
	public string admiralName;      //玩家舰队长名称
	public string admiralAvatar;    //玩家舰队长头像地址
	public int ranking;           //玩家天梯排名
	public long roleId;				//玩家舰队长id
	public int deviceModel;
	public string roleAddress = "bj";
}

public class ArenaBattleLogData
{
	public long attackerRoleId;    //玩家做为挑战方时，玩家的roleId；作为防守方时，值为0
	public long attackedRoleId;    //玩家做为挑战方时，值为0；作为防守方时，玩家roleId
	public string admiralName;     //舰队长姓名，即玩家姓名
	public bool isSelfWinner;    //战斗是否胜利
	public long battleTime;  //战斗时间
	public string battleFile;  //战斗场景文件存放地址
	
	public int rankChange;
	public int selfLastRanking;
	public int selfNowRanking;
	
	public int attackerDeviceMode;
}


public	class ArenaPushData
{
	public long roleId;    //玩家ID
	public string admiralName;   //玩家角色名称
	public bool rankingTop;     //是否是第一名
	public int rankingChange;   //排名变化
	public int winningStreakNum;//连胜场次
	public int deviceModel;
}
	
#endregion
	
