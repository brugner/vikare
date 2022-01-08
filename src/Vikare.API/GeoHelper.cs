class GeoHelper
{
    /// <summary>
    /// Gets the distance between two points.
    /// </summary>
    /// <param name="lat1">Point 1 latitude.</param>
    /// <param name="lon1">Point 1 longitude.</param>
    /// <param name="lat2">Point 2 latitude.</param>
    /// <param name="lon2">Point 2 longitude.</param>
    /// <param name="unit">Unit of measure. K: kilometers, M: miles (default), N: nautical miles.</param>
    /// <returns></returns>
    public double GetDistance(double lat1, double lon1, double lat2, double lon2, char unit)
    {
        if ((lat1 == lat2) && (lon1 == lon2))
        {
            return 0;
        }
        else
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(DegreesToRadians(lat1)) * Math.Sin(DegreesToRadians(lat2)) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) * Math.Cos(DegreesToRadians(theta));

            dist = Math.Acos(dist);
            dist = RadiansToDegrees(dist);
            dist = dist * 60 * 1.1515;

            if (unit == 'k')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'n')
            {
                dist = dist * 0.8684;
            }

            return (dist);
        }
    }

    private double DegreesToRadians(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    private double RadiansToDegrees(double rad)
    {
        return (rad / Math.PI * 180.0);
    }
}