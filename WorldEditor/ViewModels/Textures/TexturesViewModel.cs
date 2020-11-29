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
        public MainViewModel MainViewModel { get; set; }
        public TexturesViewModel(MainViewModel mainViewModel) : base()
        {
            MainViewModel = mainViewModel;
            ReloadItems();
        }
        protected override void ReloadItems()
        {
            var textures = World.Instance.Textures.Values.Select(texture => new TextureViewModel(texture, this)).ToList();

            Items = new ObservableCollection<TextureViewModel>(textures);
        }

        protected override void ExecuteAddItem()
        {
            new TextureEditWindow()
            {
                DataContext = new TextureViewModel(null, this)
            }.ShowDialog();
        }

        protected override void ExecuteEditItem()
        {
            new TextureEditWindow()
            {
                DataContext = new TextureViewModel(SelectedItem.Texture, this)
            }.ShowDialog();
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
