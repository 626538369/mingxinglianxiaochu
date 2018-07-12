using UnityEngine;
using System.Collections;

public class LeaveCopyTrigger : MonoBehaviour 
{
	void Awake()
	{
		Vector3 boxSize = new Vector3(70.0f ,20.0f ,70.0f);
		BoxCollider coll = gameObject.GetComponent<BoxCollider>() as BoxCollider;
		if (null == coll) coll = gameObject.AddComponent<BoxCollider>() as BoxCollider;
		
		coll.size = boxSize;
		coll.isTrigger = true;
		
		// LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.IGNORE_RAYCAST);
		LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.GUI);
	}
	
	public void SetSize(Vector3 center, Vector3 size)
	{
		BoxCollider coll = gameObject.GetComponent<BoxCollider>() as BoxCollider;
		if (null == coll) 
			coll = gameObject.AddComponent<BoxCollider>() as BoxCollider;
		
		// coll.center = center;
		coll.size = size;
		coll.isTrigger = true;
		LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.GUI);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (!this.enabled)
			return;
		
		if (other.CompareTag(TagMaskDefine.GFAN_ACTOR))
		{
			GameStatusManager.Instance.MCopyStatus.DisplayCopyDrops();
			this.enabled = false;
			
			Debug.Log("[LeaveCopyTrigger]: The actor enter the trigger, request leave copy.");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
	}
}
