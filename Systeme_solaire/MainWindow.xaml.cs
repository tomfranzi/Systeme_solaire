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
        private bool isDataLoaded = false; // Indique si les données sont chargées

        public MainWindow()
        {
            InitializeComponent();

            // Charger les données avant d'attacher les événements
            LoadData();

            // Attacher les événements après l'initialisation
            BodyTypeFilter.SelectionChanged += BodyTypeFilter_SelectionChanged;
        }

        // Méthode pour charger les données de l'API
        private async void LoadData()
        {
            BodyTypeFilter.IsEnabled = false; // Désactiver la ComboBox avant le chargement

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(ApiUrl);
                    var data = JsonConvert.DeserializeObject<Root>(response);

                    allBodies = data?.bodies;
                    BodiesListView.ItemsSource = allBodies; // Initialiser la DataGrid
                }

                isDataLoaded = true; // Marquer les données comme chargées
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}");
            }
            finally
            {
                BodyTypeFilter.IsEnabled = true; // Réactiver la ComboBox après le chargement
            }
        }

        // Événement déclenché lors de la saisie dans la barre de recherche
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isDataLoaded || BodiesListView == null)
            {
                return;
            }

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
                MassText.Text = $"Masse : {selectedBody.mass?.massValue} x 10^{selectedBody.mass?.massExponent} kg";
                VolumeText.Text = $"Volume : {selectedBody.vol?.volValue} x 10^{selectedBody.vol?.volExponent} km³";
                DensityText.Text = $"Densité : {selectedBody.density} g/cm³";
                DiscoveredByText.Text = $"Découverte par : {selectedBody.discoveredBy}";
                DiscoveryDateText.Text = $"Date de découverte : {selectedBody.discoveryDate}";
            }
        }

        // Filtrage par type de corps céleste
        private void BodyTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isDataLoaded || BodiesListView == null)
            {
                MessageBox.Show("Les données ne sont pas encore chargées ou l'interface utilisateur n'est pas prête.");
                return;
            }

            if (BodyTypeFilter.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedType = selectedItem.Content.ToString();

                // Filtrer les corps célestes selon le type sélectionné
                if (selectedType == "Tous")
                {
                    BodiesListView.ItemsSource = allBodies; // Afficher tous les corps célestes
                }
                else
                {
                    var filteredBodies = allBodies?.Where(body => body.bodyType.Equals(selectedType, StringComparison.OrdinalIgnoreCase)).ToList();
                    BodiesListView.ItemsSource = filteredBodies;
                }
            }
        }

        // Événement déclenché lorsque le bouton d'actualisation est cliqué
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // Bouton pour ajouter aux favoris
        private void AddToFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            if (BodiesListView.SelectedItem is Body selectedBody)
            {
                // Vérifier si le favori n'est pas déjà dans la liste
                if (!FavoritesList.Items.Contains(selectedBody))
                {
                    FavoritesList.Items.Add(selectedBody);
                    MessageBox.Show($"{selectedBody.name} a été ajouté aux favoris !");
                }
                else
                {
                    MessageBox.Show($"{selectedBody.name} est déjà dans les favoris.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un corps céleste à ajouter aux favoris.");
            }
        }

        private void RemoveFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FavoritesList.SelectedItem is Body selectedFavorite)
            {
                FavoritesList.Items.Remove(selectedFavorite);
                MessageBox.Show($"{selectedFavorite.name} a été retiré des favoris !");
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un favori à supprimer.");
            }
        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            DetailsPanel.Visibility = Visibility.Visible;
            FavoritesPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowFavoritesButton_Click(object sender, RoutedEventArgs e)
        {
            DetailsPanel.Visibility = Visibility.Collapsed;
            FavoritesPanel.Visibility = Visibility.Visible;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Pause();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Stop();
        }
    }
}