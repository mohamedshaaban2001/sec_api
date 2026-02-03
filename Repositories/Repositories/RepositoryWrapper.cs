using Contracts.interfaces.Models;
using Contracts.interfaces.Repository;
using Entities.Models;
using LoggerService;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Repositories.Models;

namespace Repositories.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {


        private ISignatureRepository _signatures;

        public ISignatureRepository Signatures 
        {
            get
            {
                if (_signatures == null)
                {
                    _signatures = new SignatureRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
                }
                return _signatures;
            }
        }

        private ISecModuleGroupRepository _secModuleGroups;

        public ISecModuleGroupRepository SecModuleGroups
        {
            get
            {
                if (_secModuleGroups == null)
                {
                    _secModuleGroups = new SecModuleGroupRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
                }
                return _secModuleGroups;
            }
        }

        private IUserRepository _users;

    public IUserRepository Users
    {
        get
        {
            if (_users == null)
            {
                _users = new UserRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _users;
        }
    }

    private ISecServiceRepository _secServices;

    public ISecServiceRepository SecServices
    {
        get
        {
            if (_secServices == null)
            {
                _secServices = new SecServiceRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secServices;
        }
    }

    private ISecPageRepository _secPages;

    public ISecPageRepository SecPages
    {
        get
        {
            if (_secPages == null)
            {
                _secPages = new SecPageRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secPages;
        }
    }

    private ISecModuleRepository _secModules;

    public ISecModuleRepository SecModules
    {
        get
        {
            if (_secModules == null)
            {
                _secModules = new SecModuleRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secModules;
        }
    }

    private ISecGroupPageRepository _secGroupPages;

    public ISecGroupPageRepository SecGroupPages
    {
        get
        {
            if (_secGroupPages == null)
            {
                _secGroupPages = new SecGroupPageRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secGroupPages;
        }
    }

    private ISecGroupJobRepository _secGroupJobs;

    public ISecGroupJobRepository SecGroupJobs
    {
        get
        {
            if (_secGroupJobs == null)
            {
                _secGroupJobs = new SecGroupJobRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secGroupJobs;
        }
    }

    private ISecGroupEmployeeRepository _secGroupEmployees;

    public ISecGroupEmployeeRepository SecGroupEmployees
    {
        get
        {
            if (_secGroupEmployees == null)
            {
                _secGroupEmployees = new SecGroupEmployeeRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secGroupEmployees;
        }
    }

    private ISecGroupControlRepository _secGroupControls;

    public ISecGroupControlRepository SecGroupControls
    {
        get
        {
            if (_secGroupControls == null)
            {
                _secGroupControls = new SecGroupControlRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secGroupControls;
        }
    }

    private ISecGroupRepository _secGroups;

    public ISecGroupRepository SecGroups
    {
        get
        {
            if (_secGroups == null)
            {
                _secGroups = new SecGroupRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secGroups;
        }
    }

    private ISecControlListRepository _secControlLists;

    public ISecControlListRepository SecControlLists
    {
        get
        {
            if (_secControlLists == null)
            {
                _secControlLists = new SecControlListRepository(_repoContext, _httpContextAccessor, _mapper, _logger);
            }
            return _secControlLists;
        }
    }
        private RepositoryContext _repoContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        public RepositoryWrapper(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
            , IMapper mapper, ILoggerManager logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _repoContext = repositoryContext;
            _mapper = mapper;
            _logger = logger;
        }








        private IPersonsRepository _persons;
    public IPersonsRepository Persons
    {
        get
        {
            if (_persons == null)
            {
                _persons = new PersonsRepository(_repoContext);
            }
            return _persons;
        }
    }

    private IJobRepository _jobs;
    public IJobRepository Jobs
    {
        get
        {
            if (_jobs == null)
            {
                _jobs = new JobRepository(_repoContext);
            }
            return _jobs;
        }
    }

    private IUniversityRepository _universities;
    public IUniversityRepository Universities
    {
        get
        {
            if (_universities == null)
            {
                _universities = new UniversityRepository(_repoContext);
            }
            return _universities;
        }
    }

    public void Save()
        {
            _repoContext.SaveChanges();
        }

        //void IRepositoryWrapper.Save()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
