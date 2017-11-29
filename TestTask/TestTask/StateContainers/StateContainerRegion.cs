using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestTask
{
    // За основу було наступне джерело: https://habrahabr.ru/company/microsoft/blog/307890/

    [ContentProperty("Conditions")]
    public class StateContainerRegion : ContentView
    {
        public List<StateConditionRegion> Conditions { get; set; } = new List<StateConditionRegion>();

        public static readonly BindableProperty StateSourceProperty =
            BindableProperty.Create(
                propertyName: nameof(StateSource),
                returnType: typeof(object),
                declaringType: typeof(StateContainerRegion),
                defaultValue: null,
                propertyChanged: OnStateSourceChanged);

        public static readonly BindableProperty ErrorMessageProperty =
            BindableProperty.Create(
                propertyName: nameof(ErrorMessage),
                returnType: typeof(string),
                declaringType: typeof(UserFormPage),
                defaultValue: default(string));

        public object StateSource
        {
            get { return GetValue(StateSourceProperty); }
            set { SetValue(StateSourceProperty, value); }
        }

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        // Переводимо статичний метод в нестатичний
        private static async void OnStateSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            await ((StateContainerRegion)bindable).OnStateSourceChanged(oldValue, newValue);                
        }

        // Підписуємось на події в середині StateSource і викликаємо зміну статусу
        private async Task OnStateSourceChanged (object oldValue, object newValue)
        {
            // Видяляємо можливе повідомлення про помилку, минулого StateSource
            ErrorMessage = null;

            var stateSource = newValue as NotifyTaskCompletion<List<APIData>>;

            if (stateSource != null)
                stateSource.PropertyChanged += OnStateSourcePropertyChanged;

            await ChooseState(stateSource);
        }

        // Перевіряємо, які властивості зазнали змін в середині StateSource і, у разі потреби, викликаємо зміну статусу
        private async void OnStateSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsFaulted":
                case "IsNotCompleted":                
                case "Result":
                    await ChooseState((NotifyTaskCompletion<List<APIData>>)sender);
                    break;
            }
        }

        private async Task ChooseState(NotifyTaskCompletion<List<APIData>> stateSource)
        {
            if (Conditions == null && Conditions?.Count == 0) return;

            if (stateSource.IsFaulted)
            {
                if (ErrorMessage != stateSource.ErrorMessage) 
                    ErrorMessage = stateSource.ErrorMessage;
                await ShowState("Error");
            }               
            else if(stateSource.IsNotCompleted)
                await ShowState("Loading");
            else
                await ShowState("Normal");

        }

        private async Task ShowState(string stateName)
        {
            foreach (var condition in Conditions)
            {
                if (condition.State.ToString() == stateName)
                {
                    if (Content != null)
                    {
                        await Content.FadeTo(0, 100U); //быстрая анимация скрытия
                        Content.IsVisible = false; //Полностью скрываем с экрана старое состояние
                        await Task.Delay(30); //Позволяем UI-потоку отработать свою очередь сообщений и гарантировано скрыть предыдущее состояние
                    }

                    // Плавно показываем новое состояние   
                    condition.Content.Opacity = 0;
                    Content = condition.Content;
                    Content.IsVisible = true;
                    await Content.FadeTo(1);

                    break;
                }
            }    
        }
    }
}
