using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class HintMsg
{
	public abstract string Name
	{
		get;
	}
	
	public abstract string Descript
	{
		get;
	}
	
	public HintMsg(object s2cData)
	{
		serverData = s2cData;
	}
	
	public abstract void OnClickMsg();
	
	protected object serverData;
}

public class ArenaHintMsg : HintMsg
{
	public ArenaHintMsg(object data) : base(data)
	{
	}
	
	public override string Name
	{
		get { return "战斗消息"; }
	}
	
	public override string Descript
	{
		get { return "有人攻击了你"; }
	}
	
	public override void OnClickMsg()
	{

	}
}

public class PkHintMsg : HintMsg
{
	public override string Name
	{
		get { return "战斗消息"; }
	}
	
	public override string Descript
	{
		get { return ""; }
	}
	
	public PkHintMsg(object data) : base(data)
	{}
	
	public override void OnClickMsg()
	{
	}
}

public class HintMsgManager
{
	public int Count
	{
		get { return hintMsgs.Count; }
	}
	
	public void ClearHintMsgs()
	{
		hintMsgs.Clear();
	}
	
	public void AddHintMsg(HintMsg msg)
	{
		hintMsgs.Add(msg);
	}
	
	public void RemoveHintMsg(HintMsg msg)
	{
		hintMsgs.Remove(msg);
	}
	
	public HintMsg GetHintMsg(int index)
	{
		if (index < -1 || index >= hintMsgs.Count)
			return null;
		
		return hintMsgs[index];
	}
	
	List<HintMsg> hintMsgs = new List<HintMsg>();
}
