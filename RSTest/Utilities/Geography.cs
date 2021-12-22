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

    }
}

