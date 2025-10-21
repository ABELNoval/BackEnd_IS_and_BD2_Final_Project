namespace Domain.Entities
{
    class Transfer
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public Department Origin { get; set; } = null!;
        public Department Destiny { get; set; } = null!;
        public Equipment Equipment { get; set; } = null!;
    }   
}