using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToInvocation))]
    public partial class Invocation
    { }
    
    internal class JsonToInvocation : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Invocation>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Invocation>>()
        {
            ["commandLine"] = (reader, root, me) => me.CommandLine = JsonToString.Read(reader, root),
            ["arguments"] = (reader, root, me) => JsonToIList<String>.Read(reader, root, me.Arguments, JsonToString.Read),
            ["responseFiles"] = (reader, root, me) => JsonToIList<ArtifactLocation>.Read(reader, root, me.ResponseFiles, JsonToArtifactLocation.Read),
            ["startTimeUtc"] = (reader, root, me) => me.StartTimeUtc = JsonToDateTime.Read(reader, root),
            ["endTimeUtc"] = (reader, root, me) => me.EndTimeUtc = JsonToDateTime.Read(reader, root),
            ["exitCode"] = (reader, root, me) => me.ExitCode = JsonToInt.Read(reader, root),
            ["ruleConfigurationOverrides"] = (reader, root, me) => JsonToIList<ConfigurationOverride>.Read(reader, root, me.RuleConfigurationOverrides, JsonToConfigurationOverride.Read),
            ["notificationConfigurationOverrides"] = (reader, root, me) => JsonToIList<ConfigurationOverride>.Read(reader, root, me.NotificationConfigurationOverrides, JsonToConfigurationOverride.Read),
            ["toolExecutionNotifications"] = (reader, root, me) => JsonToIList<Notification>.Read(reader, root, me.ToolExecutionNotifications, JsonToNotification.Read),
            ["toolConfigurationNotifications"] = (reader, root, me) => JsonToIList<Notification>.Read(reader, root, me.ToolConfigurationNotifications, JsonToNotification.Read),
            ["exitCodeDescription"] = (reader, root, me) => me.ExitCodeDescription = JsonToString.Read(reader, root),
            ["exitSignalName"] = (reader, root, me) => me.ExitSignalName = JsonToString.Read(reader, root),
            ["exitSignalNumber"] = (reader, root, me) => me.ExitSignalNumber = JsonToInt.Read(reader, root),
            ["processStartFailureMessage"] = (reader, root, me) => me.ProcessStartFailureMessage = JsonToString.Read(reader, root),
            ["executionSuccessful"] = (reader, root, me) => me.ExecutionSuccessful = JsonToBool.Read(reader, root),
            ["machine"] = (reader, root, me) => me.Machine = JsonToString.Read(reader, root),
            ["account"] = (reader, root, me) => me.Account = JsonToString.Read(reader, root),
            ["processId"] = (reader, root, me) => me.ProcessId = JsonToInt.Read(reader, root),
            ["executableLocation"] = (reader, root, me) => me.ExecutableLocation = JsonToArtifactLocation.Read(reader, root),
            ["workingDirectory"] = (reader, root, me) => me.WorkingDirectory = JsonToArtifactLocation.Read(reader, root),
            ["environmentVariables"] = (reader, root, me) => me.EnvironmentVariables = JsonToIDictionary<String, String>.Read(reader, root, null, JsonToString.Read),
            ["stdin"] = (reader, root, me) => me.Stdin = JsonToArtifactLocation.Read(reader, root),
            ["stdout"] = (reader, root, me) => me.Stdout = JsonToArtifactLocation.Read(reader, root),
            ["stderr"] = (reader, root, me) => me.Stderr = JsonToArtifactLocation.Read(reader, root),
            ["stdoutStderr"] = (reader, root, me) => me.StdoutStderr = JsonToArtifactLocation.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Invocation Read(JsonReader reader, SarifLog root = null)
        {
            Invocation item = (root == null ? new Invocation() : new Invocation(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Invocation item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Invocation item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "commandLine", item.CommandLine, default);
                JsonToIList<String>.Write(writer, "arguments", item.Arguments, JsonToString.Write);
                JsonToIList<ArtifactLocation>.Write(writer, "responseFiles", item.ResponseFiles, JsonToArtifactLocation.Write);
                JsonToDateTime.Write(writer, "startTimeUtc", item.StartTimeUtc, default);
                JsonToDateTime.Write(writer, "endTimeUtc", item.EndTimeUtc, default);
                JsonToInt.Write(writer, "exitCode", item.ExitCode, default);
                JsonToIList<ConfigurationOverride>.Write(writer, "ruleConfigurationOverrides", item.RuleConfigurationOverrides, JsonToConfigurationOverride.Write);
                JsonToIList<ConfigurationOverride>.Write(writer, "notificationConfigurationOverrides", item.NotificationConfigurationOverrides, JsonToConfigurationOverride.Write);
                JsonToIList<Notification>.Write(writer, "toolExecutionNotifications", item.ToolExecutionNotifications, JsonToNotification.Write);
                JsonToIList<Notification>.Write(writer, "toolConfigurationNotifications", item.ToolConfigurationNotifications, JsonToNotification.Write);
                JsonToString.Write(writer, "exitCodeDescription", item.ExitCodeDescription, default);
                JsonToString.Write(writer, "exitSignalName", item.ExitSignalName, default);
                JsonToInt.Write(writer, "exitSignalNumber", item.ExitSignalNumber, default);
                JsonToString.Write(writer, "processStartFailureMessage", item.ProcessStartFailureMessage, default);
                JsonToBool.Write(writer, "executionSuccessful", item.ExecutionSuccessful, default);
                JsonToString.Write(writer, "machine", item.Machine, default);
                JsonToString.Write(writer, "account", item.Account, default);
                JsonToInt.Write(writer, "processId", item.ProcessId, default);
                JsonToArtifactLocation.Write(writer, "executableLocation", item.ExecutableLocation);
                JsonToArtifactLocation.Write(writer, "workingDirectory", item.WorkingDirectory);
                JsonToIDictionary<String, String>.Write(writer, "environmentVariables", item.EnvironmentVariables, JsonToString.Write);
                JsonToArtifactLocation.Write(writer, "stdin", item.Stdin);
                JsonToArtifactLocation.Write(writer, "stdout", item.Stdout);
                JsonToArtifactLocation.Write(writer, "stderr", item.Stderr);
                JsonToArtifactLocation.Write(writer, "stdoutStderr", item.StdoutStderr);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Invocation));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Invocation)value);
        }
    }
}
