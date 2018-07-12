using UnityEngine;
using System.Runtime.InteropServices;

public class U3dIOSSendToSdk : MonoBehaviour {


    [DllImport("__Internal")]
    private static extern void InitIAPManager();//初始化

    [DllImport("__Internal")]
    private static extern bool IsProductAvailable();//判断是否可以购买

    [DllImport("__Internal")]
    private static extern void RequstProductInfo(string s, string b);//获取商品信息

	[DllImport("__Internal")]
	private static extern void U3dSavePhoth(string savePath);

    // Use this for initialization
    void Start()
    {
		if(GameDefines.OutputVerDefs == OutputVersionDefs.AppStore){
			InitIAPManager();
			Debug.Log("初始化ios内购信息");
		}
    }
	
    /// <summary>
    /// 购买商品
    /// </summary>
    /// <param name="priductid">商品ID</param>
    static public void BuyProductClick(string priductid,string orderid)
    {
        if (!IsProductAvailable())
        {
//            UIWindowMgr.showTips("无法购买此商品！请联系客服！");
//            UIWindowMgr.hideWaiting();
			Globals.Instance.MGUIManager.ShowSimpleCenterTips("无法购买此商品！请联系客服！");
			GUIRadarScan.Hide();
        }
        else
        {
            //产品id，此处id要和apple开发者后台产品id相同，每个产品id用\t相隔
            RequstProductInfo(priductid, orderid);
        }
    }


	public static void AppSavePhoth(string strAddr)
	{
		if(GameDefines.OutputVerDefs == OutputVersionDefs.AppStore){
			GUIRadarScan.Show();
			U3dSavePhoth(strAddr);
		}
	}
}
