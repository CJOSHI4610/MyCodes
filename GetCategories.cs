  foreach (var subCategory in subCategories)
            {
                Console.WriteLine(new string(' ', level * 2) + $"SubCategory: {subCategory.Name}");
                PrintSubCategories(subCategory.SubCategories, level + 1);
string cj="hello";
            }