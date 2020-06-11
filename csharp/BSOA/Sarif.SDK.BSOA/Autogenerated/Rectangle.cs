// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Rectangle'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Rectangle : PropertyBagHolder, ISarifNode, IRow
    {
        private RectangleTable _table;
        private int _index;

        public Rectangle() : this(SarifLogDatabase.Current.Rectangle)
        { }

        public Rectangle(SarifLog root) : this(root.Database.Rectangle)
        { }

        internal Rectangle(RectangleTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Rectangle(RectangleTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Rectangle(
            double top,
            double left,
            double bottom,
            double right,
            Message message,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.Rectangle)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
            Message = message;
            Properties = properties;
        }

        public Rectangle(Rectangle other) 
            : this(SarifLogDatabase.Current.Rectangle)
        {
            Top = other.Top;
            Left = other.Left;
            Bottom = other.Bottom;
            Right = other.Right;
            Message = other.Message;
            Properties = other.Properties;
        }

        [DataMember(Name = "top", IsRequired = false, EmitDefaultValue = false)]
        public double Top
        {
            get => _table.Top[_index];
            set => _table.Top[_index] = value;
        }

        [DataMember(Name = "left", IsRequired = false, EmitDefaultValue = false)]
        public double Left
        {
            get => _table.Left[_index];
            set => _table.Left[_index] = value;
        }

        [DataMember(Name = "bottom", IsRequired = false, EmitDefaultValue = false)]
        public double Bottom
        {
            get => _table.Bottom[_index];
            set => _table.Bottom[_index] = value;
        }

        [DataMember(Name = "right", IsRequired = false, EmitDefaultValue = false)]
        public double Right
        {
            get => _table.Right[_index];
            set => _table.Right[_index] = value;
        }

        [DataMember(Name = "message", IsRequired = false, EmitDefaultValue = false)]
        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Rectangle>
        public bool Equals(Rectangle other)
        {
            if (other == null) { return false; }

            if (this.Top != other.Top) { return false; }
            if (this.Left != other.Left) { return false; }
            if (this.Bottom != other.Bottom) { return false; }
            if (this.Right != other.Right) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.Properties != other.Properties) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Top != default(double))
                {
                    result = (result * 31) + Top.GetHashCode();
                }

                if (Left != default(double))
                {
                    result = (result * 31) + Left.GetHashCode();
                }

                if (Bottom != default(double))
                {
                    result = (result * 31) + Bottom.GetHashCode();
                }

                if (Right != default(double))
                {
                    result = (result * 31) + Right.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (Properties != default(IDictionary<string, string>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rectangle);
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
        #endregion

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (RectangleTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Rectangle;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Rectangle DeepClone()
        {
            return (Rectangle)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Rectangle(this);
        }
        #endregion

        public static IEqualityComparer<Rectangle> ValueComparer => EqualityComparer<Rectangle>.Default;
        public bool ValueEquals(Rectangle other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
