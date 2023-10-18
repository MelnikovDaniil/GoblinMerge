using System;
using System.Collections.Generic;
using System.Linq;

public static class CollectionExtension
{
    private static Random random = new Random();
    public static T GetRandom<T>(this IEnumerable<T> collection)
    {
        var randomElementNumber = random.Next(0, collection.Count());
        return collection.ElementAt(randomElementNumber);
    }

    public static T GetRandomOrDefault<T>(this IEnumerable<T> collection)
    {
        if (!collection.Any())
        {
            return default(T);
        }

        var randomElementNumber = random.Next(0, collection.Count());
        return collection.ElementAt(randomElementNumber);
    }
}

