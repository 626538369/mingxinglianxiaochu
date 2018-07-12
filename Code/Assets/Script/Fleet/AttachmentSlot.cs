using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttachPointInfo
{
	public GameObject MParentGameObject;
	public GameObject MAttachGameObject;
	
	public Vector3 MAttachPosition = Vector3.zero;
	public Quaternion MAttachRotation = Quaternion.identity;
	public Vector3 MAttachScale = Vector3.one;
}

/// <summary>
/// Attachment slot. One TagPoint can attach many GameObjects
/// </summary>
public class AttachmentSlot
{
	public AttachmentSlot(GameObject parent)
	{
		MParentGameObject = parent;
	}
	
	public void Initialize()
	{}
	
	public void Release()
	{
		_mAttachPointInfoList.Clear();
	}
	
	public void SetPosition(ref Vector3 localPos)
	{
	}
	
	public void SetRotation(ref Quaternion localRotation)
	{
	}
	
	public GameObject AttachObject(GameObject go, Vector3 relPosition, Quaternion relRotation)
	{
		if ( _mAttachPointInfoList.ContainsKey(go.GetInstanceID()) )
			return null;
			
		AttachPointInfo attachPoint = new AttachPointInfo();
		attachPoint.MParentGameObject = this.MParentGameObject;
		attachPoint.MAttachGameObject = go;
		attachPoint.MAttachPosition = relPosition;
		attachPoint.MAttachRotation = relRotation;
		
		attachPoint.MAttachGameObject.transform.parent = attachPoint.MParentGameObject.transform;
		attachPoint.MAttachGameObject.transform.localPosition = relPosition;
		attachPoint.MAttachGameObject.transform.localRotation = relRotation;
		
		_mAttachPointInfoList.Add(go.GetInstanceID(), attachPoint);
		
		return go;
	}
	
	public GameObject AttachObject(Object ob, Vector3 relPosition, Quaternion relRotation)
	{
		GameObject go = GameObject.Instantiate(ob, relPosition, relRotation) as GameObject;
		GameObject.DontDestroyOnLoad(go);
		
		AttachPointInfo attachPoint = new AttachPointInfo();
		attachPoint.MParentGameObject = this.MParentGameObject;
		attachPoint.MAttachGameObject = go;
		attachPoint.MAttachPosition = relPosition;
		attachPoint.MAttachRotation = relRotation;
		
		attachPoint.MAttachGameObject.transform.parent = attachPoint.MParentGameObject.transform;
		attachPoint.MAttachGameObject.transform.localPosition = relPosition;
		attachPoint.MAttachGameObject.transform.localRotation = relRotation;
		
		_mAttachPointInfoList.Add(go.GetInstanceID(), attachPoint);
		
		return go;
	}
	
	public void DetachObject(GameObject go)
	{
		AttachPointInfo attachPoint = null;
		if ( !_mAttachPointInfoList.TryGetValue(go.GetInstanceID(), out attachPoint) )
			return;
		
		_mAttachPointInfoList.Remove(go.GetInstanceID());
		
		attachPoint.MParentGameObject = null;
		attachPoint.MAttachGameObject.transform.parent = null;
		GameObject.DestroyObject(attachPoint.MAttachGameObject);
	}
	
	public bool GetAttachPointInfo(GameObject go, out AttachPointInfo attachPointInfo)
	{
		attachPointInfo = null;
		
		if ( !_mAttachPointInfoList.TryGetValue(go.GetInstanceID(), out attachPointInfo) )
			return false;
		
		return true;
	}
	
	public void ActiveAttachObject(GameObject go)
	{
		AttachPointInfo attachPointInfo = null;
		if ( !_mAttachPointInfoList.TryGetValue(go.GetInstanceID(), out attachPointInfo) )
			return;
		
		attachPointInfo.MAttachGameObject.SetActiveRecursively(true);
	}
	
	public void DeactiveAttachObject(GameObject go)
	{
		AttachPointInfo attachPointInfo = null;
		if ( !_mAttachPointInfoList.TryGetValue(go.GetInstanceID(), out attachPointInfo) )
			return;
		
		attachPointInfo.MAttachGameObject.SetActiveRecursively(false);
	}
	
	private GameObject MParentGameObject;
	private Dictionary<int, AttachPointInfo> _mAttachPointInfoList = new Dictionary<int, AttachPointInfo>();
}
