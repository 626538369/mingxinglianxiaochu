using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stage2DPort : Stage2D
{
	public Transform npcRoot;
	
	public override void Init()
	{
		Vector3 localScale = Vector3.one;
		localScale.x = Globals.Instance.MGUIManager.widthRatio;
		localScale.y = Globals.Instance.MGUIManager.heightRatio;
		transform.localScale = localScale;
		
		status = GameStatusManager.Instance.MPortStatus;
		
		// Hide all buildings
		foreach (Transform child in npcRoot.transform)
		{
			child.localScale = Vector3.zero;
		}
		
		SeaAreaData seaData = Globals.Instance.MGameDataManager.MCurrentSeaAreaData;
		PortData portData = seaData.MPortData;
		foreach (BuildingData data in portData.BuildingDataList.Values)
		{
			status.AddBuilding(data.LogicID, status.InstantiateBuilding(data));
		}
	}
	
	PortStatus status;
}