using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallGameSlid : MonoBehaviour 
{
	public delegate void OnTimeFinshedEvent(Vector3 pos);
	[HideInInspector] public  event SmallGameSlid.OnTimeFinshedEvent TimeFinshedEvent = null;//时间结束//
	
	public delegate void IsSucceed(Vector3 pos);
	[HideInInspector] public  event SmallGameSlid.IsSucceed Succeed = null;//操作成功//
	
	public delegate void StartSlid(GameObject obj,bool pressed);
	[HideInInspector] public  event SmallGameSlid.StartSlid SlidStart = null;//操作开始//
	
	public delegate void OnSlidFail();
	[HideInInspector] public  event SmallGameSlid.OnSlidFail OperateSlidFail = null;//已经开始操作，但是时间结束//
	
	public UIImageButton Start;
	public UIImageButton End;
	private bool mIsPressed = false;
	public  TweenScale scall;
	public UISprite SlidSprite;
	private bool mPressedtoUp = false;
	[HideInInspector] public int Index;
	void Awake()
	{
		UIEventListener.Get(Start.gameObject).onPress += OnPressStart;
		UIEventListener.Get(End.gameObject).onDrop += onDropEnd;
		EventDelegate.Add(scall.onFinished, Fail);
		
		NGUITools.SetActive(SlidSprite.gameObject,false);
	}
	
	void OnDestroy()
	{
	
	}
	void Update()
	{	
		if(NGUITools.GetActive(SlidSprite.gameObject) != mIsPressed)
			NGUITools.SetActive(SlidSprite.gameObject,mIsPressed);
		if(true == mIsPressed)
		{
			Vector3 pos = Input.mousePosition;
			Camera camera = Globals.Instance.MGUIManager.MGUICamera;
			pos = camera.ScreenToWorldPoint(pos);
			SlidSprite.transform.position = new Vector3 (pos.x,pos.y,SlidSprite.transform.position.z);
		}
	}
	void LateUpdate () 
	{
		if(true == mPressedtoUp)
		{
			//TimeFinshedEvent(Vector3.zero);
			if(null != OperateSlidFail)
				OperateSlidFail();
			GameObject.Destroy(this.gameObject);
		}
	}
	private void  OnPressStart(GameObject obj,bool pressed)
	{
		if(null != SlidStart)
			SlidStart(obj,pressed);
		mIsPressed = pressed;
		if(false == pressed)
		{
			mPressedtoUp = true;
		}	
	}
	public void onDropEnd(GameObject go, GameObject draggedObject)
	{
		
		if(draggedObject ==  Start.gameObject)
		{
			if(null != Succeed)
				Succeed(go.transform.parent.parent.localPosition);
			if(null != this)
				GameObject.Destroy(this.gameObject);
		}	
	}
	public void Fail()
	{
		if(null != TimeFinshedEvent)
		{
			if(false == mIsPressed)
			{
				TimeFinshedEvent(TweenScale.current.transform.parent.localPosition);
			}
			else
			{
				if(null != OperateSlidFail)
					OperateSlidFail();
			}	
		}
		if(null != this)
			GameObject.Destroy(this.gameObject);
	}

}