using StationApplication.Common.SmartTicketTypeView;
using StationApplication.Data;
using StationApplication.Entity.Entities;
using System.Linq;

namespace StationApplication.Service
{
    public interface ISmartTicketTypeService
    {
        SmartTicketTypeView SmartTicketTypeList();
    }
    public class SmartTicketTypeService : ISmartTicketTypeService
    {
        private IRepository<SmartTicketType> _SmartTicketTypeRepository;
        public SmartTicketTypeService(IRepository<SmartTicketType> SmartTicketTypeRepository)
        {
            _SmartTicketTypeRepository = SmartTicketTypeRepository;
        }
        public SmartTicketTypeView SmartTicketTypeList()
        {
            SmartTicketTypeView smartTicketTypeView = new SmartTicketTypeView();
            smartTicketTypeView.SmartTicketTypes = _SmartTicketTypeRepository.GetAll().ToList();
            return smartTicketTypeView;
        }
    }
}
