namespace Dashboard.BussinessLogic.Dtos.ProductDtos;

public class CategoryDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModified { get; set; }
}

public class GetCategoriesInput : DefaultInput
{
    public long? Id { get; set; }
    public string? Name { get; set; }
}
public class UpdateCategoryInput 
{
    public long? id;
    public string? Name { get; set; } = string.Empty;

}
public class CreateCategoryInput
{
    public string? Name { get; set; } = string.Empty;
}

