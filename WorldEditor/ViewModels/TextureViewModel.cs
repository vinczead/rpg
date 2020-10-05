using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using WorldEditor.DataAccess;

namespace WorldEditor.ViewModels
{
    public class TextureViewModel : ViewModelBase
    {
        readonly RpgTexture texture;

        public TextureViewModel(RpgTexture texture)
        {
            this.texture = texture ?? throw new ArgumentNullException("texture");
            BrowseImage = new RelayCommand(ExecuteBrowseImage);
        }

        public string Id
        {
            get => texture.Id;
            set
            {
                if (value == texture.Id)
                    return;
                texture.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public byte[] Texture
        {
            get => texture.Texture2D;
            set
            {
                if (value == texture.Texture2D)
                    return;
                texture.Texture2D = value;
                RaisePropertyChanged("Texture");
                RaisePropertyChanged("HasTextureValue");
            }
        }

        public bool HasTextureValue
        {
            get => Texture != null;
        }

        public RelayCommand BrowseImage { get; }

        public void ExecuteBrowseImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PNG files|*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                Texture = File.ReadAllBytes(openFileDialog.FileName);
            }
        }
    }
}
