using System.IO;
using System.Text;

namespace GrandChase.IO.Packet
{
    public abstract class PacketBase
    {
        public abstract int Length { get; }
        protected int _index;

        public int Position
        {
            get { return _index; }
            set { _index = value; }
        }

        public abstract byte[] ToArray();

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (byte b in this.ToArray())
            {
                sb.AppendFormat("{0:X2} ", b);
            }

            return sb.ToString();
        }

    }
}
