namespace Project.Models.Repositories
{
    static public class RepositoryMedicine
    {
        static public Medicine getDataByID(int id)
        {
            DoctorComContext cx = new DoctorComContext();
            Medicine med = new Medicine();
            med = cx.Medicines.Find(id);
            return med;
        }
    }
}
