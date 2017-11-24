﻿using System.IO;

namespace Tftp.NetStandard.Commands
{
    class TftpStreamWriter
    {
        private readonly Stream stream;

        public TftpStreamWriter(Stream stream)
        {
            this.stream = stream;
        }

        public void WriteUInt16(ushort value)
        {
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)(value & 0xFF));
        }

        public void WriteByte(byte b)
        {
            stream.WriteByte(b);
        }

        public void WriteBytes(byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
    }
}
