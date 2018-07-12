#import "IAPManager.h"
#import <UIKit/UIKit.h>
#import <sys/sysctl.h>
#import <ifaddrs.h>
#import <netdb.h>
#import <net/if.h>
#import <sys/socket.h>
#import <dlfcn.h>
#import <CommonCrypto/CommonDigest.h>
#import <CoreTelephony/CTCarrier.h>
#import <CoreTelephony/CTTelephonyNetworkInfo.h>
#import <AdSupport/ASIdentifierManager.h>
@implementation IAPManager

static NSString *buyOrderIdStr;

-(void) attachObserver{
    NSLog(@"AttachObserver");
    [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
}

-(BOOL) CanMakePayment{
    return [SKPaymentQueue canMakePayments];
}

-(void) requestProductData:(NSString *)productIdentifiers orderId:(NSString *)orderIdStr{
    buyOrderIdStr = [[NSString alloc]initWithString:orderIdStr];

	NSArray *product = [[NSArray alloc]initWithObjects:productIdentifiers,nil, nil];
    NSSet *nsset = [NSSet setWithArray:product];
    SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:nsset];
    request.delegate = self;
    [request start];
}


-(void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response{
    
    NSLog(@"-----------收到产品反馈信息--------------");
    NSArray *products = response.products;
    NSLog(@"产品Product ID:%@",response.invalidProductIdentifiers);
    NSLog(@"产品付费数量: %d", (int)[products count]);
    // populate UI

	SKProduct *mSKProduct = nil;
    for (SKProduct *p in products) {
        NSLog(@"product info");
        NSLog(@"SKProduct 描述信息%@", [products description]);
        NSLog(@"产品标题 %@" , p.localizedTitle);
        NSLog(@"产品描述信息: %@" , p.localizedDescription);
        NSLog(@"价格: %@" , p.price);
        NSLog(@"Product id: %@" , p.productIdentifier);
       
		mSKProduct = p;
    }
    SKMutablePayment *payment = [SKMutablePayment paymentWithProduct:mSKProduct];
    payment.applicationUsername = buyOrderIdStr;
    [[SKPaymentQueue defaultQueue] addPayment:payment];

    //[request autorelease];
}

-(NSString *)productInfo:(SKProduct *)product{
    NSArray *info = [NSArray arrayWithObjects:product.localizedTitle,product.localizedDescription,product.price,product.productIdentifier, nil];
    
    return [info componentsJoinedByString:@"\t"];
}

-(NSString *)transactionInfo:(SKPaymentTransaction *)transaction{
    
    return [self encode:(uint8_t *)transaction.transactionReceipt.bytes length:transaction.transactionReceipt.length];
}

-(NSString *)encode:(const uint8_t *)input length:(NSInteger) length{
    static char table[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    
    NSMutableData *data = [NSMutableData dataWithLength:((length+2)/3)*4];
    uint8_t *output = (uint8_t *)data.mutableBytes;
    
    for(NSInteger i=0; i<length; i+=3){
        NSInteger value = 0;
        for (NSInteger j= i; j<(i+3); j++) {
            value<<=8;
            
            if(j<length){
                value |=(0xff & input[j]);
            }
        }
        
        NSInteger index = (i/3)*4;
        output[index + 0] = table[(value>>18) & 0x3f];
        output[index + 1] = table[(value>>12) & 0x3f];
        output[index + 2] = (i+1)<length ? table[(value>>6) & 0x3f] : '=';
        output[index + 3] = (i+2)<length ? table[(value>>0) & 0x3f] : '=';
    }
    
    return [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
}

-(void) provideContent:(SKPaymentTransaction *)transaction{
   
    
}

//沙盒测试环境验证
#define SANDBOX @"https://sandbox.itunes.apple.com/verifyReceipt"
//正式环境验证
#define AppStore @"https://buy.itunes.apple.com/verifyReceipt"
/**
 *  验证购买，避免越狱软件模拟苹果请求达到非法购买问题
 *
 */
-(void)verifyPurchaseWithPaymentTransaction{
    //从沙盒中获取交易凭证并且拼接成请求体数据
    NSURL *receiptUrl=[[NSBundle mainBundle] appStoreReceiptURL];
    NSData *receiptData=[NSData dataWithContentsOfURL:receiptUrl];
    
    NSString *receiptString=[receiptData base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed];//转化为base64字符串
    
    NSString *bodyString = [NSString stringWithFormat:@"{\"receipt-data\" : \"%@\"}", receiptString];//拼接请求数据
    NSData *bodyData = [bodyString dataUsingEncoding:NSUTF8StringEncoding];
    
    
    //创建请求到苹果官方进行购买验证
    NSURL *url=[NSURL URLWithString:SANDBOX];
    NSMutableURLRequest *requestM=[NSMutableURLRequest requestWithURL:url];
    requestM.HTTPBody=bodyData;
    requestM.HTTPMethod=@"POST";
    //创建连接并发送同步请求
    NSError *error=nil;
    NSData *responseData=[NSURLConnection sendSynchronousRequest:requestM returningResponse:nil error:&error];
    if (error) {
        NSLog(@"验证购买过程中发生错误，错误信息：%@",error.localizedDescription);
        return;
    }
    NSDictionary *dic=[NSJSONSerialization JSONObjectWithData:responseData options:NSJSONReadingAllowFragments error:nil];
    NSLog(@"%@",dic);
    if([dic[@"status"] intValue]==0){
        NSLog(@"购买成功！");
        NSDictionary *dicReceipt= dic[@"receipt"];
        NSDictionary *dicInApp=[dicReceipt[@"in_app"] firstObject];
        NSString *productIdentifier= dicInApp[@"product_id"];//读取产品标识
        //如果是消耗品则记录购买数量，非消耗品则记录是否购买过
        NSUserDefaults *defaults=[NSUserDefaults standardUserDefaults];
        if ([productIdentifier isEqualToString:@"123"]) {
            int purchasedCount=[defaults integerForKey:productIdentifier];//已购买数量
            [[NSUserDefaults standardUserDefaults] setInteger:(purchasedCount+1) forKey:productIdentifier];
        }else{
            [defaults setBool:YES forKey:productIdentifier];
        }
        //在此处对购买记录进行存储，可以存储到开发商的服务器端
    }else{
        NSLog(@"购买失败，未通过验证！");
    }
}
//监听购买结果
- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transaction{
    for(SKPaymentTransaction *tran in transaction){
        switch (tran.transactionState) {
            case SKPaymentTransactionStatePurchased:{
                NSLog(@"交易完成");
                // 发送到苹果服务器验证凭证
               // [self verifyPurchaseWithPaymentTransaction];
               // [[SKPaymentQueue defaultQueue] finishTransaction:tran];

                [self completeTransaction:tran];
                NSLog(@"-----交易完成 --------");
            }
                break;
            case SKPaymentTransactionStatePurchasing:
                NSLog(@"商品添加进列表");
                
                break;
            case SKPaymentTransactionStateRestored:{
                NSLog(@"已经购买过商品");
                UnitySendMessage("U3dIOSReceiverObj","ReceivePurchaseFail", "已经购买过商品,请联系客服！");
                [[SKPaymentQueue defaultQueue] finishTransaction:tran];
            }
                break;
            case SKPaymentTransactionStateFailed:{
                NSLog(@"交易失败");
                UnitySendMessage("U3dIOSReceiverObj","ReceivePurchaseFail", "交易失败");
                [[SKPaymentQueue defaultQueue] finishTransaction:tran];
            }
                break;
            default:
                break;
        }
    }
}

-(void) failedTransaction:(SKPaymentTransaction *)transaction{
    NSLog(@"Failed transaction : %@",transaction.transactionIdentifier);
    
    if (transaction.error.code != SKErrorPaymentCancelled) {
        NSLog(@"!Cancelled");
    }
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
}

-(void) restoreTransaction:(SKPaymentTransaction *)transaction{
    NSLog(@"Restore transaction : %@",transaction.transactionIdentifier);
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
}

// 自定义函数，接受购买成功后的处理
- (void) completeTransaction: (SKPaymentTransaction *)transaction
{
    NSLog(@"-----completeTransaction--------");
    [self provideContent:transaction];
    // Your application should implement these two methods.
    // [transaction transactionReceipt]
    NSString *product = transaction.payment.productIdentifier;
    if ([product length] > 0) {
        [self recordTransaction:product];
        //[self provideContent:product];
        
        NSString *transactionIdentifier=transaction.transactionIdentifier;
        NSData *receipt=transaction.transactionReceipt;
        NSString *receiptString = [[NSString alloc] initWithData:receipt encoding:NSUTF8StringEncoding];
        NSString * errorString = @"Success";
        NSString * orderId = transaction.payment.applicationUsername;
        
        NSMutableDictionary* generalSettingsDict =  [NSMutableDictionary dictionary];
        [generalSettingsDict setValue:[NSNumber numberWithBool:true] forKey:@"result"];
        [generalSettingsDict setValue:errorString forKey:@"error"];
        [generalSettingsDict setValue:orderId forKey:@"orderId"];
        [generalSettingsDict setValue:product forKey:@"productId"];
        [generalSettingsDict setValue:transactionIdentifier forKey:@"cooOrderSerial"];
        [generalSettingsDict setValue:receiptString forKey:@"payDescription"];
        
        [self sendU3dMessage:@"paymentNotify" param:generalSettingsDict];
    }

    // Remove the transaction from the payment queue.
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
}

- (void) sendMacAddressIDAF
{
    NSString *adId = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
    NSString *macadress =[self getMacAddress];
    
    NSString *urlString = [NSString stringWithFormat:@"adid is @&" ,adId];
    NSLog(urlString);
    if (adId.length == 0)
    {
        NSLog(@"empty adid");
        adId = @"empty";
    }
    
    NSMutableDictionary* generalSettingsDict =  [NSMutableDictionary dictionary];
    [generalSettingsDict setValue:macadress forKey:@"macAdress"];
    [generalSettingsDict setValue:adId forKey:@"idaf"];
    
    [self sendU3dMessage:@"macAdressIDAF" param:generalSettingsDict];
}


- (NSString *)getMacAddress
{
    int                    mib[6];
    size_t                len;
    char                *buf;
    unsigned char        *ptr;
    struct if_msghdr    *ifm;
    struct sockaddr_dl    *sdl;
    
    mib[0] = CTL_NET;
    mib[1] = AF_ROUTE;
    mib[2] = 0;
    mib[3] = AF_LINK;
    mib[4] = NET_RT_IFLIST;
    
    if ((mib[5] = if_nametoindex("en0")) == 0) {
        printf("Error: if_nametoindex error/n");
        return NULL;
    }
    
    if (sysctl(mib, 6, NULL, &len, NULL, 0) < 0) {
        printf("Error: sysctl, take 1/n");
        return NULL;
    }
    
    if ((buf = (char*)malloc(len)) == NULL) {
        printf("Could not allocate memory. error!/n");
        return NULL;
    }
    
    if (sysctl(mib, 6, buf, &len, NULL, 0) < 0) {
        printf("Error: sysctl, take 2");
        return NULL;
    }
    
    ifm = (struct if_msghdr *)buf;
    sdl = (struct sockaddr_dl *)(ifm + 1);
    ptr = (unsigned char *)(sdl);
    NSString *outstring = [NSString stringWithFormat:@"%02x:%02x:%02x:%02x:%02x:%02x", *ptr, *(ptr+1), *(ptr+2), *(ptr+3), *(ptr+4), *(ptr+5)];
    free(buf);
    return [outstring uppercaseString];
}

-(void)sendU3dMessage:(NSString*)msgName param:(NSDictionary*)dict
{
    // Use key1=val1&key2=val2&key3=val3 to combine a NSString.
    NSString* param = @"";
    for (NSString *key in dict)
    {
        if ([param length] == 0) {
            param = [param stringByAppendingFormat:@"%@#%@", key, [dict valueForKey:key]];
        }
        else {
            param = [param stringByAppendingFormat:@"&&&%@#%@", key, [dict valueForKey:key]];
        }
    }
    
    UnitySendMessage("U3dIOSReceiverObj", [msgName UTF8String], [param UTF8String]);
}
//记录交易
-(void)recordTransaction:(NSString *)product{
    NSLog(@"-----记录交易--------");
}


void U3dSavePhoth(const char* readAddr)
{
    NSString *strReadAddr = [NSString stringWithUTF8String:readAddr];
    UIImage *img = [UIImage imageWithContentsOfFile:strReadAddr];
    NSLog(@"%@", [NSString stringWithFormat:@"w:%f, h:%f", img.size.width, img.size.height]);
    IAPManager *instance = [IAPManager alloc];
    UIImageWriteToSavedPhotosAlbum(img, instance,
                                   @selector(imageSaved:didFinishSavingWithError:contextInfo:), nil);
}

- ( void ) imageSaved: ( UIImage *) image didFinishSavingWithError:( NSError *)error
          contextInfo: ( void *) contextInfo
{
    NSLog(@"保存结束");
    if (error != nil) {
        NSLog(@"有错误");
        NSMutableDictionary* generalSettingsDict =  [NSMutableDictionary dictionary];
        [generalSettingsDict setValue:[NSNumber numberWithBool:false] forKey:@"result"];
        [self sendU3dMessage:@"SavePhoto" param:generalSettingsDict];
    }
    else{
        NSLog(@"保存成功");
        NSMutableDictionary* generalSettingsDict =  [NSMutableDictionary dictionary];
        [generalSettingsDict setValue:[NSNumber numberWithBool:true] forKey:@"result"];
        [self sendU3dMessage:@"SavePhoto" param:generalSettingsDict];
    }
}

@end
