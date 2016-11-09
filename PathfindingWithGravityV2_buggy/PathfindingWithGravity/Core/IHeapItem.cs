//IHeapItem from the library Pathfinding With Gravity
//Copyright(C) Félix Rivard 2016

//Author: Anthony Deschênes

//Contributors: Félix Rivard, Phillipe Tremblay, Alicia Lamontagne and Charles-Alexandre Lafond

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
    /// Interface que tous les objets voulant être placés dans la heap doivent implanter.
    /// </summary>
    /// <typeparam name="T">Le type d'objet</typeparam>
    /// <seealso cref="System.IComparable{T}" />
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get; set;
        }
    }
}
