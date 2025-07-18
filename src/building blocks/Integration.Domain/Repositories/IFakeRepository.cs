using Integration.Domain.Entities;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{

    //ESTA CLASSE FOI CRIADA PARA PODER SIMULAR COMO SERIA A IMPLEMENTAÇÃO
    //POIS COMO AINDA NÃO TEMOS BANCO DE DADOS ESTA CLASSE FOI CRIADA
    //DEPOIS BASTA REMOVERMOS TUDO REFERENTE A CLASSE FAKE
    public interface IFakeRepository: IGenericRepository<Fake>
    {

    }
}

