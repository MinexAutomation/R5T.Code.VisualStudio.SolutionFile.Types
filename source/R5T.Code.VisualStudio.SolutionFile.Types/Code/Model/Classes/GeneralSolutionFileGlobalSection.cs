using System;
using System.Collections.Generic;


namespace R5T.Code.VisualStudio.Model
{
    public class GeneralSolutionFileGlobalSection : SolutionFileGlobalSectionBase
    {
        #region Static

        public static GeneralSolutionFileGlobalSection New(string name, PreOrPostSolution preOrPostSolution)
        {
            var output = new GeneralSolutionFileGlobalSection
            {
                Name = name,
                PreOrPostSolution = preOrPostSolution,
            };
            return output;
        }

        #endregion


        public List<string> Lines { get; } = new List<string>();

        public override IEnumerable<string> ContentLines => this.Lines;
    }
}
