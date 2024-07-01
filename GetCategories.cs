public void PrintSubCategories(List subCategories,,,)
{
    var output = new StringBuilder();

    foreach (var subCategory in subCategories)
    {
        output.Append(new string(' ', level * 2) + $"SubCategory: {subCategory.Name}");
        PrintSubCategories(subCategory.SubCategories, level + 2);
    }

    Console.WriteLine(output.ToString());
}