using System;

public static class Heap<T> where T : IComparable<T>
{
    public static void Sort(T[] arr)
    {
        int length = arr.Length;

        for (int i = length / 2; i >= 0; i--)
        {
            HeapifyDown(arr, i, length);
        }

        for (int i = length - 1; i > 0; i--)
        {
            Swap(arr, 0, i);
            HeapifyDown(arr, 0, i);
        }
    }

    private static void HeapifyDown(T[] arr, int current, int border)
    {
        while (current < border / 2)
        {
            int greaterChild = Child(arr, current, border);
            if (IsGreater(arr, current, greaterChild))
            {
                break;
            }
            Swap(arr, current, greaterChild);
            current = greaterChild;
        }
    }

    private static void Swap(T[] arr, int current, int greaterChild)
    {
        T oldElement = arr[current];
        arr[current] = arr[greaterChild];
        arr[greaterChild] = oldElement;
    }

    private static int Child(T[] arr, int current, int border)
    {
        int leftIndex = current * 2 + 1;
        int childIndex = leftIndex;
        if (leftIndex + 1 < border)
        {
            int rightIndex = current * 2 + 2;
            if (IsLess(arr,leftIndex,rightIndex))
            {
                childIndex = rightIndex;
            }
        }
        return childIndex;
    }

    private static bool IsGreater(T[] arr, int leftIndex, int rightIndex)
    {
        return arr[leftIndex].CompareTo(arr[rightIndex]) > 0;
    }

    private static bool IsLess(T[] arr, int leftIndex, int rightIndex)
    {
        return arr[leftIndex].CompareTo(arr[rightIndex]) < 0;
    }
}
