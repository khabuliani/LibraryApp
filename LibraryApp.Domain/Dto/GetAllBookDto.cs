namespace LibraryApp.Domain.Dto
{
    public class GetAllBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public List<GetAllAuthorDto> Authors { get; set; }
    }
}
