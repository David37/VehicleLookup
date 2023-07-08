namespace NearestVehicleLocator;
public class CoordinateTree
{
    private class Node
    {
        public Point Point { get; set; } = new Point();
        public Node? Left { get; set; }
        public Node? Right { get; set; }
        public int Depth { get; set; }
    }

    private Node? root = null;

    public void Insert(Point point)
    {
        root = Insert(root, point);
    }

    private static Node Insert(Node? node, Point point)
    {
        if (node == null)
        {
            return new Node { Point = point };
        }

        int depth = 0;
        Node? currentNode = node;
        Node? parentNode = null;
        while (currentNode != null)
        {
            parentNode = currentNode;
            int currentAxis = depth % 2; // Alternates between latitude and longitude

            currentNode = GetChildNode(currentNode, point, currentAxis);
            depth++;
        }

        int axis = (depth - 1) % 2;
        SetChildNode(parentNode, point, axis);

        return node;
    }

    private static Node GetChildNode(Node node, Point point, int axis)
    {
        if (axis == 0)
        {
            return point.Latitude < node.Point.Latitude ? node.Left : node.Right;
        }
        else
        {
            return point.Longitude < node.Point.Longitude ? node.Left : node.Right;
        }
    }

    private static void SetChildNode(Node node, Point point, int axis)
    {
        if (axis == 0)
        {
            if (point.Latitude < node.Point.Latitude)
            {
                node.Left = new Node { Point = point };
            }
            else
            {
                node.Right = new Node { Point = point };
            }
        }
        else
        {
            if (point.Longitude < node.Point.Longitude)
            {
                node.Left = new Node { Point = point };
            }
            else
            {
                node.Right = new Node { Point = point };
            }
        }
    }

    public Point FindNearestCoordinate(Point queryPoint)
    {
        double minDistance = double.PositiveInfinity;
        Point nearestNeighbor = new();
        FindNearestNeighbor(root, queryPoint, 0, ref nearestNeighbor, ref minDistance);
        return nearestNeighbor;
    }

    private void FindNearestNeighbor(Node? node, Point queryPoint, int depth, ref Point nearestCoordinate, ref double minDistance)
    {
        if (node is null)
        {
            return;
        }

        double distance = DistanceCalculator.GetDistance(node.Point, queryPoint);

        if (distance < minDistance)
        {
            nearestCoordinate = node.Point;
            minDistance = distance;
        }

        int currentAxis = depth % 2; // Alternates between latitude and longitude

        Node? nearChild, farChild;
        if ((currentAxis == 0 && queryPoint.Latitude < node.Point.Latitude) || (currentAxis == 1 && queryPoint.Longitude < node.Point.Longitude))
        {
            nearChild = node.Left;
            farChild = node.Right;
        }
        else
        {
            nearChild = node.Right;
            farChild = node.Left;
        }

        FindNearestNeighbor(nearChild, queryPoint, depth + 1, ref nearestCoordinate, ref minDistance);

        if (ShouldCheckFarChild(currentAxis, queryPoint, node.Point, minDistance))
        {
            FindNearestNeighbor(farChild, queryPoint, depth + 1, ref nearestCoordinate, ref minDistance);
        }
    }

    private static bool ShouldCheckFarChild(int currentAxis, Point queryPoint, Point nodePoint, double minDistance)
    {
        double axisDifference = currentAxis == 0 ? queryPoint.Latitude - nodePoint.Latitude : queryPoint.Longitude - nodePoint.Longitude;
        return Math.Abs(axisDifference) < minDistance;
    }
}