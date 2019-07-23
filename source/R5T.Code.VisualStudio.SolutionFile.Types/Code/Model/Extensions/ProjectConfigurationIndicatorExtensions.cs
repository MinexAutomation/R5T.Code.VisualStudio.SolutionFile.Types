using System;

using R5T.NetStandard;

using SolutionFileConstants = R5T.Code.VisualStudio.Model.SolutionFileSpecific.Constants;


namespace R5T.Code.VisualStudio.Model
{
    public static class ProjectConfigurationIndicatorExtensions
    {
        public static string ToSolutionFileToken(this ProjectConfigurationIndicator projectConfigurationIndicator)
        {
            switch(projectConfigurationIndicator)
            {
                case ProjectConfigurationIndicator.ActiveCfg:
                    return SolutionFileConstants.ActiveCfgSolutionFileToken;

                case ProjectConfigurationIndicator.Build0:
                    return SolutionFileConstants.Build0SolutionFileToken;

                default:
                    throw new Exception(EnumHelper.UnexpectedEnumerationValueMessage(projectConfigurationIndicator));
            }
        }
    }
}
