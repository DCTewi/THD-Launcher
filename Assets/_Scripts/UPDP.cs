using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;
using UnityEngine;

public class UPDP : MonoBehaviour
{
    public static int eport = 27015;
    public static int iport = 27015;

    public static IPAddress localipv4;
    public static string localname;
    public static string exteripv4;

    public static string regex = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";

    public static string GetLocalIP()
    {
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.ToString().StartsWith("10.80.") || ip.ToString().Contains("."))
            {
                return ip.ToString();
            }
        }
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
    }

    public static string GetExternalIP()
    {
        string tempip = "unknown";

        WebClient client = new WebClient();
        client.Encoding = Encoding.Default;
        string all = client.DownloadString("http://www.taobao.com/help/getip.php");
        client.Dispose();

        int start = all.IndexOf("\"") + 1;
        int end = all.IndexOf("\"", start);
        tempip = all.Substring(start, end - start);

        return tempip;
    }

    public void Create()
    {
        upnpLib upnp = new upnpLib(upnpLib.InternetConnectTypes.IPv4);
        upnpLib.Result r = upnp.addPortMapping(GetLocalIP(), eport.ToString(), "UDP", iport.ToString(), "thd server", "0");
        Debug.Log("Host " + ((r.Status == upnpLib.ResultTypes.Success) ? "Successed!" : "Failed!"));
        Debug.Log("External Port: " + eport);
        Debug.Log("External ip: " + GetExternalIP());
    }
}

public sealed class upnpLib
{
    public enum InternetConnectTypes { IPv4, IPv6 }
    public enum ResultTypes { Success, Faild }
    public class RootDevice
    {
        #region -- Private Fields --
        private string _location;
        private string _host;
        private string _port;
        private string _server;
        private int _maxAge;
        public string _USN;
        #endregion

        #region -- Properties --
        public string Location
        {
            get { return this._location; }
            set { this._location = value; }
        }
        public string Host
        {
            get { return this._host; }
            set { this._host = value; }
        }
        public string Port
        {
            get { return this._port; }
            set { this._port = value; }
        }
        public string Server
        {
            get { return this._server; }
            set { this._server = value; }
        }
        public int MaxAge
        {
            get { return this._maxAge; }
            set { this._maxAge = value; }
        }
        public string USN
        {
            get { return this._USN; }
            set { this._USN = value; }
        }
        #endregion

        #region -- Method --
        public RootDevice(string location)
        {
            this.Host = location.Substring(location.IndexOf('/') + 2, location.IndexOf('/', location.IndexOf('/') + 2) - (location.IndexOf('/') + 2));
            this.Port = this.Host.Substring(this.Host.IndexOf(':') + 1);
            this.Host = this.Host.Substring(0, this.Host.IndexOf(':'));
            this.Location = location.Substring(location.IndexOf('/', location.IndexOf('/') + 2));
        }
        #endregion
    }
    public class DeviceService
    {
        #region -- Private Fields --
        private string _type;
        private string _id;
        private string _scpdUrl;
        private string _controlUrl;
        private string _eventSubUrl;
        private string _baseUrl;
        #endregion

        #region -- Properties --
        public string Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        public string ID
        {
            get { return this._id; }
            set { this._id = value; }
        }
        public string SCPDUrl
        {
            get { return this._scpdUrl; }
            set { this._scpdUrl = value; }
        }
        public string ControlUrl
        {
            get { return this._controlUrl; }
            set { this._controlUrl = value; }
        }
        public string EventSubUrl
        {
            get { return this._eventSubUrl; }
            set { this._eventSubUrl = value; }
        }
        public string BaseUrl
        {
            get { return this._baseUrl; }
            set { this._baseUrl = value; }
        }
        #endregion
    }
    public class ServicePoint
    {
        #region -- Private Fields --
        private string _name;
        private IList<Argument> _argumentList = new List<Argument>();
        #endregion

        #region -- Properties --
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        public IList<Argument> ArgumentList
        {
            get { return this._argumentList; }
            set { this._argumentList = value; }
        }
        #endregion
    }
    public class Argument
    {
        #region -- Private Fields --
        private string _name;
        private string _direction;
        private string _relatedStateVariable;
        #endregion

        #region -- Properties --
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        public string Direction
        {
            get { return this._direction; }
            set { this._direction = value; }
        }
        public string RelatedStateVariable
        {
            get { return this._relatedStateVariable; }
            set { this._relatedStateVariable = value; }
        }
        #endregion
    }
    public class StateVariable
    {
        #region -- Private Fields --
        private string _sendEvents;
        private string _name;
        private string _dataType;
        private IList<string> _allowedValueList = new List<string>();
        #endregion

        #region -- Properties --
        public string SendEvents
        {
            get { return this._sendEvents; }
            set { this._sendEvents = value; }
        }
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        public string DataType
        {
            get { return this._dataType; }
            set { this._dataType = value; }
        }
        public IList<string> AllowedValueList
        {
            get { return this._allowedValueList; }
            set { this._allowedValueList = value; }
        }
        #endregion
    }
    public class Result
    {
        #region -- Private Fields--
        private ResultTypes _status;
        private string _message;
        #endregion

        #region -- Properties --
        public ResultTypes Status
        {
            get { return this._status; }
            set { this._status = value; }
        }
        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }
        #endregion
    }

    #region -- Private Fields --
    private InternetConnectTypes _ict;
    private Result r = new Result();
    private DeviceService service = null;
    private IList<RootDevice> devices = null;
    private IList<DeviceService> services = null;
    private IList<ServicePoint> _actions = null;
    private IList<StateVariable> _stateVariables = null;
    #endregion

    #region -- Properties --
    public InternetConnectTypes InternetConnectType
    {
        get { return this._ict; }
        set { this._ict = value; }
    }
    public IList<ServicePoint> ActionList
    {
        get { return this._actions; }
    }
    public IList<StateVariable> StateVariableList
    {
        get { return this._stateVariables; }
    }
    #endregion

    #region -- Method --
    public upnpLib(InternetConnectTypes ict)
    {
        this.InternetConnectType = ict;
        r.Status = ResultTypes.Success;
        try
        {
            this.devices = this.discoverDevices(5);
            if (this.devices.Count == 0)
            {
                throw new Exception("没有发现可用uPnP设备!");
            }
            this.services = this.getDeviceSvc(this.getDeviceDesc(this.devices[0]));
            foreach (DeviceService s in this.services)
            {
                if (s.Type.Contains("service:WANIPConnection"))
                {
                    this.service = s;
                    string Xml = this.getServiceDesc(this.devices[0], s);
                    this._actions = this.getServicePoint(Xml);
                    this._stateVariables = this.GetStateVariables(Xml);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            r.Status = ResultTypes.Faild;
            r.Message = ex.Message;
        }
    }
    private IList<RootDevice> discoverDevices(int timeOut)
    {
        #region -- Send UDP data --
        string strData = "M-SEARCH * HTTP/1.1\r\n" +
                         "HOST: 239.255.255.250:1900\r\n" +
                         "MAN:\"ssdp:discover\"\r\n" +
                         "MX:" + timeOut.ToString() + "\r\n" +
                         "ST:upnp:rootdevice\r\n\r\n";
        byte[] data = Encoding.UTF8.GetBytes(strData);
        UdpClient uc = new UdpClient(this.InternetConnectType == InternetConnectTypes.IPv4 ? AddressFamily.InterNetwork : AddressFamily.InterNetworkV6);
        IPEndPoint IPep = new IPEndPoint(IPAddress.Broadcast, 1900);
        uc.Send(data, data.Length, IPep);
        Timer RecvTimeOut = new Timer(this.RecvTimeOutFunc, uc, timeOut * 1000, timeOut * 1000);
        IList<RootDevice> hints = new List<RootDevice>();
        #endregion

        #region -- Recive response --
        byte[] buffer = null;
        int start, end;
        string temp = "", find = "";
        RootDevice hint;
        while (buffer == null || buffer.Length == 0)
        {
            buffer = uc.Receive(ref IPep);
            if (buffer != null && buffer.Length > 0)
            {
                strData = Encoding.UTF8.GetString(buffer);
                if (!strData.Contains("upnp:rootdevice")) continue;
                // Location
                temp = ""; find = "LOCATION:";
                start = strData.IndexOf(find);
                if (start > -1)
                {
                    end = strData.IndexOf("\r\n", start);
                    temp = strData.Substring(start + find.Length, end - (start + find.Length)).Trim();
                    hint = new RootDevice(temp);
                }
                else
                {
                    continue;
                }
                // Max age
                temp = ""; find = "max-age=";
                start = strData.IndexOf(find);
                if (start > -1)
                {
                    end = strData.IndexOf("\r\n", start);
                    temp = strData.Substring(start + find.Length, end - (start + find.Length)).Trim();
                    hint.MaxAge = int.Parse(temp);
                }
                else
                {
                    continue;
                }
                // Server
                temp = ""; find = "SERVER:";
                start = strData.IndexOf(find);
                if (start > -1)
                {
                    end = strData.IndexOf("\r\n", start);
                    temp = strData.Substring(start + find.Length, end - (start + find.Length)).Trim();
                    hint.Server = temp;
                }
                else
                {
                    continue;
                }
                // USN
                temp = ""; find = "USN:";
                start = strData.IndexOf(find);
                if (start > -1)
                {
                    end = strData.IndexOf("\r\n", start);
                    temp = strData.Substring(start + find.Length, end - (start + find.Length)).Trim();
                    hint.USN = temp;
                }
                else
                {
                    continue;
                }
                hints.Add(hint);
                break;
            }
        }
        #endregion

        uc.Close();
        return hints;
    }
    private void RecvTimeOutFunc(object uc)
    {
        ((UdpClient)uc).Close();
    }
    private string getDeviceDesc(RootDevice rd)
    {
        string strData = "GET " + rd.Location + " HTTP/1.1\r\n" +
                         "HOST:" + rd.Host + ":" + rd.Port + "\r\n" +
                         "ACCEPT-LANGUAGE: \r\n\r\n", result = "";
        byte[] data = Encoding.UTF8.GetBytes(strData);
        IPAddress ipadr = (Dns.GetHostAddresses(rd.Host))[0];
        IPEndPoint IPep = new IPEndPoint(ipadr, int.Parse(rd.Port));
        TcpClient tc = new TcpClient(this.InternetConnectType == InternetConnectTypes.IPv4 ? AddressFamily.InterNetwork : AddressFamily.InterNetworkV6);
        NetworkStream ns = null;
        try
        {
            tc.Connect(IPep);
            ns = tc.GetStream();
            ns.Write(data, 0, data.Length);
            Thread.Sleep(100);
            int ReadSize = 2048;
            byte[] buff = new byte[ReadSize], readBuff;
            strData = "";
            while (ReadSize == 2048)
            {
                ReadSize = ns.Read(buff, 0, buff.Length);
                readBuff = new byte[ReadSize];
                Array.Copy(buff, 0, readBuff, 0, ReadSize);
                strData += Encoding.UTF8.GetString(readBuff);
            }
            result = strData.Substring(strData.IndexOf("\r\n\r\n") + 4).Trim();
            while (result.Substring(result.Length - 2) == "\r\n" || result.Substring(result.Length - 2) == Encoding.Default.GetString(new byte[2] { 0, 0 }))
            {
                result = result.Substring(0, result.Length - 2);
            }
        }
        catch { }
        finally
        {
            if (ns != null)
            {
                ns.Close();
                ns.Dispose();
            }
            if (tc != null)
            {
                tc.Close();
            }
        }
        return result;
    }
    private IList<DeviceService> getDeviceSvc(string Xml)
    {
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(Xml));
        XmlReader xr = XmlReader.Create(ms);
        IList<DeviceService> hints = new List<DeviceService>();
        string URLBase = "";
        while (xr.Read())
        {
            if (xr.NodeType == XmlNodeType.Element && xr.Name == "URLBase")
            {
                do
                {
                    xr.Read();
                    if (xr.NodeType == XmlNodeType.Text)
                    {
                        URLBase = xr.Value;
                    }
                } while (xr.NodeType != XmlNodeType.EndElement || xr.Name != "URLBase");
                continue;
            }
            if (xr.NodeType == XmlNodeType.Element && xr.Name == "service")
            {
                DeviceService s = new DeviceService();
                s.BaseUrl = URLBase;
                string curElement = string.Empty;
                do
                {
                    xr.Read();
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        curElement = xr.Name;
                        continue;
                    }
                    if (xr.NodeType == XmlNodeType.EndElement)
                    {
                        curElement = null;
                        continue;
                    }
                    if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (curElement)
                        {
                            case "serviceType":
                                s.Type = xr.Value;
                                break;
                            case "serviceId":
                                s.ID = xr.Value;
                                break;
                            case "SCPDURL":
                                s.SCPDUrl = xr.Value;
                                break;
                            case "controlURL":
                                s.ControlUrl = xr.Value;
                                break;
                            case "eventSubURL":
                                s.EventSubUrl = xr.Value;
                                break;
                            default:
                                break;
                        };
                    }
                } while (xr.NodeType != XmlNodeType.EndElement || xr.Name != "service");
                hints.Add(s);
            }
        }
        return hints;
    }
    private string getServiceDesc(RootDevice rd, DeviceService ds)
    {
        string strData = "GET " + ds.BaseUrl + ds.SCPDUrl + " HTTP/1.1\r\n" +
                         "HOST:" + rd.Host + ":" + rd.Port + "\r\n" +
                         "ACCEPT-LANGUAGE: \r\n\r\n", result = "";
        byte[] data = Encoding.UTF8.GetBytes(strData);
        IPAddress ipadr = (Dns.GetHostAddresses(rd.Host))[0];
        IPEndPoint IPep = new IPEndPoint(ipadr, int.Parse(rd.Port));
        TcpClient tc = new TcpClient(this.InternetConnectType == InternetConnectTypes.IPv4 ? AddressFamily.InterNetwork : AddressFamily.InterNetworkV6);
        NetworkStream ns = null;
        try
        {
            tc.Connect(IPep);
            ns = tc.GetStream();
            ns.Write(data, 0, data.Length);
            Thread.Sleep(100);
            int ReadSize = 2048;
            byte[] buff = new byte[ReadSize], readBuff;
            strData = "";
            while (ReadSize == 2048)
            {
                ReadSize = ns.Read(buff, 0, buff.Length);
                readBuff = new byte[ReadSize];
                Array.Copy(buff, 0, readBuff, 0, ReadSize);
                strData += Encoding.UTF8.GetString(readBuff);
            }
            result = strData.Substring(strData.IndexOf("\r\n\r\n") + 4).Trim();
            while (result.Substring(result.Length - 2) == "\r\n" || result.Substring(result.Length - 2) == Encoding.Default.GetString(new byte[2] { 0, 0 }))
            {
                result = result.Substring(0, result.Length - 2);
            }
        }
        catch { }
        finally
        {
            if (ns != null)
            {
                ns.Close();
                ns.Dispose();
            }
            if (tc != null)
            {
                tc.Close();
            }
        }
        return result;
    }
    private IList<ServicePoint> getServicePoint(string Xml)
    {
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(Xml));
        XmlReader xr = XmlReader.Create(ms);
        IList<ServicePoint> hints = new List<ServicePoint>();
        while (xr.Read())
        {
            if (xr.NodeType == XmlNodeType.Element && xr.Name == "action")
            {
                ServicePoint sp = new ServicePoint();
                string curElement = string.Empty;
                do
                {
                    xr.Read();
                    if (xr.NodeType == XmlNodeType.Element && xr.Name != "argumentList")
                    {
                        curElement = xr.Name;
                        continue;
                    }
                    if (xr.NodeType == XmlNodeType.EndElement && xr.Name != "argumentList")
                    {
                        curElement = null;
                        continue;
                    }
                    if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (curElement)
                        {
                            case "name":
                                sp.Name = xr.Value;
                                break;
                            default:
                                break;
                        };
                        continue;
                    }
                    if (xr.NodeType == XmlNodeType.Element && xr.Name == "argumentList")
                    {
                        string curElement2 = string.Empty;
                        do
                        {
                            xr.Read();
                            if (xr.NodeType == XmlNodeType.Element && xr.Name == "argument")
                            {
                                Argument arg = new Argument();
                                do
                                {
                                    xr.Read();
                                    if (xr.NodeType == XmlNodeType.Element)
                                    {
                                        curElement2 = xr.Name;
                                        continue;
                                    }
                                    if (xr.NodeType == XmlNodeType.EndElement)
                                    {
                                        curElement2 = null;
                                        continue;
                                    }
                                    if (xr.NodeType == XmlNodeType.Text)
                                    {
                                        switch (curElement2)
                                        {
                                            case "name":
                                                arg.Name = xr.Value;
                                                break;
                                            case "direction":
                                                arg.Direction = xr.Value;
                                                break;
                                            case "relatedStateVariable":
                                                arg.RelatedStateVariable = xr.Value;
                                                break;
                                            default:
                                                break;
                                        };
                                    }
                                } while (xr.NodeType != XmlNodeType.EndElement || xr.Name != "argument");
                                sp.ArgumentList.Add(arg);
                            }
                        } while (xr.NodeType != XmlNodeType.EndElement || xr.Name != "argumentList");
                    }
                } while (xr.NodeType != XmlNodeType.EndElement || xr.Name != "action");
                hints.Add(sp);
            }
        }
        return hints;
    }
    private IList<StateVariable> GetStateVariables(string Xml)
    {
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(Xml));
        XmlReader xr = XmlReader.Create(ms);
        IList<StateVariable> hints = new List<StateVariable>();
        while (xr.Read())
        {
            if (xr.NodeType == XmlNodeType.Element && xr.Name == "stateVariable")
            {
                StateVariable hint = new StateVariable();
                hint.SendEvents = xr.GetAttribute("sendEvents");
                string curElement = string.Empty;
                do
                {
                    xr.Read();
                    if (xr.NodeType == XmlNodeType.Element && xr.Name != "allowedValueList")
                    {
                        curElement = xr.Name;
                        continue;
                    }
                    if (xr.NodeType == XmlNodeType.EndElement && xr.Name != "allowedValueList")
                    {
                        curElement = null;
                        continue;
                    }
                    if (xr.NodeType == XmlNodeType.Text)
                    {
                        switch (curElement)
                        {
                            case "name":
                                hint.Name = xr.Value;
                                break;
                            case "dataType":
                                hint.DataType = xr.Value;
                                break;
                            default:
                                break;
                        };
                        continue;
                    }
                    if (xr.NodeType == XmlNodeType.Element && xr.Name == "allowedValueList")
                    {
                        string curElement2 = string.Empty;
                        do
                        {
                            xr.Read();
                            if (xr.NodeType == XmlNodeType.Element)
                            {
                                curElement2 = xr.Name;
                                continue;
                            }
                            if (xr.NodeType == XmlNodeType.EndElement)
                            {
                                curElement2 = null;
                                continue;
                            }
                            if (xr.NodeType == XmlNodeType.Text)
                            {
                                switch (curElement2)
                                {
                                    case "allowedValue":
                                        hint.AllowedValueList.Add(xr.Value);
                                        break;
                                    default:
                                        break;
                                };
                            }
                        } while (xr.NodeType != XmlNodeType.EndElement || xr.Name != "allowedValueList");
                    }
                } while (xr.NodeType != XmlNodeType.EndElement || xr.Name != "stateVariable");
                hints.Add(hint);
            }
        }
        return hints;
    }
    private void makeAddPortMappingSOAP(RootDevice dev, DeviceService srv, string NewRemoteHost, string NewExternalPort, string NewProtocol, string NewInternalPort, string NewPortMappingDescription, string NewLeaseDuration)
    {
        string InternalAddress = string.Empty;
        IPAddress[] addrs = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress addr in addrs)
        {
            if (addr.AddressFamily == AddressFamily.InterNetwork)
            {
                InternalAddress = addr.ToString();
            }
        }
        string strData = "<u:AddPortMapping xmlns:u=\"" + srv.Type + "\">" +
                      "<NewRemoteHost></NewRemoteHost>" +
                      "<NewExternalPort>" + NewExternalPort + "</NewExternalPort>" +
                      "<NewProtocol>" + NewProtocol + "</NewProtocol>" +
                      "<NewInternalPort>" + NewInternalPort + "</NewInternalPort>" +
                      "<NewInternalClient>" + InternalAddress + "</NewInternalClient>" +
                      "<NewEnabled>1</NewEnabled>" +
                      "<NewPortMappingDescription>" + NewPortMappingDescription + "</NewPortMappingDescription>" +
                      "<NewLeaseDuration>" + NewLeaseDuration + "</NewLeaseDuration>" +
                      "</u:AddPortMapping>";
        string result = PostSOAP(dev, srv, strData, "AddPortMapping");
    }
    private void makeDeletePortMappingSOAP(RootDevice dev, DeviceService srv, string NewRemoteHost, string NewExternalPort, string NewProtocol)
    {
        string strData = "<u:DeletePortMapping xmlns:u=\"" + srv.Type + "\">" +
                         "<NewRemoteHost>" + NewRemoteHost + "</NewRemoteHost>" +
                         "<NewExternalPort>" + NewExternalPort + "</NewExternalPort>" +
                         "<NewProtocol>" + NewProtocol + "</NewProtocol>" +
                         "</u:DeletePortMapping>";
        string result = PostSOAP(dev, srv, strData, "DeletePortMapping");
    }
    private string makeGetExternalIPAddress(RootDevice dev, DeviceService srv)
    {
        string strData = "<u:GetExternalIPAddress xmlns:u=\"" + srv.Type + "\">" +
                      "</u:GetExternalIPAddress>";
        string result = PostSOAP(dev, srv, strData, "GetExternalIPAddress");
        if (result.IndexOf("<NewExternalIPAddress>") > -1)
        {
            result = result.Substring(result.IndexOf("<NewExternalIPAddress>") + 22, result.IndexOf("</NewExternalIPAddress>") - (result.IndexOf("<NewExternalIPAddress>") + 22));
        }
        else
        {
            result = "";
        }
        return result;
    }
    private string PostSOAP(RootDevice dev, DeviceService srv, string soapData, string action)
    {
        string soap = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                         "<s:Body>" + soapData + "</s:Body>" +
                         "</s:Envelope>";

        string strData = "POST " + srv.BaseUrl + srv.ControlUrl + " HTTP/1.1\r\n" +
                   "HOST: " + dev.Host + ":" + dev.Port + "\r\n" +
                   "Content-Length: " + Encoding.UTF8.GetBytes(soap).Length.ToString() + "\r\n" +
                   "CONTENT-TYPE: text/xml; charset=\"utf-8\"\r\n" +
                   "SOAPACTION: \"" + srv.Type + "#" + action + "\"\r\n\r\n" + soap;

        byte[] data = Encoding.Default.GetBytes(strData);
        IPAddress ipaddr = (Dns.GetHostAddresses(dev.Host))[0];
        IPEndPoint IPep = new IPEndPoint(ipaddr, int.Parse(dev.Port));
        TcpClient tc = new TcpClient(AddressFamily.InterNetwork);
        NetworkStream ns = null;
        try
        {
            tc.Connect(IPep);
            ns = tc.GetStream();
            ns.Write(data, 0, data.Length);
            byte[] buffer = new byte[4096], readBuff;
            int ReadSize = ns.Read(buffer, 0, buffer.Length);
            readBuff = new byte[ReadSize];
            Array.Copy(buffer, 0, readBuff, 0, ReadSize);
            strData = Encoding.Default.GetString(readBuff);
        }
        catch { }
        finally
        {
            if (ns != null)
            {
                ns.Close();
            }
            tc.Close();
        }
        return strData;
    }
    public Result addPortMapping(string NewRemoteHost, string NewExternalPort, string NewProtocol, string NewInternalPort, string NewPortMappingDescription, string NewLeaseDuration)
    {
        if (this.r.Status == ResultTypes.Success)
        {
            try
            {
                this.makeAddPortMappingSOAP(this.devices[0], this.service, NewRemoteHost, NewExternalPort, NewProtocol, NewInternalPort, NewPortMappingDescription, NewLeaseDuration);
            }
            catch (Exception ex)
            {
                r.Status = ResultTypes.Faild;
                r.Message = ex.Message;
            }
            return this.r;
        }
        else
        {
            return this.r;
        }
    }
    public Result deletePortMapping(string NewRemoteHost, string NewExternalPort, string NewProtocol)
    {
        if (this.r.Status == ResultTypes.Success)
        {
            try
            {
                this.makeDeletePortMappingSOAP(this.devices[0], this.service, NewRemoteHost, NewExternalPort, NewProtocol);
            }
            catch (Exception ex)
            {
                r.Status = ResultTypes.Faild;
                r.Message = ex.Message;
            }
            return this.r;
        }
        else
        {
            return this.r;
        }
    }
    public Result getExternalIPAddress()
    {
        if (this.r.Status == ResultTypes.Success)
        {
            try
            {
                r.Message = this.makeGetExternalIPAddress(this.devices[0], this.service);
            }
            catch (Exception ex)
            {
                r.Status = ResultTypes.Faild;
                r.Message = ex.Message;
            }
            return this.r;
        }
        else
        {
            return this.r;
        }
    }
    #endregion
}
