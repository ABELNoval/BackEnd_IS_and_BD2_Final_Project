namespace Domain.Entities
{
    class Maintenance
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public float Cost { get; set; } = 0;
        public string Type { get; set; } = string.Empty;
        public Equipment Equipment { get; set; } = null!;
        public Technical Technical{ get; set; } = null!;
    }   
}