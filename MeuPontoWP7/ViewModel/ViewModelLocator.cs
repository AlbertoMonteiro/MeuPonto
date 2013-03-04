using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MeuPontoWP7.Repositorios;
using Microsoft.Practices.ServiceLocation;

namespace MeuPontoWP7.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            //System.Diagnostics.Debugger.Launch();
            
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                SimpleIoc.Default.Unregister<IContextProvider>();
                SimpleIoc.Default.Register<IContextProvider, ContextProvider>();
            }
            else
            {
                // Create run time view services and models
                SimpleIoc.Default.Unregister<IContextProvider>();
                SimpleIoc.Default.Register<IContextProvider, ContextProvider>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}