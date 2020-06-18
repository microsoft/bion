using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(InvocationConverter))]
    public partial class Invocation
    { }
    
    public class InvocationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Invocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadInvocation();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Invocation)value);
        }
    }
    
    internal static class InvocationJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Invocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Invocation>>()
        {
            ["commandLine"] = (reader, root, me) => me.CommandLine = reader.ReadString(root),
            ["arguments"] = (reader, root, me) => reader.ReadList(root, me.Arguments, JsonReaderExtensions.ReadString),
            ["responseFiles"] = (reader, root, me) => reader.ReadList(root, me.ResponseFiles, ArtifactLocationJsonExtensions.ReadArtifactLocation),
            ["startTimeUtc"] = (reader, root, me) => me.StartTimeUtc = reader.ReadDateTime(root),
            ["endTimeUtc"] = (reader, root, me) => me.EndTimeUtc = reader.ReadDateTime(root),
            ["exitCode"] = (reader, root, me) => me.ExitCode = reader.ReadInt(root),
            ["ruleConfigurationOverrides"] = (reader, root, me) => reader.ReadList(root, me.RuleConfigurationOverrides, ConfigurationOverrideJsonExtensions.ReadConfigurationOverride),
            ["notificationConfigurationOverrides"] = (reader, root, me) => reader.ReadList(root, me.NotificationConfigurationOverrides, ConfigurationOverrideJsonExtensions.ReadConfigurationOverride),
            ["toolExecutionNotifications"] = (reader, root, me) => reader.ReadList(root, me.ToolExecutionNotifications, NotificationJsonExtensions.ReadNotification),
            ["toolConfigurationNotifications"] = (reader, root, me) => reader.ReadList(root, me.ToolConfigurationNotifications, NotificationJsonExtensions.ReadNotification),
            ["exitCodeDescription"] = (reader, root, me) => me.ExitCodeDescription = reader.ReadString(root),
            ["exitSignalName"] = (reader, root, me) => me.ExitSignalName = reader.ReadString(root),
            ["exitSignalNumber"] = (reader, root, me) => me.ExitSignalNumber = reader.ReadInt(root),
            ["processStartFailureMessage"] = (reader, root, me) => me.ProcessStartFailureMessage = reader.ReadString(root),
            ["executionSuccessful"] = (reader, root, me) => me.ExecutionSuccessful = reader.ReadBool(root),
            ["machine"] = (reader, root, me) => me.Machine = reader.ReadString(root),
            ["account"] = (reader, root, me) => me.Account = reader.ReadString(root),
            ["processId"] = (reader, root, me) => me.ProcessId = reader.ReadInt(root),
            ["executableLocation"] = (reader, root, me) => me.ExecutableLocation = reader.ReadArtifactLocation(root),
            ["workingDirectory"] = (reader, root, me) => me.WorkingDirectory = reader.ReadArtifactLocation(root),
            ["environmentVariables"] = (reader, root, me) => reader.ReadDictionary(root, me.EnvironmentVariables, JsonReaderExtensions.ReadString, JsonReaderExtensions.ReadString),
            ["stdin"] = (reader, root, me) => me.Stdin = reader.ReadArtifactLocation(root),
            ["stdout"] = (reader, root, me) => me.Stdout = reader.ReadArtifactLocation(root),
            ["stderr"] = (reader, root, me) => me.Stderr = reader.ReadArtifactLocation(root),
            ["stdoutStderr"] = (reader, root, me) => me.StdoutStderr = reader.ReadArtifactLocation(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Invocation ReadInvocation(this JsonReader reader, SarifLog root = null)
        {
            Invocation item = (root == null ? new Invocation() : new Invocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Invocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Invocation item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("commandLine", item.CommandLine, default);
                writer.Write("arguments", item.Arguments, default);
                writer.WriteList("responseFiles", item.ResponseFiles, ArtifactLocationJsonExtensions.Write);
                writer.Write("startTimeUtc", item.StartTimeUtc, default);
                writer.Write("endTimeUtc", item.EndTimeUtc, default);
                writer.Write("exitCode", item.ExitCode, default);
                writer.WriteList("ruleConfigurationOverrides", item.RuleConfigurationOverrides, ConfigurationOverrideJsonExtensions.Write);
                writer.WriteList("notificationConfigurationOverrides", item.NotificationConfigurationOverrides, ConfigurationOverrideJsonExtensions.Write);
                writer.WriteList("toolExecutionNotifications", item.ToolExecutionNotifications, NotificationJsonExtensions.Write);
                writer.WriteList("toolConfigurationNotifications", item.ToolConfigurationNotifications, NotificationJsonExtensions.Write);
                writer.Write("exitCodeDescription", item.ExitCodeDescription, default);
                writer.Write("exitSignalName", item.ExitSignalName, default);
                writer.Write("exitSignalNumber", item.ExitSignalNumber, default);
                writer.Write("processStartFailureMessage", item.ProcessStartFailureMessage, default);
                writer.Write("executionSuccessful", item.ExecutionSuccessful, default);
                writer.Write("machine", item.Machine, default);
                writer.Write("account", item.Account, default);
                writer.Write("processId", item.ProcessId, default);
                writer.Write("executableLocation", item.ExecutableLocation);
                writer.Write("workingDirectory", item.WorkingDirectory);
                writer.Write("environmentVariables", item.EnvironmentVariables, default);
                writer.Write("stdin", item.Stdin);
                writer.Write("stdout", item.Stdout);
                writer.Write("stderr", item.Stderr);
                writer.Write("stdoutStderr", item.StdoutStderr);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
