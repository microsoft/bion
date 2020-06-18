// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Invocation'
    /// </summary>
    internal partial class InvocationTable : Table<Invocation>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> CommandLine;
        internal IColumn<IList<string>> Arguments;
        internal RefListColumn ResponseFiles;
        internal IColumn<DateTime> StartTimeUtc;
        internal IColumn<DateTime> EndTimeUtc;
        internal IColumn<int> ExitCode;
        internal RefListColumn RuleConfigurationOverrides;
        internal RefListColumn NotificationConfigurationOverrides;
        internal RefListColumn ToolExecutionNotifications;
        internal RefListColumn ToolConfigurationNotifications;
        internal IColumn<string> ExitCodeDescription;
        internal IColumn<string> ExitSignalName;
        internal IColumn<int> ExitSignalNumber;
        internal IColumn<string> ProcessStartFailureMessage;
        internal IColumn<bool> ExecutionSuccessful;
        internal IColumn<string> Machine;
        internal IColumn<string> Account;
        internal IColumn<int> ProcessId;
        internal RefColumn ExecutableLocation;
        internal RefColumn WorkingDirectory;
        internal IColumn<IDictionary<string, string>> EnvironmentVariables;
        internal RefColumn Stdin;
        internal RefColumn Stdout;
        internal RefColumn Stderr;
        internal RefColumn StdoutStderr;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal InvocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            CommandLine = AddColumn(nameof(CommandLine), ColumnFactory.Build<string>(default));
            Arguments = AddColumn(nameof(Arguments), ColumnFactory.Build<IList<string>>(default));
            ResponseFiles = AddColumn(nameof(ResponseFiles), new RefListColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            StartTimeUtc = AddColumn(nameof(StartTimeUtc), ColumnFactory.Build<DateTime>(default));
            EndTimeUtc = AddColumn(nameof(EndTimeUtc), ColumnFactory.Build<DateTime>(default));
            ExitCode = AddColumn(nameof(ExitCode), ColumnFactory.Build<int>(default));
            RuleConfigurationOverrides = AddColumn(nameof(RuleConfigurationOverrides), new RefListColumn(nameof(SarifLogDatabase.ConfigurationOverride)));
            NotificationConfigurationOverrides = AddColumn(nameof(NotificationConfigurationOverrides), new RefListColumn(nameof(SarifLogDatabase.ConfigurationOverride)));
            ToolExecutionNotifications = AddColumn(nameof(ToolExecutionNotifications), new RefListColumn(nameof(SarifLogDatabase.Notification)));
            ToolConfigurationNotifications = AddColumn(nameof(ToolConfigurationNotifications), new RefListColumn(nameof(SarifLogDatabase.Notification)));
            ExitCodeDescription = AddColumn(nameof(ExitCodeDescription), ColumnFactory.Build<string>(default));
            ExitSignalName = AddColumn(nameof(ExitSignalName), ColumnFactory.Build<string>(default));
            ExitSignalNumber = AddColumn(nameof(ExitSignalNumber), ColumnFactory.Build<int>(default));
            ProcessStartFailureMessage = AddColumn(nameof(ProcessStartFailureMessage), ColumnFactory.Build<string>(default));
            ExecutionSuccessful = AddColumn(nameof(ExecutionSuccessful), ColumnFactory.Build<bool>(default));
            Machine = AddColumn(nameof(Machine), ColumnFactory.Build<string>(default));
            Account = AddColumn(nameof(Account), ColumnFactory.Build<string>(default));
            ProcessId = AddColumn(nameof(ProcessId), ColumnFactory.Build<int>(default));
            ExecutableLocation = AddColumn(nameof(ExecutableLocation), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            WorkingDirectory = AddColumn(nameof(WorkingDirectory), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            EnvironmentVariables = AddColumn(nameof(EnvironmentVariables), ColumnFactory.Build<IDictionary<string, string>>(default));
            Stdin = AddColumn(nameof(Stdin), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Stdout = AddColumn(nameof(Stdout), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Stderr = AddColumn(nameof(Stderr), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            StdoutStderr = AddColumn(nameof(StdoutStderr), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override Invocation Get(int index)
        {
            return (index == -1 ? null : new Invocation(this, index));
        }
    }
}
