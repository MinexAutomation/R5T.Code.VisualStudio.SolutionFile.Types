using System;

using SolutionFileConstants = R5T.Code.VisualStudio.Model.SolutionFileSpecific.Constants;


namespace R5T.Code.VisualStudio.Model
{
    public static class ProjectBuildConfigurationMappingExtensions
    {
        public static string ToSolutionFileLine(this ProjectBuildConfigurationMapping projectBuildConfigurationMapping)
        {
            var projectGuidToken = projectBuildConfigurationMapping.ProjectGUID.ToStringSolutionFileFormat();
            var solutionBuildConfigurationToken = projectBuildConfigurationMapping.SolutionBuildConfiguration.ToSolutionFileToken();
            var indicatorToken = projectBuildConfigurationMapping.ProjectConfigurationIndicator.ToSolutionFileToken();

            var targetToken = $"{projectGuidToken}{SolutionFileConstants.SolutionProjectConfigurationTokenSeparator}{solutionBuildConfigurationToken}{SolutionFileConstants.SolutionProjectConfigurationTokenSeparator}{indicatorToken}";

            var mappingToken = projectBuildConfigurationMapping.MappedSolutionBuildConfiguration.ToSolutionFileToken();

            var line = $"{targetToken} = {mappingToken}";
            return line;
        }
    }
}
