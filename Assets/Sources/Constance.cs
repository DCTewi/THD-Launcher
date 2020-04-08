public static class Constance
{
    public static string DefaultProxySetting = @"
#协调服务器地址
#上海(主要)
mIp=122.152.202.54
#北京(备用)
#mIp=39.105.200.223 
#保定(备用)
#mIp=106.13.98.133

#协调服务器端口
mPort=33333

#本地服务器地址
sIp=127.0.0.1
#本地服务器端口
sPort=27015

#以下同上,不过是观战的映射(不需要的话在启动器中关闭即可)
ob_mIp=39.105.200.223
ob_mPort=33333
ob_sIp=127.0.0.1
ob_sPort=27020

";
}
