using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using WorldEditor.Utility;

namespace WorldEditor.ViewModels
{
    public class TextureViewModel : ViewModelBase
    {
        public Texture Texture { get; private set; }
        public TexturesViewModel Textures { get; set; }

        public TextureViewModel(Texture texture, TexturesViewModel texturesViewModel)
        {
            Textures = texturesViewModel;
            if (texture != null)
            {
                Texture = texture;
                Id = texture.Id;
                FileName = texture.FileName;
            }
            BrowseImage = new RelayCommand(ExecuteBrowseImage);
            SaveTexture = new RelayCommand<Window>(ExecuteSaveTexture);
        }

        private string id;

        public string Id
        {
            get => id;
            set => Set(ref id, value);
        }

        private string fileName;
        public string FileName
        {
            get => fileName;
            set
            {
                Set(ref fileName, value);
                try
                {
                    ByteArrayValue = File.ReadAllBytes(Path.Combine(World.Instance.FolderPath, value));
                }
                catch
                {
                    MessageBox.Show($"Failed to load File: {FileName}!");
                }
            }
        }

        private byte[] byteArrayValue;
        public byte[] ByteArrayValue
        {
            get => byteArrayValue;
            set => Set(ref byteArrayValue, value);
        }


        public bool HasTextureValue
        {
            get => ByteArrayValue != null;
        }

        public RelayCommand BrowseImage { get; }
        public RelayCommand<Window> SaveTexture { get; }

        public void ExecuteBrowseImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PNG files|*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var relativePath = Path.GetRelativePath(World.Instance.FolderPath, openFileDialog.FileName);
                FileName = relativePath;
            }
        }

        public void ExecuteSaveTexture(Window window)
        {
            try
            {
                if (Texture == null)
                {
                    var textureToAdd = new Texture()
                    {
                        Id = Id,
                        FileName = FileName,
                        ByteArrayValue = ByteArrayValue
                    };
                    World.Instance.Textures.Add(Id, textureToAdd);
                    Textures.Items.Add(this);
                    Texture = textureToAdd;
                    window.DialogResult = true;
                }
                else
                {
                    if (Texture.Id != Id && World.Instance.Textures.ContainsKey(Id))
                        throw new ArgumentException();
                    World.Instance.Textures.Remove(Texture.Id);
                    var originalViewModel = Textures.Items.First(item => item.Id == Texture.Id);

                    Texture.Id = Id;
                    originalViewModel.Id = Id;

                    Texture.FileName = FileName;
                    originalViewModel.FileName = FileName;

                    Texture.ByteArrayValue = ByteArrayValue;
                    originalViewModel.ByteArrayValue = ByteArrayValue;

                    //todo: raisePropertyChanged Textures.Items ?

                    World.Instance.Textures.Add(Id, Texture);
                }
                window.Close();
            }
            catch
            {
                MessageBox.Show("Failed to add Texture! A texture with the same id already exists!");
            }
        }
    }
}
