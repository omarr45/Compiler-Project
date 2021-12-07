using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Language
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void compileBtn_Click(object sender, EventArgs e)
        {
            clearBtn_Click(sender, e);
            Tiny_Language.Start_Compiling(CodeTextBox.Text);
            PrintTokens();
            
            PrintErrors();
        }
        void PrintTokens()
        {
            for (int i = 0; i < Tiny_Language.Tiny_Scanner.Tokens.Count; i++)
            {
                dataGridView1.Rows.Add(Tiny_Language.Tiny_Scanner.Tokens.ElementAt(i).lex, Tiny_Language.Tiny_Scanner.Tokens.ElementAt(i).token_type);
            }
        }

        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                ErrorsTextBox.Text += Errors.Error_List[i];
                ErrorsTextBox.Text += "\r\n";
            }
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            ErrorsTextBox.Clear();
            dataGridView1.Rows.Clear();
            Tiny_Language.TokenStream.Clear();
            Tiny_Language.Tiny_Scanner.Tokens.Clear();
            Errors.Error_List.Clear();
        }
    }
}
