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

        internal IColumn<String> CommandLine;
        internal IColumn<IList<String>> Arguments;
        internal RefListColumn ResponseFiles;
        internal IColumn<DateTime> StartTimeUtc;
        internal IColumn<DateTime> EndTimeUtc;
        internal IColumn<int> ExitCode;
        internal RefListColumn RuleConfigurationOverrides;
        internal RefListColumn NotificationConfigurationOverrides;
        internal RefListColumn ToolExecutionNotifications;
        internal RefListColumn ToolConfigurationNotifications;
        internal IColumn<String> ExitCodeDescription;
        internal IColumn<String> ExitSignalName;
        internal IColumn<int> ExitSignalNumber;
        internal IColumn<String> ProcessStartFailureMessage;
        internal IColumn<bool> ExecutionSuccessful;
        internal IColumn<String> Machine;
        internal IColumn<String> Account;
        internal IColumn<int> ProcessId;
        internal RefColumn ExecutableLocation;
        internal RefColumn WorkingDirectory;
        internal IColumn<IDictionary<String, String>> EnvironmentVariables;
        internal RefColumn Stdin;
        internal RefColumn Stdout;
        internal RefColumn Stderr;
        internal RefColumn StdoutStderr;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal InvocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            CommandLine = AddColumn(nameof(CommandLine), database.BuildColumn<String>(nameof(Invocation), nameof(CommandLine), default));
            Arguments = AddColumn(nameof(Arguments), database.BuildColumn<IList<String>>(nameof(Invocation), nameof(Arguments), default));
            ResponseFiles = AddColumn(nameof(ResponseFiles), new RefListColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            StartTimeUtc = AddColumn(nameof(StartTimeUtc), database.BuildColumn<DateTime>(nameof(Invocation), nameof(StartTimeUtc), default));
            EndTimeUtc = AddColumn(nameof(EndTimeUtc), database.BuildColumn<DateTime>(nameof(Invocation), nameof(EndTimeUtc), default));
            ExitCode = AddColumn(nameof(ExitCode), database.BuildColumn<int>(nameof(Invocation), nameof(ExitCode), default));
            RuleConfigurationOverrides = AddColumn(nameof(RuleConfigurationOverrides), new RefListColumn(nameof(SarifLogDatabase.ConfigurationOverride)));
            NotificationConfigurationOverrides = AddColumn(nameof(NotificationConfigurationOverrides), new RefListColumn(nameof(SarifLogDatabase.ConfigurationOverride)));
            ToolExecutionNotifications = AddColumn(nameof(ToolExecutionNotifications), new RefListColumn(nameof(SarifLogDatabase.Notification)));
            ToolConfigurationNotifications = AddColumn(nameof(ToolConfigurationNotifications), new RefListColumn(nameof(SarifLogDatabase.Notification)));
            ExitCodeDescription = AddColumn(nameof(ExitCodeDescription), database.BuildColumn<String>(nameof(Invocation), nameof(ExitCodeDescription), default));
            ExitSignalName = AddColumn(nameof(ExitSignalName), database.BuildColumn<String>(nameof(Invocation), nameof(ExitSignalName), default));
            ExitSignalNumber = AddColumn(nameof(ExitSignalNumber), database.BuildColumn<int>(nameof(Invocation), nameof(ExitSignalNumber), default));
            ProcessStartFailureMessage = AddColumn(nameof(ProcessStartFailureMessage), database.BuildColumn<String>(nameof(Invocation), nameof(ProcessStartFailureMessage), default));
            ExecutionSuccessful = AddColumn(nameof(ExecutionSuccessful), database.BuildColumn<bool>(nameof(Invocation), nameof(ExecutionSuccessful), default));
            Machine = AddColumn(nameof(Machine), database.BuildColumn<String>(nameof(Invocation), nameof(Machine), default));
            Account = AddColumn(nameof(Account), database.BuildColumn<String>(nameof(Invocation), nameof(Account), default));
            ProcessId = AddColumn(nameof(ProcessId), database.BuildColumn<int>(nameof(Invocation), nameof(ProcessId), default));
            ExecutableLocation = AddColumn(nameof(ExecutableLocation), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            WorkingDirectory = AddColumn(nameof(WorkingDirectory), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            EnvironmentVariables = AddColumn(nameof(EnvironmentVariables), database.BuildColumn<IDictionary<String, String>>(nameof(Invocation), nameof(EnvironmentVariables), default));
            Stdin = AddColumn(nameof(Stdin), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Stdout = AddColumn(nameof(Stdout), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Stderr = AddColumn(nameof(Stderr), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            StdoutStderr = AddColumn(nameof(StdoutStderr), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Invocation), nameof(Properties), default));
        }

        public override Invocation Get(int index)
        {
            return (index == -1 ? null : new Invocation(this, index));
        }
    }
}
