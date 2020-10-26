using Common.Models;
using Common.Utility;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WorldEditor.ViewModels
{
    public class ScriptFileViewModel : ViewModelBase
    {
        readonly ScriptFile scriptFile;
        public ScriptFileViewModel(ScriptFile scriptFile)
        {
            this.scriptFile = scriptFile;
        }

        public string Name
        {
            get => scriptFile.Name;
            set
            {
                if (value == Name)
                    return;
                scriptFile.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Text
        {
            get => scriptFile.Text;
            set
            {
                if (value == Text)
                    return;
                scriptFile.Text = value;
                RaisePropertyChanged("Text");
            }
        }
    }
}
