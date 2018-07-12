using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HQGeneralManager : MonoBehaviour
{
	void Awake()
	{
		// Recuit data
		_mHQRecruitGeneralDataList = new List<HQGeneralData>();
		
		// Dismiss data
		/*_mHQDismissGeneralDataList = new Dictionary<EGeneralProfession, List<HQGeneralData>>();
		_mHQDismissGeneralDataList[EGeneralProfession.ALL_ADAPTED] = new List<HQGeneralData>();
		_mHQDismissGeneralDataList[EGeneralProfession.CARRIER_GENERAL] = new List<HQGeneralData>();
		_mHQDismissGeneralDataList[EGeneralProfession.SURFACE_SHIP_GENERAL] = new List<HQGeneralData>();
		_mHQDismissGeneralDataList[EGeneralProfession.SUBMARINE_GENERAL] = new List<HQGeneralData>();*/
	}
	
	void OnDestroy()
	{
		ClearData();
	}
	
	private void ClearData()
	{
		_mHQRecruitGeneralDataList.Clear();
		
		/*_mHQDismissGeneralDataList[EGeneralProfession.ALL_ADAPTED].Clear();
		_mHQDismissGeneralDataList[EGeneralProfession.CARRIER_GENERAL].Clear();
		_mHQDismissGeneralDataList[EGeneralProfession.SURFACE_SHIP_GENERAL].Clear();
		_mHQDismissGeneralDataList[EGeneralProfession.SUBMARINE_GENERAL].Clear();
		_mHQDismissGeneralDataList.Clear();*/
	}
	
	public void AddRecruitGeneralData(HQGeneralData data)
	{
		if(!_mHQRecruitGeneralDataList.Contains(data))
		{
			_mHQRecruitGeneralDataList.Add(data);
		}
	}
	
	public void RemoveRecruitGeneralData(HQGeneralData data)
	{
		if(_mHQRecruitGeneralDataList.Contains(data))
		{
			_mHQRecruitGeneralDataList.Remove(data);
		}
	}
	
	public void RemoveRecruitGeneralData(int logicID)
	{
		HQGeneralData data = GetRecruitGeneralData(logicID);
		if(data != null)
		{
			RemoveRecruitGeneralData(data);
		}
	}
	
	public HQGeneralData GetRecruitGeneralData(int logicID)
	{
		for (int i = 0; i < _mHQRecruitGeneralDataList.Count; ++i)
		{
			if (logicID == _mHQRecruitGeneralDataList[i].GeneralData.BasicData.LogicID)
			{
				return _mHQRecruitGeneralDataList[i];
			}
		}
		
		return null;
	}
	
	public void UpdateRecruitGeneralData(HQGeneralData data)
	{
		for (int i = 0; i < _mHQRecruitGeneralDataList.Count; ++i)
		{
			if (data.GeneralData.BasicData.LogicID == _mHQRecruitGeneralDataList[i].GeneralData.BasicData.LogicID)
			{
				_mHQRecruitGeneralDataList[i] = data;
				break;
			}
		}
	}
	
	/*public void AddDismissGeneralData(HQGeneralData data)
	{
		_mHQDismissGeneralDataList[ data.GeneralData.BasicData.Profession ].Add(data);
	}
	
	public void RemoveDismissGeneralData(HQGeneralData data)
	{
		_mHQDismissGeneralDataList[ data.GeneralData.BasicData.Profession ].RemoveAll(delegate(HQGeneralData obj) 
		{
			if (obj.GeneralData.BasicData.LogicID == data.GeneralData.BasicData.LogicID)
				return true;
			
			return false;
		}
		);
	}*/
	
	public HQGeneralData GetDismissGeneralData(int logicID)
	{
		for (int i = 0; i < _mHQRecruitGeneralDataList.Count; ++i)
		{
			if (logicID == _mHQRecruitGeneralDataList[i].GeneralData.BasicData.LogicID)
			{
				return _mHQRecruitGeneralDataList[i];
			}
		}
		
		return null;
	}
	
	/*public void UpdateDismissGeneralData(HQGeneralData data)
	{
		List<HQGeneralData> dataList = _mHQDismissGeneralDataList[ data.GeneralData.BasicData.Profession ];
		for (int i = 0; i < dataList.Count; ++i)
		{
			if (data.GeneralData.BasicData.LogicID == dataList[i].GeneralData.BasicData.LogicID)
			{
				dataList[i] = data;
				break;
			}
		}
	}*/
	
	public List<HQGeneralData> GetHQRecruitGeneralDataList()
	{
		return _mHQRecruitGeneralDataList;
	}
	
	/*public List<HQGeneralData> GetHQDismissGeneralDataList(EGeneralProfession profession)
	{
		return _mHQDismissGeneralDataList[profession];
	}
	*/
	public void ClearHQRecruitGeneraDataList(EGeneralProfession profession)
	{
		_mHQRecruitGeneralDataList.Clear();
	}
	
	/*public void ClearHQDismissGeneraDataList(EGeneralProfession profession)
	{
		_mHQDismissGeneralDataList[profession].Clear();
	}
	*/
	private List<HQGeneralData>  _mHQRecruitGeneralDataList;
	//private Dictionary<EGeneralProfession, List<HQGeneralData>>  _mHQDismissGeneralDataList;
	
}
