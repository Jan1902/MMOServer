using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMOServer.Packets
{
    class Packet
    {
        private List<byte> _bytes = new List<byte>();

        public PacketOP OP { get; private set; }

        public Packet(PacketOP op)
        {
            OP = op;
        }

        public Packet(PacketOP op, byte[] data)
        {
            OP = op;
            _bytes = data.ToList();
            _bytes.RemoveAt(0);
        }

        public void Write(int value)
        {
            _bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(float value)
        {
            _bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(double value)
        {
            _bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(bool value)
        {
            _bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(string value)
        {
            _bytes.AddRange(BitConverter.GetBytes(value.Length));
            _bytes.AddRange(Encoding.ASCII.GetBytes(value));
        }

        public void Write(byte value)
        {
            _bytes.Add(value);
        }

        public void Write(byte[] value)
        {
            _bytes.AddRange(value);
        }

        public int ReadInt()
        {
            var value = BitConverter.ToInt32(_bytes.ToArray(), 0);
            _bytes.RemoveRange(0, sizeof(int));
            return value;
        }

        public float ReadFloat()
        {
            var value = BitConverter.ToSingle(_bytes.ToArray(), 0);
            _bytes.RemoveRange(0, sizeof(float));
            return value;
        }

        public double Double()
        {
            var value = BitConverter.ToDouble(_bytes.ToArray(), 0);
            _bytes.RemoveRange(0, sizeof(double));
            return value;
        }

        public bool ReadBool()
        {
            var value = BitConverter.ToBoolean(_bytes.ToArray(), 0);
            _bytes.RemoveRange(0, sizeof(bool));
            return value;
        }

        public byte ReadByte()
        {
            var value = _bytes[0];
            _bytes.RemoveRange(0, sizeof(byte));
            return value;
        }

        public byte[] ReadBytes(int count)
        {
            var value = new byte[count];
            for (int i = 0; i < count; i++)
            {
                value[i] = _bytes[i];
            }
            _bytes.RemoveRange(0, count);
            return value;
        }

        public byte[] CreateFinalPacket()
        {
            var data = new List<byte>();
            data.Add((byte)OP);
            data.AddRange(_bytes);
            return data.ToArray();
        }

        public virtual byte[] Create() { throw new Exception("Cant use internal create function by itself"); }
    }
}
