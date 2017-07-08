using GrandChase.IO.Packet;

namespace GrandChase
{
    public interface ISerializable
    {
        void Serialize(OutPacket p);
        void Deserialize(InPacket p);
    }
}
