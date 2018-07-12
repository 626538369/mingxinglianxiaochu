using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIDataManager : MonoBehaviour {

	void Awake()
	{
		mNewPalyerPakeageData = new NewPalyerPakeageData();
		mIngotExData = new IngotExData();
		
		HintMsgMgr = new HintMsgManager();
	}
	
	public NewPalyerPakeageData mNewPalyerPakeageData = null;
	public HintMsgManager HintMsgMgr = null;
	
	public IngotExData mIngotExData = null;
}
