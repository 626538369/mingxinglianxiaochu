using UnityEngine;
using System.Collections;

public abstract class RoleBasePanel : MonoBehaviour 
{
	public bool IsNeedCreate
	{
		get { return isNeedCreate; }
		set { isNeedCreate = value; }
	}
	
	//----------------------------------------------------
	protected virtual void Awake()
	{
		Transform root = transform;
		Transform parent = root.parent;
		while (null != parent && !parent.name.Equals("UICamera"))
		{
			root = parent;
			parent = root.parent;
		}
		
		if (null != root)
		{
			guiParent = root.gameObject.GetComponent<GUIWindow>() as GUIWindow;
		}
	}
	
	public abstract void UpdateGUI();
	public abstract void HideGUI();
	public virtual void OnClickNonFunctionalArea(GameObject obj) {}
	
	public void SetVisible(bool visible)
	{
		gameObject.SetActiveRecursively(visible);
	}
	
	protected T GetGUIParentInst<T>() where T: GUIWindow
	{
		return (T)guiParent;
	}
	
	private bool isNeedCreate = true;
	protected GUIWindow guiParent = null;
}
