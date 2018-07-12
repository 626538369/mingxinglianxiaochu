using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour 
{
	public GameObject IndicatorTP;
	public GameObject GoldChestTP;
	public GameObject BattleFogTP;
	public GameObject BuildingCreateTP;
	public GameObject BuildingDestroyTP;
	
	public delegate void EffectDelegate(GameObject effect);
	
	void Awake()
	{
		_mEffectList = new Dictionary<int, GameObject>();
	}
	
	void OnDestroy()
	{
		_mEffectList.Clear();
	}
	
	public GameObject CreateEffect(GameObject prefab)
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		
		_mEffectList.Add(go.GetInstanceID(), go);
		return go;
	}
	
	public void DestroyEffect(GameObject prefab)
	{
		_mEffectList.Remove(prefab.GetInstanceID());
		GameObject.Destroy(prefab);
	}
	
	public GameObject Create3DIndicator(Vector3 worldPos)
	{
		GameObject go = CreateEffect(IndicatorTP);
		go.name = "Indicator";
		go.transform.position = worldPos;
		
		return go;
	}
	
	public void CreateGoldChest(Vector3 worldPos, EffectDelegate endDel)
	{
		Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/OpenChest");
		StartCoroutine( DoCreateEffect(GoldChestTP, worldPos, Quaternion.identity, 1.6f, endDel) );
	}
	
	public void CreateBattleFog(Vector3 worldPos, Quaternion rotation, EffectDelegate endDel)
	{
		StartCoroutine( DoCreateEffect(BattleFogTP, worldPos, rotation, 2.0f, endDel) );
	}
	
	public void CreateBuildingChangeEffect(Vector3 worldPos, bool isCreate, EffectDelegate endDel)
	{
		if (isCreate)
		{
			StartCoroutine( DoCreateEffect(BuildingCreateTP, worldPos, Quaternion.identity, 1.5f, endDel) );
		}
		else
		{
			StartCoroutine( DoCreateEffect(BuildingDestroyTP, worldPos, Quaternion.identity, 1.5f, endDel) );
		}
	}
	
	protected IEnumerator DoCreateEffect(GameObject prefab, Vector3 worldPos, Quaternion rotation, float length, EffectDelegate endDel)
	{
		GameObject go = CreateEffect(prefab);
		go.name = prefab.name + "_Inst";
		go.transform.position = worldPos;
		go.transform.rotation = rotation;
		
		yield return new WaitForSeconds(length);
		
		if (null != endDel)
		{
			endDel(go);
		}
		
		Destroy(go);
	}
	
	private Dictionary<int, GameObject> _mEffectList;
}
