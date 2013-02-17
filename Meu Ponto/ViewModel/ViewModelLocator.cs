/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Meu_Ponto"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Meu_Ponto.ViewModel
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<BatidasPonto>();
            
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                BatidasPonto.Batidas.Clear();
                BatidasPonto.Batidas.Add(new Batida()
                    {
                        Horario = DateTime.Now,
                        NaturezaEntrada = NaturezaEntrada.Entrada
                    });
                BatidasPonto.Batidas.Add(new Batida()
                {
                    Horario = DateTime.Now.AddHours(1),
                    NaturezaEntrada = NaturezaEntrada.Saida
                });
            }
            else
            {
                // Create run time view services and models
                //SimpleIoc.Default.Register<IDataService, DataService>();
            }

            
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public BatidasPonto BatidasPonto
        {
            get { return ServiceLocator.Current.GetInstance<BatidasPonto>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}