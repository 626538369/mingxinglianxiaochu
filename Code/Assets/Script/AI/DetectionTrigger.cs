using UnityEngine;
using System.Collections;

public class DetectionTrigger : TriggerBase 
{
	protected override void Awake()
	{
		base.Awake();
		base.radius = 200.0f;
		
		base.DoChange();
		LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.IGNORE_RAYCAST);
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
		LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.DEFAULT);
	}
	
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		

	}
	
	protected override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
	}
}
