using System.ComponentModel.DataAnnotations;

namespace VNGAssignment.Models
{
    public class SearchBookRequest
    {
        public string SearchText { get; set; }
    }

    public class AddBookRequest
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string PublishedYear { get; set; }
    }

    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string PublishedYear { get; set; }
    }
}
