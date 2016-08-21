namespace LoadingNavigationPropertiesOnDerivedTypes
{
    public class Plumber : Tradesman
    {
        public bool IsCertified { get; set; }
        public int LocationId { get; set; }

        public virtual JobSite JobSite { get; set; }
    }
}