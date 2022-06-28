using Behc.Utils;

namespace Features.CoreGameplay.Models
{
    public class CoreGameplayPanel
    {
    }
    
    public class CoreGameplayPanelFactory : IFactory<CoreGameplayPanel>
    {
        public CoreGameplayPanel Create()
        {
            return new CoreGameplayPanel();
        }
    }
}