using UnityEngine;
using System.Collections;

public class BloodStrip
{
	public BloodStrip(UISlider progressBlood, long shipID, float val, Vector3 position, Vector3 offset)
	{
		_uICamera = Globals.Instance.MGUIManager.MGUICamera;
		_mainCamera = Globals.Instance.MSceneManager.mMainCamera;
		
		this._shipID = shipID;
		this.Init(progressBlood,val, position, offset);
	}
	
	private void Init(UISlider progressBlood,float val,Vector3 position, Vector3 offset){
		_bloodStrip = GameObject.Instantiate(progressBlood) as UISlider;
		_bloodStrip.gameObject.transform.parent		= Globals.Instance.M3DItemManager.EZ3DItemParent;
		_bloodStrip.gameObject.transform.localScale	= Vector3.one;
		
		// tzz added
		// for Reinforcements label sign
		WarshipL warship = Globals.Instance.MPlayerManager.GetWarship(_shipID);
		if (null != warship){
			if(warship.Property.WarshipIsAttacker && warship.Property.WarshipIsNpc){
				PackedSprite ps = (PackedSprite)GameObject.Instantiate(Globals.Instance.M3DItemManager.ReinforcementsLabelPrefab);
				ps.transform.parent 		= _bloodStrip.gameObject.transform;
			}
		}
		
		UpdateBloodStripValue(val,position,offset);
	}
	
	public void UpdateBloodStripValue(float val, Vector3 position, Vector3 offset)
	{
		// Profiler.BeginSample("BloodStrip.UpdateBloodStripValue");
	
		// tzz fucked
		// Refactor code
		//
		if(GUIManager.IsBehindCamera(position)){
			if(_bloodStrip.gameObject.active){
				_bloodStrip.gameObject.SetActiveRecursively(false);
			}
			return;
		}
		
		if(!_bloodStrip.gameObject.active){
			_bloodStrip.gameObject.SetActiveRecursively(true);
		}
		
		Vector3 sceenPosition = _mainCamera.WorldToViewportPoint(position);
		Vector3 uiSceenPosition = new Vector3((sceenPosition.x - 0.5f) * Screen.width,(sceenPosition.y - 0.5f) * Screen.height,100);
		
		Vector3 locPos = uiSceenPosition + offset;
		locPos.x *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.x;
		locPos.y *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.y;
		
		_bloodStrip.transform.localPosition = locPos;
		_bloodStrip.sliderValue = val;
		
		// Profiler.EndSample();
	}
		
	public void Destory()
	{
		MonoBehaviour.Destroy(_bloodStrip.gameObject);
	}
	
	private long _shipID;
	private UISlider _bloodStrip;
	
	private Camera _uICamera;
	private Camera _mainCamera;
}
