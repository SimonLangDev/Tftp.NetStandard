using System;
using System.Net;
using Tftp.NetStandard.Commands;

namespace Tftp.NetStandard.Channel
{
    delegate void TftpCommandHandler(ITftpCommand command, EndPoint endpoint);
    delegate void TftpChannelErrorHandler(TftpTransferError error);

    interface ITransferChannel : IDisposable
    {
        event TftpCommandHandler OnCommandReceived;
        event TftpChannelErrorHandler OnError;

        EndPoint RemoteEndpoint { get; set; }

        void Open();
        void Send(ITftpCommand command);
    }
}
