using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace TestTask
{
    /// <summary>
    /// Поведінка, для валідації введених користовачем імені та прізвища.
    /// </summary>
    /// <remarks>
    /// Джерело, взяте за основу: https://github.com/xamarin/xamarin-forms-book-samples/blob/master/Libraries/Xamarin.FormsBook.Toolkit/Xamarin.FormsBook.Toolkit/ValidEmailBehavior.cs
    /// </remarks>
    public class ValidEntryBehavior : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey 
            = BindableProperty.CreateReadOnly(
                propertyName: nameof(IsValid),
                returnType: typeof(bool),
                declaringType: typeof(ValidEntryBehavior),
                defaultValue: false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            private set { SetValue(IsValidPropertyKey, value); }
            get { return (bool)GetValue(IsValidProperty); }
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.PropertyChanged += OnEntryPropertyChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.PropertyChanged -= OnEntryPropertyChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Text" || args.PropertyName == "IsEnabled")
            {
                Entry entry = (Entry)sender;
                IsValid = IsValidNameOrSurname(entry.Text) && entry.IsEnabled;
                if (IsValid)
                    entry.TextColor = Color.Default;
                else
                    entry.TextColor = Color.Red;
            }
        }

        bool IsValidNameOrSurname(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
                return false;

            try
            {  
                return Regex.IsMatch(strIn,
                    @"^(?!.*\s\s|.*\-\-|.*\s\-|.*\-\s)[a-zа-яёїієґ][a-zа-яёїієґ\-\s'.]{1,19}[a-zа-яёїієґ.]$", 
                    RegexOptions.IgnoreCase, 
                    TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
