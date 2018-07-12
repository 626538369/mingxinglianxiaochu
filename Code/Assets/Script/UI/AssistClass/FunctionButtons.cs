using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FunctionButtons : MonoBehaviour 
{
	public GameObject FunctionList;
	public UIButton ButtonGongsi;
	public UIButton ButtonHaoyou;
	public UIButton ButtonHuanzhuang;
	public UIButton ButtonShangdian;
	public UIButton ButtonFuzhaungdian;
	public UIButton ButtonYishujia;
	
	public GameObject ChatBubbleTween;
	public UIButton ChatBubbleLabel;
	public UILabel mLabel;
	public UIButton ChatButton;
	
	public UIButton OpenButtom;
	public UIButton ButtonNull;
	private float time = 0;
	ISubscriber ChatMessagetUpdate = null;
	private bool  mIsBottomFunctionShowValue = false;
	void Awake()
	{
		ChatBubbleLabel.transform.localScale = Vector3.zero;
		FunctionList.transform.localPosition = new Vector3(1600,FunctionList.transform.localPosition.y,FunctionList.transform.localPosition.z);
		UIEventListener.Get(ButtonGongsi.gameObject).onClick += OnClickButtonGongsi;
		UIEventListener.Get(ButtonHaoyou.gameObject).onClick += OnClickButtonHaoyou;
		UIEventListener.Get(ButtonHuanzhuang.gameObject).onClick += OnClickButtonHuanzhuang;
		UIEventListener.Get(ButtonShangdian.gameObject).onClick += OnClickButtonShangdian;
		UIEventListener.Get(ButtonFuzhaungdian.gameObject).onClick += OnClickButtonFuzhaungdian;
		UIEventListener.Get(ButtonYishujia.gameObject).onClick += OnClickButtonYishujia;
		UIEventListener.Get(ChatButton.gameObject).onClick += OnClickChatButton;
		UIEventListener.Get(OpenButtom.gameObject).onClick += OnClickOpenButtom;
		UIEventListener.Get(ButtonNull.gameObject).onClick += OnClickButtonNull;
		
		FunctionList.transform.localScale = Vector3.zero;
		ButtonNull.transform.localScale = Vector3.zero;
		OpenButtom.transform.localScale = Vector3.one;
		
	}
	
	void Start()
	{
		UIEventListener.Get(ChatBubbleLabel.gameObject).onClick += OnClickChatBubble;
		ChatMessagetUpdate = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.Event_CHATMESSAGE_UPDATE);
		ChatMessagetUpdate.Handler = delegate(object[] args)
		{
			UpdateMessage();
			
		};UpdateMessage();
	}
	void Update()
	{

	}
	void UpdateMessage()
	{
		
	}
	
	void MoveScrollListDownLeft(bool is2Show)
	{
		if(is2Show) 
		{
			FunctionList.transform.localScale = Vector3.one;
			TweenPosition.Begin(ChatBubbleTween.gameObject,0.5f,new Vector3(0,240,ChatBubbleTween.transform.localPosition.z));
			NGUITools.SetTweenActive(FunctionList.gameObject,is2Show,delegate() {
				
			});
		}
		else 
		{
			TweenPosition.Begin(ChatBubbleTween.gameObject,0.5f,new Vector3(0,0,ChatBubbleTween.transform.localPosition.z));
			NGUITools.SetTweenActive(FunctionList.gameObject,is2Show,delegate() {
				FunctionList.transform.localScale = Vector3.zero;
			});
		}
	}
	
	public void OnClickButtonGongsi(GameObject obj)
	{
		
		if(Globals.Instance.MGameDataManager.MActorData.BasicData.Level < 20&&Globals.Instance.MTeachManager.IsOpenTeach)
		{
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(15003,true,2.0f);
		}
		else
		{
			
		}
		
	}
	public void OnClickButtonHaoyou(GameObject obj)
	{

	}
	public void OnClickButtonHuanzhuang(GameObject obj)
	{
		Globals.Instance.MTaskManager.NeedBone = false;
		Globals.Instance.MGUIManager.CreateWindow<GUIChangeCloth>(delegate(GUIChangeCloth Change)
		{
		});
	}

	public void OnClickButtonShangdian(GameObject obj)
	{

	}
	public void OnClickButtonFuzhaungdian(GameObject obj)
	{
		PlayerData playerData =  Globals.Instance.MGameDataManager.MActorData;	
		GUIRadarScan.Show();
		NetSender.Instance.C2GSRequestShopItems(510,(int)playerData.BasicData.Gender);
	}
	public void OnClickButtonYishujia(GameObject obj)
	{

	}
	public void OnClickChatBubble(GameObject obj)
	{
		
	}
	public void OnClickChatButton(GameObject obj)
	{
		
	}
	
	public void OnClickOpenButtom(GameObject obj)
	{
		MoveScrollListDownLeft(true);
		ButtonNull.transform.localScale = Vector3.one;
		obj.transform.localScale = Vector3.zero;
		mIsBottomFunctionShowValue = true;
	}
	public void OnClickButtonNull(GameObject obj)
	{
		MoveScrollListDownLeft(false);
		ButtonNull.transform.localScale = Vector3.zero;
		OpenButtom.transform.localScale = Vector3.one;
		mIsBottomFunctionShowValue = false;
	}
	
	void OnDestroy()
	{

	}
	
 	public bool IsBottomFunctionShow()
	{
		return mIsBottomFunctionShowValue;
	}
}