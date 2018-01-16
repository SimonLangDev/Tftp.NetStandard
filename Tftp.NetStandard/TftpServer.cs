﻿using System;
using System.Net;
using Tftp.NetStandard.Channel;
using Tftp.NetStandard.Commands;
using Tftp.NetStandard.Transfer;

namespace Tftp.NetStandard
{
    public delegate void TftpServerEventHandler(ITftpTransfer transfer, EndPoint client);
    public delegate void TftpServerErrorHandler(TftpTransferError error);

    /// <summary>
    /// A simple TFTP server class. <code>Dispose()</code> the server to close the socket that it listens on.
    /// </summary>
    public class TftpServer : IDisposable
    {
        public const int DEFAULT_SERVER_PORT = 69;

        /// <summary>
        /// Fired when the server receives a new read request.
        /// </summary>
        public event TftpServerEventHandler OnReadRequest;

        /// <summary>
        /// Fired when the server receives a new write request.
        /// </summary>
        public event TftpServerEventHandler OnWriteRequest;

        /// <summary>
        /// Fired when the server encounters an error (for example, a non-parseable request)
        /// </summary>
        public event TftpServerErrorHandler OnError;

        /// <summary>
        /// Server port that we're listening on.
        /// </summary>
        private readonly ITransferChannel serverSocket;

        /// <summary>
        /// Timeout of the tftp server.
        /// </summary>
        private readonly int Timeout;

        public TftpServer(IPEndPoint localAddress)
        {
            if (localAddress == null)
                throw new ArgumentNullException("localAddress");

            serverSocket = TransferChannelFactory.CreateServer(localAddress);
            serverSocket.OnCommandReceived += new TftpCommandHandler(serverSocket_OnCommandReceived);
            serverSocket.OnError += new TftpChannelErrorHandler(serverSocket_OnError);
        }

        public TftpServer(IPEndPoint localAddress, TimeSpan timeout)
        {
            if (localAddress == null)
                throw new ArgumentNullException("localAddress");

            Timeout = (int) timeout.TotalMilliseconds;

            serverSocket = TransferChannelFactory.CreateServer(localAddress);
            serverSocket.OnCommandReceived += new TftpCommandHandler(serverSocket_OnCommandReceived);
            serverSocket.OnError += new TftpChannelErrorHandler(serverSocket_OnError);
        }

        public TftpServer(IPAddress localAddress)
            : this(localAddress, DEFAULT_SERVER_PORT)
        {
        }

        public TftpServer(IPAddress localAddress, int port)
            : this(new IPEndPoint(localAddress, port))
        {
        }

        public TftpServer(int port)
            : this(new IPEndPoint(IPAddress.Any, port))
        {
        }

        public TftpServer()
            : this(DEFAULT_SERVER_PORT)
        {
        }

        public TftpServer(IPAddress localAddress, TimeSpan timeout)
            : this(localAddress, DEFAULT_SERVER_PORT, timeout)
        {
        }

        public TftpServer(IPAddress localAddress, int port, TimeSpan timeout)
            : this(new IPEndPoint(localAddress, port), timeout)
        {
        }

        public TftpServer(int port, TimeSpan timeout)
            : this(new IPEndPoint(IPAddress.Any, port), timeout)
        {
        }

        public TftpServer(TimeSpan timeout)
            : this(DEFAULT_SERVER_PORT, timeout)
        {
        }


        /// <summary>
        /// Start accepting incoming connections.
        /// </summary>
        public void Start()
        {
            serverSocket.Open();
        }

        void serverSocket_OnError(TftpTransferError error)
        {
            RaiseOnError(error);
        }

        private void serverSocket_OnCommandReceived(ITftpCommand command, EndPoint endpoint)
        {
            //Ignore all other commands
            if (!(command is ReadOrWriteRequest))
                return;

            //Open a connection to the client
            ITransferChannel channel = TransferChannelFactory.CreateConnection(endpoint);

            //Create a wrapper for the transfer request
            ReadOrWriteRequest request = (ReadOrWriteRequest)command;
            ITftpTransfer transfer = null;

            if (Timeout > 0)
                transfer = request is ReadRequest ? (ITftpTransfer)new LocalReadTransfer(channel, request.Filename, request.Options, Timeout) : new LocalWriteTransfer(channel, request.Filename, request.Options, Timeout);
            else
                transfer = request is ReadRequest ? (ITftpTransfer)new LocalReadTransfer(channel, request.Filename, request.Options) : new LocalWriteTransfer(channel, request.Filename, request.Options);

            if (command is ReadRequest)
                RaiseOnReadRequest(transfer, endpoint);
            else if (command is WriteRequest)
                RaiseOnWriteRequest(transfer, endpoint);
            else
                throw new Exception("Unexpected tftp transfer request: " + command);
        }

        #region IDisposable
        public void Dispose()
        {
            serverSocket.Dispose();
        }
        #endregion

        private void RaiseOnError(TftpTransferError error)
        {
            if (OnError != null)
                OnError(error);
        }

        private void RaiseOnWriteRequest(ITftpTransfer transfer, EndPoint client)
        {
            if (OnWriteRequest != null)
            {
                OnWriteRequest(transfer, client);
            }
            else
            {
                transfer.Cancel(new TftpErrorPacket(0, "Server did not provide a OnWriteRequest handler."));
            }
        }

        private void RaiseOnReadRequest(ITftpTransfer transfer, EndPoint client)
        {
            if (OnReadRequest != null)
            {
                OnReadRequest(transfer, client);
            }
            else
            {
                transfer.Cancel(new TftpErrorPacket(0, "Server did not provide a OnReadRequest handler."));
            }
        }
    }
}

