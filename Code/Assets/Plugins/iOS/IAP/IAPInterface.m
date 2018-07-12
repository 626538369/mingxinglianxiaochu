#import "IAPInterface.h"
#import "IAPManager.h"

@implementation IAPInterface

IAPManager *iapManager = nil;
//初始化商品信息
void InitIAPManager(){
    iapManager = [[IAPManager alloc] init];
    [iapManager attachObserver];
    [iapManager sendMacAddressIDAF];
}
//判断是否可以购买
bool IsProductAvailable(){
    return [iapManager CanMakePayment];
}
//获取商品信息 - 购买商品
void RequstProductInfo(void *p , void *orderIdStr){
    NSString *list = [NSString stringWithUTF8String:p];
    NSLog(@"商品列表:%@",list);
    NSString *list1 = [NSString stringWithUTF8String:orderIdStr];
    [iapManager requestProductData:list orderId:list1];
}

@end
