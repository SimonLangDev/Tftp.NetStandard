using Tftp.NetStandard.Commands;
using Tftp.NetStandard.Trace;

namespace Tftp.NetStandard.Transfer.States
{
    class ReceivedError : BaseState
    {
        private readonly TftpTransferError error;

        public ReceivedError(Error error)
            : this(new TftpErrorPacket(error.ErrorCode, error.Message)) { }

        public ReceivedError(TftpTransferError error)
        {
            this.error = error;
        }

        public override void OnStateEnter()
        {
            TftpTrace.Trace("Received error: " + error, Context);
            Context.RaiseOnError(error);
            Context.SetState(new Closed());
        }
    }
}
