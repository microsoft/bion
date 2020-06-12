using System;

namespace BSOA.Extensions
{
    public static class ArrayExtensions
    {
        private const int MinimumSize = 32;

        public static void ResizeTo<T>(ref T[] array, int neededSize, T defaultValue = default(T), int minSize = MinimumSize)
        {
            int currentLength = (array?.Length ?? 0);
            
            // Allocate new array (at least 50% growth)
            int newLength = Math.Max(minSize, Math.Max(neededSize, (currentLength + currentLength / 2)));
            T[] newArray = new T[newLength];

            // Copy existing values (if any)
            if (currentLength > 0)
            {
                array.CopyTo(newArray, 0);
            }

            // Fill new space with desired default
            if (!defaultValue.Equals(default(T)))
            {
                for (int i = currentLength; i < newLength; ++i)
                {
                    newArray[i] = defaultValue;
                }
            }

            array = newArray;
        }
    }
}
