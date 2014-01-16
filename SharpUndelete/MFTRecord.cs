using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace SharpUndelete
{
        struct NTFS_MFT_FILE
    {
	    char[]  szSignature;		// Signature "FILE"
	    char    wFixupOffset;		// offset to fixup pattern
        char    wFixupSize;			// Size of fixup-list +1
        Int64   n64LogSeqNumber;	// log file seq number
        char    wSequence;			// sequence nr in MFT
        char    wHardLinks;			// Hard-link count
        char    wAttribOffset;		// Offset to seq of Attributes
        char    wFlags;				// 0x01 = NonRes; 0x02 = Dir
	    Int32   dwRecLength;		// Real size of the record
        Int32   dwAllLength;		// Allocated size of the record
	    Int64	n64BaseMftRec;		// ptr to base MFT rec or 0
        char    wNextAttrID;		// Minimum Identificator +1
        char    wFixupPattern;		// Current fixup pattern
        Int32   dwMFTRecNumber;		// Number of this MFT Record
								    // followed by resident and
								    // part of non-res attributes
    }
    // change strict to union
    // using http://social.msdn.microsoft.com/Forums/en-US/1f91fec8-676f-4381-9358-07135e59643e/is-there-a-kind-of-union-structure-in-c-sharp
    //[StructLayout(LayoutKind.Explicit, Size=8)]
    //struct UValue
    //{
    //   [FieldOffset(0)]
    //
    // TODO CHECK SIZES
    struct NTFS_ATTRIBUTE // if resident then + RESIDENT
    {					//  else + NONRESIDENT
        int dwType;
        int dwFullLength;
        byte uchNonResFlag;
        byte uchNameLength;
        Int32 wNameOffset;
        Int32 wFlags;
        Int32 wID;

        struct Attr
        {
            struct Resident
            {
                int dwLength;
                Int32 wAttrOffset;
                byte uchIndexedTag;
                byte uchPadding;
            }
            struct NonResident
            {
                Int64 n64StartVCN;
                Int64 n64EndVCN;
                Int32 wDatarunOffset;
                Int32 wCompressionSize; // compression unit size
                byte[] uchPadding;
                Int64 n64AllocSize;
                Int64 n64RealSize;
                Int64 n64StreamSize;
                // data runs...
            }
        }
    } 

    struct ATTR_STANDARD
    {
	    Int64	n64Create;		// Creation time
	    Int64	n64Modify;		// Last Modify time
	    Int64	n64Modfil;		// Last modify of record
	    Int64	n64Access;		// Last Access time
	    Int32		dwFATAttributes;// As FAT + 0x800 = compressed
	    Int32		dwReserved1;	// unknown

    }   
  
     struct ATTR_FILENAME
    {
	    public Int64	dwMftParentDir;            // Seq-nr parent-dir MFT entry
        public Int64 n64Create;                  // Creation time
        public Int64 n64Modify;                  // Last Modify time
        public Int64 n64Modfil;                  // Last modify of record
        public Int64 n64Access;                  // Last Access time
        public Int64 n64Allocated;               // Allocated disk space
        public Int64 n64RealSize;                // Size of the file
        public Int32 dwFlags;					// attribute
        public Int32 dwEAsReparsTag;				// Used by EAs and Reparse
        public byte chFileNameLength;
        public byte chFileNameType;            // 8.3 / Unicode
        public char[] wFilename;          //512   // Name (in Unicode ?)

    }

     struct LARGE_INTEGER
     {
         public Int32 QuadPart;
     }

    class MFTRecord
    {
        // SetLastError
        const UInt64 GENERIC_READ = 0x80000000L;
        const UInt64 GENERIC_WRITE = 0x40000000L;
        const UInt64 GENERIC_EXECUTE = 0x20000000L;
        const UInt64 GENERIC_ALL = 0x10000000L;
        const uint FILE_SHARE_READ = 0x00000001;
        const uint FILE_SHARE_WRITE = 0x00000002;
        const uint OPEN_EXISTING = 0x00000003;
        const uint FILE_FLAG_DELETE_ON_CLOSE = 0x04000000;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern unsafe Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(
              string FileName,
            /* ulong*/ ulong DesiredAccess,
              uint ShareMode,
              IntPtr SecurityAttributes,
              uint CreationDisposition,
              uint FlagsAndAttributes,
              IntPtr hTemplateFile
              );

        // SetFilePointer
        public enum EMoveMethod : uint
        {
            Begin = 0,
            Current = 1,
            End = 2
        }

        [DllImport("kernel32.dll", EntryPoint = "SetFilePointer")]
        static extern uint SetFilePointer(
              [In] Microsoft.Win32.SafeHandles.SafeFileHandle hFile,
              [In] int lDistanceToMove,
              [In, Out] ref int lpDistanceToMoveHigh,
              [In] EMoveMethod dwMoveMethod);
        
        //

        #region Constructors/Destructors
        MFTRecord()
        {
            MFTRecords = new byte[512];
            attrStandard = new ATTR_STANDARD();
            attrFilename = new ATTR_FILENAME();

        }
        // virtual ~MFTRecord(); 
        #endregion

        #region protected variables
        protected int handleDrive = 0;
        protected byte[] MFTRecords;
        protected long maxMFTRecordSize = 0;
        protected long currentPos = 0;
        protected long BytesPerCluster = 0;
        protected Int64 n64StartPost = 0;
        #endregion
        
        protected int ReadRaw(Int64 LCN, byte[] Data, int lenth)
        {
            // int nRet;
           // LARGE_INTEGER n64Pos;
           // n64Pos.QuadPart = (int)BytesPerCluster;
           // n64Pos.QuadPart += (int)n64StartPost;

            Microsoft.Win32.SafeHandles.SafeFileHandle hDrv = CreateFile("\\\\.\\PhysicalDrive0",
                GENERIC_READ,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                (IntPtr)null,
                OPEN_EXISTING,
                0,
                (IntPtr)null);

            return 1;
           
        }
        
      // protected int ExtractData(NTFS_ATTRIBUTE ntfsAttr, byte[] Data, int dataLen);

        public ATTR_STANDARD attrStandard;
        public ATTR_FILENAME attrFilename;
        public bool inUse = false;

      //  public int SetRecordInfo(Int64 n64StartPos, Int32 dwRecSize, Int32 dwBytesPerCluster);
	//    public void SetDriveHandle(int /*HANDLE*/ hDrive);

	 //   public int ExtractFile(/*BYTE**/char[] puchMFTBuffer, Int32 dwLen, bool bExcludeData /*=false*/);


    }
}
