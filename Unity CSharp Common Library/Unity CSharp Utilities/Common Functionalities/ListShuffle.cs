using System;
using System.Collections.Generic;

/// <summary>
/// A class that includes an extension method Shuffle() to shuffle a generic List with a given seed. To seed the class use SetSeed(int) method.
/// </summary>
public static class ListShuffle
{
    private static System.Random random;

    public static void SetSeed(int seed)
    {
        random = new System.Random(seed);
    }

    /// <summary>
    /// <para>Returns a shuffled version of given list. Original doesn't change.</para>
    /// <para>(This method clones a new list and does shuffling operation on the clone; then returns the clone.)</para>
    /// </summary>
    public static List<T> SafeShuffle<T> (this List<T> original)
    {
        if (random == null)
        {
            throw new NullReferenceException("The random class of ListShuffle class has no seed. Please set the seed with SetSeed(int) method before calling Shuffle() method");
        }

        List<T> newList = new List<T>(original);
        int newListCount = newList.Count;

        for (int i = 0; i < newListCount; i++)
        {
            int randomIndex = random.Next(newListCount);    //get "random index"

            var temp = newList[randomIndex];                //save "random index"
            newList[randomIndex] = newList[i];              //copy "i" to "random index"
            newList[i] = temp;                              //assign original "random index" to "i"
        }

        return newList;
    }

    /// <summary>
    /// <para>Shuffles the given list. Original changes.</para> 
    /// <para>(This method does shuffling operation on the original list.)</para>
    /// </summary>
    public static void Shuffle<T> (this List<T> original)
    {
        if (random == null)
        {
            throw new NullReferenceException("The random class of ListShuffle class has no seed. Please set the seed with SetSeed(int) method before calling Shuffle() method");
        }

        int count = original.Count;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = random.Next(count);   //get "random index"

            var temp = original[randomIndex];       //save "random index"
            original[randomIndex] = original[i];    //copy "i" to "random index"
            original[i] = temp;                     //assign original "random index" to "i"
        }
    }
}