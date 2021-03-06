﻿using MeuPonto.Common.Models;
using MeuPonto.Common.Repositorios;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;
using MeuPontoWP7.ViewModel;
using MeuPontoWP7.Extensions;

namespace MeuPontoWP7.Converters
{
    public class BatidasToResumoConverter : IValueConverter
    {
        private static CacheContext _cacheContext;

        public BatidasToResumoConverter()
        {
            _cacheContext = new CacheContext();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var batidaViewModels = (KeyGroup<Batida>) value;

            var diferenca = TimeSpan.Zero;
            if (_cacheContext != null)
            {
                var configuracao = _cacheContext.Configuracoes.FirstOrDefault();

                diferenca = batidaViewModels.Resumo();

                if (configuracao != null)
                {
                    var timeSpan = diferenca - configuracao.HorarioDeTrabalhoDiario;
                    return timeSpan > TimeSpan.Zero 
                               ? "Crédito de " + timeSpan.ToString(@"hh\:mm\:ss") 
                               : "Débito de " + timeSpan.ToString(@"hh\:mm\:ss");
                }
            }

            return diferenca.ToString(@"hh\:mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
