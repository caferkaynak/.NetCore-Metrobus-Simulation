using StationApplication.Data;
using StationApplication.Entity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace StationApplication.Service
{
    public interface IStationService
    {
        List<Station> StationList();
        void AddStation(Station station);
        void UpdateStation(Station station);
        void RemoveStation(Station station);
    }
    public class StationService : IStationService
    {
        IRepository<Station> _StationRepository;
        public StationService(IRepository<Station> StationRepository)
        {
            _StationRepository = StationRepository;
        }
        public List<Station> StationList()
        {
            List<Station> station = new List<Station>();
            station = _StationRepository.GetAll().ToList();
            return station;
        }
        public void AddStation(Station station)
        {
            _StationRepository.Add(station);
        }
        public void UpdateStation(Station station)
        {
            _StationRepository.Update(station);
        }
        public void RemoveStation(Station station)
        {
            _StationRepository.Remove(station);
        }
    }
}
