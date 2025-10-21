namespace Domain.Entities
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gmail { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = null!;
    }   
}