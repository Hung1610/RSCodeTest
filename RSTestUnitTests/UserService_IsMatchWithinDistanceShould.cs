using System;
using System.Collections.Generic;
using RSTest.Entities;
using RSTest.Services;
using RSTest.Utilities;
using Xunit;

namespace RSTestUnitTests;

public class UserService_IsMatchDistanceShould
{
    Random rand = new Random();

    double distance = 500;

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

    List<(decimal, decimal)> randomPointsOutsideRadius = Geography.GetRandomPointsOutsideRadius(
        0,
        0,
        500,
        50);

    List<(decimal, decimal)> randomPointsInsideRadius = Geography.GetRandomPointsInRadius(
        0,
        0,
        500,
        50);

    [Fact]
    public void UserService_IsMatch_ValueIsWithoutDistance_ReturnFalse()
    {
        UserService userService = new();

        foreach ((decimal longitude, decimal latitude) in randomPointsOutsideRadius)
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

    [Fact]
    public void UserService_IsMatch_ValueIsWithinDistance_ReturnTrue()
    {
        UserService userService = new();

        foreach ((decimal longitude, decimal latitude) in randomPointsInsideRadius) {
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
}
