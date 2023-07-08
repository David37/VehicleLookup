using System.Text;

namespace NearestVehicleLocator;
public class Vehicle
{
    public int VehicleId;
    public string? VehicleRegistration;
    public float Latitude;
    public float Longitude;
    public ulong RecordedTimeUTC;

    public static List<Vehicle> ParseVehicleData(byte[] data)
    {
        var vehicles = new List<Vehicle>();
        int index = 0;

        while (index < data.Length)
        {
            var vehicle = ParseSingleVehicle(data, ref index);
            vehicles.Add(vehicle);
        }

        return vehicles;
    }

    private static Vehicle ParseSingleVehicle(byte[] data, ref int index)
    {
        return new Vehicle
        {
            VehicleId = ReadInt32(data, ref index),
            VehicleRegistration = ReadString(data, ref index),
            Latitude = ReadFloat(data, ref index),
            Longitude = ReadFloat(data, ref index),
            RecordedTimeUTC = ReadUInt64(data, ref index)
        };
    }

    private static int ReadInt32(byte[] data, ref int index)
    {
        int value = BitConverter.ToInt32(data, index);
        index += sizeof(int);
        return value;
    }

    private static string ReadString(byte[] data, ref int index)
    {
        int stringLength = Array.IndexOf(data, (byte)0, index) - index;
        string value = Encoding.ASCII.GetString(data, index, stringLength);
        index += stringLength + 1; // +1 to skip the null terminator
        return value;
    }

    private static float ReadFloat(byte[] data, ref int index)
    {
        float value = BitConverter.ToSingle(data, index);
        index += sizeof(float);
        return value;
    }

    private static ulong ReadUInt64(byte[] data, ref int index)
    {
        ulong value = BitConverter.ToUInt64(data, index);
        index += sizeof(ulong);
        return value;
    }
}
