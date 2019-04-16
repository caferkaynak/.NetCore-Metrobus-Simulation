using Microsoft.EntityFrameworkCore;
using StationApplication.Common;
using StationApplication.Common.StationSmartTicketView;
using StationApplication.Data;
using StationApplication.Entity.Entities;
using System;
using System.Linq;
namespace StationApplication.Service
{
    public interface ISmartTicketService
    {
        SmartTicket List(SmartTicket smartTicket);
        ServiceResult Add(SmartTicket smartTicket);
        void Update(SmartTicket smartTicket);
        void Remove(SmartTicket smartTicket);
        ServiceResult UpdateForTrip(StationSmartTicketView stationSmartTicketView);
        ServiceResult UpdateForRefund(StationSmartTicketView stationSmartTicketView);
    }
    public class SmartTicketService : ISmartTicketService
    {
        private readonly AppSettings _AppSettings;
        private IRepository<Station> _StationRepository;
        IRepository<SmartTicket> _SmartTicketRepository;
        IRepository<StationSmartTicket> _StationSmartTicketRepository;
        public SmartTicketService(IRepository<SmartTicket> SmartTicketRepository,
            IRepository<Station> StationRepository,
            AppSettings AppSettings,
            IRepository<StationSmartTicket> StationSmartTicketRepository)
        {
            _SmartTicketRepository = SmartTicketRepository;
            _StationRepository = StationRepository;
            _StationSmartTicketRepository = StationSmartTicketRepository;
            _AppSettings = AppSettings;
        }
        public SmartTicket List(SmartTicket smartTicket)
        {
            smartTicket = _SmartTicketRepository.GetAll().Where(w => w.Id == smartTicket.Id && w.UniqueCode == smartTicket.UniqueCode).FirstOrDefault();
            return smartTicket;
        }
        public ServiceResult Add(SmartTicket smartTicket)
        {
            ServiceResult serviceResult = new ServiceResult();
            byte[] buffer = Guid.NewGuid().ToByteArray();
            smartTicket.UniqueCode = BitConverter.ToUInt32(buffer, 8).ToString();
            int Id = _SmartTicketRepository.AddSmartTicket(smartTicket);
            serviceResult.Id = Id;
            serviceResult.Result = true;
            serviceResult.UniqueCode = smartTicket.UniqueCode;
            return serviceResult;
        }
        public void Update(SmartTicket smartTicket)
        {
            double arrear = smartTicket.Arrears;
            smartTicket = _SmartTicketRepository.GetAll().Where(w => w.Id == smartTicket.Id).FirstOrDefault();
            smartTicket.Arrears += arrear;
            _SmartTicketRepository.Update(smartTicket);
        }
        public ServiceResult UpdateForTrip(StationSmartTicketView stationSmartTicketView)
        {
            ServiceResult serviceResult = new ServiceResult();
            var smartTicket = _SmartTicketRepository.GetAll()
                .Where(w => w.Id == stationSmartTicketView.SmartTicket.Id)
                .Include(i => i.SmartTicketType)
                .FirstOrDefault();
            double pay = StartPayAlgorithm(stationSmartTicketView.Station.Id, smartTicket.SmartTicketType.Name);
            if (smartTicket.Arrears > pay)
            {

                serviceResult.Pay = pay;
                serviceResult.Result = true;
                smartTicket.Arrears = smartTicket.Arrears - pay;
                smartTicket.Status = true;
                serviceResult.Message = "Kullanılan Tutar : -" + pay;
                _SmartTicketRepository.Update(smartTicket);
            }
            else
            {
                serviceResult.Result = false;
                serviceResult.Message = "Yetersiz Bakiye";
            }

            return serviceResult;
        }
        public ServiceResult UpdateForRefund(StationSmartTicketView stationSmartTicketView)
        {
            ServiceResult serviceResult = new ServiceResult();
            stationSmartTicketView.SmartTicket = _SmartTicketRepository.GetAll()
                .Where(w => w.Id == stationSmartTicketView.SmartTicket.Id)
                .Include(i => i.SmartTicketType)
                .FirstOrDefault();
            if (stationSmartTicketView.SmartTicket.Status == true)
            {
                double Refund = RefundPayAlgorithm(stationSmartTicketView.SmartTicket.Id,
                    stationSmartTicketView.Station.Id,
                    stationSmartTicketView.SmartTicket.SmartTicketType.Name);
                stationSmartTicketView.SmartTicket = _SmartTicketRepository.GetAll()
                    .Where(w => w.Id == stationSmartTicketView.SmartTicket.Id)
                    .FirstOrDefault();
                stationSmartTicketView.SmartTicket.Arrears += Refund;
                var stationSmartTicket = _StationSmartTicketRepository.GetAll()
                  .Where(w => w.SmartTicket.Id == stationSmartTicketView.SmartTicket.Id)
                  .Last();
                if (Refund > 0)
                {
                    stationSmartTicketView.SmartTicket.Status = false;
                    serviceResult.Result = true;
                    serviceResult.Id = stationSmartTicket.Id;
                    serviceResult.Refund = Refund;
                    serviceResult.Message = "İade Yapıldı Tutar : " + Refund;
                    _SmartTicketRepository.Update(stationSmartTicketView.SmartTicket);
                }
                else
                    serviceResult.Message = "İade Tutarı 0 TL'dir İade Yapılamaz";
            }
            else
            {
                serviceResult.Result = false;
                serviceResult.Message = "İade alabilmeniz için yolculuk yapmanız lazım";
            }
            return serviceResult;
        }
        public void Remove(SmartTicket smartTicket) => _SmartTicketRepository.Remove(smartTicket);
        private double StartPayAlgorithm(int stationId, string smartTicketType)
        {
            double Student = _AppSettings.Student;
            if (smartTicketType != "Ogrenci")
            {
                double Pay = _AppSettings.Pay;
                double MinPay = _AppSettings.MinPay;
                double Distance = _StationRepository.GetAll()
                    .Where(w => w.Id == stationId)
                    .FirstOrDefault()
                    .StartDistance;
                double MaxDintance = _StationRepository.GetAll()
                    .Select(s => s.StartDistance)
                    .Max();
                double DistanceForPay = MaxDintance - Distance;
                if (DistanceForPay > Distance)
                    Pay = Pay * DistanceForPay;
                else
                    Pay = Pay * Distance;
                return MinPay + Pay;
            }
            return Student;
        }
        private double RefundPayAlgorithm(int smartTicketId, int _stationId, string smartTicketType)
        {
            if (smartTicketType != "Ogrenci")
            {
                double refund = 0;
                double Pay = _AppSettings.Pay;
                double MinPay = _AppSettings.MinPay;
                var stationId = _StationSmartTicketRepository.GetAll()
                       .Where(w => w.SmartTicket.Id == smartTicketId)
                       .Last()
                       .StartStationId;
                double Paid = StartPayAlgorithm(stationId, smartTicketType);
                double StartDistance = _StationRepository.GetAll()
                   .Where(w => w.Id == stationId)
                   .FirstOrDefault()
                   .StartDistance;
                double FinishDistance = _StationRepository.GetAll()
                    .Where(w => w.Id == _stationId)
                    .FirstOrDefault()
                    .StartDistance;
                double DistanceForPay = 1;
                if (StartDistance > FinishDistance)
                    DistanceForPay = StartDistance - FinishDistance;
                else if (StartDistance < FinishDistance)
                    DistanceForPay = FinishDistance - StartDistance;
                Pay = MinPay + Pay * DistanceForPay;
                if (Paid > Pay)
                {
                    refund = Paid - Pay;
                }
                return refund;
            }
            return 0;
        }
    }
}