using System;
using System.Windows.Forms;
using UpdateAssemblyInfo.Business;

namespace UpdateAssemblyInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void XuExecute_Click(object sender, EventArgs e)
        {
            XuExecute.Enabled = false;

            var api = new AssemblyInfoApi
            {
                AssemblyCompany = XuAssemblyCompany.Text,
                AssemblyCopyright = XuAssemblyCopyright.Text,
                AssemblyFileVersion = XuAssemblyFileVersion.Text,
                AssemblyTrademark = XuAssemblyTrademark.Text,
                AssemblyVersion = XuAssemblyVersion.Text,
                NeutralResourcesLanguageAttribute = XuNeutralLanguage.Text,
                AssemblyProduct = XuAssemblyProduct.Text
            };

            api.Execute();

            XuExecute.Enabled = true;
        }

        private void XuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XuAssemblyFileVersion.Text = String.Format("{0:yy}.{1:00}.01",DateTime.Now,DateTime.Now.Month+1);
        }
    }
}
