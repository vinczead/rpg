using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using WorldEditor.Utility;

namespace WorldEditor.ViewModels
{
    public class ContentsViewModel : ViewModelBase
    {
        private ContentType selectedContentType;
        public ContentType SelectedContentType
        {
            get => selectedContentType;
            set => Set(ref selectedContentType, value);
        }
    }

}
