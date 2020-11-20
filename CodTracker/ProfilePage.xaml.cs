using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;
using CodTracker.Model;
using System.Resources;

namespace CodTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {

        JObject JsonProfile;
        Dictionary<string,string> modeDict = new Dictionary<string, string> { { "br", "Battle Royale" }, { "br_dmz", "Plunder" } , { "br_all", "All Modes" } };
        Dictionary<string, string> modeMapDict = new Dictionary<string, string> { { "Battle Royale", "br" }, {  "Plunder", "br_dmz"}, { "All Modes", "br_all"} };
        List<string> modeList = new List<string>();
        public ProfilePage(SearchSettings settings)
        {
            InitializeComponent();
            JsonProfile = settings.Json;


            if (settings.Mode == "warzone")
            {
                ModePicker.IsVisible = true;

                foreach (var item in JsonProfile.Properties())
                {
                    string itemName = item.Name;
                    modeList.Add(modeDict.ContainsKey(itemName) ? modeDict[itemName] : itemName);
                }

                ModePicker.ItemsSource = modeList;
                
            }
            else {
                ModePicker.IsVisible = false;
            }
            PlatformLabel.Text = settings.Platform;
            UsernameLabel.Text = settings.Username;
        }

        private void ModePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Picker picker = (Picker)sender;
            string choice = (string)picker.SelectedItem;
            
            if (choice == null) {
                return;
            }
            string choiceMap = modeMapDict[choice];

            DataLayout.Children.Clear();
            JObject data = (JObject)JsonProfile[choiceMap];

            foreach (var item in data.Properties())
            {
                Label label = new Label();
                label.Style = (Style)Application.Current.Resources["DataLabelStyle"];
                DataLayout.Children.Add(label);
                label.Parent = DataLayout;
                label.Text = $"{item.Name}: {item.Value}";
                

            }
        }
    }
}