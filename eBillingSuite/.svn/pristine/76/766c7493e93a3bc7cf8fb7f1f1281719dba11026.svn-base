using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace eBillingSuite.ViewModels
{
   

    public class StatsDigitalVM
    {
        private Repositories.IEDigitalDocHistoryRepository _eDigitalDocHistoryRepository;


        public StatsDigitalVM(Repositories.IEDigitalDocHistoryRepository _eDigitalDocHistoryRepository,
            Repositories.IEDigitalIntancesRepository _eDigitalInstancesRepository,
            Repositories.IEDigitalDocTypeRepository _eDigitalDocTypeRepository)
        {
            this._eDigitalDocHistoryRepository = _eDigitalDocHistoryRepository;

            #region BYYEAR
            var temp = this._eDigitalDocHistoryRepository.Set
                .GroupBy(sa => new
                {
                    sa.DtaModificacao.Year,
                })
                .Select(c => new EstatisticasYear
                {
                    Year = c.Key.Year,
                    Count = c.Count()
                }).ToList();

            if (temp.Count == 1)
            {
                EstatisticasYear t = new EstatisticasYear();
                t.Year = temp[0].Year - 1;
                t.Count = 0;

                temp.Add(t);
            }

            DataByYear = temp.OrderBy(by => by.Year).ToList();
            #endregion

            #region BYMONTHYEAR
            DataByMonthYear = this._eDigitalDocHistoryRepository.Set
                .OrderBy(sa => sa.DtaModificacao.Year)
                .ThenBy(sa => sa.DtaModificacao.Month)
                .GroupBy(sa => new
                {
                    sa.DtaModificacao.Year,
                    sa.DtaModificacao.Month
                })
                .Select(c => new EstatisticasMouthYear
                {
                    MonthYear = (c.Key.Month < 10 ? "0" + c.Key.Month.ToString() : c.Key.Month.ToString()) + "-" + c.Key.Year.ToString(),
                    Count = c.Count()
                }).ToList();
            #endregion

            #region BYDOCTYPE
            //GET DOC TYPES
            DataByDocType = new List<EstatisticasDocType>();
            var types = _eDigitalDocTypeRepository.Set.ToList();
            foreach (var type in types)
            {
                //var doccabs = _eDigitalDocHistoryCabRepository.Where(edhc => edhc.Valor == type.nome).Count();
                var doccabs = _eDigitalDocHistoryRepository.Where(edhc => edhc.tpoFatura == type.nome).Count();

                DataByDocType.Add(
                    new EstatisticasDocType
                    {
                        DocTYpe = RemoveDiacritics(type.nome.ToString()),
                        Count = doccabs
                    });
            }
            #endregion

            #region ByUser
            DataByUser = this._eDigitalDocHistoryRepository.Set
                .GroupBy(u => new
                {
                    u.Utilizador
                })
                .Select(c => new EstatisticasUser
                {
                    User = c.Key.Utilizador.ToString(),
                    Count = c.Count()
                }).ToList();
            #endregion

            #region ByCompany
            DataByCompany = new List<EstatisticasCompany>();
            List<string> p = new List<string>();
            p.Add("");

            var instances = _eDigitalInstancesRepository.Set.ToList();
            foreach (var instance in instances)
            {
                var itemC = _eDigitalDocHistoryRepository.Where(c => c.company == instance.InternalCode).Count();

                DataByCompany.Add(new EstatisticasCompany
                {
                    Companhia = instance.Name,
                    Count = itemC
                });

                p.Add(instance.VatNumber);
            }

            #endregion

            #region ByUserRangeTime
            var demo = _eDigitalDocHistoryRepository
                            .Where(o => o.Estado == 4)
                            .Select(o => new
                            {
                                o.Utilizador,
                                o.dtaCriacao,
                                o.DtaModificacao
                            }).ToList();


            DataByUserRangeTime = demo
                            .GroupBy(o => new
                            {
                                o.Utilizador
                            })
                            .Select(o => new EstatisticasUserRangeTime
                            {
                                User = o.Key.Utilizador,
                                RangeTime = Math.Round(o.Average(s => (s.DtaModificacao.Date - s.dtaCriacao.Date).TotalDays), 2)
                            }).ToList();

            #endregion
        }

        public List<EstatisticasDocType> DataByDocType { get; set; }

        public List<EstatisticasYear> DataByYear { get; set; }

        public List<EstatisticasMouthYear> DataByMonthYear { get; set; }

        public List<EstatisticasUser> DataByUser { get; set; }

        public List<EstatisticasCompany> DataByCompany { get; set; }

        public List<EstatisticasUserCompany> DataByUserCompany { get; set; }

        public List<EstatisticasUserRangeTime> DataByUserRangeTime { get; set; }
        public List<EstatisticasCompanyRangeTime> DataByCompanyRangeTime { get; set; }

        public List<EstatisticasBothRangeTime> DataByBothRangeTime { get; set; }



        #region classes necessárias
        public class LiveTimeView
        {
            public string User { get; set; }
            public string Document { get; set; }
        }

        public class EstatisticasUser
        {
            public string User { get; set; }
            public int Count { get; set; }
        }
        public class EstatisticasYear
        {
            public int Year { get; set; }
            public int Count { get; set; }
        }
        public class EstatisticasMouthYear
        {
            public string MonthYear { get; set; }
            public int Count { get; set; }
        }
        public class EstatisticasDocType
        {
            public string DocTYpe { get; set; }
            public int Count { get; set; }
        }
        public class EstatisticasCompany
        {
            public string Companhia { get; set; }
            public int Count { get; set; }
        }
        public class EstatisticasUserCompany
        {
            public string User { get; set; }
            public string Companhia { get; set; }
            public int Count { get; set; }
        }
        public class EstatisticasUserRangeTime
        {
            public string User { get; set; }
            public double RangeTime { get; set; }
        }
        public class EstatisticasCompanyRangeTime
        {
            public string Company { get; set; }
            public double RangeTime { get; set; }
        }
        public class EstatisticasBothRangeTime
        {
            public string User { get; set; }
            public string Company { get; set; }
            public double RangeTime { get; set; }
        }
        #endregion

        #region AUXILIARYMETHODS
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        #endregion
    }
}