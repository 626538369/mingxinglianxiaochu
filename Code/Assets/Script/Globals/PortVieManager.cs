using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortVieManager : MonoBehaviour 
{		
	void Awake()
	{
		isPortVieOn = false;
	}
	
	public void VieTimerStart()
	{
		InvokeRepeating("VieTimerTickNotify",0,1);
	}
	
	public void VieTimerEnd()
	{
		//CancelInvoke();
		//sPortVieRemainTime = 0;
		//isPortVieOn = false;
		//
		//if(sPortVie != null)
		//{
		//	sPortVie.Close();
		//	sPortVie = null;
		//}
		//
		//if(Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>() != null && !sIsPortVieRewardOn)
		//{
		//	Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().RemoveLeftBtn(GUIMain.EFunctionButtonType.Juezhan);
		//}
	}
	
	public void MCancelInvoke()
	{
		CancelInvoke();
	}
	
	public void VieTimerTickNotify()
	{
	//	if(sPortVieRemainTime < 0)
	//	{
	//		VieTimerEnd();
	//	}
	//	sPortVie.UpdateRemainTime();
	//	sPortVieRemainTime--;
	//	
	//	if(isVieFailCD)
	//	{
	//		vieFailCDTime--;
	//		if(vieFailCDTime <= 0)
	//		{
	//			isVieFailCD = false;
	//		}
	//		sPortVie.VieFailCDControl(vieFailCDTime,isVieFailCD);
	//	}
	}

	
	public void CreateGUIPortVieReward()
	{
	}
	
	[HideInInspector]public bool isPortVieOn;
	//[HideInInspector]public GUIPortVie sPortVie;
	[HideInInspector]public int sPortVieRemainTime;
	[HideInInspector]public bool sIsVieBattling;
	[HideInInspector]public int vieFailCDTime;
	[HideInInspector]public bool isVieFailCD;
	[HideInInspector]public System.DateTime lastCDDate;
	[HideInInspector]public int puVieDiamondInspire;
	[HideInInspector]public int puVieDiamondOrder;
	[HideInInspector]public bool puFlagReverseFleetPosition;
	//--------------port vie reward----------------------------
	[HideInInspector]public bool sIsPortVieRewardOn;
	[HideInInspector]public bool sIsShowVieRewardView;
}
