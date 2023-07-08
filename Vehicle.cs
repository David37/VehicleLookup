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
        var vehicle = new Vehicle();
        vehicle.VehicleId = BitConverter.ToInt32(data, index);
        index += sizeof(int);
        int stringLength = Array.IndexOf(data, (byte)0, index) - index;
        vehicle.VehicleRegistration = Encoding.ASCII.GetString(data, index, stringLength);
        index += stringLength + 1; // +1 to skip the null terminator
        vehicle.Latitude = BitConverter.ToSingle(data, index);
        index += sizeof(float);
        vehicle.Longitude = BitConverter.ToSingle(data, index);
        index += sizeof(float);
        vehicle.RecordedTimeUTC = BitConverter.ToUInt64(data, index);
        index += sizeof(ulong);
        return vehicle;
    }
}
