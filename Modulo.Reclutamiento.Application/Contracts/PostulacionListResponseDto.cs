namespace Modulo.Reclutamiento.Application.Contracts
{
    public class PostulacionListResponseDto
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<PostulacionListItemDto> Items { get; set; } = new();
    }
}
