using UnityEngine;
using System.Collections;

public class GUITaskPicView : GUIWindow {
	
	public float mLeaveTime = 3;
	private float time;
	
	void Update ()
	{
		if(time > 0)
		{
			time -= Time.deltaTime;
			if(time <= 0)
				Object.Destroy(gameObject);
		}
	}
	
	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		
		this.gameObject.transform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		this.gameObject.transform.localPosition = new Vector3(0f,0f, 0);
		this.GUILevel = 14;
		time = mLeaveTime;
	}
	
	public void UpdateData(TaskDialogConfig.TaskDialogObject talkObject)
	{
		time = mLeaveTime;
	}
}
