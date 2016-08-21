namespace LoadingNavigationPropertiesOnDerivedTypes
{
    public class Foreman
    {
        public int Id { get; set; }

        public int LocationId { get; set; }

        public string Name { get; set; }

        public virtual JobSite JobSite { get; set; }
    }
}
