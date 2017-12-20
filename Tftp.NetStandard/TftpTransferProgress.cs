namespace Tftp.NetStandard
{
    public class TftpTransferProgress
    {
        /// <summary>
        /// Number of bytes that have already been transferred.
        /// </summary>
        public int TransferredBytes { get; private set; }
        public byte[] TransferredBytesData { get; private set; }

        /// <summary>
        /// Total number of bytes being transferred. May be 0 if unknown.
        /// </summary>
        public int TotalBytes { get; private set; }

        public TftpTransferProgress(int transferred, byte[] byteData, int total)
        {
            TransferredBytes = transferred;
            TransferredBytesData = byteData;
            TotalBytes = total;
        }

        public override string ToString()
        {
            if (TotalBytes > 0)
                return (TransferredBytes * 100) / TotalBytes + "% completed";
            else
                return TransferredBytes + " bytes transferred";
        }
    }
}
