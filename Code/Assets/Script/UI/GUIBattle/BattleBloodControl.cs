using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleBloodControl
{
	public PackedSprite effectFontPreb;
	public SpriteText textSkillPreb;
	public PackedSprite numberPreb;
	public UISlider progressBarBlood;
	
	//effect font
	public void EmitEffectFont(Transform parent,Vector3 position ,Vector3 offset,BattleEffectFont.EffectType type , int val, string skillName)
	{
		BattleEffectFont effectFont = new BattleEffectFont(parent, 
			effectFontPreb, textSkillPreb, numberPreb, 
			position, offset, type, val, skillName);
	}
	
	//blood strip
	public static void DestoryBloodStrip(long shipID)
	{
		if(_bloodStrips.ContainsKey(shipID))
		{
			BloodStrip bloodStrip = _bloodStrips[shipID];
			bloodStrip.Destory();
			_bloodStrips.Remove(shipID);
		}
	}
	
	public bool DisplayBloodStrip(long shipID, float val, Vector3 position, Vector3 offset)
	{
		// Profiler.BeginSample("BattleBloodControl.DisplayBloodStrip");
		
		//_bloodStrips
		if(_bloodStrips.ContainsKey(shipID))
		{
			//Debug.Log("Update Blood Strip");
			BloodStrip bloodStrip = _bloodStrips[shipID];
			bloodStrip.UpdateBloodStripValue(val, position, offset);
		}
		else
		{
			// Debug.Log("Create Blood Strip");
			BloodStrip bloodStrip = new BloodStrip(progressBarBlood,shipID, val, position, offset);
			_bloodStrips.Add(shipID, bloodStrip);
		}
		
		// Profiler.EndSample();		
		return true;
	}

	
	private static Dictionary<long, BloodStrip> _bloodStrips = new Dictionary<long, BloodStrip>();
}
