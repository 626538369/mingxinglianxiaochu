using UnityEngine;
using System.Collections;

public class Status2DPort : PortStatus 
{
	public override void Initialize()
	{
		_mPublisher.NotifyEnterPort();
		
		Globals.Instance.MFingerEvent.Add3DEventListener(this);
		this.SetFingerEventActive(true);
		
		InitPortBuildings_impl();
		Globals.Instance.MTeachManager.NewTeachEnterPort();
		
		// tzz added for GUINewCardRetreiveBlueprintBtn clicked
		if(EnterPortDoneEvent != null){
			EnterPortDoneEvent();
			EnterPortDoneEvent = null;
		}
		
		// // Interactive with other Component
		// UIManager.instance.AddMouseTouchPtrListener(EZGUIPtrListener);
	}
	
	void InitPortBuildings_impl()
	{
		_mHoldBuildingList.Clear();
		
		GameObject stageRoot = GameObject.Find("Stage2DRoot");
		stagePort = stageRoot.GetComponent<Stage2DPort>() as Stage2DPort;
		stagePort.Init();
	}
	
	public override bool OnFingerDownEvent( int fingerIndex, Vector2 fingerPos )
	{
		return true;
	}
	
	public override bool OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
		return true;
	}
	
	public override bool OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		return false;	
	}
	
	public override bool OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos )
	{
		return false;
	}
	
	public Stage2DPort getStage2dPort()
	{
		return stagePort ;
	}
	
	Stage2DPort stagePort = null;
}
