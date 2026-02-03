
using Contracts.DTOs.Signature;
using Contracts.interfaces.Repository;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISignatureRepository : IRepositoryBase<Signature,SignatureDto,SignatureCreateDto,SignatureUpdateDto>
{
}
