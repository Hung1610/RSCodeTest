using System;
using System.Collections.Generic;
using RSTest.Entities;
using RSTest.Services;
using Xunit;

namespace RSTestUnitTests;

public class UserService_IsMatchShould
{
    Random rand = new Random();

    User userTest1 = new User
    {
        Name = Guid.NewGuid().ToString("n").Substring(0, 8),
        ReferralCode = Guid.NewGuid().ToString("n").Substring(0, 8),
        Address = new Address
        {
            StreetAddress = Guid.NewGuid().ToString("n").Substring(0, 8),
            State = Guid.NewGuid().ToString("n").Substring(0, 8),
            Suburb = Guid.NewGuid().ToString("n").Substring(0, 8),
            Latitude = 0,
            Longitude = 0,
        }
    };

    [Fact]
    public void UserService_IsMatch_ValueIsSameUser_ReturnTrue()
    {
        UserService userService = new();

        Assert.True(userService.IsMatch(userTest1, userTest1), "The same user should match");
    }

    [Theory]
    [InlineData(500)]
    public void UserService_IsMatch_ValueIsWithoutDistance_ReturnFalse(double distance)
    {
        UserService userService = new();

        List<(decimal, decimal)> randomPoints = GetRandomPointsOutsideRadius(
            userTest1.Address.Longitude,
            userTest1.Address.Latitude,
            distance,
            50);


        foreach ((decimal longitude, decimal latitude) in randomPoints)
        {
            User userTest2 = new User
            {
                Name = Guid.NewGuid().ToString("n").Substring(0, 8),
                ReferralCode = Guid.NewGuid().ToString("n").Substring(0, 8),
                Address = new Address
                {
                    StreetAddress = Guid.NewGuid().ToString("n").Substring(0, 8),
                    State = Guid.NewGuid().ToString("n").Substring(0, 8),
                    Suburb = Guid.NewGuid().ToString("n").Substring(0, 8),
                    Latitude = latitude,
                    Longitude = longitude,
                }
            };
            Assert.False(userService.IsMatch(userTest1, userTest2), "These two users within distance should match");
        }
    }

    [Theory]
    [InlineData(500)]
    public void UserService_IsMatch_ValueIsWithinDistance_ReturnTrue(double distance)
    {
        UserService userService = new();

        List<(decimal, decimal)> randomPoints = GetRandomPointsInRadius(
            userTest1.Address.Longitude,
            userTest1.Address.Latitude,
            distance,
            50);

        Assert.True(userService.IsMatch(userTest1, userTest1), "The same user should match");

        foreach ((decimal longitude, decimal latitude) in randomPoints) {
            User userTest2 = new User
            {
                Name = Guid.NewGuid().ToString("n").Substring(0, 8),
                ReferralCode = Guid.NewGuid().ToString("n").Substring(0, 8),
                Address = new Address
                {
                    StreetAddress = Guid.NewGuid().ToString("n").Substring(0, 8),
                    State = Guid.NewGuid().ToString("n").Substring(0, 8),
                    Suburb = Guid.NewGuid().ToString("n").Substring(0, 8),
                    Latitude = latitude,
                    Longitude = longitude,
                }
            };
            Assert.True(userService.IsMatch(userTest1, userTest2), "These two users within distance should match");
        }
    }

    [Theory]
    [InlineData("!@#$%^&*?", 500)]
    public void UserService_IsMatch_ValueIsSameAddress_ReturnTrue(string specialChars, double distance)
    {
        UserService userService = new();

        List<(decimal, decimal)> randomPoints = GetRandomPointsOutsideRadius(
            userTest1.Address.Longitude,
            userTest1.Address.Latitude,
            distance,
            50);

        foreach ((decimal randomLongitude, decimal randomLatitude) in randomPoints)
        {
            string strAddrWithSpecialChars = specialChars.Insert(
                new Random().Next(0, specialChars.Length),
                userTest1.Address.StreetAddress);
            string stateWithSpecialChars = specialChars.Insert(
                new Random().Next(0, specialChars.Length),
                userTest1.Address.State);
            string suburbWithSpecialChars = specialChars.Insert(
                new Random().Next(0, specialChars.Length),
                userTest1.Address.Suburb);

            User userTest2 = new User
            {
                Name = Guid.NewGuid().ToString("n").Substring(0, 8),
                ReferralCode = Guid.NewGuid().ToString("n").Substring(0, 8),
                Address = new Address
                {
                    StreetAddress = strAddrWithSpecialChars,
                    State = stateWithSpecialChars,
                    Suburb = suburbWithSpecialChars,
                    Latitude = randomLatitude,
                    Longitude = randomLongitude,
                }
            };
            Assert.True(userService.IsMatch(userTest1, userTest2), "These two users with same address should match");

        }
    }

    [Theory]
    [InlineData(500)]
    public void UserService_IsMatch_ValueIsDifferentName_ReturnFalse(double distance)
    {
        UserService userService = new();

        List<(decimal, decimal)> randomPoints = GetRandomPointsOutsideRadius(
            userTest1.Address.Longitude,
            userTest1.Address.Latitude,
            distance,
            1);

        User userTest2 = new User
        {
            Name = Guid.NewGuid().ToString("n").Substring(0, 8),
            ReferralCode = Guid.NewGuid().ToString("n").Substring(0, 8),
            Address = new Address
            {
                StreetAddress = Guid.NewGuid().ToString("n").Substring(0, 8),
                State = Guid.NewGuid().ToString("n").Substring(0, 8),
                Suburb = Guid.NewGuid().ToString("n").Substring(0, 8),
                Latitude = randomPoints[0].Item2,
                Longitude = randomPoints[0].Item1,
            }
        };
        Assert.False(userService.IsMatch(userTest1, userTest2), "These two users with same name should match");
    }

    [Fact]
    public void UserService_IsMatch_ValueIsSameReferral_ReturnTrue()
    {
        UserService userService = new();

        userTest1.ReferralCode = "ABC123";

        User userTest2 = new User
        {
            Name = Guid.NewGuid().ToString("n").Substring(0, 8),
            ReferralCode = "ABC321",
            Address = new Address
            {
                StreetAddress = Guid.NewGuid().ToString("n").Substring(0, 8),
                State = Guid.NewGuid().ToString("n").Substring(0, 8),
                Suburb = Guid.NewGuid().ToString("n").Substring(0, 8),
                Latitude = 1,
                Longitude = 2,
            }
        };
        Assert.True(userService.IsMatch(userTest1, userTest2), "These two users with same name should match");


        User userTest3 = new User
        {
            Name = Guid.NewGuid().ToString("n").Substring(0, 8),
            ReferralCode = "AB21C3",
            Address = new Address
            {
                StreetAddress = Guid.NewGuid().ToString("n").Substring(0, 8),
                State = Guid.NewGuid().ToString("n").Substring(0, 8),
                Suburb = Guid.NewGuid().ToString("n").Substring(0, 8),
                Latitude = 1,
                Longitude = 2,
            }
        };
        Assert.True(userService.IsMatch(userTest1, userTest3), "These two users with same name should match");

    }

    private List<(decimal, decimal)> GetRandomPointsInRadius(
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

    private List<(decimal, decimal)> GetRandomPointsOutsideRadius(
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
