using UnityEngine;
using System.Collections;


public abstract class TipsLayout
{
	public TipsLayout(GUIToolTips gui)
	{
		_mToolTips = gui;
	}
	
	public abstract void DoLayout(Vector3 pos, Vector2 size);
	
	protected GUIToolTips _mToolTips;
}


public class ItemTipsLayout : TipsLayout
{
	public ItemTipsLayout(GUIToolTips gui) 
		: base(gui)
	{
		_mGUILimitRect = Globals.Instance.MGUIManager.M3DScreenRect;
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		if (base._mToolTips.IsLoaded)
		{
			float halfWidth = base._mToolTips.GUIWidth * 0.5f;
			float halfHeight = base._mToolTips.GUIHeight * 0.5f;
			
			Vector3 orignalPos = base._mToolTips.transform.position;
			Vector3 destPos = orignalPos;
			
			// Adjust x value
			if (pos.x > 0)
			{
				// Screen left
				// Tips at target ui right
				destPos.x = pos.x - 0.5f * size.x - halfWidth;
				destPos.y = pos.y + 0.5f * size.y + halfHeight;
			}
			else
			{
				// Screen right
				// Tips at target ui left
				destPos.x = pos.x + 0.5f * size.x + halfWidth;
				destPos.y = pos.y + 0.5f * size.y + halfHeight;
			}
			
			float left = destPos.x - halfWidth;
			float right = destPos.x + halfWidth;
			float top = destPos.y + halfHeight;
			float bottom = destPos.y - halfHeight;
			
			float minx = _mGUILimitRect.topLeft.x;
			float maxx = _mGUILimitRect.bottomRight.x;
			float maxy = _mGUILimitRect.topLeft.y;
			float miny = _mGUILimitRect.bottomRight.y;
			
			// Adjust y value
			if (top > maxy)
			{
				float diff = top - maxy;
				destPos.y -= diff;
			}
			else if (bottom < miny)
			{
				float diff = bottom - miny;
				destPos.y += diff;
			}
			
			base._mToolTips.transform.position = destPos;
		}
	}
	
	// Use EZGUI Rect3D
	private Rect3D _mGUILimitRect;
}

public class BuffTipsLayout : TipsLayout
{
	public BuffTipsLayout(GUIToolTips gui) 
		: base(gui)
	{
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		if (_mToolTips.IsLoaded)
		{
			float halfWidth = _mToolTips.GUIWidth * 0.5f;
			float halfHeight = _mToolTips.GUIHeight * 0.5f;
			
			Vector3 orignalPos = _mToolTips.transform.position;
			Vector3 destPos = orignalPos;
			
			// At the target left-bottom
			destPos.x = pos.x - 0.5f * size.x - halfWidth;
			destPos.y = pos.y - 0.5f * size.y - halfHeight;
			_mToolTips.transform.position = destPos;
		}
	}
}

public class SimpleTipsLayout : TipsLayout
{
	public SimpleTipsLayout(GUIToolTips gui) 
		: base(gui)
	{
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		if (_mToolTips.IsLoaded)
		{
			_mToolTips.transform.localPosition = new Vector3(0.0f, 0.0f, _mToolTips.transform.localPosition.z);
		}
	}
}

public class TopCenterLayout : TipsLayout
{
	public TopCenterLayout(GUIToolTips gui) 
		: base(gui)
	{
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		if (_mToolTips.IsLoaded)
		{
			_mToolTips.transform.localPosition = new Vector3(0.0f, 0.0f, _mToolTips.transform.localPosition.z);
		}
	}
}