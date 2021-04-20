using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavioralPatterns
{
    public class Proxy
    {
        public interface IWeapon
        {
            public int BulletCount { get; }
            public void Reload();
            public void Shoot();
        }

        public class Weapon : IWeapon
        {
            public int BulletCount { get; private set; }
            public void Reload()
            {
                Console.WriteLine("Reloading..");
                //Task.Delay(4 * 1000).Wait();
                BulletCount = 10;
            }

            public void Shoot()
            {
                if (BulletCount <= 0) { throw new InvalidOperationException("No more Bullets"); }

                Console.WriteLine("Shoot");
                BulletCount--;
                //Task.Delay(500).Wait();
            }
        }

        public class WeaponProxy : IWeapon
        {
            private readonly IWeapon _weapon;

            public WeaponProxy(IWeapon weapon)
            {
                _weapon = weapon;
            }

            public int BulletCount => _weapon.BulletCount;

            public void Reload()
            {
                _weapon.Reload();
            }

            public void Shoot()
            {
                try
                {
                    _weapon.Shoot();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        [Test]
        public void Try()
        {
            IWeapon weapon = new WeaponProxy(new Weapon());

            weapon.Reload();
            Enumerable.Range(0, 11).ToList().ForEach(s => weapon.Shoot());            
        }

    }
}
