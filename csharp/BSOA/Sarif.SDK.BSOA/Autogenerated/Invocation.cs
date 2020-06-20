// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Invocation'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Invocation : PropertyBagHolder, ISarifNode, IRow
    {
        private InvocationTable _table;
        private int _index;

        public Invocation() : this(SarifLogDatabase.Current.Invocation)
        { }

        public Invocation(SarifLog root) : this(root.Database.Invocation)
        { }

        internal Invocation(InvocationTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal Invocation(InvocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Invocation(
            string commandLine,
            IList<string> arguments,
            IList<ArtifactLocation> responseFiles,
            DateTime startTimeUtc,
            DateTime endTimeUtc,
            int exitCode,
            IList<ConfigurationOverride> ruleConfigurationOverrides,
            IList<ConfigurationOverride> notificationConfigurationOverrides,
            IList<Notification> toolExecutionNotifications,
            IList<Notification> toolConfigurationNotifications,
            string exitCodeDescription,
            string exitSignalName,
            int exitSignalNumber,
            string processStartFailureMessage,
            bool executionSuccessful,
            string machine,
            string account,
            int processId,
            ArtifactLocation executableLocation,
            ArtifactLocation workingDirectory,
            IDictionary<string, string> environmentVariables,
            ArtifactLocation stdin,
            ArtifactLocation stdout,
            ArtifactLocation stderr,
            ArtifactLocation stdoutStderr,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Invocation)
        {
            CommandLine = commandLine;
            Arguments = arguments;
            ResponseFiles = responseFiles;
            StartTimeUtc = startTimeUtc;
            EndTimeUtc = endTimeUtc;
            ExitCode = exitCode;
            RuleConfigurationOverrides = ruleConfigurationOverrides;
            NotificationConfigurationOverrides = notificationConfigurationOverrides;
            ToolExecutionNotifications = toolExecutionNotifications;
            ToolConfigurationNotifications = toolConfigurationNotifications;
            ExitCodeDescription = exitCodeDescription;
            ExitSignalName = exitSignalName;
            ExitSignalNumber = exitSignalNumber;
            ProcessStartFailureMessage = processStartFailureMessage;
            ExecutionSuccessful = executionSuccessful;
            Machine = machine;
            Account = account;
            ProcessId = processId;
            ExecutableLocation = executableLocation;
            WorkingDirectory = workingDirectory;
            EnvironmentVariables = environmentVariables;
            Stdin = stdin;
            Stdout = stdout;
            Stderr = stderr;
            StdoutStderr = stdoutStderr;
            Properties = properties;
        }

        public Invocation(Invocation other) 
            : this(SarifLogDatabase.Current.Invocation)
        {
            CommandLine = other.CommandLine;
            Arguments = other.Arguments;
            ResponseFiles = other.ResponseFiles;
            StartTimeUtc = other.StartTimeUtc;
            EndTimeUtc = other.EndTimeUtc;
            ExitCode = other.ExitCode;
            RuleConfigurationOverrides = other.RuleConfigurationOverrides;
            NotificationConfigurationOverrides = other.NotificationConfigurationOverrides;
            ToolExecutionNotifications = other.ToolExecutionNotifications;
            ToolConfigurationNotifications = other.ToolConfigurationNotifications;
            ExitCodeDescription = other.ExitCodeDescription;
            ExitSignalName = other.ExitSignalName;
            ExitSignalNumber = other.ExitSignalNumber;
            ProcessStartFailureMessage = other.ProcessStartFailureMessage;
            ExecutionSuccessful = other.ExecutionSuccessful;
            Machine = other.Machine;
            Account = other.Account;
            ProcessId = other.ProcessId;
            ExecutableLocation = other.ExecutableLocation;
            WorkingDirectory = other.WorkingDirectory;
            EnvironmentVariables = other.EnvironmentVariables;
            Stdin = other.Stdin;
            Stdout = other.Stdout;
            Stderr = other.Stderr;
            StdoutStderr = other.StdoutStderr;
            Properties = other.Properties;
        }

        partial void Init();

        public string CommandLine
        {
            get => _table.CommandLine[_index];
            set => _table.CommandLine[_index] = value;
        }

        public IList<string> Arguments
        {
            get => _table.Arguments[_index];
            set => _table.Arguments[_index] = value;
        }

        public IList<ArtifactLocation> ResponseFiles
        {
            get => _table.Database.ArtifactLocation.List(_table.ResponseFiles[_index]);
            set => _table.Database.ArtifactLocation.List(_table.ResponseFiles[_index]).SetTo(value);
        }

        public DateTime StartTimeUtc
        {
            get => _table.StartTimeUtc[_index];
            set => _table.StartTimeUtc[_index] = value;
        }

        public DateTime EndTimeUtc
        {
            get => _table.EndTimeUtc[_index];
            set => _table.EndTimeUtc[_index] = value;
        }

        public int ExitCode
        {
            get => _table.ExitCode[_index];
            set => _table.ExitCode[_index] = value;
        }

        public IList<ConfigurationOverride> RuleConfigurationOverrides
        {
            get => _table.Database.ConfigurationOverride.List(_table.RuleConfigurationOverrides[_index]);
            set => _table.Database.ConfigurationOverride.List(_table.RuleConfigurationOverrides[_index]).SetTo(value);
        }

        public IList<ConfigurationOverride> NotificationConfigurationOverrides
        {
            get => _table.Database.ConfigurationOverride.List(_table.NotificationConfigurationOverrides[_index]);
            set => _table.Database.ConfigurationOverride.List(_table.NotificationConfigurationOverrides[_index]).SetTo(value);
        }

        public IList<Notification> ToolExecutionNotifications
        {
            get => _table.Database.Notification.List(_table.ToolExecutionNotifications[_index]);
            set => _table.Database.Notification.List(_table.ToolExecutionNotifications[_index]).SetTo(value);
        }

        public IList<Notification> ToolConfigurationNotifications
        {
            get => _table.Database.Notification.List(_table.ToolConfigurationNotifications[_index]);
            set => _table.Database.Notification.List(_table.ToolConfigurationNotifications[_index]).SetTo(value);
        }

        public string ExitCodeDescription
        {
            get => _table.ExitCodeDescription[_index];
            set => _table.ExitCodeDescription[_index] = value;
        }

        public string ExitSignalName
        {
            get => _table.ExitSignalName[_index];
            set => _table.ExitSignalName[_index] = value;
        }

        public int ExitSignalNumber
        {
            get => _table.ExitSignalNumber[_index];
            set => _table.ExitSignalNumber[_index] = value;
        }

        public string ProcessStartFailureMessage
        {
            get => _table.ProcessStartFailureMessage[_index];
            set => _table.ProcessStartFailureMessage[_index] = value;
        }

        public bool ExecutionSuccessful
        {
            get => _table.ExecutionSuccessful[_index];
            set => _table.ExecutionSuccessful[_index] = value;
        }

        public string Machine
        {
            get => _table.Machine[_index];
            set => _table.Machine[_index] = value;
        }

        public string Account
        {
            get => _table.Account[_index];
            set => _table.Account[_index] = value;
        }

        public int ProcessId
        {
            get => _table.ProcessId[_index];
            set => _table.ProcessId[_index] = value;
        }

        public ArtifactLocation ExecutableLocation
        {
            get => _table.Database.ArtifactLocation.Get(_table.ExecutableLocation[_index]);
            set => _table.ExecutableLocation[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public ArtifactLocation WorkingDirectory
        {
            get => _table.Database.ArtifactLocation.Get(_table.WorkingDirectory[_index]);
            set => _table.WorkingDirectory[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public IDictionary<string, string> EnvironmentVariables
        {
            get => _table.EnvironmentVariables[_index];
            set => _table.EnvironmentVariables[_index] = value;
        }

        public ArtifactLocation Stdin
        {
            get => _table.Database.ArtifactLocation.Get(_table.Stdin[_index]);
            set => _table.Stdin[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public ArtifactLocation Stdout
        {
            get => _table.Database.ArtifactLocation.Get(_table.Stdout[_index]);
            set => _table.Stdout[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public ArtifactLocation Stderr
        {
            get => _table.Database.ArtifactLocation.Get(_table.Stderr[_index]);
            set => _table.Stderr[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public ArtifactLocation StdoutStderr
        {
            get => _table.Database.ArtifactLocation.Get(_table.StdoutStderr[_index]);
            set => _table.StdoutStderr[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Invocation>
        public bool Equals(Invocation other)
        {
            if (other == null) { return false; }

            if (this.CommandLine != other.CommandLine) { return false; }
            if (this.Arguments != other.Arguments) { return false; }
            if (this.ResponseFiles != other.ResponseFiles) { return false; }
            if (this.StartTimeUtc != other.StartTimeUtc) { return false; }
            if (this.EndTimeUtc != other.EndTimeUtc) { return false; }
            if (this.ExitCode != other.ExitCode) { return false; }
            if (this.RuleConfigurationOverrides != other.RuleConfigurationOverrides) { return false; }
            if (this.NotificationConfigurationOverrides != other.NotificationConfigurationOverrides) { return false; }
            if (this.ToolExecutionNotifications != other.ToolExecutionNotifications) { return false; }
            if (this.ToolConfigurationNotifications != other.ToolConfigurationNotifications) { return false; }
            if (this.ExitCodeDescription != other.ExitCodeDescription) { return false; }
            if (this.ExitSignalName != other.ExitSignalName) { return false; }
            if (this.ExitSignalNumber != other.ExitSignalNumber) { return false; }
            if (this.ProcessStartFailureMessage != other.ProcessStartFailureMessage) { return false; }
            if (this.ExecutionSuccessful != other.ExecutionSuccessful) { return false; }
            if (this.Machine != other.Machine) { return false; }
            if (this.Account != other.Account) { return false; }
            if (this.ProcessId != other.ProcessId) { return false; }
            if (this.ExecutableLocation != other.ExecutableLocation) { return false; }
            if (this.WorkingDirectory != other.WorkingDirectory) { return false; }
            if (this.EnvironmentVariables != other.EnvironmentVariables) { return false; }
            if (this.Stdin != other.Stdin) { return false; }
            if (this.Stdout != other.Stdout) { return false; }
            if (this.Stderr != other.Stderr) { return false; }
            if (this.StdoutStderr != other.StdoutStderr) { return false; }
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
                if (CommandLine != default(string))
                {
                    result = (result * 31) + CommandLine.GetHashCode();
                }

                if (Arguments != default(IList<string>))
                {
                    result = (result * 31) + Arguments.GetHashCode();
                }

                if (ResponseFiles != default(IList<ArtifactLocation>))
                {
                    result = (result * 31) + ResponseFiles.GetHashCode();
                }

                if (StartTimeUtc != default(DateTime))
                {
                    result = (result * 31) + StartTimeUtc.GetHashCode();
                }

                if (EndTimeUtc != default(DateTime))
                {
                    result = (result * 31) + EndTimeUtc.GetHashCode();
                }

                if (ExitCode != default(int))
                {
                    result = (result * 31) + ExitCode.GetHashCode();
                }

                if (RuleConfigurationOverrides != default(IList<ConfigurationOverride>))
                {
                    result = (result * 31) + RuleConfigurationOverrides.GetHashCode();
                }

                if (NotificationConfigurationOverrides != default(IList<ConfigurationOverride>))
                {
                    result = (result * 31) + NotificationConfigurationOverrides.GetHashCode();
                }

                if (ToolExecutionNotifications != default(IList<Notification>))
                {
                    result = (result * 31) + ToolExecutionNotifications.GetHashCode();
                }

                if (ToolConfigurationNotifications != default(IList<Notification>))
                {
                    result = (result * 31) + ToolConfigurationNotifications.GetHashCode();
                }

                if (ExitCodeDescription != default(string))
                {
                    result = (result * 31) + ExitCodeDescription.GetHashCode();
                }

                if (ExitSignalName != default(string))
                {
                    result = (result * 31) + ExitSignalName.GetHashCode();
                }

                if (ExitSignalNumber != default(int))
                {
                    result = (result * 31) + ExitSignalNumber.GetHashCode();
                }

                if (ProcessStartFailureMessage != default(string))
                {
                    result = (result * 31) + ProcessStartFailureMessage.GetHashCode();
                }

                if (ExecutionSuccessful != default(bool))
                {
                    result = (result * 31) + ExecutionSuccessful.GetHashCode();
                }

                if (Machine != default(string))
                {
                    result = (result * 31) + Machine.GetHashCode();
                }

                if (Account != default(string))
                {
                    result = (result * 31) + Account.GetHashCode();
                }

                if (ProcessId != default(int))
                {
                    result = (result * 31) + ProcessId.GetHashCode();
                }

                if (ExecutableLocation != default(ArtifactLocation))
                {
                    result = (result * 31) + ExecutableLocation.GetHashCode();
                }

                if (WorkingDirectory != default(ArtifactLocation))
                {
                    result = (result * 31) + WorkingDirectory.GetHashCode();
                }

                if (EnvironmentVariables != default(IDictionary<string, string>))
                {
                    result = (result * 31) + EnvironmentVariables.GetHashCode();
                }

                if (Stdin != default(ArtifactLocation))
                {
                    result = (result * 31) + Stdin.GetHashCode();
                }

                if (Stdout != default(ArtifactLocation))
                {
                    result = (result * 31) + Stdout.GetHashCode();
                }

                if (Stderr != default(ArtifactLocation))
                {
                    result = (result * 31) + Stderr.GetHashCode();
                }

                if (StdoutStderr != default(ArtifactLocation))
                {
                    result = (result * 31) + StdoutStderr.GetHashCode();
                }

                if (Properties != default(IDictionary<string, SerializedPropertyInfo>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Invocation);
        }

        public static bool operator ==(Invocation left, Invocation right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Invocation left, Invocation right)
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

        void IRow.Next()
        {
            _index++;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Invocation;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Invocation DeepClone()
        {
            return (Invocation)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Invocation(this);
        }
        #endregion

        public static IEqualityComparer<Invocation> ValueComparer => EqualityComparer<Invocation>.Default;
        public bool ValueEquals(Invocation other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
