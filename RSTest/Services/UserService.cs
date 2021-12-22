using System;
using RSTest.Entities;

namespace RSTest.Services
{
    public class UserService: IUserMatcher
    {
        public UserService()
        {
        }

        public bool IsMatch(User user, User otherUser)
        {
            return user.IsMatch(otherUser);
        }
    }
}

