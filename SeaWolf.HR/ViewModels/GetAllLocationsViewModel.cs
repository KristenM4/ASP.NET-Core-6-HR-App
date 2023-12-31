﻿namespace SeaWolf.HR.ViewModels
{
    public class GetAllLocationsViewModel
    {
        public string LocationName { get; set; }
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
