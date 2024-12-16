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

namespace SolarSystemApp
{
    public partial class MainWindow : Window
    {
        // URL de l'API
        private const string ApiUrl = "https://api.le-systeme-solaire.net/rest/bodies/";
        private List<Body> allBodies; // Pour stocker toutes les données récupérées

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        // Méthode pour charger les données de l'API
        private async void LoadData()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Appel de l'API
                    var response = await client.GetStringAsync(ApiUrl);
                    var data = JsonConvert.DeserializeObject<Root>(response);

                    // Stocker les données et les afficher dans le DataGrid
                    allBodies = data?.bodies;
                    BodiesListView.ItemsSource = allBodies;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}");
            }
        }

        // Événement déclenché lors de la saisie dans la barre de recherche
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text.ToLower();

            // Filtrer les données selon le texte saisi
            var filteredBodies = allBodies?.Where(body =>
                body.name.ToLower().Contains(searchText) ||
                body.englishName.ToLower().Contains(searchText) ||
                body.bodyType.ToLower().Contains(searchText)).ToList();

            // Actualiser la source du DataGrid
            BodiesListView.ItemsSource = filteredBodies;
        }

        // Événement déclenché lors de la sélection d'un élément dans le DataGrid
        private void BodiesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BodiesListView.SelectedItem is Body selectedBody)
            {
                // Affichage des détails de l'objet sélectionné
                NameText.Text = $"Nom : {selectedBody.name}";
                EnglishNameText.Text = $"Nom en anglais : {selectedBody.englishName}";
                BodyTypeText.Text = $"Type : {selectedBody.bodyType}";
                GravityText.Text = $"Gravité : {selectedBody.gravity} m/s²";
                RadiusText.Text = $"Rayon moyen : {selectedBody.meanRadius} km";
                DiscoveredByText.Text = $"Découverte par : {selectedBody.discoveredBy}";
                DiscoveryDateText.Text = $"Date de découverte : {selectedBody.discoveryDate}";
            }
        }

        // Événement déclenché lorsque le bouton d'actualisation est cliqué
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    
        public class AroundPlanet
        {
            public string planet { get; set; }
            public string rel { get; set; }
        }

        public class Body
        {
            public string id { get; set; }
            public string name { get; set; }
            public string englishName { get; set; }
            public bool isPlanet { get; set; }
            public object moons { get; set; }
            public long semimajorAxis { get; set; } 
            public long perihelion { get; set; }     
            public long aphelion { get; set; }       
            public double eccentricity { get; set; }
            public double inclination { get; set; }
            public Mass mass { get; set; }
            public Vol vol { get; set; }
            public double density { get; set; }
            public double gravity { get; set; }
            public double escape { get; set; }
            public double meanRadius { get; set; }
            public double equaRadius { get; set; }
            public double polarRadius { get; set; }
            public double flattening { get; set; }
            public string dimension { get; set; }
            public double sideralOrbit { get; set; }
            public double sideralRotation { get; set; }
            public AroundPlanet aroundPlanet { get; set; }
            public string discoveredBy { get; set; }
            public string discoveryDate { get; set; }
            public string alternativeName { get; set; }
            public double axialTilt { get; set; }
            public int avgTemp { get; set; }
            public double? mainAnomaly { get; set; }
            public double? argPeriapsis { get; set; }
            public double? longAscNode { get; set; }
            public string bodyType { get; set; }
            public string rel { get; set; }
        }

        public class Mass
        {
            public double massValue { get; set; }
            public int massExponent { get; set; }
        }

        public class Root
        {
            public List<Body> bodies { get; set; }
        }

        public class Vol
        {
            public double volValue { get; set; }
            public int volExponent { get; set; }
        }
    }
}