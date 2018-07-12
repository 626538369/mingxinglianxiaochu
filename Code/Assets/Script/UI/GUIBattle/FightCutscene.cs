using UnityEngine;
using System.Collections;

public class FightCutscene : MonoBehaviour {
	
	public static readonly float		TopBottomMoveTime		= 0.5f;
	
	public static readonly float		FightBeginScale			= 2.0f;
	
	public static readonly float		FightScaleTime			= 0.3f;
	public static readonly float		FightScale				= 1.6f;
	
	public static readonly float		FightShakeTime			= 0.8f;
	public static readonly float		FightShakeAmount		= 10.0f;
	
	public static readonly float		LightMaskFlashDelayTime	= 0.5f;
	public static readonly float		LightMaskFlashTime		= 0.8f;

	public PackedSprite		Top;
	public PackedSprite		Bottom;
	public PackedSprite		FightLeft;
	public PackedSprite		FightRight;
	public PackedSprite		FightFlash;
	public PackedSprite		LightMask;
	
	private iTween.EventDelegate	mPlayDoneDelegate;
	
	// Use this for initialization
	void Awake () {
		if(Globals.Instance.MGUIManager != null){
			transform.parent			= Globals.Instance.MGUIManager.MGUICamera.transform;
			transform.localPosition		= new Vector3(0,0,GUIManager.GUI_NEAREST_Z + 1);
			transform.localScale		= new Vector3(Globals.Instance.MGUIManager.widthRatio,Globals.Instance.MGUIManager.heightRatio,1);
		}
	}
	
	void Start(){
		// tzz_test_level
		if(Globals.Instance.MGUIManager == null){
			StartCutscene(null);
		}
	}
	
	/// <summary>
	/// Starts the cutscene.
	/// </summary>
	/// <param name='playDoneDelegate'>
	/// Play done delegate.
	/// </param>
	public void StartCutscene(iTween.EventDelegate playDoneDelegate){
		
		if(Globals.Instance.MGUIManager != null){
			transform.localPosition		= new Vector3(0,0,GUIManager.GUI_NEAREST_Z + 1);
		}
		
		Top.transform.localPosition		= new Vector3(0,Top.height,0);
		Bottom.transform.localPosition	= new Vector3(0,-Bottom.height,0);
		LightMask.Color					= new Color(1,1,1,0);
		
		FightFlash.Color				= new Color(1,1,1,0);
		FightLeft.Color					= new Color(1,1,1,0);
		FightRight.Color				= new Color(1,1,1,0);
		
		FightLeft.transform.localScale	= Vector3.one;
		FightRight.transform.localScale	= Vector3.one;
		FightFlash.transform.localScale = Vector3.one;
		
		FightLeft.transform.localPosition	= new Vector3(-(FightLeft.width + Top.width) / 2,0,-1);
		FightRight.transform.localPosition	= new Vector3((FightLeft.width + Top.width) / 2,0,-1);
		FightFlash.transform.localPosition	= new Vector3(0,0,-2);
		
		iTween.MoveTo(Top.gameObject,iTween.Hash("position",new Vector3(0,Top.height / 2,0),"islocal",true,"time",TopBottomMoveTime,"easetype",iTween.EaseType.easeInQuart),null,delegate() {
			
			FadeSpriteAlpha.Do(FightLeft,EZAnimation.ANIM_MODE.To,new Color(1,1,1,0.5f),EZAnimation.quadraticIn,FightScaleTime,0,null,null);			
			FadeSpriteAlpha.Do(FightRight,EZAnimation.ANIM_MODE.To,new Color(1,1,1,0.5f),EZAnimation.quadraticIn,FightScaleTime,0,null,null);
			
			iTween.MoveTo(FightLeft.gameObject,iTween.Hash("position",new Vector3(0,0,-1),"islocal",true,"time",FightScaleTime,"easetype",iTween.EaseType.easeInQuart),null,delegate() {
				
				iTween.ShakePosition(FightLeft.gameObject,new Vector3(5,5,1),LightMaskFlashTime / 2);
				iTween.ShakePosition(FightRight.gameObject,new Vector3(5,5,1),LightMaskFlashTime / 2);
				
				FadeSpriteAlpha.Do(FightFlash,EZAnimation.ANIM_MODE.To,new Color(1,1,1,1),EZAnimation.quadraticIn,LightMaskFlashTime / 2,0,null,null);
								
				iTween.ScaleTo(FightFlash.gameObject,new Vector3(FightScale,FightScale,1),LightMaskFlashTime);
				
				iTween.ScaleTo(FightLeft.gameObject,new Vector3(FightScale,FightScale,1),LightMaskFlashTime);
				iTween.ScaleTo(FightRight.gameObject,new Vector3(FightScale,FightScale,1),LightMaskFlashTime);
				
				FadeSpriteAlpha.Do(LightMask,EZAnimation.ANIM_MODE.To,Color.white,EZAnimation.quarticIn,LightMaskFlashTime,0,null,delegate{
					
					if(playDoneDelegate != null){
						try{
							playDoneDelegate();
						}catch{}
					}
					
					Top.transform.localPosition		= new Vector3(0,3000,0);
					Bottom.transform.localPosition	= new Vector3(0,3000,0);
					
					FightLeft.transform.localPosition	= new Vector3(0,3000,0);
					FightRight.transform.localPosition	= new Vector3(0,3000,0);
					FightFlash.transform.localPosition	= new Vector3(0,3000,0);
					
					FadeSpriteAlpha.Do(LightMask,EZAnimation.ANIM_MODE.To,Color.clear,EZAnimation.quarticIn,LightMaskFlashTime,0,null,delegate{
						transform.localPosition = new Vector3(0,6000,0);
					});
				});	
			});
			
			iTween.MoveTo(FightRight.gameObject,iTween.Hash("position",new Vector3(0,0,-1),"islocal",true,"time",FightScaleTime,"easetype",iTween.EaseType.easeInQuart),null,null);
		});
		
		iTween.MoveTo(Bottom.gameObject,iTween.Hash("position",new Vector3(0,-Bottom.height / 2,0),"islocal",true,"time",TopBottomMoveTime,"easetype",iTween.EaseType.easeInQuart),null,null);
	}
}
