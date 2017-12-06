using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;

namespace TestTask
{
    /// <summary>
    /// Спроба реалізувати <see cref="SearchBar"/> з "випадаючим" списком результатів пошуку.
    /// </summary>
    public partial class ImproveSearchBar : ContentView
    {
        new public event EventHandler<FocusEventArgs> Focused;
        new public event EventHandler<FocusEventArgs> Unfocused;

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(
                propertyName: nameof(Placeholder),
                returnType: typeof(string),
                declaringType: typeof(ImproveSearchBar),
                defaultValue: default(string));

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                propertyName: nameof(Text),
                returnType: typeof(string),
                declaringType: typeof(ImproveSearchBar),
                defaultValue: string.Empty,
                propertyChanged: OnTextChanged);

        public static readonly BindableProperty InputDataObjectProperty =
            BindableProperty.Create(
                propertyName: nameof(InputDataObject),
                returnType: typeof(object),
                declaringType: typeof(ImproveSearchBar),
                defaultValue: null,
                propertyChanged: OnInputDataObjectChanged);

        public static readonly BindableProperty OutputDataObjectProperty =
            BindableProperty.Create(
                propertyName: nameof(OutputDataObject),
                returnType: typeof(object),
                declaringType: typeof(ImproveSearchBar),
                defaultValue: null);

        public static readonly BindableProperty SearchCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(SearchCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(ImproveSearchBar),
                defaultValue: null);

        public static readonly BindableProperty SearchResultProperty =
            BindableProperty.Create(
                propertyName: nameof(SearchResult),
                returnType: typeof(NotifyTaskCompletion<List<APIData>>),
                declaringType: typeof(ImproveSearchBar),
                defaultValue: null,
                propertyChanged: OnSearchResultChanged);

        public static readonly BindableProperty ErrorMessageProperty =
            BindableProperty.Create(
                propertyName: nameof(ErrorMessage),
                returnType: typeof(string),
                declaringType: typeof(ImproveSearchBar),
                defaultValue: default(string));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Об'єкт вхідних даних. Використовується для перевірки готовності ViewModel до початку пошуку.   
        /// </summary>
        public object InputDataObject
        {
            get { return GetValue(InputDataObjectProperty); }
            set { SetValue(InputDataObjectProperty, value); }
        }

        /// <summary>
        /// Команда пошуку   
        /// </summary>
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        /// <summary>
        /// Результат виконання команди пошуку у вигляді списку результатів, оберненного в NotifyTaskCompletion  
        /// </summary>
        public NotifyTaskCompletion<List<APIData>> SearchResult
        {
            get { return (NotifyTaskCompletion<List<APIData>>)GetValue(SearchResultProperty); }
            set { SetValue(SearchResultProperty, value); }
        }

        /// <summary>
        /// Об'єкт вихідних даних, обраний користувачем із списку результатів.   
        /// </summary>
        public object OutputDataObject
        {
            get { return GetValue(OutputDataObjectProperty); }
            set { SetValue(OutputDataObjectProperty, value); }
        }

        /// <summary>
        /// Повідомлення про помилку під час пошуку. Служить для прив'язки до аналогічної властивості <see cref="Page"/>   
        /// </summary>
        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public ImproveSearchBar()
        {
            InitializeComponent();

            searchBar.Focused += OnSearchBarFocused;
            searchBar.Unfocused += OnSearchBarUnfocused;
        }

        private void OnSearchBarFocused(object sender, FocusEventArgs e)
        {
            // Блокуємо подію Focused, якщо контрол не активний.
            if (!IsEnabled)
                return;

            // При отриманны фокусу SearchBar, відображаєм фрейм зі списком результатів пошуку
            // і запускаємо пошук на основі данних, що містяться в полі Text.
            if (Focused != null)
            {
                var arg = new FocusEventArgs(this, true);
                Focused(this, arg);
                dropDownArea.IsVisible = true;

                if (Text == null)
                    Text = string.Empty;

                Search(Text);
            }                
        }

        private void OnSearchBarUnfocused(object sender, FocusEventArgs e)
        {
            if (Unfocused != null)
            {
                // Ховаєм фрейм зі списком 
                dropDownArea.IsVisible = false;

                // Прокручуємо скрол на початок списку 
                scroll.ScrollToAsync(0, 0, true);

                // Повертаємо назву знайденого та обраного зі списку об'єкту, ящо такий є 
                ShowOutputDataObject();

                var arg = new FocusEventArgs(this, true);
                Unfocused(this, arg);
            }
        }

        private void ShowOutputDataObject()
        {
            try
            {
                Text = OutputDataObject.ToString();
            }
            catch (NullReferenceException exp)
            {
                Text = string.Empty;
            }
        }

        static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ImproveSearchBar)bindable).OnTextChanged((string)newValue);
        }

        // Кожного разу при зміні текстового поля (вводі нової літери чи видалення раніше введених) 
        // запускається новий пошук результатів.
        void OnTextChanged(string userInput)
        {
            if (userInput == null || InputDataObject == null || !searchBar.IsFocused)
                return;
            else
                Search(userInput);
        }

        // При зміні вхідних даних, вихідні дані обнуляються.
        static void OnInputDataObjectChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = ((ImproveSearchBar)bindable);
            control.OutputDataObject = null;
            control.ShowOutputDataObject();
        }

        // При отриманны результату пошуку...
        private static void OnSearchResultChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ImproveSearchBar control = (ImproveSearchBar)bindable;
            NotifyTaskCompletion<List<APIData>> searchResult = (NotifyTaskCompletion<List<APIData>>) newValue;

            // ...очищаємо можливе повідомлення про помилку з минулого пошуку
            control.ErrorMessage = null;

            searchResult.PropertyChanged += control.OnSearchResultPropertyChanged;
            control.SearchResultProcessing(searchResult); 
        }

        private void OnSearchResultPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SearchResultProcessing((NotifyTaskCompletion<List<APIData>>)sender, e);
        }

        private void SearchResultProcessing(NotifyTaskCompletion<List<APIData>> searchResult, PropertyChangedEventArgs e = null)
        {
            // ... вимикаємо ActivityIndicator
            if (e == null || e.PropertyName == "IsCompleted")
                if (searchResult.IsCompleted)
                    ActivityIndicatorOff();

            // ... обробляємо помилки, якщо вони є
            if (e == null || e.PropertyName == "IsFaulted")
                if (searchResult.ErrorMessage != null)
                {
                    searchBar.Unfocus();
                    Text = null;
                    OutputDataObject = null;

                    ErrorMessage = searchResult.ErrorMessage;

                    return;
                }

            // ... виводимо результат пошуку у вигляді списку
            if (e == null || e.PropertyName == "Result")
                dropDownList.ItemsSource = searchResult.Result;
        }

        private void Search(string inputText)
        {
            if (SearchCommand == null)
                return;

            ActivityIndicatorOn();
            SearchCommand.Execute(inputText);
        }

        private void ActivityIndicatorOn()
        {
            scroll.Opacity = 0;
            scroll.ScrollToAsync(0, 0, true);
            activityIndicator.IsRunning = true;
        }

        private void ActivityIndicatorOff()
        {
            activityIndicator.IsRunning = false;
            scroll.Opacity = 1;
        }

        private void OnDropDownListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            OutputDataObject = ((ListView)sender).SelectedItem;
            ShowOutputDataObject();
        }
    }
}