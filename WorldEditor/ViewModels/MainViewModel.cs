using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WorldEditor.Utility;

namespace WorldEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ToolType selectedTool;
        public ToolType SelectedTool
        {
            get => selectedTool;
            set => Set(ref selectedTool, value);
        }

        public RelayCommand<ToolType> SetTool { get; }
        public RelayCommand<Window> Close { get;  }

        public MainViewModel()
        {
            SetTool = new RelayCommand<ToolType>(tool => SelectedTool = tool);
            Close = new RelayCommand<Window>(ExecuteCloseWindow);
        }

        private void ExecuteCloseWindow(Window window)
        {
            if (window != null)
                window.Close();
        }
    }
}
