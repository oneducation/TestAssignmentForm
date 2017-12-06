using System.Collections.Generic;
using System.Net;
using System.Windows.Input;

namespace TestTask
{
    public class AppViewModel : ViewModelBase
    {
        const string _urlAPIRegions = "https://donor.ua/api/regions";
        const string _citiesSearchQueryTemplate = 
            "https://donor.ua/api/cities?$filter=startswith(name,'{0}') and regionId eq {1}";
        const string _centersSearchQueryTemplate = 
            "https://donor.ua/api/centers?$filter=startswith(tolower(name),tolower('{0}')) and cityId eq {1}";
        string _name, _surname;
        NotifyTaskCompletion<List<APIData>> _regions, _cities, _centers;
        APIData _region, _city, _center;

        public string Name
        {
            set { SetProperty(ref _name, value); }
            get { return _name; }
        }

        public string Surname
        {
            set { SetProperty(ref _surname, value); }
            get { return _surname; }
        }

        public NotifyTaskCompletion<List<APIData>> Regions
        {
            protected set { SetProperty(ref _regions, value); }
            get { return _regions; }
        }

        public APIData Region
        {
            set { SetProperty(ref _region, value); }
            get { return _region; }
        }

        public NotifyTaskCompletion<List<APIData>> Cities
        {
            protected set { SetProperty(ref _cities, value); }
            get { return _cities; }
        }
       
        public APIData City
        {
            set { SetProperty(ref _city, value); }
            get { return _city; }
        }

        public NotifyTaskCompletion<List<APIData>> Centers
        {
            protected set { SetProperty(ref _centers, value); }
            get { return _centers; }
        }

        public APIData Center
        {
            set { SetProperty(ref _center, value); }
            get { return _center; }
        }

        public ICommand LoadRegionsCommand { private set; get; }
        public ICommand CitiesSearchCommand { private set; get; }
        public ICommand CentersSearchCommand { private set; get; }

        public AppViewModel()
        {
            LoadRegions();

            LoadRegionsCommand = new Xamarin.Forms.Command(LoadRegions);

            CitiesSearchCommand = new Xamarin.Forms.Command<string>(
                execute: (string inputText) =>
                {
                    Cities = Search(_citiesSearchQueryTemplate, Region.Id, inputText);
                });

            CentersSearchCommand = new Xamarin.Forms.Command<string>(
                execute: (string inputText) =>
                {
                    Centers = Search(_centersSearchQueryTemplate, City.Id, inputText);
                });

        }

        private void LoadRegions()
        {
            Regions = new NotifyTaskCompletion<List<APIData>>(InternetHelper.LoadControlsDataAsync<APIData>(_urlAPIRegions));
        }

        NotifyTaskCompletion<List<APIData>>Search(string queryTemplate, int id, string inputText)
        {
            var encodedInputText = WebUtility.UrlEncode(inputText.Replace("'", "''"));
            var query = string.Format(queryTemplate, encodedInputText, id);
            var serchResult = new NotifyTaskCompletion<List<APIData>>(InternetHelper.LoadControlsDataAsync<APIData>(query));
            return serchResult;
        }
    }
}
