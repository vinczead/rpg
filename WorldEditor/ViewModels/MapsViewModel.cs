using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WorldEditor.DataAccess;

namespace WorldEditor.ViewModels
{
    public class MapsViewModel : ItemsViewModel<RegionViewModel>
    {
        public MapsViewModel(WorldRepository worldRepository) : base(worldRepository)
        {
            CreateMaps();
            worldRepository.Maps.MapAdded += Maps_MapAdded;
        }

        private void Maps_MapAdded(object sender, Utility.EntityEventArgs<Common.Models.Region> e)
        {
            var mapViewModel = new RegionViewModel(e.Entity);
            Items.Add(mapViewModel);
        }

        private void CreateMaps()
        {
            var maps = WorldRepository.Maps.GetMaps().Select(map => new RegionViewModel(map)).ToList();

            Items = new ObservableCollection<RegionViewModel>(maps);
        }

        protected override void ExecuteAddItem()
        {
            WorldRepository.Maps.AddNewMap();
        }

        protected override void ExecuteRemoveItem()
        {
            WorldRepository.Maps.RemoveAt(SelectedIndex);
        }
    }
}
