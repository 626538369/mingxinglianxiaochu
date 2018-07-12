using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUITaskFontView : GUIWindow {
	
	void Update ()
	{
		if(time > 0)
		{
			time -= Time.deltaTime;
			if(time <= 0)
			{
				if(mIsAutoDestroy)
					Object.Destroy(gameObject);
			}
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
		//TaskDialogConfig taskDialogConfig = Globals.Instance.MDataTableManager.GetConfig<TaskDialogConfig>();
		//Dictionary<int, string> mTaskDialogDic;
		//taskDialogConfig.GeTTaskDialogDic(out mTaskDialogDic);
		//
		//string strsrc = mTaskDialogDic[talkObject.DialogID];
		//CreateText(strsrc, mLeaveTime, true, true);
	}
	
	public void CreateText(string str, float leaveTime, bool isAutoDestroy, bool isCanSkip)
	{
		int strLen = 10;
		string strsrc = str;
		strsrc = strsrc+"                    ";
		for(int i = 0; i < strsrc.Length/strLen; i++)
		{
			char[] deschar = new char[strLen];
			strsrc.CopyTo(i*strLen, deschar, 0, strLen);
			string newstr = new string(deschar, 0, strLen);
			
			GameObject go = transform.Find("Text").gameObject;
			if(go != null)
			{
				GameObject textClone = Instantiate(transform.Find("Text").gameObject) as GameObject;
				textClone.name = "Text"+i;
				textClone.transform.parent = this.gameObject.transform;
				textClone.transform.localPosition = new Vector3(0, -240-30*i, -1);
				SpriteText text = textClone.GetComponent( typeof(SpriteText) ) as SpriteText;
				text.Text = newstr;
			}
		}
		Object.Destroy(transform.Find("Text").gameObject);
		
		time = leaveTime;
		mIsAutoDestroy = isAutoDestroy;
		mIsCanSkip = isCanSkip;
	}
	
	void FingerGestures_OnFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
    {
		if(!mIsAutoDestroy && mIsCanSkip)
			Object.Destroy(gameObject);
    }
	
	void OnEnable()
    {
		FingerGestures.OnFingerUp += FingerGestures_OnFingerUp; 
    }
  
    void OnDisable()
    {
		FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp; 
    }
	
	public float mLeaveTime = 10;
	private bool mIsAutoDestroy = true;
	private bool mIsCanSkip = true;
	private float time;
}
