using System;

using R5T.NetStandard;

using SolutionFileUtilities = R5T.Code.VisualStudio.Model.SolutionFileSpecific.Constants;


namespace R5T.Code.VisualStudio.Model
{
    public static class PlatformTargetExtensions
    {
        public static string ToSolutionFileToken(this PlatformTarget platformTarget)
        {
            switch(platformTarget)
            {
                case PlatformTarget.AnyCPU:
                    return SolutionFileUtilities.AnyCpuSolutionFileReleasePlatformToken;

                case PlatformTarget.x32:
                    return SolutionFileUtilities.x86SolutionFileReleasePlatformToken; // x32 is x86.

                case PlatformTarget.x64:
                    return SolutionFileUtilities.x64SolutionFileReleasePlatformToken;

                default:
                    throw new Exception(EnumHelper.UnexpectedEnumerationValueMessage(platformTarget));
            }
        }
    }
}
