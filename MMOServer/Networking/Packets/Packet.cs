using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMOServer.Networking.Packets
{
    class Packet
    {
        public List<byte> Bytes { get; private set; } = new List<byte>();

        public PacketOP OP { get; private set; }

        public Packet(PacketOP op)
        {
            OP = op;
            Bytes.Add((byte)op);
        }

        public Packet(byte[] data)
        {
            OP = (PacketOP)data[0];
            Bytes = data.ToList();
            Bytes.RemoveAt(0);
        }

        public void Write(short value)
        {
            Bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(int value)
        {
            Bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(long value)
        {
            Bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(float value)
        {
            Bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(double value)
        {
            Bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(bool value)
        {
            Bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(string value)
        {
            Bytes.AddRange(BitConverter.GetBytes(value.Length));
            Bytes.AddRange(Encoding.ASCII.GetBytes(value));
        }

        public void Write(byte value)
        {
            Bytes.Add(value);
        }

        public void Write(byte[] value)
        {
            Bytes.AddRange(value);
        }

        public int ReadShort()
        {
            var value = BitConverter.ToInt16(Bytes.ToArray(), 0);
            Bytes.RemoveRange(0, sizeof(short));
            return value;
        }

        public int ReadInt()
        {
            var value = BitConverter.ToInt32(Bytes.ToArray(), 0);
            Bytes.RemoveRange(0, sizeof(int));
            return value;
        }

        public long ReadLong()
        {
            var value = BitConverter.ToInt64(Bytes.ToArray(), 0);
            Bytes.RemoveRange(0, sizeof(long));
            return value;
        }

        public float ReadFloat()
        {
            var value = BitConverter.ToSingle(Bytes.ToArray(), 0);
            Bytes.RemoveRange(0, sizeof(float));
            return value;
        }

        public double Double()
        {
            var value = BitConverter.ToDouble(Bytes.ToArray(), 0);
            Bytes.RemoveRange(0, sizeof(double));
            return value;
        }

        public bool ReadBool()
        {
            var value = BitConverter.ToBoolean(Bytes.ToArray(), 0);
            Bytes.RemoveRange(0, sizeof(bool));
            return value;
        }

        public string ReadString()
        {
            var length = BitConverter.ToInt32(Bytes.ToArray(), 0);
            Bytes.RemoveRange(0, sizeof(int));
            var value = Encoding.ASCII.GetString(Bytes.GetRange(0, length).ToArray());
            Bytes.RemoveRange(0, length);
            return value;
        }

        public byte ReadByte()
        {
            var value = Bytes[0];
            Bytes.RemoveRange(0, sizeof(byte));
            return value;
        }

        public byte[] ReadBytes(int count)
        {
            var value = Bytes.GetRange(0, count).ToArray();
            Bytes.RemoveRange(0, count);
            return value;
        }

        public virtual byte[] Create() { throw new Exception("Cant use internal create function by itself"); }
    }
}
