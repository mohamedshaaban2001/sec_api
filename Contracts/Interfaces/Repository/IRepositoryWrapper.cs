using Contracts.interfaces.Models;

namespace Contracts.interfaces.Repository;

public interface IRepositoryWrapper
{

    ISignatureRepository Signatures { get; }
    ISecModuleGroupRepository SecModuleGroups{ get; }

    IUserRepository Users{ get; }

    ISecServiceRepository SecServices{ get; }

    ISecPageRepository SecPages{ get; }

    ISecModuleRepository SecModules{ get; }

    ISecGroupPageRepository SecGroupPages{ get; }

    ISecGroupJobRepository SecGroupJobs{ get; }

    ISecGroupEmployeeRepository SecGroupEmployees{ get; }

    ISecGroupControlRepository SecGroupControls{ get; }

    ISecGroupRepository SecGroups{ get; }

    ISecControlListRepository SecControlLists{ get; }

    IPersonsRepository Persons { get; }
    IJobRepository Jobs { get; }
    IUniversityRepository Universities { get; }

    void Save();
}
