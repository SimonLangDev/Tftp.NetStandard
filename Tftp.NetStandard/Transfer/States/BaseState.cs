using System.Net;
using Tftp.NetStandard.Commands;

namespace Tftp.NetStandard.Transfer.States
{
    class BaseState : ITransferState
    {
        public TftpTransfer Context { get; set; }

        public virtual void OnStateEnter()
        {
            //no-op
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnCancel(TftpErrorPacket reason)
        {
        }

        public virtual void OnCommand(ITftpCommand command, EndPoint endpoint)
        {
        }

        public virtual void OnTimer()
        {
            //Ignore timer events
        }
    }
}
