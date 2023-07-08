using System.Diagnostics;
using NearestVehicleLocator;

var stopwatch = new Stopwatch();
var coordinates = new[]
{
    new Point { Latitude = 34.544909f, Longitude = -102.100843f },
    new Point { Latitude = 32.345544f, Longitude = -99.123124f },
    new Point { Latitude = 33.234235f, Longitude = -100.214124f },
    new Point { Latitude = 35.195739f, Longitude = -95.348899f },
    new Point { Latitude = 31.895839f, Longitude = -97.789573f },
    new Point { Latitude = 32.895839f, Longitude = -101.789573f },
    new Point { Latitude = 34.115839f, Longitude = -100.225732f },
    new Point { Latitude = 32.335839f, Longitude = -99.992232f },
    new Point { Latitude = 33.535339f, Longitude = -94.792232f },
    new Point { Latitude = 32.234235f, Longitude = -100.222222f }
};
var fileName = "VehiclePositions.dat";
var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
long totalElapsedTime = 0, currentElapsedTime;
stopwatch.Start();
var vehicleData = FileReader.ReadData(filePath);
stopwatch.Stop();
currentElapsedTime = stopwatch.ElapsedMilliseconds;
totalElapsedTime += currentElapsedTime;
PrintOutTime("Reading bytes from file took:", currentElapsedTime);

stopwatch.Restart();
var vehicles = Vehicle.ParseVehicleData(vehicleData);
stopwatch.Stop();
currentElapsedTime = stopwatch.ElapsedMilliseconds;
totalElapsedTime += currentElapsedTime;
PrintOutTime("Parsing data took:", currentElapsedTime);

stopwatch.Restart();
var vehicleCoordinateTree = BuildVehicleCoordinateTree(vehicles);
stopwatch.Stop();
currentElapsedTime = stopwatch.ElapsedMilliseconds;
totalElapsedTime += currentElapsedTime;
PrintOutTime("Building coordinate tree took:", currentElapsedTime);

stopwatch.Restart();
for (int i = 0; i < coordinates.Length; i++)
{
    vehicleCoordinateTree.FindNearestCoordinate(coordinates[i]);
}
stopwatch.Stop();
currentElapsedTime = stopwatch.ElapsedMilliseconds;
totalElapsedTime += currentElapsedTime;
PrintOutTime("Finding nearest coordinates took:", currentElapsedTime);
PrintOutTime("Total Time:", totalElapsedTime);
Console.WriteLine("\nPress any key to continue...");
Console.ReadKey();

static CoordinateTree BuildVehicleCoordinateTree(List<Vehicle> vehicles)
{
    var coordinateTree = new CoordinateTree();

    for (int i = 0; i < vehicles.Count; i++)
    {
        var vehicle = vehicles[i];
        coordinateTree.Insert(new Point
        {
            Latitude = vehicle.Latitude,
            Longitude = vehicle.Longitude
        });
    }
    return coordinateTree;
}

static void PrintOutTime(string text, long elapsedMilliseconds)
{
    Console.WriteLine($"{text} {elapsedMilliseconds} ms");
}