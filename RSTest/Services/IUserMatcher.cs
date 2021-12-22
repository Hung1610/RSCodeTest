using System;
using RSTest.Entities;

namespace RSTest.Services
{
    public interface IUserMatcher
    {
        bool IsMatch(User newUser, User existingUser);
    }
}

