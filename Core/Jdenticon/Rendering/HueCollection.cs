using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// A collection of hue values.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The collection internally operates on hues in turns in the range [0, 1). It will
    /// accept insertion and queries in multiple units, but they will always be converted 
    /// to turns, and then normalized to the first turn in the range [0, 1).
    /// </para>
    /// <para>
    /// Queries will always return the internal hue specified in turns.
    /// </para>
    /// </remarks>
    public class HueCollection : ICollection<float>, IEquatable<HueCollection>
    {
        private List<float> hues;

        /// <summary>
        /// Creates an empty <see cref="HueCollection"/>.
        /// </summary>
        public HueCollection()
        {
            hues = new List<float>();
        }

        /// <summary>
        /// Creates a new <see cref="HueCollection"/> containing the specified hue
        /// values specified in turns.
        /// </summary>
        /// <param name="hues">Enumerable of hues to be added to the new collection. 
        /// Hues should be specified in turns and will be normalized to the range [0, 1).</param>
        /// <exception cref="ArgumentNullException"><paramref name="hues"/> was <c>Nothing</c>.</exception>
        public HueCollection(IEnumerable<float> hues)
        {
            if (hues == null) throw new ArgumentNullException(nameof(hues));
            this.hues = new List<float>();
            
            foreach (var hue in hues)
            {
                this.hues.Add(Normalize(hue, HueUnit.Turns));
            }
        }
        
        /// <summary>
        /// Gets the number of hues in this collection.
        /// </summary>
        public int Count => hues.Count;

        /// <summary>
        /// Gets a value indicating whether this is a read-only collection.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets the hue (in turns, range [0, 1)) at the specified index in this collection.
        /// </summary>
        /// <param name="index">Hue index.</param>
        public float this[int index] => hues[index];

        private static float Normalize(float hue, HueUnit hueUnit)
        {
            // Convert to turns
            switch (hueUnit)
            {
                case HueUnit.Degrees:
                    hue /= 360;
                    break;

                case HueUnit.Gradians:
                    hue /= 400;
                    break;

                case HueUnit.Radians:
                    hue /= 2 * (float)Math.PI;
                    break;
            }

            // Normalize hue to range [0, 1)
            hue = hue % 1;

            if (hue < 0)
            {
                hue += 1;
            }

            return hue;
        }

        /// <summary>
        /// Adds a hue to the collection.
        /// </summary>
        /// <param name="hue">Hue specified in turns. The hue will be normalized to the range [0, 1) turns.</param>
        /// <exception cref="ArgumentException"><paramref name="hue"/> was NaN or infinite.</exception>
        public void Add(float hue)
        {
            if (float.IsNaN(hue)) throw new ArgumentException("NaN is not a valid hue.", nameof(hue));
            if (float.IsPositiveInfinity(hue)) throw new ArgumentException("Positive infinity is not a valid hue.", nameof(hue));
            if (float.IsNegativeInfinity(hue)) throw new ArgumentException("Negative infinity is not a valid hue.", nameof(hue));

            hues.Add(Normalize(hue, HueUnit.Turns));
        }

        /// <summary>
        /// Adds a hue to the collection.
        /// </summary>
        /// <param name="hue">Hue in a specified unit. The hue will be converted and normalized to turns in the range [0, 1).</param>
        /// <param name="hueUnit">The unit of <paramref name="hue"/>.</param>
        public void Add(float hue, HueUnit hueUnit)
        {
            if (float.IsNaN(hue)) throw new ArgumentException("NaN is not a valid hue.", nameof(hue));
            if (float.IsPositiveInfinity(hue)) throw new ArgumentException("Positive infinity is not a valid hue.", nameof(hue));
            if (float.IsNegativeInfinity(hue)) throw new ArgumentException("Negative infinity is not a valid hue.", nameof(hue));

            hues.Add(Normalize(hue, hueUnit));
        }

        /// <summary>
        /// Empties this collection.
        /// </summary>
        public void Clear()
        {
            hues.Clear();
        }

        /// <summary>
        /// Checks whether a specified hue exists in this collection.
        /// </summary>
        /// <param name="hue">Hue in turns to find in the collection.</param>
        /// <returns><c>true</c> if found, otherwise <c>false</c>.</returns>
        /// <remarks>
        /// The specified hue will be normalized to turns in the range [0, 1). This means that if 
        /// you have added the hue 180 degrees to the collection and then asks this method if the hue
        /// 1.5 turns is existing in the collection, it will return <c>true</c>.
        /// </remarks>
        public bool Contains(float hue)
        {
            return hues.Contains(Normalize(hue, HueUnit.Turns));
        }

        /// <summary>
        /// Checks whether a specified hue exists in this collection.
        /// </summary>
        /// <param name="hue">Hue to find in the collection.</param>
        /// <param name="hueUnit">Unit of <paramref name="hueUnit"/>.</param>
        /// <returns><c>true</c> if found, otherwise <c>false</c>.</returns>
        /// <remarks>
        /// The specified hue will be normalized to turns in the range [0, 1). This means that if 
        /// you have added the hue 180 degrees to the collection and then asks this method if the hue
        /// 1.5 turns is existing in the collection, it will return <c>true</c>.
        /// </remarks>
        public bool Contains(float hue, HueUnit hueUnit)
        {
            return hues.Contains(Normalize(hue, hueUnit));
        }

        /// <summary>
        /// Copies the hues in this collection to the specified array.
        /// </summary>
        /// <param name="array">Array to copy the hues to.</param>
        /// <param name="arrayIndex">The index in <paramref name="array"/> to which the first hue will be copied.</param>
        public void CopyTo(float[] array, int arrayIndex)
        {
            hues.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets an enumerator for enumerating over the hues (in turns, range [0, 1)) in this collection.
        /// </summary>
        public IEnumerator<float> GetEnumerator()
        {
            return hues.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified hue from this collection.
        /// </summary>
        /// <param name="hue">Hue in turns. Will be normalized to turns in the range [0, 1).</param>
        /// <returns><c>true</c> if the hue was found and removed from the collection.</returns>
        public bool Remove(float hue)
        {
            return hues.Remove(Normalize(hue, HueUnit.Turns));
        }

        /// <summary>
        /// Removes the specified hue from this collection.
        /// </summary>
        /// <param name="hue">Hue. Will be normalized to turns in the range [0, 1).</param>
        /// <param name="hueUnit">The unit of <paramref name="hue"/>.</param>
        /// <returns><c>true</c> if the hue was found and removed from the collection.</returns>
        public bool Remove(float hue, HueUnit hueUnit)
        {
            return hues.Remove(Normalize(hue, hueUnit));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return hues.GetEnumerator();
        }

        /// <summary>
        /// Gets a hash code for this <see cref="HueCollection"/>.
        /// </summary>
        public override int GetHashCode()
        {
            return hues.GetHashCode();
        }

        /// <summary>
        /// Checks if this style is identical to another object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as HueCollection);
        }

        /// <summary>
        /// Checks whether this <see cref="HueCollection"/> contains the same
        /// hues and the same order as <paramref name="other"/>. 
        /// </summary>
        /// <param name="other">Collection to compare with.</param>
        public bool Equals(HueCollection other)
        {
            if (other == null ||
                other.hues.Count != hues.Count)
            {
                return false;
            }

            for (var i = 0; i < hues.Count; i++)
            {
                if (hues[i] != other.hues[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
