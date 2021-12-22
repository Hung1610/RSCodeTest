# RateSetter Code Test
Simple C# console application serving as source code for the code test. Scenario and requirements below.
Presuming the required data has already been acquired from a database. The code resolves the problem at the most fundamental level using purely C# programming.
Provided basic unit testing.

## Scenario
RateSetter needs to compare new member registrations against our existing members, using a number of rules to reject new registrations. Currently we use the following (hypothetical!) rules:
1. Distance
If the new user lives within 500 metres of an existing user their registration will be rejected.
2. Address
If the new member’s name and address matches an existing member they will be rejected.
Our users may include unusual characters in their address, so we need to allow for this when matching. For example, the address “Level 3,! 51_Pitt Street, Sydney NSW-2000” would be a match for “Level 3, 51 Pitt Street, Sydney, NSW 2000”. We store our users’ names in title case.
3. Referral Code
A user can enter a referral code when registering. We need to ensure that no other user has entered the same code.
Unfortunately, one of our affiliate partners has a buggy referral code generation method. Their code always results in 3 characters of the referral code being reversed, which we need to account for - for example, “ABC123” would match to (among others) both “ABC321” and “AB21C3”.

## Assignment
Implement the following interface to determine whether a particular user from our database is a match for a new user (and should thus be rejected):
```
public interface IUserMatcher
{
  bool IsMatch(User newUser, User existingUser);
}
```

## Requirements
Your solution should:
1. Contain production quality code,
2. Be a zipped Visual Studio solution (ideally 2012+),
3. Assume we may add an arbitrary number of new rules in future, and
4. Provide suitable tests that prove your solution works.
