using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace DefenderTool
{
    public class AboutFrom : Form
    {
        private PictureBox pictureBoxLogo;
        private Label lblTitle;
        private Label lblDescription;
        private Label lblPrecaution;
        private Button btnOk;
        private Button btnLearnMore;
        private TableLayoutPanel mainLayout;
        private TableLayoutPanel buttonLayout;
        string CurrentVersion = Program.Version;
        string Language = Program.Language;

        public AboutFrom()
        {
            InitializeComponent();
            LoadLogoImage();
        }

        private void InitializeComponent()
        {
            // Propriétés de la Form
            this.Text = "À propos de DefenderTool";
            this.Size = new Size(600, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Initialisation du TableLayoutPanel principal
            mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 2;
            mainLayout.RowCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F)); // Colonne pour l'image
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F)); // Colonne pour les textes et boutons
            this.Controls.Add(mainLayout);

            // PictureBox pour le logo
            pictureBoxLogo = new PictureBox();
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.Dock = DockStyle.Fill;
            mainLayout.Controls.Add(pictureBoxLogo, 0, 0);

            // Panel pour les textes et les boutons
            var textAndButtonsPanel = new TableLayoutPanel();
            textAndButtonsPanel.Dock = DockStyle.Fill;
            textAndButtonsPanel.ColumnCount = 1;
            textAndButtonsPanel.RowCount = 3;
            textAndButtonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60F)); // Pour les textes
            textAndButtonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F)); // Pour les boutons
            textAndButtonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F)); // Pour la précaution
            mainLayout.Controls.Add(textAndButtonsPanel, 1, 0);

            // Initialisation du panel pour les textes
            var textsPanel = new TableLayoutPanel();
            textsPanel.Dock = DockStyle.Fill;
            textsPanel.ColumnCount = 1;
            textsPanel.RowCount = 2;
            textsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Titre
            textsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Description
            textAndButtonsPanel.Controls.Add(textsPanel, 0, 0);

            // Titre
            lblTitle = new Label();
            lblTitle.Text = $"Defender Tool";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Dock = DockStyle.Fill;
            textsPanel.Controls.Add(lblTitle, 0, 0);

            // Description
            lblDescription = new Label();
            lblDescription.Text = $"Développé par danbenba.\n\nWindows Defender Tool est un outil pour windows defender.\nVersion {CurrentVersion} ({Language} Version)";
            lblDescription.Font = new Font("Arial", 10);
            lblDescription.AutoSize = true;
            lblDescription.TextAlign = ContentAlignment.MiddleLeft;
            lblDescription.Dock = DockStyle.Fill;
            textsPanel.Controls.Add(lblDescription, 0, 1);

            // Initialisation du panel pour les boutons
            buttonLayout = new TableLayoutPanel();
            buttonLayout.Dock = DockStyle.Fill;
            buttonLayout.ColumnCount = 2;
            buttonLayout.RowCount = 1;
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            buttonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            textAndButtonsPanel.Controls.Add(buttonLayout, 0, 1);

            // Bouton Learn More
            btnLearnMore = new Button();
            btnLearnMore.Text = "Learn More";
            btnLearnMore.Size = new Size(100, 30);
            btnLearnMore.Anchor = AnchorStyles.Right;
            btnLearnMore.Click += BtnLearnMore_Click;
            buttonLayout.Controls.Add(btnLearnMore, 0, 0);

            // Bouton OK
            btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Size = new Size(100, 30);
            btnOk.Anchor = AnchorStyles.Left;
            btnOk.Click += BtnOk_Click;
            buttonLayout.Controls.Add(btnOk, 1, 0);

            // Label pour la précaution
            lblPrecaution = new Label();
            lblPrecaution.Text = "     Attention : utilisez ce programme avec prudence.   ";
            lblPrecaution.Font = new Font("Arial", 10, FontStyle.Bold);
            lblPrecaution.ForeColor = Color.Red;
            lblPrecaution.Dock = DockStyle.Fill;
            lblPrecaution.TextAlign = ContentAlignment.MiddleCenter;
            textAndButtonsPanel.Controls.Add(lblPrecaution, 0, 2);
        }

        /// <summary>
        /// Charge l'image du logo à partir des resources locales.
        /// </summary>
        private void LoadLogoImage()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                // Le nom de la ressource doit inclure le namespace et le chemin relatif au dossier Resources
                using (Stream imageStream = assembly.GetManifestResourceStream("DefenderTool.Resources.images.icon.png"))
                {
                    if (imageStream != null)
                    {
                        pictureBoxLogo.Image = Image.FromStream(imageStream);
                    }
                    else
                    {
                        MessageBox.Show("L'image du logo n'a pas été trouvée dans les ressources intégrées.", "Erreur",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible de charger le logo JetBrains : {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Optionnel : Charger une image par défaut ou laisser le PictureBox vide
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnLearnMore_Click(object sender, EventArgs e)
        {
            string githubUrl = "https://github.com/danbenba"; // Remplacez par votre véritable lien GitHub
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = githubUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d'ouvrir le lien GitHub : {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
