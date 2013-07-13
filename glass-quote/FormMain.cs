using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace glass_quote
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            backgroundWorkerLicense.DoWork += new DoWorkEventHandler(licenseCheck);
            backgroundWorkerLicense.RunWorkerAsync();
        }

        private void licenseCheck(object sender, DoWorkEventArgs e)
        {
            try
            {
                string latitude = "40.7142";        // 40.7142° N for New York
                string longitude = "-74.0064";      // 74.0064° W for New York

                string currentTimeURL = "http://www.earthtools.org/timezone-1.1/" + latitude + "/" + longitude;

                XDocument xmlDoc = XDocument.Load(currentTimeURL);
                var currentTimeQuery = from c in xmlDoc.Descendants("timezone")
                                       select new
                                       {
                                           date = c.Element("isotime").Value
                                       };

                DateTime now = new DateTime();
                foreach (var obj in currentTimeQuery)
                {
                    now = DateTime.ParseExact(obj.date.ToString(), "yyyy-MM-dd HH:mm:ss zzz", null);
                }

                string expiryTimeURL = "http://webservices.indinovatechnologies.com/licensing_service/LicenseService.php";

                xmlDoc = XDocument.Load(expiryTimeURL);
                
                var expiryTimeQuery = from c in xmlDoc.Descendants("client")
                            .Where(i => i.Element("appname").Value == "Glass Quote")
                            select new
                            {
                                date = c.Element("expirydate").Value
                            };

                DateTime lastDate = new DateTime();
                foreach (var obj in expiryTimeQuery)
                {
                    lastDate = DateTime.ParseExact(obj.date.ToString(), "yyyy-MM-dd HH:mm:ss zzz", null);
                }

                int result = DateTime.Compare(lastDate, now);

                if (result != 1)
                {
                    MessageBox.Show("License period expired. Please contact krishan@indinovatechnologies.com", "License error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Exit();
                }
            }
            catch (Exception e1)
            {
                LogHelper.testLogger.Error(e1.ToString());
                // MessageBox.Show("License validation failed. Please contact krishan@indinovatechnologies.com", "License validation failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                // Application.Exit();
            }
        }
    }
}
