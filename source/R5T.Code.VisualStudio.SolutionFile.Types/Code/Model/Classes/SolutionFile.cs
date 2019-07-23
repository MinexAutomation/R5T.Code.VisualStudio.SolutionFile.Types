using System;
using System.Collections.Generic;

using R5T.Code.VisualStudio.IO;

using SolutionFileConstants = R5T.Code.VisualStudio.Model.SolutionFileSpecific.Constants;


namespace R5T.Code.VisualStudio.Model
{
    public class SolutionFile
    {
        #region Static

        public static SolutionFile Load(string solutionFilePath)
        {
            var serializer = new SolutionFileSerializer();

            var solutionFile = serializer.Deserialize(solutionFilePath);
            return solutionFile;
        }

        public static void Save(string solutionFilePath, SolutionFile solutionFile)
        {
            var serializer = new SolutionFileSerializer();

            serializer.Serialize(solutionFilePath, solutionFile);
        }

        /// <summary>
        /// Creates the result of "dotnet new sln".
        /// </summary>
        public static SolutionFile NewVisualStudio2017()
        {
            var solutionFile = new SolutionFile
            {
                FormatVersion = Version.Parse("12.00"),
                VisualStudioMoniker = "# Visual Studio 15",
                VisualStudioVersion = Version.Parse("15.0.26124.0"),
                MinimumVisualStudioVersion = Version.Parse("15.0.26124.0"),
            };

            var solutionConfigurationPlatformsGlobalSection = solutionFile.GlobalSections.AddSolutionConfigurationPlatformsGlobalSection();

            solutionConfigurationPlatformsGlobalSection.SolutionBuildConfigurationMappings.AddRange(new[]
            {
                new SolutionBuildConfigurationMapping { SolutionBuildConfiguration = SolutionBuildConfiguration.DebugAnyCPU, MappedSolutionBuildConfiguration = SolutionBuildConfiguration.DebugAnyCPU },
                new SolutionBuildConfigurationMapping { SolutionBuildConfiguration = SolutionBuildConfiguration.DebugX64, MappedSolutionBuildConfiguration = SolutionBuildConfiguration.DebugX64 },
                new SolutionBuildConfigurationMapping { SolutionBuildConfiguration = SolutionBuildConfiguration.DebugX86, MappedSolutionBuildConfiguration = SolutionBuildConfiguration.DebugX86 },
                new SolutionBuildConfigurationMapping { SolutionBuildConfiguration = SolutionBuildConfiguration.ReleaseAnyCPU, MappedSolutionBuildConfiguration = SolutionBuildConfiguration.ReleaseAnyCPU },
                new SolutionBuildConfigurationMapping { SolutionBuildConfiguration = SolutionBuildConfiguration.ReleaseX64, MappedSolutionBuildConfiguration = SolutionBuildConfiguration.ReleaseX64 },
                new SolutionBuildConfigurationMapping { SolutionBuildConfiguration = SolutionBuildConfiguration.ReleaseX86, MappedSolutionBuildConfiguration = SolutionBuildConfiguration.ReleaseX86 },
            });

            solutionFile.GlobalSections.AddGlobalSection(SolutionFile.CreateSolutionPropertiesGlobalSection);

            return solutionFile;
        }

        public static GeneralSolutionFileGlobalSection CreateSolutionPropertiesGlobalSection()
        {
            var solutionProperties = new GeneralSolutionFileGlobalSection
            {
                Name = SolutionFileConstants.SolutionPropertiesSolutionGlobalSectionName,
                PreOrPostSolution = PreOrPostSolution.PreSolution,
            };

            solutionProperties.Lines.Add("HideSolutionNode = FALSE");

            return solutionProperties;
        }

        public static GeneralSolutionFileGlobalSection CreateExtensibilityGlobals()
        {
            var solutionProperties = new GeneralSolutionFileGlobalSection
            {
                Name = SolutionFileConstants.ExtensibilityGlobalsSolutionGlobalSectionName,
                PreOrPostSolution = PreOrPostSolution.PostSolution,
            };

            solutionProperties.Lines.Add($"SolutionGuid = {Guid.NewGuid().ToStringSolutionFileFormat()}");

            return solutionProperties;
        }

        #endregion


        /// <summary>
        /// Example: the "12.00" in "Microsoft Visual Studio Solution File, Format Version 12.00".
        /// </summary>
        public Version FormatVersion { get; set; }
        /// <summary>
        /// Example: "# Visual Studio 15".
        /// </summary>
        public string VisualStudioMoniker { get; set; }
        /// <summary>
        /// Example: "VisualStudioVersion = 15.0.26124.0".
        /// </summary>
        public Version VisualStudioVersion { get; set; }
        /// <summary>
        /// Example: "MinimumVisualStudioVersion = 15.0.26124.0".
        /// </summary>
        public Version MinimumVisualStudioVersion { get; set; }
        /// <summary>
        /// List of solution project references.
        /// </summary>
        public List<SolutionFileProjectReference> SolutionFileProjectReferences { get; } = new List<SolutionFileProjectReference>();
        /// <summary>
        /// List of solution file global sections.
        /// </summary>
        public List<ISolutionFileGlobalSection> GlobalSections { get; } = new List<ISolutionFileGlobalSection>();
    }
}
