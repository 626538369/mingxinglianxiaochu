//
//  IAPManager.h
//  Unity-iPhone
//
//  Created by MacMini on 14-5-16.
//
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface IAPManager : NSObject<SKProductsRequestDelegate, SKPaymentTransactionObserver>{
    SKProduct *proUpgradeProduct;
    SKProductsRequest *productsRequest;
}

-(void)attachObserver;
-(BOOL)CanMakePayment;
-(void)requestProductData:(NSString *)productIdentifiers orderId:(NSString *)orderIdStr;
-(void)completeTransaction: (SKPaymentTransaction *)transaction;
-(void) sendMacAddressIDAF;
@end
