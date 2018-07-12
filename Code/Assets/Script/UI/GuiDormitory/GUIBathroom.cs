using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIBathroom : GUIWindowForm 
{
	
	public UIImageButton ImageExitBtn;
	
	
	//-------------------------------------------------
	protected override void Awake()
	{		
		if(!Application.isPlaying || null == Globals.Instance.MGUIManager) return;
	
		base.Awake();
	
	}
	protected override void OnDestroy ()
	{
		base.OnDestroy ();
	
	}

	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;	
		this.GUILevel = 10;	
		transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,100);
		
	}
	
	protected virtual void Start ()
	{
		base.Start();
	
	}
	private void OnClickImageExitBtn(GameObject obj)
	{
		
	}


	ISubscriber netUpdatePackageItemSub;
	ISubscriber netOpenPackageSlotSub;

}
