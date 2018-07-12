using UnityEngine;
using System.Collections;

public class ChestTrigger : TriggerBase 
{
	public int ChestID
	{
		set { chestID = value; }
	}
	
	protected override void Awake()
	{
		base.Awake();
		
		BoxCollider coll = gameObject.GetComponent<BoxCollider>() as BoxCollider;
		if (null == coll) coll = gameObject.AddComponent<BoxCollider>() as BoxCollider;
		coll.size = boxSize;
		_mCollider = coll;
		_mCollider.isTrigger = true;
		
		// LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.IGNORE_RAYCAST);
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
		// LayerMaskDefine.SetLayerRecursively(this.gameObject, LayerMaskDefine.DEFAULT);
	}
	
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		
		if (other.gameObject.CompareTag(TagMaskDefine.GFAN_ACTOR))
		{
			DropData tDropData = ItemDataManager.Instance.CopyDropData;
			if(tDropData != null)
			{
				tDropData.PickupItemList.Add(chestID);
			}
			
			NetSender.Instance.RequestRandomCopyChest(chestID);
			
			PackedSprite chestAnim = GetComponentInChildren<PackedSprite>() as PackedSprite;
			if (null != chestAnim)
			{
				chestAnim.PlayAnim(1); // Unseal a chest animation
				// chestAnim.PlayAnim("OpenBaoxiang");
				float animLen = chestAnim.GetCurAnim().GetLength();
				
				StartCoroutine(DoWaitForSeconds(animLen + 0.2f, delegate()
				{
					Status2DCopy status2DCopy = (Status2DCopy)GameStatusManager.Instance.MCopyStatus;
					if (null != status2DCopy && null != status2DCopy.StageCopy)
					{
						status2DCopy.StageCopy.DestroyChest(gameObject);
					}
				}));
			}
		}
	}
	
	protected override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
	}
	
	IEnumerator DoWaitForSeconds(float waitTime, iTween.EventDelegate completeDel = null)
	{
		yield return new WaitForSeconds(waitTime);
		if (null != completeDel)
			completeDel();
	}
	
	private int chestID;
}
