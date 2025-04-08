﻿using Core.Entities;
using System.Text.Json;

namespace Infrastructure.Data;
public class StoredContextSeed
{
    public static async Task SeedAsync(StoredContext context)
    {
        if (!context.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            if(products is null)
                return;

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        if (!context.DeliveryMethods.Any())
        {
            var dmData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

            var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

            if (methods is null)
                return;

            context.DeliveryMethods.AddRange(methods);
            await context.SaveChangesAsync();
        }
    }
}
