using UnityEngine;
using System.Collections;

public class RefCountObject
{
	public int RefCount { get { return _refCount; } }

	~RefCountObject()
	{
		Debug.Assert(_refCount == 0, "Deleting referenced object");
	}

	public void AddRef()
	{
		_refCount++;
	}

	public void ReleaseRef()
	{
		Debug.Assert(_refCount > 0, "mRefCount > 0");
		_refCount--;
	}

	private int _refCount = 0;
}
