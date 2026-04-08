namespace GoVibe.API.Models.Categories
{
    public class CategoryModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime UpdatedAt { get; set; }
    }

    public class AddCategoryRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public class UpdateCategoryRequest
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }

    public class DeleteManyCategoriesRequest
    {
        public List<string> Ids { get; set; } = [];
    }

    public class CategoryStats
    {
        public string Id { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public int PlaceCount { get; set; }
    }
}
