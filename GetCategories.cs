```csharp
public void PrintSubCategories(List subCategories, int level)
{
    StringBuilder output = new StringBuilder();

    foreach (var subCategory in subCategories)
    {
        output.AppendLine(new string(' ', level * 2) + $"SubCategory: {subCategory.Name}");
        PrintSubCategories(subCategory.SubCategories, level + 2);
    }

    Console.WriteLine(output.ToString());
}