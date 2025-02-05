namespace PetTimeBackend.Entities
{
    public class Article
    {
            public long Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public long CityId { get; set; }
            public City City { get; set; }
        
    }
}
