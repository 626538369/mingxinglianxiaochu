using UnityEngine;
using System.Collections;

public class BuildingSet : MonoBehaviour 
{
	UIButton buildingBtn = null;
	UIButton buildingNameBtn = null;
	UILabel  buildingTitle = null;
	void Awake()
	{
		if (null == buildingBtn)
		{
			buildingBtn = transform.GetComponent<UIButton>() as UIButton;
			buildingNameBtn = transform.Find("NameBtn").GetComponent<UIButton>() as UIButton;
			buildingTitle = transform.Find("UILable").GetComponent<UILabel>() as UILabel;
		}
	}
	
	public void UpdateData(BuildingData data)
	{
		if (buildingBtn)
		{
			buildingBtn.Data = data;
			buildingNameBtn.Data = data;
			buildingTitle.text = data.Name;
		}
	}
	
	public void SetValueChangedDel(UIEventListener.VoidDelegate del)
	{
		UIEventListener.Get(buildingBtn.gameObject).onClick += del;
		UIEventListener.Get(buildingNameBtn.gameObject).onClick += del;
	}
}