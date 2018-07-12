using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U3dIOSReceiver : MonoBehaviour {

    // return value type is int or bool
    public static readonly string KeyResult = "result";
    // If the error return value is 0, then API is callback success, return value type is int
    public static readonly string KeyError = "error";
    // return value type is string
    public static readonly string KeyIosOrderId = "cooOrderSerial";
    // return value type is string
    public static readonly string KeyProductId = "productId";
    // return value type is string
    public static readonly string KeyPayDescription = "payDescription";

    public static readonly string KeyOrderId = "orderId";

    const string majorDelimiter = "&&&";
    const string assignDelimiter = "#";

    //获取product列表
    void ShowProductList(string s)
    {
        Debug.Log("获取product列表:" + s);
    }

    //获取商品回执
    void ProvideContent(string s)
    {
        Debug.Log("获取商品回执 : " + s);
    }

    void paymentNotify(string args)
    {
        ParseReceiveParams(args);

        string result = GetReceiveParam(KeyResult);
        string error = GetReceiveParam(KeyError);

        Debug.Log("[U3dAppReceiver]: paymentNotify------------------- the result is " + result);
        Debug.Log("[U3dAppReceiver]: paymentNotify------------------- the error is " + error);

        if ("Success" == error)
        {
            string productID = GetReceiveParam(KeyProductId);
            string changeIdentity = GetReceiveParam(KeyIosOrderId);
            string changeReceipt = GetReceiveParam(KeyPayDescription);
            string keyOrderId = GetReceiveParam(KeyOrderId);

            Debug.Log("[U3dAppReceiver]: productID is  " + productID);
            Debug.Log("[U3dAppReceiver]: changeIdentity is  " + changeIdentity);
            Debug.Log("[U3dAppReceiver]: changeReceipt is  " + changeReceipt);
            Debug.Log("[U3dAppReceiver]: keyOrderId is  " + keyOrderId);

            if(keyOrderId != null && keyOrderId != ""){
                ShopDataManager.AddPendingOrderId(keyOrderId, GameDefines.OutPutChannelsIdentity, changeIdentity, changeReceipt);
                NetSender.Instance.RequestAppStoreChargeConfirm(keyOrderId, changeReceipt, changeIdentity);
            }else{
                ShopDataManager.AddPendingOrderId(ShopDataManager.PayCommodityData.orderId, GameDefines.OutPutChannelsIdentity, changeIdentity, changeReceipt);
                NetSender.Instance.RequestAppStoreChargeConfirm(ShopDataManager.PayCommodityData.orderId, changeReceipt, changeIdentity);
            }
        }
        else
        {
//            UIWindowMgr.hideWaiting();
			GUIRadarScan.Hide();
        }
    }

    Dictionary<string, string> receiveParams = new Dictionary<string, string>();
    bool ParseReceiveParams(string argvs)
    {
        receiveParams.Clear();

        Debug.Log("[ParseReceiveParams]: the argvs is " + argvs);


        string[] sections = argvs.Split(majorDelimiter.ToCharArray());
        foreach (string section in sections)
        {
            string[] keyValues = section.Split(assignDelimiter.ToCharArray());
            if (2 != keyValues.Length)
            {
                Debug.Log("[U3d2NdReceiver]: The parameters has some invalid data,early break.");
            }
            else
            {
                Debug.Log("keyValues[0] is : " + keyValues[0]);
                Debug.Log("keyValues[1] is : " + keyValues[1]);
                receiveParams.Add(keyValues[0], keyValues[1]);
            }
        }

        return true;
    }

    string GetReceiveParam(string key)
    {
        string val = "";
        receiveParams.TryGetValue(key, out val);

        return val;
    }

    void ReceivePurchaseFail(string args)
    {
        if (!string.IsNullOrEmpty(args))
        {
//            UIWindowMgr.showTips(args);
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(args);
        }
//        UIWindowMgr.hideWaiting();
		GUIRadarScan.Hide();
		Debug.Log("ReceivePurchaseFail = " + args);
    }

	public void SavePhoto(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		Debug.Log("SavePhoto result is " + result);
		GUIRadarScan.Hide();
		if (result == "true" || result == "1")
		{
			Debug.Log("ShowSimpleCenterTips 4011" );
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4011);
		}
	}


	public static readonly string KeyMACAdress = "macAdress";

	public static readonly string KeyIDAF = "idaf";
	public void macAdressIDAF(string args)
	{
		IDAFParseReceiveParams(args);
		string macAdress  = GetReceiveParamIDAF(KeyMACAdress);
		string idafstr  = GetReceiveParamIDAF(KeyIDAF);
		GameDefines.systemMacAdress = macAdress ;
		if ("empty" != idafstr)
		{
			GameDefines.systemIFAD = idafstr;
			GameDefines.systemUDID = idafstr;
		}

		Debug.Log("macAdress is " + macAdress );
		Debug.Log("idafstr is " + idafstr );
	}
	string GetReceiveParamIDAF(string key)
	{
		string val = "";
		receiveIDAFParams.TryGetValue(key, out val);

		return val;
	}
	Dictionary<string, string> receiveIDAFParams = new Dictionary<string, string>();
	const string majorDelimiterIDAF = "&&&";
	const string assignDelimiterIDAF = "#";
	bool IDAFParseReceiveParams(string argvs)
	{
		receiveIDAFParams.Clear();

		Debug.Log("[IDAFParseReceiveParams]: the argvs is " + argvs);


		string[] sections = argvs.Split(majorDelimiterIDAF.ToCharArray());
		foreach (string section in sections)
		{
			string[] keyValues = section.Split(assignDelimiterIDAF.ToCharArray());
			if (2 != keyValues.Length)
			{
				Debug.Log("[U3d2NdReceiver]: The parameters has some invalid data,early break.");
			}
			else
			{
				receiveIDAFParams.Add(keyValues[0], keyValues[1]);
			}
		}

		return true;
	}
}
