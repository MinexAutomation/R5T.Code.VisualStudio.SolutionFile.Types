using System;

using R5T.NetStandard;

using SolutionFileConstants = R5T.Code.VisualStudio.Model.SolutionFileSpecific.Constants;


namespace R5T.Code.VisualStudio.Model
{
    public static class SolutionBuildConfigurationExtensions
    {
        public static string ToSolutionFileToken(this SolutionBuildConfiguration solutionBuildConfiguration)
        {
            var buildConfigurationToken = solutionBuildConfiguration.BuildConfiguration.ToSolutionFileToken();
            var platformTargetToken = solutionBuildConfiguration.PlatformTarget.ToSolutionFileToken();

            var token = $"{buildConfigurationToken}{SolutionFileConstants.SolutionBuildConfigurationTokenSeparator}{platformTargetToken}";
            return token;
        }
    }
}
