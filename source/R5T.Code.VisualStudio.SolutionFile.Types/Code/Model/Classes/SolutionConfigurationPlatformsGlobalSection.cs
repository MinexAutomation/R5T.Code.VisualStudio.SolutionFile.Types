using System;
using System.Collections.Generic;


namespace R5T.Code.VisualStudio.Model
{
    public class SolutionConfigurationPlatformsGlobalSection : SolutionFileGlobalSectionBase
    {
        public const string SolutionFileGlobalSectionName = "SolutionConfigurationPlatforms";


        #region Static

        /// <summary>
        /// Creates a new <see cref="SolutionConfigurationPlatformsGlobalSection"/> with the <see cref="SolutionConfigurationPlatformsGlobalSection.SolutionFileGlobalSectionName"/> and <see cref="PreOrPostSolution.PreSolution"/>.
        /// </summary>
        public static SolutionConfigurationPlatformsGlobalSection New()
        {
            var output = new SolutionConfigurationPlatformsGlobalSection
            {
                Name = SolutionConfigurationPlatformsGlobalSection.SolutionFileGlobalSectionName,
                PreOrPostSolution = PreOrPostSolution.PreSolution,
            };
            return output;
        }

        #endregion


        public List<SolutionBuildConfigurationMapping> SolutionBuildConfigurationMappings { get; } = new List<SolutionBuildConfigurationMapping>();

        public override IEnumerable<string> ContentLines
        {
            get
            {
                foreach (var solutionBuildConfigurationMapping in this.SolutionBuildConfigurationMappings)
                {
                    var line = solutionBuildConfigurationMapping.ToSolutionFileLine();
                    yield return line;
                }
            }
        }
    }
}
