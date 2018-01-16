namespace Tftp.NetStandard
{
    public class TftpTransferProgress
    {
        /// <summary>
        /// Number of bytes that have already been transferred.
        /// </summary>
        public int Transferred { get; private set; }

        /// <summary>
        /// Block of bytes of the cuurent transfer.
        /// </summary>
        public byte[] TransferredBytes { get; private set; }

        /// <summary>
        /// Total number of bytes being transferred. May be 0 if unknown.
        /// </summary>
        public int TotalBytes { get; private set; }

        public TftpTransferProgress(int transferred, byte[] tansferredBytes, int total)
        {
            Transferred = transferred;
            TransferredBytes = tansferredBytes;
            TotalBytes = total;
        }

        public override string ToString()
        {
            if (TotalBytes > 0)
                return (Transferred * 100) / TotalBytes + "% completed";
            else
                return Transferred + " bytes transferred";
        }
    }
}
