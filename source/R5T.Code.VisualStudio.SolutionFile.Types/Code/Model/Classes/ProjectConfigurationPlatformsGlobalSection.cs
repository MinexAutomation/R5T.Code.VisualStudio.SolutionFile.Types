using System;
using System.Collections.Generic;


namespace R5T.Code.VisualStudio.Model
{
    public class ProjectConfigurationPlatformsGlobalSection : SolutionFileGlobalSectionBase
    {
        public const string SolutionFileGlobalSectionName = "ProjectConfigurationPlatforms";


        #region Static

        /// <summary>
        /// Creates a new <see cref="ProjectConfigurationPlatformsGlobalSection"/> with the <see cref="ProjectConfigurationPlatformsGlobalSection.SolutionFileGlobalSectionName"/> and <see cref="PreOrPostSolution.PostSolution"/>.
        /// </summary>
        public static ProjectConfigurationPlatformsGlobalSection New()
        {
            var output = new ProjectConfigurationPlatformsGlobalSection
            {
                Name = ProjectConfigurationPlatformsGlobalSection.SolutionFileGlobalSectionName,
                PreOrPostSolution = PreOrPostSolution.PostSolution,
            };
            return output;
        }

        #endregion


        public List<ProjectBuildConfigurationMapping> ProjectBuildConfigurationMappings { get; } = new List<ProjectBuildConfigurationMapping>();

        public override IEnumerable<string> ContentLines
        {
            get
            {
                foreach (var projectBuildConfigurationMapping in ProjectBuildConfigurationMappings)
                {
                    var line = projectBuildConfigurationMapping.ToSolutionFileLine();
                    yield return line;
                }
            }
        }
    }
}
