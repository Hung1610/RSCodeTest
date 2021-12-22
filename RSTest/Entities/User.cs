namespace RSTest.Entities
{
    public class User
    {
        public User()
        {
        }
        public User(
            Address address,
            string name,
            string referralCode)
        {
            Address = address;
            Name = name;
            ReferralCode = referralCode;
        }

        public Address Address { get; set; }
        public string Name { get; set; }
        public string ReferralCode { get; set; }

        //Check if user matches another
        public bool IsMatch(User user) =>
            IsUserWithinDistance(user, 500.0m) ||
            IsUserMatchAddress(user) ||
            user.Name.Trim().ToLower().Equals(this.Name.Trim().ToLower()) ||
            IsUserMatchReferral(user);

        // Check for distance criteria
        private bool IsUserWithinDistance(User user, decimal distance)
        {
            return Utilities.Utility.IsWithinDistance(
                distance,
                this.Address.Longitude,
                this.Address.Latitude,
                user.Address.Longitude,
                user.Address.Latitude);
        }

        // Check for address criteria
        private bool IsUserMatchAddress(User user)
        {
            string normalizedCurrentStreetName = Utilities.Utility.NormalizeString(this.Address.StreetAddress);
            string normalizedCurrentState = Utilities.Utility.NormalizeString(this.Address.State);
            string normalizedCurrentSuburb = Utilities.Utility.NormalizeString(this.Address.Suburb);

            string normalizedStreetName = Utilities.Utility.NormalizeString(user.Address.StreetAddress);
            string normalizedState = Utilities.Utility.NormalizeString(user.Address.State);
            string normalizedSuburb = Utilities.Utility.NormalizeString(user.Address.Suburb);

            return normalizedCurrentState.Equals(normalizedState)
                && normalizedCurrentStreetName.Equals(normalizedStreetName)
                && normalizedSuburb.Equals(normalizedSuburb);
        }

        // Check if another user matches the referral code
        private bool IsUserMatchReferral(User user)
        {
            return Utilities.Utility.AreAnagrams(
                user.ReferralCode.ToLower(),
                this.ReferralCode.ToLower());
        }
    }
}

