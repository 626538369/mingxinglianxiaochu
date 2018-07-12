using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public static class ObjectUtil
{
	public static GameObject FindChildObject( GameObject parent, string name )
	{
		if ( name == "" )
			return null;
		
		string lowName = name.ToLower();
			
		Component [] children = parent.GetComponentsInChildren( typeof( Transform ), true );
		
		foreach ( Component child in children )
		{
			if ( child.gameObject.name.ToLower() == lowName )
				return child.gameObject;
		}
		
		return null;
	}
	
	public static GameObject FindChildObject( GameObject parent, string name, uint depth )
	{
		List<GameObject> lstObj = GetChildObjectsByDepth( parent, depth );
		
		foreach ( GameObject o in lstObj )
		{
			if ( o.name == name )
				return o;
		}
		
		return null;
	}
	
	public static List<GameObject> FindChildObjects( GameObject parent, string name )
	{
		List<GameObject> res = new List<GameObject>();
		
		if ( name == "" )
			return res;
		
		string lowName = name.ToLower();
			
		Component [] children = parent.GetComponentsInChildren( typeof( Transform ), true );
		
		foreach ( Component child in children )
		{
			if ( child.gameObject.name.ToLower() == lowName )
				res.Add( child.gameObject );
		}
		
		return res;
	}
	
	public static List<GameObject> GetChildObjectsByDepth( GameObject parent, uint depth )
	{
		List<GameObject> rsList = new List<GameObject>();
		
		Component [] children = parent.GetComponentsInChildren( typeof( Transform ), true );
		
		uint prD = GetTransformDepth( parent.transform );
		
		foreach ( Transform child in children )
		{
			if ( GetTransformDepth( child ) - prD == depth )
				rsList.Add( child.gameObject );
		}
		
		return rsList;
	}
	
	public static uint GetTransformDepth( Transform trans )
	{
		uint depth = 0;
		
		Transform parent = trans.parent;
		
		while ( parent != null )
		{
			depth ++;
			parent = parent.parent;
		}
		
		return depth;
	}
	
	public static string GetTransRootPath( Transform trans )
	{
		StringBuilder bld = new StringBuilder( trans.name );
		
		Transform parent = trans.parent;
		
		while ( parent != null )
		{
			bld.Insert( 0, Defines.DIRECTORY_CHAR );
			bld.Insert( 0, parent.name );
			
			parent = parent.parent;
		}
		
		return bld.ToString();
	}
	
	public static Transform GetTransFromRoot(Transform trans, string path)
	{
		path = path.Replace('\\', Defines.DIRECTORY_CHAR);
		path = path.Trim(Defines.DIRECTORY_CHAR);
		
		int dirIndex = path.IndexOf(Defines.DIRECTORY_CHAR);
		
		if (dirIndex<0)
		{
			if (path.Equals(trans.name.ToLower()))
				return trans;
			else
				return null;
		}
		else
		{
			if (!path.Substring(0,dirIndex).Equals(trans.name.ToLower()))
				return null;
			else
				return trans.Find(path.Substring(dirIndex + 1, path.Length - dirIndex - 1));				
		}
	}
	
	private static Vector3 KeepSign( Vector3 a, Vector3 b )
	{
		if ( a.x >= 0 )
			b.x = Mathf.Abs( b.x );
		else
			b.x = -b.x;
		
		if ( a.y >= 0 )
			b.y = Mathf.Abs( b.y );
		else
			b.y = -b.y;
		
		if ( a.z >= 0 )
			b.z = Mathf.Abs( b.z );
		else
			b.z = -b.z;
		
		return b;
	}
	
	public static void AttachToParent( GameObject parent, GameObject child, Vector3 localPos, Quaternion localRot )
	{
		child.transform.parent = parent.transform;
		child.transform.localPosition = localPos;
		child.transform.localRotation = localRot;
	}

	public static void AttachToParentAndResetLocalTrans(GameObject parent, GameObject child)
	{
		child.transform.parent = parent.transform;
		child.transform.localPosition = Vector3.zero;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localScale = Vector3.one;
	}

	public static void AttachToParentAndResetLocalPosAndRotation( GameObject parent, GameObject child )
	{
		child.transform.parent = parent.transform;
		child.transform.localPosition = Vector3.zero;
		child.transform.localRotation = Quaternion.identity;
	}
	
	public static void AttachToParentAndKeepWorldTrans( GameObject parent, GameObject child )
	{
		Vector3 oldPos = child.transform.position;
		Quaternion oldRot = child.transform.rotation;
		child.transform.parent = parent.transform;
		child.transform.position = oldPos;
		child.transform.rotation = oldRot;
	}
	
	public static void AttachToParentAndKeepLocalTrans( GameObject parent, GameObject child )
	{
		Vector3 oldLPos = child.transform.localPosition;
		Quaternion oldLRot = child.transform.localRotation;
		child.transform.parent = parent.transform;
		child.transform.localPosition = oldLPos;
		child.transform.localRotation = oldLRot;
	}
	
	public static void DestroyChildObjects( GameObject parent )
	{
		Component [] children = parent.GetComponentsInChildren( typeof( Transform ), true );
		
		foreach ( Component child in children )
		{
			if ( child.gameObject == parent )
				continue;
			
			GameObject.Destroy( child.gameObject );
		}
	}
	
	public static void SetObjectLayer( GameObject obj, int layer )
	{
		Component [] children = obj.GetComponentsInChildren( typeof( Transform ), true );
		
		foreach ( Component child in children )
		{
			child.gameObject.layer = layer;
		}
	}
	
	public static void UnifyWorldTrans( GameObject src, GameObject dst )
	{
		dst.transform.position = src.transform.position;
		dst.transform.rotation = src.transform.rotation;
	}
	
	public static Bounds GetMaxBounds( GameObject obj )
	{
		Bounds bd = new Bounds( obj.transform.position, Vector3.zero );
		
		Vector3 min = new Vector3( Mathf.Infinity, Mathf.Infinity, Mathf.Infinity );
		Vector3 max = new Vector3( -Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity );
		bool setBd = false;
		
		Component []rnds = obj.GetComponentsInChildren( typeof( Renderer ), true );
		
		foreach ( Renderer rnd in rnds )
		{
			// Skip particle bounds.
			if ( rnd is ParticleRenderer )
				continue;
			
			Bounds b = rnd.bounds;
			
			if ( b.min.x < min.x )
				min.x = b.min.x;
			
			if ( b.min.y < min.y )
				min.y = b.min.y;
			
			if ( b.min.z < min.z )
				min.z = b.min.z;
			
			if ( b.max.x > max.x )
				max.x = b.max.x;
			
			if ( b.max.y > max.y )
				max.y = b.max.y;
			
			if ( b.max.z > max.z )
				max.z = b.max.z;
			
			setBd = true;
		}
		
		if ( setBd )
			bd.SetMinMax( min, max );
		
		return bd;
	}
}