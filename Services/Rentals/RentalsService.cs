using System.Threading.Tasks;
using MongoDB.Entities;
using Models.RentalsModel;
using Services.Moto.Interfaces.IMotoService;
using Services.DeliveryPerson.Interface.IDeliveryPersonService;
using Models.Rentals.Requests.CreateRentalDTO;
using Models.Rentals.Requests.DevolutionRentalDTO;
using Models.Rentals.Responses.RentalResponse;
using Services.Rentals.Interface.IRentalsService;
using Models.Rentals.Responses.DevolutionRentalResponse;
using Models.DeliveryPersonModel;
using Models.MotoModel;

public class RentalsService : IRentalsService
{
    public RentalsService()
    {
    }

    public async Task<RentalResponse> CreateRentalAsync(CreateRentalDTO rentalDTO)
    {
        // metodo para criar uma rental
        try
        {
            var entregador= await DB.Find<DeliveryPerson>()
                .Match(e=>e.Identifier==rentalDTO.identifier)
                .ExecuteFirstAsync();
            if (entregador == null)//confirmaçao da existencia do entregador
            {
                throw new Exception("DeliveryPerson not found");
            }
            var moto=await DB.Find<Moto>()
                .Match(moto=>moto.Identifier==rentalDTO.moto_id)
                .ExecuteFirstAsync();
            if (moto == null)// confirmaçao da existencia da moto
            {
                throw new Exception("Moto not found");
            }
            else if (moto.IsRented)// verificaçao se a moto já está alugada
            {
                throw new Exception("Moto is currently rented out");
            }

                var plan_price = new Dictionary<int, float>
            {
                { 7, 30.00f },
                { 15, 28.00f },
                { 30, 22.00f },
                { 45, 20.00f },
                { 50, 18.00f }
            };// esse dicionario foi feito para que no momento que o plano fosse inserido como entrada o valor da diaria fosse retornado

            var rental = new Rental
            {
                Identifier = rentalDTO.identifier,
                DeliveryPersonId = entregador.Identifier,
                MotoId = moto.Identifier,
                StartDate = rentalDTO.start_date,
                EndDate = rentalDTO.end_date,
                DevolutionPrevisionDate = rentalDTO.prevision_devolution_date,
                DevolutionDate = null,
                PlanType = rentalDTO.plan,
                DailyPrice = plan_price[rentalDTO.plan]// input do plano e retorno do valor da diaria
            };
            moto.IsRented = true;// atualiza o status da moto para alugada
            await moto.SaveAsync();
            await rental.SaveAsync();

            return new RentalResponse
            {
                id = rental.ID,
                identifier = rental.Identifier,
                delivery_personId = rental.DeliveryPersonId,
                moto_id = rental.MotoId,
                start_date = rental.StartDate,
                end_date = rental.EndDate,
                prevision_devolution_date = rental.DevolutionPrevisionDate,
                dailyPrice = rental.DailyPrice,
                plan = rental.PlanType,
                totalPrice=rental.TotalPrice
            };

        }
        catch(Exception ex)
        {
            throw new Exception($"Error creating rental: {ex.Message}");
        }
    }
    public async Task<RentalResponse> GetRentalById(string id)
    {
        // O método faz a consulta no banco para procurar a rental que possui esse identificador
        try
        {
            var rental=await DB.Find<Rental>()
                .Match(l=>l.Identifier==id)
                .ExecuteFirstAsync();
            return rental == null
                ? throw new Exception("Rental not found")
                : new RentalResponse
                {
                    id = rental.ID,
                    identifier = rental.Identifier,
                    delivery_personId = rental.DeliveryPersonId,
                    moto_id = rental.MotoId,
                    start_date = rental.StartDate,
                    end_date = rental.EndDate,
                    prevision_devolution_date = rental.DevolutionPrevisionDate,
                    dailyPrice = rental.DailyPrice,
                    plan = rental.PlanType,
                    totalPrice= rental.TotalPrice
                };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving rental by id: {ex.Message}");
        }
    }
    public async Task<DevolutionResponse> UpdateDevolutionDate(string id, DevolutionRentalDTO devolucaoRentalDTO)
    {
        //esse método é feito para que quando a pessoa informar a data de devolução ser feito o calculo do total a ser pago
        try
        {
            var rental = await DB.Find<Rental>()
                .Match(l => l.Identifier == id)
                .ExecuteFirstAsync();

            if (rental == null)
            {
                throw new Exception("Rental not found");
            }

            rental.DevolutionDate = devolucaoRentalDTO.devolution_date;
            if (rental.DevolutionDate.HasValue)
            {
                DateTime prevision= rental.DevolutionPrevisionDate.Date;
                DateTime devolution = rental.DevolutionDate.Value.Date;

                if (devolution < prevision)
                {
                    // caso a devolução seja antecipada:
                    int antecipationDays= (prevision - devolution).Days;
                    rental.TotalPrice = (rental.DailyPrice * Math.Abs(antecipationDays - rental.PlanType)) + (rental.DailyPrice * (rental.PlanType == 7 ? 0.2f : 0.4f) * antecipationDays);
                    Console.WriteLine($"Dias de adiantamento {antecipationDays} R${rental.TotalPrice}");
                    // A lógica usada é simplesmente olhar o dia que está sendo devolvido ignorando as horas
                    // (não foi informado quanto tempo seria um atraso então deixei somente atrasos de dia na data ex:data da devolução 21/08/2025 e data prevista 22/08/2025 = 1 dia de atraso, desconsiderando as horas)
                    // não foi informado se havia alguma taxa no plano de 30 dias ou mais então mantive os 40%
                }
                else if (devolution > prevision)
                {
                    // mesma lógica acima, porém mudando os valores, diaria atrasada +50 reais
                    int delayDays = (devolution - prevision).Days;
                    rental.TotalPrice = (rental.DailyPrice * rental.PlanType) + (delayDays * 50.00f);
                    Console.WriteLine($"Dias de atraso {delayDays} R${rental.TotalPrice}");
                }
                else
                {
                    rental.TotalPrice = rental.DailyPrice * rental.PlanType;
                    Console.WriteLine($"No delay. Total Price: R${rental.TotalPrice}");
                    // entregue no mesmo dia da previsão
                }
            }

            await rental.SaveAsync();
            return new DevolutionResponse
            {
                TotalPrice=rental.TotalPrice,
                DevolutionDate = rental.DevolutionDate
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error updating devolution date: {ex.Message}");
        }
    }
}