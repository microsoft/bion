// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Notification'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Notification : PropertyBagHolder, ISarifNode, IRow
    {
        private NotificationTable _table;
        private int _index;

        public Notification() : this(SarifLogDatabase.Current.Notification)
        { }

        public Notification(SarifLog root) : this(root.Database.Notification)
        { }

        internal Notification(NotificationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Notification(NotificationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Notification(
            IList<Location> locations,
            Message message,
            FailureLevel level,
            int threadId,
            DateTime timeUtc,
            ExceptionData exception,
            ReportingDescriptorReference descriptor,
            ReportingDescriptorReference associatedRule,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.Notification)
        {
            Locations = locations;
            Message = message;
            Level = level;
            ThreadId = threadId;
            TimeUtc = timeUtc;
            Exception = exception;
            Descriptor = descriptor;
            AssociatedRule = associatedRule;
            Properties = properties;
        }

        public Notification(Notification other) 
            : this(SarifLogDatabase.Current.Notification)
        {
            Locations = other.Locations;
            Message = other.Message;
            Level = other.Level;
            ThreadId = other.ThreadId;
            TimeUtc = other.TimeUtc;
            Exception = other.Exception;
            Descriptor = other.Descriptor;
            AssociatedRule = other.AssociatedRule;
            Properties = other.Properties;
        }

        [DataMember(Name = "locations", IsRequired = false, EmitDefaultValue = false)]
        public IList<Location> Locations
        {
            get => _table.Database.Location.List(_table.Locations[_index]);
            set => _table.Database.Location.List(_table.Locations[_index]).SetTo(value);
        }

        [DataMember(Name = "message", IsRequired = false, EmitDefaultValue = false)]
        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "level", IsRequired = false, EmitDefaultValue = false)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.EnumConverter))]
        public FailureLevel Level
        {
            get => (FailureLevel)_table.Level[_index];
            set => _table.Level[_index] = (int)value;
        }

        [DataMember(Name = "threadId", IsRequired = false, EmitDefaultValue = false)]
        public int ThreadId
        {
            get => _table.ThreadId[_index];
            set => _table.ThreadId[_index] = value;
        }

        [DataMember(Name = "timeUtc", IsRequired = false, EmitDefaultValue = false)]
        public DateTime TimeUtc
        {
            get => _table.TimeUtc[_index];
            set => _table.TimeUtc[_index] = value;
        }

        [DataMember(Name = "exception", IsRequired = false, EmitDefaultValue = false)]
        public ExceptionData Exception
        {
            get => _table.Database.ExceptionData.Get(_table.Exception[_index]);
            set => _table.Exception[_index] = _table.Database.ExceptionData.LocalIndex(value);
        }

        [DataMember(Name = "descriptor", IsRequired = false, EmitDefaultValue = false)]
        public ReportingDescriptorReference Descriptor
        {
            get => _table.Database.ReportingDescriptorReference.Get(_table.Descriptor[_index]);
            set => _table.Descriptor[_index] = _table.Database.ReportingDescriptorReference.LocalIndex(value);
        }

        [DataMember(Name = "associatedRule", IsRequired = false, EmitDefaultValue = false)]
        public ReportingDescriptorReference AssociatedRule
        {
            get => _table.Database.ReportingDescriptorReference.Get(_table.AssociatedRule[_index]);
            set => _table.AssociatedRule[_index] = _table.Database.ReportingDescriptorReference.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Notification>
        public bool Equals(Notification other)
        {
            if (other == null) { return false; }

            if (this.Locations != other.Locations) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.Level != other.Level) { return false; }
            if (this.ThreadId != other.ThreadId) { return false; }
            if (this.TimeUtc != other.TimeUtc) { return false; }
            if (this.Exception != other.Exception) { return false; }
            if (this.Descriptor != other.Descriptor) { return false; }
            if (this.AssociatedRule != other.AssociatedRule) { return false; }
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
                if (Locations != default(IList<Location>))
                {
                    result = (result * 31) + Locations.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (Level != default(FailureLevel))
                {
                    result = (result * 31) + Level.GetHashCode();
                }

                if (ThreadId != default(int))
                {
                    result = (result * 31) + ThreadId.GetHashCode();
                }

                if (TimeUtc != default(DateTime))
                {
                    result = (result * 31) + TimeUtc.GetHashCode();
                }

                if (Exception != default(ExceptionData))
                {
                    result = (result * 31) + Exception.GetHashCode();
                }

                if (Descriptor != default(ReportingDescriptorReference))
                {
                    result = (result * 31) + Descriptor.GetHashCode();
                }

                if (AssociatedRule != default(ReportingDescriptorReference))
                {
                    result = (result * 31) + AssociatedRule.GetHashCode();
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
            return Equals(obj as Notification);
        }

        public static bool operator ==(Notification left, Notification right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Notification left, Notification right)
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
            _table = (NotificationTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Notification;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Notification DeepClone()
        {
            return (Notification)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Notification(this);
        }
        #endregion

        public static IEqualityComparer<Notification> ValueComparer => EqualityComparer<Notification>.Default;
        public bool ValueEquals(Notification other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
