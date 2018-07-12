using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipPropertyPanel : RoleBasePanel 
{
	public UIButton returnBtn;
	public KnightFullInfo knightFullInfo;
	
	protected override void Awake()
	{
		base.Awake();

	}
	
	public override void UpdateGUI()
	{
		
	}
	
	public override void HideGUI()
	{
	}
	
	public override void OnClickNonFunctionalArea(GameObject obj) 
	{
	}
	
	void UpdateData(GirlData data)
	{
		knightFullInfo.UpdateFullInfo(data);
	}
}
