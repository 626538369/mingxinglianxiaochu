using UnityEngine;
using System.Collections;

[System.Serializable]
public enum ColliderStyle
{
	BOX,
	SPHERE,
	MESH,
}

[ExecuteInEditMode]
public class TriggerBase : MonoBehaviour 
{
	public class AttributeMirror
	{
		public AttributeMirror()
		{
		}
		
		public void Mirror(TriggerBase source)
		{
			this.colliderStyle = source.colliderStyle;
			this.center = source.center;
			this.boxSize = source.boxSize;
			this.radius = source.radius;
			this.mesh = source.mesh;
			this.lineWidth = source.lineWidth;
			this.isDraw = source.isDraw;
		}
		
		public bool IsChange(TriggerBase source)
		{
			if (this.colliderStyle != source.colliderStyle)
				return true;
			
			if (this.center != source.center)
				return true;
			
			if (this.boxSize != source.boxSize)
			{
				if (this.colliderStyle == ColliderStyle.BOX)
					return true;
				else 
					return false;
			}
			
			if (this.radius != source.radius)
			{
				if (this.colliderStyle == ColliderStyle.SPHERE)
					return true;
				else 
					return false;
			}
			
			if (this.mesh != source.mesh)
				return true;
			
			if (this.lineWidth != source.lineWidth)
				return true;
			
			
			if (this.isDraw != source.isDraw)
				return true;
			
			return false;
		}
		
		public ColliderStyle colliderStyle;
		public Vector3 center;
		public Vector3 boxSize;
		public float radius;
		public Mesh mesh;
		public float lineWidth;
		
		public bool isDraw;
	}
	
	//--------------------------------------------------------
	public delegate void OnTriggerEvent(GameObject first, GameObject other);
	//--------------------------------------------------------
	
	public float Radius
	{
		get { return radius; }
		set
		{
			radius = value;
			colliderStyle = ColliderStyle.SPHERE;
			DoChange();
		}
	}
	
	public Vector3 BoxSize
	{
		get { return boxSize; }
		set
		{
			boxSize = value;
			colliderStyle = ColliderStyle.BOX;
			DoChange();
		}
	}
	
	protected virtual void Awake()
	{
		colliderStyle = ColliderStyle.SPHERE;
		
		_mDrawGameObj = null;
		_mCollider = gameObject.GetComponent<Collider>() as Collider;
		if (null != _mCollider)
			_mCollider.isTrigger = true;
	}

	protected virtual void OnDestroy()
	{
		_mCollider = null;
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{
		DoMirror();
	}
	
	protected virtual void OnBecameVisible()
	{
	}
	
	protected virtual void OnBecameInvisible()
	{
	}
	
	protected virtual void OnTriggerEnter(Collider other)
	{
		if (null == TriggerEnterEvents)
			return;

		{
			TriggerEnterEvents(gameObject, other.gameObject);
		}
	}
	
	protected virtual void OnTriggerExit(Collider other)
	{
		if (null == TriggerExitEvents)
			return;
		
		if (isInChild)
		{
			GameObject first = transform.parent.gameObject;
			TriggerExitEvents(first, other.gameObject);
		}
		else
		{
			TriggerExitEvents(this.gameObject, other.gameObject);
		}
	}
	
	private void DoMirror()
	{
		if (Application.isPlaying)
			return;
		
		if (null == _mMirror)
			_mMirror = new AttributeMirror();
		
		if (_mMirror.IsChange(this))
		{
			DoChange();
		}
		
		_mMirror.Mirror(this);
	}
	
	protected void DoChange()
	{
		ChangeCollider();
	}
	
	protected void ChangeCollider()
	{
		// Remove the old collider component
		if (null != _mCollider)
		{
			Component.DestroyImmediate(_mCollider);
			_mCollider = null;
		}
		
		switch (colliderStyle)
		{
		case ColliderStyle.BOX:
		{
			BoxCollider coll = gameObject.GetComponent<BoxCollider>() as BoxCollider;
			if (null == coll) coll = gameObject.AddComponent<BoxCollider>() as BoxCollider;
			
			coll.center = center;
			coll.size = boxSize;
			
			_mCollider = coll;
			break;
		}
		case ColliderStyle.SPHERE:
		{
			SphereCollider coll = gameObject.GetComponent<SphereCollider>() as SphereCollider;
			if (null == coll) coll = gameObject.AddComponent<SphereCollider>() as SphereCollider;
			
			coll.center = center;
			coll.radius = radius;
			
			_mCollider = coll;
			break;
		}
		case ColliderStyle.MESH:
		{
			MeshCollider coll = gameObject.GetComponent<MeshCollider>() as MeshCollider;
			if (null == coll) coll = gameObject.AddComponent<MeshCollider>() as MeshCollider;
			
			coll.sharedMesh = mesh;
			
			_mCollider = coll;
			break;
		}
		}
		
		_mCollider.isTrigger = true;
		// DrawCollider();
	}
	
	private void DrawCollider()
	{
		if (null != _mDrawGameObj)
		{
			DestroyImmediate(_mDrawGameObj);
			_mDrawGameObj = null;
		}
		
		if (!isDraw)
			return;
		
		switch (colliderStyle)
		{
		case ColliderStyle.BOX:
		{
			// Gizmos.DrawWireCube(center, boxSize);
			break;
		}
		case ColliderStyle.SPHERE:
		{
			_mDrawGameObj = GraphicsTools.DrawDottedCircle(center, radius, 6, lineWidth, lineWidth);
			// Gizmos.DrawWireSphere(center, radius);
			break;
		}
		case ColliderStyle.MESH:
		{
			break;
		}
		}
		_mDrawGameObj.transform.parent = gameObject.transform;
		_mDrawGameObj.transform.localPosition += new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	// void OnDrawGizmos()
	// {
	// 	if (isDraw)
	// 	{
	// 		Gizmos.color = Color.yellow;
	// 		DrawCollider();
	// 	}
	// }
	// 
	// void OnDrawGizmosSelected()
	// {
	// 	if (isDraw)
	// 	{
	// 		Gizmos.color = new Color(1f, 0, 0.5f, 1f);
	// 		DrawCollider();
	// 	}
	// }
	
	// void TestDrawLine()
	// {
	// 	Vector3[] list = new Vector3[3];
	// 	list[0] = Vector3.zero;
	// 	list[1] = Vector3.one;
	// 	list[2] = Vector3.one * 0.5f;
	// 	
	// 	GraphicsTools.DrawDottedLine(list, 1, 1, Color.white, Color.white);
	// }
	
	[HideInInspector] public event TriggerBase.OnTriggerEvent TriggerEnterEvents;
	[HideInInspector] public event TriggerBase.OnTriggerEvent TriggerExitEvents;
	
	[HideInInspector] public ColliderStyle colliderStyle;
	[HideInInspector] public Vector3 center = Vector3.zero;
	[HideInInspector] public Vector3 boxSize = Vector3.one;
	[HideInInspector] public float radius = 1.0f;
	[HideInInspector] public Mesh mesh;
	[HideInInspector] public float lineWidth = 1f;
	
	[HideInInspector] public bool isDraw = true;
	[HideInInspector] public bool isInChild = false;
	
	protected Collider _mCollider;
	protected GameObject _mDrawGameObj;
	protected AttributeMirror _mMirror;
}
