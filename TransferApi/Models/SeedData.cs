using Microsoft.EntityFrameworkCore;


namespace TransferApi.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TransferContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TransferContext>>()))
            {
                if (context == null || context.TransferTransactions == null)
                {
                    throw new ArgumentNullException("Null TransferContext");
                }

                if (context.Transfers.Any())
                {
                    return; // DB has been seeded
                }

                // new card
                var card = new Cart
                {
                    Balance = 3300.00m,
                    CardNumber = "4001 5900 0000 0001",
                    CartInfo = new CartInfo
                    {
                        CardNumber = "4001 5900 0000 0001",
                        CVV2 = 441,
                        ExpireDate = "12/22"
                    }
                };
                context.Carts.Add(card);

                // new Transfers
                var transfer = new Transfer
                {
                    ID = new System.Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    CreationDate = DateTime.ParseExact("2022-03-31 20:40:50,000", "yyyy-MM-dd HH:mm:ss,fff", null),
                    CounterpartyName = "john smith",
                    CounterpartyIBAN = "23-4542678-166",
                    
                    Amount = 3000.00m,
                    Description = "loan transfer",
                    IsSigned = false,
                    CardNumber = card.CardNumber
                };
                context.Transfers.Add(
                    transfer
                );

                // new TransferTransactions
                context.TransferTransactions.AddRange(
                    new TransferTransaction
                    {
                        ResultCode = 110,
                        ResultDescription = "failed",
                        TraceNumber = 87687666,
                        SendDate = DateTime.ParseExact("2022-03-31 20:40:52,531", "yyyy-MM-dd HH:mm:ss,fff", null),
                        Status = Status.Pending,
                        ResponseDate = DateTime.ParseExact("2009-03-31 20:40:53,531", "yyyy-MM-dd HH:mm:ss,fff", null),
                        Transfer = transfer
                    },

                    new TransferTransaction
                    {
                        ResultCode = 220,
                        ResultDescription = "done successfullt",
                        TraceNumber = 1245397869,
                        SendDate = DateTime.ParseExact("2022-03-31 20:40:52,531", "yyyy-MM-dd HH:mm:ss,fff", null),
                        Status = Status.Success,
                        ResponseDate = DateTime.ParseExact("2009-03-31 20:40:54,531", "yyyy-MM-dd HH:mm:ss,fff", null),
                        Transfer = transfer
                    }
                );
                context.SaveChanges();
            }
        }
    }
}