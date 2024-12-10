using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace Systeme_solaire
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la sélection de la planète ou de la lune
            string selectedBody = (PlanetComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(selectedBody))
            {
                MessageBox.Show("Veuillez sélectionner une planète ou une lune.");
                return;
            }

            // Récupérer les données de la planète ou lune sélectionnée
            await GetPlanetData(selectedBody.ToLower());
        }

        private async Task GetPlanetData(string bodyName)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"https://api.le-systeme-solaire.net/rest/bodies/{bodyName}");

                // Log pour vérifier la réponse
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Réponse brute de l'API : " + content);

                if (response.IsSuccessStatusCode)
                {
                    if (string.IsNullOrEmpty(content))
                    {
                        MessageBox.Show("Aucune donnée reçue.");
                        return;
                    }

                    Root root = JsonConvert.DeserializeObject<Root>(content);

                    if (root != null && root.Properties != null)
                    {
                        // Mise à jour de l'interface utilisateur avec les données extraites
                        UpdateUIWithPlanetData(root.Properties);
                    }
                    else
                    {
                        MessageBox.Show("Les données de la planète ne sont pas disponibles.");
                    }
                }
                else
                {
                    MessageBox.Show($"Erreur de récupération des données : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
        private void PlanetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Cette méthode est appelée lorsque l'utilisateur sélectionne un élément dans le ComboBox
            // Si vous voulez effectuer une action lorsqu'un choix est effectué, vous pouvez le faire ici

            // Par exemple, vous pouvez afficher la planète sélectionnée dans la console :
            string selectedBody = (PlanetComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            Console.WriteLine($"Planète ou Lune sélectionnée : {selectedBody}");
        }

        private void UpdateUIWithPlanetData(Properties properties)
        {
            try
            {
                NameTextBlock.Text = properties.name?.type ?? "Inconnu";
                KnownCountTextBlock.Text = properties.knownCount?.Value.ToString() ?? "0";
                MassTextBlock.Text = $"{properties.mass?.properties?.massValue?.type ?? "0"} x 10^{properties.mass?.properties?.massExponent?.type ?? "0"} kg";
                GravityTextBlock.Text = properties.gravity?.type ?? "0 m/s²";
                AvgTempTextBlock.Text = $"{properties.avgTemp?.type ?? "0"} K";

                // Affichage des lunes si elles existent
                if (properties.moons != null && properties.moons.items != null)
                {
                    Moon1TextBlock.Text = properties.moons.items.properties?.name?.type ?? "Pas de lune";
                }
                else
                {
                    Moon1TextBlock.Text = "Aucune lune disponible";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la mise à jour de l'UI : {ex.Message}");
            }
        }

        public class _200
        {
            public string description { get; set; }
            public Schema schema { get; set; }
        }

        public class AlternativeName
        {
            public string type { get; set; }
        }

        public class Aphelion
        {
            public string type { get; set; }
        }

        public class ArgPeriapsis
        {
            public string type { get; set; }
        }

        public class AroundPlanet
        {
            public string type { get; set; }
            public Properties properties { get; set; }
        }

        public class AvgTemp
        {
            public string type { get; set; }
        }

        public class AxialTilt
        {
            public string type { get; set; }
        }

        public class Bodies
        {
            public Get get { get; set; }
        }

        public class Bodies2
        {
            public string type { get; set; }
            public Items items { get; set; }
        }

        public class BodiesId
        {
            public Get get { get; set; }
        }

        public class BodyType
        {
            public string type { get; set; }
        }

        public class Density
        {
            public string type { get; set; }
        }

        public class Dimension
        {
            public string type { get; set; }
        }

        public class DiscoveredBy
        {
            public string type { get; set; }
        }

        public class DiscoveryDate
        {
            public string type { get; set; }
        }

        public class Eccentricity
        {
            public string type { get; set; }
        }

        public class EnglishName
        {
            public string type { get; set; }
        }

        public class EquaRadius
        {
            public string type { get; set; }
        }

        public class Escape
        {
            public string type { get; set; }
        }

        public class Flattening
        {
            public string type { get; set; }
        }

        public class Get
        {
            public List<string> tags { get; set; }
            public string summary { get; set; }
            public List<Parameter> parameters { get; set; }
            public Responses responses { get; set; }
        }

        public class Gravity
        {
            public string type { get; set; }
        }

        public class Id
        {
            public string type { get; set; }
        }

        public class Inclination
        {
            public string type { get; set; }
        }

        public class Info
        {
            public string title { get; set; }
            public string description { get; set; }
            public string version { get; set; }
        }

        public class IsPlanet
        {
            public string type { get; set; }
        }

        public class Items
        {
            public string type { get; set; }
            public Properties properties { get; set; }
        }

        public class Knowncount
        {
            public Get get { get; set; }
            public int Value { get; set; }
        }

        public class Knowncount2
        {
            public string type { get; set; }
            public Items items { get; set; }
        }

        public class KnownCount3
        {
            public string type { get; set; }
        }

        public class KnowncountId
        {
            public Get get { get; set; }
        }

        public class KnownCount
        {
            public int Value { get; set; }
            public string Description { get; set; }
        }

        public class LongAscNode
        {
            public string type { get; set; }
        }

        public class MainAnomaly
        {
            public string type { get; set; }
        }

        public class Mass
        {
            public string type { get; set; }
            public Properties properties { get; set; }
        }

        public class MassExponent
        {
            public string type { get; set; }
        }

        public class MassValue
        {
            public string type { get; set; }
        }

        public class MeanRadius
        {
            public string type { get; set; }
        }

        public class Moon
        {
            public string type { get; set; }
        }

        public class Moons
        {
            public string type { get; set; }
            public Items items { get; set; }
        }

        public class Name
        {
            public string type { get; set; }
        }

        public class Parameter
        {
            public string name { get; set; }
            public string @in { get; set; }
            public string description { get; set; }
            public bool required { get; set; }
            public string type { get; set; }
            public string collectionFormat { get; set; }
            public Items items { get; set; }
            public List<string> @enum { get; set; }
        }

        public class Paths
        {
            [JsonProperty("/bodies")]
            public Bodies bodies { get; set; }

            [JsonProperty("/bodies/{id}")]
            public BodiesId bodiesid { get; set; }

            [JsonProperty("/knowncount")]
            public Knowncount knowncount { get; set; }

            [JsonProperty("/knowncount/{id}")]
            public KnowncountId knowncountid { get; set; }
        }

        public class Perihelion
        {
            public string type { get; set; }
        }

        public class Planet
        {
            public string type { get; set; }
        }

        public class PolarRadius
        {
            public string type { get; set; }
        }

        public class Properties
        {
            public Bodies bodies { get; set; }
            public VolValue volValue { get; set; }
            public VolExponent volExponent { get; set; }
            public Planet planet { get; set; }
            public Rel rel { get; set; }
            public Knowncount knowncount { get; set; }
            public Id id { get; set; }
            public KnownCount knownCount { get; set; }
            public UpdateDate updateDate { get; set; }
            public Name name { get; set; }
            public EnglishName englishName { get; set; }
            public IsPlanet isPlanet { get; set; }
            public Moons moons { get; set; }
            public SemimajorAxis semimajorAxis { get; set; }
            public Perihelion perihelion { get; set; }
            public Aphelion aphelion { get; set; }
            public Eccentricity eccentricity { get; set; }
            public Inclination inclination { get; set; }
            public Mass mass { get; set; }
            public Vol vol { get; set; }
            public Density density { get; set; }
            public Gravity gravity { get; set; }
            public Escape escape { get; set; }
            public MeanRadius meanRadius { get; set; }
            public EquaRadius equaRadius { get; set; }
            public PolarRadius polarRadius { get; set; }
            public Flattening flattening { get; set; }
            public Dimension dimension { get; set; }
            public SideralOrbit sideralOrbit { get; set; }
            public SideralRotation sideralRotation { get; set; }
            public AroundPlanet aroundPlanet { get; set; }
            public DiscoveredBy discoveredBy { get; set; }
            public DiscoveryDate discoveryDate { get; set; }
            public AlternativeName alternativeName { get; set; }
            public AxialTilt axialTilt { get; set; }
            public AvgTemp avgTemp { get; set; }
            public MainAnomaly mainAnomaly { get; set; }
            public ArgPeriapsis argPeriapsis { get; set; }
            public LongAscNode longAscNode { get; set; }
            public BodyType bodyType { get; set; }
            public Moon moon { get; set; }
            public MassValue massValue { get; set; }
            public MassExponent massExponent { get; set; }
        }

        public class Rel
        {
            public string type { get; set; }
        }

        public class Responses
        {
            [JsonProperty("200")]
            public _200 _200 { get; set; }
        }

        public class Root
        {
            public string swagger { get; set; }
            public Info info { get; set; }
            public string host { get; set; }
            public string basePath { get; set; }
            public List<string> schemes { get; set; }
            public List<string> consumes { get; set; }
            public List<string> produces { get; set; }
            public List<Tag> tags { get; set; }
            public Paths paths { get; set; }
            public Properties Properties { get; set; }
        }

        public class Schema
        {
            public string type { get; set; }
            public Properties properties { get; set; }
        }

        public class SemimajorAxis
        {
            public string type { get; set; }
        }

        public class SideralOrbit
        {
            public string type { get; set; }
        }

        public class SideralRotation
        {
            public string type { get; set; }
        }

        public class Tag
        {
            public string name { get; set; }
            public string description { get; set; }
        }

        public class UpdateDate
        {
            public string type { get; set; }
        }

        public class Vol
        {
            public string type { get; set; }
            public Properties properties { get; set; }
        }

        public class VolExponent
        {
            public string type { get; set; }
        }

        public class VolValue
        {
            public string type { get; set; }
        }
    }
}