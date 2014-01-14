using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SharpUndelete
{
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

    class MFTRecord
    {


        #region protected variables
        protected int handleDrive;
        protected byte[] MFTRecords;
        protected long maxMFTRecordSoze;
        protected long currentPos;
        protected long ByesPercluster;
        protected Int64 StartPost;
        #endregion

        protected int ReadRaw(Int64 LCN, byte[] Data, int lenth);
        protected int ExtractData(NTFS_ATTRIBUTE ntfsAttr, byte[] Data, int dataLen);

       // public 
    }
}
