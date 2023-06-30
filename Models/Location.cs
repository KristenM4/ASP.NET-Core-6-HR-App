namespace SeaWolf.HR.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Phone { get; set; }
        public Employee? PersonInCharge { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set;} = string.Empty;
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
