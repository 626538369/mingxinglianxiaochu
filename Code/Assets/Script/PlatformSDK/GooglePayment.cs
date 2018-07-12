using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class GooglePayment : MonoBehaviour, IStoreListener
{

    public static GooglePayment Instance;
//    #if UNITY_ANDROID
    private const string product_gold_98y = "com.mingyou.star.gold.98y";
	private const string product_198y = "com.mingyou.star.198y";
	private const string product_30y = "com.mingyou.star.30y";
	private const string product_648y = "com.mingyou.star.648y";
	private const string product_present_6y = "com.mingyou.star.present.6y";
	private const string product_gold_6y = "com.mingyou.star.gold.6y";
	private const string product_98y = "com.mingyou.star.98y";
	private const string product_6y = "com.mingyou.star.6y";
	private const string product_328y = "com.mingyou.star.328y";
	private const string product_gold_30y = "com.mingyou.star.gold.30y";
//    #elif UNITY_IPHONE
//    private const string product_gold_98y = "com.mingyou.mystarsingle.gold.98y";
//    private const string product_198y = "com.mingyou.mystarsingle.198y";
//    private const string product_30y = "com.mingyou.mystarsingle.30y";
//    private const string product_648y = "com.mingyou.mystarsingle.648y";
//    private const string product_present_6y = "com.mingyou.mystarsingle.present.6y";
//    private const string product_gold_6y = "com.mingyou.mystarsingle.gold.6y";
//    private const string product_98y = "com.mingyou.mystarsingle.98y";
//    private const string product_6y = "com.mingyou.mystarsingle.6y";
//    private const string product_328y = "com.mingyou.mystarsingle.328y";
//    private const string product_gold_30y = "com.mingyou.mystarsingle.gold.30y";
//    #endif

    private static IStoreController m_StoreController;
	private static IExtensionProvider m_StoreExtensionProvider;

	void Start () {
        Instance = this;
		if(GameDefines.OutputVerDefs == OutputVersionDefs.WPay){
			InitializePurchasing ();
		}

	}


	private void InitializePurchasing(){

		if (IsInitialized ()) {
			return;
		}

		var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
		//添加商品ID和类型 对应定义的商品ID
		builder.AddProduct (product_gold_98y, ProductType.Consumable);
		builder.AddProduct (product_198y, ProductType.Consumable);
		builder.AddProduct (product_30y, ProductType.Consumable);
		builder.AddProduct (product_648y, ProductType.Consumable);
		builder.AddProduct (product_present_6y, ProductType.Consumable);
		builder.AddProduct (product_gold_6y, ProductType.Consumable);
		builder.AddProduct (product_6y, ProductType.Consumable);
		builder.AddProduct (product_328y, ProductType.Consumable);
		builder.AddProduct (product_gold_30y, ProductType.Consumable);
		UnityPurchasing.Initialize (this, builder);
	}

	private bool IsInitialized ()
	{
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				m_StoreController.InitiatePurchase(product);
			}else
			{
				GUIRadarScan.Hide ();
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}else
		{
			GUIRadarScan.Hide ();
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void OnInitialized (IStoreController controller, IExtensionProvider extensions){
		Debug.Log ("OnInitialized : PASS");
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
	}

	public void OnInitializeFailed (InitializationFailureReason error){

		Debug.Log ("OnInitializeFailed InitializationFailureReason : " + error);
	}

	public void OnPurchaseFailed (Product i, PurchaseFailureReason p){
		GUIRadarScan.Hide ();
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", i.definition.storeSpecificId, p));
	}

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e){

		Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
		var unifiedReceipt = JsonUtility.FromJson<UnifiedReceipt>(e.purchasedProduct.receipt);
		Debug.Log("receipt : " + e.purchasedProduct.receipt);

		if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay) {
			if (unifiedReceipt != null && !string.IsNullOrEmpty (unifiedReceipt.Payload)) {
				Debug.Log ("Payload: " + unifiedReceipt.Payload);
				var purchaseReceipt = JsonUtility.FromJson<UnityChannelPurchaseReceipt> (unifiedReceipt.Payload);

				string orderId = ShopDataManager.PayCommodityData.orderId;
				string purchase_data = purchaseReceipt.json;
				string purchase_signature = purchaseReceipt.signature;

				Debug.Log ("purchase_data: " + purchase_data);
				Debug.Log ("purchase_signature: " + purchase_signature);
				NetSender.Instance.C2GSPayGooglePlayReq (orderId, purchase_data, purchase_signature);
			}
		} else if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore){
			string orderId = ShopDataManager.PayCommodityData.orderId;
			NetSender.Instance.RequestAppStoreChargeConfirm (orderId,e.purchasedProduct.receipt,e.purchasedProduct.transactionID);
		}
		return PurchaseProcessingResult.Complete;
	}
}
