using EventBookingSystem.Application.Dtos;
using EventBookingSystem.Application.Services.Interfaces;
using EventBookingSystem.Domain.Enums;
using EventBookingSystem.Domain.Models;
using EventBookingSystem.Infrastructure.Repositories.Interfaces;

namespace EventBookingSystem.Application.Services.Implementations
{
    public class WalletService(IWalletRepository walletRepository, IPaymentGateway paymentGateway, IWalletTransactionRepository walletTransactionRepository) : IWalletService
    {
        private readonly IWalletRepository _walletRepository = walletRepository;
        private readonly IWalletTransactionRepository _walletTransactionRepository = walletTransactionRepository;
        private readonly IPaymentGateway _paymentGateway = paymentGateway;

        public async Task<ApiResponse<string>> InitiateTopUpAsync(InitiateTopUpRequest request,string UserId)
        {
            try
            {
                if (request.Amount <= 0)
                    return ApiResponse<string>.BadRequest("Invalid amount.");

                var wallet = await _walletRepository
                    .GetAsync(w => w.UserId == UserId);

                if (wallet == null)
                    return ApiResponse<string>.NotFound("Wallet not found.");

                
                var existing = await _walletTransactionRepository
                    .GetAsync(t => t.IdempotencyKey == request.IdempotencyKey);

                if (existing != null)
                    return ApiResponse<string>.Ok(existing.Reference, "Transaction already initiated.");

                var reference = $"PAY-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6]}";

                var transaction = new WalletTransaction
                {
                    WalletId = wallet.Id,
                    Amount = request.Amount,
                    Type = TransactionType.Credit,
                    Status = TransactionStatus.Pending,
                    Reference = reference,
                    IdempotencyKey = request.IdempotencyKey
                };

                await _walletTransactionRepository.AddAsync(transaction);

                await _paymentGateway.InitializePayment(request.Amount, reference);

                await _walletTransactionRepository.SaveChangesAsync();

                return ApiResponse<string>.Ok(reference, "Top up initiated.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> ConfirmTopUpAsync(string reference)
        {
            try
            {
                var transaction = await _walletTransactionRepository.GetTransaction
                    (t => t.Reference == reference);

                if (transaction == null)
                    return ApiResponse<bool>.NotFound("Transaction not found.");

                if (transaction.Status == TransactionStatus.Completed)
                    return ApiResponse<bool>.Ok(true, "Transaction already completed."); // Idempotent safe

                if (transaction.Status == TransactionStatus.Failed)
                    return ApiResponse<bool>.BadRequest("Transaction already failed.");

                var paymentResult = await _paymentGateway.VerifyPayment(reference);

                if (!paymentResult.IsSuccessful)
                {
                    transaction.Status = TransactionStatus.Failed;
                    await _walletTransactionRepository.SaveChangesAsync();
                    return ApiResponse<bool>.Ok(false, "Payment verification failed.");
                }

                // Credit wallet
                transaction.Wallet.Balance += transaction.Amount;
                transaction.Status = TransactionStatus.Completed;

                await _walletTransactionRepository.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, "Top up confirmed and wallet credited.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.InternalServerError(ex.Message);
            }
        }
    }
}
