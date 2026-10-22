namespace Lab10v2;

/// <summary>
/// Сортування бульбашкою: O(n^2), з ранньою зупинкою, якщо обмінів не було.
/// </summary>
public class BubbleSort : ISortable
{
    public void Sort(int[] array)
    {
        ArgumentNullException.ThrowIfNull(array);

        for (int i = 0; i < array.Length - 1; i++)
        {
            bool swapped = false;

            for (int j = 0; j < array.Length - 1 - i; j++)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    swapped = true;
                }
            }

            if (!swapped)
            {
                break;
            }
        }
    }
}
