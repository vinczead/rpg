using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using GameScript;
using GameScript.Visitors;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;

namespace RpgEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<ScriptFile> files = new ObservableCollection<ScriptFile>();

        public MainWindow()
        {
            //Load highlighting definition
            IHighlightingDefinition vigasHighlighting;
            using (Stream s = typeof(MainWindow).Assembly.GetManifestResourceStream("RpgEditor.ViGaSHighlighting.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    vigasHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            HighlightingManager.Instance.RegisterHighlighting("ViGaS", new string[] { ".vgs" }, vigasHighlighting);

            InitializeComponent();

            FilesTabControl.ItemsSource = files;
        }

        private void CheckForErrorsClick(object sender, RoutedEventArgs e)
        {
            var errors = Executer.CheckErrors(files.Select(f => f.Document.Text).ToList());
            messages.Text = "";

            foreach (var error in errors)
            {
                messages.AppendText(error + Environment.NewLine);
            }
            messages.AppendText($"Error check completed, {errors.Count} error(s) were found.");
        }

        private void BuildWorldClick(object sender, RoutedEventArgs e)
        {
            var errors = Executer.CheckErrors(files.Select(f => f.Document.Text).ToList());

            messages.Text = "";

            if (errors.Count > 0)
            {
                messages.AppendText("Scripts contain errors, aborting world building...");
                return;
            }

            messages.AppendText("Building world...\n");

            var world = Executer.BuildWorld(files.Select(f => f.Document.Text).ToList());

            messages.AppendText("World builded successfully.");
        }

        private void AddNewScript(object sender, RoutedEventArgs e)
        {
            files.Add(new ScriptFile()
            {
                ShortFileName = $"File {files.Count}",
                Document = new TextDocument()
            });
            FilesTabControl.SelectedIndex = files.Count - 1;
        }
    }
}
