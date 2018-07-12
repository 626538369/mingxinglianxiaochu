using UnityEngine;
using System.Collections;

[AddComponentMenu ("Gfan/BattleTrigger")]
public class BattleTrigger : TriggerBase 
{
	protected override void Awake()
	{
		base.Awake();
		// base.DoChange();
		
		//LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.IGNORE_RAYCAST);
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
		//LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.DEFAULT);
	}
	
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}
	
	protected override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
	}
	
	// void OnTriggerStay(Collider other)
	// {
	// 	if (!enabled)
	// 		return;
	// 	
	// }
	// 
	// void OnCollisionEnter(Collision collision)
	// {
	// 	if (!enabled)
	// 		return;
	// }
	// 
	// void OnCollisionExit(Collision collision)
	// {
	// 	return;
	// 	
	// 	if (!enabled)
	// 		return;
	// }
}
