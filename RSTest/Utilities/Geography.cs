using System;
namespace RSTest.Utilities
{
	public static class Geography
    {
        // Check if provided coordinates are within distance
        public static bool IsWithinDistance(decimal distance,
                                            decimal longitude,
                                            decimal latitude,
                                            decimal otherLongitude,
                                            decimal otherLatitude)
        {
            decimal calcDistance = GetDistance(longitude, latitude, otherLongitude, otherLatitude);
            return distance >= calcDistance;
        }

        // Get distance between provided coordinates
        public static decimal GetDistance(decimal longitude,
                                          decimal latitude,
                                          decimal otherLongitude,
                                          decimal otherLatitude)
        {
            var baseRad = (decimal)Math.PI * latitude / 180;
            var targetRad = (decimal)Math.PI * otherLatitude / 180;
            var theta = longitude - otherLongitude;
            var thetaRad = (decimal)Math.PI * theta / 180;

            double dist =
                Math.Sin((double)baseRad) * Math.Sin((double)targetRad) + Math.Cos((double)baseRad) *
                Math.Cos((double)targetRad) * Math.Cos((double)thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return 1609.34m * (decimal)dist;
        }

        // Get random coordinates within provided radius from origin point. Output is tuple of decimals.
        public static List<(decimal, decimal)> GetRandomPointsInRadius(
            decimal x,
            decimal y,
            double rMaxMeters,
            int n)
        {
            double PI = 3.141592653589;
            // Convert radius from meters to degrees.
            double rMax = rMaxMeters / 111320f;
            // Result vector
            List<(decimal, decimal)> res = new List<(decimal, decimal)>();
            for (int i = 0; i < n; i++)
            {

                // Get Angle in radians
                double theta = 2.0
                    * PI
                    * new Random().NextDouble();

                // Get length from center
                double len = Math.Sqrt(new Random().NextDouble()) * rMax;

                // Add point to results.
                res.Add((x + (decimal)(len * Math.Cos(theta)),
                                 y + (decimal)(len * Math.Sin(theta))));
            }

            // Return the N points
            return res;
        }

        // Get random coordinates outside provided radius from origin poiint. Output is tuple of decimals.
        public static List<(decimal, decimal)> GetRandomPointsOutsideRadius(
            decimal x,
            decimal y,
            double rMaxMeters,
            int n
            )
        {
            // Result vector
            List<(decimal, decimal)> res = new List<(decimal, decimal)>();
            for (int i = 0; i < n; i++)
            {
                // Create random point
                decimal longitude = (decimal)new Random().NextDouble();
                decimal latitude = (decimal)new Random().NextDouble();
                bool isWithinDistance = RSTest.Utilities.Geography.IsWithinDistance(
                    (decimal)rMaxMeters,
                    x,
                    y,
                    longitude,
                    latitude);
                // Check if within distance, regenerate if so
                while (isWithinDistance)
                {
                    longitude = (decimal)new Random().NextDouble();
                    latitude = (decimal)new Random().NextDouble();
                    isWithinDistance = RSTest.Utilities.Geography.IsWithinDistance(
                        (decimal)rMaxMeters,
                        x,
                        y,
                        longitude,
                        latitude);
                }
                res.Add((longitude, latitude));
            }
            return res;
        }
    }
}

