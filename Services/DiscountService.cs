using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext,ILogger<DiscountService> logger) :DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupan= await dbContext.Coupones.FirstOrDefaultAsync(x=>x.ProductName == request.ProductName);

            if (coupan is null)
                coupan = new Models.Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount for this Product" };

            
            logger.LogInformation("Discount is received for product : {productName}",request.ProductName);
            
            var couponModel=coupan.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupan=request.Coupon.Adapt<Coupon>();
            if (coupan is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));

            dbContext.Coupones.Add(coupan);
             await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully created for : {productName}", request.Coupon.ProductName);
            var coupanModel = coupan.Adapt<CouponModel>();
            return coupanModel;



        }


        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupan = request.Coupon.Adapt<Coupon>();
            if (coupan is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));

            dbContext.Coupones.Update(coupan);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully update for : {productName}", request.Coupon.ProductName);
            var coupanModel = coupan.Adapt<CouponModel>();
            return coupanModel;
        }


        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupan = await dbContext.Coupones.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupan is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"The product is not found for product {request.ProductName}"));


            dbContext.Coupones.Remove(coupan);
            await dbContext.SaveChangesAsync();

            return new DeleteDiscountResponse { Success = true };
        }

    }
}
