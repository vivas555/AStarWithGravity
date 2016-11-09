//Heap from the library Pathfinding With Gravity
//Copyright(C) Félix Rivard 2016

//Author: Felix Rivard 

//Contributors: Anthony Deschênes, Phillipe Tremblay, Alicia Lamontagne and Charles-Alexandre Lafond

//Year 2016

//This program is free software; you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation; either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software Foundation,
//Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA

using System;

namespace Core
{
    /// <summary>
    /// Structure de données qui va toujours retourner l'object ayant la plus petite valeur.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Heap<T> where T : IHeapItem<T>
    {
        private readonly T[] _items;

        /// <summary>
        /// Instancie une heap class.
        /// </summary>
        /// <param name="maxHeapSize">La taille maximale de la heap</param>
        public Heap(int maxHeapSize)
        {
            _items = new T[maxHeapSize];
        }

        /// <summary>
        /// Ajoute l'élément spécifié dans la heap et le classe automatiquement
        /// </summary>
        /// <param name="item">L'objet à ajouter</param>
        public void Add(T item)
        {
            item.HeapIndex = Count;
            _items[Count] = item;
            SortUp(item);
            Count++;
        }

        private void SortUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = _items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        /// <summary>
        /// Retourne le premeir élément dans la heap et le supprime
        /// </summary>
        /// <returns>LE premier élément dans la heap</returns>
        public T RemoveFirst()
        {
            T firstItem = _items[0];
            Count--;
            _items[0] = _items[Count];
            _items[0].HeapIndex = 0;
            SortDown(_items[0]);
            return firstItem;
        }

        /// <summary>
        /// Obtiens le nombre d'élément dans la heap
        /// </summary>
        /// <value>
        /// Lenombre d'éléments dans la heap
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Mets à jour l'objet spécifié dans la heap.
        /// </summary>
        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        /// <summary>
        /// Détermine si l'objet spécifié est dans la heap
        /// </summary>
        /// <param name="item">L'object spécifié</param>
        /// <returns>
        ///   <c>true</c> si la heap contiens l'object spécifié; sinon, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            return Equals(_items[item.HeapIndex], item);
        }

        /// <summary>
        /// Vide la heap de tous ses éléments, mais ne supprime pas la heap.
        /// </summary>
        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            Count = 0;
        }

        private void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < Count)
                {
                    swapIndex = childIndexLeft;
                    if (childIndexRight < Count)
                    {
                        if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(_items[swapIndex]) < 0)
                    {
                        Swap(item, _items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }

                else
                {
                    return;

                }

            }

        }

        private void Swap(T itemA, T itemB)
        {
            _items[itemA.HeapIndex] = itemB;
            _items[itemB.HeapIndex] = itemA;
            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }
}
