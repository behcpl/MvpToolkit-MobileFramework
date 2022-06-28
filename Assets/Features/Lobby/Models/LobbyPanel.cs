using Behc.Utils;

namespace Features.Lobby.Models
{
    public class LobbyPanel
    {
        
    }

    public class LobbyPanelFactory : IFactory<LobbyPanel>
    {
        public LobbyPanel Create()
        {
            return new LobbyPanel();
        }
    }
}