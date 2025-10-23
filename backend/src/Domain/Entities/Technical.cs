namespace Domain.Entities
{
    public class Technical:User
    {
        public int Experience { get; set; }
        public string Specialty { get; set; } = string.Empty;
    }
}