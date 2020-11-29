using Common.Models;
using Common.Script.Utility;
using Common.Script.Visitors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WorldEditor.ViewModels
{
    public class BreedViewModel : ViewModelBase
    {
        public Thing Thing { get; private set; }
        public RelayCommand<Window> SaveBreed { get; set; }
        public RelayCommand<TextEditor> GoToError { get; set; }

        public BreedViewModel(Thing thing)
        {
            if (thing != null)
            {
                Thing = thing;
                Id = Thing.Id;
                Type = Thing.GetType().Name;
                Script = Thing.Serialize();
            }
            Messages = new List<Error>();
            SaveBreed = new RelayCommand<Window>(ExecuteSaveBreedCommand);
            GoToError = new RelayCommand<TextEditor>(ExecuteGoToErrorCommand);
        }

        private void ExecuteSaveBreedCommand(Window window)
        {
            if (Thing != null)
                World.Instance.Breeds.Remove(Thing.Id); //Temporarily remove old Breed from World

            Messages = ErrorVisitor.CheckBreedScriptErrors(Script, World.Instance.ToScope());
            if (Messages.Count(message => message.Severity == ErrorSeverity.Error) == 0)
            {
                var createdBreed = ExecutionVisitor.BuildBreed(Script, out var executionErrors);
                if (executionErrors.Count > 0 || createdBreed == null)
                {
                    Messages = executionErrors;
                    if (createdBreed != null)
                        World.Instance.Breeds.Remove(createdBreed.Id); //remove newly inserted breed from World
                    if (Thing != null)
                        World.Instance.Breeds.Add(Thing.Id, Thing); //Reinsert old breed to World
                    MessageBox.Show("Failed to save Breed.");
                }
                else
                {
                    Id = createdBreed.Id;
                    type = createdBreed.GetType().Name;
                    Script = createdBreed.Serialize();
                    if (Thing == null)
                    {
                        window.DialogResult = true;
                    }
                    else
                    {
                        foreach (var instance in World.Instance.Instances)
                        {
                            if (instance.Value.Breed == Thing)
                            {
                                instance.Value.Breed = createdBreed;    //todo: should update $ID texts in scripts?
                            }
                        }
                    }
                    Thing = createdBreed;
                    window.Close();
                }
            }
            else
            {
                if (Thing != null)
                    World.Instance.Breeds.Add(Thing.Id, Thing); //Reinsert old breed to World
            }
        }

        private void ExecuteGoToErrorCommand(TextEditor editor)
        {
            var offset = editor.Document.GetOffset(new TextLocation(SelectedItem.Line, SelectedItem.Column));
            editor.CaretOffset = offset;
            editor.ScrollToLine(SelectedItem.Line);
            editor.Focus();
        }

        private string id;

        public string Id
        {
            get => id;
            set => Set(ref id, value);
        }

        private string type;

        public string Type
        {
            get => type;
            set => Set(ref type, value);
        }

        private string script;

        public string Script
        {
            get => script;
            set => Set(ref script, value);
        }
        public Error SelectedItem { get; set; }

        private List<Error> messages;

        public List<Error> Messages
        {
            get => messages;
            set => Set(ref messages, value);
        }

    }
}
