using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DefenderTool
{
    internal static class Program
    {
        // Configuration de l'application (Private CLass)
        private const string CurrentVersion = "1.2";
        private const string CurrentLanguage = "EN-US";
        private const string VersionUrl = "https://raw.githubusercontent.com/danbenba/DefenderTool/refs/heads/project/version";
        private const string ReleaseUrl = "https://github.com/danbenba/DefenderTool/releases/latest";

        // Set configuration to public
        public const string Version = CurrentVersion;
        public const string Language = CurrentLanguage;

        // Liste des ressources à extraire avec leurs noms de ressources incorporées
        private static readonly (string ResourceName, string FileName)[] ResourcesToExtract = new[]
        {
            ("DefenderTool.Resources.enable.bat", "enable.bat"),
            ("DefenderTool.Resources.disable.bat", "disable.bat"),
            ("DefenderTool.Resources.defrv.exe", "defrv.exe"),
            ("DefenderTool.Resources.images.icon.ico", "icon.ico")
        };

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Exécuter la vérification de version de manière synchrone
            if (CheckForUpdates().GetAwaiter().GetResult())
            {
                // Extraire les ressources avant de lancer l'application
                ExtractResources();

                Application.Run(new MainForm());
            }
            else
            {
                // La vérification a échoué ou une mise à jour est disponible
                // L'action appropriée a déjà été effectuée dans CheckForUpdates()
                // Vous pouvez éventuellement fermer l'application ici
            }
        }

        private static async Task<bool> CheckForUpdates()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Récupérer le contenu du fichier de version
                    string latestVersion = await client.GetStringAsync(VersionUrl);
                    latestVersion = latestVersion.Trim(); // Supprimer les espaces blancs

                    if (latestVersion == CurrentVersion)
                    {
                        // La version est à jour
                        return true;
                    }
                    else
                    {
                        // Une mise à jour est disponible
                        // Vérifier si l'URL de release est accessible
                        if (await IsUrlReachable(ReleaseUrl))
                        {
                            DialogResult result = MessageBox.Show(
                                $"Une nouvelle version ({latestVersion}) est disponible. Voulez-vous la télécharger ?",
                                "Mise à jour disponible",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information
                            );

                            if (result == DialogResult.Yes)
                            {
                                // Ouvrir le navigateur par défaut vers l'URL de la release
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = ReleaseUrl,
                                    UseShellExecute = true
                                });
                            }
                            if (result == DialogResult.No)
                            {
                                // Continuer à lancer l'application même si une mise à jour est disponible
                                // Vous pouvez choisir de ne pas lancer l'application si nécessaire
                                return true;
                            }
                        }
                        else
                        {
                            // L'URL de release n'est pas accessible
                            MessageBox.Show(
                                "Une mise à jour est disponible, mais l'URL de téléchargement est inaccessible.",
                                "Erreur de mise à jour",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }

                        // Continuer à lancer l'application même si une mise à jour est disponible
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions (par exemple, problèmes de connexion)
                MessageBox.Show(
                    $"Erreur lors de la vérification des mises à jour : {ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return true;
            }
        }

        private static async Task<bool> IsUrlReachable(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Effectuer une requête HEAD pour vérifier la disponibilité
                    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extrait toutes les ressources définies dans ResourcesToExtract vers le répertoire temporaire.
        /// Si le fichier existe déjà, il sera remplacé.
        /// </summary>
        private static void ExtractResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string tempPath = Path.GetTempPath();

            foreach (var (ResourceName, FileName) in ResourcesToExtract)
            {
                try
                {
                    using (Stream resourceStream = assembly.GetManifestResourceStream(ResourceName))
                    {
                        if (resourceStream == null)
                        {
                            MessageBox.Show($"Erreur : La ressource '{ResourceName}' est introuvable.", "Erreur de ressource", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        string filePath = Path.Combine(tempPath, FileName);

                        // Copier le fichier de ressource dans le répertoire temporaire (écrase s'il existe)
                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            resourceStream.CopyTo(fileStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'extraction de la ressource '{ResourceName}' : {ex.Message}", "Erreur d'extraction", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
