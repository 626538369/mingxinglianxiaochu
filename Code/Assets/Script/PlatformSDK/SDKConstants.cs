using UnityEngine;
using System.Collections;

public class SDKConstants 
{
	public const string CALLBACK_METHOD_OnInitSDK           = "OnInitSDK";
    public const string CALLBACK_METHOD_OnCloseSDK          = "OnCloseSDK";
    public const string CALLBACK_METHOD_OnLogin             = "OnLogin";
    public const string CALLBACK_METHOD_OnLogout            = "OnLogout";
	public const string CALLBACK_METHOD_OnRegister             = "OnRegister";
    public const string CALLBACK_METHOD_OnPayProduct        = "OnPayProduct";
    public const string CALLBACK_METHOD_OnRecharge          = "OnRecharge";
    public const string CALLBACK_METHOD_OnImproveInformation = "OnImproveInformation";
	public const string CALLBACK_METHOD_OnAppVersionUpdate = "OnAppVersionUpdate";
	
	public const string KEY_CALLBACK_METHOD = "CallbackMethod";
	public const string KEY_JSON_DATA = "Data";
	
	public const string KEY_ERROR_CODE = "ErrorCode";
	
	public const string KEY_USER_ID = "UserId";
	public const string KEY_USER_SESSION_ID = "SessionId";
	public const string KEY_USER_NICKNAME = "NickName";
	
	public const string KEY_ORDER_ID = "OrderId";
	public const string KEY_PRODUCT_ID = "ProductId";
	public const string KEY_PRODUCT_NAME = "ProductName";
	public const string KEY_PRODUCT_COUNT = "ProductCnt";
	public const string KEY_PAY_MONEY = "PayMoney";
	public const string KEY_PAY_DESCRIPTION = "PayDescription";
	public const string KEY_PAY_CUSTOM_INFO = "CustomInfo";
}