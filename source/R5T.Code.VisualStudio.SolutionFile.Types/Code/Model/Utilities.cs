using System;

using R5T.NetStandard;


namespace R5T.Code.VisualStudio.Model.SolutionFileSpecific
{
    public static class Utilities
    {
        public static PreOrPostSolution ToPreOrPostSolution(string value)
        {
            switch(value)
            {
                case "preSolution":
                    return PreOrPostSolution.PreSolution;

                case "postSolution":
                    return PreOrPostSolution.PostSolution;

                default:
                    throw new Exception(EnumHelper.UnrecognizedEnumerationValueMessage<PreOrPostSolution>(value));
            }
        }

        public static string ToStringStandard(PreOrPostSolution preOrPostSolution)
        {
            switch(preOrPostSolution)
            {
                case PreOrPostSolution.PreSolution:
                    return "preSolution";

                case PreOrPostSolution.PostSolution:
                    return "postSolution";

                default:
                    throw new Exception(EnumHelper.UnexpectedEnumerationValueMessage(preOrPostSolution));
            }
        }

        public static BuildConfiguration ToBuildConfiguration(string value)
        {
            switch(value)
            {
                case Constants.DebugSolutionFileBuildConfigurationToken:
                    return BuildConfiguration.Debug;

                case Constants.ReleaseSolutionFileBuildConfigurationToken:
                    return BuildConfiguration.Release;

                default:
                    throw new Exception(EnumHelper.UnrecognizedEnumerationValueMessage<BuildConfiguration>(value));
            }
        }

        public static PlatformTarget ToPlatformTarget(string value)
        {
            switch(value)
            {
                case Constants.AnyCpuSolutionFileReleasePlatformToken:
                    return PlatformTarget.AnyCPU;

                case Constants.x86SolutionFileReleasePlatformToken:
                    return PlatformTarget.x32;

                case Constants.x64SolutionFileReleasePlatformToken:
                    return PlatformTarget.x64;

                default:
                    throw new Exception(EnumHelper.UnrecognizedEnumerationValueMessage<PlatformTarget>(value));
                }
        }

        public static ProjectConfigurationIndicator ToProjectConfigurationIndicator(string value)
        {
            switch(value)
            {
                case Constants.ActiveCfgSolutionFileToken:
                    return ProjectConfigurationIndicator.ActiveCfg;

                case Constants.Build0SolutionFileToken:
                    return ProjectConfigurationIndicator.Build0;

                default:
                    throw new Exception(EnumHelper.UnrecognizedEnumerationValueMessage<ProjectConfigurationIndicator>(value));
            }
        }
    }
}
