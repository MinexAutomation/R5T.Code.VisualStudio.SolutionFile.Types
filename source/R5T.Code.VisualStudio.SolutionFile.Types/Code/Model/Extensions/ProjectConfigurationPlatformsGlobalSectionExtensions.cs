using System;


namespace R5T.Code.VisualStudio.Model
{
    public static class ProjectConfigurationPlatformsGlobalSectionExtensions
    {
        public static void AddProjectConfigurations(this ProjectConfigurationPlatformsGlobalSection projectConfigurationPlatforms, Guid projectGUID, SolutionConfigurationPlatformsGlobalSection solutionConfigurationPlatforms)
        {
            var indicators = new[]
            {
                ProjectConfigurationIndicator.ActiveCfg,
                ProjectConfigurationIndicator.Build0,
            };

            foreach (var solutionBuildConfigurationMapping in solutionConfigurationPlatforms.SolutionBuildConfigurationMappings)
            {
                var mappedSolutionBuildConfiguration = solutionBuildConfigurationMapping.SolutionBuildConfiguration.BuildConfiguration == BuildConfiguration.Debug
                    ? SolutionBuildConfiguration.DebugAnyCPU
                    : SolutionBuildConfiguration.ReleaseAnyCPU;

                foreach (var indicator in indicators)
                {
                    projectConfigurationPlatforms.ProjectBuildConfigurationMappings.Add(new ProjectBuildConfigurationMapping
                    {
                        ProjectGUID = projectGUID,
                        SolutionBuildConfiguration = solutionBuildConfigurationMapping.SolutionBuildConfiguration,
                        MappedSolutionBuildConfiguration = mappedSolutionBuildConfiguration,
                        ProjectConfigurationIndicator = indicator,
                    });
                }
            }
        }
    }
}
