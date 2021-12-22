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

    [Theory]
    [InlineData(300)]
    [InlineData(400)]
    [InlineData(500)]
    public void UserService_IsMatch_ValueIsWithinDistance_ReturnTrue(double distance)
    {
        UserService userService = new();

        List<(decimal, decimal)> randomPoints = GetRandomPoints(
            userTest1.Address.Longitude,
            userTest1.Address.Latitude,
            distance,
            5);

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

    [Fact]
    public void UserService_IsMatch_ValueIsSameAddress_ReturnTrue()
    {
        UserService userService = new();

        string strAddrWithSpecialChars = String.Format("!@#$%^{0}&*?", userTest1.Address.StreetAddress);
        string stateWithSpecialChars = String.Format("!@#$%^{0}&*?", userTest1.Address.State);
        string suburbWithSpecialChars = String.Format("!@#$%^{0}&*?", userTest1.Address.Suburb);

        User userTest2 = new User
        {
            Name = Guid.NewGuid().ToString("n").Substring(0, 8),
            ReferralCode = Guid.NewGuid().ToString("n").Substring(0, 8),
            Address = new Address
            {
                StreetAddress = strAddrWithSpecialChars,
                State = stateWithSpecialChars,
                Suburb = suburbWithSpecialChars,
                Latitude = 1,
                Longitude = 2,
            }
        };
        Assert.True(userService.IsMatch(userTest1, userTest2), "These two users with same address should match");
    }

    [Fact]
    public void UserService_IsMatch_ValueIsSameName_ReturnTrue()
    {
        UserService userService = new();

        User userTest2 = new User
        {
            Name = userTest1.Name,
            ReferralCode = Guid.NewGuid().ToString("n").Substring(0, 8),
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

    private List<(decimal, decimal)> GetRandomPoints(decimal x, decimal y, double rMaxMeters, int n)
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
}
