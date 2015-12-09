using System;
using System.Windows.Forms;
using UpdateAssemblyInfo.Business;

namespace UpdateAssemblyInfo
{
    public partial class Form1 : Form
    {

        private Settings Settings { get; set; }
        
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
                UpdateAssemblyFileVersion = XuUpdateFileVersion.Checked,

                IncrementAssemblyFileVersion = XuIncrementRevison.Checked,
                AssemblyTrademark = XuAssemblyTrademark.Text,
                
                AssemblyVersion = XuAssemblyVersion.Text,
                UpdateAssemblyVersion = XuUpdateAssemblyVersion.Checked,

                NeutralResourcesLanguageAttribute = XuNeutralLanguage.Text,
                
                AssemblyProduct = XuAssemblyProduct.Text,
                AssemblyProductVersion = XuProductVersion.Text,
                
                UpdateAssemblyProductVersion = XuUpdateProductVersion.Checked
            };

            api.Execute();

            MessageBox.Show(String.Format("Updated {0} files", api.FilesUpdate));

            XuExecute.Enabled = true;
        }

        private void XuClose_Click(object sender, EventArgs e)
        {
            Settings.AssemblyVersion = XuAssemblyVersion.Text;
            Settings.FileVersion = XuAssemblyFileVersion.Text;
            Settings.ProductVersion = XuProductVersion.Text;
            Settings.Save();

            Close();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Settings = new Settings();

            XuAssemblyFileVersion.Text = Settings.FileVersion;
            XuAssemblyVersion.Text = Settings.AssemblyVersion;
            XuProductVersion.Text = Settings.ProductVersion;
        }


        /// <summary>
        /// Update File Version
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XuUpdateFileVersion_CheckedChanged(object sender, EventArgs e)
        {
            XuAssemblyFileVersion.Enabled = XuUpdateFileVersion.Checked;
            
            XuIncrementRevison.Checked = false;
            XuIncrementRevison.Enabled = !(XuUpdateFileVersion.Checked);

            XuUpdateAssemblyVersion.Checked = XuUpdateAssemblyVersion.Checked && XuUpdateFileVersion.Checked;
            XuUpdateAssemblyVersion.Enabled = XuUpdateFileVersion.Checked;
        }


        /// <summary>
        /// XuUpdateAssemblyVersion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XuUpdateVersion_CheckedChanged(object sender, EventArgs e)
        {
            XuUpdateProductVersion.Checked = XuUpdateAssemblyVersion.Checked && XuUpdateProductVersion.Checked;
            XuUpdateProductVersion.Enabled = XuUpdateAssemblyVersion.Checked;
            XuAssemblyVersion.Enabled = XuUpdateAssemblyVersion.Checked;
        }


        /// <summary>
        /// Update Product version
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XuUpdateProductVersion_CheckedChanged(object sender, EventArgs e)
        {
            XuProductVersion.Enabled = XuUpdateProductVersion.Checked;
        }
    }
}
