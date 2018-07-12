using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct LoginDayData
{
	public int itemTypeId;
	public int itemCount;
}

// Reward package Base Class
public abstract class RewardPackageData
{
	public bool isCanReceive = true;
	public bool isReceive = false;
	
	public string name;
	public string descript;
	
	public RewardPackageData(string iName, string iDesc)
	{
		name = iName;
		descript = iDesc;
	}
	
	public abstract void OnReceiveReward();
}

public class DiamonReward : RewardPackageData
{
	public int canReceiveCount = -1;
	public long canReceiveRemainTime = -1;
	
	public DiamonReward(string iName = "", string iDesc = "") : base(iName, iDesc)
	{
	}
	
	public override void OnReceiveReward()
	{
		long actorid = Globals.Instance.MGameDataManager.MActorData.PlayerID;
		NetSender.Instance.RequestIngotPackage(actorid);
	}
}

public class DailyOilReward : RewardPackageData
{
	public DailyOilReward(string iName = "", string iDesc = "") : base(iName, iDesc)
	{
		base.name = Globals.Instance.MDataTableManager.GetWordText(22200019);
	}
	
	public override void OnReceiveReward()
	{
		NetSender.Instance.RequestReceiveOil();
	}
}

public class DailyMoneyReward : RewardPackageData
{
	public DailyMoneyReward(string iName = "", string iDesc = "") : base(iName, iDesc)
	{
		base.name = Globals.Instance.MDataTableManager.GetWordText(22200018);
	}
	
	public override void OnReceiveReward()
	{
		NetSender.Instance.RequestReceiveMoney();
	}
}

public class RankReward : RewardPackageData
{
	public RankReward(string iName = "", string iDesc = "") : base(iName, iDesc)
	{
		base.name = "军衔礼包";
	}
	
	public override void OnReceiveReward()
	{
		NetSender.Instance.RequestReceiveRankPackage();
	}
}

public class ArenaReward : RewardPackageData
{
	public ArenaReward(string iName = "", string iDesc = "") : base(iName, iDesc)
	{
		base.name = "竞技礼包";
	}
	
	public override void OnReceiveReward()
	{
	}
}

//public class ActivityReward : RewardPackageData
//{
//	sg.Gift_Is_Has_Res.Gift_Wait_Receive serverInfo = null;
//	public ActivityReward(sg.Gift_Is_Has_Res.Gift_Wait_Receive info) : base(info.giftName, info.giftExplain)
//	{
//		serverInfo = info;
//	}
//	
//	public override void OnReceiveReward()
//	{
//		if (null == serverInfo)
//			return;
//		
//		NetSender.Instance.RequestReceiveGift(serverInfo.giftId);
//	}
//}

public class NewPalyerPakeageData 
{
	// public bool chargePackageCanReceive = false;
	// public bool chargePackageIsReceive = false;
	
	public bool ingotPackageCanReceive = false;
	public bool ingotPackageIsReceive = false;
	public int ingotPackageReceiveNum;
	public float ingotPackageRemainTime;
	public int ingotPackageNum;
	
	public bool rankPackageCanReceive = false;
	public bool rankPackageIsReceive = false;
	
	public bool arenaPackageCanReceive = false;
	public bool arenaPackageIsReceive = false;
	
	public bool loginPackageIsReceive = false;
	public int loginPackageReceiveDay;
	public List<LoginDayData> loginDayList = new List<LoginDayData>();
	
	public List<List<LoginDayData> > fiveDaysDataList = new List<List<LoginDayData> >();
	
	public DiamonReward diamonReward = null;
	public List<RewardPackageData> canReceiveRewardPackages = new List<RewardPackageData>();
}
