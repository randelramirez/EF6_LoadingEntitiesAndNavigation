namespace LoadingNavigationPropertiesOnDerivedTypes
{
    public class Location
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZIPCode { get; set; }

        public int PhoneId { get; set; }

        public virtual Phone Phone { get; set; }
    }
}