using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    abstract class Weapon
    {
        protected int f_AmmoCount;

        public int AmmoCount
        {
            get { return f_AmmoCount; }
            set
            {
                if(value < 0)
                    throw new InvalidOperationException(
                        "Количество потронов не может быть меньше нуля");
                f_AmmoCount = value;
            }
        }

        public abstract void Shot();
    }

    class Postolet : Weapon
    {
        /// <inheritdoc />
        public override void Shot()
        {
            if (f_AmmoCount > 0)
            {
                Console.WriteLine("|");
                f_AmmoCount--;
            }
            else
            {
                Console.WriteLine("Боеприпасы закончились");
            }
        }
    }

    class Avtomat : Weapon
    {
        /// <inheritdoc />
        public override void Shot()
        {
            for (int i = 0; i < 5; i++)
            {
                if (f_AmmoCount > 0)
                {
                    Console.Write("|");
                    f_AmmoCount--;
                }
                else
                {
                    Console.Write("Боеприпасы закончились");
                    break;
                }
            }
            Console.WriteLine();
        }
    }
}
