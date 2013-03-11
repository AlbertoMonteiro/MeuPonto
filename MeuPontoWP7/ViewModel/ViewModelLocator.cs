using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MeuPonto.Common.Repositorios;
using MeuPontoWP7.Converters;
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
            SimpleIoc.Default.Register<LancamentoViewModel>();
            SimpleIoc.Default.Register<RelatorioViewModel>();
            SimpleIoc.Default.Register<BatidasToResumoConverter>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public LancamentoViewModel Lancamento
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LancamentoViewModel>();
            }
        }

        public RelatorioViewModel Relatorio
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RelatorioViewModel>();
            }
        }

        public BatidasToResumoConverter BatidasToResumoConverter
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BatidasToResumoConverter>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}