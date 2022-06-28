using Behc.Utils;
using UnityEngine;

namespace Features.CoreGameplay.Models
{
    public class PreparePanel
    {
        private CoreGameplay _coreGameplay;

        public PreparePanel(CoreGameplay coreGameplay)
        {
            _coreGameplay = coreGameplay;
        }

        public void GoBack()
        {
            Debug.Log("PreparePanel::GoBack");
            _coreGameplay.Close();
        }
    }

    public class PreparePanelFactory : IFactory<CoreGameplay, PreparePanel>
    {
        public PreparePanel Create(CoreGameplay coreGameplay)
        {
            return new PreparePanel(coreGameplay);
        }
    }
}