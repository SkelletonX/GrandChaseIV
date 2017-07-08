using GrandChase.IO;
using GrandChase.IO.Packet;
using System;
using System.Net;
using System.Net.Sockets;
using GrandChase.Utilities;

namespace GrandChase.Net
{
    public abstract class Session
    {
        protected Socket _socket;

        private byte[] _buffer;
        private int _bufferIndex;

        private bool _header;
        private bool _connected;

        private string _label;

        private uint riv;
        private uint siv;

        private object _lock;

        public string Label
        {
            get
            {
                return _label;
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connected;
            }
        }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                try
                {
                    return (IPEndPoint)this._socket.RemoteEndPoint;
                }
                catch
                {
                    return new IPEndPoint(0, 0);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the Session class.
        /// </summary>
        /// <param name="socket"></param>
        public Session(Socket socket)
        {
            this._socket = socket;
            this._socket.NoDelay = true;

            try
            {
                this._label = this._socket.RemoteEndPoint.ToString();
            }
            catch(SocketException)
            {
                this._label = "Error";
            }

            this._connected = true;

            this._lock = new object();
        }

        /// <summary>
        /// Initiates the receiving mechanism.
        /// </summary>
        /// <param name="length">The length of the data</param>
        /// <param name="header">Indicates if a header is received</param>
        protected void InitiateReceive(uint length, bool header = false)
        {
            if (!this._connected)
            {
                return;
            }

            this._header = header;
            this._buffer = new byte[length];
            this._bufferIndex = 0;

            this.BeginReceive();
        }

        /// <summary>
        /// Begins to asynchronously receive data from the socket.
        /// </summary>
        private void BeginReceive()
        {
            if (!this._connected)
            {
                return;
            }

            var error = SocketError.Success;

            this._socket.BeginReceive(this._buffer,
                this._bufferIndex,
                this._buffer.Length - this._bufferIndex,
                SocketFlags.None, out error,
                EndReceive,
                null);

            if (error != SocketError.Success)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Reads the data from the callback and handles it.
        /// </summary>
        /// <param name="iar"></param>
        private void EndReceive(IAsyncResult iar)
        {
            if (!this._connected)
            {
                return;
            }

            var error = SocketError.Success;
            int received = this._socket.EndReceive(iar, out error);

            if (received == 0 || error != SocketError.Success)
            {
                this.Close();
                return;
            }

            this._bufferIndex += received;

            if (this._bufferIndex == this._buffer.Length)
            {
                if (this._header)
                {
                    //EndianConvert.EndianConverter(_buffer, 0, 2);

                    uint header = BitConverter.ToUInt16(this._buffer, 0);

                    this.InitiateReceive(header - 2, false);
                }
                else
                {
                    this.OnPacket(new InPacket(this._buffer));
                    this.InitiateReceive(2, true);
                }
            }
            else
            {
                this.BeginReceive();
            }
        }

        /// <summary>
        /// Sends a GrandChase.IO.OutPacket array to the socket.
        /// </summary>
        /// <param name="outPacket"></param>
        public void Send(OutPacket outPacket)
        {
            //Log.Inform("Sent {0} packet to {1}.", outPacket.Opcode.ToString(), this.Label);

            this.Send(outPacket.getBuffer());

            /*
            using (OutPacket oPacket = new OutPacket())
            {
                using (OutPacket oInBuffer = new OutPacket())
                {
                    // original packet
                    oInBuffer.WriteInt(outPacket.Position + 6);
                    oInBuffer.WriteShort((short)outPacket.Opcode);
                    oInBuffer.WriteBytes(outPacket.ToArray());

                    oPacket.WriteInt(6 + 4 + oInBuffer.Position);
                    oPacket.WriteShort((short)outPacket.Opcode);
                    oPacket.WriteInt(0); // MD5 Checksum

                    oPacket.WriteBytes(oInBuffer.ToArray());
                }

                this.Send(oPacket.ToArray());
            }
            */

        }

        /// <summary>
        /// Sends data to the socket.
        /// </summary>
        /// <param name="buffer"></param>
        public void Send(byte[] buffer)
        {
            lock (_lock)
            {

                if (!this._connected)
                {
                    return;
                }

                this.SendRaw(buffer);
            }
        }

        public void SendRaw(byte[] data)
        {
            lock (_lock)
            {
                if (!this._connected)
                {
                    return;
                }

                int offset = 0;

                while (offset < data.Length)
                {
                    SocketError error = SocketError.Success;
                    
                    int sent = this._socket.Send(data, offset, data.Length - offset, SocketFlags.None, out error);
                   
                    if (sent == 0 || error != SocketError.Success)
                    {
                        throw new PacketSendException();
                    }

                    offset += sent;
                }
            }
        }

        /// <summary>
        /// Closes the socket.
        /// </summary>
        public void Close()
        {
            lock (_lock)
            {
                if (!this._connected)
                {
                    return;
                }
				
				this._connected = false;
                this._socket.Shutdown(SocketShutdown.Both);
                this._socket.Close();

                this.OnDisconnect();
            }
        }

        public abstract void OnDisconnect();
        public abstract void OnPacket(InPacket inPacket);
    }
}
