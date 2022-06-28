using Behc.Mvp.Models;

namespace Features.Units.Models
{
    public class UnitsPanel
    {
        public readonly DataCollection<UnitTile> UnitsCollection;
        
        public UnitsPanel()
        {
            UnitsCollection = new DataCollection<UnitTile>();
        }
    }
}