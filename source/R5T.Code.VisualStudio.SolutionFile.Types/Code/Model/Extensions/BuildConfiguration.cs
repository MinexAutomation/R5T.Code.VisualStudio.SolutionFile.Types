using System;

using R5T.NetStandard;

using SolutionFileConstants = R5T.Code.VisualStudio.Model.SolutionFileSpecific.Constants;


namespace R5T.Code.VisualStudio.Model
{
    public static class BuildConfigurationExtensions
    {
        public static string ToSolutionFileToken(this BuildConfiguration buildConfiguration)
        {
            switch(buildConfiguration)
            {
                case BuildConfiguration.Debug:
                    return SolutionFileConstants.DebugSolutionFileBuildConfigurationToken;

                case BuildConfiguration.Release:
                    return SolutionFileConstants.ReleaseSolutionFileBuildConfigurationToken;

                default:
                    throw new Exception(EnumHelper.UnexpectedEnumerationValueMessage(buildConfiguration));
            }
        }
    }
}
