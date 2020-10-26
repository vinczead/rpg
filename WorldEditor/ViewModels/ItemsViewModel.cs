using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WorldEditor.DataAccess;

namespace WorldEditor.ViewModels
{
    public abstract class ItemsViewModel<T>: ViewModelBase where T: ViewModelBase
    {
        public WorldRepository WorldRepository { get; set; }
        public ObservableCollection<T> Items { get; set; }

        private T selectedItem;
        public T SelectedItem
        {
            get => selectedItem;
            set
            {
                Set(ref selectedItem, value);
                RaisePropertyChanged(() => IsItemSelected);
            }
        }

        public bool IsItemSelected { get => SelectedItem != null; }
        public int SelectedIndex { get; set; }
        public RelayCommand AddItem { get; }
        public RelayCommand RemoveItem { get; }

        public ItemsViewModel(WorldRepository worldRepository)
        {
            WorldRepository = worldRepository;
            AddItem = new RelayCommand(ExecuteAddItem);
            RemoveItem = new RelayCommand(ExecuteRemoveItem);
        }

        protected abstract void ExecuteAddItem();

        protected abstract void ExecuteRemoveItem();
    }
}
