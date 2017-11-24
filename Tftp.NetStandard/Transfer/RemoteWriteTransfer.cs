using System;
using Tftp.NetStandard.Channel;
using Tftp.NetStandard.Transfer.States;

namespace Tftp.NetStandard.Transfer
{
    class RemoteWriteTransfer : TftpTransfer
    {
        public RemoteWriteTransfer(ITransferChannel connection, String filename)
            : base(connection, filename, new StartOutgoingWrite())
        {
        }
    }
}
