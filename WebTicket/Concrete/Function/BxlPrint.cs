using System.Runtime.InteropServices;

namespace WebTicket.Concrete.Function
{
    public class BxlPrint
    {
        // This file supports all Platforms, but this sample code only supports PDA Platforms
        // so if you want to use this sample code in PC platform, you should create a new project for 
        // PC Platform and add file in your project.

        //public const string BXLDIR = "bxl.dll";
        public const string BXLDIR = "BXL.dll";
        private BxlPrint() { }

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "LoadLibrary")]
        public static extern IntPtr LoadLibrary(string LibName);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "FreeLibrary")]
        public static extern Int32 FreeLibrary(IntPtr hLib);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "PrinterOpen")]
        public static extern Int32 PrinterOpen(string strPort, Int32 lTimeout);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "PrinterClose")]
        public static extern Int32 PrinterClose();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "LineFeed")]
        public static extern Int32 LineFeed(Int32 nFeed);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "PrintBitmap")]
        public static extern Int32 PrintBitmap(string FileName, Int32 Width, Int32 Alignment, Int32 Level);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "PrintBarcode")]
        public static extern Int32 PrintBarcode(byte[] Data, Int32 Symbology, Int32 Height, Int32 Width, Int32 Alignment, Int32 TextPosition);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "PrintText")]
        public static extern Int32 PrintText(string Data, Int32 Alignment, Int32 Attribute, Int32 TextSize);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "PrintTextW")]
        public static extern Int32 PrintTextW(string Data, Int32 Alignment, Int32 Attribute, Int32 TextSize);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetCharacterSet")]
        public static extern Int32 SetCharacterSet(Int32 Value);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetInterChrSet")]
        public static extern Int32 SetInterChrSet(Int32 Value);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "GetCharacterSet")]
        public static extern Int32 GetCharacterSet();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "GetInterChrSet")]
        public static extern Int32 GetInterChrSet();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "DirectIO")]
        public static extern Int32 DirectIO(byte[] Data, UInt32 iWrite, byte[] pRequest, ref UInt32 iRead);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "MsrReadReady")]
        public static extern Int32 MsrReadReady();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "MsrReadCancel")]
        public static extern Int32 MsrReadCancel();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "MsrReadTrack")]
        public static extern Int32 MsrReadTrack(byte[] Data1, byte[] Data2, byte[] Data3);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "CheckPrinter")]
        public static extern Int32 CheckPrinter();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "GetPowerValue")]
        public static extern Int32 GetPowerValue();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "GetStat")]
        public static extern Int32 GetStat();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "GetResultCode")]
        public static extern Int32 GetResultCode();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "NextPrintPos")]
        public static extern Int32 NextPrintPos();

        //file version 1.0.0.13 main version 1.0.6 add function (for MSR)
        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "GetTrack1")]
        public static extern string GetTrack1();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "GetTrack2")]
        public static extern string GetTrack2();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "GetTrack3")]
        public static extern string GetTrack3();

        //20090407 Added
        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "MsrReadFullTrack")]
        public static extern Int32 MsrReadFullTrack(byte[] FullTrack, UInt32 iLen);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "MsrRead1Track")]
        public static extern Int32 MsrRead1Track(byte[] FullTrack, UInt32 iLen);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "MsrRead2Track")]
        public static extern Int32 MsrRead2Track(byte[] FullTrack, UInt32 iLen);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "MsrRead3Track")]
        public static extern Int32 MsrRead3Track(byte[] FullTrack, UInt32 iLen);


        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetMsrMsgMode")]
        public static extern Int32 SetMsrMsgMode(Boolean bValue);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SelectMode")]
        public static extern Int32 SelectMode(Boolean bValue);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "AutoCalibration")]
        public static extern Int32 AutoCalibration();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SelectPageMode")]
        public static extern Int32 SelectPageMode(Boolean bValue);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetPrintAreaInPageMode")]
        public static extern Int32 SetPrintAreaInPageMode(Int32 x, Int32 y, Int32 width, Int32 height);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SelectPrintDirectionInPageMode")]
        public static extern Int32 SelectPrintDirectionInPageMode(Int32 printDirection);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetAbsoluteVerticalPrintPositionInPageMode")]
        public static extern Int32 SetAbsoluteVerticalPrintPositionInPageMode(Int32 motionUnit);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetAbsolutePrintPosition")]
        public static extern Int32 SetAbsolutePrintPosition(Int32 motionUnit);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "PrintDataInPageMode")]
        public static extern Int32 PrintDataInPageMode();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "InitializePrinter")]
        public static extern Int32 InitializePrinter();


        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetPrintAreaInPM")]
        public static extern Int32 SetPrintAreaInPM(Int32 x, Int32 y, Int32 width, Int32 height);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetPrintDirectionInPM")]
        public static extern Int32 SetPrintDirectionInPM(Int32 printDirection);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetVerticalPositionInPM")]
        public static extern Int32 SetVerticalPositionInPM(Int32 motionUnit);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetHorizontalPositionInPM")]
        public static extern Int32 SetHorizontalPositionInPM(Int32 motionUnit);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "TransactionStart")]
        public static extern void TransactionStart();

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "TransactionEnd")]
        public static extern Int32 TransactionEnd(Boolean bCompleteChk, Int32 lTimeout);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "DrawLineInPM")]
        public static extern Int32 DrawLineInPM(Int32 Xs, Int32 Ys, Int32 Xe, Int32 Ye, Int32 thick);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "DrawBoxInPM")]
        public static extern Int32 DrawBoxInPM(Int32 Xs, Int32 Ys, Int32 Xe, Int32 Ye, Int32 thick);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "PrintBitmapLZMA")]
        public static extern Int32 PrintBitmapLZMA(string FileName, Int32 Width, Int32 Alignment, Int32 Level);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetUpsideDown")]
        public static extern Int32 SetUpsideDown(Int32 mode);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetLeftMargin")]
        public static extern Int32 SetLeftMargin(Int32 margin);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "SetPrintWidth")]
        public static extern Int32 SetPrintWidth(Int32 width);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "DebugMode")]
        public static extern Int32 DebugMode(Boolean dbgEnable, Boolean savePrn);

        public delegate int BxlCallBackDelegate(int status);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "BidiSetCallBack")]
        public static extern Int32 BidiSetCallBack(BxlCallBackDelegate status);

        [DllImport(BXLDIR, SetLastError = true, EntryPoint = "BidiCancelCallBack")]
        public static extern Int32 BidiCancelCallBack();

        //Method return value
        public const Int32 BXL_SUCCESS = 0;
        public const Int32 BXL_READBUFFER_EMPTY = -1;

        public const Int32 BXL_OTHERPTR_OPENED = -100;
        public const Int32 BXL_NOT_OPENED = -101;
        public const Int32 BXL_CREATE_ERROR = -102;
        public const Int32 BXL_STATUS_ERROR = -103;
        public const Int32 BXL_WATING_OPEN = -104;
        public const Int32 BXL_CONNECT_ERROR = -105;
        public const Int32 BXL_BDADDR_ERROR = -106;
        public const Int32 BXL_NOT_SUPPORT = -107;
        public const Int32 BXL_BAD_ARGUMENT = -108;
        public const Int32 BXL_BUFFER_ERROR = -109;

        public const Int32 BXL_REGISTRY_ERROR = -200;
        public const Int32 BXL_WRITE_ERROR = -300;
        public const Int32 BXL_READ_ERROR = -301;

        public const Int32 BXL_BITMAPLOAD_ERROR = -400;
        public const Int32 BXL_BITMAPDATA_ERROR = -401;

        public const Int32 BXL_BC_DATA_ERROR = -500;
        public const Int32 BXL_BC_NOT_SUPPORT = -501;

        //MSR State Return
        public const Int32 BXLMSR_FAILEDMODE = -601;
        public const Int32 BXLMSR_NOTREADY = -602;
        public const Int32 BXLMSR_DATAEMPTY = -603;

        //Printer Status flag
        public const Int32 BXL_STS_NORMAL = 0;
        public const Int32 BXL_STS_PAPEREMPTY = 1;
        public const Int32 BXL_STS_COVEROPEN = 2;
        public const Int32 BXL_STS_POWEROVER = 4;
        public const Int32 BXL_STS_MSR_READY = 8;
        public const Int32 BXL_STS_PRINTING = 16;
        public const Int32 BXL_STS_ERROR = 32;
        public const Int32 BXL_STS_NOT_OPEN = 64;
        public const Int32 BXL_STS_ERROR_OCCUR = 128;

        //Power Status
        public const Int32 BXL_PWR_FULL = 0;
        public const Int32 BXL_PWR_HIGH = 1;
        public const Int32 BXL_PWR_MIDDLE = 2;
        public const Int32 BXL_PWR_LOW = 3;

        //Alignment Code
        public const Int32 BXL_ALIGNMENT_LEFT = 0;
        public const Int32 BXL_ALIGNMENT_CENTER = 1;
        public const Int32 BXL_ALIGNMENT_RIGHT = 2;

        //Text Attribute
        public const Int32 BXL_FT_DEFAULT = 0;
        public const Int32 BXL_FT_FONTB = 1;
        public const Int32 BXL_FT_FONTC = 16;
        public const Int32 BXL_FT_BOLD = 2;
        public const Int32 BXL_FT_UNDERLINE = 4;
        public const Int32 BXL_FT_REVERSE = 8;
        public const Int32 BXL_ExFT_CHINA_FONTA = 0;
        public const Int32 BXL_ExFT_CHINA_FONTB = 32;
        public const Int32 BXL_FT_UNDERTHICK = 6;

        //Text Size Attribute
        public const Int32 BXL_TS_0WIDTH = 0;
        public const Int32 BXL_TS_1WIDTH = 16;
        public const Int32 BXL_TS_2WIDTH = 32;
        public const Int32 BXL_TS_3WIDTH = 48;
        public const Int32 BXL_TS_4WIDTH = 64;
        public const Int32 BXL_TS_5WIDTH = 80;
        public const Int32 BXL_TS_6WIDTH = 96;
        public const Int32 BXL_TS_7WIDTH = 112;

        public const Int32 BXL_TS_0HEIGHT = 0;
        public const Int32 BXL_TS_1HEIGHT = 1;
        public const Int32 BXL_TS_2HEIGHT = 2;
        public const Int32 BXL_TS_3HEIGHT = 3;
        public const Int32 BXL_TS_4HEIGHT = 4;
        public const Int32 BXL_TS_5HEIGHT = 5;
        public const Int32 BXL_TS_6HEIGHT = 6;
        public const Int32 BXL_TS_7HEIGHT = 7;

        // 'Width Full
        public const Int32 BXL_WIDTH_FULL = -1;
        public const Int32 BXL_WIDTH_NONE = -2;

        // 'Barcode
        public const Int32 BXL_BCS_PDF417 = 200;
        public const Int32 BXL_BCS_QRCODE_MODEL2 = 202;
        public const Int32 BXL_BCS_QRCODE_MODEL1 = 203;
        public const Int32 BXL_BCS_DATAMATRIX = 204;
        public const Int32 BXL_BCS_MAXICODE_MODE2 = 205;
        public const Int32 BXL_BCS_MAXICODE_MODE3 = 206;
        public const Int32 BXL_BCS_MAXICODE_MODE4 = 207;

        public const Int32 BXL_BCS_UPCA = 101;
        public const Int32 BXL_BCS_UPCE = 102;
        public const Int32 BXL_BCS_EAN8 = 103;
        public const Int32 BXL_BCS_EAN13 = 104;
        public const Int32 BXL_BCS_JAN8 = 105;
        public const Int32 BXL_BCS_JAN13 = 106;
        public const Int32 BXL_BCS_ITF = 107;
        public const Int32 BXL_BCS_Codabar = 108;
        public const Int32 BXL_BCS_Code39 = 109;
        public const Int32 BXL_BCS_Code93 = 110;
        public const Int32 BXL_BCS_Code128 = 111;

        //'Barcode text position
        public const Int32 BXL_BC_TEXT_NONE = 0;
        public const Int32 BXL_BC_TEXT_ABOVE = 1;
        public const Int32 BXL_BC_TEXT_BELOW = 2;

        // 'CharaterSet
        // Updated ver 1.0.3
        public const Int32 BXL_CS_PC437 = 0;
        public const Int32 BXL_CS_PC850 = 2;
        public const Int32 BXL_CS_PC860 = 3;
        public const Int32 BXL_CS_PC863 = 4;
        public const Int32 BXL_CS_PC865 = 5;
        public const Int32 BXL_CS_WPC1252 = 16;
        public const Int32 BXL_CS_PC866 = 17;
        public const Int32 BXL_CS_PC852 = 18;
        public const Int32 BXL_CS_PC858 = 19;
        public const Int32 BXL_CS_PC864 = 22;
        public const Int32 BXL_CS_THAI42 = 23;
        public const Int32 BXL_CS_WPC1253 = 24;
        public const Int32 BXL_CS_WPC1254 = 25;
        public const Int32 BXL_CS_WPC1257 = 26;
        public const Int32 BXL_CS_FARSI = 27;
        public const Int32 BXL_CS_WPC1251 = 28;
        public const Int32 BXL_CS_PC737 = 29;
        public const Int32 BXL_CS_PC775 = 30;
        public const Int32 BXL_CS_THAI14 = 31;
        public const Int32 BXL_CS_PC862 = 33;
        public const Int32 BXL_CS_THAI11 = 34;
        public const Int32 BXL_CS_PC855 = 36;
        public const Int32 BXL_CS_PC857 = 37;
        public const Int32 BXL_CS_PC928 = 38;
        public const Int32 BXL_CS_THAI16 = 39;
        public const Int32 BXL_CS_WPC1256 = 40;
        public const Int32 BXL_CS_USER = 255;

        // 'International CharacterSet
        public const Int32 BXL_ICS_USA = 0;
        public const Int32 BXL_ICS_FRANCE = 1;
        public const Int32 BXL_ICS_GERMANY = 2;
        public const Int32 BXL_ICS_UK = 3;
        public const Int32 BXL_ICS_DENMARK1 = 4;
        public const Int32 BXL_ICS_SWEDEN = 5;
        public const Int32 BXL_ICS_ITALY = 6;
        public const Int32 BXL_ICS_SPAIN = 7;
        public const Int32 BXL_ICS_NORWAY = 9;
        public const Int32 BXL_ICS_DENMARK2 = 10;
        public const Int32 BXL_ICS_SPAIN2 = 11;
        public const Int32 BXL_ICS_LATIN = 12;
        public const Int32 BXL_ICS_KOREA = 13;

        // 'MSR Mode 
        public const Int32 BXL_MSRMODE_CMDTRACK12 = 0;
        public const Int32 BXL_MSRMODE_CMDTRACK23 = 1;
        public const Int32 BXL_MSRMODE_AUTOTRACK1 = 2;
        public const Int32 BXL_MSRMODE_AUTOTRACK2_1 = 3;
        public const Int32 BXL_MSRMODE_AUTOTRACK12 = 4;
        public const Int32 BXL_MSRMODE_AUTOTRACK2_2 = 5;
        public const Int32 BXL_MSRMODE_AUTOTRACK3 = 6;
        public const Int32 BXL_MSRMODE_AUTOTRACK23 = 7;

        //file ver 1.0.0.13 add Const, Message (for MSR)
        public const Int32 WM_MSR_ARRIVED = (0x400 + 3);
        public const Int32 BXL_LPARAM_MSR = 10;

        //wParam argument 
        public const Int32 BXL_MSG_TRACK1 = 0x01;
        public const Int32 BXL_MSG_TRACK2 = 0x02;
        public const Int32 BXL_MSG_TRACK3 = 0x04;
        public const Int32 BXL_MSG_TRACK_ENCRIPTION = 0x08;

        public const Int32 BXL_PD_LEFT_TO_RIGHT = 48;
        public const Int32 BXL_PD_BOTTOM_TO_TOP = 49;
        public const Int32 BXL_PD_RIGHT_TO_LEFT = 50;
        public const Int32 BXL_PD_TOP_TO_BOTTOM = 51;

        public const Int32 BXL_FT_UPSIDEDOWN = 10;
    }
}
