namespace DoAnWebNangCao.Data
{
    public class ProductFilterViewModel
    {
        public List<AuthorCheckboxViewModel> Authors { get; set; }
        public List<GenreCheckboxViewModel> Genres { get; set; }
        public List<int> SelectedAuthorIds { get; set; }
        public List<int> SelectedGenreIds { get; set; }
        public string PriceSort { get; set; }
    }

    public class AuthorCheckboxViewModel
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class GenreCheckboxViewModel
    {
        public int Id { get; set; }
        public string GenreName { get; set; }
        public bool IsSelected { get; set; }
    }

}
