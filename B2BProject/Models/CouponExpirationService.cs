using B2BProject.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CouponExpirationService
{
    private Timer _timer;

    public void Start()
    {
        // 1 saat aralıklarla çalışacak şekilde ayarlanır (3600000 milisaniye)
        _timer = new Timer(CheckExpiredCoupons, null, 0, 3600000);
    }

    private void CheckExpiredCoupons(object state)
    {
        using (var entities = new B2BDbEntities())
        {
            var expiredCoupons = entities.Coupons
                .Where(c => c.End_date < DateTime.Now && c.IsActive)
                .ToList();

            foreach (var coupon in expiredCoupons)
            {
                coupon.IsActive = false;
            }

            entities.SaveChanges();
        }
    }

    public void Stop()
    {
        _timer?.Dispose();
    }
}
