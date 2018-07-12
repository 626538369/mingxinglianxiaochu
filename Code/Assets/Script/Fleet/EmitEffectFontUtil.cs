using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmitEffectFontUtil : MonoBehaviour 
{
	public void EmitEffectFont(GameObject thisGameObject, GameData.BattleGameData.AttackedState currentAttackState, int currentEffectFontDamage)
	{
		EmitEffectFontData emitEffectFontData = new EmitEffectFontData();
		emitEffectFontData._currBeAttackedState = currentAttackState;
		emitEffectFontData._currentEffectFontDamage = currentEffectFontDamage;
		emitEffectFontData._thisGameObject = thisGameObject;
		
		_emitBeAttackedFontDataList.Add(emitEffectFontData);
	}
	
	public void EmitEffectFont(GameObject thisGameObject, GameData.BattleGameData.AttackState attackState, string skillName)
	{
		EmitEffectFontData emitEffectFontData = new EmitEffectFontData();
		
		emitEffectFontData._currAttackState = attackState;
		emitEffectFontData._thisGameObject = thisGameObject;
		emitEffectFontData.skillName = skillName;
		
		_emitAttackFontDataList.Add(emitEffectFontData);
	}
	
	void Update()
	{
		if (null != _emitBeAttackedFontDataList && _emitBeAttackedFontDataList.Count > 0)
		{
			_emitEffectFontTime -= Time.deltaTime;
			if(_emitEffectFontTime < 0f)
			{
				EmitEffectFontData emitEffectFontDatat = _emitBeAttackedFontDataList[0];
				this.EmitBeAttackedFontInvoke(emitEffectFontDatat);
				
				_emitBeAttackedFontDataList.RemoveAt(0);
				_emitEffectFontTime = EMIT_EFFECT_FONT_TIME_CONS;
			}
		}
		
		if (null != _emitAttackFontDataList && _emitAttackFontDataList.Count > 0)
		{
			EmitEffectFontData emitEffectFontDatat = _emitAttackFontDataList[0];
			this.EmitAttackFontInvoke(emitEffectFontDatat);
			
			_emitAttackFontDataList.RemoveAt(0);
		}
	}
	
	void OnDestroy()
	{
		_emitBeAttackedFontDataList.Clear();
		_emitAttackFontDataList.Clear();
	}
	
	private void EmitAttackFontInvoke(EmitEffectFontData effectFontData)
	{
		BattleBloodControl bloodControl = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattleMain>().bloodControl;
		if(bloodControl == null) 
		{
			Debug.Log("bloodControl is null");
			return;
		}
		
		GameData.BattleGameData.AttackState attackState = effectFontData._currAttackState;
		GameObject gameObject = effectFontData._thisGameObject;
		
		Vector3 worldPos = gameObject.transform.position;
		worldPos.z += FightCellSlot.EffectSkillNameOffset;
		Vector3 offset = new Vector3(0f, 20f, 0f);
		switch(attackState){
			case GameData.BattleGameData.AttackState.NORMAL_ATTACK:
			case GameData.BattleGameData.AttackState.SKILL_ATTACK:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.SKILL, 0, effectFontData.skillName);
				break;	
			case GameData.BattleGameData.AttackState.NORMAL_ATTACK_CRIT:
			case GameData.BattleGameData.AttackState.SKILL_ATTACK_CRIT:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.CRIT_SKILL, 0, effectFontData.skillName);
				break;
			case GameData.BattleGameData.AttackState.TREAT_SKILL_ATTACKED:
			case GameData.BattleGameData.AttackState.REDUCE_DAMAGE_SKILL_ATTACKED:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.SKILL, 0, effectFontData.skillName);
				break;
			default:
				Debug.LogError("Error AttackState EmitAttackFontInvoke " + attackState);
				break;
		}
	}
	
	private void EmitBeAttackedFontInvoke(EmitEffectFontData effectFontData)
	{
		GameData.BattleGameData.AttackedState currentAttackState = effectFontData._currBeAttackedState;
		GameObject gameObject = effectFontData._thisGameObject;
		int damage = effectFontData._currentEffectFontDamage;
			
		BattleBloodControl bloodControl = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattleMain>().bloodControl;
		if(bloodControl == null) 
		{
			Debug.Log("bloodControl is null");
			return;
		}
		
		// tzz modified for Refactoring
		Vector3 worldPos = gameObject.transform.position;
		worldPos.z += FightCellSlot.EffectDamageBloodOffset;
		Vector3 offset = new Vector3(0f, 20f, 0f);
		switch(currentAttackState){
			case GameData.BattleGameData.AttackedState.DODGE:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.DODGE, damage, "");
				break;
			case GameData.BattleGameData.AttackedState.NORMAL_ATTACKED:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.NUMBER_GRAY, damage, "");
				break;
			case GameData.BattleGameData.AttackedState.NORMAL_CRIT:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.NUMBER_RED, damage, "");
				break;
			case GameData.BattleGameData.AttackedState.SKILL_ATTACKED:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.NUMBER_GRAY, damage, "");
				break;
			case GameData.BattleGameData.AttackedState.SKILL_CRIT:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.NUMBER_RED, damage, "");
				break;
			case GameData.BattleGameData.AttackedState.TREAT_SKILL_ATTACKED:
			case GameData.BattleGameData.AttackedState.REDUCE_DAMAGE_SKILL_ATTACKED:
				bloodControl.EmitEffectFont(gameObject.transform, worldPos, offset, BattleEffectFont.EffectType.NUMBER_GREEN, damage, "");
				break;
			default:
				Debug.LogError("Error AttackedState EmitBeAttackedFontInvoke " + currentAttackState);
				break;
		}
	}	
	
	public float EMIT_EFFECT_FONT_TIME_CONS = 0.4f;
	private float _emitEffectFontTime = 0f;
	
	private List<EmitEffectFontData> _emitBeAttackedFontDataList = new List<EmitEffectFontData>();
	private List<EmitEffectFontData> _emitAttackFontDataList = new List<EmitEffectFontData>();
	
	private class EmitEffectFontData
	{
		public GameData.BattleGameData.AttackedState _currBeAttackedState;
		public int _currentEffectFontDamage = 0;
		public GameObject _thisGameObject;
		
		public GameData.BattleGameData.AttackState _currAttackState;
		public string skillName = "";
	}
}
