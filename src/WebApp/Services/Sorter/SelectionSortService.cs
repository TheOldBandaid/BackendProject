namespace WebApp.Services.Sorter;

public class SelectionSortService
{
    public int[] Sort(int[] array)
    {
        var result = (int[])array.Clone();

        for (var i = 0; i < result.Length - 1; i++)
        {
            var minIndex = i;

            for (var j = i + 1; j < result.Length; j++)
            {
                if (result[j] < result[minIndex])
                    minIndex = j;
            }

            if (minIndex != i)
                (result[i], result[minIndex]) = (result[minIndex], result[i]);
        }

        return result;
    }
}