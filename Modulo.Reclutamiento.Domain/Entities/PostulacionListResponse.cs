namespace Modulo.Reclutamiento.Domain.Entities
{
    public class PostulacionListResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<PostulacionListItem> Items { get; set; } = new();
    }
}
