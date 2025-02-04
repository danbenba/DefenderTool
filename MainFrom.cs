using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace DefenderTool
{
    public class MainForm : Form
    {
        private Label labelTitle;
        private ComboBox comboBoxScripts;
        private Button btnApply;
        private CheckBox checkBoxRestart;
        private Label labelVersion;
        private Button btnSystemInfo;
        private Button btnAbout;
        private RichTextBox richTextBoxLogs;
        private CheckBox checkBoxAdvanced; // Not used, you can remove it or implement it

        string systemVersion = Environment.OSVersion.VersionString;
        string dotNetVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        string CurrentVersion = Program.Version;

        // Chemins des fichiers extraits
        private readonly string tempPath = Path.GetTempPath();
        private readonly string enableBatPath;
        private readonly string disableBatPath;
        private readonly string defrvExePath;
        private readonly string iconPath;

        public MainForm()
        {
            // Initialiser les chemins des fichiers extraits
            enableBatPath = Path.Combine(tempPath, "enable.bat");
            disableBatPath = Path.Combine(tempPath, "disable.bat");
            defrvExePath = Path.Combine(tempPath, "defrv.exe");
            iconPath = Path.Combine(tempPath, "icon.ico");

            InitializeComponent();
            LoadIconFromFile();
        }

        private void InitializeComponent()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;

            // Initialiser l'icône depuis le fichier extrait
            // L'icône sera chargée dans LoadIconFromFile()

            // Initialize components
            this.labelTitle = new Label();
            this.comboBoxScripts = new ComboBox();
            this.btnApply = new Button();
            this.checkBoxRestart = new CheckBox();
            this.labelVersion = new Label();
            this.btnSystemInfo = new Button();
            this.btnAbout = new Button();
            this.richTextBoxLogs = new RichTextBox();
            this.checkBoxAdvanced = new CheckBox();

            // 
            // labelTitle
            // 
            this.labelTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.labelTitle.Location = new Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new Size(500, 40);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "DefenderTool";
            this.labelTitle.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // comboBoxScripts
            // 
            this.comboBoxScripts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxScripts.Font = new Font("Segoe UI", 12F);
            this.comboBoxScripts.FormattingEnabled = true;
            this.comboBoxScripts.Location = new Point(18, 62);
            this.comboBoxScripts.Name = "comboBoxScripts";
            this.comboBoxScripts.Size = new Size(280, 29);
            this.comboBoxScripts.TabIndex = 1;
            this.comboBoxScripts.SelectedIndexChanged += new EventHandler(this.comboBoxScripts_SelectedIndexChanged);

            // 
            // btnApply
            // 
            this.btnApply.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.btnApply.Location = new Point(312, 59);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new Size(112, 36);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new EventHandler(this.btnApply_Click);

            // 
            // checkBoxRestart
            // 
            this.checkBoxRestart.AutoSize = true;
            this.checkBoxRestart.Font = new Font("Segoe UI", 12F);
            this.checkBoxRestart.Location = new Point(18, 108);
            this.checkBoxRestart.Name = "checkBoxRestart";
            this.checkBoxRestart.Size = new Size(267, 25);
            this.checkBoxRestart.TabIndex = 3;
            this.checkBoxRestart.Text = "Restart PC after execution";
            this.checkBoxRestart.UseVisualStyleBackColor = true;

            // 
            // labelVersion
            // 
            this.labelVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new Font("Segoe UI", 9F);
            this.labelVersion.Location = new Point(15, 386);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new Size(137, 15);
            this.labelVersion.TabIndex = 4;
            this.labelVersion.Text = $"Version {CurrentVersion} - By danbenba";

            // 
            // btnAbout
            // 
            this.btnAbout.Font = new Font("Segoe UI", 10F);
            this.btnAbout.Location = new Point(430, 59);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new Size(85, 36);
            this.btnAbout.TabIndex = 6;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new EventHandler(this.btnAbout_Click);

            // 
            // richTextBoxLogs
            // 
            this.richTextBoxLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.richTextBoxLogs.BackColor = Color.White;
            this.richTextBoxLogs.Font = new Font("Consolas", 10F);
            this.richTextBoxLogs.Location = new Point(18, 155);
            this.richTextBoxLogs.Name = "richTextBoxLogs";
            this.richTextBoxLogs.ReadOnly = true;
            this.richTextBoxLogs.Size = new Size(400, 140);
            this.richTextBoxLogs.TabIndex = 7;
            this.richTextBoxLogs.Text = "";

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(434, 311);
            this.Controls.Add(this.richTextBoxLogs);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.checkBoxRestart);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.comboBoxScripts);
            this.Controls.Add(this.labelTitle);
            this.Font = new Font("Segoe UI", 11F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Defender Tool";
            this.Load += new EventHandler(this.MainForm_Load);
        }

        /// <summary>
        /// Charge l'icône de l'application depuis le fichier extrait.
        /// </summary>
        private void LoadIconFromFile()
        {
            try
            {
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
                else
                {
                    // Gestion d'erreur si l'icône n'est pas trouvée
                    MessageBox.Show("Fichier d'icône introuvable dans le répertoire temporaire.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Gestion d'erreur lors du chargement de l'icône
                MessageBox.Show("Erreur lors du chargement de l'icône : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Initialiser le ComboBox
            comboBoxScripts.Items.Add("Enable W-Defender");
            comboBoxScripts.Items.Add("Disable W-Defender");
            comboBoxScripts.Items.Add("Remove Defender");
            comboBoxScripts.Items.Add("Disable All Security Mitigations");

            comboBoxScripts.SelectedIndex = -1;
            btnApply.Enabled = false;

            // Détecter si l'application est exécutée en tant qu'administrateur
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);

            string adminStatus = isAdmin ? "Running as administrator" : "Not running as administrator";

            labelVersion.Text =
                $"OS:  {systemVersion}" +
                "                                                  " +
                $".NET Runtime: {dotNetVersion}";
            labelTitle.Text = $"Defender Tool v{CurrentVersion}";

            // Afficher les informations de démarrage dans les logs
            AddLog($"[SUCCESS] All functions loaded.\n", Color.Green);
        }

        private void comboBoxScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnApply.Enabled = (comboBoxScripts.SelectedIndex >= 0);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (comboBoxScripts.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un payload.",
                                "Avertissement",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Obtenir le script sélectionné
            string selectedScript = comboBoxScripts.SelectedItem.ToString();

            // Définir le chemin du script en fonction du payload sélectionné
            string scriptFilePath = GetScriptFilePath(selectedScript);
            string fileExtension = Path.GetExtension(scriptFilePath);
            string additionalArguments = "";

            // Définir un message de confirmation spécifique et les arguments supplémentaires si nécessaire
            string confirmationMessage = "";

            switch (selectedScript)
            {
                case "Enable W-Defender":
                    confirmationMessage = "Voulez-vous vraiment ACTIVER Windows Defender ?";
                    break;

                case "Disable W-Defender":
                    confirmationMessage = "Voulez-vous vraiment DÉSACTIVER Windows Defender ?";
                    break;

                case "Remove Defender":
                    confirmationMessage = "La suppression de Windows Defender est une action irréversible. Continuer ?";
                    additionalArguments = "/r"; // Argument spécifique
                    break;

                case "Disable All Security Mitigations":
                    confirmationMessage = "Voulez-vous vraiment désactiver TOUTES les mesures de sécurité (UAC Admin, etc) ?";
                    additionalArguments = "/s"; // Argument spécifique
                    break;
            }

            // Vérifier si le fichier existe
            if (!File.Exists(scriptFilePath))
            {
                MessageBox.Show("Erreur : Le fichier de script est introuvable dans le répertoire temporaire.", "Erreur de fichier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Demander confirmation à l'utilisateur
            DialogResult dr = MessageBox.Show(
                confirmationMessage,
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (dr == DialogResult.No)
            {
                AddLog("[INFO] L'utilisateur a annulé l'opération.\n", Color.DarkOrange);
                return;
            }

            // Exécuter le script
            ExecuteScript(scriptFilePath, fileExtension, additionalArguments, selectedScript);
        }

        /// <summary>
        /// Retourne le chemin du fichier de script/exécutable en fonction du payload sélectionné.
        /// </summary>
        private string GetScriptFilePath(string selectedScript)
        {
            return selectedScript switch
            {
                "Enable W-Defender" => enableBatPath,
                "Disable W-Defender" => disableBatPath,
                "Remove Defender" => defrvExePath,
                "Disable All Security Mitigations" => defrvExePath,
                _ => throw new ArgumentException("Script non reconnu.")
            };
        }

        private void ExecuteScript(string scriptFilePath, string fileExtension, string additionalArguments, string selectedScript)
        {
            try
            {
                AddLog($"[EXEC] Lancement du payload : {selectedScript}\n", Color.DarkCyan);

                string command;
                ProcessStartInfo psi;

                if (fileExtension.Equals(".bat", StringComparison.OrdinalIgnoreCase))
                {
                    // Pour les fichiers batch, exécuter via cmd.exe
                    command = $"/c \"{scriptFilePath}\"";
                    psi = new ProcessStartInfo("cmd.exe", command)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                }
                else if (fileExtension.Equals(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    // Pour les exécutables, exécuter directement avec les arguments
                    command = $"{scriptFilePath} {additionalArguments}";
                    psi = new ProcessStartInfo(scriptFilePath, additionalArguments)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                }
                else
                {
                    MessageBox.Show("Type de fichier non supporté pour l'exécution.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Process process = new Process();
                process.StartInfo = psi;
                process.EnableRaisingEvents = true;

                process.OutputDataReceived += (s, ev) =>
                {
                    if (!string.IsNullOrEmpty(ev.Data))
                    {
                        AddLog($"[OUT] {ev.Data}\n", Color.Gray);
                    }
                };

                process.ErrorDataReceived += (s, ev) =>
                {
                    if (!string.IsNullOrEmpty(ev.Data))
                    {
                        AddLog($"[ERR] {ev.Data}\n", Color.Red);
                    }
                };

                process.Exited += (s, ev) =>
                {
                    if (process.ExitCode == 0)
                    {
                        this.Invoke(new Action(() =>
                        {
                            richTextBoxLogs.Clear();
                            AddLog($"[SUCCESS] Playload Installed !\n", Color.Green);
                            AddLog($"[INFO] Reboot Required !\n", Color.Orange);
                        }));

                        string message = selectedScript switch
                        {
                            "Enable W-Defender" => "Windows Defender a été activé avec succès.",
                            "Disable W-Defender" => "Windows Defender a été désactivé avec succès.",
                            "Remove Defender" => "Windows Defender a été supprimé avec succès.",
                            "Disable All Security Mitigations" => "Toutes les mesures de sécurité ont été désactivées.",
                            _ => "Processus terminé."
                        };

                        Invoke(new Action(() =>
                        {
                            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (checkBoxRestart.Checked)
                            {
                                AddLog("[INFO] Redémarrage en cours...\n", Color.DarkOrange);
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = "shutdown",
                                    Arguments = "-r -t 0",
                                    CreateNoWindow = true,
                                    UseShellExecute = false
                                });
                            }

                            btnApply.Enabled = true;
                        }));
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show($"Le payload a échoué avec le code d'erreur : {process.ExitCode}",
                                            "Erreur",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            btnApply.Enabled = true;
                        }));
                    }

                    process.Dispose();
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                AddLog($"[ERROR] Exception : {ex.Message}\n", Color.Red);
                btnApply.Enabled = true;
            }
        }

        private void btnSystemInfo_Click(object sender, EventArgs e)
        {
            // Résumé système succinct
            string systemVersion = Environment.OSVersion.VersionString;
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);

            string adminStatus = isAdmin
                ? "Running as administrator"
                : "Not running as administrator";

            // Ajouter plus d'informations si nécessaire (par exemple, version .NET)
            string dotNetVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

            MessageBox.Show(
                $"Operating System: {systemVersion}\n" +
                $"{adminStatus}\n" +
                $".NET Runtime: {dotNetVersion}",
                "System Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            // Ouvrir la fenêtre AboutForm
            AboutFrom aboutForm = new AboutFrom();
            aboutForm.ShowDialog(); // Modal
        }

        /// <summary>
        /// Méthode utilitaire pour ajouter un log avec couleur dans le RichTextBox
        /// </summary>
        private void AddLog(string message, Color? color = null)
        {
            if (richTextBoxLogs.InvokeRequired)
            {
                richTextBoxLogs.Invoke(new Action(() => AddLog(message, color)));
            }
            else
            {
                Color c = color ?? Color.Black;
                richTextBoxLogs.SelectionStart = richTextBoxLogs.TextLength;
                richTextBoxLogs.SelectionLength = 0;
                richTextBoxLogs.SelectionColor = c;
                richTextBoxLogs.AppendText(message);
                richTextBoxLogs.SelectionColor = richTextBoxLogs.ForeColor;
                // Défilement automatique vers le bas
                richTextBoxLogs.ScrollToCaret();
            }
        }
    }
}
