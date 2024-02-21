using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        public readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? CouponDtos = new();

            ResponseDto? response = await _couponService.GetAllCouponAsync();
            if (response != null && response.IsSuccess)
            {
                CouponDtos = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }

            return View(CouponDtos);
        }

        [HttpGet]
		public async Task<IActionResult> CreateCoupon()
        {
			return PartialView("_ParitalModalCreateCoupon");
        }

        [HttpPost]
		public async Task<IActionResult> CreateCoupon(CouponDto couponDto)
        {
			return RedirectToAction("CouponIndex", "Coupon");
		}
    }
}
