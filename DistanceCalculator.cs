namespace NearestVehicleLocator;
public static class DistanceCalculator
{
    public static double GetDistance(Point point1, Point point2)
    {
        if (point1.Latitude == point2.Latitude && point1.Longitude == point2.Longitude)
        {
            return 0;
        }

        var longitudeDifference = point1.Longitude - point2.Longitude;
        var latitude1Rad = DegreesToRadians(point1.Latitude);
        var latitude2Rad = DegreesToRadians(point2.Latitude);
        var centralAngle = Math.Sin(latitude1Rad) * Math.Sin(latitude2Rad)
            + Math.Cos(latitude1Rad) * Math.Cos(latitude2Rad) * Math.Cos(DegreesToRadians(longitudeDifference));
        var distanceInRadians = Math.Acos(centralAngle);
        var distanceInDegrees = RadiansToDegrees(distanceInRadians);
        var distanceInMiles = distanceInDegrees * 60 * 1.1515;
        var distanceInKilometers = distanceInMiles * 1.609344;
        return distanceInKilometers;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static double RadiansToDegrees(double radians)
    {
        return radians / Math.PI * 180.0;
    }
}