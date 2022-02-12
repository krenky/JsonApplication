using JsonApp1.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Forms;

public class ColorsViewModel : INotifyPropertyChanged
{
    private string name;
    private string year;
    private string color;

    public string Name
    {
        get { return name; }
        private set
        {
            name = value;
            OnPropertyChanged("Name");
        }
    }

    public string Year
    {
        get { return year; }
        private set
        {
            year = value;
            OnPropertyChanged("Year");
        }
    }
    public string Color
    {
        get { return color; }
        private set
        {
            color = value;
            OnPropertyChanged("color");
        }
    }

    public ICommand LoadDataCommand { protected set; get; }

    public ColorsViewModel()
    {
        this.LoadDataCommand = new Command(LoadData);
    }

    private async void LoadData()
    {
        string url = "https://reqres.in/api/products/4";

        try
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var response = await client.GetAsync(client.BaseAddress);
            response.EnsureSuccessStatusCode(); // выброс исключения, если произошла ошибка

            // десериализация ответа в формате json
            var content = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(content);

            var str = o.SelectToken(@"$.data");
            var ColorsInfo = JsonConvert.DeserializeObject<ColorsModel>(str.ToString());

            this.Name = ColorsInfo.Name;
            this.Year = ColorsInfo.Year;
            this.Color = ColorsInfo.Color;
        }
        catch (Exception ex)
        { }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}

