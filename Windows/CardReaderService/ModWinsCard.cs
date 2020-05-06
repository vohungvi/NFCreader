using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CardReaderService
{
    [StructLayout(LayoutKind.Sequential)]
    public struct APDURec
    {
        Byte CLA;
        Byte INS;
        Byte P1;
        Byte P2;
        Byte Lc;
        Byte Le;
        Byte[] Data;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SCDIRECTORY
    {
        Byte[] DirID;
        Byte NoF;
        Byte DirFlag;
        Byte KeyType;
        int IKFLength;
        Byte AC1;
        Byte AC2;
        Byte[] AID;
        Byte[] Key;
    };

    [StructLayout(LayoutKind.Auto)]
    public struct SCFILE
    {
        Byte[] FileID;
        int FileLength;
        Byte[] Prop;
        Byte ShortID;
        Byte AC1;
        Byte AC2;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SCKEYFILE
    {
        Byte KeyId;
        Byte AC1;
        Byte AC2;
        Byte State;
        Byte KeyUsage;
        Byte ErrorRetry;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct SCARD_IO_REQUEST
    {
        public int dwProtocol;
        public int cbPciLength;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SCARD_READERSTATE
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public String RdrName;
        public IntPtr UserData;
        public int RdrCurrState;
        public int RdrEventState;
        public int ATRLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public byte[] ATRValue;
    };

    public class ModWinsCard
    {
        public const int SCARD_S_SUCCESS = 0;
        public const int SCARD_ATR_LENGTH = 33;

        //===========================================================
        //  Memory Card type constants
        //===========================================================
        public const int CT_MCU = 0x0;                     // MCU
        public const int CT_IIC_Auto = 0x1;               // IIC (Auto Detect Memory Size)
        public const int CT_IIC_1K = 0x2;               // IIC (1K)
        public const int CT_IIC_2K = 0x3;                  // IIC (2K)
        public const int CT_IIC_4K = 0x4;                  // IIC (4K)
        public const int CT_IIC_8K = 0x5;                  // IIC (8K)
        public const int CT_IIC_16K = 0x6;                 // IIC (16K)
        public const int CT_IIC_32K = 0x7;                // IIC (32K)
        public const int CT_IIC_64K = 0x8;                 // IIC (64K)
        public const int CT_IIC_128K = 0x9;                // IIC (128K)
        public const int CT_IIC_256K = 0xA;                // IIC (256K)
        public const int CT_IIC_512K = 0xB;                // IIC (512K)
        public const int CT_IIC_1024K = 0xC;               // IIC (1024K)
        public const int CT_AT88SC153 = 0xD;               // AT88SC153
        public const int CT_AT88SC1608 = 0xE;              // AT88SC1608
        public const int CT_SLE4418 = 0xF;                 // SLE4418
        public const int CT_SLE4428 = 0x10;                // SLE4428
        public const int CT_SLE4432 = 0x11;                // SLE4432
        public const int CT_SLE4442 = 0x12;                // SLE4442
        public const int CT_SLE4406 = 0x13;                // SLE4406
        public const int CT_SLE4436 = 0x14;                // SLE4436
        public const int CT_SLE5536 = 0x15;                // SLE5536
        public const int CT_MCUT0 = 0x16;                  // MCU T=0
        public const int CT_MCUT1 = 0x17;                  // MCU T=1
        public const int CT_MCU_Auto = 0x18;               // MCU Autodetect

        //===============================================================
        // CONTEXT SCOPE
        //===============================================================
        public const int SCARD_SCOPE_USER = 0;
        //===============================================================
        //  The context is a user context, and any database operations 
        //  are performed within the domain of the user.
        //===============================================================
        public const int SCARD_SCOPE_TERMINAL = 1;
        //===============================================================
        // The context is that of the current terminal, and any database 
        // operations are performed within the domain of that terminal.  
        // (The calling application must have appropriate access 
        // permissions for any database actions.)
        //===============================================================
        public const int SCARD_SCOPE_SYSTEM = 2;
        //===============================================================
        // The context is the system context, and any database operations 
        // are performed within the domain of the system.  (The calling
        // application must have appropriate access permissions for any 
        // database actions.)
        //===============================================================
        //=============================================================== 
        // Context Scope
        //=============================================================== 
        public const int SCARD_STATE_UNAWARE = 0x0;
        //===============================================================
        // The application is unaware of the current state, and would like 
        // to know. The use of this value results in an immediate return
        // from state transition monitoring services. This is represented
        // by all bits set to zero.
        //===============================================================
        public const int SCARD_STATE_IGNORE = 0x1;
        //===============================================================
        // The application requested that this reader be ignored. No other
        // bits will be set.
        //===============================================================
        public const int SCARD_STATE_CHANGED = 0x2;
        //===============================================================
        // This implies that there is a difference between the state 
        // believed by the application, and the state known by the Service
        // Manager.When this bit is set, the application may assume a
        // significant state change has occurred on this reader.
        //===============================================================
        public const int SCARD_STATE_UNKNOWN = 0x4;
        //===============================================================
        // This implies that the given reader name is not recognized by
        // the Service Manager. If this bit is set, then SCARD_STATE_CHANGED
        // and SCARD_STATE_IGNORE will also be set.
        //===============================================================
        public const int SCARD_STATE_UNAVAILABLE = 0x8;
        //===============================================================
        // This implies that the actual state of this reader is not
        // available. If this bit is set, then all the following bits are
        // clear.
        //===============================================================
        public const int SCARD_STATE_EMPTY = 0x10;
        //===============================================================
        // This implies that there is not card in the reader. If this bit
        // is set, all the following bits will be clear.
        //===============================================================
        public const int SCARD_STATE_PRESENT = 0x20;
        //===============================================================
        // This implies that there is a card in the reader.
        //===============================================================
        public const int SCARD_STATE_ATRMATCH = 0x40;
        //===============================================================
        // This implies that there is a card in the reader with an ATR
        // matching one of the target cards. If this bit is set,
        // SCARD_STATE_PRESENT will also be set.  This bit is only returned
        // on the SCardLocateCard() service.
        //===============================================================
        public const int SCARD_STATE_EXCLUSIVE = 0x80;
        //===============================================================
        // This implies that the card in the reader is allocated for 
        // exclusive use by another application. If this bit is set,
        // SCARD_STATE_PRESENT will also be set.
        //===============================================================
        public const int SCARD_STATE_INUSE = 0x100;
        //===============================================================
        // This implies that the card in the reader is in use by one or 
        // more other applications, but may be connected to in shared mode.  
        // If this bit is set, SCARD_STATE_PRESENT will also be set.
        //===============================================================
        public const int SCARD_STATE_MUTE = 0x200;
        //===============================================================
        // This implies that the card in the reader is unresponsive or not
        // supported by the reader or software.
        //===============================================================
        public const int SCARD_STATE_UNPOWERED = 0x400;
        //===============================================================
        // This implies that the card in the reader has not been powered up.
        //===============================================================

        public const int SCARD_SHARE_EXCLUSIVE = 1;
        //===============================================================
        // This application is not willing to share this card with other 
        //applications.
        //===============================================================
        public const int SCARD_SHARE_SHARED = 2;
        //===============================================================
        // This application is willing to share this card with other 
        // applications.
        //===============================================================
        public const int SCARD_SHARE_DIRECT = 3;
        //===============================================================
        // This application demands direct control of the reader, so it 
        // is not available to other applications.
        //===============================================================

        //===========================================================
        //   Disposition
        //===========================================================
        public const int SCARD_LEAVE_CARD = 0;   // Don//t do anything special on close
        public const int SCARD_RESET_CARD = 1;   // Reset the card on close
        public const int SCARD_UNPOWER_CARD = 2; // Power down the card on close
        public const int SCARD_EJECT_CARD = 3;   // Eject the card on close

        //===========================================================
        // ACS IOCTL class
        //===========================================================
        public const long FILE_DEVICE_SMARTCARD = 0x310000;

        // Reader action IOCTLs
        public const long IOCTL_SMARTCARD_DIRECT = FILE_DEVICE_SMARTCARD + 2050 * 4;
        public const long IOCTL_SMARTCARD_SELECT_SLOT = FILE_DEVICE_SMARTCARD + 2051 * 4;
        public const long IOCTL_SMARTCARD_DRAW_LCDBMP = FILE_DEVICE_SMARTCARD + 2052 * 4;
        public const long IOCTL_SMARTCARD_DISPLAY_LCD = FILE_DEVICE_SMARTCARD + 2053 * 4;
        public const long IOCTL_SMARTCARD_CLR_LCD = FILE_DEVICE_SMARTCARD + 2054 * 4;
        public const long IOCTL_SMARTCARD_READ_KEYPAD = FILE_DEVICE_SMARTCARD + 2055 * 4;
        public const long IOCTL_SMARTCARD_READ_RTC = FILE_DEVICE_SMARTCARD + 2057 * 4;
        public const long IOCTL_SMARTCARD_SET_RTC = FILE_DEVICE_SMARTCARD + 2058 * 4;
        public const long IOCTL_SMARTCARD_SET_OPTION = FILE_DEVICE_SMARTCARD + 2059 * 4;
        public const long IOCTL_SMARTCARD_SET_LED = FILE_DEVICE_SMARTCARD + 2060 * 4;
        public const long IOCTL_SMARTCARD_LOAD_KEY = FILE_DEVICE_SMARTCARD + 2062 * 4;
        public const long IOCTL_SMARTCARD_READ_EEPROM = FILE_DEVICE_SMARTCARD + 2065 * 4;
        public const long IOCTL_SMARTCARD_WRITE_EEPROM = FILE_DEVICE_SMARTCARD + 2066 * 4;
        public const long IOCTL_SMARTCARD_GET_VERSION = FILE_DEVICE_SMARTCARD + 2067 * 4;
        public const long IOCTL_SMARTCARD_GET_READER_INFO = FILE_DEVICE_SMARTCARD + 2051 * 4;
        public const long IOCTL_SMARTCARD_SET_CARD_TYPE = FILE_DEVICE_SMARTCARD + 2060 * 4;

        //===========================================================
        //   Error Codes
        //===========================================================
        public const uint SCARD_F_INTERNAL_ERROR = 0x80100001;
        public const uint SCARD_E_CANCELLED = 0x80100002;
        public const uint SCARD_E_INVALID_HANDLE = 0x80100003;
        public const uint SCARD_E_INVALID_PARAMETER = 0x80100004;
        public const uint SCARD_E_INVALID_TARGET = 0x80100005;
        public const uint SCARD_E_NO_MEMORY = 0x80100006;
        public const uint SCARD_F_WAITED_TOO_Integer = 0x80100007;
        public const uint SCARD_E_INSUFFICIENT_BUFFER = 0x80100008;
        public const uint SCARD_E_UNKNOWN_READER = 0x80100009;
        public const uint SCARD_E_TIMEOUT = 0x8010000A;
        public const uint SCARD_E_SHARING_VIOLATION = 0x8010000B;
        public const uint SCARD_E_NO_SMARTCARD = 0x8010000C;
        public const uint SCARD_E_UNKNOWN_CARD = 0x8010000D;
        public const uint SCARD_E_CANT_DISPOSE = 0x8010000E;
        public const uint SCARD_E_PROTO_MISMATCH = 0x8010000F;
        public const uint SCARD_E_NOT_READY = 0x80100010;
        public const uint SCARD_E_INVALID_VALUE = 0x80100011;
        public const uint SCARD_E_SYSTEM_CANCELLED = 0x80100012;
        public const uint SCARD_F_COMM_ERROR = 0x80100013;
        public const uint SCARD_F_UNKNOWN_ERROR = 0x80100014;
        public const uint SCARD_E_INVALID_ATR = 0x80100015;
        public const uint SCARD_E_NOT_TRANSACTED = 0x80100016;
        public const uint SCARD_E_READER_UNAVAILABLE = 0x80100017;
        public const uint SCARD_P_SHUTDOWN = 0x80100018;
        public const uint SCARD_E_PCI_TOO_SMALL = 0x80100019;
        public const uint SCARD_E_READER_UNSUPPORTED = 0x8010001A;
        public const uint SCARD_E_DUPLICATE_READER = 0x8010001B;
        public const uint SCARD_E_CARD_UNSUPPORTED = 0x8010001C;
        public const uint SCARD_E_NO_SERVICE = 0x8010001D;
        public const uint SCARD_E_SERVICE_STOPPED = 0x8010001E;
        public const uint SCARD_W_UNSUPPORTED_CARD = 0x80100065;
        public const uint SCARD_W_UNRESPONSIVE_CARD = 0x80100066;
        public const uint SCARD_W_UNPOWERED_CARD = 0x80100067;
        public const uint SCARD_W_RESET_CARD = 0x80100068;
        public const uint SCARD_W_REMOVED_CARD = 0x80100069;
        //===========================================================
        //   PROTOCOL
        //===========================================================
        public const int SCARD_PROTOCOL_UNDEFINED = 0x0;           // There is no active protocol.
        public const int SCARD_PROTOCOL_T0 = 0x1;                  // T=0 is the active protocol.
        public const int SCARD_PROTOCOL_T1 = 0x2;                  // T=1 is the active protocol.
        public const int SCARD_PROTOCOL_RAW = 0x10000;             // Raw is the active protocol.
        public const uint SCARD_PROTOCOL_DEFAULT = 0x80000000;      // Use implicit PTS.
        //===========================================================
        //   READER STATE
        //===========================================================
        public const int SCARD_UNKNOWN = 0;
        //===============================================================
        // This value implies the driver is unaware of the current 
        // state of the reader.
        //===============================================================
        public const int SCARD_ABSENT = 1;
        //===============================================================
        // This value implies there is no card in the reader.
        //===============================================================
        public const int SCARD_PRESENT = 2;
        //===============================================================
        // This value implies there is a card is present in the reader, 
        //but that it has not been moved into position for use.
        //===============================================================
        public const int SCARD_SWALLOWED = 3;
        //===============================================================
        // This value implies there is a card in the reader in position 
        // for use.  The card is not powered.
        //===============================================================
        public const int SCARD_POWERED = 4;
        //===============================================================
        // This value implies there is power is being provided to the card, 
        // but the Reader Driver is unaware of the mode of the card.
        //===============================================================
        public const int SCARD_NEGOTIABLE = 5;
        //===============================================================
        // This value implies the card has been reset and is awaiting 
        // PTS negotiation.
        //===============================================================
        public const int SCARD_SPECIFIC = 6;
        //===============================================================
        // This value implies the card has been reset and specific 
        // communication protocols have been established.
        //===============================================================

        public const int INFINITE = 2147483647;
        //==========================================================================
        // Prototypes
        //==========================================================================

        public const string winscardDLLFName = "winscard.dll";
        [DllImport(winscardDLLFName)]
        public static extern int SCardEstablishContext(int dwScope, int pvReserved1, int pvReserved2, ref int phContext);

        [DllImport(winscardDLLFName)]
        public static extern int SCardReleaseContext(int phContext);

        [DllImport(winscardDLLFName, EntryPoint = "SCardConnectA", CharSet = CharSet.Ansi)]
        public static extern int SCardConnect(int hContext, string szReaderName, int dwShareMode, int dwPrefProtocol, ref int phCard, ref int ActiveProtocol);

        [DllImport(winscardDLLFName)]
        public static extern int SCardDisconnect(int hCard, int Disposition);

        [DllImport(winscardDLLFName)]
        public static extern int SCardEndTransaction(int hCard, int Disposition);

        [DllImport(winscardDLLFName)]
        public static extern int SCardBeginTransaction(int hCard);

        [DllImport(winscardDLLFName)]
        public static extern int SCardState(int hCard, ref uint State, ref uint Protocol, ref byte ATR, ref uint ATRLen);

        [DllImport(winscardDLLFName)]
        public static extern int SCardStatus(int hCard, string szReaderName, ref int pcchReaderLen, ref int State, ref int Protocol, ref byte ATR, ref int ATRLen);

        [DllImport(winscardDLLFName)]
        public static extern int SCardTransmit(int hCard, ref SCARD_IO_REQUEST pioSendRequest, ref byte SendBuff, int SendBuffLen, ref SCARD_IO_REQUEST pioRecvRequest, ref byte RecvBuff, ref int RecvBuffLen);

        [DllImport(winscardDLLFName, EntryPoint = "SCardListReadersA", CharSet = CharSet.Ansi)]
        public static extern int SCardListReaders(int hContext, byte[] Groups, byte[] Readers, ref int pcchReaders);

        [DllImport(winscardDLLFName, EntryPoint = "SCardGetStatusChangeA")]
        public static extern int SCardGetStatusChange(int hContext, int TimeOut, ref SCARD_READERSTATE ReaderState, int ReaderCount);

        [DllImport(winscardDLLFName)]
        public static extern int SCardControl(int hCard, uint dwControlCode, ref byte SendBuff, int SendBuffLen, ref byte RecvBuff, int RecvBuffLen, ref int pcbBytesReturned);

        [DllImport(winscardDLLFName)]
        public static extern int SCardGetAttrib(int hCard, int dwAttrId, byte[] pbAttr, ref int pcbAttrLen);

        [DllImport(winscardDLLFName)]
        public static extern int SCardGetCardTypeProviderName(int hContext, string szCardName, int dwProviderId, byte[] szProvider, ref int pcchProvider);

        [DllImport(winscardDLLFName)]
        public static extern int SCardCancel(int hContext);

        [DllImport(winscardDLLFName)]
        public static extern int SCardFreeMemory(int hContext, byte[] pbAttr);

        //===========================================================
        //   ATTRIBUTE VALUE
        //===========================================================
        public const int SCARD_ATTR_VENDOR_IFD_SERIAL_NO = 65795;
        public const int SCARD_ATTR_VENDOR_IFD_VERSION = 65794;
        public const int SCARD_ATTR_VENDOR_NAME = 65792;
    }
}
