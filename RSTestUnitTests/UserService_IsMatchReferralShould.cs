using System;
using System.Collections.Generic;
using RSTest.Entities;
using RSTest.Services;
using RSTest.Utilities;
using Xunit;

namespace RSTestUnitTests;

public class UserService_IsMatchReferralShould
{
    Random rand = new Random();

    User userTest1 = new User
    {
        Name = Guid.NewGuid().ToString("n").Substring(0, 8),
        ReferralCode = "ABC123",
        Address = new Address
        {
            StreetAddress = Guid.NewGuid().ToString("n").Substring(0, 8),
            State = Guid.NewGuid().ToString("n").Substring(0, 8),
            Suburb = Guid.NewGuid().ToString("n").Substring(0, 8),
            Latitude = 0,
            Longitude = 0,
        }
    };

    List<(decimal, decimal)> randomPoints = Geography.GetRandomPointsOutsideRadius(
        0,
        0,
        500,
        1);

    [Theory]
    [InlineData("ABC123")]
    [InlineData("ABC321")]
    [InlineData("CBA123")]
    [InlineData("AB21C3")]
    [InlineData("A1CB23")]
    public void UserService_IsMatch_ValueIsSameReferral_ReturnTrue(string testReferralCode)
    {
        UserService userService = new();

        User userTest2 = new User
        {
            Name = Guid.NewGuid().ToString("n").Substring(0, 8),
            ReferralCode = testReferralCode,
            Address = new Address
            {
                StreetAddress = Guid.NewGuid().ToString("n").Substring(0, 8),
                State = Guid.NewGuid().ToString("n").Substring(0, 8),
                Suburb = Guid.NewGuid().ToString("n").Substring(0, 8),
                Latitude = randomPoints[0].Item2,
                Longitude = randomPoints[0].Item1,
            }
        };
        Assert.True(userService.IsMatch(userTest1, userTest2), "These two users with same referral code should match");

    }

    [Fact]
    public void UserService_IsMatch_ValueIsDifferReferral_ReturnFalse()
    {
        UserService userService = new();

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
        Assert.False(userService.IsMatch(userTest1, userTest2), "These two users with different code should not match");

    }

}
