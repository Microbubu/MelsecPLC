
namespace PlcCommunication.Common
{
    public interface ICommunication
    {
        int Read(int station, string startAddress, int length);

        bool Write(int station, string startAdress, int length, object data);
    }
}
