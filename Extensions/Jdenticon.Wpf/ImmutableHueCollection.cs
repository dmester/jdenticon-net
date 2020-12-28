using Jdenticon.Rendering;
using Jdenticon.Wpf.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Jdenticon.Wpf
{
    [TypeConverter(typeof(ImmutableHueCollectionConverter))]
    public class ImmutableHueCollection : ICollection<float>
    {
        private static readonly string[] HueUnitSuffixes = new[]
        {
            "turn",
            "grad",
            "deg",
            "rad",
        };
        private static readonly float[] HueUnitPeriod = new[]
        {
            1f,
            400f,
            360f,
            (float)(2 * Math.PI),
        };

        private readonly IList<Hue> hues;

        class Hue
        {
            public float Value;
            public float Turns;
            public string Suffix;

            public static Hue FromTurns(float turns)
            {
                turns = turns % 1f;
                if (turns < 0)
                {
                    turns += 1f;
                }

                return new Hue
                {
                    Value = turns,
                    Turns = turns,
                    Suffix = "",
                };
            }

            public static Hue Parse(string value)
            {
                value = value.Trim();
                var suffix = "";
                var period = 1f;

                for (var unitIndex = 0; unitIndex < HueUnitSuffixes.Length; unitIndex++)
                {
                    var unitSuffix = HueUnitSuffixes[unitIndex];
                    if (value.EndsWith(unitSuffix))
                    {
                        suffix = HueUnitSuffixes[unitIndex];
                        period = HueUnitPeriod[unitIndex];
                        value = value.Substring(0, value.Length - unitSuffix.Length);
                        break;
                    }
                }

                var parsedValue = float.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);

                var normalizedValue = parsedValue % period;
                if (normalizedValue < 0)
                {
                    normalizedValue += period;
                }

                return new Hue
                {
                    Value = normalizedValue,
                    Turns = normalizedValue / period,
                    Suffix = suffix,
                };
            }

            public override string ToString()
            {
                return Value.ToString("0.##", CultureInfo.InvariantCulture) + Suffix;
            }
        }

        private ImmutableHueCollection()
        {
            this.hues = new List<Hue>();
        }

        public ImmutableHueCollection(IEnumerable<float> hues)
        {
            this.hues = hues
                .Select(hue => Hue.FromTurns(hue))
                .ToList();
        }

        /// <summary>
        /// Gets an empty hue collection.
        /// </summary>
        public static ImmutableHueCollection Empty { get; } = new ImmutableHueCollection(new float[0]);

        public float this[int index] => hues[index].Turns;

        /// <inheritdoc/>
        public int Count => hues.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => true;

        /// <inheritdoc/>
        void ICollection<float>.Add(float item)
        {
            throw new InvalidOperationException("This collection is read-only.");
        }

        /// <inheritdoc/>
        void ICollection<float>.Clear()
        {
            throw new InvalidOperationException("This collection is read-only.");
        }

        /// <inheritdoc/>
        public bool Contains(float item) => hues.Any(hue => item == hue.Turns);

        /// <inheritdoc/>
        public void CopyTo(float[] array, int arrayIndex)
        {
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (array.Length < arrayIndex + hues.Count)
            {
                throw new ArgumentException("Insufficient space in the specified array.", nameof(array));
            }

            for (var i = 0; i < hues.Count; i++)
            {
                array[arrayIndex + i] = hues[i].Turns;
            }
        }

        /// <inheritdoc/>
        public IEnumerator<float> GetEnumerator() => hues.Select(hue => hue.Turns).GetEnumerator();
        
        /// <inheritdoc/>
        bool ICollection<float>.Remove(float item)
        {
            throw new InvalidOperationException("This collection is read-only.");
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static ImmutableHueCollection Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Empty;
            }

            var hues = new ImmutableHueCollection();

            foreach (var hueString in value.Split(','))
            {
                hues.hues.Add(Hue.Parse(hueString));
            }

            return hues;
        }

        public override string ToString()
        {
            return string.Join(", ", hues);
        }
    }
}
