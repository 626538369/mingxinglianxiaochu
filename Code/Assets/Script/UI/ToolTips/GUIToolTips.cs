using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIToolTips : GUIWindow 
{
	//---------------------------------------------------------------------------
	public override float GUIWidth
	{
		get { return _mContent.GetSize().x; }
		set 
		{ 
			_mContent.SetSize(value, _mContent.GetSize().y);
		}
	}
	
	public override float GUIHeight
	{
		get { return _mContent.GetSize().y; }
		set 
		{ 
			_mContent.SetSize(_mContent.GetSize().x, value);
		}
	}
	
	public bool IsAutoHide
	{
		get { return _mIsAutoHide; }
		set 
		{ 
			_mIsAutoHide = value; 
		}
	}
	
	public float Duration
	{
		get { return _mDurationTime; }
		set 
		{ 
			_mDurationTime = value; 
		}
	}
	
	//---------------------------------------------------------------------------
	
	//---------------------------------------------------------------------------
	protected override void Awake()
	{
		base.Awake();
		
		_mTipsContentList = new Dictionary<string, TipsContent>();
		
	}
			
	protected override void OnDestroy(){
		base.OnDestroy();
		_mTipsContentList.Clear();
	}
	
	void Update()
	{
		if (!base._mIsLoaded)
		{
			return;
		}
		
		if (IsAutoHide && (Time.realtimeSinceStartup - _mActiveTime) >= _mDurationTime )
		{
			SetVisible(false);
		}
	}
	
	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		
		_mRootTransform = this.transform;
		_mRootTransform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		_mRootTransform.localPosition = new Vector3(0, 0, 10f);
		
		// Traverse all children
		int childCount = this.transform.GetChildCount();
		for (int i = 0; i < childCount; ++i)
		{
			Transform child = this.transform.GetChild(i);
			TipsContent content = child.GetComponent<TipsContent>();
			content.Init();
			
			_mTipsContentList.Add(content.GetType().Name, content);
		}
		
		base.GUILevel = 1;
	}
	
	public void ShowTips(System.Type classType, Vector3 pos, Vector2 size,bool Isopen, params object[] args)
	{
		TweenScale ts = _mRootTransform.Find("SimpleTips").GetComponent<TweenScale>();
		TweenRotation tr = _mRootTransform.Find("SimpleTips").GetComponent<TweenRotation>();
		ts.ResetToBeginning();		
		tr.ResetToBeginning();
		ts.PlayForward();	
		tr.PlayForward();
		
		this.gameObject.SetActiveRecursively(false);
		this.gameObject.active = true;
		_mActiveTime = Time.realtimeSinceStartup;
	
		_mTipsContentList.TryGetValue(classType.Name, out _mContent);
		if (null != _mContent)
		{
			_mContent.SetActiveBG(Isopen);
			_mContent.gameObject.SetActiveRecursively(true);
			_mContent.SetValue(args);
			_mContent.DoLayout(pos, size);
		}
		
		Debug.Log("[GUIToolTips:] Gui position " + this.transform.position);
	}
	
	// GUIToolTips
	private Transform _mRootTransform;
	private UIButton _mGUIToolTips;
	
	private bool _mIsAutoHide = true;
	private float _mActiveTime = 0.0f;
	private float _mDurationTime = 1.5f;
	
	private TipsContent _mContent;
	private TipsLayout _mTipsLayout;
	
	private Dictionary<string, TipsContent> _mTipsContentList;
}