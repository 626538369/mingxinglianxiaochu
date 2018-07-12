using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * user behavir statistics
 * author: tzz
 * date: 2012-09-11
 */ 
public class Statistics  {
	
	//! instance
	public static readonly Statistics INSTANCE = new Statistics();
	
	//! send data interval (second)
	private static readonly float	SendDataIntervalSec = 2 * 60;
		
	//! statistics type 
	public enum StatType{
		
		//! finger press
		e_fingerPress,
		
		//! finger release
		e_fingerRelease,
		
		//! require create window
		e_requireCreateWnd,
		
		//! create window
		e_createdWnd,
		
		//! open to show window
		e_openWnd,
		
		//! close window
		e_destoryWnd,
		
		//! hide window
		e_hideWnd,
						
		//! button event 
		e_btnEvent,
	};
	
	//! stat
	class StatData{
		
		public long 	m_tickTime;
		
		public string 	m_data;
		
		public StatType	m_type;
		
		public StatData(string _data, StatType _type){
			m_data		= _data;
			m_type		= _type;
			m_tickTime	= HelpUtil.CurrTimeStamp();
		}
		
		public string ToString(){
			return string.Format("{0} {1} {2}",(int)m_type,m_tickTime,m_data);
		}
	}
	
	private bool enabledStatData = false;
	//! the statistics data list
	private List<StatData>	m_statDateList = new List<StatData>();
	
	//! 
	private List<StatData>	m_statDateListResend = new List<StatData>();
	
	//! the send data timer
	private float	m_sendDataTimer		= 0.0f;
	
	//! prevent construct out of singleton instance
	private Statistics(){}
	
	#region Sub-Function
	//! send data to the server
	private void SendData(){
		if(m_statDateList.Count <= 0){
			return;
		}
		
		sg.C2GS_Statistics_Client_Req req	= new sg.C2GS_Statistics_Client_Req();		
		req.requestTime 					=   HelpUtil.CurrTimeStamp();
		
		foreach(StatData data in m_statDateList){
			req.clientAction.Add(data.ToString());
			
			//m_statDateListResend.Add(data);
		}	
		
		m_statDateList.Clear();
		
		Packet pak 			= new Packet();
		pak.m_object 		= req;
		pak.m_packetType 	= PacketOpcode.C2GS_STATISTICS_CLIENT_REQ;
		
		//Globals.Instance.MGSNetManager.Send(pak);
	}
	
	//! add the finger statistics data to list
	private void FingerData(StatType _type,int _fingerIdx,Vector2 _pos){
		if (!enabledStatData) return;
		
		string t_data = string.Format("{0} {1},{2}",_fingerIdx,_pos.x,_pos.y);
		
		StatData t_sd = new StatData(t_data,_type);
		m_statDateList.Add(t_sd);
	}
	
	#endregion
	
	
	#region Pub-Function
	//! must be called every frame
	public void Update(){
		if (!enabledStatData) return;
		
		if(Globals.Instance.MGameDataManager == null 
		|| Globals.Instance.MGameDataManager.MActorData == null 
		|| Globals.Instance.MGameDataManager.MActorData.BasicData == null){
			// the player has been NOT created
			return;
		}
		
		UnityEngine.Profiling.Profiler.BeginSample("Statistics.Update");
		
		m_sendDataTimer += Time.deltaTime;
		if(m_sendDataTimer > SendDataIntervalSec){
			m_sendDataTimer = 0.0f;
			
			SendData();
		}
		
		// touch statistics
		//
		bool t_touchScreen = false;
		
#if UNITY_IPHONE || UNITY_ANDROID
		t_touchScreen = !Application.isEditor;
#endif
		
		bool t_clicked=false;
		bool t_btnUp=false;
				
		if(t_touchScreen){
			
			for(int i = 0;i < Input.touches.Length;i++){
			
				t_btnUp		= false;
				t_clicked	= Input.touches[i].phase == TouchPhase.Began;
				t_btnUp		= Input.touches[i].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended;
				
				if(t_clicked){
					FingerData(StatType.e_fingerPress,i,Input.touches[i].position);
				}else if(t_btnUp){
					FingerData(StatType.e_fingerRelease,i,Input.touches[i].position);
				}
			}
		}else{
			if((t_clicked = Input.GetMouseButtonDown(0)) || (t_btnUp = Input.GetMouseButtonUp(0)) ){
				Vector2 t_pos		= new Vector2(Input.mousePosition.x,Input.mousePosition.y);
				
				if(t_clicked){
					FingerData(StatType.e_fingerPress,0,t_pos);
				}else if(t_btnUp){
					FingerData(StatType.e_fingerRelease,0,t_pos);
				}
			}
		}
		
		UnityEngine.Profiling.Profiler.EndSample();		
	}
		
	//! EZ button click event
	//public void BtnClicked(AutoSpriteControlBase _btn){
	//	if (!enabledStatData) return;
	//	
	//	string t_name = null;
	//	Transform t_transform = _btn.transform;
	//	
	//	while(t_transform != null){
	//		if(t_name != null){
	//			t_name = t_transform.name + "/" + t_name;
	//		}else{
	//			t_name = t_transform.name;
	//		}
	//		
	//		t_transform = t_transform.parent;
	//	}
	//	m_statDateList.Add(new StatData(t_name,StatType.e_btnEvent));
	//}
	
	//! the 
	public void GUIWndRequireCreateWnd(string _name){
		if (!enabledStatData) return;
		
		m_statDateList.Add(new StatData(_name,StatType.e_requireCreateWnd));
	}
	
	public void GUIWndCreatedWnd(string _name){
		if (!enabledStatData) return;
		
		m_statDateList.Add(new StatData(_name,StatType.e_createdWnd));
	}
	
	//! the window open event
	public void GUIWndOpenShow(string _name){	
		if (!enabledStatData) return;
		
		m_statDateList.Add(new StatData(_name,StatType.e_openWnd));
	}
	
	//! the window close event
	public void GUIWndDestory(string _name){
		if (!enabledStatData) return;
		
		m_statDateList.Add(new StatData(_name,StatType.e_destoryWnd));
	}
	
	//! hide the window
	public void GUIWndHide(string _name){	
		if (!enabledStatData) return;
		
		m_statDateList.Add(new StatData(_name,StatType.e_hideWnd));
	}
	#endregion
	
	
	public enum CustomEventType{
		ShangChengItem,
		PackageGrid,
		BlueprintGain,
		BlueprintCompose,
		
		GeneralTrain,
		TechSpeedUp,
		TaskDaily,
		SweepSpeedUp,
		
		ArenaCleanup,
		ArenaIncreaseTime,
		PKCleanup,
		EliteCopy,
		
		EquReinforce,
		IngotExchange,
		BuyOil,
		BuyTreasureMap,
		
		BuyJunHunGrub,
		PortInvest,
		AddShipBlood,
		NewbieTeachScene,
		
		NewbieLogin,
		EnterCopy,
		InterruptCopy,
		FailedCopyBattle,
		
		MakeItem,
		UpdateLevel,
		TaskComplete,
		WantToPay,
		
		PortDefend,
		PortVieCommand,
		PortVieInspire,
				
		Paied,
		ChangeFleetName,
		
		IngotConsume,
		MoneyConsume,
	}
	
	/// <summary>
	/// Customs the event call.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='param'>
	/// Parameter.
	/// </param>
	public void CustomEventCall(CustomEventType type,params object[] param){
		return;
		try{
			Globals.Instance.MConnectManager.PutFlurryEvent(type.ToString(),param);
			
			ProcessTalkingDataPurchaseEvent(type,param);
		}catch{};
	}
	
	/// <summary>
	/// Charges the event when successfully
	/// </summary>
	/// <param name='orderID'>
	/// Order I.
	/// </param>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='payName'>
	/// Pay name.
	/// </param>
	/// <param name='count'>
	/// Count.
	/// </param>
	/// <param name='price'>
	/// Price.
	/// </param>
	public void ChargeEvent(string orderID,string type,string payName,string count,string price){
		
		double tPrice	= StrParser.ParseFloat(price,0);
		int tAmount		= StrParser.ParseDecInt(count,0);
		
		Statistics.INSTANCE.CustomEventCall(Statistics.CustomEventType.Paied,"Type",type,"PayName",payName,"Amount",tAmount,"Price",tPrice);
			
		TDGAChargeInfo info = new TDGAChargeInfo();
		info.setChargeID(orderID).setChargeType(TDGAPaymentType.Other).setVirtualCurrency(tPrice).setCurrency(tPrice);
		TalkingDataGA.Charge(info,Globals.Instance.MGameDataManager.MActorData.GetHighestLevelGeneral());
	}
	
	/// <summary>
	/// Processes the talking data purchase event.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='param'>
	/// Parameter.
	/// </param>
	private void ProcessTalkingDataPurchaseEvent(CustomEventType type,params object[] param){
		
		switch(type){
		case CustomEventType.BlueprintCompose:
			if((bool)param[1]){
				UploadTalkingDataPurchaseEvent(type.ToString(),100);
			}			
			break;
		case CustomEventType.TechSpeedUp:
			if((int)param[3] == 2){
				UploadTalkingDataPurchaseEvent(type.ToString(),(int)param[5]);
			}
			break;
		case CustomEventType.IngotExchange:
			UploadTalkingDataPurchaseEvent(type.ToString(),(int)param[1]);
			break;
		case CustomEventType.AddShipBlood:
			UploadMoneyConsumeEvent(type.ToString(),(int)param[1]);
			break;
		case CustomEventType.BuyTreasureMap:
			UploadMoneyConsumeEvent(type.ToString(),(int)param[1]);
			break;				
		default:
			// upload the purchase talkingdata event
			//
			for(int idx = 0;idx < param.Length;idx++){
				
				object p = param[idx];
				
				// has
				if(p is string && p.ToString() == "price" && idx < param.Length - 1){
					
					bool tUploadIngot = true;
					
					for(int i = 0;i < param.Length;i++){
						
						if(param[i] is string && param[i].ToString() == "useIngot"){
							
							if(!(bool)(param[i + 1])){
								tUploadIngot = false;		
							}
						}
					}
					
					int priceOrMoney = int.Parse(param[idx + 1].ToString());
					
					if(tUploadIngot){
						UploadTalkingDataPurchaseEvent(type.ToString(),priceOrMoney);
					}else{
						UploadMoneyConsumeEvent(type.ToString(),priceOrMoney);
					}
					
					break;
				}
			}
			
			break;
		}
		
	}
		
	/// <summary>
	/// Uploads the talking data purchase event.
	/// </summary>
	/// <param name='itemLogicID'>
	/// Item logic I.
	/// </param>
	/// <param name='price'>
	/// Price.
	/// </param>
	private void UploadTalkingDataPurchaseEvent(string itemLogicID,int price){
		
		if(Globals.Instance.MGameDataManager.MActorData.WealthData.GoldIngot < price){
			// just return if ingot is NOT enough
			return;
		}
		
		// Purchase TalkingData
		//
		TDGAPurchaseInfo purchaseInfo = new TDGAPurchaseInfo();
		purchaseInfo.setItemId(itemLogicID).setVirtualCurrency(price).setItemNumber(1);
		
		TalkingDataGA.Purchase(purchaseInfo,Globals.Instance.MGameDataManager.MActorData.GetHighestLevelGeneral());
		
		Globals.Instance.MConnectManager.PutFlurryEvent(CustomEventType.IngotConsume.ToString(),itemLogicID,price);
	}
	
	/// <summary>
	/// Uploads the money consume event.
	/// </summary>
	/// <param name='item'>
	/// Item.
	/// </param>
	/// <param name='money'>
	/// Money.
	/// </param>
	private void UploadMoneyConsumeEvent(string item,int money){
		
		if(Globals.Instance.MGameDataManager.MActorData.WealthData.Money < money){
			// just return if Money is NOT enough
			return;
		}
		
		Globals.Instance.MConnectManager.PutFlurryEvent(CustomEventType.MoneyConsume.ToString(),item,money);
	}
}

