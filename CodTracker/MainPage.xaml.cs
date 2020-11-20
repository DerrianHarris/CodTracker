using CodTracker.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodTracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        string AppKey = "cddee3dc1fmsh2033a3f1179103bp13edeejsn277f7910d181";
        string HostURL = "call-of-duty-modern-warfare.p.rapidapi.com";

        Color currColor;
        string XboxColorHex = "#107c10";
        string PlaystationColorHex = "#00439c";
        string BattleNetColorHex = "#0090d6";
        string ActivisionIdColorHex = "#000000";

        SearchSettings Settings;

        Dictionary<string, string> platformDict = new Dictionary<string, string> { { "Xbox","xbl"}, { "Playstation", "psn" }, { "Activision Id", "uno" }, { "Battle.net", "battle" } };
        List<string> ModeList = new List<string> { "Warzone", "Multiplayer"};
        public MainPage()
        {
            InitializeComponent();
            PlatformPicker.ItemsSource = platformDict.Keys.ToList();
            ModePicker.ItemsSource = ModeList;
            currColor = BackgroundColor;
        }

        async void Handle_ClickedSearch(object sender, System.EventArgs e) {
            Settings = new SearchSettings();
            Settings.Platform = (string)PlatformPicker.SelectedItem;
            string platform = platformDict[Settings.Platform];
            
            Settings.Username = Entry.Text.Replace('#','%');
            Settings.Mode = ((string)ModePicker.SelectedItem).ToLower();

            string uri = $"https://{HostURL}/{Settings.Mode}/{Settings.Username}/{platform}";
            var client = new RestClient(uri);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", HostURL);
            request.AddHeader("x-rapidapi-key", AppKey);
            IRestResponse response = client.Execute(request);
            Settings.Json = JObject.Parse(response.Content);
            await Navigation.PushAsync(new ProfilePage(Settings));

            
        }

        private void PlatformPicker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Picker Picker = (Picker)sender;
            string choice = (string)Picker.SelectedItem;
            Entry.Placeholder = $"Enter {choice} Username";
            Color newColor = Color.White;
            switch (choice) {
                case "Xbox":
                    newColor = Color.FromHex(XboxColorHex);
                    break;
                case "Playstation":
                    newColor = Color.FromHex(PlaystationColorHex);
                    break;
                case "Battle.net":
                    newColor = Color.FromHex(BattleNetColorHex);
                    break;
                case "Activision Id":
                    newColor = Color.FromHex(ActivisionIdColorHex);
                    break;
            }
            
            animateColor(newColor.MultiplyAlpha(.5));
            
        }
        private void animateColor(Color color) {
            new Animation(callback: v => BackgroundColor = Color.FromHsla((currColor.Hue - color.Hue) * v + color.Hue, (currColor.Saturation - color.Saturation) * v + color.Saturation, (currColor.Luminosity - color.Luminosity) * v + color.Luminosity, .5),
             start: 0,
            end: 1).Commit(this, "Animation", 16, 4000, Easing.Linear, (v, c) => BackgroundColor = currColor = color) ;          
        }
    }
}
