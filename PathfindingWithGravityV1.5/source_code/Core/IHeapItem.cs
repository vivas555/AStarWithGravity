﻿using System;

namespace Core
{
    /// <summary>
    /// interface que tous les objects voulant être placé dans la heap doivent implanté.
    /// </summary>
    /// <typeparam name="T">Le type d'objet</typeparam>
    /// <seealso cref="System.IComparable{T}" />
    public interface IHeapItem<T> : IComparable<T>
    {
        /// <summary>
        /// Gets ou sets la valeur de Heapindex
        /// </summary>
        int HeapIndex
        {
            get; set;
        }
    }
}
