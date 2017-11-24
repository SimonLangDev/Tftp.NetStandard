﻿using Tftp.NetStandard.Commands;

namespace Tftp.NetStandard.Transfer.States
{
    class CancelledByUser : BaseState
    {
        private readonly TftpErrorPacket reason;

        public CancelledByUser(TftpErrorPacket reason)
        {
            this.reason = reason;
        }

        public override void OnStateEnter()
        {
            Error command = new Error(reason.ErrorCode, reason.ErrorMessage);
            Context.GetConnection().Send(command);
            Context.SetState(new Closed());
        }
    }
}
