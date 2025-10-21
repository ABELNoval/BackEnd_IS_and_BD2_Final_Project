namespace Domain.Entities
{
    class Assessment
    {
        public int Id { get; set; }
        public string Coment { get; set; } = string.Empty;
        public float Punctuation { get; set; } = 0;
        public DateTime Date{ get; set; }
        public Director Director { get; set; } = null!;
        public Technical Technical { get; set; } = null!;
        
    }
}