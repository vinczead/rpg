using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WorldEditor.DataAccess;

namespace WorldEditor.ViewModels
{
    public class MapsViewModel : ItemsViewModel<MapViewModel>
    {
        public MapsViewModel(WorldRepository worldRepository) : base(worldRepository)
        {
            CreateMaps();
            worldRepository.Maps.MapAdded += Maps_MapAdded;
        }

        private void Maps_MapAdded(object sender, Utility.EntityEventArgs<Common.Models.Map> e)
        {
            var mapViewModel = new MapViewModel(e.Entity);
            Items.Add(mapViewModel);
        }

        private void CreateMaps()
        {
            var maps = WorldRepository.Maps.GetMaps().Select(map => new MapViewModel(map)).ToList();

            Items = new ObservableCollection<MapViewModel>(maps);
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
