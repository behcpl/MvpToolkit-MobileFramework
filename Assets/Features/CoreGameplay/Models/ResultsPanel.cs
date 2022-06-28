using Behc.Utils;

namespace Features.CoreGameplay.Models
{
    public class ResultsPanel { }

    public class ResultsPanelFactory : IFactory<ResultsPanel>
    {
        public ResultsPanel Create()
        {
            return new ResultsPanel();
        }
    }
}