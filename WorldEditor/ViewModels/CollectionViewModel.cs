﻿using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using WorldEditor.Views;

namespace WorldEditor.ViewModels
{
    public abstract class CollectionViewModel<T>: ViewModelBase where T: ViewModelBase
    {
        private ObservableCollection<T> items;
        public virtual ObservableCollection<T> Items { get => items; set => Set(ref items, value); }

        private T selectedItem;
        public T SelectedItem {
            get => selectedItem;
            set
            {
                Set(ref selectedItem, value);
                RaisePropertyChanged(() => IsItemSelected);
                EditItem.RaiseCanExecuteChanged();
                RemoveItem.RaiseCanExecuteChanged();
            }
        }

        public bool IsItemSelected { get => SelectedItem != null; }

        public RelayCommand AddItem { get; }
        public RelayCommand EditItem { get; }
        public RelayCommand RemoveItem { get; }

        public CollectionViewModel()
        {
            AddItem = new RelayCommand(ExecuteAddItem);
            EditItem = new RelayCommand(ExecuteEditItem, () => IsItemSelected);
            RemoveItem = new RelayCommand(ExecuteRemoveItem, () => IsItemSelected);
        }

        protected abstract void ReloadItems();

        protected abstract void ExecuteAddItem();

        protected abstract void ExecuteEditItem();

        protected abstract void ExecuteRemoveItem();
    }
}
