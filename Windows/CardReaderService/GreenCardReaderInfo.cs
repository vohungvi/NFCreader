using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CardReaderService
{
    public class GreenCardReaderInfo : IGreenCardReaderInfo
    {
        /// <summary>
        /// Type can be 'ModWinsCard', 'Tcp Ip Client', 'Tcp Ip Server', 'Remode Card'
        /// </summary>
        public string Type { get; set; }
        public string CallName { get; set; }
        /// <summary>
        /// Using when Type ="ModWinsCard"
        /// </summary>
        public string DeviceName { get; set; }
        public string SerialNumber { get; set; }
        /// <summary>
        /// Using when Type in ['Tcp Ip Client', 'Tcp Ip Server']
        /// </summary>
        public string TcpIp { get; set; }
        public ushort Port { get; set; }
        public byte ActiveCode { get; set; }
        public byte InactiveCode { get; set; }
    }
   
    public class ModWinsCardReader : IGreenCardReader
    {
        private int _context;
        private int _cardHandle;
        private bool _canRead;
        private DateTime connectTime = DateTime.Now;
        private int _activeProtocol;
        SCARD_IO_REQUEST _sendRequest = new SCARD_IO_REQUEST();
        SCARD_IO_REQUEST _recvRequest = new SCARD_IO_REQUEST();
        byte[] _recvBuff;
        byte[] _sendBuff;
        Task _task;
        public ModWinsCardReader(IGreenCardReaderInfo info)
        {
            this._info = new GreenCardReaderInfo()
            {
                Type = "ModWinsCard",
                SerialNumber = info.SerialNumber
            };
            _cardHandle = -1;
            _activeProtocol = -1;
            _recvBuff = new byte[128];
            _sendBuff = new byte[128];
            this._state = CardState.IsDisable;
            GetReady();
            ReleaseContext();
        }
        private void ReleaseContext()
        {
            int retCode = ModWinsCard.SCardCancel(_context);
            //if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            //    Console.WriteLine("Cancel failed");

            retCode = ModWinsCard.SCardReleaseContext(_context);
            //if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            //    Console.WriteLine("Release failed");
        }
        public static List<string> ListModWinsCards()
        {
            int _context = -1;
            List<string> lst = new List<string>();
            int retCode = ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, ref _context);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                return null;
            }
            if (_context == -1)
            {
                return null;
            }
            int readerCount = 255;

            Byte[] bytes = new Byte[readerCount];

            retCode = ModWinsCard.SCardListReaders(_context, null, bytes, ref readerCount);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                return null;
            }

            try
            {
                string[] readerArr = System.Text.ASCIIEncoding.ASCII.GetString(bytes, 0, readerCount).Split('\0');
                foreach (string readerName in readerArr)
                {
                    if (!string.IsNullOrEmpty(readerName) && readerName.Length > 1)
                    {
                        // http://stackoverflow.com/questions/6940824/getting-pcsc-reader-serial-number-with-winscard
                        int readerHandle = 0;

                        int protocol = 0;
                        int ret = ModWinsCard.SCardConnect(_context, readerName, ModWinsCard.SCARD_SHARE_DIRECT, ModWinsCard.SCARD_PROTOCOL_UNDEFINED, ref readerHandle, ref protocol);

                        byte[] data = new byte[128];
                        int leng = 128;
                        ret = ModWinsCard.SCardGetAttrib(readerHandle, ModWinsCard.SCARD_ATTR_VENDOR_IFD_SERIAL_NO, data, ref leng);

                        string serialNo = System.Text.ASCIIEncoding.ASCII.GetString(data, 0, leng);

                        //int b = ModWinsCard.SCardFreeMemory(_context, data);

                        ModWinsCard.SCardDisconnect(readerHandle, ModWinsCard.SCARD_LEAVE_CARD);

                        lst.Add(serialNo);
                    }
                }
            }
            catch
            {
                return null;
            }
            return lst;
        }
        private void GetReady()
        {
            int retCode = ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, ref _context);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                this._canRead = false;
                return;
            }
            if (_context == -1)
            {
                this._canRead = false;
                return;
            }
            int readerCount = 255;

            Byte[] bytes = new Byte[readerCount];

            retCode = ModWinsCard.SCardListReaders(_context, null, bytes, ref readerCount);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                this._canRead = false;
                return;
            }

            try
            {
                string[] readerArr = System.Text.ASCIIEncoding.ASCII.GetString(bytes, 0, readerCount).Split('\0');
                foreach (string readerName in readerArr)
                {
                    GetSerialNumber(readerName, (res, b) =>
                    {
                        if (b && res == this._info.SerialNumber)
                        {
                            this._info.DeviceName = readerName;
                            this._canRead = true;
                            return;
                        }
                    });
                }
            }
            catch
            {
                this._canRead = false;
                return;
            }

        }
        private void GetSerialNumber(string ModWinsDeviceName, Action<string, bool> complete)
        {
            if (!string.IsNullOrEmpty(ModWinsDeviceName) && ModWinsDeviceName.Length > 1)
            {
                // http://stackoverflow.com/questions/6940824/getting-pcsc-reader-serial-number-with-winscard
                int readerHandle = 0;

                int protocol = 0;
                int ret = ModWinsCard.SCardConnect(_context, ModWinsDeviceName, ModWinsCard.SCARD_SHARE_DIRECT, ModWinsCard.SCARD_PROTOCOL_UNDEFINED, ref readerHandle, ref protocol);

                byte[] data = new byte[128];
                int leng = 128;
                ret = ModWinsCard.SCardGetAttrib(readerHandle, ModWinsCard.SCARD_ATTR_VENDOR_IFD_SERIAL_NO, data, ref leng);

                string serialNo = System.Text.ASCIIEncoding.ASCII.GetString(data, 0, leng);

                //int b = ModWinsCard.SCardFreeMemory(_context, data);

                ModWinsCard.SCardDisconnect(readerHandle, ModWinsCard.SCARD_LEAVE_CARD);

                if (complete != null)
                    if (ret != ModWinsCard.SCARD_S_SUCCESS)
                        complete(string.Empty, false);
                    else
                        complete(serialNo, true);
            }
            else
                complete(string.Empty, false);
        }
        private IGreenCardReaderInfo _info;
        public IGreenCardReaderInfo Info
        {
            get
            {
                return _info;
            }
            set
            {
                if (this._info == null)
                {
                    this._info = new GreenCardReaderInfo()
                    {
                        Type = "ModWinsCard",
                        DeviceName = string.Empty,
                        SerialNumber = string.Empty
                    };
                    this._state = CardState.IsDisable;
                }
            }
        }
        private CardState _state;
        public CardState State
        {
            get
            {
                return this._state;
            }
            set
            {
                if (this._state != CardState.IsReady)
                    this._state = CardState.IsDisable;
            }
        }

        public event GreenCardReaderEventHandler ReadingCompleted;
        public event GreenCardReaderEventHandler TakingOffCompleted;
        public bool Connect()
        {
            try
            {
                if (_canRead)
                {
                    this._state = CardState.IsReady;
                    _task = Task.Factory.StartNew(() => ReadingThread(), TaskCreationOptions.LongRunning);
                    return true;
                }
                else
                {
                    GetReady();
                    Thread.Sleep(1000);
                    if (_canRead)
                    {
                        this._state = CardState.IsReady;
                        _task = Task.Factory.StartNew(() => ReadingThread(), TaskCreationOptions.LongRunning);
                    }
                    return true;
                }
            }
            catch
            {
                this._state = CardState.IsDisable;
                int retCode = ModWinsCard.SCardCancel(_context);
                if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                    Console.WriteLine(string.Format("{0} cancel failed", this._info.SerialNumber));
                retCode = ModWinsCard.SCardReleaseContext(_context);
                if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                    Console.WriteLine(string.Format("{0} release failed", this._info.SerialNumber));
                return false;
            }
        }
        private void ReadingThread()
        {

            try
            {
                SCARD_READERSTATE readerState;
                readerState.RdrCurrState = ModWinsCard.SCARD_STATE_UNAWARE;
                readerState.RdrEventState = ModWinsCard.SCARD_STATE_UNKNOWN;
                readerState.UserData = new IntPtr(0);
                readerState.ATRLength = 0;
                readerState.ATRValue = new byte[36];
                readerState.RdrName = this._info.DeviceName;
                while (true)
                {
                    if (_canRead && _state == CardState.IsReady)
                    {
                        DateTime tmp = DateTime.Now;
                        if (connectTime <= tmp)
                        {
                            connectTime = tmp.AddMilliseconds(2);
                            try
                            {
                                int retCode = ModWinsCard.SCardGetStatusChange(_context, ModWinsCard.INFINITE, ref readerState, 1);

                                if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                                {
                                    readerState.RdrCurrState = ModWinsCard.SCARD_STATE_UNAWARE;
                                    readerState.RdrEventState = ModWinsCard.SCARD_STATE_UNKNOWN;
                                    readerState.UserData = new IntPtr(0);
                                    readerState.ATRLength = 0;
                                    readerState.ATRValue = new byte[36];
                                    readerState.RdrName = this._info.DeviceName;

                                    ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, ref _context);
                                    Thread.Sleep(1000);

                                    //ReadingCompleted(this, new GreenCardReaderEventArgs() { CardID = string.Empty, CardReader = this, ex = new Exception("Reading failed") });
                                    continue;
                                }

                                if ((readerState.RdrEventState & ModWinsCard.SCARD_STATE_CHANGED) == ModWinsCard.SCARD_STATE_CHANGED)
                                {
                                    if ((readerState.RdrEventState & ModWinsCard.SCARD_STATE_EMPTY) == ModWinsCard.SCARD_STATE_EMPTY)
                                    {
                                        if (TakingOffCompleted != null)
                                            TakingOffCompleted(this, new GreenCardReaderEventArgs() { CardID = string.Empty, CardReader = this });
                                    }
                                    else if (((readerState.RdrEventState & ModWinsCard.SCARD_STATE_PRESENT) == ModWinsCard.SCARD_STATE_PRESENT)
                                        && ((readerState.RdrEventState & ModWinsCard.SCARD_STATE_PRESENT) != (readerState.RdrCurrState & ModWinsCard.SCARD_STATE_PRESENT)))
                                    {
                                        GetCardId();
                                        //GetCardIdLisa();
                                    }
                                }

                                readerState.RdrCurrState = readerState.RdrEventState;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("BUGGG: {0}", ex.Message);
                                this.DisConnect();
                                GetReady();
                                Thread.Sleep(1000);
                                this.Connect();
                            }

                        }
                    }
                    else
                    {
                        this.DisConnect();
                        GetReady();
                        Thread.Sleep(1000);
                        this.Connect();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("BUGGG: {0}", exception.Message);
                this.DisConnect();
                GetReady();
                System.Threading.Thread.Sleep(1000);
                this.Connect();
            }

        }
        public void DisConnect()
        {
            this._state = CardState.IsDisable;
            int retCode = ModWinsCard.SCardCancel(_context);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                Console.WriteLine(string.Format("{0} cancel failed", this._info.SerialNumber));
            retCode = ModWinsCard.SCardReleaseContext(_context);
            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                Console.WriteLine(string.Format("{0} release failed", this._info.SerialNumber));
        }
        private int ConnectCard()
        {
            return ModWinsCard.SCardConnect(_context, this._info.DeviceName, ModWinsCard.SCARD_SHARE_SHARED, ModWinsCard.SCARD_PROTOCOL_T0 | ModWinsCard.SCARD_PROTOCOL_T1, ref _cardHandle, ref _activeProtocol);
        }
        #region Felica
        public string IdDm { get; set; }
        private string GetCardIdFelica()
        {
            if (ConnectCard() != ModWinsCard.SCARD_S_SUCCESS)
            {
                return string.Empty;
            }

            Array.Clear(_sendBuff, 0, _sendBuff.Length);
            Array.Clear(_recvBuff, 0, _recvBuff.Length);
            int sendBuffLen = 0x0B;
            int RecvBuffLen = 0x2D;
            string CodeData = "FF46010206CB1880008001";
            OpcodeConv(CodeData);
            _sendRequest.dwProtocol = _activeProtocol;
            _sendRequest.cbPciLength = Marshal.SizeOf(_sendRequest);
            _recvRequest.dwProtocol = _activeProtocol;
            _recvRequest.cbPciLength = Marshal.SizeOf(_recvRequest);

            int retCode = ModWinsCard.SCardTransmit(_cardHandle, ref _sendRequest, ref _sendBuff[0], sendBuffLen, ref _recvRequest, ref _recvBuff[0], ref RecvBuffLen);

            string sCardID = string.Empty;
            for (int i = 0; i < RecvBuffLen - 2; i++)
                sCardID = sCardID + String.Format("{0:X2}", _recvBuff[i]);

            string StrDateTime = "";
            if (!string.IsNullOrWhiteSpace(sCardID))
            {
                this.IdDm = !string.IsNullOrWhiteSpace(sCardID) ? sCardID.Substring(0, 16) : "";
                string TimeRide = "";
                if (sCardID.Length > 85)
                {
                    TimeRide = !string.IsNullOrWhiteSpace(sCardID) ? ConvertHex(sCardID.Substring(54, 32)) : "";
                    //"2017011610:47:03"

                    //dd/MM/yyyy HH:mm:ss
                    StrDateTime = string.Format("{0}/{1}/{2} {3}", TimeRide.Substring(6, 2), TimeRide.Substring(4, 2), TimeRide.Substring(0, 4), TimeRide.Substring(8, 8));
                }
                //sCardID = "zxcvbnmasd";
                if (sCardID.Length < 16)
                    sCardID = string.Empty;
                else
                    sCardID = sCardID.Substring(0, 16);
                //sCardID = ConvertHex(sCardID.Substring(22, 32));
            }
            if (ReadingCompleted != null)
                ReadingCompleted(this, new GreenCardReaderEventArgs() { CardID = sCardID, TimeRide = StrDateTime, CardReader = this });

            // Disconnect card after reading completed
            retCode = ModWinsCard.SCardDisconnect(_cardHandle, ModWinsCard.SCARD_LEAVE_CARD);

            return sCardID;
        }
        private void OpcodeConv(String opcode)
        {
            Byte[] toBytes = Encoding.ASCII.GetBytes(opcode);
            for (int i = 0; i < opcode.Length; i++)
            {
                switch (toBytes[i])
                {
                    case 65:
                        toBytes[i] = (Byte)0x0A;
                        break;
                    case 97:
                        toBytes[i] = (Byte)0x0A;
                        break;
                    case 66:
                        toBytes[i] = (Byte)0x0B;
                        break;
                    case 98:
                        toBytes[i] = (Byte)0x0B;
                        break;
                    case 67:
                        toBytes[i] = (Byte)0x0C;
                        break;
                    case 99:
                        toBytes[i] = (Byte)0x0C;
                        break;
                    case 68:
                        toBytes[i] = (Byte)0x0D;
                        break;
                    case 100:
                        toBytes[i] = (Byte)0x0D;
                        break;
                    case 69:
                        toBytes[i] = (Byte)0x0E;
                        break;
                    case 101:
                        toBytes[i] = (Byte)0x0E;
                        break;
                    case 70:
                        toBytes[i] = (Byte)0x0F;
                        break;
                    case 102:
                        toBytes[i] = (Byte)0x0F;
                        break;
                    default:
                        toBytes[i] -= (Byte)0x30;
                        break;
                }
            }
            for (Byte i = 0; i < opcode.Length / 2; i++)
            {
                _sendBuff[i] = Convert.ToByte(toBytes[2 * i] * 0x10 | toBytes[2 * i + 1]);
            }
        }
        public string ConvertHex(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }
        #endregion
        private string GetCardId()
        {
            lock (this)
            {
                _recvBuff = new byte[128];
                _sendBuff = new byte[128];
                if (ConnectCard() != ModWinsCard.SCARD_S_SUCCESS)
                {
                    return string.Empty;
                }
                int RecvBuffLen = 0x6;
                Array.Clear(_sendBuff, 0, _sendBuff.Length);

                _sendBuff[0] = 0xFF;      //CLA
                _sendBuff[1] = 0xCA;      //P1 : Same for all source type
                _sendBuff[2] = 0x0;       //INS : for stored key input
                _sendBuff[3] = 0x0;       //P2  : for stored key input
                _sendBuff[4] = 0x0;          //P3  : for stored key input
                int sendBuffLen = 0x5;

                _sendRequest.dwProtocol = _activeProtocol;
                _sendRequest.cbPciLength = Marshal.SizeOf(_sendRequest);

                _recvRequest.dwProtocol = _activeProtocol;
                _recvRequest.cbPciLength = Marshal.SizeOf(_recvRequest);

                int retCode = ModWinsCard.SCardTransmit(_cardHandle, ref _sendRequest, ref _sendBuff[0], sendBuffLen, ref _recvRequest, ref _recvBuff[0], ref RecvBuffLen);

                string sCardID = string.Empty;
                for (int i = 0; i < RecvBuffLen - 2; i++)
                {
                    sCardID = sCardID + String.Format("{0:X2}", _recvBuff[i]);
                }
                if (string.IsNullOrEmpty(sCardID) || sCardID.Contains("0000000") || "00000000000000000000000000000000".Contains(sCardID))
                    //return GetCardIdFelica();
                    sCardID = string.Empty;
                if (ReadingCompleted != null)
                    ReadingCompleted(this, new GreenCardReaderEventArgs() { CardID = sCardID, CardReader = this });
                // Disconnect card after reading completed
                retCode = ModWinsCard.SCardDisconnect(_cardHandle, ModWinsCard.SCARD_LEAVE_CARD);
                return sCardID;
            }
        }
        public Object GetController()
        {
            return null;
        }
    }
   
    public static class CurrentListCardReader
    {
        public static List<IGreenCardReaderInfo> ListCardInfo { get; set; }
        public static List<IGreenCardReader> ListCard { get; set; }
        public static void AddCardInfo(IGreenCardReaderInfo info)
        {
            if (ListCardInfo == null)
                ListCardInfo = new List<IGreenCardReaderInfo>();
            var cif = ListCardInfo.FirstOrDefault(c => c.Type == info.Type &&
                       ((c.Type == "ModWinsCard" && c.SerialNumber == info.SerialNumber)
                           || (c.Type != "ModWinsCard" && c.TcpIp == info.TcpIp && c.Port == info.Port)
                       )
                   );
            if (cif == null)
                ListCardInfo.Add(info);
            if (ListCard == null)
                ListCard = new List<IGreenCardReader>();
            var crd = ListCard.FirstOrDefault(c => c.Info.Type == info.Type &&
                        ((c.Info.Type == "ModWinsCard" && c.Info.SerialNumber == info.SerialNumber)
                            || (c.Info.Type != "ModWinsCard" && c.Info.TcpIp == info.TcpIp && c.Info.Port == info.Port)
                        )
                    );
            if (crd == null)
            {
                switch (info.Type)
                {
                    //'ModWinsCard', 'Tcp Ip Client', 'Tcp Ip Server', 'Remode Card'
                    case "ModWinsCard":
                        ListCard.Add(new ModWinsCardReader(info));
                        break;
                   
                }
            }
        }
        public static void RemoveCards()
        {
            if (ListCard != null)
            {
                foreach (var c in ListCard)
                {
                    c.DisConnect();
                }
                ListCard.Clear();
            }
            if (ListCardInfo != null)
                ListCardInfo.Clear();
        }
        public static void RemoveCardInfo(IGreenCardReaderInfo info)
        {
            if (ListCardInfo != null)
            {
                var cif = ListCardInfo.FirstOrDefault(c => c.Type == info.Type &&
                        ((c.Type == "ModWinsCard" && c.SerialNumber == info.SerialNumber)
                            || (c.Type != "ModWinsCard" && c.TcpIp == info.TcpIp && c.Port == info.Port)
                        )
                    );
                if (cif != null)
                {
                    ListCardInfo.Remove(cif);
                }
                cif = ListCardInfo.FirstOrDefault(c => c.Type == info.Type &&
                        ((c.Type == "ModWinsCard" && c.SerialNumber == info.SerialNumber)
                            || (c.Type != "ModWinsCard" && c.TcpIp == info.TcpIp && c.Port == info.Port)
                        )
                    );
                if (cif == null && ListCard != null)
                {
                    var crd = ListCard.FirstOrDefault(c => c.Info.Type == info.Type &&
                        ((c.Info.Type == "ModWinsCard" && c.Info.SerialNumber == info.SerialNumber)
                            || (c.Info.Type != "ModWinsCard" && c.Info.TcpIp == info.TcpIp && c.Info.Port == info.Port)
                        )
                    );
                    if (crd != null)
                    {
                        crd.DisConnect();
                        ListCard.Remove(crd);
                    }
                }
            }
        }
        public static string RefreshListCard()
        {
            if (ListCard == null)
                return "Không tìm thấy đầu đọc thẻ";
            string res = string.Empty;
            foreach (var crd in ListCard)
            {
                if (crd.State != CardState.IsReady)
                {
                    crd.Connect();
                }
                else
                {
                    if (crd.Info.Type == "Tcp Ip Server")
                        crd.Connect();
                }
                string rowString = string.Empty;
                if (crd.State == CardState.IsReady)
                {
                    res += string.Format("{0}:{1} -- đã sẵn sàng {2}", crd.Info.Type, crd.Info.Type == "ModWinsCard" ? crd.Info.SerialNumber : crd.Info.TcpIp + ":" + crd.Info.Port, Environment.NewLine);
                }
                else
                {
                    res += string.Format("{0}:{1} -- không thể kết nối {2}", crd.Info.Type, crd.Info.Type == "ModWinsCard" ? crd.Info.SerialNumber : crd.Info.TcpIp + ":" + crd.Info.Port, Environment.NewLine);
                }

            }
            return res;
        }
        public static bool StartGreenCardReader(List<IGreenCardReaderInfo> lstInfo, GreenCardReaderEventHandler read, GreenCardReaderEventHandler takeoff)
        {
            if (lstInfo == null)
                return false;
            bool b = false;
            foreach (var info in lstInfo)
            {
                if (ListCard == null || ListCard.Count == 0)
                {
                    AddCardInfo(info);
                    RefreshListCard();
                }
                var crd = ListCard.FirstOrDefault(c => c.Info.Type == info.Type &&
                       ((c.Info.Type == "ModWinsCard" && c.Info.SerialNumber == info.SerialNumber)
                           || (c.Info.Type != "ModWinsCard" && c.Info.TcpIp == info.TcpIp && c.Info.Port == info.Port)
                       )
                   );
                if (crd == null)
                {
                    AddCardInfo(info);
                    RefreshListCard();
                    crd = ListCard.FirstOrDefault(c => c.Info.Type == info.Type &&
                       ((c.Info.Type == "ModWinsCard" && c.Info.SerialNumber == info.SerialNumber)
                           || (c.Info.Type != "ModWinsCard" && c.Info.TcpIp == info.TcpIp && c.Info.Port == info.Port)
                       )
                   );
                }
                if (crd != null)
                {
                    crd.ReadingCompleted += read;
                    crd.TakingOffCompleted += takeoff;
                    b = true;
                }
            }
            return b;
        }
        public static bool StoptGreenCardReader(List<IGreenCardReaderInfo> lstInfo, GreenCardReaderEventHandler read, GreenCardReaderEventHandler takeoff)
        {
            if (lstInfo == null || ListCard == null || ListCard.Count == 0)
                return false;
            bool b = false;
            foreach (var info in lstInfo)
            {
                var crd = ListCard.FirstOrDefault(c => c.Info.Type == info.Type &&
                       ((c.Info.Type == "ModWinsCard" && c.Info.SerialNumber == info.SerialNumber)
                           || (c.Info.Type != "ModWinsCard" && c.Info.TcpIp == info.TcpIp && c.Info.Port == info.Port)
                       )
                   );
                if (crd != null)
                {
                    crd.ReadingCompleted -= read;
                    crd.TakingOffCompleted -= takeoff;
                    b = true;
                }
            }
            return b;
        }
       
    }
}
