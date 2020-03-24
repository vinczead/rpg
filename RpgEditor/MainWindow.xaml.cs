using System;
using System.Collections.Generic;
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
using ICSharpCode.AvalonEdit.Highlighting;
using Rpg.Models;

namespace RpgEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        }

        private void CheckForErrorsClick(object sender, RoutedEventArgs e)
        {
            var syntaxErrors = new List<GameScript.Model.Error>();
            var tree = Executer.ReadAST(textEditor.Text, out syntaxErrors);
            var errorVisitor = Executer.CheckErrors(tree);
            errorList.Text = "";
            
            foreach (var error in syntaxErrors.Concat(errorVisitor.errors))
            {
                errorList.AppendText(error.ToString() + Environment.NewLine);
            }
            MessageBox.Show("Error check finished.");
        }
    }
}
