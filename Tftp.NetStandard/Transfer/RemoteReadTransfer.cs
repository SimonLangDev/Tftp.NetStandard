using System;
using Tftp.NetStandard.Channel;
using Tftp.NetStandard.Transfer.States;

namespace Tftp.NetStandard.Transfer
{
    class RemoteReadTransfer : TftpTransfer
    {
        public RemoteReadTransfer(ITransferChannel connection, String filename)
            : base(connection, filename, new StartOutgoingRead())
        {
        }

        public override int ExpectedSize
        {
            get { return base.ExpectedSize; }
            set { throw new NotSupportedException("You cannot set the expected size of a file that is remotely transferred to this system."); }
        }
    }
}
