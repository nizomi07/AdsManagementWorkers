using AdsWithQuartz.DTOs;

namespace UnitTest.CategoryTests;

public class AddCategoryTests : CategoryServiceTestBase
{
    [Fact]
    public async Task AddCategoryAsync_ShouldAddCategory()
    {
        var service = CreateService();
        var dto = new CreateCategoryDto
        {
            Name = "Fantasy",
        };

        var result = await service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.NotEqual(0, result.Data.Id);
        Assert.Equal("Fantasy", result.Data.Name);
    }
}