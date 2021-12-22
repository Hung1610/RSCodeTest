using System;
namespace RSTest.Entities
{
    public class Address
    {
        public Address ()
        {
        }

        public Address(
            string streetAddress,
            string suburb,
            string state,
            decimal latitude,
            decimal longtitude)
        {
            StreetAddress = streetAddress;
            Suburb = suburb;
            State = state;
            Latitude = latitude;
            Longitude = longtitude;
        }

        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}

