﻿namespace RSTest.Entities
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
            string normalizedCurrentStreetName = Utilities.String.NormalizeString(this.Address.StreetAddress);
            string normalizedCurrentState = Utilities.String.NormalizeString(this.Address.State);
            string normalizedCurrentSuburb = Utilities.String.NormalizeString(this.Address.Suburb);

            string normalizedStreetName = Utilities.String.NormalizeString(user.Address.StreetAddress);
            string normalizedState = Utilities.String.NormalizeString(user.Address.State);
            string normalizedSuburb = Utilities.String.NormalizeString(user.Address.Suburb);

            return normalizedCurrentState.Equals(normalizedState)
                && normalizedCurrentStreetName.Equals(normalizedStreetName)
                && normalizedSuburb.Equals(normalizedSuburb);
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
                var reversedSubstr = Utilities.String.Reverse(lowerCaseOtherReferral.Substring(i, 3));
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

