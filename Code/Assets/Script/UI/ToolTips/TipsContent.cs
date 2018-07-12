using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TipsContent : MonoBehaviour
{
	public virtual void Init()
	{
		_mToolTips = this.transform.parent.GetComponent<GUIToolTips>() as GUIToolTips;
	}
	
	public abstract void SetSize(float w, float h);
	public abstract Vector2 GetSize();
	
	public abstract void Destroy();
	public abstract void SetActiveBG(bool IsOpen);
	public abstract void SetValue(params object[] args);
	public abstract void DoLayout(Vector3 pos, Vector2 size);
	
	protected GUIToolTips _mToolTips;
	protected TipsLayout _mTipsLayout;
}