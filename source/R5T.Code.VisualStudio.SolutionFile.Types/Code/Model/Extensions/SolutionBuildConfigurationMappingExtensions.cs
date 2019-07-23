using System;


namespace R5T.Code.VisualStudio.Model
{
    public static class SolutionBuildConfigurationMappingExtensions
    {
        public static string ToSolutionFileLine(this SolutionBuildConfigurationMapping solutionBuildConfigurationMapping)
        {
            var solutionBuildConfigurationToken = solutionBuildConfigurationMapping.SolutionBuildConfiguration.ToSolutionFileToken();
            var mappedSolutionBuildConfigurationToken = solutionBuildConfigurationMapping.MappedSolutionBuildConfiguration.ToSolutionFileToken();

            var token = $"{solutionBuildConfigurationToken} = {mappedSolutionBuildConfigurationToken}";
            return token;
        }
    }
}
