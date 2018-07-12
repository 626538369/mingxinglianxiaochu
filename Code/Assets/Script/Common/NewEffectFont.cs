using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// New effect font.
/// </summary>
public class NewEffectFont : MonoBehaviour {
	
	private static readonly float	BackgroundMoveTime = 0.1f;
	
	private static readonly float	FlagScaleTime		= 0.15f;
	
	private static readonly float	FontAppearDelayTime	= 0.08f;
	
	private static readonly float	FontScaleTime		= 0.15f;
	
	private static readonly float	FontShakeTime		= 0.2f;

	private static readonly float	FontShakeStrangth	= 1.0f;
	
	private static readonly float	DisappearTime		= 1.0f;

	
	
	/// <summary>
	/// Effect type.
	/// </summary>
	public enum EffectType{
		BeiTouXi,
		DengJiTiSheng,
		JieShouRenWu,
		JunXianTiSheng,
		
		TouXiChengGong,
		WanChengRenWu,
		ZhanDouShengLi,
		ZhanDouPingJu,		
		
		ZhanDouShiBai,
		WuXueTiSheng,
	};
	
	private static readonly string[,] FontStr = 
	{
		{"bei","deng",	"jie",	"jun",	"tou",	"wan",	"zhan2","zhan1","zhan"},
		{"tou","ji",	"shou",	"xian",	"xi",	"cheng","dou2",	"dou1",	"dou"},	
		{"xi",	"ti",	"ren",	"ti",	"cheng","ren",	"sheng","ping",	"shi"},	
		{"",	"sheng","wu",	"sheng","gong",	"wu",	"li",	"ju",	"bai"}
	};
	
	/// <summary>
	/// The normal background.
	/// </summary>
	public GameObject	NormalBackground;
	
	/// <summary>
	/// The failed background.
	/// </summary>
	public GameObject	FailedBackground;
	
	public PackedSprite  mEffectPictrue;
	
	/// <summary>
	/// The font list.
	/// </summary>
	public PackedSprite[]		FontList;
		
	private EZ3DItemManager.EffectEndDelegate	mPlayDoneEvent;
	
	/// <summary>
	/// The sm instance.
	/// </summary>
	private static NewEffectFont	smInstance = null;
	
	/// <summary>
	/// The sm playing.
	/// </summary>
	private static bool				smPlaying	= false;
	
	/// <summary>
	/// The sm buffered type list.
	/// </summary>
	private static List<NewEffectFontBuffer>	smBufferedTypeList = new List<NewEffectFontBuffer>();
	
	/// <summary>
	/// The type of the m curr.
	/// </summary>
	private EffectType	mCurrType;
	
	class NewEffectFontBuffer{
		public EffectType 							type;
		public EZ3DItemManager.EffectEndDelegate	dele;
	}
	
	void Awake(){
		DontDestroyOnLoad(gameObject);
		
		if(Globals.Instance.MGUIManager != null){
			transform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		}
		
		StopAnimation();
	}
	
	// Use this for initialization
	void Update () {
		if(!smPlaying && smBufferedTypeList.Count != 0){
			NextEffectFont();
		}
	}
	
	/// <summary>
	/// Show the specified type.
	/// </summary>
	/// <param name='type'>
	/// Type of effect
	/// </param>
	public static void Show(EffectType type,EZ3DItemManager.EffectEndDelegate endDele = null){
		NewEffectFontBuffer tBuffer = new NewEffectFontBuffer();
		tBuffer.type	= type;
		tBuffer.dele	= endDele;
		
		smBufferedTypeList.Insert(0,tBuffer);
		
		if(smInstance == null){
			smInstance = (NewEffectFont)Instantiate(Globals.Instance.M3DItemManager.NewEffectFontPrefab);
		}
	}
	
	private void ShowImpl(){
		
		smPlaying = true; 
		
		//GameObject tFlagBg ;
		// prepare the background
		if(mCurrType == EffectType.ZhanDouShiBai ){
			//NormalBackground.SetActiveRecursively(false);
			mEffectPictrue.PlayAnim("ZhandouShibai");
			//tFlagBg		= FailedBackground;
		}else if (mCurrType == EffectType.ZhanDouShengLi ) {
			mEffectPictrue.PlayAnim("ZhandouChengong");
			//FailedBackground.SetActiveRecursively(false);
			//tFlagBg		= NormalBackground;
		}
		else if (mCurrType == EffectType.ZhanDouPingJu)
		{
			mEffectPictrue.PlayAnim("ZhandouPingju");
		}
		else if (mCurrType == EffectType.DengJiTiSheng)
		{
			mEffectPictrue.PlayAnim("DengJiTiSheng");
		}
		else if (mCurrType == EffectType.WanChengRenWu)
		{
			mEffectPictrue.PlayAnim("WanChengRenWu");
		}
		
		//tFlagBg.SetActiveRecursively(true);
		//tFlagBg.transform.localScale = new Vector3(0,1,1);
				
		// hide the font
		//foreach(PackedSprite ps in FontList){
			//ps.transform.localScale = Vector3.one;
	//	}
		
		transform.localPosition = new Vector3(-(Screen.width + GetComponent<PackedSprite>().width * gameObject.transform.localScale.x) / 2,
												0,GUIManager.GUI_NEAREST_Z + 10);
		
		iTween.MoveTo(gameObject,iTween.Hash("position",
		new Vector3(0,transform.localPosition.y,transform.localPosition.z),"time",BackgroundMoveTime,"isLocal",true),null,
		delegate(){
			StartDisappear();
			//iTween.ScaleTo (tFlagBg,iTween.Hash("scale",Vector3.one,"time",FlagScaleTime),null,delegate(){
				//PrepareFont(mCurrType);
			//});
		});
	}
	
	//! subfunction
	private void PrepareFont(EffectType type){
		int tFontNum = FontList.Length;
		
		if(type == EffectType.BeiTouXi){
			int tInterval = 10;
			
			// reposition the font sprite
			float y = FontList[0].transform.localPosition.y;
			float z = FontList[0].transform.localPosition.z;
			
			FontList[0].transform.localPosition = new Vector3(-(FontList[0].width + tInterval),y,z);
			FontList[1].transform.localPosition = new Vector3(0,y,FontList[1].transform.localPosition.z);
			FontList[2].transform.localPosition = new Vector3((FontList[2].width + tInterval),y,z);
			
			FontList[3].transform.localScale = Vector3.one;
			
			tFontNum = 3;
		}
				
		Vector3 tStartScale;
		Vector3 tEndScale = Vector3.one;
		
		if(type == EffectType.ZhanDouShengLi){
			tStartScale = new Vector3(2,2,1);
		}else{
			tStartScale = new Vector3(0,0,1);
		}
		
		// set the font animation
		for(int i = 0;i < tFontNum;i++){
			
			int FontIdx = i;
			
			FontList[FontIdx].PlayAnim(FontStr[FontIdx,(int)type]);
			FontList[FontIdx].transform.localScale = tStartScale;

			iTween.EventDelegate tCompleteEvent = delegate(){
				if(type == EffectType.ZhanDouShengLi){				
					iTween.ShakePosition(FontList[FontIdx].gameObject,new Vector3(FontShakeStrangth,FontShakeStrangth,0),FontShakeTime);
				}
				
				if(FontIdx == tFontNum - 1){
					StartDisappear();
				}
			};
			
			iTween.ScaleTo(FontList[FontIdx].gameObject,iTween.Hash("scale",tEndScale,"easetype",iTween.EaseType.easeInQuad,"time",FontScaleTime,"delay",i * FontAppearDelayTime),
			delegate(){	
				if(FontList[FontIdx].IsHidden()){
					FontList[FontIdx].transform.localScale = Vector3.zero;
				}				
			},tCompleteEvent);
		}
	}
	
	//! fade out all object
	private void StartDisappear(){
		
		EZAnimation.CompletionDelegate dele = delegate(EZAnimation ani){
			if(mPlayDoneEvent != null){
				try{
					mPlayDoneEvent();
				}catch(System.Exception e){
					Debug.LogError(e.GetType().Name + " " + e.Message);
				}
			}
			
			try{
				NextEffectFont();
			}catch(System.Exception e){
				Debug.LogError(e.Message + "\n" + e.StackTrace);
			}			
		};
		
		if(mCurrType == EffectType.TouXiChengGong){
			//  TouXiChengGong NewEffectFont must move to battle buffer position
			//
			GUIBattle battle = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
			if(battle != null && battle.GetBufferInfoRoot() != null){
				UIButton[] btns = battle.GetBufferInfoRoot().GetComponentsInChildren<UIButton>();
				foreach(UIButton btn in btns){
					
					if(btn.Data is BuffData){
						// get the buffData
						//
						BuffData TouXiBuf = (BuffData)btn.Data;
						
						if(TouXiBuf.ID == 1429999999){ // is right touxiBuffer
							iTween.MoveTo(gameObject,btn.transform.position,DisappearTime);
							
							EZAnimation.CompletionDelegate backDele = dele;
							iTween.ScaleTo(gameObject,new Vector3(0,0,1),DisappearTime);
							break;
						}		
					}
				}
			}
		}
		
		SpriteRoot[] tSprite = GetComponentsInChildren<SpriteRoot>();		
		
		for(int i = 0;i < tSprite.Length;i++){
			FadeSpriteAlpha tSpriteFade = FadeSpriteAlpha.Do(tSprite[i],EZAnimation.ANIM_MODE.To,Color.clear,EZAnimation.linear,DisappearTime,FontShakeTime,null,
															i == tSprite.Length - 1?dele:null);
		}
	}
	
	private void StopAnimation(){
				
		iTween.Stop(gameObject);
		PackedSprite[] pss = GetComponentsInChildren<PackedSprite>();
		foreach(PackedSprite ps in pss){
			iTween.Stop(ps.gameObject);
		}
		
		SpriteRoot[] tSprite = GetComponentsInChildren<SpriteRoot>();
		foreach(SpriteRoot sr in tSprite){
			EZAnimator.instance.Stop(sr);
			sr.Color = Color.white;
		}
				
		// restore the default scale
		if(Globals.Instance.MGUIManager != null){
			gameObject.transform.localScale = new Vector3(Globals.Instance.MGUIManager.widthRatio,Globals.Instance.MGUIManager.heightRatio,1);
		}else{
			gameObject.transform.localScale = new Vector3(Screen.width / GUIManager.DEFAULT_SCREEN_WIDTH,Screen.height / GUIManager.DEFAULT_SCREEN_HEIGHT,1);
		}
		
		transform.localPosition = new Vector3(0,30000,0);
		
		smPlaying = false;
	}
	
	private static void NextEffectFont(){
		
		smInstance.StopAnimation();
		
		if(smBufferedTypeList.Count != 0){
			
			int tIdx = 0;
			
			smInstance.mCurrType 		= smBufferedTypeList[tIdx].type;		
			smInstance.mPlayDoneEvent	= smBufferedTypeList[tIdx].dele;
						
			smBufferedTypeList.RemoveAt(tIdx);
			
			smInstance.ShowImpl();
			
		}
	}
	
	
	/// <summary>
	/// Destroies all effect font.
	/// </summary>
	public static void DestroyAllEffectFont(){
		
		foreach(NewEffectFontBuffer buff in smBufferedTypeList){
			
			if(buff.dele != null){
				try{
					buff.dele();
				}catch(System.Exception e){
					Debug.LogError(e.Message + "\n" + e.StackTrace);
				}
			}
		}
		
		smBufferedTypeList.Clear();
		
		if(smInstance != null){
			smInstance.StopAnimation();
		}		
	}
}
