namespace Lab10v2;

/// <summary>
/// Швидке сортування (схема Ломуто): у середньому O(n log n).
/// </summary>
public class QuickSort : ISortable
{
    public void Sort(int[] array)
    {
        ArgumentNullException.ThrowIfNull(array);
        Sort(array, 0, array.Length - 1);
    }

    private static void Sort(int[] array, int low, int high)
    {
        if (low >= high)
        {
            return;
        }

        int pivotIndex = Partition(array, low, high);
        Sort(array, low, pivotIndex - 1);
        Sort(array, pivotIndex + 1, high);
    }

    private static int Partition(int[] array, int low, int high)
    {
        int pivot = array[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (array[j] <= pivot)
            {
                i++;
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        (array[i + 1], array[high]) = (array[high], array[i + 1]);
        return i + 1;
    }
}
