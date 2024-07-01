public void PrintSubCategories(List subCategories, int level, StringBuilder output)
{
    foreach (var subCategory in subCategories)
    {
        output.Append(new string(' ', level * 2));
        output.AppendLine($"SubCategory: {subCategory?.Name}");

        if (subCategory?.SubCategories != null)
        {
            PrintSubCategories(subCategory.SubCategories, level + 2, output);
        }
    }
}