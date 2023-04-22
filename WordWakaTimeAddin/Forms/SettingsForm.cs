﻿using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WakaTime.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly Shared.ExtensionUtils.WakaTime _wakaTime;
        internal event EventHandler ConfigSaved;

        public SettingsForm(ref Shared.ExtensionUtils.WakaTime wakaTime)
        {
            _wakaTime = wakaTime;
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                txtAPIKey.Text = _wakaTime.Config.ApiKey;
                txtProxy.Text = _wakaTime.Config.Proxy;
                chkDebugMode.Checked = _wakaTime.Config.Debug;
            }
            catch (Exception ex)
            {
                _wakaTime.Logger.Error("Error when loading form SettingsForm:", ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                var matched = Regex.IsMatch(txtAPIKey.Text.Trim(), "(?im)^(waka_)?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}$");         
                                     
                if (matched)
                {
                    _wakaTime.Config.ApiKey = txtAPIKey.Text.Trim();
                    _wakaTime.Config.Proxy = txtProxy.Text.Trim();
                    _wakaTime.Config.Debug = chkDebugMode.Checked;
                    _wakaTime.Config.Save();
                    OnConfigSaved();
                }
                else
                {
                    MessageBox.Show(@"Please enter valid Api Key.");
                    DialogResult = DialogResult.None; // do not close dialog box
                }
            }
            catch (Exception ex)
            {
                _wakaTime.Logger.Error("Error when saving data from SettingsForm:", ex);
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual void OnConfigSaved()
        {
            var handler = ConfigSaved;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
