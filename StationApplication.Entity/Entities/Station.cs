namespace StationApplication.Entity.Entities
{
    public class Station:BaseEntity<int>
    {
        public string Name { get; set; }
        public double StartDistance { get; set; }
    }
}
