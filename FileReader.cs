namespace NearestVehicleLocator;
public static class FileReader
{
    public static byte[] ReadData(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
        using BinaryReader reader = new(stream);
        var fileLength = (int)reader.BaseStream.Length;
        return reader.ReadBytes(fileLength);
    }
}
