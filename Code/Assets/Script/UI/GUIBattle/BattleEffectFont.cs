using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EZGUI.Common;

public class BattleEffectFont
{
	private PackedSprite effectFontPreb;
	private SpriteText textSkillPreb;
	private PackedSprite numberPreb;
	
	const float AttackNumAnimPosDelayTime = 0.0f;
	const float AttackNumAnimPosDurationTime = 1.0f;
	const float AttackNumRiseUpHeight = 45.0f;
	
	const float AnimScaleDurationTime = 0.3f;
	
	const float AnimAlphaDelayTime = 1.2f;
	const float AnimAlphaDurationTime = 1.5f;
	
	public enum EffectType
	{
		CRIT,
		DODGE,
		SKILL,
		CRIT_SKILL,
		NUMBER_RED,
		NUMBER_GREEN,
		NUMBER_GRAY
	}

	//construct for battle effect font 
	public BattleEffectFont (Transform parent,PackedSprite effectFont,SpriteText textSkill,PackedSprite number,Vector3 position, Vector3 offset, EffectType type, int val, string skillName)
	{
		// _mainCamera = Globals.Instance.MSceneManager.mMainCamera;
		// _uICamera = Globals.Instance.MGUIManager.MGUICamera;
		
		this.effectFontPreb = effectFont;
		this.textSkillPreb = textSkill;
		this.numberPreb = number;

		this.ReleaseEffect (parent, position, type, val, skillName, offset);
	}
	
	//crit font
	private void ReleaseEffect (Transform parent, Vector3 position, EffectType type, int val, string skillName, Vector3 offset)
	{
		// Vector3 warshipViewPosition = _mainCamera.WorldToViewportPoint(position);
		// Vector3 uiOffset = new Vector3((warshipViewPosition.x - 0.5f) * Screen.width,(warshipViewPosition.y - 0.5f) * Screen.height,100);

		_gobjCrit = new GameObject ("BattleCritFont");
		_gobjCrit.transform.position = position;
		_gobjCrit.transform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		_gobjCrit.transform.localScale = new Vector3 (Globals.Instance.MGUIManager.widthRatio,
			Globals.Instance.MGUIManager.heightRatio,1);
		// _gobjCrit.transform.localPosition = uiOffset + offset;
		
		PackedSprite effectFont = null;
		SpriteText textSkill = null;
		PackedSprite number = null;
		
		switch (type) 
		{
		case EffectType.CRIT:
			// effectFont = GameObject.Instantiate(effectFontPreb) as PackedSprite;
			// effectFont.transform.parent = _gobjCrit.transform;
			// effectFont.transform.localPosition = new Vector3 (0, 0, 0);	
			// effectFont.PlayAnim ("RedCrit");
			// AnimateScale.Do (_gobjCrit, EZAnimation.ANIM_MODE.To, Vector3.one * 2, EZAnimation.linear, AnimScaleDurationTime, 0f, null, null);
			// FadeSpriteAlpha.Do (effectFont, EZAnimation.ANIM_MODE.To, Color.clear, EZAnimation.linear, AnimAlphaDurationTime, AnimAlphaDelayTime, null, DestroyEffectObj);
			break;
		case EffectType.DODGE:
			effectFont = GameObject.Instantiate(effectFontPreb) as PackedSprite;
			effectFont.transform.parent = _gobjCrit.transform;
			effectFont.transform.localPosition = new Vector3 (0, 0, 0);	
			effectFont.transform.localScale = Vector3.one;
			effectFont.PlayAnim ("Shan");
			
			AnimateScale.Do (_gobjCrit, EZAnimation.ANIM_MODE.To, Vector3.one * 2, EZAnimation.linear, AnimScaleDurationTime, 0f, null, null);
			FadeSpriteAlpha.Do (effectFont, EZAnimation.ANIM_MODE.To, Color.clear, EZAnimation.linear, AnimAlphaDurationTime, AnimAlphaDelayTime, null, DestroyEffectObj);
			
			break;
		case EffectType.SKILL:
			//delay = 0;
			//duration = 6;
			textSkill = GameObject.Instantiate(textSkillPreb) as SpriteText;
			textSkill.transform.parent = _gobjCrit.transform;
			textSkill.transform.localPosition = new Vector3(0,0,0);
			textSkill.transform.localScale = Vector3.one;
			textSkill.Text = skillName;
			//textSkill.SetColor(new Color32(255,129,54,255));
			// textSkill.SetColor(new Color32(255,255,0,255));
			textSkill.SeaColor = SeaClientColorType.DarkRed210000005;
			//textSkill.SetColor(Color.magenta);
			textSkill.SetCharacterSize(40);
			FadeText.Do(textSkill, EZAnimation.ANIM_MODE.To, Color.clear, EZAnimation.linear, AnimAlphaDurationTime, AnimAlphaDelayTime, null, DestroyEffectObj);
			break;
		case EffectType.CRIT_SKILL:
			//delay = 0;
			//duration = 6;
			// effectFont = GameObject.Instantiate(effectFontPreb) as PackedSprite;
			// effectFont.transform.parent = _gobjCrit.transform;
			// effectFont.transform.localPosition = new Vector3 (0, 20, 0);	
			// effectFont.PlayAnim ("RedCrit");
			// //DoAnimationFade (effectFont, DestroyEffectObj);
			// AnimateScale.Do (_gobjCrit, EZAnimation.ANIM_MODE.To, Vector3.one * 1.5f, EZAnimation.linear, AnimScaleDurationTime, 0f, null, null);
			// FadeSpriteAlpha.Do (effectFont, EZAnimation.ANIM_MODE.To, Color.clear, EZAnimation.linear, AnimAlphaDurationTime, AnimAlphaDelayTime, null, DestroyEffectObj);
			
			textSkill = GameObject.Instantiate(textSkillPreb) as SpriteText;
			textSkill.transform.parent = _gobjCrit.transform;
			textSkill.transform.localPosition = new Vector3(0,0,0);
			textSkill.transform.localScale = Vector3.one;
			textSkill.Text = skillName;
			
			// textSkill.SetColor(new Color32(255,255,0,255));
			textSkill.SeaColor = SeaClientColorType.DarkRed210000005;
			//textSkill.SetColor(Color.magenta);
			textSkill.SetCharacterSize(40);
			FadeText.Do (textSkill, EZAnimation.ANIM_MODE.To, Color.clear, EZAnimation.linear, AnimAlphaDurationTime, AnimAlphaDelayTime, null, DestroyEffectObj);
			
			break;
		case EffectType.NUMBER_GRAY:
			this.PlayAttackNumber (type, "NumMinus", "NumRed", val);
			break;
		case EffectType.NUMBER_RED:
			this.PlayAttackNumber (type, "NumMinus", "NumRed", val);
			break;
		case EffectType.NUMBER_GREEN:
			this.PlayAttackNumber (type, "GreenPlus", "NumGreen", val);
			break;
		}
		
		if(type == EffectType.CRIT || type == EffectType.CRIT_SKILL || type == EffectType.NUMBER_RED){
			iTween.ShakePosition(_gobjCrit, new Vector3(5,5,0), AnimAlphaDurationTime);
			// _gobjCrit.transform.localScale = new Vector3(2.5f,2.5f,1);
		}
	}
	
	public void PlayAttackNumber (EffectType type, string animationName, string animationStr, int val)
	{
		delay = AttackNumAnimPosDelayTime;
		duration = AttackNumAnimPosDurationTime;
			
		val = Mathf.Abs (val);
		int length = val.ToString ().Length + 1;
		
		// Scale 0.5f
		float scaleFactor = 1.0f;
		if (type == EffectType.NUMBER_GREEN
			|| type == EffectType.NUMBER_GRAY)
		{
			scaleFactor = 0.5f;
		}
		
		float singleWidth = numberPreb.width;
		singleWidth *= scaleFactor;
		float width = length * singleWidth;
		
		if (!animationName.Equals("GreenPlus"))
		{
			PackedSprite number = GameObject.Instantiate(numberPreb) as PackedSprite;
			number.width *= scaleFactor;
			number.height *= scaleFactor;
			
			number.transform.parent = _gobjCrit.transform;
			number.transform.localPosition = new Vector3 (-width / 2, 0, 0);
			number.transform.localScale = Vector3.one;
			
			number.SetAnchor (SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT);
			number.PlayAnim (animationName);
			FadeSpriteAlpha.Do (number, EZAnimation.ANIM_MODE.To, Color.clear, EZAnimation.linear, AnimAlphaDurationTime, AnimAlphaDelayTime, null, DestroyEffectObj);
			AnimatePosition.Do(_gobjCrit.gameObject, EZAnimation.ANIM_MODE.To,_gobjCrit.transform.localPosition + new Vector3 (0, AttackNumRiseUpHeight, 0),EZAnimation.linear, duration,delay,null,DestroyEffectObj);
		}
		
		for (int i = 0; i < length - 1; i++)
		{
			int currNum = val / (int)Mathf.Pow (10, length - 2 - i) % 10;
			
			PackedSprite number2 = GameObject.Instantiate(numberPreb) as PackedSprite;
			number2.width *= scaleFactor;
			number2.height *= scaleFactor;
			number2.transform.parent = _gobjCrit.transform;
			number2.transform.localPosition = new Vector3 (-width / 2 + singleWidth * (i + 1), 0, 0);
			number2.transform.localScale = Vector3.one;
			
			number2.SetAnchor (SpriteRoot.ANCHOR_METHOD.MIDDLE_LEFT);
			number2.PlayAnim (animationStr + currNum);
			FadeSpriteAlpha.Do (number2, EZAnimation.ANIM_MODE.To, Color.clear, EZAnimation.linear, AnimAlphaDurationTime, AnimAlphaDelayTime, null, null);
			AnimatePosition.Do(_gobjCrit.gameObject, EZAnimation.ANIM_MODE.To,_gobjCrit.transform.localPosition + new Vector3 (0, AttackNumRiseUpHeight, 0),EZAnimation.linear, duration,delay,null,DestroyEffectObj);
		}
	}
	//destroy the effect font
	public void DestroyEffectObj (EZAnimation obj)
	{
		GameObject.Destroy(_gobjCrit);
	}
	//----
	private Camera _mainCamera;
	private Camera _uICamera;
	private Transform _parent;
	private Vector3 _offset;
	private GameObject _gobjCrit;
	private float delay = 3;
	private float duration = 5;
}
