using System;
using System.Collections.Generic;
using RSTest.Entities;
using RSTest.Services;
using RSTest.Utilities;
using Xunit;

namespace RSTestUnitTests;

public class UserService_IsMatchAddressShould
{
    Random rand = new Random();

    string specialChars = "!@#$%^&*?";

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

    List<(decimal, decimal)> randomPoints = Geography.GetRandomPointsOutsideRadius(
        0,
        0,
        500,
        1);

    [Fact]
    public void UserService_IsMatch_ValueIsSameAddress_ReturnTrue()
    {
        UserService userService = new();

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
                    Latitude = randomPoints[0].Item2,
                    Longitude = randomPoints[0].Item1,
                }
            };
            Assert.True(userService.IsMatch(userTest1, userTest2), "These two users with same address should match");

    }

    [Theory]
    // Completely different address
    [InlineData(true, true, true)]
    // Only same suburb
    [InlineData(true, true, false)]
    // Only same state
    [InlineData(true, false, true)]
    // Only same street
    [InlineData(false, true, true)]
    // Only state differ
    [InlineData(false, true, false)]
    // Only suburb differ
    [InlineData(false, false, true)]
    // Only street differ
    [InlineData(true, false, false)]
    public void UserService_IsMatch_ValueIsDifferentAddress_ReturnFalse(
        bool difStrAddress, bool difState, bool difSuburb)
    {
        UserService userService = new();
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
                StreetAddress = difStrAddress ? Guid.NewGuid().ToString("n")[..8] : strAddrWithSpecialChars,
                State = difState ? Guid.NewGuid().ToString("n")[..8] : stateWithSpecialChars,
                Suburb = difSuburb ? Guid.NewGuid().ToString("n")[..8] : suburbWithSpecialChars,
                Latitude = randomPoints[0].Item2,
                Longitude = randomPoints[0].Item1,
            }
        };
        Assert.False(userService.IsMatch(userTest1, userTest2), "Two users with different address should not match");

    }
}
