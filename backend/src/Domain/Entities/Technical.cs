namespace Domain.Entities
{
    class Technical:User
    {
        public int Experience { get; set; }
        public string Specialty { get; set; } = string.Empty;
    }
}