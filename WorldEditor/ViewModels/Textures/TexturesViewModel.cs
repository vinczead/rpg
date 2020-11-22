using Common.Models;
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
    public class TexturesViewModel : CollectionViewModel<TextureViewModel>
    {
        protected override void RefreshItems()
        {
            var textures = World.Instance.Textures.Values.Select(texture => new TextureViewModel(texture)).ToList();

            Items = new ObservableCollection<TextureViewModel>(textures);
        }

        protected override void ExecuteAddItem()
        {
            var textureViewModel = new TextureViewModel(null);
            var window = new TextureEditWindow()
            {
                DataContext = textureViewModel
            };

            if (window.ShowDialog() == true)
            {
                Items.Add(textureViewModel);
            }
        }

        protected override void ExecuteEditItem()
        {
            new TextureEditWindow()
            {
                DataContext = new TextureViewModel(SelectedItem.Texture)
            }.ShowDialog();
            RefreshItems();
            RaisePropertyChanged("Items");
        }

        protected override void ExecuteRemoveItem()
        {
            var references = World.Instance.Models.Where(model => model.Value.SpriteSheet.Id == SelectedItem.Id).Select(model => model.Value.Id).ToList();

            if (references.Count > 0)
                MessageBox.Show($"{SelectedItem.Id} cannot be removed, because it is referenced {references.Count} times: {string.Join(',', references)}", "Error");
            else
            {
                World.Instance.Textures.Remove(SelectedItem.Id);
                Items.Remove(SelectedItem);
                SelectedItem = null;
            }
        }
    }
}
