using System;

namespace P01_HospitalDatabase
{
    using Data;
    using P01_HospitalDatabase.Initializer;
    class StartUp
    {
        static void Main(string[] args)
        {
            using (HospitalContext context = new HospitalContext())
            {

                DatabaseInitializer.InitialSeed(context);
            }
        }
    }
}
