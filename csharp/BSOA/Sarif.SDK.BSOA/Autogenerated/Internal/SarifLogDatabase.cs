// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Database for 'SarifLog'
    /// </summary>
    internal partial class SarifLogDatabase : Database
    {
        [ThreadStatic]
        private static WeakReference<SarifLogDatabase> _lastCreated;

        internal static SarifLogDatabase Current => (_lastCreated.TryGetTarget(out SarifLogDatabase value) ? value : new SarifLogDatabase());
        
        internal SarifLogTable SarifLog { get; }
        internal AddressTable Address { get; }
        internal ArtifactTable Artifact { get; }
        internal ArtifactChangeTable ArtifactChange { get; }
        internal ArtifactContentTable ArtifactContent { get; }
        internal ArtifactLocationTable ArtifactLocation { get; }
        internal AttachmentTable Attachment { get; }
        internal CodeFlowTable CodeFlow { get; }
        internal ConfigurationOverrideTable ConfigurationOverride { get; }
        internal ConversionTable Conversion { get; }
        internal EdgeTable Edge { get; }
        internal EdgeTraversalTable EdgeTraversal { get; }
        internal ExceptionDataTable ExceptionData { get; }
        internal ExternalPropertiesTable ExternalProperties { get; }
        internal ExternalPropertyFileReferenceTable ExternalPropertyFileReference { get; }
        internal ExternalPropertyFileReferencesTable ExternalPropertyFileReferences { get; }
        internal FixTable Fix { get; }
        internal GraphTable Graph { get; }
        internal GraphTraversalTable GraphTraversal { get; }
        internal InvocationTable Invocation { get; }
        internal LocationTable Location { get; }
        internal LocationRelationshipTable LocationRelationship { get; }
        internal LogicalLocationTable LogicalLocation { get; }
        internal MessageTable Message { get; }
        internal MultiformatMessageStringTable MultiformatMessageString { get; }
        internal NodeTable Node { get; }
        internal NotificationTable Notification { get; }
        internal PhysicalLocationTable PhysicalLocation { get; }
        internal PropertyBagTable PropertyBag { get; }
        internal RectangleTable Rectangle { get; }
        internal RegionTable Region { get; }
        internal ReplacementTable Replacement { get; }
        internal ReportingDescriptorTable ReportingDescriptor { get; }
        internal ReportingConfigurationTable ReportingConfiguration { get; }
        internal ReportingDescriptorReferenceTable ReportingDescriptorReference { get; }
        internal ReportingDescriptorRelationshipTable ReportingDescriptorRelationship { get; }
        internal ResultTable Result { get; }
        internal ResultProvenanceTable ResultProvenance { get; }
        internal RunTable Run { get; }
        internal RunAutomationDetailsTable RunAutomationDetails { get; }
        internal SpecialLocationsTable SpecialLocations { get; }
        internal StackTable Stack { get; }
        internal StackFrameTable StackFrame { get; }
        internal SuppressionTable Suppression { get; }
        internal ThreadFlowTable ThreadFlow { get; }
        internal ThreadFlowLocationTable ThreadFlowLocation { get; }
        internal ToolTable Tool { get; }
        internal ToolComponentTable ToolComponent { get; }
        internal ToolComponentReferenceTable ToolComponentReference { get; }
        internal TranslationMetadataTable TranslationMetadata { get; }
        internal VersionControlDetailsTable VersionControlDetails { get; }
        internal WebRequestTable WebRequest { get; }
        internal WebResponseTable WebResponse { get; }

        public SarifLogDatabase()
        {
            _lastCreated = new WeakReference<SarifLogDatabase>(this);

            SarifLog = AddTable(nameof(SarifLog), new SarifLogTable(this));
            Address = AddTable(nameof(Address), new AddressTable(this));
            Artifact = AddTable(nameof(Artifact), new ArtifactTable(this));
            ArtifactChange = AddTable(nameof(ArtifactChange), new ArtifactChangeTable(this));
            ArtifactContent = AddTable(nameof(ArtifactContent), new ArtifactContentTable(this));
            ArtifactLocation = AddTable(nameof(ArtifactLocation), new ArtifactLocationTable(this));
            Attachment = AddTable(nameof(Attachment), new AttachmentTable(this));
            CodeFlow = AddTable(nameof(CodeFlow), new CodeFlowTable(this));
            ConfigurationOverride = AddTable(nameof(ConfigurationOverride), new ConfigurationOverrideTable(this));
            Conversion = AddTable(nameof(Conversion), new ConversionTable(this));
            Edge = AddTable(nameof(Edge), new EdgeTable(this));
            EdgeTraversal = AddTable(nameof(EdgeTraversal), new EdgeTraversalTable(this));
            ExceptionData = AddTable(nameof(ExceptionData), new ExceptionDataTable(this));
            ExternalProperties = AddTable(nameof(ExternalProperties), new ExternalPropertiesTable(this));
            ExternalPropertyFileReference = AddTable(nameof(ExternalPropertyFileReference), new ExternalPropertyFileReferenceTable(this));
            ExternalPropertyFileReferences = AddTable(nameof(ExternalPropertyFileReferences), new ExternalPropertyFileReferencesTable(this));
            Fix = AddTable(nameof(Fix), new FixTable(this));
            Graph = AddTable(nameof(Graph), new GraphTable(this));
            GraphTraversal = AddTable(nameof(GraphTraversal), new GraphTraversalTable(this));
            Invocation = AddTable(nameof(Invocation), new InvocationTable(this));
            Location = AddTable(nameof(Location), new LocationTable(this));
            LocationRelationship = AddTable(nameof(LocationRelationship), new LocationRelationshipTable(this));
            LogicalLocation = AddTable(nameof(LogicalLocation), new LogicalLocationTable(this));
            Message = AddTable(nameof(Message), new MessageTable(this));
            MultiformatMessageString = AddTable(nameof(MultiformatMessageString), new MultiformatMessageStringTable(this));
            Node = AddTable(nameof(Node), new NodeTable(this));
            Notification = AddTable(nameof(Notification), new NotificationTable(this));
            PhysicalLocation = AddTable(nameof(PhysicalLocation), new PhysicalLocationTable(this));
            PropertyBag = AddTable(nameof(PropertyBag), new PropertyBagTable(this));
            Rectangle = AddTable(nameof(Rectangle), new RectangleTable(this));
            Region = AddTable(nameof(Region), new RegionTable(this));
            Replacement = AddTable(nameof(Replacement), new ReplacementTable(this));
            ReportingDescriptor = AddTable(nameof(ReportingDescriptor), new ReportingDescriptorTable(this));
            ReportingConfiguration = AddTable(nameof(ReportingConfiguration), new ReportingConfigurationTable(this));
            ReportingDescriptorReference = AddTable(nameof(ReportingDescriptorReference), new ReportingDescriptorReferenceTable(this));
            ReportingDescriptorRelationship = AddTable(nameof(ReportingDescriptorRelationship), new ReportingDescriptorRelationshipTable(this));
            Result = AddTable(nameof(Result), new ResultTable(this));
            ResultProvenance = AddTable(nameof(ResultProvenance), new ResultProvenanceTable(this));
            Run = AddTable(nameof(Run), new RunTable(this));
            RunAutomationDetails = AddTable(nameof(RunAutomationDetails), new RunAutomationDetailsTable(this));
            SpecialLocations = AddTable(nameof(SpecialLocations), new SpecialLocationsTable(this));
            Stack = AddTable(nameof(Stack), new StackTable(this));
            StackFrame = AddTable(nameof(StackFrame), new StackFrameTable(this));
            Suppression = AddTable(nameof(Suppression), new SuppressionTable(this));
            ThreadFlow = AddTable(nameof(ThreadFlow), new ThreadFlowTable(this));
            ThreadFlowLocation = AddTable(nameof(ThreadFlowLocation), new ThreadFlowLocationTable(this));
            Tool = AddTable(nameof(Tool), new ToolTable(this));
            ToolComponent = AddTable(nameof(ToolComponent), new ToolComponentTable(this));
            ToolComponentReference = AddTable(nameof(ToolComponentReference), new ToolComponentReferenceTable(this));
            TranslationMetadata = AddTable(nameof(TranslationMetadata), new TranslationMetadataTable(this));
            VersionControlDetails = AddTable(nameof(VersionControlDetails), new VersionControlDetailsTable(this));
            WebRequest = AddTable(nameof(WebRequest), new WebRequestTable(this));
            WebResponse = AddTable(nameof(WebResponse), new WebResponseTable(this));
        }
    }
}
