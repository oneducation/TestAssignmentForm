using System;
using Xamarin.Forms;

namespace TestTask
{
    public partial class UserFormPage : ContentPage
    {
        public static readonly BindableProperty ErrorMessageProperty =
            BindableProperty.Create(
                propertyName: nameof(ErrorMessage),
                returnType: typeof(string),
                declaringType: typeof(UserFormPage),
                defaultValue: default(string),
                propertyChanged: OnErrorMessagePropertyChanged);

        private static void OnErrorMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var page = (UserFormPage)bindable;
            if (newValue != null)
            {
                Device.BeginInvokeOnMainThread(() => {page.DisplayAlert("Виникла помилка:", (string)newValue, "OK");});
            }
        }

        /// <summary>
        /// Служить для централізованого збору всіх повідомлення про помилки і інформування про них користувача.    
        /// </summary>
        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public UserFormPage()
        {
            InitializeComponent();
        }

        private void OnImproveSearchBarFocused(object sender, FocusEventArgs e)
        {
            View control = (View)sender;

            foreach (View view in gridUI.Children)
                if (view != control)
                    view.IsVisible = false;

            gridUI.RowSpacing = 0;
        }

        private void OnImproveSearchBarUnfocused(object sender, FocusEventArgs e)
        {
            View control = (View)sender;

            foreach (View view in gridUI.Children)
                view.IsVisible = true;

            gridUI.RowSpacing = 6;
        }

        async private void OnButtonFillFormClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FilledOutFormPage());
        }
    }
}