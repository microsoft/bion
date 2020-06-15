// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Collections;
using BSOA.IO;
using BSOA.Model;

namespace BSOA.Column
{
    /// <summary>
    ///  BooleanColumn implements IColumn for bool on top of a NumberColumn&lt;uint&gt;
    /// </summary>
    public class BooleanColumn : LimitedList<bool>, IColumn<bool>
    {
        private BitVector _vector;

        public BooleanColumn(bool defaultValue)
        {
            _vector = new BitVector(defaultValue, 0);
        }

        public override int Count => _vector.Capacity;

        public override bool this[int index]
        {
            // Check or set the bit in the right uint (each one holds 32 bits)
            get { return _vector[index]; }
            set { _vector[index] = value; }
        }

        // Exposed BitVector members
        public int CountTrue => _vector.Count;
        public void SetAll(bool value) => _vector.SetAll(value);

        public void Trim()
        {
            // Nothing to do
        }

        public override void Clear()
        {
            _vector.Clear();
        }

        public override void RemoveFromEnd(int count)
        {
            _vector.RemoveFromEnd(count);
        }

        public void Read(ITreeReader reader)
        {
            _vector.Read(reader);
        }

        public void Write(ITreeWriter writer)
        {
            _vector.Write(writer);
        }
    }
}
