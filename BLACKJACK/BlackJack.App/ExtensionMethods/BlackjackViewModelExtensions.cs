using BlackJack.App.Models;
using BlackJack.App.ViewModel;
using System.Runtime.CompilerServices;

namespace BlackJack.App.ExtensionMethods;

public static class BlackjackViewModelExtensions
{
    /// <summary>
    ///  Extension Method : Shuffles the items based on a clause 
    /// </summary>
    /// <typeparam name="T">Represents the Type (T) Of an item inside  a collection</typeparam>
    /// <typeparam name="T2">Represents the item based on how the List will be ordered</typeparam>
    /// <param name="items"> Represents the collection of items</param>
    /// <param name="Condition"> Based on clause</param>
    /// <returns></returns>
    public static IEnumerable<T> ShuffleBy<T, T2>(this IEnumerable<T> items, Func<T2> Condition)
    {
        ArgumentNullException.ThrowIfNull(items);


        return items.OrderBy(x => Condition());

    }


    public static bool SwitchValue(this bool value)
    {
        return value = !value;
    }
    public static bool SwitchValue(this bool value , bool toValue) 
    {
        return value = toValue;
    }


    /// <summary>
    ///  Extension Method : Shuffles the Items based on  Fisher–Yates Algorithm
    /// </summary>
    /// <typeparam name="T">Generic parameter represents the Type (T) of an item inside a Collection</typeparam>
    /// <param name="items"> Represents the entire collection</param>
    /// <returns></returns>
    public static IEnumerable<T> ShuffleData<T>(this IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        Random rnd = new Random();
        T[] array = items.ToArray();

        for (int i = array.Length - 1; i > 0; i--)

        {
            int j = rnd.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
        return array;

    }

    public static T GetOneCard<T>(this IList<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);


        T item = items[Random.Shared.Next(items.Count)];
        items.Remove(item);
        return item;
    }

    /// <summary>
    /// Calculates the sum of the cards for BlackJack Game
    /// </summary>
    /// <param name="items">Represents the list of cards </param>
    /// <returns></returns>
    public static int CalculateSum<TCard>(this IList<TCard> items,Predicate<int> indexLogic,Func<int, int, bool> aceCalculatorLogic)
        where TCard : class
    {
        ArgumentNullException.ThrowIfNull(items);

        int sum = 0;
        int aceCount = 0;

        foreach (var card in items)
        {
            int[] values = GetCardValues(card)
                ?? throw new ArgumentException($"Type {typeof(TCard)} does not contain a Value property");

            if (indexLogic(values.Length))
            {
                sum += values[0];
            }
            else
            {
                sum += 1;
                aceCount++;
            }
        }

        while (aceCalculatorLogic(aceCount, sum))
        {
            sum += 10;
            aceCount--;
        }

        return sum;
    }


   

    private static int[]? GetCardValues<TCard>(TCard card)
    {
        var prop = card?.GetType().GetProperty("Value");
        return prop?.GetValue(card) as int[];
    }

}
