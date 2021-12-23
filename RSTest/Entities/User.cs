using RSTest.Extensions;

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
            return Utilities.Geography.IsWithinDistance(
                distance,
                this.Address.Longitude,
                this.Address.Latitude,
                user.Address.Longitude,
                user.Address.Latitude);
        }

        // Check for address criteria
        private bool IsUserMatchAddress(User user)
        {
            string normalizedCurrentStreetName = this.Address.StreetAddress.Standandize();
            string normalizedCurrentState = this.Address.State.Standandize();
            string normalizedCurrentSuburb = this.Address.Suburb.Standandize();

            string normalizedStreetName = user.Address.StreetAddress.Standandize();
            string normalizedState = user.Address.State.Standandize();
            string normalizedSuburb = user.Address.Suburb.Standandize();

            return normalizedCurrentState.Equals(normalizedState)
                && normalizedCurrentStreetName.Equals(normalizedStreetName)
                && normalizedCurrentSuburb.Equals(normalizedSuburb);
        }

        // Check if another user matches the referral code
        private bool IsUserMatchReferral(User user)
        {
            var lowerCaseReferral = this.ReferralCode.ToLower();
            var lowerCaseOtherReferral = user.ReferralCode.ToLower();

            if (lowerCaseReferral.Equals(lowerCaseOtherReferral))
                return true;

            for (int i = 0; i <= this.ReferralCode.Length - 3; i++)
            {
                var reversedSubstr = lowerCaseOtherReferral.Substring(i, 3).Reverse();
                var correctedString = String.Format("{0}{1}{2}",
                    lowerCaseReferral.Substring(0, i),
                    reversedSubstr,
                    lowerCaseReferral.Substring(i + 3, lowerCaseReferral.Length - (i+3)));

                if (lowerCaseReferral.Equals(correctedString))
                    return true;
            }

            return false;
        }
    }
}

