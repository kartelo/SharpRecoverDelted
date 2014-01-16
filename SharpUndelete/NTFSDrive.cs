using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpUndelete
{
    struct NTFS_PART_BOOT_SEC
    {
        NTFS_PART_BOOT_SEC(int unused)
        {
            JumpInstrction = new char[3];
            OemID = new char[4];
            Dummy = new char[4];
            bootstrapcode = new char[426];
            SecMark = new char();
        }

        char[] JumpInstrction; // 3
        char[] OemID; // 4
        char[] Dummy; //4
        struct NTFS_BPB
        {
            char		wBytesPerSec;
		    byte		uchSecPerClust;
		    char		wReservedSec;
		    byte		uchReserved; //3
		    char		wUnused1;
		    byte		uchMediaDescriptor;
		    char		wUnused2;
		    char		wSecPerTrack;
		    char		wNumberOfHeads;
		    Int32		dwHiddenSec;
		    Int32		dwUnused3;
		    Int32		dwUnused4;
		    Int64	n64TotalSec;
		    Int64	n64MFTLogicalClustNum;
		    Int64	n64MFTMirrLogicalClustNum;
		    int			nClustPerMFTRecord;
		    int			nClustPerIndexRecord;
		    Int64	n64VolumeSerialNum;
		    Int32		dwChecksum;
        }
        char[] bootstrapcode; //426
        char SecMark;
    }
    

    class NTFSDrive
    {
        protected int hDrive; //HANDLE
        protected Int32 StartSecotr;
        protected bool bInitialized;

        protected Int32 BytesPerCluster;
        protected Int32 ByesPerSector;

       // protected int LoadMFT(Int64 StartCluster);

        protected byte[] puchMFT; //pointer
        protected Int32 MFTLen;
        protected byte puchMFTRecored;
        protected Int32 MFTRecordSize;

        public struct ST_FILEINFO// this struct is to retrieve the file detail from this class
	    {
            char[] szFilename; //[_MAX_PATH]; // file name
		    Int64 n64Create;		// Creation time
            Int64 n64Modify;		// Last Modify time
            Int64 n64Modfil;		// Last modify of record
            Int64 n64Access;		// Last Access time
		    Int32 dwAttributes;	// file attribute
            Int64 n64Size;		// no of cluseters used
		    bool  bDeleted;		// if true then its deleted file
        }


       
	//int GetFileDetail(Int32 nFileSeq, ST_FILEINFO stFileInfo);
	//int Read_File(Int32 nFileSeq, byte[] puchFileData, Int32 dwFileDataLen);
	
	//void SetDriveHandle(int hDrive);
	//void  SetStartSector(Int32 dwStartSector, Int32 dwBytesPerSector);

	//int Initialize();

	// NTFSDrive();

// 	virtual ~CNTFSDrive();




    }
}
