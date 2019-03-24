using Domain.Base;

namespace Domain.Entities
{
    public class Book : Entity<int> 
    {
        public string Title { get; set; }
        public string Author { get; set; }
    } 
}
